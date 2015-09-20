// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesDataQueryEngineBase.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base class for DataQueryEngine used for Time series output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    /// <summary>
    /// The base class for DataQueryEngine used for Time series output
    /// </summary>
    internal abstract class SeriesDataQueryEngineBase : DataQueryEngineBase<DataRetrievalInfoSeries, MappedValues>
    {
        #region Methods

        /// <summary>
        /// The _logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SeriesDataQueryEngineBase));

        /// <summary>
        /// Writes the attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="info">The info.</param>
        protected static void WriteAttributes(IEnumerable<ComponentValue> attributes, DataRetrievalInfoSeries info)
        {
            if (!info.WriteAttributes)
            {
                return;
            }

            var dataWriterEngine = info.SeriesWriter;

            // write  attributes
            foreach (var keyValuePair in attributes)
            {
                var componentEntity = keyValuePair.Key;
                var value = keyValuePair.Value;

                // SODIHD-1272 write optional attribute only if it is not empty.
                if (componentEntity.ComponentType != SdmxComponentType.Attribute || componentEntity.AttStatus != AssignmentStatus.Conditional || !string.IsNullOrEmpty(value))
                {
                    dataWriterEngine.WriteAttributeValue(componentEntity.Id, value);
                }
            }
        }

        /// <summary>
        /// Write the SDMX compliant values for dataset attached attributes to the <see cref="DataRetrievalInfoSeries.SeriesWriter"/> if <see cref="MappedValues.StartedDataSet"/> is true
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        protected static void TryWriteDataSet(MappedValues componentValues, DataRetrievalInfoSeries info)
        {
            if (!componentValues.StartedDataSet)
            {
                WriteDataSetResults(componentValues, info);
                componentValues.StartedDataSet = true;
            }
        }

        /// <summary>
        /// Write observation element.
        /// </summary>
        /// <param name="info">The current Data Retrieval state</param>
        /// <param name="dimensionAtObsValue">The dimension at observation value.</param>
        /// <param name="value">The observation value</param>
        /// <param name="attributeObservationValues">The attribute observation values.</param>
        /// <returns>
        /// The number of observations stored. Is is always 1
        /// </returns>
        protected static int WriteObservation(DataRetrievalInfoSeries info, string dimensionAtObsValue, string value, IEnumerable<ComponentValue> attributeObservationValues)
        {
            if (!info.WriteObservations)
            {
                return 1;
            }

            // write dimension at observation and obs value
            if (info.IsAllDimensions)
            {
                info.SeriesWriter.WriteObservation(info.EffectiveDimensionAtObservation, dimensionAtObsValue, value);
            }
            else
            {
                info.SeriesWriter.WriteObservation(dimensionAtObsValue, value);
            }

            // write observation attributes
            WriteAttributes(attributeObservationValues, info);

            return 1;
        }

        /// <summary>
        /// Write a series element.
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        protected static void WriteSeries(DataRetrievalInfoSeries info, MappedValues row)
        {
            // start series
            var dataWriterEngine = info.SeriesWriter;
            dataWriterEngine.StartSeries();

            var dimensionAtObservation = info.DimensionAtObservation;
            var effectiveDimensionAtObservation = info.EffectiveDimensionAtObservation;
            var isAllDimensions = info.IsAllDimensions;

            // write dimensions
            for (int index = 0; index < row.DimensionValues.Count; index++)
            {
                var dimensionValue = row.DimensionValues[index];
                var id = dimensionValue.Key.Id;
                if (!id.Equals(dimensionAtObservation) && (!isAllDimensions || !id.Equals(effectiveDimensionAtObservation)))
                {
                    dataWriterEngine.WriteSeriesKeyValue(id, dimensionValue.Value);
                }
            }

            // write time period if it is not the dimension at observation
            if (!info.IsTimePeriodAtObservation && info.TimeTranscoder != null)
            {
                var id = info.TimeTranscoder.Component.Id;
                if (!isAllDimensions || !id.Equals(effectiveDimensionAtObservation))
                {
                    dataWriterEngine.WriteSeriesKeyValue(id, row.TimeValue);
                }
            }

            // write series attributes
            WriteAttributes(row.AttributeSeriesValues, info);
        }

        /// <summary>
        /// Create and return a <see cref="IMappedValues"/> implementation
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        /// The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <returns>
        /// a <see cref="IMappedValues"/> implementation 
        /// </returns>
        protected override MappedValues CreateMappedValues(DataRetrievalInfoSeries info, IDataReader reader)
        {
            if (info == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotDataRetrievalInfoSeries, "info");
            }

            return new MappedValues(info) { StartedDataSet = info.DataSetAttributes.Count > 0 };
        }

        /// <summary>
        /// Execute any additional queries if overridden in a subclass. This override executes queries for DataSet and Groups
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="connection">
        /// The <see cref="DbConnection"/> to the DDB 
        /// </param>
        protected override void RunAdditionalQueries(DataRetrievalInfoSeries info, DbConnection connection)
        {
            if (info == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotDataRetrievalInfoSeries, "info");
            }

            // handle dataset attributes
            if (info.WriteAttributes && info.DataSetAttributes.Count > 0)
            {
                ExecuteDataSetAttributeSqlQuery(info, connection);
            }

            if (info.WriteGroups)
            {
                // check if there are groups and mapped group attributes
                foreach (var groupInformation in info.Groups)
                {
                    this.ExecuteGroupSqlQuery(groupInformation.Value, info, connection);
                }
            }
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="groupInformation">
        /// The current group related information 
        /// </param>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        protected virtual void StoreTimeSeriesGroupResults(
            DataRetrievalInfoSeries info, GroupInformation groupInformation, MappedValues row)
        {
            info.SeriesWriter.StartGroup(groupInformation.ThisGroup.Id);

            // write group dimensions
            foreach (var dimensionValue in row.DimensionValues)
            {
                info.SeriesWriter.WriteGroupKeyValue(dimensionValue.Key.Id, dimensionValue.Value);
            }

            // write group attributes
            WriteAttributes(row.AttributeGroupValues, info);
        }

        /// <summary>
        /// This method executes an SQL query for retrieving the dataset attributes on the dissemination database and writes it to <see cref="DataRetrievalInfoSeries.SeriesWriter"/>
        /// </summary>
        /// <param name="info">
        /// The current DataRetrieval state 
        /// </param>
        /// <param name="connection">
        /// The connection to the dissemination database 
        /// </param>
        private static void ExecuteDataSetAttributeSqlQuery(DataRetrievalInfoSeries info, DbConnection connection)
        {
            if (string.IsNullOrEmpty(info.DataSetSqlString))
            {
                // handle case where all dataset attributes are constants.
                if (info.DataSetAttributes.All(mapping => !string.IsNullOrWhiteSpace(mapping.Mapping.Constant)))
                {
                    var componentValues = new MappedValues(info, info.DataSetAttributes);
                    if (HandleComponentMapping(null, componentValues, info.DataSetAttributes, info))
                    {
                        WriteDataSetResults(componentValues, info);
                    }
                }
                else
                {
                    Debug.Assert(info.DataSetAttributes.Count == 0, "No dataset SQL string generated and there are non constant dataset attributes");   
                }

                return;
            }

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = info.DataSetSqlString;

                // for pc-axis
                command.CommandTimeout = 0;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (IDataReader reader = command.ExecuteReader())
                {
                    stopwatch.Stop();
                    _logger.Info(
                         string.Format(
                             CultureInfo.InvariantCulture, "Execute DataSet Attribute SQL Reader: {0}", stopwatch.Elapsed));
                    bool found = false;
                    while (!found && reader.Read())
                    {
                        var componentValues = new MappedValues(info, info.DataSetAttributes);
                        if (HandleComponentMapping(reader, componentValues, info.DataSetAttributes, info))
                        {
                            WriteDataSetResults(componentValues, info);
                            found = true;
                        }
                    }

                    // To avoid populating the IDataReader.RecordsAffected on IDataReader close which can take some time on some cases (typec)
                    // this requires an updated MySQL driver, 6.3.7 or later. See http://bugs.mysql.com/bug.php?id=60541
                    command.Cancel();
                }
            }
        }

        /// <summary>
        /// Check if the specified <paramref name="targetGroup"/> contains the <paramref name="componentValues"/>
        /// </summary>
        /// <param name="targetGroup">
        /// The target group 
        /// </param>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <param name="componentValues">
        /// The component values 
        /// </param>
        /// <returns>
        /// A value indicating whether the key is already processed or not. 
        /// </returns>
        private static bool ProcessedKeySet(
            GroupInformation targetGroup, DataRetrievalInfoSeries info, MappedValues componentValues)
        {
            var current = new ReadOnlyKey(componentValues, info.GroupNameTable);

            if (!targetGroup.KeySet.ContainsKey(current))
            {
                targetGroup.KeySet.Add(current, null);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Write the SDMX compliant values for dataset attached attributes to the <see cref="DataRetrievalInfoSeries.SeriesWriter"/>
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        private static void WriteDataSetResults(MappedValues componentValues, DataRetrievalInfoSeries info)
        {
            WriteAttributes(componentValues.AttributeDataSetValues, info);
        }

        /// <summary>
        /// This method executes an SQL query for a group on the dissemination database and writes it to <see cref="DataRetrievalInfoSeries.SeriesWriter"/> The SQL query is located inside <paramref name="groupInformation"/> at <see cref="GroupInformation.SQL"/> . The group information is located in <paramref name="groupInformation"/>
        /// </summary>
        /// <param name="groupInformation">
        /// The current Time Series Group information 
        /// </param>
        /// <param name="info">
        /// The current DataRetrieval state 
        /// </param>
        /// <param name="connection">
        /// The connection to the dissemination database 
        /// </param>
        private void ExecuteGroupSqlQuery(
            GroupInformation groupInformation, DataRetrievalInfoSeries info, DbConnection connection)
        {
            if (string.IsNullOrEmpty(groupInformation.SQL))
            {
                return;
            }

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = groupInformation.SQL;

                command.CommandTimeout = 0;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (IDataReader reader = command.ExecuteReader())
                {
                    stopwatch.Stop();
                    _logger.Info(
                         string.Format(CultureInfo.InvariantCulture, "Execute Group SQL Reader: {0}", stopwatch.Elapsed));

                    while (reader.Read())
                    {
                        var componentValues = new MappedValues(info, groupInformation.ComponentMappings);
                        if (HandleComponentMapping(reader, componentValues, groupInformation.ComponentMappings, info)
                            && !ProcessedKeySet(groupInformation, info, componentValues))
                        {
                            this.StoreTimeSeriesGroupResults(info, groupInformation, componentValues);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
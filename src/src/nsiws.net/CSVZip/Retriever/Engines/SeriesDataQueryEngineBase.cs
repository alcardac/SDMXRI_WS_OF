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

namespace CSVZip.Retriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;

    using CSVZip.Retriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    /// <summary>
    /// The base class for DataQueryEngine used for Time series output
    /// </summary>
    internal abstract class SeriesDataQueryEngineBase : DataQueryEngineBase
    {
        #region Methods

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SeriesDataQueryEngineBase));

        /// <summary>
        /// Writes the attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="info">The info.</param>
        protected static void WriteAttributes(IEnumerable<ComponentValue> attributes, DataRetrievalInfoSeries info)
        {
            // write  attributes
            foreach (var keyValuePair in attributes)
            {
                var componentEntity = keyValuePair.Key;
                var value = keyValuePair.Value;

                // SODIHD-1272 write optional attribute only if it is not empty.
                if (componentEntity.ComponentType != SdmxComponentType.Attribute || componentEntity.AttStatus != AssignmentStatus.Conditional || !string.IsNullOrEmpty(value))
                {
                    info.SeriesWriter.WriteAttributeValue(componentEntity.Id, value);
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
        /// Write observation element. The time, observation and attribute values are in <paramref name="row"/>
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="value">
        /// The observation value 
        /// </param>
        /// <returns>
        /// The number of observations stored. Is is always 1 
        /// </returns>
        protected static int WriteObservation(DataRetrievalInfoSeries info, MappedValues row, string value)
        {
            // write time and obs value
            info.SeriesWriter.WriteObservation(row.DimensionAtObservationValue, value);

            // write observation attributes
            WriteAttributes(row.AttributeObservationValues, info);

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
            info.SeriesWriter.StartSeries();

            // write dimensions
            foreach (var dimensionValue in row.DimensionValues)
            {
                if (!dimensionValue.Key.Id.Equals(info.DimensionAtObservation))
                {
                    info.SeriesWriter.WriteSeriesKeyValue(dimensionValue.Key.Id, dimensionValue.Value);
                }
            }

            // write time period if it is not the dimension at observation
            if (!info.IsTimePeriodAtObservation && info.TimeTranscoder != null)
            {
                info.SeriesWriter.WriteSeriesKeyValue(info.TimeTranscoder.Component.Id, row.TimeValue);
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
        protected override IMappedValues CreateMappedValues(DataRetrievalInfo info, IDataReader reader)
        {
            var seriesInfo = info as DataRetrievalInfoSeries;
            if (seriesInfo == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotDataRetrievalInfoSeries, "info");
            }

            return new MappedValues(info) { StartedDataSet = seriesInfo.DataSetAttributes.Count > 0 };
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
        protected override void RunAdditionalQueries(DataRetrievalInfo info, DbConnection connection)
        {
            var seriesInfo = info as DataRetrievalInfoSeries;
            if (seriesInfo == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotDataRetrievalInfoSeries, "info");
            }

            // handle dataset attributes
            if (seriesInfo.DataSetAttributes.Count > 0)
            {
                ExecuteDataSetAttributeSqlQuery(seriesInfo, connection);
            }

            // check if there are groups and mapped group attributes
            foreach (var groupInformation in seriesInfo.Groups)
            {
                this.ExecuteGroupSqlQuery(groupInformation.Value, seriesInfo, connection);
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
                    Logger.Info(
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
                    Logger.Info(
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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesDataQueryEngineXsMeasureBuffered.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The series data query engine xs measure.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using CSVZip.Retriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The series data query engine xs measure.
    /// </summary>
    internal class SeriesDataQueryEngineXsMeasureBuffered : SeriesDataQueryEngineBase, IDataQueryEngine
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesDataQueryEngineXsMeasureBuffered _instance =
            new SeriesDataQueryEngineXsMeasureBuffered();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesDataQueryEngineXsMeasureBuffered" /> class from being created.
        /// </summary>
        private SeriesDataQueryEngineXsMeasureBuffered()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesDataQueryEngineXsMeasureBuffered Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method executes an SQL query on the dissemination database and writes it to <see cref="DataRetrievalInfoSeries.SeriesWriter"/> . The SQL query is located inside <paramref name="info"/> at <see cref="DataRetrievalInfo.SqlString"/>
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="info"/>
        ///   is null
        /// </exception>
        /// <exception cref="DataRetrieverException">
        /// <see cref="ErrorTypes"/>
        /// </exception>
        /// <param name="info">
        /// The current DataRetrieval state 
        /// </param>
        public void ExecuteSqlQuery(DataRetrievalInfo info)
        {
            info.BuildXSMeasures();
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read data from DDB up the specified number of observations, perfom mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
        /// </summary>
        /// <param name="info">
        ///   The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        ///   The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <param name="componentValues">
        ///   The component Values. 
        /// </param>
        /// <param name="limit">
        ///   The maximum number of observations, should be greater than 0 
        /// </param>
        /// <param name="mappings">
        ///   The collection of component mappings 
        /// </param>
        protected override void ReadData(DataRetrievalInfo info, IDataReader reader, IMappedValues componentValues, int limit, IList<IComponentMapping> mappings)
        {
            base.ReadData(info, reader, componentValues, limit, mappings);
            var mappedValues = componentValues as MappedValues;
            if (mappedValues != null)
            {
                WriteXsMeasureCache(info as DataRetrievalInfoSeries, mappedValues.XSMeasureCaches);
            }
        }

        /// <summary>
        /// Read data from DDB, perfom mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
        /// </summary>
        /// <param name="info">
        ///   The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        ///   The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <param name="componentValues">
        ///   The component Values. 
        /// </param>
        /// <param name="mappings">
        ///   The collection of component mappings 
        /// </param>
        protected override void ReadData(DataRetrievalInfo info, IDataReader reader, IMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            base.ReadData(info, reader, componentValues, mappings);
            var mappedValues = componentValues as MappedValues;
            if (mappedValues != null)
            {
                WriteXsMeasureCache(info as DataRetrievalInfoSeries, mappedValues.XSMeasureCaches);
            }
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored 
        /// </returns>
        protected override int StoreResults(IMappedValues componentValues, DataRetrievalInfo info)
        {
            var row = componentValues as MappedValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedValues type");
            }

            int maxMeasures = info.CrossSectionalMeasureMappings.Count;
            var count = WriteXsMeasures(info as DataRetrievalInfoSeries, maxMeasures, row);

            return count;
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="limit">
        /// The limit. 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored 
        /// </returns>
        protected override int StoreResults(IMappedValues componentValues, int limit, DataRetrievalInfo info)
        {
            var row = componentValues as MappedValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedValues type");
            }

            int maxMeasures = Math.Min(info.CrossSectionalMeasureMappings.Count, limit);
            int count = WriteXsMeasures(info as DataRetrievalInfoSeries, maxMeasures, row);
            return count;
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
        protected override void StoreTimeSeriesGroupResults(
            DataRetrievalInfoSeries info, GroupInformation groupInformation, MappedValues row)
        {
            if (groupInformation.MeasureComponent == null)
            {
                base.StoreTimeSeriesGroupResults(info, groupInformation, row);
                return;
            }

            foreach (var measureDimensionQueryValue in info.CrossSectionalMeasureMappings)
            {
                info.SeriesWriter.StartGroup(groupInformation.ThisGroup.Id);

                // write group dimensions
                foreach (var dimensionValue in row.DimensionValues)
                {
                    info.SeriesWriter.WriteGroupKeyValue(dimensionValue.Key.Id, dimensionValue.Value);
                }

                var xsComponent = measureDimensionQueryValue.Components[0];

                info.SeriesWriter.WriteGroupKeyValue(
                    groupInformation.MeasureComponent.Id, xsComponent.CrossSectionalMeasureCode);

                // write group attributes
                WriteAttributes(row.AttributeGroupValues, info);
            }
        }

        /// <summary>
        /// Write the cached <paramref name="xsMeasures"/> .
        /// </summary>
        /// <param name="info">
        /// The current dataretrieval info 
        /// </param>
        /// <param name="xsMeasures">
        /// The xs measures. 
        /// </param>
        private static void WriteXsMeasureCache(
            DataRetrievalInfoSeries info, IEnumerable<KeyValuePair<ComponentEntity, XsMeasureCache>> xsMeasures)
        {
            foreach (var xsMeasure in xsMeasures)
            {
                // start series
                info.SeriesWriter.StartSeries();
                foreach (var dimensionValue in xsMeasure.Value.CachedSeriesKey)
                {
                    info.SeriesWriter.WriteSeriesKeyValue(dimensionValue.Key.Id, dimensionValue.Value);
                }

                info.SeriesWriter.WriteSeriesKeyValue(info.MeasureComponent.Id, xsMeasure.Value.XSMeasureCode);

                WriteAttributes(xsMeasure.Value.CachedSeriesAttributes, info);

                for (int index = 0; index < xsMeasure.Value.XSMeasureCachedObservations.Count; index++)
                {
                    var cachedObservation = xsMeasure.Value.XSMeasureCachedObservations[index];
                    var attributes = xsMeasure.Value.Attributes[index];

                    info.SeriesWriter.WriteObservation(cachedObservation.Key, cachedObservation.Value);
                    WriteAttributes(attributes, info);
                }
            }
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="maxMeasures">
        /// The max number of measures to write 
        /// </param>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <returns>
        /// The number of observations stored 
        /// </returns>
        private static int WriteXsMeasures(DataRetrievalInfoSeries info, int maxMeasures, MappedValues row)
        {
            int count = 0;

            if (row.IsNewKey())
            {
                TryWriteDataSet(row, info);
                WriteXsMeasureCache(info, row.XSMeasureCaches);

                row.InitXsBuffer();
            }

            for (int i = 0; i < maxMeasures; i++)
            {
                var crossSectionalMeasureMapping = info.CrossSectionalMeasureMappings[i];
                var xsComponent = crossSectionalMeasureMapping.Components[0];
                row.AddToBuffer(xsComponent);
                count++;
            }

            return count;
        }

        #endregion
    }
}
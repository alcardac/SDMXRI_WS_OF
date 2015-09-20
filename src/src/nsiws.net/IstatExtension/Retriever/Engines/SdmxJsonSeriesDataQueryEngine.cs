// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data query engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Engines
{
    using System;
    using System.Data;
    using System.Collections.Generic;

    using IstatExtension.Retriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using IstatExtension.Controllers.Engine;
    using IstatExtension.SdmxJson.DataWriter.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// The data query engine.
    /// </summary>
    public class SdmxJsonSeriesDataQueryEngine : SeriesDataQueryEngineBase, IDataQueryEngine
    {
        #region Constants and Fields

        System.Collections.Hashtable dimensionValues = new System.Collections.Hashtable();
        
        public Int64 DimCount { get { return dimensionValues.Count; } }
        
        public IDataStructureObject _dsd;
        public IDataflowObject _dfo;

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SdmxJsonSeriesDataQueryEngine _instance = new SdmxJsonSeriesDataQueryEngine();

        private static bool StartedObservations = false;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesDataQueryEngine" /> class from being created.
        /// </summary>
        private SdmxJsonSeriesDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SdmxJsonSeriesDataQueryEngine Instance
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
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods


        protected static void WriteSdmxJsonSeries(DataRetrievalInfoSeries info, MappedValues row)
        {
            // start series
            //((SdmxJsonBaseDataWriter)info.SeriesWriter).WriteSeriesKey(row);
            ((SdmxJsonBaseDataWriter)info.SeriesWriter).WriteSeriesKey(row);
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
            
            var seriesInfo = info as DataRetrievalInfoSeries;
            
            ((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter).SetJsonStructure(componentValues);

            if (row.IsNewKey())
            {
                if (((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter).StartedObservations) 
                {
                    ((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter).CloseObject();
                    ((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter).CloseObject();
                    ((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter)._startedObservations = false;
                }
                TryWriteDataSet(row, seriesInfo);
                WriteSdmxJsonSeries(seriesInfo, row);
                
            }

            return WriteObservation(seriesInfo, row, row.PrimaryMeasureValue.Value);
        }

        protected new static int WriteObservation(DataRetrievalInfoSeries info, MappedValues row, string value)
        {

            if (info.IsTimePeriodAtObservation) 
            {
                //info.SeriesWriter.WriteObservation(((SdmxJsonBaseDataWriter)info.SeriesWriter).GetDimValPosition(row.TimeDimensionValue.Key.Id, row.DimensionAtObservationValue), value);
                info.SeriesWriter.WriteObservation(((SdmxJsonBaseDataWriter)info.SeriesWriter).GetObsValPosition(row.DimensionAtObservationValue), value);
            }
            else
            {
                // write time and obs value
                //info.SeriesWriter.WriteObservation(((SdmxJsonBaseDataWriter)info.SeriesWriter).GetDimValPosition(info.DimensionAtObservation, row.DimensionAtObservationValue), value);
                info.SeriesWriter.WriteObservation(((SdmxJsonBaseDataWriter)info.SeriesWriter).GetObsValPosition(row.DimensionAtObservationValue), value);
            }
            // write observation attributes
            ((SdmxJsonBaseDataWriter)info.SeriesWriter).WriteAttributes(row.AttributeObservationValues);
            
            ((SdmxJsonBaseDataWriter)info.SeriesWriter).CloseArray();
            
            return 1;
        }

        protected new static void TryWriteDataSet(MappedValues componentValues, DataRetrievalInfoSeries info)
        {
            #region USELESS
            //if (!componentValues.StartedDataSet)
            //{
            //    //((SdmxJsonDelayedDataWriterEngine)info.SeriesWriter).StartElement("attributes",false);
            //    ((SdmxJsonBaseDataWriter)info.SeriesWriter).StartElement("attributes", false);
            //    WriteAttributes(componentValues.AttributeDataSetValues, info);
            //    //((SdmxJsonDelayedDataWriterEngine)info.SeriesWriter).CloseArray();
            //    ((SdmxJsonBaseDataWriter)info.SeriesWriter).CloseArray();
            //    info.SeriesWriter.StartSeries();
            //    componentValues.StartedDataSet = true;
            //}
            #endregion
            ((SdmxJsonBaseDataWriter)info.SeriesWriter).TryWriteDataSet(componentValues);
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
            return this.StoreResults(componentValues, info);
        }

        protected override void ReadData(DataRetrievalInfo info, IDataReader reader, IMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            int count = 0;

            while (reader.Read())
            {
                if (HandleMappings(reader, info, componentValues, mappings))
                {
                    count += this.StoreResults(componentValues, info);
                }
            }
            var seriesInfo = info as DataRetrievalInfoSeries;

            info.RecordsRead = count;

            if (count > 0)
            {                
                ((SdmxJsonBaseDataWriter)seriesInfo.SeriesWriter).WriteSDMXJsonStructure(this._dfo);                
            }

            
        }

        private static bool HandleMappings(IDataReader reader, DataRetrievalInfo info, IMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            return HandleComponentMapping(reader, componentValues, mappings, info)
                   && HandleTimeDimensionMapping(reader, info, componentValues); // MAT-262
        }

        private static bool HandleTimeDimensionMapping(
            IDataReader reader, DataRetrievalInfo info, IMappedValues mappedValues)
        {
            if (info.TimeMapping != null)
            {
                string val = info.TimeTranscoder.MapComponent(reader, mappedValues.FrequencyValue);
                if (val != null)
                {
                    mappedValues.TimeValue = val;
                    if (info.IsTimePeriodAtObservation)
                    {
                        mappedValues.DimensionAtObservationValue = val;
                    }
                }
                else
                {
                    return false; // null value found at time dimension
                }
            }

            return true;
        }

        protected new static bool HandleComponentMapping(IDataReader reader, IMappedValues componentValues, IList<IComponentMapping> componentMappings, DataRetrievalInfo info)
        {
            var dimensionAtObservationMapping = info.DimensionAtObservationMapping;
            for (int index = 0; index < componentMappings.Count; index++)
            {
                var componentMapping = componentMappings[index];

                var val = componentMapping.MapComponent(reader);
                if (val != null)
                {
                    componentValues.Add(index, val);
                    if (componentMapping.Component.FrequencyDimension)
                    {
                        componentValues.FrequencyValue = val;
                    }

                    if (componentMapping.Equals(dimensionAtObservationMapping))
                    {
                        componentValues.DimensionAtObservationValue = val;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
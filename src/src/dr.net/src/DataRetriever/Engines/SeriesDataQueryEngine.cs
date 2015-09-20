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

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;

    /// <summary>
    /// The data query engine.
    /// </summary>
    internal class SeriesDataQueryEngine : SeriesDataQueryEngineBase, IDataQueryEngine<DataRetrievalInfoSeries>
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesDataQueryEngine _instance = new SeriesDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesDataQueryEngine" /> class from being created.
        /// </summary>
        private SeriesDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesDataQueryEngine Instance
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
        public void ExecuteSqlQuery(DataRetrievalInfoSeries info)
        {
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data with max observation per series.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="componentValues">The component values.</param>
        /// <param name="mappings">The mappings.</param>
        protected override void ReadDataMaxObsPerSeries(DataRetrievalInfoSeries info, IDataReader reader, MappedValues componentValues, IList<IComponentMapping> mappings)
        {
            int count = 0;
            Predicate<int> isBelowLimit;
            var limit = info.Limit;
            if (limit > 0)
            {
                isBelowLimit = i => i < limit;
            }
            else
            {
                isBelowLimit = i => true;
            }

            using (var maxObsStatus = new MaxObsStatus(info, WriteObservation))
            {
                while (!info.IsTruncated && reader.Read())
                {
                    if (HandleMappings(reader, info, componentValues, mappings))
                    {
                        if (!isBelowLimit(count))
                        {
                            info.IsTruncated = true;
                        }
                        else
                        {
                            count += this.StoreResultsMaxObs(componentValues, info, maxObsStatus);
                        }
                    }
                }
            }

            info.RecordsRead = count;
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store.
        /// </summary>
        /// <param name="componentValues">The map between components and their values</param>
        /// <param name="info">The current Data Retrieval state</param>
        /// <param name="maxObsStatus">The max observation status.</param>
        /// <returns>
        /// The number of observations stored
        /// </returns>
        /// <exception cref="System.NotImplementedException">This method needs to be implemented.</exception>
        protected virtual int StoreResultsMaxObs(MappedValues componentValues, DataRetrievalInfoSeries info, MaxObsStatus maxObsStatus)
        {
            var row = componentValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedValues type");
            }

            // TODO change to Generic.
            var seriesInfo = info;

            if (row.IsNewKey())
            {
                maxObsStatus.ResetCount();
                TryWriteDataSet(row, seriesInfo);
                WriteSeries(seriesInfo, row);
            }

            string value = row.PrimaryMeasureValue.Value;

            // must copy.
            var attributeObservationValues = row.AttributeObservationValues.ToArray();
            var dimensionAtObservationValue = row.DimensionAtObservationValue;

            // TODO change to Tuple.
            return maxObsStatus.Add(new Tuple<string, string, IEnumerable<ComponentValue>>(dimensionAtObservationValue, value, attributeObservationValues));
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
        protected override int StoreResults(MappedValues componentValues, DataRetrievalInfoSeries info)
        {
            var row = componentValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedValues type");
            }

            if (row.IsNewKey())
            {
                TryWriteDataSet(row, info);
                WriteSeries(info, row);
            }

            string value = row.PrimaryMeasureValue.Value;
            return WriteObservation(info, row.DimensionAtObservationValue, value, row.AttributeObservationValues);
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
        protected override int StoreResults(MappedValues componentValues, int limit, DataRetrievalInfoSeries info)
        {
            return this.StoreResults(componentValues, info);
        }

        #endregion
    }
}
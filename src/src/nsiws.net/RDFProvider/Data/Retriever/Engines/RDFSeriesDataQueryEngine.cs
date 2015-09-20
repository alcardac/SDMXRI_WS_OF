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

namespace RDFProvider.Retriever.Engines
{
    using System;

    using RDFProvider.Retriever.Model;

    /// <summary>
    /// The data query engine.
    /// </summary>
    internal class RDFSeriesDataQueryEngine : RDFSeriesDataQueryEngineBase, IRDFDataQueryEngine
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly RDFSeriesDataQueryEngine _instance = new RDFSeriesDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesDataQueryEngine" /> class from being created.
        /// </summary>
        private RDFSeriesDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static RDFSeriesDataQueryEngine Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        public void ExecuteSqlQuery(DataRetrievalInfo info)
        {
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods

        protected override int StoreResults(IMappedValues componentValues, DataRetrievalInfo info)
        {
            var row = componentValues as MappedValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedValues type");
            }

            var seriesInfo = info as DataRetrievalInfoSeries;

            RDFWriteSeries(seriesInfo, row);
            return RDFWriteObservation(seriesInfo, row, row.PrimaryMeasureValue.Value);
        }

        protected override int StoreResults(IMappedValues componentValues, int limit, DataRetrievalInfo info)
        {
            return this.StoreResults(componentValues, info);
        }

        #endregion
    }
}
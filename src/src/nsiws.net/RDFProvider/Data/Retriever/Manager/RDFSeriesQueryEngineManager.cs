﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesQueryEngineManager.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="IQueryEngineManager" /> impementation for time series
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Retriever.Manager
{
    using System.Collections.Generic;

    using RDFProvider.Retriever.Engines;
    using RDFProvider.Retriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The <see cref="IQueryEngineManager"/> impementation for time series
    /// </summary>
    public class RDFSeriesQueryEngineManager : IRDFQueryEngineManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly RDFSeriesQueryEngineManager _instance = new RDFSeriesQueryEngineManager();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesQueryEngineManager" /> class from being created.
        /// </summary>
        private RDFSeriesQueryEngineManager()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static RDFSeriesQueryEngineManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get a <see cref="IDataQueryEngine"/> implementation based on the specified <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// a <see cref="IDataQueryEngine"/> implementation based on the specified <paramref name="info"/> 
        /// </returns>
        public IRDFDataQueryEngine GetQueryEngine(DataRetrievalInfo info)
        {
            ICollection<MappingEntity> crossSectionalMeasureMappings = info.BuildXSMeasures();
            IRDFDataQueryEngine queryEngine = null;

            if (info.MeasureComponent == null)
            {
                queryEngine = RDFSeriesDataQueryEngine.Instance;
            }

            return queryEngine;
        }

        #endregion
    }
}
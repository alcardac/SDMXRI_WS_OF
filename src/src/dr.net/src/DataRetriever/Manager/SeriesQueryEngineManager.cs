// --------------------------------------------------------------------------------------------------------------------
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

namespace Estat.Nsi.DataRetriever.Manager
{
    using System.Collections.Generic;

    using Estat.Nsi.DataRetriever.Engines;
    using Estat.Nsi.DataRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The <see cref="IQueryEngineManager{T}"/> implementation for time series
    /// </summary>
    internal class SeriesQueryEngineManager : IQueryEngineManager<DataRetrievalInfoSeries>
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesQueryEngineManager _instance = new SeriesQueryEngineManager();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesQueryEngineManager" /> class from being created.
        /// </summary>
        private SeriesQueryEngineManager()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesQueryEngineManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get a <see cref="IDataQueryEngine{T}"/> implementation based on the specified <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// a <see cref="IDataQueryEngine{T}"/> implementation based on the specified <paramref name="info"/> 
        /// </returns>
        public IDataQueryEngine<DataRetrievalInfoSeries> GetQueryEngine(DataRetrievalInfoSeries info)
        {
            ICollection<MappingEntity> crossSectionalMeasureMappings = info.BuildXSMeasures();
            IDataQueryEngine<DataRetrievalInfoSeries> queryEngine = null;

            if (info.MeasureComponent == null)
            {
                queryEngine = SeriesDataQueryEngine.Instance;
            }
            else if (crossSectionalMeasureMappings.Count > 0)
            {
                queryEngine = SeriesDataQueryEngineXsMeasureBuffered.Instance;
            }

            return queryEngine;
        }

        #endregion
    }
}
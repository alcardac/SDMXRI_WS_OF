// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryEngineManager.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The interface for manager classes that determine which <see cref="IDataQueryEngine" /> to use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Manager
{
    using Estat.Nsi.DataRetriever.Engines;
    using Estat.Nsi.DataRetriever.Model;

    /// <summary>
    /// The interface for manager classes that determine which <see cref="IDataQueryEngine{T}"/> to use.
    /// </summary>
    /// <typeparam name="T">
    /// The DR state type.
    /// </typeparam>
    internal interface IQueryEngineManager<in T>
        where T : DataRetrievalInfo
    {
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
        IDataQueryEngine<T> GetQueryEngine(T info);

        #endregion
    }
}
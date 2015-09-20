// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The i data query engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using Estat.Nsi.DataRetriever.Model;

    /// <summary>
    /// The i data query engine.
    /// </summary>
    /// <typeparam name="T">The DR state type.</typeparam>
    internal interface IDataQueryEngine<in T>
        where T : DataRetrievalInfo
    {
        #region Public Methods

        /// <summary>
        /// This method executes an SQL query on the dissemination database and writes it to the writer in <see cref="DataRetrievalInfo"/> . The SQL query is located inside <paramref name="info"/> at <see cref="DataRetrievalInfo.SqlString"/>
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
        void ExecuteSqlQuery(T info);

        #endregion
    }
}
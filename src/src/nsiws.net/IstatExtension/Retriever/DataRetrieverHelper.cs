// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrieverHelper.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A helper class with static methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever
{

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// A helper class with static methods
    /// </summary>
    public static class DataRetrieverHelper
    {
        #region Public Methods

        /// <summary>
        /// This method retrieves the <see cref="Estat.Sdmx.Model.Query.DataFlowBean"/> from a <see cref="Estat.Sdmx.Model.Query.QueryBean"/>
        /// </summary>
        /// <param name="query">
        /// The <see cref="Estat.Sdmx.Model.Query.QueryBean"/> conaining the <see cref="Estat.Sdmx.Model.Query.DataFlowBean"/> 
        /// </param>
        /// <exception cref="DataRetrieverException">
        /// See the
        ///   <see cref="ErrorTypes.QUERY_PARSING_ERROR"/>
        /// </exception>
        /// <returns>
        /// The <see cref="Estat.Sdmx.Model.Query.DataFlowBean"/> 
        /// </returns>
        public static IDataflowObject GetDataflowFromQuery(IDataQuery query)
        {
            return query.Dataflow;
        }

        #endregion
    }
}
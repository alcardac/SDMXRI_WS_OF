// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryBuilderManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Query
{
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// Builds a DataQuery in the required format 
    /// </summary>
    public interface IDataQueryBuilderManager
    {
        /// <summary>
        /// Builds a data query in the requested format
        /// </summary>
        /// <param name="dataQuery">the query to build a representation of</param>
        /// <param name="dataQueryFormat">the required format</param>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <returns>representation of query in the desired format.</returns>
        T BuildDataQuery<T>(IDataQuery dataQuery, IDataQueryFormat<T> dataQueryFormat); 
    }
}

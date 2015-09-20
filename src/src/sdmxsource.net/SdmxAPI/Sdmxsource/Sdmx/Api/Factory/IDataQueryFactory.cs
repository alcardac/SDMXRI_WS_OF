// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A DataQueryFactory is responsible fore creating a DataQueryBuilder that build a query that can be used for services external to the SdmxSource framework
    /// </summary>
    public interface IDataQueryFactory
    {
        /// <summary>
        /// Returns a DataQueryBuilder only if this factory understands the DataQueryFormat.  If the format is unknown, null will be returned
        /// </summary>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <param name="format">Format</param>
        /// <returns>DataQueryBuilder is this factory knows how to build this query format, or null if it doesn't</returns>
        IDataQueryBuilder<T> GetDataQueryBuilder<T>(IDataQueryFormat<T> format);
    }
}

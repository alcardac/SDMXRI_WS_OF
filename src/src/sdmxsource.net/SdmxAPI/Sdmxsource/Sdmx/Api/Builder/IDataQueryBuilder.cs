// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryBuilder.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// Responsible for building a Data Query that can be used to query services external to the SdmxSource framework
    /// </summary>
    public interface IDataQueryBuilder<out T>
    {
        /// <summary>
        /// Builds a DataQuery that matches the passed in format
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns></returns>
        T BuildDataQuery(IDataQuery query);
    }
}

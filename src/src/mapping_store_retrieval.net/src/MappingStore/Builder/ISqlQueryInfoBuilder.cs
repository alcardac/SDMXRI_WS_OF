// -----------------------------------------------------------------------
// <copyright file="ISqlQueryBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;

    /// <summary>
    /// The <see cref="SqlQueryInfo"/> builder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The source type
    /// </typeparam>
    internal interface ISqlQueryInfoBuilder<in T> : IBuilder<SqlQueryInfo, T>
    {
    }
}
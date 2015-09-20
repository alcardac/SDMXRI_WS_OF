// -----------------------------------------------------------------------
// <copyright file="ICommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;

    /// <summary>
    /// The CommandBuilder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="SqlQueryBase"/> based type
    /// </typeparam>
    internal interface ICommandBuilder<in T> : IBuilder<DbCommand, T>
        where T : SqlQueryBase
    {
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The AuthCommandBuilder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Collections.Generic;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The Authorization aware <see cref="DbCommand"/> Builder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input
    /// </typeparam>
    internal interface IAuthCommandBuilder<in T> where T : SqlQueryBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="SqlQueryBase"/> based class to build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        DbCommand Build(T buildFrom, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}
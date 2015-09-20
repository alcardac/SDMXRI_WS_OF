// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRESTStructureQueryManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Webservice
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    ///     The REST Structure Query Manager is responsible for sending a REST query message to a REST Uri
    /// </summary>
    public interface IRestStructureQueryManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Brokers a Structure Query to the REST Uri returning the resulting SdmxObjects
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="restUrl">
        /// The REST Uri.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/> .
        /// </returns>
        ISdmxObjects BrokerStructureQuery(IRestStructureQuery query, Uri restUrl);

        #endregion
    }
}
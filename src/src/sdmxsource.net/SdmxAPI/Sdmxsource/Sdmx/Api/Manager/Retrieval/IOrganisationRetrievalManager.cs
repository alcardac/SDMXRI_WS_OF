// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrganisationRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Used to retrieve Organisations
    /// </summary>
    public interface IOrganisationRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the agency with the given id
        /// </summary>
        /// <param name="agencyId">
        /// a period separated agency id, where the periods are used to define sub Agencies
        /// </param>
        /// <returns>
        /// DataProvider, or null if one can not be found with the given reference parameters
        /// </returns>
        IAgency GetAgency(string agencyId);

        /// <summary>
        /// Gets the data consumer belonging to the given agency, with the given id
        /// </summary>
        /// <param name="agencyId">
        /// a period separated agency id, where the periods are used to define sub Agencies
        /// </param>
        /// <param name="id">
        /// id of the data provider to return
        /// </param>
        /// <returns>
        /// DataProvider, or null if one can not be found with the given reference parameters
        /// </returns>
        IDataConsumer GetDataConsumerObject(string agencyId, string id);

        /// <summary>
        /// Gets the data provider belongning to the given agency, with the given id
        /// </summary>
        /// <param name="agencyId">
        /// a period separated agency id, where the periods are used to define sub Agencies
        /// </param>
        /// <param name="id">
        /// id of the data provider to return
        /// </param>
        /// <returns>
        /// DataProvider, or null if one can not be found with the given reference parameters
        /// </returns>
        IDataProvider GetDataProvider(string agencyId, string id);

        #endregion
    }
}
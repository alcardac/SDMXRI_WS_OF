// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthorizationProvider.cs" company="Eurostat">
//   Date Created : 2011-05-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Interface for Authorization Providers
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// Interface for Authorization Providers
    /// </summary>
    public interface IAuthorizationProvider
    {
        #region Public Methods

        /// <summary>
        /// Check if there is at least one dataflow with the specified ID in list of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow ID
        /// </param>
        /// <returns>
        /// True if there is at least one dataflow with the specified ID in list of allowed dataflows. Else false
        /// </returns>
        bool AccessControl(IUser user, string dataflowId);

        /// <summary>
        /// Check if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowRef">
        /// The <see cref="IMaintainableRefObject"/> to check
        /// </param>
        /// <returns>
        /// True if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </returns>
        bool AccessControl(IUser user, IMaintainableRefObject dataflowRef);

        /// <summary>
        /// Get the collection of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        ICollection<IMaintainableRefObject> GetDataflows(IUser user);

        /// <summary>
        /// Get the collection of allowed dataflows with the specific dataflow id
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow id
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        IEnumerable<IMaintainableRefObject> GetDataflows(IUser user, string dataflowId);

        #endregion
    }
}
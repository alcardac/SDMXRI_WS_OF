// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoAccessAuthorizationProvider.cs" company="Eurostat">
//   Date Created : 2011-06-20
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This implementation of IAuthorizationProvider interface provides no acess to any dataflow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// This implementation of IAuthorizationProvider interface provides no access to any dataflow.
    /// </summary>
    public class NoAccessAuthorizationProvider : IAuthorizationProvider
    {
        #region Public Methods

        /// <summary>
        /// Check if there is at least one dataflow with the specified ID in list of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance with the user name.
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow ID
        /// </param>
        /// <returns>
        /// True if there is at least one dataflow with the specified ID in list of allowed dataflows. Else false
        /// </returns>
        public bool AccessControl(IUser user, string dataflowId)
        {
            return false;
        }

        /// <summary>
        /// Check if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance with the user name.
        /// </param>
        /// <param name="dataflowRef">
        /// The <see cref="IMaintainableRefObject"/> to check
        /// </param>
        /// <returns>
        /// True if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </returns>
        public bool AccessControl(IUser user, IMaintainableRefObject dataflowRef)
        {
            return false;
        }

        /// <summary>
        /// Get the collection of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance with the user name.
        /// </param>
        /// <returns>
        /// An empty collection
        /// </returns>
        public ICollection<IMaintainableRefObject> GetDataflows(IUser user)
        {
            return new MaintainableRefObjectImpl[0];
        }

        /// <summary>
        /// Get the collection of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance with the user name.
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow ID
        /// </param>
        /// <returns>
        /// An empty collection
        /// </returns>
        public IEnumerable<IMaintainableRefObject> GetDataflows(IUser user, string dataflowId)
        {
            return new MaintainableRefObjectImpl[0];
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthenticationProvider.cs" company="Eurostat">
//   Date Created : 2011-05-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The interface for Authentication Provider
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// The interface for Authentication Provider
    /// </summary>
    public interface IAuthenticationProvider
    {
        #region Public Methods

        /// <summary>
        /// Authenticate the specified user
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> instance containing the user information
        /// </param>
        /// <returns>
        /// If the user is authenticated <see cref="AuthorizationProviderFactory"/> else null
        /// </returns>
        AuthorizationProviderFactory Authenticate(IUser user);

        #endregion
    }
}
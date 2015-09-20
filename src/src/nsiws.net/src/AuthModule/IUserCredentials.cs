// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserCredentials.cs" company="Eurostat">
//   Date Created : 2011-05-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Interface for implementations that retrieve user name and password
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Web;

    /// <summary>
    /// Interface for implementations that retrieve user name and password
    /// </summary>
    public interface IUserCredentials
    {
        #region Public Methods

        /// <summary>
        /// Parse the HTTP response from <paramref name="application"/> and populate the <paramref name="user"/>
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="user">
        /// The <see cref="IUser"/> object to populate
        /// </param>
        /// <returns>
        /// True if retrieving the user credentials were successfull
        /// </returns>
        bool ParseResponse(HttpApplication application, IUser user);

        /// <summary>
        /// Request authentication from client. This might be a no-op for some implementations
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="domain">
        /// The domain/realm to use when requesting authentication
        /// </param>
        void RequestAuthentication(HttpApplication application, string domain);

        #endregion
    }
}
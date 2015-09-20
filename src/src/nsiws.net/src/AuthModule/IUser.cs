// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUser.cs" company="Eurostat">
//   Date Created : 2011-06-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Interface for storing user credential and checking passwords
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// Interface for storing user credential and checking passwords
    /// </summary>
    public interface IUser
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Domain/Realm
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        string UserName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method checks if the given password matches with the possibly encypted <see cref="Password"/>
        /// </summary>
        /// <param name="password">
        /// The password from the authentication provider
        /// </param>
        /// <returns>
        /// True if specifed password == g(<see cref="Password"/>) where g is the encryption method depending on the implementation
        /// </returns>
        bool CheckPasswordEnc(string password);

        #endregion
    }
}
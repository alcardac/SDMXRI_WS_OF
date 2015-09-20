// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserPassThrough.cs" company="Eurostat">
//   Date Created : 2011-05-18
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An implementation of this <see cref="IUser" /> interface. This implementation doesn't encypt or encode the password in any way
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// An implementation of this <see cref="IUser"/> interface. This implementation doesn't encypt or encode the password in any way
    /// </summary>
    public class UserPassThrough : IUser
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Domain/Realm
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method checks if the given password matches with the  <see cref="IUser.Password"/>
        /// </summary>
        /// <param name="password">
        /// The password from the authentication provider
        /// </param>
        /// <returns>
        /// True if specifed password == <see cref="IUser.Password"/>. Else false
        /// </returns>
        public virtual bool CheckPasswordEnc(string password)
        {
            return string.Equals(this.Password, password);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthConfigSection.cs" company="Eurostat">
//   Date Created : 2011-09-07
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The main configuration section of the AuthModule
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Configuration;

    using Estat.Nsi.AuthModule.Config;

    /// <summary>
    /// The main configuration section of the AuthModule
    /// </summary>
    public class AuthConfigSection : ConfigurationSection
    {
        #region Constants and Fields

        /// <summary>
        /// The anon user name.
        /// </summary>
        private const string AnonUserName = "anonymousUser";

        /// <summary>
        /// The authentication impl name.
        /// </summary>
        private const string AuthenticationImplName = "authenticationImplementation";

        /// <summary>
        /// The authorization impl name.
        /// </summary>
        private const string AuthorizationImplName = "authorizationImplementation";

        /// <summary>
        /// The db auth name.
        /// </summary>
        private const string DbAuthName = "dbAuth";

        // attribute names

        /// <summary>
        /// The realm name.
        /// </summary>
        private const string RealmName = "realm";

        /// <summary>
        /// The user cred impl name.
        /// </summary>
        private const string UserCredImplName = "userCredentialsImplementation";

        /// <summary>
        /// The user impl name.
        /// </summary>
        private const string UserImplName = "userImplementation";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Anonymous user. if its not set or is empty or null, it's disabled
        /// </summary>
        [ConfigurationProperty(AnonUserName)]
        public string AnonymousUser
        {
            get
            {
                return (string)this[AnonUserName];
            }

            set
            {
                this[AnonUserName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IAuthenticationProvider"/> implementation element
        /// </summary>
        [ConfigurationProperty(AuthenticationImplName, IsRequired = true)]
        public AuthenticationImplementationElement AuthenticationImplementation
        {
            get
            {
                return (AuthenticationImplementationElement)this[AuthenticationImplName];
            }

            set
            {
                this[AuthenticationImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IAuthorizationProvider"/> implementation element
        /// </summary>
        [ConfigurationProperty(AuthorizationImplName, IsRequired = false)]
        public AuthorizationImplementationElement AuthorizationImplementation
        {
            get
            {
                return (AuthorizationImplementationElement)this[AuthorizationImplName];
            }

            set
            {
                this[AuthorizationImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/> configuration section
        /// </summary>
        [ConfigurationProperty(DbAuthName, IsRequired = false)]
        public DBAuthElement DBAuth
        {
            get
            {
                return (DBAuthElement)this[DbAuthName];
            }

            set
            {
                this[DbAuthName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Realm/Domain
        /// </summary>
        [ConfigurationProperty(RealmName, DefaultValue = "nsi")]
        public string Realm
        {
            get
            {
                return (string)this[RealmName];
            }

            set
            {
                this[RealmName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IUserCredentials"/> implementation element
        /// </summary>
        [ConfigurationProperty(UserCredImplName, IsRequired = true)]
        public UserCredentialsImplementationElement UserCredentialsImplementation
        {
            get
            {
                return (UserCredentialsImplementationElement)this[UserCredImplName];
            }

            set
            {
                this[UserCredImplName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IUser"/> implementation element
        /// </summary>
        [ConfigurationProperty(UserImplName, IsRequired = true)]
        public UserImplementationElement UserImplementation
        {
            get
            {
                return (UserImplementationElement)this[UserImplName];
            }

            set
            {
                this[UserImplName] = value;
            }
        }

        #endregion
    }
}
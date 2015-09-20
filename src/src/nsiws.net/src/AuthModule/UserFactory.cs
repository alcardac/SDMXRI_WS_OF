// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserFactory.cs" company="Eurostat">
//   Date Created : 2011-06-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Singleton class for creating <see cref="IUser" /> objects using the implementation specified in <see cref="AuthConfigSection.UserImplementation" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// Singleton class for creating <see cref="IUser"/> objects using the implementation specified in <see cref="AuthConfigSection.UserImplementation"/>
    /// </summary>
    public class UserFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly UserFactory _instance = new UserFactory();

        /// <summary>
        /// Holds the configured implementation to use
        /// </summary>
        private readonly string _configuredType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="UserFactory"/> class from being created. 
        /// Initialize a new instance of the <see cref="UserFactory"/> class
        /// </summary>
        private UserFactory()
        {
            this._configuredType = ConfigManager.Instance.Config.UserImplementation.ImplementationType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static UserFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IUser"/> implementation using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <param name="domain">
        /// The authentication domain/realm
        /// </param>
        /// <returns>
        /// A new <see cref="IUser"/> implementation object
        /// </returns>
        public IUser CreateUser(string domain)
        {
            return this.CreateUser(domain, null);
        }

        /// <summary>
        /// Create a new <see cref="IUser"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <param name="domain">
        /// The authentication domain/realm
        /// </param>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateUser(string)"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IUser"/> implementation object
        /// </returns>
        public IUser CreateUser(string domain, string type)
        {
            IUser user = Create<IUser>(!string.IsNullOrEmpty(type) ? type : this._configuredType);
            if (user != null)
            {
                user.Domain = domain;
            }

            return user;
        }

        #endregion
    }
}
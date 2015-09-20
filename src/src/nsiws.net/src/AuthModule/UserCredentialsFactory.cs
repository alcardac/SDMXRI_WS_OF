// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserCredentialsFactory.cs" company="Eurostat">
//   Date Created : 2011-06-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Singleton class for creating <see cref="IUserCredentials" /> objects using the implementation specified in <see cref="AuthConfigSection.UserCredentialsImplementation" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// Singleton class for creating <see cref="IUserCredentials"/> objects using the implementation specified in <see cref="AuthConfigSection.UserCredentialsImplementation"/>
    /// </summary>
    public class UserCredentialsFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly UserCredentialsFactory _instance = new UserCredentialsFactory();

        /// <summary>
        /// Holds the configured implementation to use
        /// </summary>
        private readonly string _configuredType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="UserCredentialsFactory"/> class from being created. 
        /// Initialize a new instance of the <see cref="UserCredentialsFactory"/> class
        /// </summary>
        private UserCredentialsFactory()
        {
            this._configuredType = ConfigManager.Instance.Config.UserCredentialsImplementation.ImplementationType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static UserCredentialsFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IUserCredentials"/> implementation using the implementation specified in <see cref="AuthConfigSection.UserCredentialsImplementation"/>
        /// </summary>
        /// <returns>
        /// A new <see cref="IUserCredentials"/> implementation object
        /// </returns>
        public IUserCredentials CreateUserCredentials()
        {
            return this.CreateUserCredentials(null);
        }

        /// <summary>
        /// Create a new <see cref="IUserCredentials"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.UserCredentialsImplementation"/>
        /// </summary>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateUserCredentials()"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IUserCredentials"/> implementation object
        /// </returns>
        public IUserCredentials CreateUserCredentials(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return Create<IUserCredentials>(this._configuredType);
            }

            return Create<IUserCredentials>(type);
        }

        #endregion
    }
}
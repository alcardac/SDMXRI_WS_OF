// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationProviderFactory.cs" company="Eurostat">
//   Date Created : 2011-06-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Singleton class for creating <see cref="IAuthenticationProvider" /> objects using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// Singleton class for creating <see cref="IAuthenticationProvider"/> objects using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation"/>
    /// </summary>
    public class AuthenticationProviderFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly AuthenticationProviderFactory _instance = new AuthenticationProviderFactory();

        /// <summary>
        /// Holds the configured implementation to use
        /// </summary>
        private readonly string _configuredType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AuthenticationProviderFactory"/> class from being created. 
        /// Initialize a new instance of the <see cref="AuthenticationProviderFactory"/> class
        /// </summary>
        private AuthenticationProviderFactory()
        {
            this._configuredType = ConfigManager.Instance.Config.AuthenticationImplementation.ImplementationType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static AuthenticationProviderFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IAuthenticationProvider"/> implementation using the implementation specified in <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <returns>
        /// A new <see cref="IAuthenticationProvider"/> implementation object
        /// </returns>
        public IAuthenticationProvider CreateAuthenticationProvider()
        {
            return this.CreateAuthenticationProvider(null);
        }

        /// <summary>
        /// Create a new <see cref="IAuthenticationProvider"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.AuthenticationImplementation"/>
        /// </summary>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateAuthenticationProvider()"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IAuthenticationProvider"/> implementation object
        /// </returns>
        public IAuthenticationProvider CreateAuthenticationProvider(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return Create<IAuthenticationProvider>(this._configuredType);
            }

            return Create<IAuthenticationProvider>(type);
        }

        #endregion
    }
}
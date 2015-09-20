// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizationProviderFactory.cs" company="Eurostat">
//   Date Created : 2011-06-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Class for creating <see cref="IAuthorizationProvider" /> objects using the implementation specified in <see cref="AuthConfigSection.AuthorizationImplementation" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// Class for creating <see cref="IAuthorizationProvider"/> objects using the implementation specified in <see cref="AuthConfigSection.AuthorizationImplementation"/>
    /// </summary>
    public class AuthorizationProviderFactory : AbstractFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The _configured type.
        /// </summary>
        private readonly string _configuredType;

        /// <summary>
        /// The _default authorization provider.
        /// </summary>
        private readonly IAuthorizationProvider _defaultAuthorizationProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderFactory"/> class. 
        /// Create a new instance of the <see cref="AuthorizationProviderFactory"/> class 
        /// </summary>
        public AuthorizationProviderFactory()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderFactory"/> class. 
        /// Create a new instance of the <see cref="AuthorizationProviderFactory"/> class with the specified default <see cref="IAuthorizationProvider"/>
        /// </summary>
        /// <param name="defaultAuthorizationProvider">
        /// The <see cref="IAuthorizationProvider"/> based object
        /// </param>
        public AuthorizationProviderFactory(IAuthorizationProvider defaultAuthorizationProvider)
        {
            if (ConfigManager.Instance.Config.AuthorizationImplementation != null)
            {
                this._configuredType = ConfigManager.Instance.Config.AuthorizationImplementation.ImplementationType;
            }

            this._defaultAuthorizationProvider = defaultAuthorizationProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new <see cref="IAuthorizationProvider"/> implementation using the implementation specified in <see cref="AuthConfigSection.AuthorizationImplementation"/>
        /// or the default <see cref="IAuthorizationProvider"/> specified in the constructor
        /// </summary>
        /// <returns>
        /// A new <see cref="IAuthorizationProvider"/> implementation object
        /// </returns>
        public IAuthorizationProvider CreateAuthorizationProvider()
        {
            return this.CreateAuthorizationProvider(null);
        }

        /// <summary>
        /// Create a new <see cref="IAuthorizationProvider"/> implementation using the <paramref name="type"/> or the <see cref="AuthConfigSection.AuthorizationImplementation"/>
        /// </summary>
        /// <param name="type">
        /// The implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method. If it is null then it is ignored and this method behaves like <see cref="CreateAuthorizationProvider()"/>
        /// </param>
        /// <returns>
        /// A new <see cref="IAuthorizationProvider"/> implementation object
        /// </returns>
        public IAuthorizationProvider CreateAuthorizationProvider(string type)
        {
            IAuthorizationProvider provider;
            switch (type)
            {
                case null:
                    if (!string.IsNullOrEmpty(this._configuredType))
                    {
                        provider = Create<IAuthorizationProvider>(this._configuredType);
                    }
                    else
                    {
                        return this._defaultAuthorizationProvider;
                    }

                    break;
                default:
                    provider = Create<IAuthorizationProvider>(type);
                    break;
            }

            return provider;
        }

        #endregion
    }
}
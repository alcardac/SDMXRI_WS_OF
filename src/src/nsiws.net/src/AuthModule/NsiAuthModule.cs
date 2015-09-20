// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NsiAuthModule.cs" company="Eurostat">
//   Date Created : 2011-05-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A HTTP Auth Module for IIS for restricting access to dataflows for data and structure requests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Globalization;
    using System.Security.Principal;
    using System.Web;

    using log4net;

    /// <summary>
    /// A HTTP Auth Module for IIS for restricting access to dataflows for data and structure requests.
    /// </summary>
    /// <remarks>
    /// You will need to configure this module in the web.config file of your
    /// web and register it with IIS before being able to use it. For more information
    /// see the following link: http://go.microsoft.com/?linkid=8101007
    /// </remarks>
    public class NsiAuthModule : IHttpModule
    {
        #region Constants and Fields

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NsiAuthModule));

        /// <summary>
        /// The _anon user.
        /// </summary>
        private readonly IUser _anonUser;

        /// <summary>
        /// The _authentication.
        /// </summary>
        private readonly IAuthenticationProvider _authentication;

        /// <summary>
        /// The _realm.
        /// </summary>
        private readonly string _realm;

        /// <summary>
        /// The _user cred.
        /// </summary>
        private readonly IUserCredentials _userCred;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NsiAuthModule"/> class. 
        /// Create a new instance of the <see cref="NsiAuthModule"/> class
        /// </summary>
        public NsiAuthModule()
        {
            _log.Debug("Starting SRI Authentication and dataflow authorization module.");
            AuthUtils.ValidateConfig(ConfigManager.Instance.Config, this.GetType());
            this._userCred = UserCredentialsFactory.Instance.CreateUserCredentials();

            this._authentication = AuthenticationProviderFactory.Instance.CreateAuthenticationProvider();

            this._realm = ConfigManager.Instance.Config.Realm;

            string anonUser = ConfigManager.Instance.Config.AnonymousUser;
            if (!string.IsNullOrEmpty(anonUser))
            {
                this._anonUser = UserFactory.Instance.CreateUser(this._realm);
                if (this._anonUser != null)
                {
                    this._anonUser.UserName = anonUser;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        /// <remarks>
        /// Noop for this implementation
        /// </remarks>
        public void Dispose()
        {
            // clean-up code here.
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application.
        /// </param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += this.OnAuthenticateRequest;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authentication Request event handler
        /// </summary>
        /// <param name="sender">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="e">
        /// Not used
        /// </param>
        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            IUser user = UserFactory.Instance.CreateUser(this._realm);
            if (user == null)
            {
                throw new AuthConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Errors.ImplementationMissingAttr, "IUser"));
            }

            var application = (HttpApplication)sender;
            AuthorizationProviderFactory authorizationProviderFactory;

            if (this._userCred.ParseResponse(application, user))
            {
                // there were user credentials in the request
                authorizationProviderFactory = this._authentication.Authenticate(user); // try to authenticate user
                if (authorizationProviderFactory == null)
                {
                    // failed
                    this._userCred.RequestAuthentication(application, this._realm);
                    return;
                }
            }
            else
            {
                if (this._anonUser == null)
                {
                    // anon user not allowed so request authentication (depends on implementation)
                    this._userCred.RequestAuthentication(application, this._realm);
                    return;
                }

                // anon user exists
                authorizationProviderFactory = new AuthorizationProviderFactory(new NoAccessAuthorizationProvider());

                user = this._anonUser;
            }

            IAuthorizationProvider authorizationProvider = authorizationProviderFactory.CreateAuthorizationProvider();
            if (authorizationProvider == null)
            {
                // couldn't create an authorization provider so we fail by default
                throw new AuthConfigurationException(Errors.MissingAuthorizationImplementation);
            }

            application.Context.User =
                new DataflowPrincipal(
                    new GenericIdentity(user.UserName, this._authentication.GetType().Name), authorizationProvider, user);
        }

        #endregion
    }
}
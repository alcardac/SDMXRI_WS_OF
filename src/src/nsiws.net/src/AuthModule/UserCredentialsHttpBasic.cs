// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserCredentialsHttpBasic.cs" company="Eurostat">
//   Date Created : 2011-05-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An implentation of the <see cref="IUserCredentials" /> interface.
//   This implementation uses HTTP HEADER and HTTP Basic authentication to retrieve the user credentials
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// An implentation of the <see cref="IUserCredentials"/> interface.
    /// This implementation uses HTTP HEADER and HTTP Basic authentication to retrieve the user credentials
    /// </summary>
    public class UserCredentialsHttpBasic : IUserCredentials
    {
        #region Constants and Fields

        /// <summary>
        /// Regular expression that parses the HTTP Basic Authentication header
        /// </summary>
        private static readonly Regex _basic = new Regex("^\\s*Basic\\s+(?<b64>[a-zA-Z0-9\\+/]+={0,2})\\s*$");

        /// <summary>
        /// The Basic authentication separator
        /// </summary>
        private static readonly char[] _basicSep = new[] { ':' };

        #endregion

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
        public bool ParseResponse(HttpApplication application, IUser user)
        {
            string response = application.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(response))
            {
                Match match = _basic.Match(response);
                if (match.Success)
                {
                    response = match.Result("${b64}");
                    if (!string.IsNullOrEmpty(response))
                    {
                        byte[] decoded = Convert.FromBase64String(response);

                        string s = Encoding.UTF8.GetString(decoded);
                        string[] creds = s.Split(_basicSep, 2);
                        if (creds.Length == 2)
                        {
                            user.UserName = creds[0];
                            user.Password = creds[1];
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Request authentication from client
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        /// <param name="domain">
        /// The domain/realm to use when requesting authentication
        /// </param>
        public void RequestAuthentication(HttpApplication application, string domain)
        {
            CreateDeniedResponse(application);

            // use AddHeader because IIS 6.0 and .net 2.0 should be supported
            application.Response.AddHeader(
                "WWW-Authenticate", string.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", domain));
            application.CompleteRequest();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create an 401 (Access Denied) HTTP response
        /// </summary>
        /// <param name="application">
        /// The current <see cref="HttpApplication"/> instance
        /// </param>
        private static void CreateDeniedResponse(HttpApplication application)
        {
            application.Response.StatusCode = 401;
            application.Response.StatusDescription = "Authorization Required";
            application.Response.Write("401 Unauthorized");
        }

        #endregion
    }
}
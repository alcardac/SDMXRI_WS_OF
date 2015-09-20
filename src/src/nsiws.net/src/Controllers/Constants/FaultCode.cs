// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultCode.cs" company="Eurostat">
//   Date Created : 2013-10-24
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The fault code defaults.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Constants
{
    using System.ServiceModel;

    /// <summary>
    /// The fault code defaults.
    /// </summary>
    public static class FaultCodeDefaults
    {
        #region Static Fields

        /// <summary>
        /// The _client.
        /// </summary>
        private static readonly FaultCode _client;

        /// <summary>
        /// The _server.
        /// </summary>
        private static readonly FaultCode _server;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="FaultCodeDefaults"/> class.
        /// </summary>
        static FaultCodeDefaults()
        {
            _client = new FaultCode("Client", "http://schemas.xmlsoap.org/soap/envelope/");
            _server = new FaultCode("Server", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the client.
        /// </summary>
        public static FaultCode Client
        {
            get
            {
                return _client;
            }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public static FaultCode Server
        {
            get
            {
                return _server;
            }
        }

        #endregion
    }
}
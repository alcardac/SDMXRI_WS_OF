// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageFaultSoapv20Builder.cs" company="Eurostat">
//   Date Created : 2013-10-25
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The web fault exception rest builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.ServiceModel;

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Controllers.Properties;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The web fault exception rest builder.
    /// </summary>
    public class MessageFaultSoapv20Builder
    {
        #region Static Fields

        /// <summary>
        /// The _error number client.
        /// </summary>
        private static readonly int _errorNumberClient;

        /// <summary>
        /// The _error number server.
        /// </summary>
        private static readonly int _errorNumberServer;

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(MessageFaultSoapv20Builder));

        #endregion

        #region Fields

        /// <summary>
        ///     The _client fault reason.
        /// </summary>
        private readonly FaultReason _clientFaultReason;

        /// <summary>
        ///     The _fault reason server.
        /// </summary>
        private readonly FaultReason _faultReasonServer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="MessageFaultSoapv20Builder" /> class.
        /// </summary>
        static MessageFaultSoapv20Builder()
        {
            _errorNumberServer = int.Parse(SdmxV20Errors.ErrorNumberServer);
            _errorNumberClient = int.Parse(SdmxV20Errors.ErrorNumberClient);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageFaultSoapv20Builder" /> class.
        /// </summary>
        public MessageFaultSoapv20Builder()
        {
            this._faultReasonServer = new FaultReason(Resources.ErrorInternalError);
            this._clientFaultReason = new FaultReason(Resources.ErrorClientMessage);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type  <see cref="FaultException"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="FaultException{String}"/>
        /// </returns>
        public FaultException<SdmxFault> BuildException(SdmxException buildFrom, string uri)
        {
            _log.ErrorFormat("SdmxError : {0}, code : {1}", buildFrom.SdmxErrorCode.ErrorString, buildFrom.SdmxErrorCode.ClientErrorCode);
            _log.Error(buildFrom.FullMessage, buildFrom);
            if (buildFrom.SdmxErrorCode.EnumType.IsClientError())
            {
                var clientFault = new SdmxFault(buildFrom.SdmxErrorCode.ErrorString, _errorNumberClient, uri);
                return this.CreateExceptionClient(clientFault);
            }

            var sdmxFault = new SdmxFault(buildFrom.SdmxErrorCode.ErrorString, _errorNumberServer, uri);
            return this.CreateExceptionServer(sdmxFault);
        }

        /// <summary>
        /// Builds an object of type  <see cref="FaultException{String}"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        /// <returns>
        /// Object of type <see cref="FaultException{String}"/>
        /// </returns>
        public FaultException<SdmxFault> BuildException(Exception buildFrom, string uri)
        {
            var faultException = buildFrom as FaultException<SdmxFault>;
            if (faultException != null)
            {
                return faultException;
            }

            var webException = buildFrom as FaultException;
            if (webException != null)
            {
                SdmxFault fault;
                if (webException.Code.IsReceiverFault)
                {
                    fault = new SdmxFault(Resources.ErrorInternalError, _errorNumberServer, uri);
                    return this.CreateExceptionServer(fault);
                }

                fault = new SdmxFault(buildFrom.Message, _errorNumberClient, uri);
                return this.CreateExceptionClient(fault);
            }

            var sdmxException = buildFrom.ToSdmxException();
            if (sdmxException != null)
            {
                return this.BuildException(sdmxException, uri);
            }

            return this.CreateExceptionServer(new SdmxFault(Resources.ErrorInternalError, _errorNumberServer, uri));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the exception client.
        /// </summary>
        /// <param name="sdmxFault">
        /// The SDMX fault.
        /// </param>
        /// <returns>
        /// The <see cref="FaultException"/>.
        /// </returns>
        private FaultException<SdmxFault> CreateExceptionClient(SdmxFault sdmxFault)
        {
            return new FaultException<SdmxFault>(sdmxFault, this._clientFaultReason, FaultCodeDefaults.Client);
        }

        /// <summary>
        /// Creates the exception server.
        /// </summary>
        /// <param name="sdmxFault">
        /// The SDMX fault.
        /// </param>
        /// <returns>
        /// The <see cref="FaultException"/>.
        /// </returns>
        private FaultException<SdmxFault> CreateExceptionServer(SdmxFault sdmxFault)
        {
            return new FaultException<SdmxFault>(sdmxFault, this._faultReasonServer, FaultCodeDefaults.Server);
        }

        #endregion
    }
}
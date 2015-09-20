// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandler.cs" company="Eurostat">
//   Date Created : 2013-10-25
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The error handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Estat.Sri.Ws.Controllers.Model;

    using log4net;

    /// <summary>
    ///     The error handler.
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ErrorHandler));

        #endregion

        #region Fields

        /// <summary>
        ///     The _fault builder
        /// </summary>
        private readonly Func<Exception, FaultException<SdmxFault>> _faultBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandler"/> class.
        /// </summary>
        /// <param name="faultBuilder">
        /// The fault builder.
        /// </param>
        public ErrorHandler(Func<Exception, FaultException<SdmxFault>> faultBuilder)
        {
            this._faultBuilder = faultBuilder;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and
        ///     the instance context in certain cases.
        /// </summary>
        /// <returns>
        /// true if  should not abort the session (if there is one) and instance context if the instance context is not
        ///     <see cref="F:System.ServiceModel.InstanceContextMode.Single"/>; otherwise, false. The default is false.
        /// </returns>
        /// <param name="error">
        /// The exception thrown during processing.
        /// </param>
        public bool HandleError(Exception error)
        {
            _log.Error("Handle error", error);
            return true;
        }

        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an
        ///     exception in the course of a service method.
        /// </summary>
        /// <param name="error">
        /// The <see cref="T:System.Exception"/> object thrown in the course of the service operation.
        /// </param>
        /// <param name="version">
        /// The SOAP version of the message.
        /// </param>
        /// <param name="fault">
        /// The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or
        ///     service, in the duplex case.
        /// </param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (fault != null && fault.IsFault)
            {
                return;
            }

            var faultException = this._faultBuilder(error);
            var messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, string.Empty);
        }

        #endregion
    }
}
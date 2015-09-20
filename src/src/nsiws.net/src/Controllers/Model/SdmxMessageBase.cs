// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxMessageBase.cs" company="Eurostat">
//   Date Created : 2013-11-18
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base SDMX message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Model
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Controller;

    using log4net;

    /// <summary>
    ///     The base SDMX message.
    /// </summary>
    public abstract class SdmxMessageBase : Message
    {
        #region Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SdmxMessageBase));

        /// <summary>
        ///     The _actions
        /// </summary>
        private readonly Queue<Action> _actions = new Queue<Action>();

        /// <summary>
        ///     The controller
        /// </summary>
        private readonly IStreamController<XmlWriter> _controller;

        /// <summary>
        ///     The _exception handler
        /// </summary>
        private readonly Func<Exception, FaultException> _exceptionHandler;

        /// <summary>
        ///     The _message version
        /// </summary>
        private readonly MessageVersion _messageVersion;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxMessageBase"/> class.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <param name="exceptionHandler">
        /// The exception handler.
        /// </param>
        /// <param name="messageVersion">
        /// The message version.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// controller
        ///     or
        ///     exceptionHandler
        ///     or
        ///     messageVersion
        /// </exception>
        protected SdmxMessageBase(IStreamController<XmlWriter> controller, Func<Exception, FaultException> exceptionHandler, MessageVersion messageVersion)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            if (exceptionHandler == null)
            {
                throw new ArgumentNullException("exceptionHandler");
            }

            if (messageVersion == null)
            {
                throw new ArgumentNullException("messageVersion");
            }

            this._controller = controller;
            this._exceptionHandler = exceptionHandler;
            this._messageVersion = messageVersion;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     When overridden in a derived class, gets the headers of the message.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.ServiceModel.Channels.MessageHeaders" /> object that represents the headers of the message.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
        public override MessageHeaders Headers
        {
            get
            {
                return new MessageHeaders(this._messageVersion);
            }
        }

        /// <summary>
        ///     When overridden in a derived class, gets a set of processing-level annotations to the message.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.ServiceModel.Channels.MessageProperties" /> that contains a set of processing-level
        ///     annotations to the message.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
        public override MessageProperties Properties
        {
            get
            {
                return new MessageProperties();
            }
        }

        /// <summary>
        ///     When overridden in a derived class, gets the SOAP version of the message.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.ServiceModel.Channels.MessageVersion" /> object that represents the SOAP version.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
        public override MessageVersion Version
        {
            get
            {
                return this._messageVersion;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the action queue
        /// </summary>
        protected Queue<Action> ActionQueue
        {
            get
            {
                return this._actions;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the message body is written to an XML file.
        /// </summary>
        /// <param name="writer">
        /// A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write this message body to an
        ///     XML file.
        /// </param>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            try
            {
                this._controller.StreamTo(writer, this._actions);
                writer.Flush();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw this._exceptionHandler(e);
            }
        }

        #endregion
    }
}
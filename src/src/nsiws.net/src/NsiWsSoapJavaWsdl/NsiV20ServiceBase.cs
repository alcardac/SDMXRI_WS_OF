// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NsiV20ServiceBase.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX  v2.0 service base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Web;
    using System.Xml;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Controller;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The SDMX  v2.0 service base class.
    /// </summary>
    public abstract class NsiV20ServiceBase
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NsiV20ServiceBase));

        /// <summary>
        ///     The _fault builder
        /// </summary>
        private static readonly MessageFaultSoapv20Builder _messageFaultBuilder = new MessageFaultSoapv20Builder();

        #endregion

        #region Fields

        /// <summary>
        ///     The _controller builder
        /// </summary>
        private readonly ControllerBuilder _controllerBuilder = new ControllerBuilder();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the type of the endpoint.
        /// </summary>
        /// <value>
        ///     The type of the endpoint.
        /// </value>
        protected abstract WebServiceEndpoint EndpointType { get; }

        /// <summary>
        ///     Gets the namespace
        /// </summary>
        /// <value>
        ///     The namespace
        /// </value>
        protected abstract string Ns { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message DefaultHandler(Message request)
        {
            throw _messageFaultBuilder.BuildException(new SdmxNotImplementedException("Method not implemented"), "Default Handler");
        }

        /// <summary>
        /// Web Method that is used to retrieve SDMX data in compact format based on a SDMX query
        /// </summary>
        /// <param name="request">
        /// The SDMX query
        /// </param>
        /// <returns>
        /// The queried data in SDMX compact format
        /// </returns>
        public Message GetCompactData(Message request)
        {
            return this.HandleDataRequest(request, BaseDataFormatEnumType.Compact);
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in cross sectional format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx compact format
        /// </returns>
        public Message GetCrossSectionalData(Message request)
        {
            return this.HandleDataRequest(request, BaseDataFormatEnumType.CrossSectional);
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in generic format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx compact format
        /// </returns>
        public Message GetGenericData(Message request)
        {
            return this.HandleDataRequest(request, BaseDataFormatEnumType.Generic);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// Not implemented/
        /// </returns>
        public Message GetUtilityData(Message request)
        {
            throw new WebFaultException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx structural metadata based on a sdmx query structure request
        /// </summary>
        /// <param name="request">
        /// The sdmx query structure request
        /// </param>
        /// <returns>
        /// The sdmx structural metadata inside a RegistryInterface QueryStructureResponse
        /// </returns>
        public Message QueryStructure(Message request)
        {
            var controller = this._controllerBuilder.BuildQueryStructureV20FromMessage(this.EndpointType, HttpContext.Current.User as DataflowPrincipal);
            return HandleRequest(request, controller, new XmlQualifiedName("QueryStructureResponse", this.Ns), SoapOperation.QueryStructure);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method that handle the processing of the sdmx query and orchestrate the
        ///     calls to different building blocks.
        /// </summary>
        /// <param name="input">
        /// The xml containing the SDMX Query
        /// </param>
        /// <param name="controller">
        /// The Controller of the request
        /// </param>
        /// <param name="xmlQualifiedName">
        /// Name of the XML qualified.
        /// </param>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <returns>
        /// The queried data in specified format
        /// </returns>
        /// <exception cref="System.ServiceModel.Web.WebFaultException">
        /// Internal server error.
        /// </exception>
        private static Message HandleRequest(Message input, IController<Message, XmlWriter> controller, XmlQualifiedName xmlQualifiedName, SoapOperation operation)
        {
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Request: {0}\n", input);
            }

            try
            {
                IStreamController<XmlWriter> streamController = controller.ParseRequest(input);
                WebOperationContext ctx = WebOperationContext.Current;
                if (ctx == null)
                {
                    _log.Error("Current WebOperationContext is null. Please check service configuration.");
                    throw new WebFaultException(HttpStatusCode.InternalServerError);
                }

                Message message = new SdmxMessageSoap(
                    streamController, 
                    xmlQualifiedName: xmlQualifiedName, 
                    exceptionHandler: exception => _messageFaultBuilder.BuildException(exception, operation.ToString()));

                return message;
            }
            catch (Exception e)
            {
                _log.Error(operation.ToString(), e);
                throw _messageFaultBuilder.BuildException(e, operation.ToString());
            }
        }

        /// <summary>
        /// The handle data request.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="baseDataFormat">
        /// The base data format.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        private Message HandleDataRequest(Message request, BaseDataFormatEnumType baseDataFormat)
        {
            var dataFormat = BaseDataFormat.GetFromEnum(baseDataFormat);
            var operation = dataFormat.GetSoapOperation(SdmxSchemaEnumType.VersionTwo);
            try
            {
                var response = operation.GetResponse().ToString();
                var controller = this._controllerBuilder.BuildDataV20FromMessage(HttpContext.Current.User as DataflowPrincipal, dataFormat);
                return HandleRequest(request, controller, new XmlQualifiedName(response, this.Ns), operation);
            }
            catch (FaultException<SdmxFault>)
            {
                throw;
            }
            catch (Exception e)
            {
                _log.Error(operation.ToString(), e);
                throw _messageFaultBuilder.BuildException(e, operation.ToString());
            }
        }

        #endregion
    }
}
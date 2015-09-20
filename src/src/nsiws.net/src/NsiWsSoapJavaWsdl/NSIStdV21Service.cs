// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NSIStdV21Service.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMXv2.1 SOAP service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.Globalization;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
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
    ///     The SDMXv2.1 SOAP service.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NSIStdV21Service : INSIStdV21Service
    {
        #region Constants

        /// <summary>
        ///     The Name-space of SDMX v2.1 services.
        /// </summary>
        private const string Ns = SoapNamespaces.SdmxV21;

        #endregion

        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NSIStdV21Service));

        /// <summary>
        ///     The _fault builder
        /// </summary>
        private static readonly MessageFaultSoapv21Builder _messageFaultBuilder = new MessageFaultSoapv21Builder();

        #endregion

        #region Fields

        /// <summary>
        ///     The _controller builder
        /// </summary>
        private readonly ControllerBuilder _controllerBuilder = new ControllerBuilder();

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
            var soapOperation = request.GetSoapOperation();
            if (soapOperation != SoapOperation.Null)
            {
                throw _messageFaultBuilder.BuildException(new SdmxNotImplementedException("Method not implemented"), soapOperation.ToString());
            }

            throw _messageFaultBuilder.BuildException(new SdmxSyntaxException(string.Format(CultureInfo.InvariantCulture, "Syntax error: invalid operation {0}", soapOperation)), soapOperation.ToString());
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetCategorisation(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.Categorisation);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetCategoryScheme(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.CategoryScheme);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetCodelist(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.CodeList);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetConceptScheme(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.ConceptScheme);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetDataStructure(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.Dsd);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetDataflow(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.Dataflow);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        public Message GetGenericData(Message request)
        {
            return this.HandleDataRequest(request, BaseDataFormatEnumType.Generic);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        public Message GetGenericTimeSeriesData(Message request)
        {
            throw _messageFaultBuilder.BuildException(new SdmxNotImplementedException("Method not implemented"), "GetGenericTimeSeriesData");
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetHierarchicalCodelist(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.HierarchicalCodelist);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        public Message GetStructureSpecificData(Message request)
        {
            return this.HandleDataRequest(request, BaseDataFormatEnumType.Compact);
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        public Message GetStructureSpecificTimeSeriesData(Message request)
        {
            throw _messageFaultBuilder.BuildException(new SdmxNotImplementedException("Method not implemented"), "GetStructureSpecificTimeSeriesData");
        }

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        public Message GetStructures(Message request)
        {
            return this.HandleStructureRequest(request, SdmxStructureEnumType.Null);
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
        /// <param name="getSoapOperation">
        /// The get SOAP operation.
        /// </param>
        /// <returns>
        /// The queried data in specified format
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxInternalServerException">
        /// Not initialized correctly
        /// </exception>
        private static Message HandleRequest(Message input, IController<Message, XmlWriter> controller, XmlQualifiedName xmlQualifiedName, SoapOperation getSoapOperation)
        {
            try
            {
                IStreamController<XmlWriter> streamController = controller.ParseRequest(input);
                WebOperationContext ctx = WebOperationContext.Current;
                if (ctx == null)
                {
                    _log.Error("Current WebOperationContext is null. Please check service configuration.");
                    throw new SdmxInternalServerException("Not initialized correctly");
                }

                Message message = new SdmxMessageSoap(streamController, exception => _messageFaultBuilder.BuildException(exception, getSoapOperation.ToString()), xmlQualifiedName);

                return message;
            }
            catch (Exception e)
            {
                _log.Error(xmlQualifiedName, e);

                ////return Message.CreateMessage(MessageVersion.Soap11, _messageFaultBuilder.Build(e, xmlQualifiedName.Name), string.Empty);
                throw _messageFaultBuilder.BuildException(e, getSoapOperation.ToString());
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
            var response = dataFormat.GetSoapOperationResponse(SdmxSchemaEnumType.VersionTwoPointOne).ToString();
            var controller = this._controllerBuilder.BuildDataV21Advanced(HttpContext.Current.User as DataflowPrincipal, dataFormat);
            return HandleRequest(request, controller, new XmlQualifiedName(response, Ns), dataFormat.GetSoapOperation(SdmxSchemaEnumType.VersionTwoPointOne));
        }

        /// <summary>
        /// The handle structure request.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="sdmxStructure">
        /// The sdmx structure.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        private Message HandleStructureRequest(Message request, SdmxStructureEnumType sdmxStructure)
        {
            var soapOperation = StructureOutputFormatEnumType.SdmxV21StructureDocument.GetSoapOperation(SdmxSchemaEnumType.VersionTwoPointOne, sdmxStructure);
            var responseElement = soapOperation.GetResponse().ToString();
            var controller = this._controllerBuilder.BuildAdvancedQueryStructureV21(HttpContext.Current.User as DataflowPrincipal, soapOperation);
            return HandleRequest(request, controller, new XmlQualifiedName(responseElement, Ns), soapOperation);
        }

        #endregion
    }
}
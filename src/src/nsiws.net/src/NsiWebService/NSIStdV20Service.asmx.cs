// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NSIStdV20Service.asmx.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Data Dissemination Web Service is used by the NSI to diseminate
//   data, exposing it for queries. It is the main class of the NSI web service
//   it encapsulates of the application logic and it is responsible for performing
//   the call to the QueryParser, applying the query and returning the resulting dataset.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.DataDisseminationWS
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Services;
    using System.Xml;
    using System.Xml.Serialization;

    using Estat.Nsi.AuthModule;
    using Estat.Nsi.SdmxSoapValidatorExtension;
    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Controller;
    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     Data Dissemination Web Service is used by the NSI to disseminate
    ///     data, exposing it for queries. It is the main class of the NSI web service
    ///     it encapsulates of the application logic and it is responsible for performing
    ///     the call to the QueryParser, applying the query and returning the resulting dataset.
    /// </summary>
    /// <example>
    ///     A simple <see cref="NSIStdV20Service" /> client in C#
    ///     <code source="ReUsingExamples\NsiWebService\ReUsingWebService.cs" lang="cs" />
    ///     The <code>WSDLSettings</code> class used by the client
    ///     <code source="ReUsingExamples\NsiWebService\WSDLSettings.cs" lang="cs" />
    /// </example>
    [WebService(Namespace = "http://ec.europa.eu/eurostat/sri/service/2.0", 
        Description = "Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the standard SDMX 2.0 Schema files")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class NSIStdV20Service : WebService
    {
        #region Static Fields

        /// <summary>
        ///     The logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NSIStdV20Service));

        #endregion

        #region Fields

        /// <summary>
        ///     The _controller builder
        /// </summary>
        private readonly ControllerBuilder _controllerBuilder;

        /// <summary>
        /// The _soap fault exception builder
        /// </summary>
        private readonly SoapFaultExceptionBuilder _soapFaultExceptionBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NSIStdV20Service" /> class.
        /// </summary>
        public NSIStdV20Service()
        {
            _controllerBuilder = new ControllerBuilder();
            _soapFaultExceptionBuilder = new SoapFaultExceptionBuilder();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="SdmxSchema" /> for this service
        /// </summary>
        protected virtual WebServiceEndpoint Endpoint
        {
            get
            {
                return WebServiceEndpoint.StandardEndpoint;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in compact format based on a sdmx query
        /// </summary>
        /// <param name="query">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx compact format
        /// </returns>
        [SdmxSoapValidator]
        [WebMethod(Description = "Web Method used to retrieve sdmx data in compact format", BufferResponse = false)]
        [SuppressMessage("Microsoft.Design", "CA1059", Justification = "It is a web method, arguments and return type needs to be serializable")]
        public XmlStreamWrapper GetCompactData([XmlElement("Query")] XmlNode query)
        {
            // to enable it for WS-I
            // [return: XmlAnyElement]
            // [XmlAnyElement]XmlDocument Query
            try
            {
                var simpleDataController = this._controllerBuilder.BuildCompactDataV20(this.Context.User as DataflowPrincipal);
                return HandleRequest(query, simpleDataController, SoapOperation.GetCompactData);
            }
            catch (Exception ex)
            {
                throw this._soapFaultExceptionBuilder.Build(ex, "GetCompactData");
            }
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in cross sectional format based on a sdmx query
        /// </summary>
        /// <param name="query">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx cross sectional format
        /// </returns>
        [SdmxSoapValidator]
        [WebMethod(Description = "Web Method used to retrieve sdmx data in cross sectional format", BufferResponse = false)]
        [SuppressMessage("Microsoft.Design", "CA1059", Justification = "It is a web method, arguments and return type needs to be serializable")]
        public XmlStreamWrapper GetCrossSectionalData([XmlElement("Query")] XmlDocument query)
        {
            try
            {
                var simpleDataController = this._controllerBuilder.BuildCrossSectionalDataV20(this.Context.User as DataflowPrincipal);
                return HandleRequest(query, simpleDataController, SoapOperation.GetCrossSectionalData);
            }
            catch (Exception ex)
            {
                throw this._soapFaultExceptionBuilder.Build(ex, "GetCrossSectionalData");
            }
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in generic format based on a sdmx query
        /// </summary>
        /// <param name="query">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx generic format
        /// </returns>
        [SdmxSoapValidator]
        [WebMethod(Description = "Web Method used to retrieve sdmx data in generic format", BufferResponse = false)]
        [SuppressMessage("Microsoft.Design", "CA1059", Justification = "It is a web method, arguments and return type needs to be serializable")]
        public XmlStreamWrapper GetGenericData([XmlElement("Query")] XmlDocument query)
        {
            try
            {
                var simpleDataController = this._controllerBuilder.BuildGenericDataV20(this.Context.User as DataflowPrincipal);
                return HandleRequest(query, simpleDataController, SoapOperation.GetGenericData);
            }
            catch (Exception ex)
            {
                throw this._soapFaultExceptionBuilder.Build(ex, "GetGenericData");
            }
        }

        /// <summary>
        /// Web Method that is used to retrieve sdmx structural metadata based on a sdmx query structure request
        /// </summary>
        /// <param name="query">
        /// The sdmx query structure request
        /// </param>
        /// <returns>
        /// The sdmx structural metadata inside a RegistryInterface QueryStructureResponse
        /// </returns>
        [SdmxSoapValidator]
        [WebMethod(Description = "Web Method that is used to retrieve sdmx structural metadata", BufferResponse = false)]
        [SuppressMessage("Microsoft.Design", "CA1059", Justification = "It is a web method, arguments and return type needs to be serializable")]
        public XmlStreamWrapper QueryStructure([XmlElement("Query")] XmlDocument query)
        {
            // to enable it for WS-I
            // [return: XmlAnyElement]
            // [XmlAnyElement]XmlDocument Query
            try
            {
                var simpleDataController = this._controllerBuilder.BuildQueryStructureV20(this.Endpoint, this.Context.User as DataflowPrincipal);
                return HandleRequest(query, simpleDataController, SoapOperation.QueryStructure);
            }
            catch (Exception ex)
            {
                throw this._soapFaultExceptionBuilder.Build(ex, "QueryStructure");
            }
        }

        /// <summary>
        /// Web Method that is used for testing purposes and retrieve SDMX data by calling the other
        ///     web methods of this service. The method accept as an input a file path to the xml file
        ///     containing the SDMX query, and a string specifying the format for the output data
        /// </summary>
        /// <param name="xmlQueryPath">
        /// The complete file path to the file containing the SDMX query
        /// </param>
        /// <param name="targetFormat">
        /// The string specifying the format. The accepted values are:
        ///     <list type="bullet">
        /// <item>
        /// compact
        ///         </item>
        /// <item>
        /// cross
        ///         </item>
        /// <item>
        /// general
        ///         </item>
        /// </list>
        /// </param>
        /// <returns>
        /// The result of the query in the specified format
        /// </returns>
        [WebMethod(Description = "Web Method used for testing purposes using a file as an input request instead of a " + "XmlDocuments objects")]
        [SuppressMessage("Microsoft.Design", "CA1059", Justification = "It is a web method, arguments and return type needs to be serializable")]
        public XmlStreamWrapper Test(string xmlQueryPath, string targetFormat)
        {
            if (xmlQueryPath == null)
            {
                throw new ArgumentNullException("xmlQueryPath");
            }

            if (targetFormat == null)
            {
                throw new ArgumentNullException("targetFormat");
            }

            var input = new XmlDocument();

            input.LoadXml(xmlQueryPath);

            XmlStreamWrapper output = null;
            targetFormat = targetFormat.ToUpperInvariant();
            if (targetFormat == BaseDataFormatEnumType.Compact.ToString().ToUpperInvariant())
            {
                output = this.GetCompactData(input);
            }
            else if (targetFormat == BaseDataFormatEnumType.CrossSectional.ToString().ToUpperInvariant())
            {
                output = this.GetCrossSectionalData(input);
            }
            else if (targetFormat == BaseDataFormatEnumType.Generic.ToString().ToUpperInvariant())
            {
                output = this.GetGenericData(input);
            }

            return output;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method that handle the processing of the SDMX query and orchestrate the
        /// calls to different building blocks.
        /// </summary>
        /// <param name="input">The xml containing the query bean</param>
        /// <param name="controller">The Controller of the request</param>
        /// <param name="soapOperation">The SOAP operation.</param>
        /// <returns>
        /// The queried data in specified format
        /// </returns>
        private XmlStreamWrapper HandleRequest(XmlNode input, IController<XmlNode, XmlWriter> controller, SoapOperation soapOperation)
        {
            _logger.DebugFormat(CultureInfo.InvariantCulture, "Received request for {0} for endpoint {1}", soapOperation, this.Endpoint);
            IStreamController<XmlWriter> streamController = controller.ParseRequest(input);

            return new XmlStreamWrapper(streamController, soapOperation);
        }

        #endregion
    }
}
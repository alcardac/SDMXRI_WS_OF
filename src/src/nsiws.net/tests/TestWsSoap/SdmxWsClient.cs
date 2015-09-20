// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxWsClient.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test stream SDMX SOAP WS client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestWsSoap
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    using TestWsSoap.Properties;

    /// <summary>
    ///     Test stream SDMX SOAP WS client.
    /// </summary>
    public class SdmxWsClient
    {
        #region Constants

        /// <summary>
        ///     The buffer size.
        /// </summary>
        private const int BufferSize = 4096;

        /// <summary>
        ///     This field holds a template for soap 1.1 request envelope
        /// </summary>
        private const string SoapRequest =
            "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:{0}=\"{1}\"><soap:Header/><soap:Body><{0}:{2}>{3}</{0}:{2}></soap:Body></soap:Envelope>";

        /// <summary>
        ///     This field holds a template for soap 1.1 request envelope
        /// </summary>
        private const string SoapRequestParameter = "<{0}:{1}>{2}</{0}:{1}>";

        /// <summary>
        ///     The prefix for the web service namespace it can be anything
        /// </summary>
        private const string WsPrefix = "web";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxWsClient));

        #endregion

        #region Fields

        /// <summary>
        /// The WS info.
        /// </summary>
        private readonly WsInfo _webServiceInfo;

        /// <summary>
        /// The _settings
        /// </summary>
        private readonly Settings _settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxWsClient"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="sdmxSchema">
        /// The sdmx Schema.
        /// </param>
        public SdmxWsClient(Uri endpoint, SdmxSchemaEnumType sdmxSchema)
        {
            var info = SetupRequestInfo(endpoint);
            info.SdmxSchemaVersion = SdmxSchema.GetFromEnum(sdmxSchema);
            this._webServiceInfo = info;
            this._settings = Settings.Default;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Invokes the service.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="compress">
        /// The compress.
        /// </param>
        public void InvokeService(SoapOperation operation, FileInfo request, FileInfo response, bool compress = false)
        {
            try
            {
                if (response.DirectoryName != null)
                {
                    Directory.CreateDirectory(response.DirectoryName);
                }

                var requestInfo = new RequestInfo { Operation = operation, Request = request, Response = response, Compress = compress };
                var operationName = requestInfo.Operation.ToString();
                requestInfo.Parameter = this._webServiceInfo.WsdlSettings.GetParameterName(operationName);
                requestInfo.SoapAction = this._webServiceInfo.WsdlSettings.GetSoapAction(operationName);
                this.SendRequest(requestInfo);
            }
            catch (Exception ex)
            {
                _log.Error("Error while sending request", ex);
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The setup request info.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <returns>
        /// The <see cref="WsInfo"/>.
        /// </returns>
        private static WsInfo SetupRequestInfo(Uri endpoint)
        {
            var info = new WsInfo { Endpoint = endpoint.AbsoluteUri };

            WSDLSettings wsdlSettings;
            try
            {
                wsdlSettings = new WSDLSettings(info.Endpoint + "?wsdl");
            }
            catch (Exception ex)
            {
                _log.Error("Error while retrieving WSDL", ex);
                throw;
            }

            info.WsdlSettings = wsdlSettings;
            info.TargetNamespace = wsdlSettings.TargetNamespace;
            return info;
        }

        /// <summary>
        /// Read the response from the web service and stream it to a file. Streaming happens here.
        /// </summary>
        /// <param name="output">
        /// The output file
        /// </param>
        /// <param name="webRequest">
        /// The web request to read the response from
        /// </param>
        private static void StreamReadResponse(FileInfo output, WebRequest webRequest)
        {
            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var buffer = new byte[BufferSize];
                        using (FileStream fileStream = output.Create())
                        {
                            int len;
                            while ((len = stream.Read(buffer, 0, BufferSize)) > 0)
                            {
                                fileStream.Write(buffer, 0, len);
                            }

                            fileStream.Flush();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("No response from server");
                    }
                }
            }
        }

        /// <summary>
        /// Get the SDMX message element from the request
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The first message element or null
        /// </returns>
        private XmlNode GetFirstSdmxNode(RequestInfo request)
        {
            var ws = this._webServiceInfo;
            var xmlDoc = new XmlDocument();
            using (FileStream fileStream = request.Request.Open(FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fileStream);
            }

            var ns = ws.SdmxSchemaVersion.EnumType == SdmxSchemaEnumType.VersionTwoPointOne ? SdmxConstants.MessageNs21 : SdmxConstants.MessageNs20;
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("*", ns);

            XmlNode firstNode = nodes.Item(0);
            return firstNode;
        }

        /// <summary>
        /// Send request.
        /// </summary>
        /// <param name="requestInfo">
        /// The request Info.
        /// </param>
        private void SendRequest(RequestInfo requestInfo)
        {
            var info = this._webServiceInfo;
            var webRequest = (HttpWebRequest)WebRequest.Create(info.Endpoint);
            if (!string.IsNullOrEmpty(requestInfo.SoapAction))
            {
                webRequest.Headers.Add("SOAPAction", requestInfo.SoapAction);
            }

            webRequest.Credentials = new NetworkCredential(this._settings.userName, this._settings.password);

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            webRequest.Timeout = 400000;

            XmlNode firstNode = this.GetFirstSdmxNode(requestInfo);
            if (firstNode == null)
            {
                throw new InvalidOperationException("Could not find an SDMX message");
            }

            this.SendSoapRequest(requestInfo, webRequest, firstNode);

            StreamReadResponse(requestInfo.Response, webRequest);
        }

        /// <summary>
        /// Build a soap request and send it to the web server
        /// </summary>
        /// <param name="requestInfo">
        /// The request Info.
        /// </param>
        /// <param name="webRequest">
        /// The web request
        /// </param>
        /// <param name="firstNode">
        /// The first SDMX node of the request
        /// </param>
        private void SendSoapRequest(RequestInfo requestInfo, WebRequest webRequest, XmlNode firstNode)
        {
            var info = this._webServiceInfo;
            string query = string.IsNullOrEmpty(requestInfo.Parameter)
                               ? firstNode.OuterXml
                               : string.Format(CultureInfo.InvariantCulture, SoapRequestParameter, WsPrefix, requestInfo.Parameter, firstNode.OuterXml);

            string soapMessage = string.Format(CultureInfo.InvariantCulture, SoapRequest, WsPrefix, info.TargetNamespace, requestInfo.Operation, query);
           
            _log.DebugFormat("Requested WS: {0}", webRequest.RequestUri);
            _log.DebugFormat("Request Headers:\n{0}", webRequest.Headers);
            _log.DebugFormat("Request SOAP Message:\n{0}", soapMessage);
            using (Stream stream = webRequest.GetRequestStream())
            {
                var buffer = new byte[BufferSize];
                using (Stream fileStream = new MemoryStream(new UTF8Encoding().GetBytes(soapMessage)))
                {
                    int len;
                    while ((len = fileStream.Read(buffer, 0, BufferSize)) > 0)
                    {
                        stream.Write(buffer, 0, len);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// The request info.
        /// </summary>
        public class RequestInfo
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets a value indicating whether [compress].
            /// </summary>
            /// <value>
            ///     <c>true</c> if [compress]; otherwise, <c>false</c>.
            /// </value>
            public bool Compress { get; set; }

            /// <summary>
            ///     Gets or sets the operation
            /// </summary>
            public SoapOperation Operation { get; set; }

            /// <summary>
            ///     Gets or sets the extra parameter (.net-ism)
            /// </summary>
            public string Parameter { get; set; }

            /// <summary>
            ///     Gets or sets the SOAP Request.
            /// </summary>
            public FileInfo Request { get; set; }

            /// <summary>
            ///     Gets or sets the SOAP response.
            /// </summary>
            public FileInfo Response { get; set; }

            /// <summary>
            ///     Gets or sets the soap action
            /// </summary>
            public string SoapAction { get; set; }

            #endregion
        }

        /// <summary>
        ///     The WS info.
        /// </summary>
        public sealed class WsInfo
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the endpoint url
            /// </summary>
            public string Endpoint { get; set; }

            /// <summary>
            ///     Gets or sets the SDMX schema version.
            /// </summary>
            /// <value>
            ///     The SDMX schema version.
            /// </value>
            public SdmxSchema SdmxSchemaVersion { get; set; }

            /// <summary>
            ///     Gets or sets the target namespace
            /// </summary>
            public string TargetNamespace { get; set; }

            /// <summary>
            /// Gets or sets the WSDL settings.
            /// </summary>
            public WSDLSettings WsdlSettings { get; set; }

            #endregion
        }
    }
}
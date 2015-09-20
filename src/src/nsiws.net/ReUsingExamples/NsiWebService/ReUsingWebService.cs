// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingWebService.cs" company="Eurostat">
//   Date Created : 2012-03-06
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test stream SDMX v2.0 WS client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ReUsingExamples.NsiWebService
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Test stream SDMX v2.0 WS client.
    /// </summary>
    public static class ReUsingWebService
    {
        #region Constants and Fields

        /// <summary>
        ///   The buffer size.
        /// </summary>
        private const int BufferSize = 4096;

        /// <summary>
        ///   The SDMX v2.0 Message XML namespace
        /// </summary>
        private const string SdmxV2MessageNamespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message";

        /// <summary>
        ///   This field holds a template for soap 1.1 request envelope
        /// </summary>
        private const string SoapRequest =
            "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:{0}=\"{1}\"><soap:Header/><soap:Body><{0}:{2}>{3}</{0}:{2}></soap:Body></soap:Envelope>";

        /// <summary>
        ///   This field holds a template for soap 1.1 request envelope
        /// </summary>
        private const string SoapRequestParameter = "<{0}:{1}>{2}</{0}:{1}>";

        /// <summary>
        ///   The prefix for the web service namespace it can be anything
        /// </summary>
        private const string WsPrefix = "web";

        #endregion

        #region Methods

        /// <summary>
        /// Get the SDMX message element from the request
        /// </summary>
        /// <param name="request">
        /// The FileInfo object that points to the request file 
        /// </param>
        /// <returns>
        /// The first message element or null 
        /// </returns>
        private static XmlNode GetFirstSdmxNode(FileInfo request)
        {
            var xmlDoc = new XmlDocument();
            using (FileStream fileStream = request.Open(FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fileStream);
            }

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("*", SdmxV2MessageNamespace);

            XmlNode firstNode = nodes.Item(0);
            return firstNode;
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args. 
        /// </param>
        public static void Main(string[] args)
        {
            var info = new RequestInfo
                {
                    Endpoint = "http://localhost/nsiws/NSIStdV20Service.asmx", 
                    Request = new FileInfo(@"query.xml"), 
                    Response = new FileInfo(@"response.xml"), 
                    Operation = "GetCompactData"
                };

            WSDLSettings wsdlSettings;
            try
            {
                wsdlSettings = new WSDLSettings(info.Endpoint + "?wsdl");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error while retrieving WSDL");
                Console.Error.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }

            info.Parameter = wsdlSettings.GetParameterName(info.Operation);
            info.SoapAction = wsdlSettings.GetSoapAction(info.Operation);
            info.TargetNamespace = wsdlSettings.TargetNamespace;

            try
            {
                SendRequest(info);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error while sending request");
                Console.Error.WriteLine(ex.ToString());
                Console.ReadLine();
                throw;
            }

            Console.WriteLine("Success");
        }

        /// <summary>
        /// Send request.
        /// </summary>
        /// <param name="info">
        /// The response info. 
        /// </param>
        private static void SendRequest(RequestInfo info)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(info.Endpoint);
            if (!string.IsNullOrEmpty(info.SoapAction))
            {
                webRequest.Headers.Add("SOAPAction", info.SoapAction);
            }

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            webRequest.Timeout = 400000;

            XmlNode firstNode = GetFirstSdmxNode(info.Request);
            if (firstNode == null)
            {
                throw new InvalidOperationException("Could not find an SDMX message");
            }

            SendSoapRequest(info, webRequest, firstNode);

            StreamReadResponse(info.Response, webRequest);
        }

        /// <summary>
        /// Build a soap request and send it to the web server
        /// </summary>
        /// <param name="info">
        /// The model containing the web service information 
        /// </param>
        /// <param name="webRequest">
        /// The web request 
        /// </param>
        /// <param name="firstNode">
        /// The first SDMX node of the request 
        /// </param>
        private static void SendSoapRequest(RequestInfo info, WebRequest webRequest, XmlNode firstNode)
        {
            string query = string.IsNullOrEmpty(info.Parameter)
                               ? firstNode.OuterXml
                               : string.Format(
                                   CultureInfo.InvariantCulture, 
                                   SoapRequestParameter, 
                                   WsPrefix, 
                                   info.Parameter, 
                                   firstNode.OuterXml);

            string soapMessage = string.Format(
                CultureInfo.InvariantCulture, SoapRequest, WsPrefix, info.TargetNamespace, info.Operation, query);

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

        #endregion

        /// <summary>
        /// The response info.
        /// </summary>
        private sealed class RequestInfo
        {
            #region Public Properties

            /// <summary>
            ///   Gets or sets the endpoint url
            /// </summary>
            public string Endpoint { get; set; }

            /// <summary>
            ///   Gets or sets the operation
            /// </summary>
            public string Operation { get; set; }

            /// <summary>
            ///   Gets or sets the extra parameter (.net-ism)
            /// </summary>
            public string Parameter { get; set; }

            /// <summary>
            ///   Gets or sets the SOAP Request.
            /// </summary>
            public FileInfo Request { get; set; }

            /// <summary>
            ///   Gets or sets the SOAP response.
            /// </summary>
            public FileInfo Response { get; set; }

            /// <summary>
            ///   Gets or sets the soap action
            /// </summary>
            public string SoapAction { get; set; }

            /// <summary>
            ///   Gets or sets the target namespace
            /// </summary>
            public string TargetNamespace { get; set; }

            #endregion
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxServiceTestInvoker.cs" company="Eurostat">
//   Date Created : 2013-10-23
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sdmx service test invoker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.NsiWebService
{
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Xml;

    using TestWsSoap;

    /// <summary>
    /// The sdmx service test invoker.
    /// </summary>
    public class SdmxServiceTestInvoker
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxServiceTestInvoker));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Invokes the service.
        /// </summary>
        /// <param name="wsClient">
        /// The ws client.
        /// </param>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <param name="compress">
        /// if set to <c>true</c> [compress].
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InvokeService(SdmxWsClient wsClient, SoapOperation operation, FileInfo request, FileInfo response, SdmxSchemaEnumType schemaVersion, bool compress = false)
        {
            var sdmxMessage = new FileInfo(Path.Combine(response.DirectoryName ?? string.Empty, response.Name + ".sdmx.xml"));
            try
            {
                wsClient.InvokeService(operation, request, response, compress);
                using (var reader = XmlReader.Create(response.FullName, new XmlReaderSettings() {IgnoreWhitespace = true, IgnoreComments = true, IgnoreProcessingInstructions = true}))
                using (var writer = XmlWriter.Create(sdmxMessage.FullName, new XmlWriterSettings() {Encoding = Encoding.UTF8, Indent = true }))
                {
                    SdmxMessageUtil.FindSdmx(reader);
                    writer.WriteNode(reader, true);

                    writer.Flush();
                }
            }
            catch (WebException e)
            {
                _log.Error(e.Message, e);
                var webResponse = e.Response;
                if (webResponse != null)
                {
                    var tempFileName = Path.GetTempFileName();
                    _log.ErrorFormat("Logging error at {0}", tempFileName);
                    WriteResponse(webResponse, tempFileName);
                    var errorCode = GetErrorCode(tempFileName);
                    _log.ErrorFormat("Error code : {0} ", errorCode);
                    _log.Error(File.ReadAllText(tempFileName));
                    File.Delete(tempFileName);
                }

                throw;
            }

            using (IReadableDataLocation location = new FileReadableDataLocation(sdmxMessage))
            {
                var actualVersion = SdmxMessageUtil.GetSchemaVersion(location);
                if (actualVersion != schemaVersion)
                {
                    return false;
                }

                var actualMessageType = SdmxMessageUtil.GetMessageType(location);
                var expectedMessageType = operation.GetMessageType();
                if (actualMessageType != expectedMessageType)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Invokes the service error assert.
        /// </summary>
        /// <param name="wsClient">
        /// The ws client.
        /// </param>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="expectedError">
        /// The expected error.
        /// </param>
        /// <param name="compress">
        /// if set to <c>true</c> [compress].
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InvokeServiceErrorAssert(SdmxWsClient wsClient, SoapOperation operation, FileInfo request, SdmxErrorCodeEnumType expectedError, bool compress = false)
        {
            var response = new FileInfo(Path.GetTempFileName());
            try
            {
                wsClient.InvokeService(operation, request, response, compress);
            }
            catch (WebException e)
            {
                if (CheckErrorResponse(SdmxErrorCode.GetFromEnum(expectedError).ClientErrorCode, e))
                {
                    return true;
                }
            }

            response.Refresh();
            response.Delete();

            return false;
        }

        /// <summary>
        /// Invokes the service error assert.
        /// </summary>
        /// <param name="wsClient">
        /// The WS client.
        /// </param>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="expectedError">
        /// The expected error.
        /// </param>
        /// <param name="compress">
        /// if set to <c>true</c> [compress].
        /// </param>
        /// <returns>
        /// True if the returned error matches the <paramref name="expectedError"/> ; otherwise false.
        /// </returns>
        public bool InvokeServiceErrorAssert(SdmxWsClient wsClient, SoapOperation operation, FileInfo request, int expectedError, bool compress = false)
        {
            var response = new FileInfo(Path.GetTempFileName());
            try
            {
                wsClient.InvokeService(operation, request, response, compress);
            }
            catch (WebException e)
            {
                if (CheckErrorResponse(expectedError, e))
                {
                    return true;
                }
            }

            response.Refresh();
            response.Delete();

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the error response.
        /// </summary>
        /// <param name="expectedError">
        /// The expected error.
        /// </param>
        /// <param name="e">
        /// The decimal.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool CheckErrorResponse(int expectedError, WebException e)
        {
            var webResponse = e.Response;
            string errorCode = null;
            if (webResponse != null)
            {
                var tempFileName = Path.GetTempFileName();
                WriteResponse(webResponse, tempFileName);
                errorCode = GetErrorCode(tempFileName);
                File.Delete(tempFileName);
            }

            int code;
            if (errorCode != null && int.TryParse(errorCode, NumberStyles.Integer, CultureInfo.InvariantCulture, out code))
            {
                if (code != expectedError)
                {
                    _log.ErrorFormat("Expected code : '{0}' got '{1}'", expectedError, code);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <param name="tempFileName">
        /// Name of the temporary file.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetErrorCode(string tempFileName)
        {
            string errorCode = null;
            using (IReadableDataLocation location = new FileReadableDataLocation(tempFileName))
            {
                if (XmlUtil.IsXML(location))
                {
                    using (XmlReader reader = XmlReader.Create(location.InputStream))
                    {
                        string localName = null;
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    localName = reader.LocalName;
                                    break;
                                case XmlNodeType.EndElement:
                                    localName = null;
                                    break;
                                case XmlNodeType.Text:
                                    if ("ErrorNumber".Equals(localName))
                                    {
                                        return reader.Value;
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            return errorCode;
        }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="webResponse">
        /// The web response.
        /// </param>
        /// <param name="tempFileName">
        /// Name of the temporary file.
        /// </param>
        private static void WriteResponse(WebResponse webResponse, string tempFileName)
        {
            using (var stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var output = new FileStream(tempFileName, FileMode.Create))
                    {
                        int len;
                        var buffer = new byte[32768];
                        while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, len);
                        }

                        output.Flush();
                    }
                }
            }
        }

        #endregion
    }
}
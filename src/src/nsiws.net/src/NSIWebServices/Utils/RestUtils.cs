// -----------------------------------------------------------------------
// <copyright file="RestUtils.cs" company="Eurostat">
//   Date Created : 2014-05-30
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Controller;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// REST related utils
    /// </summary>
    public class RestUtils
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StructureResource));

        /// <summary>
        /// The _builder.
        /// </summary>
        private static readonly WebFaultExceptionRestBuilder _builder;

        /// <summary>
        /// Initializes static members of the <see cref="RestUtils"/> class.
        /// </summary>
        static RestUtils() 
        {
             _builder = new WebFaultExceptionRestBuilder();
        }

        /// <summary>
        /// Gets the char set encoding.
        /// </summary>
        /// <param name="acceptValue">The accept value.</param>
        /// <returns>The response encoding</returns>
        public static Encoding GetCharSetEncoding(ContentType acceptValue)
        {
            if (!string.IsNullOrWhiteSpace(acceptValue.CharSet))
            {
                try
                {
                    var encoding = Encoding.GetEncoding(acceptValue.CharSet);
                    if (encoding.Equals(Encoding.UTF8))
                    {
                        return new UTF8Encoding(false);
                    }

                    return encoding;
                }
                catch (Exception e)
                {
                    _logger.Error(acceptValue.CharSet, e);
                }
            }

            return new UTF8Encoding(false);
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="acceptValue">The accept value.</param>
        /// <param name="resolvedContentType">Type of the resolved content.</param>
        /// <returns>The response content type</returns>
        public static string GetContentType(ContentType acceptValue, ContentType resolvedContentType)
        {
            switch (acceptValue.MediaType)
            {
                case SdmxMedia.ApplicationXml:
                case SdmxMedia.TextXml:
                    return acceptValue.ToString();
                case "text/*":
                    return SdmxMedia.TextXml;
                default:
                    return resolvedContentType.ToString();
            }
        }

        /// <summary>
        /// Streams the structural metadata.
        /// </summary>
        /// <param name="schemaVersion">The schema version.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="streamController">The stream controller.</param>
        /// <param name="encoding">The response encoding.</param>
        public static void StreamXml(SdmxSchema schemaVersion, Stream stream, IStreamController<XmlWriter> streamController, Encoding encoding)
        {
            try
            {
                if (schemaVersion.IsXmlFormat())
                {
                    using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = encoding }))
                    {
                        var actions = new Queue<Action>();
                        streamController.StreamTo(writer, actions);
                        writer.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);

                // HACK. 
                // The following is needed because for some reason WebOperationContext.CreateStreamResponse closes the connection and does not return a 
                // status code and/or error message to the user.
                var normalizedException = _builder.Build(e);

                //// Setting the WebOperationContext does not seem to work.... 
                //// WebOperationContext.Current.OutgoingResponse.StatusCode = normalizedException.StatusCode; 
                
                HttpContext.Current.Response.StatusCode = (int)normalizedException.StatusCode;
                HttpContext.Current.Response.ContentType = "text/html;";
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine("{0} - {1}", normalizedException.Message, normalizedException.Detail);
                    streamWriter.Flush();
                }
            }
        }


        /// <summary>
        /// Streams the DSPL structural metadata.
        /// </summary>
        /// <param name="schemaVersion">The schema version.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="streamController">The stream controller.</param>
        /// <param name="encoding">The response encoding.</param>
        /// 
        public static void StreamDspl(Stream stream, IStreamController<DsplDataFormat.Engine.DsplTextWriter> streamController, Encoding encoding)
        {
            try
            {
                using (var writer = new DsplDataFormat.Engine.DsplTextWriter(new StreamWriter(stream, encoding)))
                {
                    streamController.StreamTo(writer, new Queue<Action>());
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
        }


        /// <summary>
        /// Streams the CSV  .
        /// </summary>
        /// <param name="schemaVersion">The schema version.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="streamController">The stream controller.</param>
        /// <param name="encoding">The response encoding.</param>
        /// 


        public static void StreamCSV(Stream stream, IStreamController<CSVZip.Engine.CsvZipTextWriter> streamController, Encoding encoding)
        {
            try
            {
                using (var writer = new CSVZip.Engine.CsvZipTextWriter(new StreamWriter(stream, encoding)))
                {
                    streamController.StreamTo(writer, new Queue<Action>());
                    //writer.Flush();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
        }

        #region SDMXJSON
        /// <summary>
        /// Streams the structural metadata.
        /// </summary>
        /// <param name="schemaVersion">The schema version.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="streamController">The stream controller.</param>
        /// <param name="encoding">The response encoding.</param>
        public static void StreamJson(Stream stream,
                                      IStreamController<Newtonsoft.Json.JsonTextWriter> streamController,
                                      Encoding encoding)
        {
            try
            {
                using (var writer = new Newtonsoft.Json.JsonTextWriter(new StreamWriter(stream, encoding)))
                {
                    streamController.StreamTo(writer, new Queue<Action>());
                    writer.Flush();

                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
        }





        public static void StreamStructureJson(SdmxSchema schemaVersion, Stream stream, Encoding encoding, StringBuilder json)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
                using (var streamWriter = new StreamWriter(stream))
                    {
                        //string json = "{\"x\":\"true\"}";
                        streamWriter.Write(json.ToString());
                        streamWriter.Flush();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
        }


        public static void StreamJsonZip(Stream stream, IStreamController<Newtonsoft.Json.JsonTextWriter> streamController, Encoding encoding)
        {
            try
            {
                Stream zipStream = new System.IO.Compression.GZipStream(stream,
                                                System.IO.Compression.CompressionMode.Compress);

                using (var writer = new Newtonsoft.Json.JsonTextWriter(new StreamWriter(zipStream, encoding)))
                {
                    streamController.StreamTo(writer, new Queue<Action>());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
        }

        #endregion









        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="ctx">
        /// The current <see cref="WebOperationContext"/>.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="defaultMediaType">
        /// Default type of the media.
        /// </param>
        /// <exception cref="WebFaultException{String}">
        /// Cannot serve content type
        /// </exception>
        /// <returns>
        /// The <see cref="ContentType"/>.
        /// </returns>
        public static ContentType GetContentType(WebOperationContext ctx, Func<ContentType, bool> predicate, ContentType defaultMediaType)
        {
            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();
            var contentType = acceptHeaderElements.FirstOrDefault(predicate);

            if (contentType == null)
            {
                if (acceptHeaderElements.Count == 0)
                {
                    contentType = defaultMediaType;
                }
                else
                {
                    string accept = ctx.IncomingRequest.Accept;
                    _logger.Error("Cannot serve content type: " + accept);
                    throw new WebFaultException<string>("Cannot serve content type: " + accept, HttpStatusCode.NotAcceptable);
                }
            }

            _logger.Debug(string.Format(CultureInfo.InvariantCulture, "Select media type: {0}", contentType.MediaType));

            return contentType;
        }

        /// <summary>
        /// The get version from media type.
        /// </summary>
        /// <param name="mediaType">
        /// The media type.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxSchema"/>.
        /// </returns>
        public static SdmxSchema GetVersionFromMediaType(ContentType mediaType)
        {
            if (mediaType.Parameters == null)
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);
            }

            if (!mediaType.Parameters.ContainsKey("version"))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);
            }

            if (mediaType.Parameters["version"].Equals("2.1"))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);
            }

            if (mediaType.Parameters["version"].Equals("2.0"))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);
            }

            return null;
        }
    }
}
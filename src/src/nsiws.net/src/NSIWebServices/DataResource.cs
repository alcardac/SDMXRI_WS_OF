// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataResource.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data resource.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Web;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Manager;
    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Rest.Utils;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;

    /// <summary>
    /// The data resource.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DataResource : IDataResource
    {
        #region Static Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataResource));

        #endregion

        #region Fields

        /// <summary>
        ///     The _controller builder
        /// </summary>
        private readonly ControllerBuilder _controllerBuilder;
        private readonly RDFProvider.Controller.Builder.RDFControllerBuilder _RDFControllerBuilder;
        private readonly DsplDataFormat.Controller.Builder.DsplControllerBuilder _DsplControllerBuilder;
        private readonly CSVZip.Controller.Builder.CsvZipControllerBuilder _CsvZipControllerBuilder;
        private readonly IstatExtension.Controllers.Builder.ControllerBuilder _JsonControllerBuilder;

        /// <summary>
        ///     The _fault exception rest builder
        /// </summary>
        private readonly WebFaultExceptionRestBuilder _faultExceptionRestBuilder = new WebFaultExceptionRestBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataResource" /> class.
        /// </summary>
        public DataResource()
        {
            this._controllerBuilder = new ControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._RDFControllerBuilder = new RDFProvider.Controller.Builder.RDFControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._DsplControllerBuilder = new DsplDataFormat.Controller.Builder.DsplControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._CsvZipControllerBuilder = new CSVZip.Controller.Builder.CsvZipControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._JsonControllerBuilder = new IstatExtension.Controllers.Builder.ControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get generic data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException">
        /// </exception>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        public Message GetGenericData(string flowRef, string key, string providerRef)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                

                if (string.IsNullOrEmpty(flowRef))
                {
                    throw new WebFaultException(HttpStatusCode.NotImplemented); // $$$ Strange 501 ?
                }

                return this.ProcessRequest(flowRef, key, providerRef, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetGenericData : {0}/{1}/{2}", flowRef, key, providerRef);
                Logger.Error("GetGenericData", e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
        }

        /// <summary>
        /// The get generic data all keys.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message GetGenericDataAllKeys(string flowRef)
        {
            return this.GetGenericData(flowRef, "ALL", "ALL");
        }

        /// <summary>
        /// The get generic data all providers.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message GetGenericDataAllProviders(string flowRef, string key)
        {
            return this.GetGenericData(flowRef, key, "ALL");
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build query bean.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <param name="queryParameters">
        /// The query parameters.
        /// </param>
        /// <returns>
        /// The <see cref="IRestDataQuery"/>.
        /// </returns>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        private IRestDataQuery BuildQueryBean(string flowRef, string key, string providerRef, NameValueCollection queryParameters)
        {
            var queryString = new string[4];
            queryString[0] = "data";
            queryString[1] = flowRef;
            queryString[2] = key;
            queryString[3] = providerRef;

            IDictionary<string, string> paramsDict = HeaderUtils.GetQueryStringAsDict(queryParameters);
            IRestDataQuery restQuery;

            try
            {
                restQuery = new RESTDataQueryCore(queryString, paramsDict);
            }
            catch (SdmxException e)
            {
                Logger.Error(e.Message, e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
                throw new WebFaultException<string>(e.Message, HttpStatusCode.BadRequest);
            }

            return restQuery;
        }

        /// <summary>
        /// The get version from accept.
        /// </summary>
        /// <param name="acceptHeaderList">
        /// The accept header list.
        /// </param>
        /// <param name="mediatype">
        /// The mediatype.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetVersionFromAccept(IEnumerable<ContentType> acceptHeaderList, string mediatype)
        {
            string version = null;

            var accept = acceptHeaderList.Where(h => h.MediaType.Contains(mediatype)).FirstOrDefault();
            if (accept != null)
            {
                if (accept.Parameters != null && accept.Parameters.ContainsKey("version"))
                {
                    version = accept.Parameters["version"];
                }
            }

            return version;
        }

        /// <summary>
        /// The get version from media type.
        /// </summary>
        /// <param name="selectedMediaTypeWithVersion">
        /// The selected media type with version.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxSchema"/>.
        /// </returns>
        private static SdmxSchema GetVersionFromMediaType(ContentType selectedMediaTypeWithVersion, BaseDataFormat format)
        {
            SdmxSchema version = null;

            switch (format.EnumType)
            {
                case BaseDataFormatEnumType.Edi:
                    version = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.Edi);
                    break;
                case BaseDataFormatEnumType.Csv:
                    version = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.Csv);
                    break;
                default:
                    version = RestUtils.GetVersionFromMediaType(selectedMediaTypeWithVersion);
                    break;
            }

            return version;
        }

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <param name="ctx">
        /// The ctx.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        private Message ProcessRequest(string flowRef, string key, string providerRef, WebOperationContext ctx)
        {
            var requestAccept = ctx.IncomingRequest.Accept;

            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();

            var defaultMediaType = DataMediaType.GetFromEnum(DataMediaEnumType.GenericData).MediaType;
            var contentType = RestUtils.GetContentType(ctx, h => DataMediaType.GetTypeFromName(h.MediaType) != null, defaultMediaType);

            string filename = DateTime.Now.ToString();

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var datamediaType = DataMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = datamediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            BaseDataFormat format = DataMediaType.GetTypeFromName(selectedMediaTypeWithVersion.MediaType).Format;
            SdmxSchema version = GetVersionFromMediaType(selectedMediaTypeWithVersion, format);

            Logger.Info("Selected representation info for the controller: format =" + format + " , smdx_schema=" + version);
            IRestDataQuery query = this.BuildQueryBean(flowRef, key, providerRef, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            HttpContext context = HttpContext.Current;

            if (datamediaType.EnumType == DataMediaEnumType.DsplData)
            {
                var controller = this._DsplControllerBuilder.BuildDsplDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(stream => RestUtils.StreamDspl(stream, streamController, charSetEncoding), responseContentType);
            }
            else if (datamediaType.EnumType == DataMediaEnumType.CsvZipData)
            {
                var controller = this._CsvZipControllerBuilder.BuildCsvZipDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(stream => RestUtils.StreamCSV(stream, streamController, charSetEncoding), responseContentType);
            }
            else if (datamediaType.EnumType == DataMediaEnumType.CsvData)
            {
                var controller = this._CsvZipControllerBuilder.BuildCsvZipDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(stream => RestUtils.StreamCSV(stream, streamController, charSetEncoding), responseContentType);
            }
            else if (datamediaType.EnumType == DataMediaEnumType.RdfData)
            {
                var controller = this._RDFControllerBuilder.BuildDataRest(context.User as DataflowPrincipal);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                //var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                var responseContentType = "text/xml";
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(stream => RestUtils.StreamXml(SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne), stream, streamController, charSetEncoding), responseContentType);
            }
            else if (datamediaType.EnumType == DataMediaEnumType.JsonZipData)
            {
                var controller = this._JsonControllerBuilder.BuildJsonDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                //var responseContentType = "application/x-gzip-compressed";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".zip");
                return ctx.CreateStreamResponse(stream => RestUtils.StreamJsonZip(stream, streamController, charSetEncoding), responseContentType);
            }
            else if (datamediaType.EnumType == DataMediaEnumType.JsonData)
            {
                var controller = this._JsonControllerBuilder.BuildJsonDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                //var responseContentType = "application/octet-stream";
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".txt");
                return ctx.CreateStreamResponse(stream => RestUtils.StreamJson(stream, streamController, charSetEncoding), responseContentType);
            }
            else
            {
                var controller = this._controllerBuilder.BuildDataRest(context.User as DataflowPrincipal, format, version);
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(stream => RestUtils.StreamXml(version, stream, streamController, charSetEncoding), responseContentType);
            }
        }


        #region SDMXJSON
        /// <summary>
        /// The get generic data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException">
        /// </exception>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        public Message GetJsonData(string flowRef, string key, string providerRef)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (string.IsNullOrEmpty(flowRef))
                {
                    throw new WebFaultException(HttpStatusCode.NotImplemented); // $$$ Strange 501 ?
                }

                return this.ProcessJsonRequest(flowRef, key, providerRef, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetJsonData : {0}/{1}/{2}", flowRef, key, providerRef);
                Logger.Error("GetJsonData", e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
        }


        public Message GetJsonZipData(string flowRef, string key, string providerRef)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (string.IsNullOrEmpty(flowRef))
                {
                    throw new WebFaultException(HttpStatusCode.NotImplemented); // $$$ Strange 501 ?
                }

                return this.ProcessJsonZipRequest(flowRef, key, providerRef, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetJsonData : {0}/{1}/{2}", flowRef, key, providerRef);
                Logger.Error("GetJsonData", e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
        }
        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <param name="ctx">
        /// The ctx.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        private Message ProcessJsonRequest(string flowRef, string key, string providerRef, WebOperationContext ctx)
        {
            var requestAccept = ctx.IncomingRequest.Accept;

            string filename = DateTime.Now.ToString();

            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();

            var defaultMediaType = DataMediaType.GetFromEnum(DataMediaEnumType.JsonData).MediaType;
            var contentType = RestUtils.GetContentType(ctx, h => DataMediaType.GetTypeFromName(h.MediaType) != null, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var datamediaType = DataMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = datamediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            BaseDataFormat format = DataMediaType.GetTypeFromName(selectedMediaTypeWithVersion.MediaType).Format;
            SdmxSchema version = GetVersionFromMediaType(selectedMediaTypeWithVersion, format);

            Logger.Info("Selected representation info for the controller: format =" + format + " , smdx_schema=" + version);
            IRestDataQuery query = this.BuildQueryBean(flowRef, key, providerRef, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            HttpContext context = HttpContext.Current;
            var controller = this._JsonControllerBuilder.BuildJsonDataRest(context.User as DataflowPrincipal, format, version);
            var streamController = controller.ParseRequest(query);

            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            var responseContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".txt");
            return ctx.CreateStreamResponse(stream => RestUtils.StreamJson(stream, streamController, charSetEncoding), responseContentType);
        }

        private Message ProcessJsonZipRequest(string flowRef, string key, string providerRef, WebOperationContext ctx)
        {
            var requestAccept = ctx.IncomingRequest.Accept;

            string filename = DateTime.Now.ToString();

            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();

            var defaultMediaType = DataMediaType.GetFromEnum(DataMediaEnumType.JsonData).MediaType;
            var contentType = RestUtils.GetContentType(ctx, h => DataMediaType.GetTypeFromName(h.MediaType) != null, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var datamediaType = DataMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = datamediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            BaseDataFormat format = DataMediaType.GetTypeFromName(selectedMediaTypeWithVersion.MediaType).Format;
            SdmxSchema version = GetVersionFromMediaType(selectedMediaTypeWithVersion, format);

            Logger.Info("Selected representation info for the controller: format =" + format + " , smdx_schema=" + version);
            IRestDataQuery query = this.BuildQueryBean(flowRef, key, providerRef, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            HttpContext context = HttpContext.Current;
            var controller = this._JsonControllerBuilder.BuildJsonDataRest(context.User as DataflowPrincipal, format, version);
            var streamController = controller.ParseRequest(query);

            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            var responseContentType = "application/x-gzip-compressed";
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".zip");
            return ctx.CreateStreamResponse(stream => RestUtils.StreamJsonZip(stream, streamController, charSetEncoding), responseContentType);
        }
        #endregion

        #region DSPL
        /// <summary>
        /// The get DSPL data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException">
        /// </exception>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        public Message GetDsplData(string flowRef, string key, string providerRef)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (string.IsNullOrEmpty(flowRef))
                {
                    throw new WebFaultException(HttpStatusCode.NotImplemented);
                }

                return this.ProcessDsplRequest(flowRef, key, providerRef, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetDsplData : {0}/{1}/{2}", flowRef, key, providerRef);
                Logger.Error("GetDsplData", e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
        }

        private Message ProcessDsplRequest(string flowRef, string key, string providerRef, WebOperationContext ctx)
        {
            var requestAccept = ctx.IncomingRequest.Accept;

            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();

            var defaultMediaType = DataMediaType.GetFromEnum(DataMediaEnumType.CsvData).MediaType;
            var contentType = RestUtils.GetContentType(ctx, h => DataMediaType.GetTypeFromName(h.MediaType) != null, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var datamediaType = DataMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = datamediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            BaseDataFormat format = DataMediaType.GetTypeFromName(selectedMediaTypeWithVersion.MediaType).Format;
            SdmxSchema version = GetVersionFromMediaType(selectedMediaTypeWithVersion, format);

            Logger.Info("Selected representation info for the controller: format =" + format + " , smdx_schema=" + version);
            IRestDataQuery query = this.BuildQueryBean(flowRef, key, providerRef, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            HttpContext context = HttpContext.Current;
            DsplDataFormat.Controller.Builder.DsplControllerBuilder _dsplControllerBuilder = new DsplDataFormat.Controller.Builder.DsplControllerBuilder();
            var controller = _dsplControllerBuilder.BuildDsplDataRest(context.User as DataflowPrincipal, format, version);
            var streamController = controller.ParseRequest(query);

            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            var responseContentType = "application/dspl";

            return ctx.CreateStreamResponse(stream => RestUtils.StreamDspl(stream, streamController, charSetEncoding), responseContentType);
        }

        #endregion

        #region CsvZip
        /// <summary>
        /// The get CSVZip data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException">
        /// </exception>
        /// <exception cref="WebFaultException{T}">
        /// </exception>
        /// 

        public Message GetCSVData(string flowRef, string key, string providerRef)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (string.IsNullOrEmpty(flowRef))
                {
                    throw new WebFaultException(HttpStatusCode.NotImplemented); // $$$ Strange 501 ?
                }

                return this.ProcessCSVRequest(flowRef, key, providerRef, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetCSVData : {0}/{1}/{2}", flowRef, key, providerRef);
                Logger.Error("GetCSVData", e);
                throw this._faultExceptionRestBuilder.Build(e);
            }
        }

        private Message ProcessCSVRequest(string flowRef, string key, string providerRef, WebOperationContext ctx)
        {
            var requestAccept = ctx.IncomingRequest.Accept;

            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();

            var defaultMediaType = DataMediaType.GetFromEnum(DataMediaEnumType.CsvData).MediaType;
            var contentType = RestUtils.GetContentType(ctx, h => DataMediaType.GetTypeFromName(h.MediaType) != null, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var datamediaType = DataMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = datamediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            BaseDataFormat format = DataMediaType.GetTypeFromName(selectedMediaTypeWithVersion.MediaType).Format;
            SdmxSchema version = GetVersionFromMediaType(selectedMediaTypeWithVersion, format);

            Logger.Info("Selected representation info for the controller: format =" + format + " , smdx_schema=" + version);
            IRestDataQuery query = this.BuildQueryBean(flowRef, key, providerRef, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            HttpContext context = HttpContext.Current;
            CSVZip.Controller.Builder.CsvZipControllerBuilder _dsplControllerBuilder = new CSVZip.Controller.Builder.CsvZipControllerBuilder();
            var controller = _dsplControllerBuilder.BuildCsvZipDataRest(context.User as DataflowPrincipal, format, version);
            var streamController = controller.ParseRequest(query);


            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            var responseContentType = "application/csv";

            return ctx.CreateStreamResponse(stream => RestUtils.StreamCSV(stream, streamController, charSetEncoding), responseContentType);
        }
        #endregion        

        #endregion

    }
}
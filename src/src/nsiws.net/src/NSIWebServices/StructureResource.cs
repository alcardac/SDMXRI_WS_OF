// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureResource.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Xml;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.Ws.Controllers.Builder;
    using Estat.Sri.Ws.Controllers.Controller;
    using Estat.Sri.Ws.Controllers.Manager;
    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Rest.Utils;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;

    /// <summary>
    ///  The SDMX-ML Structural meta-data resource implementation 
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StructureResource : IStructureResource
    {
        #region Static Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StructureResource));

        /// <summary>
        /// The _builder.
        /// </summary>
        private static readonly WebFaultExceptionRestBuilder _builder;
        private readonly IstatExtension.Controllers.Builder.ControllerBuilder _JsonControllerBuilder;
        private readonly RDFProvider.Controller.Builder.RDFControllerBuilder _RDFcontrollerBuilder;

        #endregion

        #region Fields

        /// <summary>
        /// The _controller builder.
        /// </summary>
        private readonly ControllerBuilder _controllerBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="StructureResource"/> class.
        /// </summary>
        static StructureResource()
        {
            _builder = new WebFaultExceptionRestBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureResource"/> class. 
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public StructureResource()
        {
            this._controllerBuilder = new ControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._JsonControllerBuilder = new IstatExtension.Controllers.Builder.ControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
            this._RDFcontrollerBuilder = new RDFProvider.Controller.Builder.RDFControllerBuilder(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get structure.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        /// <exception cref="WebFaultException{T}">
        /// Bad Request.
        /// </exception>
        public Message GetStructure(string structure, string agencyId, string resourceId, string version)
        {
            WebOperationContext ctx = WebOperationContext.Current;

            if (!IsStructureValid(structure))
            {
                throw new WebFaultException<string>("Invalid structure: " + structure, HttpStatusCode.BadRequest);
            }

            try
            {
                return this.ProcessRequest(structure, agencyId, resourceId, version, ctx);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw _builder.Build(e);
            }
        }

        
        public Message GetJsonStructure(string structure, string agencyId, string resourceId, string version)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (!IsStructureValid(structure))
                {
                    throw new WebFaultException<string>("Invalid structure: " + structure, HttpStatusCode.BadRequest);
                }

                return this.ProcessJsonRequest(structure, agencyId, resourceId, version, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetJsonData : {0}/{1}", structure, resourceId);
                Logger.Error("GetJsonData", e);
                throw _builder.Build(e);
            }
        }

        public Message GetRDFStructure(string structure, string agencyId, string resourceId, string version)
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;

                if (!IsStructureValid(structure))
                {
                    throw new WebFaultException<string>("Invalid structure: " + structure, HttpStatusCode.BadRequest);
                }

                return this.ProcessRDFRequest(structure, agencyId, resourceId, version, ctx);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("GetRDFSTRUCTURE : {0}/{1}", structure, resourceId);
                Logger.Error("GetJsonData", e);
                throw _builder.Build(e);
            }
        }
        
        /// <summary>
        /// The get structure all.
        /// </summary>
        /// <param name="structures">
        /// The structures.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message GetStructureAll(string structures)
        {
            return this.GetStructure(structures, "ALL", "ALL", "latest");
        }

        /// <summary>
        /// The get structure all ids latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyIds">
        /// The agency ids.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message GetStructureAllIdsLatest(string structure, string agencyIds)
        {
            return this.GetStructure(structure, agencyIds, "ALL", "latest");
        }

        /// <summary>
        /// The get structure latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message GetStructureLatest(string structure, string agencyId, string resourceId)
        {
            return this.GetStructure(structure, agencyId, resourceId, "latest");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the rest query bean.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="agencyId">The agency id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="version">The version.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>
        /// The <see cref="IRestStructureQuery" />.
        /// </returns>
        /// <exception cref="WebFaultException{String}">An exception is thrown</exception>
        private static IRestStructureQuery BuildRestQueryBean(string structure, string agencyId, string resourceId, string version, NameValueCollection queryParameters)
        {
            var queryString = new string[4];
            queryString[0] = structure;
            queryString[1] = agencyId;
            queryString[2] = resourceId;
            queryString[3] = version;

            var paramsDict = HeaderUtils.GetQueryStringAsDict(queryParameters);

            IRestStructureQuery query;

            try
            {
                query = new RESTStructureQueryCore(queryString, paramsDict);
            }
            catch (SdmxException e)
            {
                Logger.Error(e.Message, e);
                throw _builder.Build(e);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
                throw new WebFaultException<string>(e.Message, HttpStatusCode.BadRequest);
            }

            return query;
        }

        /// <summary>
        /// The is structure valid.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsStructureValid(string structure)
        {
            Match match = Regex.Match(
                structure, 
                @"^(datastructure|metadatastructure|categoryscheme|conceptscheme|codelist|hierarchicalcodelist|organisationscheme|agencyscheme|dataproviderscheme|dataconsumerscheme|organisationunitscheme|dataflow|metadataflow|reportingtaxonomy|provisionagreement|structureset|process|categorisation|contentconstraint|attachmentconstraint|structure)$", 
                RegexOptions.IgnoreCase);

            return match.Success;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="agencyId">The agency id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="version">The version.</param>
        /// <param name="ctx">The current <see cref="WebOperationContext"/>.</param>
        /// <returns>
        /// The <see cref="Message" />.
        /// </returns>
        /// <exception cref="System.Web.HttpBrowserCapabilitiesBase"></exception>
        /// <exception cref="WebFaultException{String}">
        /// Cannot serve content type
        /// </exception>
        /// <exception cref="WebFaultException{String}">Cannot serve content type</exception>
        private Message ProcessRequest(string structure, string agencyId, string resourceId, string version, WebOperationContext ctx)
        {
            Match match = Regex.Match(resourceId, @"[A-Za-z0-9\-]+$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            var defaultMediaType = StructureMediaType.GetFromEnum(StructureMediaEnumType.Structure).MediaTypeName;
            var requestAccept = ctx.IncomingRequest.Accept;
            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();
            Func<ContentType, bool> predicate = type => StructureMediaType.GetTypeFromName(type.MediaType) != null;
            var contentType = RestUtils.GetContentType(ctx, predicate, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var selectedStructureMediaType = StructureMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = selectedStructureMediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            SdmxSchema schemaVersion = RestUtils.GetVersionFromMediaType(selectedMediaTypeWithVersion);

            var context = HttpContext.Current;
            var controller = this._controllerBuilder.BuildQueryStructureRest(schemaVersion, context.User as DataflowPrincipal);

            Logger.Info("Selected representation info for the controller: format =" + "TODO" + " , smdx_schema=" + version);
            
            IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);

            if (selectedStructureMediaType.EnumType == StructureMediaEnumType.JsonStructure)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);
                String jsonres = "{\"x\":\"true\"}";
                StringWriter jsonres1 = new StringWriter();

                jsonres1 = this._JsonControllerBuilder.StructureResponse(structure, agencyId, resourceId, version, sw);

                return ctx.CreateStreamResponse(
                    stream => RestUtils.StreamStructureJson(schemaVersion, stream, charSetEncoding, sb), responseContentType);
            }
            else if (selectedStructureMediaType.EnumType == StructureMediaEnumType.Rdf)
            {
                var controllerRDF = this._RDFcontrollerBuilder.BuildQueryStructureRest(context.User as DataflowPrincipal);
                //IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
                var streamController = controllerRDF.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);

                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                responseContentType = "application/xml";
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(
                    stream => RestUtils.StreamXml(SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne), stream, streamController, charSetEncoding), responseContentType);
            }
            else
            {
                var streamController = controller.ParseRequest(query);
                var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);

                var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
                selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
                return ctx.CreateStreamResponse(
                    stream => RestUtils.StreamXml(schemaVersion, stream, streamController, charSetEncoding), responseContentType);

            }
        }


        /// <summary>
        /// Processes the JSON request.
        /// </summary>
        
        private Message ProcessJsonRequest(string structure, string agencyId, string resourceId, string version, WebOperationContext ctx)
        {

            Match match = Regex.Match(resourceId, @"[A-Za-z0-9\-]+$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            var defaultMediaType = StructureMediaType.GetFromEnum(StructureMediaEnumType.Structure).MediaTypeName;
            var requestAccept = ctx.IncomingRequest.Accept;
            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();
            Func<ContentType, bool> predicate = type => StructureMediaType.GetTypeFromName(type.MediaType) != null;
            var contentType = RestUtils.GetContentType(ctx, predicate, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var selectedStructureMediaType = StructureMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = selectedStructureMediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            SdmxSchema schemaVersion = RestUtils.GetVersionFromMediaType(selectedMediaTypeWithVersion);

            var context = HttpContext.Current;
           // var controller = this._controllerBuilder.BuildQueryStructureRest(schemaVersion, context.User as DataflowPrincipal);

          //  var controller = this._JsonControllerBuilder.BuildJsonQueryStructureRest(schemaVersion, context.User as DataflowPrincipal);

            Logger.Info("Selected representation info for the controller: format =" + "TODO" + " , smdx_schema=" + version);

           // IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            //var streamController = controller.ParseRequest(query);
            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);

            var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            String jsonres = "{\"x\":\"true\"}";
            StringWriter jsonres1 = new StringWriter();
            
            jsonres1 = this._JsonControllerBuilder.StructureResponse(structure, agencyId, resourceId, version, sw);


            /*
            return ctx.CreateStreamResponse(
                stream => RestUtils.StreamStructureJson(schemaVersion, stream, streamController, charSetEncoding), responseContentType);
            */
            return ctx.CreateStreamResponse(
                stream => RestUtils.StreamStructureJson(schemaVersion, stream, charSetEncoding, sb), responseContentType);
        }

        /// <summary>
        /// Processes the JSON request.
        /// </summary>

        private Message ProcessRDFRequest(string structure, string agencyId, string resourceId, string version, WebOperationContext ctx)
        {

            Match match = Regex.Match(resourceId, @"[A-Za-z0-9\-]+$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            var defaultMediaType = StructureMediaType.GetFromEnum(StructureMediaEnumType.Structure).MediaTypeName;
            var requestAccept = ctx.IncomingRequest.Accept;
            Logger.Info("Got request Content type= " + requestAccept);

            IList<ContentType> acceptHeaderElements = ctx.IncomingRequest.GetAcceptHeaderElements();
            Func<ContentType, bool> predicate = type => StructureMediaType.GetTypeFromName(type.MediaType) != null;
            var contentType = RestUtils.GetContentType(ctx, predicate, defaultMediaType);

            string requestedVersion = HeaderUtils.GetVersionFromAccept(acceptHeaderElements, contentType.MediaType);

            var selectedStructureMediaType = StructureMediaType.GetTypeFromName(contentType.MediaType);
            var selectedMediaTypeWithVersion = selectedStructureMediaType.GetMediaTypeVersion(requestedVersion);

            if (selectedMediaTypeWithVersion == null)
            {
                Logger.Error("Cannot serve content type: " + requestAccept);
                throw new WebFaultException<string>("Cannot serve content type: " + requestAccept, HttpStatusCode.NotAcceptable);
            }

            Logger.Debug("Select mediatype with version if required: " + selectedMediaTypeWithVersion);

            SdmxSchema schemaVersion = RestUtils.GetVersionFromMediaType(selectedMediaTypeWithVersion);

            var context = HttpContext.Current;
            // var controller = this._controllerBuilder.BuildQueryStructureRest(schemaVersion, context.User as DataflowPrincipal);

            //  var controller = this._JsonControllerBuilder.BuildJsonQueryStructureRest(schemaVersion, context.User as DataflowPrincipal);

            Logger.Info("Selected representation info for the controller: format =" + "TODO" + " , smdx_schema=" + version);

            // IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            //var streamController = controller.ParseRequest(query);
            var charSetEncoding = RestUtils.GetCharSetEncoding(contentType);

            var responseContentType = RestUtils.GetContentType(contentType, selectedMediaTypeWithVersion);
            responseContentType = "application/xml";
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            var controllerRDF = this._RDFcontrollerBuilder.BuildQueryStructureRest(context.User as DataflowPrincipal);
            //IRestStructureQuery query = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);
            var streamController = controllerRDF.ParseRequest(query);
           

            
            selectedMediaTypeWithVersion.CharSet = charSetEncoding.WebName;
            return ctx.CreateStreamResponse(
                stream => RestUtils.StreamXml(SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne), stream, streamController, charSetEncoding), responseContentType);
        }
        
        #endregion
    }
}
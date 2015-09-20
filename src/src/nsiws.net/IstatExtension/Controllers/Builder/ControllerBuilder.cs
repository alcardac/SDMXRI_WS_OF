// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The controller builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace IstatExtension.Controllers.Builder
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Nsi.AuthModule;
    using Estat.Nsi.DataRetriever;
    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Controller;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Estat.Sri.Ws.Controllers.Builder;
    using IstatExtension.Controllers.Controller;
    using IstatExtension.Retriever;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using System.IO;
    using Newtonsoft.Json;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;

    /// <summary>
    ///     The controller builder.
    /// </summary>
    public class ControllerBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The compact data format.
        /// </summary>
        private static readonly BaseDataFormat _compactDataFormat;

        /// <summary>
        ///     The cross sectional.
        /// </summary>
        private static readonly BaseDataFormat _crossSectional;

        /// <summary>
        ///     The generic data format.
        /// </summary>
        private static readonly BaseDataFormat _genericDataFormat;

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ControllerBuilder));

        /// <summary>
        ///     The SDMX schema v 20.
        /// </summary>
        private static readonly SdmxSchema _sdmxSchemaV20;

        /// <summary>
        ///     The SDMX schema v 21.
        /// </summary>
        private static readonly SdmxSchema _sdmxSchemaV21;

        private IMutableStructureSearchManager _structureSearchManager;

        #endregion

        #region Fields

        /// <summary>
        ///     The _advanced mutable structure search manager.
        /// </summary>
        private readonly IAdvancedMutableStructureSearchManager _advancedMutableStructureSearchManager;

        /// <summary>
        ///     The _retrieval with writer.
        /// </summary>
        private readonly IAdvancedSdmxDataRetrievalWithWriter _advancedRetrievalWithWriter;

        /// <summary>
        ///     The authorization aware advanced mutable structure search manager.
        /// </summary>
        private readonly IAuthAdvancedMutableStructureSearchManager _authAdvancedMutableStructureSearchManager;

        /// <summary>
        ///     The authorization aware mutable structure search manager v 20.
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authMutableStructureSearchManagerV20;

        /// <summary>
        ///     The authorization aware mutable structure search manager V21
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authMutableStructureSearchManagerV21;

        /// <summary>
        ///     The _mutable structure search manager v 20.
        /// </summary>
        private readonly IMutableStructureSearchManager _mutableStructureSearchManagerV20;

        /// <summary>
        ///     The _mutable structure search manager v 21.
        /// </summary>
        private readonly IMutableStructureSearchManager _mutableStructureSearchManagerV21;

        /// <summary>
        ///     The _retrieval with cross writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithCrossWriter _retrievalWithCrossWriter;

        /// <summary>
        ///     The _retrieval with writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithWriter _retrievalWithWriter;

        /// <summary>
        ///     The _retrieval with writer for v2.1.
        /// </summary>
        private readonly ISdmxDataRetrievalWithWriter _retrievalWithWriterv21;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="ControllerBuilder" /> class.
        /// </summary>
        static ControllerBuilder()
        {
            _compactDataFormat = BaseDataFormat.GetFromEnum(BaseDataFormatEnumType.Compact);
            _genericDataFormat = BaseDataFormat.GetFromEnum(BaseDataFormatEnumType.Generic);
            _crossSectional = BaseDataFormat.GetFromEnum(BaseDataFormatEnumType.CrossSectional);
            _sdmxSchemaV20 = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);
            _sdmxSchemaV21 = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerBuilder" /> class.
        /// </summary>
        public ControllerBuilder()
            : this(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreConnectionSettings">
        /// The mapping store connection settings.
        /// </param>
        /// <param name="defaultHeader">
        /// The default header.
        /// </param>
        public ControllerBuilder(ConnectionStringSettings mappingStoreConnectionSettings, IHeader defaultHeader)
        {
            if (mappingStoreConnectionSettings == null)
            {
                _log.Error("No connection string defined. Please check the web.config.");
                throw new ArgumentNullException("mappingStoreConnectionSettings");
            }

            var dataRetrieverCore = new JsonDataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwo);
            var dataRetrieverV21 = new JsonDataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwoPointOne);
            this._retrievalWithCrossWriter = dataRetrieverCore;
            this._retrievalWithWriter = dataRetrieverCore;
            this._retrievalWithWriterv21 = dataRetrieverV21;
            this._advancedRetrievalWithWriter = dataRetrieverV21;

            // structure search factories
            IStructureSearchManagerFactory<IAdvancedMutableStructureSearchManager> advancedFactory = new AdvancedMutableStructureSearchManagerFactory();
            IStructureSearchManagerFactory<IAuthAdvancedMutableStructureSearchManager> autAdvancedFactory = new AuthAdvancedMutableStructureSearchManagerFactory();

            IStructureSearchManagerFactory<IMutableStructureSearchManager> structureSearchManager = new MutableStructureSearchManagerFactory();
            IStructureSearchManagerFactory<IAuthMutableStructureSearchManager> autFactory = new AuthMutableStructureSearchManagerFactory();

            // advanced structure search managers 
            this._advancedMutableStructureSearchManager = advancedFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
            this._authAdvancedMutableStructureSearchManager = autAdvancedFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);

            // simple structure search managers
            this._mutableStructureSearchManagerV20 = structureSearchManager.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV20);
            this._authMutableStructureSearchManagerV20 = autFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV20);

            this._mutableStructureSearchManagerV21 = structureSearchManager.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
            this._authMutableStructureSearchManagerV21 = autFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the Advanced Structure controller for SDMX V21 SOAP.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="soapOperation">
        /// The SOAP operation.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<Message, XmlWriter> BuildAdvancedQueryStructureV21(DataflowPrincipal principal, SoapOperation soapOperation)
        {
            IWriterBuilder<IStructureWriterManager, XmlWriter> structureManagerBuilder = new StructureBuilder(WebServiceEndpoint.StandardEndpoint, _sdmxSchemaV21);
            IResponseGenerator<XmlWriter, ISdmxObjects> responseGenerator = new StructureResponseGenerator(structureManagerBuilder, StructureOutputFormatEnumType.SdmxV21StructureDocument);
            var structureRequestController = new StructureRequestV21AdvancedController<XmlWriter>(
                responseGenerator, 
                this._authAdvancedMutableStructureSearchManager, 
                this._advancedMutableStructureSearchManager, 
                principal, 
                soapOperation);
            return structureRequestController;
        }

        /// <summary>
        /// Builds the compact data controller for SDMX V20.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<XmlNode, XmlWriter> BuildCompactDataV20(DataflowPrincipal principal)
        {
            var compactDataFormat = _compactDataFormat;
            var sdmxSchemaV20 = _sdmxSchemaV20;
            return this.GetSimpleDataController(compactDataFormat, sdmxSchemaV20, principal);
        }

        /// <summary>
        /// Builds the cross sectional data controller for SDMX V20.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<XmlNode, XmlWriter> BuildCrossSectionalDataV20(DataflowPrincipal principal)
        {
            var dataWriterBuilder = new CrossDataWriterBuilder();
            var simpleDataResponseGenerator = new SimpleCrossDataResponseGenerator<XmlWriter>(this._retrievalWithCrossWriter, dataWriterBuilder);
            var simpleDataController = new SimpleDataController<XmlWriter>(simpleDataResponseGenerator, new DataRequestValidator(_crossSectional, _sdmxSchemaV20), principal);
            return simpleDataController;
        }

        /// <summary>
        /// Builds the generic data controller for SDMX V21.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="baseDataFormat">
        /// The base data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Impossible request. Requested CrossSectional for SDMX v2.1.
        /// </exception>
        public IController<IRestDataQuery, XmlWriter> BuildDataRest(DataflowPrincipal principal, BaseDataFormat baseDataFormat, SdmxSchema sdmxSchema)
        {
            return this.GetSimpleDataController(baseDataFormat, sdmxSchema, principal);
        }

        /// <summary>
        /// Builds the generic data controller for SDMX V21.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="baseDataFormat">
        /// The base data format.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XElement,XmlWriter}"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Impossible request. Requested CrossSectional for SDMX v2.1.
        /// </exception>
        public IController<Message, XmlWriter> BuildDataV20FromMessage(DataflowPrincipal principal, BaseDataFormat baseDataFormat)
        {
            return this.GetSimpleDataController(baseDataFormat, _sdmxSchemaV20, principal);
        }

        /// <summary>
        /// Builds the generic data controller for SDMX V21.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="dataFormat">
        /// The data format.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<Message, XmlWriter> BuildDataV21Advanced(DataflowPrincipal principal, BaseDataFormat dataFormat)
        {
            var sdmxSchema = _sdmxSchemaV21;
            return this.GetAdvancedDataController(dataFormat, sdmxSchema, principal);
        }

        /// <summary>
        /// Builds the generic data controller for SDMX V20.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<XmlNode, XmlWriter> BuildGenericDataV20(DataflowPrincipal principal)
        {
            var compactDataFormat = _genericDataFormat;
            var sdmxSchemaV20 = _sdmxSchemaV20;
            return this.GetSimpleDataController(compactDataFormat, sdmxSchemaV20, principal);
        }

        /// <summary>
        /// Builds the query structure for REST for the specified <paramref name="schema"/>.
        /// </summary>
        /// <param name="schema">
        /// The SDMX schema version.
        /// </param>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{IRestStructureQuery,XmlWriter}"/>.
        /// </returns>
        public IController<IRestStructureQuery, XmlWriter> BuildQueryStructureRest(SdmxSchema schema, DataflowPrincipal principal)
        {
            IWriterBuilder<IStructureWriterManager, XmlWriter> structureManagerBuilder = new StructureBuilder(WebServiceEndpoint.StandardEndpoint, schema);
            StructureOutputFormatEnumType outputFormat;

            IAuthMutableStructureSearchManager authMutableStructureSearchManager;
            IMutableStructureSearchManager mutableStructureSearchManager;
            switch (schema.EnumType)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    authMutableStructureSearchManager = this._authMutableStructureSearchManagerV20;
                    mutableStructureSearchManager = this._mutableStructureSearchManagerV20;
                    outputFormat = StructureOutputFormatEnumType.SdmxV2StructureDocument;

                    break;
                default:
                    authMutableStructureSearchManager = this._authMutableStructureSearchManagerV21;
                    mutableStructureSearchManager = this._mutableStructureSearchManagerV21;
                    outputFormat = StructureOutputFormatEnumType.SdmxV21StructureDocument;

                    break;
            }

            IResponseGenerator<XmlWriter, ISdmxObjects> responseGenerator = new StructureResponseGenerator(structureManagerBuilder, outputFormat);
            var structureRequestController = new StructureRequestRestController<XmlWriter>(responseGenerator, mutableStructureSearchManager, authMutableStructureSearchManager, principal);
            return structureRequestController;
        }



        public StringWriter StructureResponse(string structure, string agencyId, string resourceId, string version, StringWriter sw)
        {
            StringWriter jsonres= null;

            /*
            var structure = "codelist";
            var agencyId = agenzia; // "IT1";
            var resourceId = codelist; // "CL_REFAREA";
            var version = versione; // "1.4";
            */


            System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
            Org.Sdmxsource.Sdmx.Api.Model.Query.IRestStructureQuery input = BuildRestQueryBean(structure, agencyId, resourceId, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);

            /*
            object tipo;
            */
            

            
            //var codelistsType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.CodelistsType();
            // var structures = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.StructuresType();
            // structures.Codelists = codelistsType;
            IMutableStructureSearchManager mutableStructureSearchManagerV21;

            /**/
            //var wx = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DataflowsType();
            //structures.Dataflows = wx; 
            /**/
            Estat.Nsi.StructureRetriever.Factory.IStructureSearchManagerFactory<IMutableStructureSearchManager> structureSearchManager = new MutableStructureSearchManagerFactory();
            var sdmxSchemaV21 = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);

            mutableStructureSearchManagerV21 = structureSearchManager.GetStructureSearchManager(SettingsManager.MappingStoreConnectionSettings, sdmxSchemaV21);
            this._structureSearchManager = mutableStructureSearchManagerV21; ;
            Org.Sdmxsource.Sdmx.Api.Model.Mutable.IMutableObjects mutableObjects = this._structureSearchManager.GetMaintainables(input);
            var immutableObj = mutableObjects.ImmutableObjects;
            immutableObj.Header = Estat.Sri.Ws.Controllers.Manager.SettingsManager.Header;
           
            
            switch (structure)
            {
                case "codelist":
                    ISet<ICodelistObject> codelists = immutableObj.Codelists;
                    jsonres = WriteCodelist(codelists, resourceId, sw);
                    break;
                case "datastructure":
                    ISet<IDataStructureObject> datastructure = immutableObj.DataStructures;
                    jsonres = WriteDatastructure(datastructure, resourceId, sw);
                    break;
                case "dataflow":
                    ISet<IDataflowObject> dataflow = immutableObj.Dataflows;
                    jsonres = WriteDataflow(dataflow, resourceId, sw);
                    break;
                case "agencyscheme":
                    ISet<IAgencyScheme> agencies = immutableObj.AgenciesSchemes;
                    jsonres = WriteAgenciesScheme(agencies, resourceId, sw);
                    break;
                case "categorisation":
                    ISet<ICategorisationObject> categorisations = immutableObj.Categorisations;
                    jsonres = WriteCategorisation(categorisations, resourceId, sw);
                    break;
                case "dataproviderscheme":
                    ISet<IDataProviderScheme> dataproviderscheme = immutableObj.DataProviderSchemes;
                    jsonres = WriteDataProviderScheme(dataproviderscheme, resourceId, sw);
                    break;
                case "dataconsumerscheme":
                    ISet<IDataConsumerScheme> dataconsumerscheme = immutableObj.DataConsumerSchemes;
                    break;
                case "contentconstraint":
                    ISet<IContentConstraintObject> contentconstraint = immutableObj.ContentConstraintObjects;
                    break;
                case "categoryscheme":
                    ISet<ICategorySchemeObject> categoryscheme = immutableObj.CategorySchemes;
                    jsonres = WriteCategoryScheme(categoryscheme, resourceId, sw);
                    break;
                case "conceptscheme":
                    ISet<IConceptSchemeObject> conceptscheme = immutableObj.ConceptSchemes;
                    jsonres = WriteConceptScheme(conceptscheme, resourceId, sw);
                    break;
                case "provisionagreement":
                    ISet<IProvisionAgreementObject> provisionagreement = immutableObj.ProvisionAgreements;
                    break;
                case "organisationunitscheme":
                    ISet<IOrganisationUnitSchemeObject> organisationunitscheme = immutableObj.OrganisationUnitSchemes;
                    break;
                case "metadatastructure":
                    ISet<IMetadataStructureDefinitionObject> metadatastructure = immutableObj.MetadataStructures;
                    break;
                case "metadataflow":
                    ISet<IMetadataFlow> metadataflow = immutableObj.Metadataflows;
                    break;
                case "hierarchicalcodelist":
                    ISet<IHierarchicalCodelistObject> hierarchicalcodelist = immutableObj.HierarchicalCodelists;
                    break;

            }



            return jsonres;
        }

        private StringWriter WriteCategoryScheme(ISet<ICategorySchemeObject> categoryscheme, string resourceId, StringWriter sw)
        {
            if (categoryscheme.Count > 0)
            {


                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                    if (isIndented == true)
                    {
                        writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    }
                    writer.WriteStartObject();
                    foreach (ICategorySchemeObject categoryscheme1 in categoryscheme)
                    {
                        writer.WritePropertyName("CGS");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(categoryscheme1.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(categoryscheme1.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(categoryscheme1.Version);
                        writer.WritePropertyName("stub");
                        writer.WriteValue("false");
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(categoryscheme1.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(categoryscheme1.IsExternalReference.IsTrue);

                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in categoryscheme1.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();

                        writer.WritePropertyName("descriptions");
                        writer.WriteStartObject();
                        foreach (var item in categoryscheme1.Descriptions)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();

                        writer.WritePropertyName("categories");
                        writer.WriteStartObject();
                        foreach (var item in categoryscheme1.Items)
                        {
                            writer.WritePropertyName(item.Id);
                            writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue(item.Id);
                            writer.WritePropertyName("names");
                            writer.WriteStartObject();
                            foreach (var item1 in item.Names)
                            {
                                writer.WritePropertyName(item1.Locale);
                                writer.WriteValue(item1.Value);
                            }
                            writer.WriteEndObject();
                            writer.WritePropertyName("descriptions");
                            writer.WriteStartObject();
                            foreach (var item1 in item.Descriptions)
                            {
                                writer.WritePropertyName(item1.Locale);
                                writer.WriteValue(item1.Value);
                            }
                            writer.WriteEndObject();

                            writer.WritePropertyName("categories");
                            writer.WriteStartObject();

                            foreach (var item1 in item.Items)
                            {
                                writer.WritePropertyName(item1.Id);
                                writer.WriteStartObject();
                                       writer.WritePropertyName("id");
                                       writer.WriteValue(item1.Id);
                                       writer.WritePropertyName("refclass");
                                       writer.WriteValue("Category");
                                       writer.WritePropertyName("names");
                                       writer.WriteStartObject();
                                       foreach (var item2 in item1.Names)
                                       {
                                           writer.WritePropertyName(item2.Locale);
                                           writer.WriteValue(item2.Value);
                                       }
                                       writer.WriteEndObject();
                                writer.WriteEndObject();
                            }


                            writer.WriteEndObject();



                            writer.WriteEndObject();

                        }
                        //               writer.WriteEndObject();



                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("CGS");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();




                    }
                    writer.WriteEndObject();
                }

            }
            return sw;

        }


        private StringWriter WriteConceptScheme(ISet<IConceptSchemeObject>  conceptschemes, string resourceId, StringWriter sw)
        {
            if (conceptschemes.Count > 0)
            {
               

                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        foreach (IConceptSchemeObject conceptscheme in conceptschemes)
                {
                        writer.WritePropertyName("CNS");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(conceptscheme.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(conceptscheme.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(conceptscheme.Version);
                        writer.WritePropertyName("stub");
                        writer.WriteValue("false");
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(conceptscheme.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(conceptscheme.IsExternalReference.IsTrue);

                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in conceptscheme.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();

                        writer.WritePropertyName("concepts");
                        writer.WriteStartObject();
                        foreach (var item in conceptscheme.Items)
                        {
                            writer.WritePropertyName(item.Id);
                            writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue(item.Id);
                                writer.WritePropertyName("names");
                                writer.WriteStartObject();
                                foreach (var item1 in item.Names)
                                {
                                    writer.WritePropertyName(item1.Locale);
                                    writer.WriteValue(item1.Value);
                                }
                                writer.WriteEndObject();
                                writer.WritePropertyName("descriptions");
                                writer.WriteStartObject();
                                foreach (var item1 in item.Descriptions)
                                {
                                    writer.WritePropertyName(item1.Locale);
                                    writer.WriteValue(item1.Value);
                                }
                                writer.WriteEndObject();
                            writer.WriteEndObject();

                        }
         //               writer.WriteEndObject();

                        

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("CNS");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        



                    }
                        writer.WriteEndObject();
                }

            }
            return sw;

        }

        private StringWriter WriteAgenciesScheme(ISet<IAgencyScheme>  agencies, string resourceId, StringWriter sw)
        {
            if (agencies.Count > 0)
            {
                foreach (IAgencyScheme agencie in agencies)
                {

                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        writer.WritePropertyName("AGS");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(agencie.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(agencie.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(agencie.Version);
                        writer.WritePropertyName("stub");
                        writer.WriteValue(agencie.Version);
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(agencie.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(agencie.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in agencie.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();
                        writer.WritePropertyName("descriptions");
                        writer.WriteStartObject();
                        foreach (var item in agencie.Descriptions)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();

                        writer.WritePropertyName("annotations");
                        writer.WriteStartObject();

                        ////////////////////////
                        
                        writer.WritePropertyName("texts");
                        
                        foreach (var item in agencie.Annotations)
                        {
                            writer.WriteStartObject();
                            writer.WritePropertyName("texts");
                            writer.WriteValue(item.Text);
                            writer.WritePropertyName("id");
                            writer.WriteValue(item.Id);
                            writer.WritePropertyName("title");
                            writer.WriteValue(item.Title);    
                            writer.WriteEndObject();
                        
                        }
                        ////////////////////////

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("AGS");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        writer.WriteEndObject();



                    }
                }

            }
            return sw;
        }

       
        private StringWriter WriteCategorisation(ISet<ICategorisationObject> categorisations, string resourceId, StringWriter sw)
        {
            if (categorisations.Count > 0)
            {
               

                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        foreach (ICategorisationObject categorisation in categorisations)
                        {
                        writer.WritePropertyName("CTS");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(categorisation.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(categorisation.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(categorisation.Version);
                        writer.WritePropertyName("stub");
                        writer.WriteValue("???false");
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(categorisation.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(categorisation.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in categorisation.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();
                        writer.WritePropertyName("categoryReference");
                        writer.WriteStartObject();

                        ////////////////////////

                        writer.WritePropertyName("categoryId");
                        writer.WriteValue(categorisation.CategoryReference.FullId);


                        writer.WritePropertyName("categoryScheme");
                        writer.WriteStartObject();
                                writer.WritePropertyName("id");
                                writer.WriteValue(categorisation.CategoryReference.MaintainableId);
                                writer.WritePropertyName("agencyID");
                                writer.WriteValue(categorisation.CategoryReference.AgencyId);
                                writer.WritePropertyName("version");
                                writer.WriteValue(categorisation.CategoryReference.Version);
                       writer.WriteEndObject();
                        ////////////////////////



                       writer.WritePropertyName("dataflowReference");
                       writer.WriteStartObject();

                       ////////////////////////

                       writer.WritePropertyName("id");
                       writer.WriteValue(categorisation.StructureReference.MaintainableId);
                       writer.WritePropertyName("agencyID");
                       writer.WriteValue(categorisation.StructureReference.AgencyId);
                       writer.WritePropertyName("version");
                       writer.WriteValue(categorisation.StructureReference.Version);
                       
                       ////////////////////////

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("CTS");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        



                    }
                        writer.WriteEndObject();
                }

            }
            return sw;
        }
        
        
        private StringWriter WriteDataProviderScheme(ISet<IDataProviderScheme> dataproviderscheme, string resourceId, StringWriter sw)
        {

            if (dataproviderscheme.Count > 0)
            {
                foreach (IDataProviderScheme dataprovider in dataproviderscheme)
                {

                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                       // writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        writer.WritePropertyName("DPS");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue("DATA_PROVIDERS");
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(dataprovider.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(dataprovider.Version);
                        writer.WritePropertyName("stub");
                        writer.WriteValue("false");
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(dataprovider.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(dataprovider.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in dataprovider.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();
                        writer.WritePropertyName("descriptions");
                        writer.WriteStartObject();
                        foreach (var item in dataprovider.Descriptions)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();

                        writer.WritePropertyName("dataProviders");
                        writer.WriteStartObject();

                        ////////////////////////
                        foreach (var item in dataprovider.Items)
                        {
                            writer.WritePropertyName(item.Id);
                            writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue(item.Id);
                            writer.WritePropertyName("names");
                           
                            foreach (var item1 in item.Names)
                            {
                                writer.WriteStartObject();
                                writer.WritePropertyName(item1.Locale);
                                writer.WriteValue(item1.Value);
                                writer.WriteEndObject();
                            }
                            
                            
                            writer.WriteEndObject();
                        }
                        ////////////////////////

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("DPS");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        writer.WriteEndObject();



                    }
                }

            }
            return sw;
        }
        

        private StringWriter WriteCodelist(ISet<ICodelistObject> codelists, string resourceId, StringWriter sw)
        {
            if (codelists.Count > 0)
            {
               
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        
                        foreach (ICodelistObject codelistBean in codelists)
                        {
                            writer.WritePropertyName("CDL");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");

                        writer.WriteValue(codelistBean.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(codelistBean.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(codelistBean.Version);
                        if (codelistBean.StartDate!= null)
                        {
                            writer.WritePropertyName("startDate");
                            writer.WriteValue(codelistBean.StartDate);
                        }
                        if (codelistBean.EndDate != null)
                        {
                            writer.WritePropertyName("endDate");
                            writer.WriteValue(codelistBean.EndDate);
                        }
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(codelistBean.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(codelistBean.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject(); 
                        foreach (var item in codelistBean.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);
                           
                        }
                        writer.WriteEndObject(); 
                        writer.WritePropertyName("codes");

                        writer.WriteStartObject(); 
                        ////////////////////////
                        IList<ICode> codes = codelistBean.Items;
                        if (codes.Count > 0)
                        {
                            foreach (ICode codeBean in codes)
                            {
                                writer.WritePropertyName(codeBean.Id);
                                writer.WriteStartObject();
                                writer.WritePropertyName("id");
                                writer.WriteValue(codeBean.Id);
                                writer.WritePropertyName("names");
                                writer.WriteStartObject();
                                foreach (var codeName in codeBean.Names)
                                {
                                 
                                    writer.WritePropertyName(codeName.Locale);
                                    writer.WriteValue(codeBean.Id);
                                    
                                }
                               
                                writer.WriteEndObject();
                               
                                // writer.WriteComment("(broken)");
                                writer.WritePropertyName("descriptions");
                                writer.WriteStartObject();
                                foreach (var codeDesc in codeBean.Names)
                                {
                                    writer.WritePropertyName(codeDesc.Locale);
                                    writer.WriteValue(codeDesc.Value);

                                }
                                writer.WriteEndObject();
                                writer.WriteEndObject();
                               
                            }
                        }
                        
                       
                       
                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("CDL");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                       
                    }
                       
                        writer.WriteEndObject();
                }

            }
            return sw;
        
        }

        private StringWriter WriteDataflow(ISet<IDataflowObject> dataflow, string resourceId, StringWriter sw)
        {
            if (dataflow.Count > 0)
            {
                
                   
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        foreach (IDataflowObject dataflowBean in dataflow)
                        {
                        writer.WritePropertyName("DFD");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(dataflowBean.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(dataflowBean.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(dataflowBean.Version);
                        if (dataflowBean.StartDate != null)
                        {
                            writer.WritePropertyName("startDate");
                            writer.WriteValue(dataflowBean.StartDate);
                        }
                        if (dataflowBean.EndDate != null)
                        {
                            writer.WritePropertyName("endDate");
                            writer.WriteValue(dataflowBean.EndDate);
                        }
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(dataflowBean.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(dataflowBean.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in dataflowBean.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();
                        writer.WritePropertyName("structure");
                        writer.WriteStartObject(); 

                        ////////////////////////

                        writer.WritePropertyName("id");
                        writer.WriteValue(dataflowBean.DataStructureRef.MaintainableId);
                        

                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(dataflowBean.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(dataflowBean.Version);
                        writer.WritePropertyName("refclass"); 
                        writer.WriteValue("Data Structure Definition");
                        writer.WritePropertyName("urn");
                        writer.WriteValue(dataflowBean.Urn);
                        ////////////////////////

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("DFD");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                      

                        

                    }
                        writer.WriteEndObject();
                }

            }
            return sw;
        }

        private StringWriter WriteDatastructure(ISet<IDataStructureObject> datastructure, string resourceId, StringWriter sw)
        {
            string tre = "";
            if (datastructure.Count > 0)
            {
              
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        bool isIndented = Convert.ToBoolean(ConfigurationManager.AppSettings["isIndented"]);
                        if (isIndented == true)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        writer.WriteStartObject();
                        foreach (IDataStructureObject datastructureBean in datastructure)
                        {
                        writer.WritePropertyName("DSD");
                        if (datastructureBean.Id.Contains("SEP_POP_NUZ") == true)
                                tre = "ASIA";
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                        writer.WritePropertyName("id");
                        writer.WriteValue(datastructureBean.Id);
                        writer.WritePropertyName("agencyID");
                        writer.WriteValue(datastructureBean.AgencyId);
                        writer.WritePropertyName("version");
                        writer.WriteValue(datastructureBean.Version);
                        if (datastructureBean.StartDate != null)
                        {
                            if (datastructureBean.StartDate.DateInSdmxFormat != null)
                            {
                                writer.WritePropertyName("startDate");
                                writer.WriteValue(datastructureBean.StartDate.DateInSdmxFormat);
                            }
                        }
                        if (datastructureBean.EndDate != null)
                        {
                            if (datastructureBean.EndDate.DateInSdmxFormat != null)
                            {
                                writer.WritePropertyName("endDate");
                                writer.WriteValue(datastructureBean.EndDate.DateInSdmxFormat);
                            }
                        }
                        writer.WritePropertyName("isFinal");
                        writer.WriteValue(datastructureBean.IsFinal.IsTrue);
                        writer.WritePropertyName("externalReference");
                        writer.WriteValue(datastructureBean.IsExternalReference.IsTrue);
                        writer.WritePropertyName("names");
                        writer.WriteStartObject();
                        foreach (var item in datastructureBean.Names)
                        {
                            writer.WritePropertyName(item.Locale);
                            writer.WriteValue(item.Value);

                        }
                        writer.WriteEndObject();
                        writer.WritePropertyName("dimensions");

                        writer.WriteStartObject();
                        foreach (IDimension dimensions in datastructureBean.DimensionList.Dimensions)
                        {
                          
                            ////////////////////////

                            
                            //writer.WritePropertyName("FREQ");
                            writer.WritePropertyName(dimensions.Id);
                            writer.WriteStartObject();
                                writer.WritePropertyName("id");
                                writer.WriteValue(dimensions.Id);
                                writer.WritePropertyName("refclass");
                                writer.WriteValue("Dimension");
                                writer.WritePropertyName("position");
                                writer.WriteValue(dimensions.Position);
                                writer.WritePropertyName("conceptScheme");
                                writer.WriteStartObject();
                                    writer.WritePropertyName("id");
                                   //
                                    // ???????????????????????????????????
                                    writer.WriteValue(dimensions.ConceptRef.MaintainableId);
                                    writer.WritePropertyName("agencyID");
                                    writer.WriteValue(dimensions.ConceptRef.AgencyId);
                                    //writer.WriteValue("ESTAT");
                                    writer.WritePropertyName("version");
                                    writer.WriteValue(dimensions.ConceptRef.Version);
                                    writer.WritePropertyName("refclass");
                                    writer.WriteValue("Concept");
                                    writer.WritePropertyName("conceptID");
                                    writer.WriteValue(dimensions.ConceptRef.FullId);
                                    //writer.WriteValue("FREQ");
                                writer.WriteEndObject();

                                //var erx = dimensi;
                            if(dimensions.Representation != null) {
                                if (dimensions.Representation.CrossReferences != null)
                                {
                                ISet<ICrossReference> isr = dimensions.Representation.CrossReferences;
                                foreach (var x in isr)
                                {
                                    //var codelist = x.MaintainableReference.MaintainableId;
                                    //var agenzia = x.MaintainableReference.AgencyId;
                                    //var versione = x.MaintainableReference.Version;
                                    //var urn = x.TargetUrn.AbsoluteUri;
                                    

                               

                            writer.WritePropertyName("localRepresentation");
                            writer.WriteStartObject();
                                writer.WritePropertyName("urn");
                                writer.WriteValue(x.TargetUrn.AbsoluteUri);
                                //writer.WriteValue("urn:sdmx:org.sdmx.infomodel.codelist.Codelist=ECB:CL_FREQ(1.0)");
                                writer.WritePropertyName("codelistRef");
                                writer.WriteStartObject();
                                    writer.WritePropertyName("id");
                                    writer.WriteValue(x.MaintainableReference.MaintainableId);
                                    //writer.WriteValue("CL_FREQ");
                                    writer.WritePropertyName("agencyID");
                                    writer.WriteValue(x.MaintainableReference.AgencyId);
                                    writer.WritePropertyName("version");
                                    writer.WriteValue(x.MaintainableReference.Version);
                                writer.WriteEndObject();
                            writer.WriteEndObject();
                                }
                            }
                        }
                            //non sempre
                            if (dimensions.ConceptRole != null) {
                             IList<ICrossReference> ConRol = dimensions.ConceptRole;
                             foreach (var xw in ConRol)
                             {
                                 var s = xw.MaintainableId;
                                       
                                
                                
                            
                            writer.WritePropertyName("conceptRole");
                            writer.WriteStartObject();
                                writer.WritePropertyName("FREQUENCY");
                                writer.WriteStartObject();
                                    writer.WritePropertyName("id");
                                    writer.WriteValue("COMPONENT_ROLES");
                                    writer.WritePropertyName("agencyID");
                                    writer.WriteValue(xw.AgencyId);
                                    writer.WritePropertyName("version");
                                    writer.WriteValue(xw.Version);
                                    writer.WritePropertyName("conceptID");
                                    writer.WriteValue("FREQUENCY");
                                writer.WriteEndObject();
                            writer.WriteEndObject();
                        }
                        }
                            //------------------
                            writer.WriteEndObject();
                        }
                            writer.WriteEndObject();
                       
                        ////////////////////////
                        // groups//////////////


                            IList<IGroup> gplist = datastructureBean.Groups;
                            if (gplist.Count > 0)
                            {
                                foreach (var gp1 in gplist){
                                  
                                
                                writer.WritePropertyName("groups");
                                writer.WriteStartObject();
                                ////////////////////////


                                writer.WritePropertyName("Sibling");
                                writer.WriteStartObject();
                                writer.WritePropertyName("id");
                                writer.WriteValue(gp1.Id);
                                    //writer.WriteValue("Sibling");
                                writer.WritePropertyName("uri");
                                writer.WriteValue( gp1.Uri);
                                writer.WritePropertyName("position");
                                writer.WriteValue("0");
                                writer.WritePropertyName("dimensions");

                                IList<string> gp12 =  gp1.DimensionRefs;

                                  
                                writer.WriteStartArray();
                                foreach (var gppp in gp12)
                                {
                                    writer.WriteValue(gppp.ToString());
                                    //writer.WriteValue("REF_AREA");
                                   // writer.WriteValue("ADJUSTMENT");
                                   // writer.WriteValue("STS_INDICATOR");
                                   // writer.WriteValue("STS_ACTIVITY");
                                   // writer.WriteValue("STS_INSTITUTION");
                                   // writer.WriteValue("STS_BASE_YEAR");
                                }
                                writer.WriteEndArray();
                                writer.WriteEndObject();


                                writer.WriteEndObject();
                                }

                            }


                            //------------------
                          //  writer.WriteEndObject();
                        ///////////////////////

                        ///////////////////////
                        //attributes///////////
                       
                    var attributes = datastructureBean.AttributeList;
                    if (!(attributes == null))
                    {
                          
                        writer.WritePropertyName("attributes");
                            writer.WriteStartObject();
                            foreach (var item in attributes.Attributes)
                            {
                                
                                writer.WritePropertyName(item.Id);
                                writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue(item.Id);
                            writer.WritePropertyName("refclass");
                            writer.WriteValue("Data Attribute");
                            writer.WritePropertyName("assignmentStatus");
                            writer.WriteValue(item.AssignmentStatus);
                            writer.WritePropertyName("attachmentLevel");
                            writer.WriteValue(item.AttachmentLevel);
                            writer.WritePropertyName("attachmentGroup");
                            writer.WriteValue(item.AttachmentGroup);
                            var t = item.ConceptRef.AgencyId;
                            writer.WritePropertyName("conceptScheme");
                            writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue("STS_SCHEME");
                            writer.WritePropertyName("agencyID");
                            writer.WriteValue(item.ConceptRef.AgencyId);
                            writer.WritePropertyName("version");
                            writer.WriteValue(item.ConceptRef.Version);
                            writer.WritePropertyName("refclass");
                            writer.WriteValue("Concept");
                            writer.WritePropertyName("conceptID");
                            writer.WriteValue(item.ConceptRef.FullId);
                            writer.WriteEndObject();


                          // var isr3 = item.AllTextTypes;
                           var isr4 = item.DimensionReferences;
                            //foreach (var x in isr3)
                            //{
                             // ????????????????????????????????????????
                            writer.WritePropertyName("localRepresentation");
                            writer.WriteStartObject();
                            writer.WritePropertyName("textFormat");
                                                                    writer.WriteStartObject();

                                                                    writer.WritePropertyName("isMultiLingual");
                                                                    writer.WriteValue("false");
                                                                    writer.WritePropertyName("textType");
                                                                    writer.WriteValue("STRING");
                                                                    writer.WritePropertyName("isSequence");
                                                                    writer.WriteValue("false");
                                                                    writer.WriteEndObject();

                           writer.WriteEndObject();
                           // }
                                /////////////////////////
                           writer.WritePropertyName("attributeRelationship");
                           if (item.DimensionReferences.Count > 0)
                           {
                               writer.WriteStartObject();
                               writer.WritePropertyName("dimensions");
                               writer.WriteStartArray();
                              for (int i=0; i < item.DimensionReferences.Count; i++){
                                  writer.WriteValue(item.DimensionReferences[i].ToString());
                               }
                               writer.WriteEndArray();
                               writer.WriteEndObject();
                           }

                           else
                           {
                               if (!String.IsNullOrEmpty(item.PrimaryMeasureReference)){
                                   writer.WriteStartObject();
                                   
                                   writer.WritePropertyName("primaryMeasure");
                                   writer.WriteValue(item.PrimaryMeasureReference);
                                   writer.WriteEndObject();
                               
                               }
                               
                               


                               //writer.WriteEndObject();
                           }


                                //////////////////
                           writer.WriteEndObject();
                            }
                          /*
                            writer.WriteStartObject();
                            writer.WritePropertyName("id");
                            writer.WriteValue("Sibling");
                            writer.WritePropertyName("uri");
                            writer.WriteValue("");
                            writer.WritePropertyName("position");
                            writer.WriteValue("0");
                            writer.WritePropertyName("dimensions");
                            writer.WriteStartArray();
                                   writer.WriteValue("REF_AREA");
                                   writer.WriteValue("ADJUSTMENT");
                                   writer.WriteValue("STS_INDICATOR");
                                   writer.WriteValue("STS_ACTIVITY");
                                   writer.WriteValue("STS_INSTITUTION");
                                   writer.WriteValue("STS_BASE_YEAR");
                                   writer.WriteEndArray();
                            writer.WriteEndObject();

                            
                            writer.WriteEndObject();
                         

                          
                            writer.WriteEndObject();
                        */
                        //////////////////////
                           ///////////////////////
                           //attributes///////////
                           writer.WriteEndObject();

                    }

                    IMeasureList mslist = datastructureBean.MeasureList;

                           writer.WritePropertyName("measures");
                           writer.WriteStartObject();
                           writer.WritePropertyName(mslist.PrimaryMeasure.ConceptRef.FullId);
                                        writer.WriteStartObject();
                                        writer.WritePropertyName("id");
                                        writer.WriteValue(mslist.PrimaryMeasure.ConceptRef.FullId);
                                        writer.WritePropertyName("refclass");
                                        writer.WriteValue("Primary Measure");
                                        writer.WritePropertyName("conceptScheme");
                                                            writer.WriteStartObject();
                                                            writer.WritePropertyName("id");
                                                            writer.WriteValue("STS_SCHEME");

                                                            writer.WritePropertyName("agencyID");
                        //mslist.AsReference.AgencyId
                                                            writer.WriteValue(mslist.PrimaryMeasure.ConceptRef.AgencyId);
                                                            writer.WritePropertyName("version");
                                                            writer.WriteValue(mslist.PrimaryMeasure.ConceptRef.Version);
                                                            writer.WritePropertyName("refclass");
                                                            writer.WriteValue("Concept");
                                                            writer.WritePropertyName("conceptID");
                                                            writer.WriteValue(mslist.PrimaryMeasure.ConceptRef.FullId);
                                                            writer.WriteEndObject();
                        
                                                            writer.WritePropertyName("localRepresentation");
                                                            writer.WriteStartObject();
                                                            writer.WritePropertyName("textFormat");
                                                            writer.WriteStartObject();


                                                            writer.WritePropertyName("isMultiLingual");

                                                            writer.WriteValue("false");
                                                            writer.WritePropertyName("textType");
                                                            writer.WriteValue("?????DOUBLE");
                                                            writer.WritePropertyName("isSequence");
                                                            writer.WriteValue("false");
                                                            
                                                            writer.WriteEndObject();
                                                            writer.WriteEndObject();

                          
                           writer.WriteEndObject();

                          // writer.WriteEndObject();


                        /////////////////////////////

                        writer.WriteEnd();
                        writer.WritePropertyName("accessLevel");
                        writer.WriteValue("PUBLIC");
                        //writer.WritePropertyName("lastUpdateDate");
                        //writer.WriteValue(codelistBean.IsExternalReference);
                        writer.WritePropertyName("type");
                        writer.WriteValue("DSD");
                        writer.WritePropertyName("canBeMaintained");
                        writer.WriteValue("true");
                       // writer.WriteEndObject();
                        writer.WriteEndArray();
                       






                    }
                        writer.WriteEndObject();
                }
            }
            return sw;
        }


        private static IRestStructureQuery BuildRestQueryBean(string structure, string agencyId, string resourceId, string version, System.Collections.Specialized.NameValueCollection queryParameters)
        {
            var queryString = new string[4];
            queryString[0] = structure;
            queryString[1] = agencyId;
            queryString[2] = resourceId;
            queryString[3] = version;

            IDictionary<string, string> paramsDict = new Dictionary<string, string>();

            Org.Sdmxsource.Sdmx.Api.Model.Query.IRestStructureQuery query;

            try
            {
                query = new Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.RESTStructureQueryCore(queryString, paramsDict);
            }

            catch (Exception e)
            {

                throw new System.ServiceModel.Web.WebFaultException<string>(e.Message, System.Net.HttpStatusCode.BadRequest);
            }

            return query;
        }
		

        /// <summary>
        /// Builds the query structure response for SDMX V20.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        public IController<XmlNode, XmlWriter> BuildQueryStructureV20(WebServiceEndpoint endpoint, DataflowPrincipal dataflowPrincipal)
        {
            IWriterBuilder<IStructureWriterManager, XmlWriter> structureManagerBuilder = new StructureBuilder(endpoint, _sdmxSchemaV20);
            IResponseGenerator<XmlWriter, ISdmxObjects> responseGenerator = new StructureResponseGenerator(structureManagerBuilder, StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument);
            var structureRequestController = new StructureRequestV20Controller<XmlWriter>(
                responseGenerator, 
                this._mutableStructureSearchManagerV20, 
                this._authMutableStructureSearchManagerV20, 
                dataflowPrincipal);
            return structureRequestController;
        }

        /// <summary>
        /// Builds the query structure for SDMX v20 SOAP requests
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XElement,XmlWriter}"/>.
        /// </returns>
        public IController<Message, XmlWriter> BuildQueryStructureV20FromMessage(WebServiceEndpoint endpoint, DataflowPrincipal principal)
        {
            IWriterBuilder<IStructureWriterManager, XmlWriter> structureManagerBuilder = new StructureBuilder(endpoint, _sdmxSchemaV20);
            IResponseGenerator<XmlWriter, ISdmxObjects> responseGenerator = new StructureResponseGenerator(structureManagerBuilder, StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument);
            var structureRequestController = new StructureRequestV20Controller<XmlWriter>(
                responseGenerator, 
                this._mutableStructureSearchManagerV20, 
                this._authMutableStructureSearchManagerV20, 
                principal);
            return structureRequestController;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data retrieval with writer.
        /// </summary>
        /// <param name="sdmxSchemaVersion">
        /// The SDMX schema version.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxDataRetrievalWithWriter"/>.
        /// </returns>
        private ISdmxDataRetrievalWithWriter GetDataRetrievalWithWriter(SdmxSchemaEnumType sdmxSchemaVersion)
        {
            switch (sdmxSchemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._retrievalWithWriterv21;
                default:
                    return this._retrievalWithWriter;
            }
        }

        /// <summary>
        /// Gets the Advanced (SDMX v2.1 SOAP) data controller.
        /// </summary>
        /// <param name="dataFormat">
        /// The compact data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema V20.
        /// </param>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        private IController<Message, XmlWriter> GetAdvancedDataController(BaseDataFormat dataFormat, SdmxSchema sdmxSchema, DataflowPrincipal principal)
        {
            var dataWriterBuilder = new DataWriterBuilder(dataFormat, sdmxSchema);
            var dataResponseGenerator = new AdvancedDataResponseGenerator<XmlWriter>(this._advancedRetrievalWithWriter, dataWriterBuilder);
            var operation = dataFormat.GetSoapOperation(SdmxSchemaEnumType.VersionTwoPointOne);
            var dataController = new AdvancedDataController<XmlWriter>(dataResponseGenerator, new DataRequestValidator(dataFormat, sdmxSchema), principal, operation);
            return dataController;
        }

        /// <summary>
        /// Gets the simple data controller for SDMX v2.0 SOAP or REST.
        /// </summary>
        /// <param name="dataFormat">
        /// The compact data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema V20.
        /// </param>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        private SimpleDataController<XmlWriter> GetSimpleDataController(BaseDataFormat dataFormat, SdmxSchema sdmxSchema, DataflowPrincipal principal)
        {
            SimpleDataController<XmlWriter> simpleDataController;
            if (dataFormat.EnumType == BaseDataFormatEnumType.CrossSectional)
            {
                if (sdmxSchema.EnumType != SdmxSchemaEnumType.VersionTwo)
                {
                    throw new SdmxSemmanticException("Impossible request. Requested CrossSectional for SDMX v2.1.");
                }

                var dataWriterBuilder = new CrossDataWriterBuilder();
                var simpleDataResponseGenerator = new SimpleCrossDataResponseGenerator<XmlWriter>(this._retrievalWithCrossWriter, dataWriterBuilder);
                simpleDataController = new SimpleDataController<XmlWriter>(simpleDataResponseGenerator, new DataRequestValidator(_crossSectional, _sdmxSchemaV20), principal);
            }
            else
            {
                var dataWriterBuilder = new DataWriterBuilder(dataFormat, sdmxSchema);
                var simpleDataResponseGenerator = new SimpleDataResponseGenerator<XmlWriter>(this.GetDataRetrievalWithWriter(sdmxSchema.EnumType), dataWriterBuilder);
                simpleDataController = new SimpleDataController<XmlWriter>(simpleDataResponseGenerator, new DataRequestValidator(dataFormat, sdmxSchema), principal);
            }

            return simpleDataController;
        }

        #endregion

        #region SDMXJSON

        /// <summary>
        /// Gets the data retrieval with writer.
        /// </summary>
        /// <param name="sdmxSchemaVersion">
        /// The SDMX schema version.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxDataRetrievalWithWriter"/>.
        /// </returns>
        private ISdmxDataRetrievalWithWriter SdmxJsonGetDataRetrievalWithWriter()
        {
            return _retrievalWithWriter;
        }



        //private static readonly BaseDataFormat _jsonDataFormat;

        /// <summary>
        /// Builds the json data controller for SDMX V21.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="baseDataFormat">
        /// The base data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <returns>
        /// The <see cref="IController{XmlNode,XmlWriter}"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Impossible request. Requested CrossSectional for SDMX v2.1.
        /// </exception>
        public IController<IRestDataQuery, Newtonsoft.Json.JsonTextWriter> BuildJsonDataRest(DataflowPrincipal principal, BaseDataFormat baseDataFormat, SdmxSchema sdmxSchema)
        {
            return this.GetSdmxJsonDataController(baseDataFormat, sdmxSchema, principal);
        }


        private SdmxJsonDataController<Newtonsoft.Json.JsonTextWriter> GetSdmxJsonDataController(BaseDataFormat dataFormat, SdmxSchema sdmxSchema, DataflowPrincipal principal)
        {
            SdmxJsonDataController<Newtonsoft.Json.JsonTextWriter> sdmxjsonDataController;

            var dataWriterBuilder = new SdmxJsonDataWriterBuilder(dataFormat, sdmxSchema);

            var sdmxjsonDataResponseGenerator = new SdmxJsonDataResponseGenerator<Newtonsoft.Json.JsonTextWriter>(this.SdmxJsonGetDataRetrievalWithWriter(), dataWriterBuilder);
            
            sdmxjsonDataController = new SdmxJsonDataController<Newtonsoft.Json.JsonTextWriter>(sdmxjsonDataResponseGenerator, new DataRequestValidator(dataFormat, sdmxSchema), principal);

            return sdmxjsonDataController;
        }

        #endregion
    }
}

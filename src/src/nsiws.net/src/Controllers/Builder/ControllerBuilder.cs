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
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Nsi.AuthModule;
    using Estat.Nsi.DataRetriever;
    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
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
    using Org.Sdmxsource.Sdmx.DataParser.Manager;

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

            var dataRetrieverCore = new DataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwo);
            var dataRetrieverV21 = new DataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwoPointOne);
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
    }
}
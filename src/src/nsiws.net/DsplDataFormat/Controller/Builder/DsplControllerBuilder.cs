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
namespace DsplDataFormat.Controller.Builder
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
    using DsplDataFormat.Engine;    
    using DsplDataFormat.Engine.Manager;
    using DsplDataFormat.Retriever;
    using Estat.Sri.Ws.Controllers.Builder;

    /// <summary>
    ///     The controller builder.
    /// </summary>
    public class DsplControllerBuilder
    {
        #region Static Fields

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

        //andrea
        private readonly IDataRetrieverTabular _retrievalTabular;

        /// <summary>
        ///     The _retrieval with writer for v2.1.
        /// </summary>
        private readonly IDsplDataRetrievalWithWriter _retrievalWithWriterv21;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="ControllerBuilder" /> class.
        /// </summary>
        static DsplControllerBuilder()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerBuilder" /> class.
        /// </summary>
        public DsplControllerBuilder()
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
        public DsplControllerBuilder(ConnectionStringSettings mappingStoreConnectionSettings, IHeader defaultHeader)
        {
            if (mappingStoreConnectionSettings == null)
            {
                _log.Error("No connection string defined. Please check the web.config.");
                throw new ArgumentNullException("mappingStoreConnectionSettings");
            }

            var dataRetrieverCore = new DataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwo);
            var dataRetrieverV21 = new DataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwoPointOne);
            var dataRetrieverDSPL = new DsplDataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwoPointOne);
            this._retrievalWithCrossWriter = dataRetrieverCore;
            this._retrievalWithWriter = dataRetrieverCore;
            this._retrievalWithWriterv21 = dataRetrieverDSPL;
            this._advancedRetrievalWithWriter = dataRetrieverV21;

            //andrea
            this._retrievalTabular = dataRetrieverCore;

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
        /// Builds the DSPL data controller for SDMX V21.
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
        public IController<IRestDataQuery, DsplTextWriter> BuildDsplDataRest(DataflowPrincipal principal, BaseDataFormat baseDataFormat, SdmxSchema sdmxSchema)
        {
            DsplDataController<DsplTextWriter> dsplDataController;

            var dataWriterBuilder = new DsplDataWriterBuilder(baseDataFormat, sdmxSchema);            
            var dsplDataResponseGenerator = new DsplDataResponseGenerator<DsplTextWriter>(this.GetDataRetrievalWithWriter(sdmxSchema.EnumType), dataWriterBuilder);
            dsplDataController = new DsplDataController<DsplTextWriter>(dsplDataResponseGenerator, new DataRequestValidator(baseDataFormat, sdmxSchema), principal);

            return dsplDataController;

        }

        public IController<IRestDataQuery, DsplTextWriter> BuildDataRest(DataflowPrincipal principal)
        {
            return this.GetDsplDataController(principal);
        }

        public IController<Message, DsplTextWriter> BuildDsplDataFromMessage(DataflowPrincipal principal, BaseDataFormat baseDataFormat)
        {
            return this.GetDsplDataController(principal);
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
        private IDsplDataRetrievalWithWriter GetDataRetrievalWithWriter(SdmxSchemaEnumType sdmxSchemaVersion)
        {
            return this._retrievalWithWriterv21;            
        }
        private IDsplDataRetrievalWithWriter GetDataRetrievalWithWriter()
        {
            return this._retrievalWithWriterv21;
        }

        /// <summary>
        /// Gets the DSPL data controller for SDMX v2.0 SOAP or REST.
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
        //private DsplDataController<DsplTextWriter> GetDsplDataController(BaseDataFormat dataFormat, SdmxSchema sdmxSchema, DataflowPrincipal principal)
        //{
        //    DsplDataController<DsplTextWriter> dsplDataController;

        //    var dataWriterBuilder = new DsplDataWriterBuilder(dataFormat, sdmxSchema);
        //    var dsplDataResponseGenerator = new DsplDataResponseGenerator<DsplTextWriter>(this.GetDataRetrievalWithWriter(sdmxSchema.EnumType), dataWriterBuilder);
        //    dsplDataController = new DsplDataController<DsplTextWriter>(dsplDataResponseGenerator, new DataRequestValidator(dataFormat, sdmxSchema), principal);

        //    return dsplDataController;
        //}

        private DsplDataController<DsplTextWriter> GetDsplDataController(DataflowPrincipal principal)
        {
            DsplDataController<DsplTextWriter> dsplDataController;

            var dataWriterBuilder = new DsplDataWriterBuilder();
            var dsplDataResponseGenerator = new DsplDataResponseGenerator<DsplTextWriter>(this.GetDataRetrievalWithWriter(), dataWriterBuilder);
            dsplDataController = new DsplDataController<DsplTextWriter>(dsplDataResponseGenerator, new DsplDataFormat.Controller.DataRequestValidator(), principal);

            return dsplDataController;
        }



        #endregion
    }
}
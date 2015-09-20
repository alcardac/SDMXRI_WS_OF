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
namespace RDFProvider.Controller.Builder
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Nsi.AuthModule;    
    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;    
    using Estat.Sri.Ws.Controllers.Constants;    
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Manager;
    using Estat.Sri.Ws.Controllers.Builder;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using RDFProvider.Retriever;
    using RDFProvider.Retriever.Manager;
    using RDFProvider.Structure.Controllers.Builder;
    using RDFProvider.Structure.Manager.Output;
    

    /// <summary>
    ///     The controller builder.
    /// </summary>
    public class RDFControllerBuilder
    {
        #region Static Fields

        private static readonly ILog _log = LogManager.GetLogger(typeof(RDFControllerBuilder));

        private static readonly SdmxSchema _sdmxSchemaV21;

        #endregion

        #region Fields

        private readonly IAdvancedMutableStructureSearchManager _advancedMutableStructureSearchManager;

        private readonly IAuthAdvancedMutableStructureSearchManager _authAdvancedMutableStructureSearchManager;

        private readonly IAuthMutableStructureSearchManager _authMutableStructureSearchManagerV20;

        private readonly IAuthMutableStructureSearchManager _authMutableStructureSearchManagerV21;

        private readonly IMutableStructureSearchManager _mutableStructureSearchManagerV20;

        private readonly IMutableStructureSearchManager _mutableStructureSearchManagerV21;

        private readonly IRDFDataRetrievalWithWriter _retrievalWithWriterv21;

        #endregion

        #region Constructors and Destructors

        public RDFControllerBuilder()
            : this(SettingsManager.MappingStoreConnectionSettings, SettingsManager.Header)
        {
        }

        public RDFControllerBuilder(ConnectionStringSettings mappingStoreConnectionSettings, IHeader defaultHeader)
        {
            if (mappingStoreConnectionSettings == null)
            {
                _log.Error("No connection string defined. Please check the web.config.");
                throw new ArgumentNullException("mappingStoreConnectionSettings");
            }
            
            var dataRetrieverV21 = new RDFDataRetrieverCore(defaultHeader, mappingStoreConnectionSettings, SdmxSchemaEnumType.VersionTwoPointOne);
            this._retrievalWithWriterv21 = dataRetrieverV21;

            // structure search factories
            IStructureSearchManagerFactory<IAdvancedMutableStructureSearchManager> advancedFactory = new AdvancedMutableStructureSearchManagerFactory();
            IStructureSearchManagerFactory<IAuthAdvancedMutableStructureSearchManager> autAdvancedFactory = new AuthAdvancedMutableStructureSearchManagerFactory();

            IStructureSearchManagerFactory<IMutableStructureSearchManager> structureSearchManager = new MutableStructureSearchManagerFactory();
            IStructureSearchManagerFactory<IAuthMutableStructureSearchManager> autFactory = new AuthMutableStructureSearchManagerFactory();

            // advanced structure search managers 
            this._advancedMutableStructureSearchManager = advancedFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
            this._authAdvancedMutableStructureSearchManager = autAdvancedFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);

            this._mutableStructureSearchManagerV21 = structureSearchManager.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
            this._authMutableStructureSearchManagerV21 = autFactory.GetStructureSearchManager(mappingStoreConnectionSettings, _sdmxSchemaV21);
        }

        #endregion

        #region Public Methods and Operators

        public IController<IRestDataQuery, XmlWriter> BuildDataRest(DataflowPrincipal principal)
        {
            return this.GetSimpleDataController(principal);
        }

        public IController<IRestStructureQuery, XmlWriter> BuildQueryStructureRest(DataflowPrincipal principal)
        {
            
            

            IAuthMutableStructureSearchManager authMutableStructureSearchManager;
            IMutableStructureSearchManager mutableStructureSearchManager;
            IResponseGenerator<XmlWriter, ISdmxObjects> responseGenerator;

            IRDFWriterBuilder<IStructureRDFWriterManager, XmlWriter> structureRDFManagerBuilder = new StructureBuilder(WebServiceEndpoint.StandardEndpoint);

            authMutableStructureSearchManager = this._authMutableStructureSearchManagerV21;
            mutableStructureSearchManager = this._mutableStructureSearchManagerV21;            

            responseGenerator = new StructureResponseGenerator(structureRDFManagerBuilder);

            var structureRequestController = new StructureRequestRestController<XmlWriter>(responseGenerator, mutableStructureSearchManager, authMutableStructureSearchManager, principal);
            return structureRequestController;
        }

   

        #endregion

        #region Methods

        private IRDFDataRetrievalWithWriter GetDataRetrievalWithWriter()
        {

            return this._retrievalWithWriterv21;
         
        }
       
        private RDFSimpleDataController<XmlWriter> GetSimpleDataController(DataflowPrincipal principal)
        {
            RDFSimpleDataController<XmlWriter> simpleDataController;
                var dataWriterBuilder = new RDFDataWriterBuilder();
                var simpleDataResponseGenerator = new RDFSimpleDataResponseGenerator<XmlWriter>(this.GetDataRetrievalWithWriter(), dataWriterBuilder);
                simpleDataController = new RDFSimpleDataController<XmlWriter>(simpleDataResponseGenerator, new DataRequestValidator(), principal);
            

            return simpleDataController;
        }

        #endregion
    }
}
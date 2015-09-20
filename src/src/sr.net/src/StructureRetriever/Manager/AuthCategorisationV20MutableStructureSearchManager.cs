// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthCategorisationV20MutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2013-06-17
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The categorisation SDMX v2.0 mutable structure search manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     The categorisation SDMX v2.0 mutable structure search manager.
    /// </summary>
    public class AuthCategorisationV20MutableStructureSearchManager : AuthMutableStructureSearchManagerBase
    {
        #region Static Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthCategorisationV20MutableStructureSearchManager));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCategorisationV20MutableStructureSearchManager"/> class. 
        /// </summary>
        /// <param name="mutableRetrievalManagerFactory">
        /// The mutable retrieval manager factory.
        /// </param>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthCategorisationV20MutableStructureSearchManager(
            IAuthAdvancedMutableRetrievalManagerFactory mutableRetrievalManagerFactory, IAuthCrossRetrievalManagerFactory crossReferenceManager, ConnectionStringSettings connectionStringSettings)
            : base(mutableRetrievalManagerFactory, crossReferenceManager, connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCategorisationV20MutableStructureSearchManager"/> class. 
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthCategorisationV20MutableStructureSearchManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCategorisationV20MutableStructureSearchManager"/> class. 
        /// </summary>
        /// <param name="fullRetrievalManager">
        /// The full retrieval manager. Used for <see cref="StructureQueryDetailEnumType.Full"/> or
        ///     <see cref="StructureQueryDetailEnumType.ReferencedStubs"/>
        /// </param>
        /// <param name="crossReferenceManager">
        /// The cross reference manager. Set it to be able to retrieve cross references. Used for
        ///     <see cref="StructureQueryDetailEnumType.Full"/>
        ///     .
        /// </param>
        public AuthCategorisationV20MutableStructureSearchManager(IAuthSdmxMutableObjectRetrievalManager fullRetrievalManager, IAuthCrossRetrievalManagerFactory crossReferenceManager)
            : base(fullRetrievalManager, crossReferenceManager)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve the <paramref name="queries"/> and populate the <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="mutableObjects">
        ///     The mutable objects.
        /// </param>
        /// <param name="queries">
        ///     The structure queries
        /// </param>
        /// <param name="returnLatest">
        ///     Set to <c>true</c> to retrieve the latest; otherwise set to <c>false</c> to retrieve all versions
        /// </param>
        /// <param name="returnStub">
        ///     Set to <c>true</c> to retrieve artefacts as stubs; otherwise set to <c>false</c> to retrieve full artefacts.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed dataflows.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        ///     The cross-reference manager
        /// </param>
        protected override void PopulateMutables(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager, IMutableObjects mutableObjects, IList<IStructureReference> queries, bool returnLatest, StructureQueryDetailEnumType returnStub, IList<IMaintainableRefObject> allowedDataflows, IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager)
        {
            var dataflowLessQueries = new List<IStructureReference>();
            var dataflowQueries = new List<IStructureReference>();
            foreach (var query in queries)
            {
                if (query.MaintainableStructureEnumType.EnumType == SdmxStructureEnumType.Dataflow)
                {
                    dataflowQueries.Add(query);
                }
                else
                {
                    dataflowLessQueries.Add(query);
                }
            }

            base.PopulateMutables(retrievalManager, mutableObjects, dataflowLessQueries, returnLatest, returnStub, allowedDataflows, crossReferenceMutableRetrievalManager);
            
            // get the latest for dataflows to emulate the intermediate SR behavior.
            base.PopulateMutables(retrievalManager, mutableObjects, dataflowQueries, true, returnStub, allowedDataflows, crossReferenceMutableRetrievalManager);
            if (queries.NeedsCategorisation())
            {
                _log.Info("SDMX v2.0 structure search manager used. Trying to retrieve categorisations all dataflows and categorisations.");
                IMutableObjects objects = new MutableObjectsImpl(mutableObjects.Dataflows);
                objects.AddIdentifiables(mutableObjects.CategorySchemes);

                // get categorisations
                IResolverFactory factory = new ResolverFactory();
                var resolver = factory.GetResolver(StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Parents), crossReferenceMutableRetrievalManager);
                resolver.ResolveReferences(objects, returnStub == StructureQueryDetailEnumType.AllStubs, allowedDataflows);

                // add them to mutable objects
                mutableObjects.AddIdentifiables(objects.Categorisations);
            }
        }

        #endregion
    }
}
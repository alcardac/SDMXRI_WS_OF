// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthMutableStructureSearchManagerBase.cs" company="Eurostat">
//   Date Created : 2013-09-24
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The AUTH mutable structure search manager base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Builder;
    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Factory;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The AUTH mutable structure search manager base class.
    /// </summary>
    public abstract class AuthMutableStructureSearchManagerBase : AuthAdvancedMutableStructureSearchManagerBase, IAuthMutableStructureSearchManager
    {
        #region Static Fields

        /// <summary>
        /// The _complex query builder
        /// </summary>
        private static readonly StructureQuery2ComplexQueryBuilder _complexQueryBuilder = new StructureQuery2ComplexQueryBuilder();

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthMutableStructureSearchManagerBase));

        /// <summary>
        ///     The _resolver factory
        /// </summary>
        private static readonly IResolverFactory _resolverFactory;

        #endregion

        #region Fields

        /// <summary>
        /// The advanced retrieval advanced manager
        /// </summary>
        private readonly IAuthAdvancedSdmxMutableObjectRetrievalManager _retrievalAdvancedManager;

        /// <summary>
        /// The advanced mutable retrieval manager factory
        /// </summary>
        private readonly IAuthAdvancedMutableRetrievalManagerFactory _advancedMutableRetrievalManagerFactory;

        /// <summary>
        ///     The cross reference manager.
        /// </summary>
        private readonly IAuthCrossRetrievalManagerFactory _crossReferenceManager;

        /// <summary>
        ///     The _database.
        /// </summary>
        private readonly Database _database;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="AuthMutableStructureSearchManagerBase" /> class.
        /// </summary>
        static AuthMutableStructureSearchManagerBase()
        {
            _resolverFactory = new ResolverFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManagerBase"/> class.
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
        protected AuthMutableStructureSearchManagerBase(
            IAuthAdvancedMutableRetrievalManagerFactory mutableRetrievalManagerFactory, 
            IAuthCrossRetrievalManagerFactory crossReferenceManager, 
            ConnectionStringSettings connectionStringSettings)
        {
            this._advancedMutableRetrievalManagerFactory = mutableRetrievalManagerFactory ?? new AuthAdvancedMutableRetrievalManagerFactory();
            this._crossReferenceManager = crossReferenceManager ?? new AuthCrossMutableRetrievalManagerFactory();
            var database = new Database(connectionStringSettings);
            this._advancedMutableRetrievalManagerFactory.GetRetrievalManager(database);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManagerBase"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        protected AuthMutableStructureSearchManagerBase(ConnectionStringSettings connectionStringSettings)
        {
            this._database = new Database(connectionStringSettings);
            this._crossReferenceManager = new AuthCrossMutableRetrievalManagerFactory();

            // advanced
            this._advancedMutableRetrievalManagerFactory = new AuthAdvancedMutableRetrievalManagerFactory();
            this._retrievalAdvancedManager = this._advancedMutableRetrievalManagerFactory.GetRetrievalManager(this._database);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManagerBase"/> class.
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
        protected AuthMutableStructureSearchManagerBase(IAuthSdmxMutableObjectRetrievalManager fullRetrievalManager, IAuthCrossRetrievalManagerFactory crossReferenceManager)
        {
            if (fullRetrievalManager == null)
            {
                throw new ArgumentNullException("fullRetrievalManager");
            }

            this._crossReferenceManager = crossReferenceManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the latest version of the maintainable for the given maintainable input
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable Object.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        public virtual IMaintainableMutableObject GetLatest(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (maintainableObject == null)
            {
                return null;
            }

            // Create a reference *without* the version, because we want the latest.
            IStructureReference reference = new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id, null, maintainableObject.StructureType);
            IMutableObjects objects = new MutableObjectsImpl();
            this.PopulateMutables(objects, new[] { reference }, true, StructureQueryDetailEnumType.Full, allowedDataflows);
            var maintainable = objects.GetMaintainables(maintainableObject.StructureType).FirstOrDefault();

            if (maintainable == null)
            {
                throw new SdmxNoResultsException("No structures found for the specific query");
            }

            return maintainable;
        }

        /// <summary>
        /// Returns a set of maintainable that match the given query parameters
        /// </summary>
        /// <param name="structureQuery">
        /// The structure Query.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="structureQuery"/> is null
        /// </exception>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        public virtual IMutableObjects GetMaintainables(IRestStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, "Query for maintainable artefact(s): {0}", structureQuery);
            if (structureQuery == null)
            {
                throw new ArgumentNullException("structureQuery");
            }

            IMutableObjects mutableObjects = new MutableObjectsImpl();

            var cachedRetrievalManager = this._advancedMutableRetrievalManagerFactory.GetRetrievalManager(this._retrievalAdvancedManager);
            var crossReferenceMutableRetrievalManager = this._crossReferenceManager.GetCrossRetrievalManager(this._database, cachedRetrievalManager);
            this.PopulateMutables(
                cachedRetrievalManager, 
                mutableObjects, 
                new[] { structureQuery.StructureReference }, 
                structureQuery.StructureQueryMetadata.IsReturnLatest, 
                structureQuery.StructureQueryMetadata.StructureQueryDetail, 
                allowedDataflows, 
                crossReferenceMutableRetrievalManager);

            GetDetails(structureQuery, mutableObjects, crossReferenceMutableRetrievalManager, allowedDataflows);

            if (mutableObjects.AllMaintainables.Count == 0)
            {
                throw new SdmxNoResultsException("No structures found for the specific query");
            }

            return mutableObjects;
        }

        /// <summary>
        /// Retrieves all structures that match the given query parameters in the list of query objects.  The list
        ///     must contain at least one StructureQueryObject.
        /// </summary>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="resolveReferences">
        /// - if set to true then any cross referenced structures will also be available in the SdmxObjects container
        /// </param>
        /// <param name="returnStub">
        /// - if set to true then only stubs of the returned objects will be returned.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        public virtual IMutableObjects RetrieveStructures(IList<IStructureReference> queries, bool resolveReferences, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            IMutableObjects mutableObjects = new MutableObjectsImpl();

            var cachedRetrievalManager = this._advancedMutableRetrievalManagerFactory.GetRetrievalManager(this._retrievalAdvancedManager);

            var crossReferenceMutableRetrievalManager = this._crossReferenceManager.GetCrossRetrievalManager(this._database, cachedRetrievalManager);
            this.PopulateMutables(cachedRetrievalManager, mutableObjects, queries, false, returnStub.GetStructureQueryDetail(), allowedDataflows, crossReferenceMutableRetrievalManager);

            if (resolveReferences)
            {
                var resolver = _resolverFactory.GetResolver(StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Children), crossReferenceMutableRetrievalManager);
                resolver.ResolveReferences(mutableObjects, returnStub, allowedDataflows);
            }

            if (mutableObjects.AllMaintainables.Count == 0)
            {
                throw new SdmxNoResultsException("No structures found for the specific query");
            }

            return mutableObjects;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve the structures referenced by <paramref name="queries"/> and populate the
        ///     <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="mutableObjects">
        ///     The mutable objects.
        /// </param>
        /// <param name="queries">
        ///     The queries.
        /// </param>
        /// <param name="returnLatest">
        ///     The return Latest.
        /// </param>
        /// <param name="returnStub">
        ///     The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        protected virtual void PopulateMutables(IMutableObjects mutableObjects, IList<IStructureReference> queries, bool returnLatest, StructureQueryDetailEnumType returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            var cachedRetrievalManager = this._advancedMutableRetrievalManagerFactory.GetRetrievalManager(this._retrievalAdvancedManager);

            var crossReferenceMutableRetrievalManager = this._crossReferenceManager.GetCrossRetrievalManager(this._database, cachedRetrievalManager);
            PopulateMutables(cachedRetrievalManager, mutableObjects, queries, returnLatest, returnStub, allowedDataflows, crossReferenceMutableRetrievalManager);
        }

        /// <summary>
        /// Retrieve the structures referenced by <paramref name="queries"/> and populate the
        ///     <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="mutableObjects">
        ///     The mutable objects.
        /// </param>
        /// <param name="queries">
        ///     The queries.
        /// </param>
        /// <param name="returnLatest">
        ///     The return Latest.
        /// </param>
        /// <param name="returnStub">
        ///     The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        ///     The cross reference mutable retrieval manager.
        /// </param>
        protected virtual void PopulateMutables(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager, IMutableObjects mutableObjects, IList<IStructureReference> queries, bool returnLatest, StructureQueryDetailEnumType returnStub, IList<IMaintainableRefObject> allowedDataflows, IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager)
        {
            //// changes here might also apply to AuthAdvancedMutableStructureSearchManagerBase 
            for (int i = 0; i < queries.Count; i++)
            {
                var structureReference = queries[i];
                IRestStructureQuery restStructureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(returnStub), StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None), null, structureReference, returnLatest);
                var complexStructureQuery = _complexQueryBuilder.Build(restStructureQuery);
                base.PopulateMutables(retrievalManager, mutableObjects, complexStructureQuery, allowedDataflows, crossReferenceMutableRetrievalManager);
            }
        }

        /// <summary>
        /// Get details specified in <paramref name="structureQuery"/> of the specified <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        /// The cross Reference Mutable Retrieval Manager.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// Not implemented value at <see cref="StructureReferenceDetail"/> of <paramref name="structureQuery"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Invalid value at <see cref="StructureReferenceDetail"/> of <paramref name="structureQuery"/>
        /// </exception>
        private static void GetDetails(
            IRestStructureQuery structureQuery, 
            IMutableObjects mutableObjects, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflow)
        {
            _log.InfoFormat("Reference detail: {0}", structureQuery.StructureQueryMetadata.StructureReferenceDetail);

            bool returnStub = structureQuery.StructureQueryMetadata.StructureQueryDetail.EnumType != StructureQueryDetailEnumType.Full;
            var resolver = _resolverFactory.GetResolver(
                structureQuery.StructureQueryMetadata.StructureReferenceDetail, 
                crossReferenceMutableRetrievalManager, 
                structureQuery.StructureQueryMetadata.SpecificStructureReference);
            resolver.ResolveReferences(mutableObjects, returnStub, allowedDataflow);
        }

        #endregion
    }
}
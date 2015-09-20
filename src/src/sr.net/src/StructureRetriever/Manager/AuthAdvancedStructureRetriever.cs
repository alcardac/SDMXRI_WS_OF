// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthAdvancedStructureRetriever.cs" company="Eurostat">
//   Date Created : 2013-09-20
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The auth advanced mutable structure search manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Factory;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    /// The AUTH advanced mutable structure search manager.
    /// </summary>
    public class AuthAdvancedStructureRetriever : AuthAdvancedMutableStructureSearchManagerBase, IAuthAdvancedMutableStructureSearchManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthAdvancedStructureRetriever));

        /// <summary>
        ///     The _resolver factory
        /// </summary>
        private static readonly IResolverFactory _resolverFactory;

        #endregion

        #region Fields

        /// <summary>
        /// The _cross reference manager factory.
        /// </summary>
        private readonly IAuthCrossRetrievalManagerFactory _crossReferenceManagerFactory;

        /// <summary>
        /// The _retrieval manager.
        /// </summary>
        private readonly IAuthAdvancedSdmxMutableObjectRetrievalManager _retrievalManager;

        /// <summary>
        /// The _database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// The _retrieval factory.
        /// </summary>
        private readonly IAuthAdvancedMutableRetrievalManagerFactory _retrievalFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="AuthAdvancedStructureRetriever" /> class.
        /// </summary>
        static AuthAdvancedStructureRetriever()
        {
            _resolverFactory = new ResolverFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthAdvancedStructureRetriever(ConnectionStringSettings connectionStringSettings)
            : this(null, null, connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="mutableRetrievalManagerFactory">
        /// The mutable retrieval manager factory.
        /// </param>
        /// <param name="crossReferenceManagerFactory">
        /// The cross reference manager factory.
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthAdvancedStructureRetriever(
            IAuthAdvancedMutableRetrievalManagerFactory mutableRetrievalManagerFactory, 
            IAuthCrossRetrievalManagerFactory crossReferenceManagerFactory, 
            ConnectionStringSettings connectionStringSettings)
        {
            this._crossReferenceManagerFactory = crossReferenceManagerFactory ?? new AuthCrossMutableRetrievalManagerFactory();
            this._retrievalFactory = mutableRetrievalManagerFactory ?? new AuthAdvancedMutableRetrievalManagerFactory();
            this._database = new Database(connectionStringSettings);
            this._retrievalManager = this._retrievalFactory.GetRetrievalManager(this._database);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Process the specified <paramref name="structureQuery"/> returning an
        ///     <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.IMutableObjects"/> container which contains the Maintainable
        ///     Structure hat correspond to the <paramref name="structureQuery"/> query parameters.
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.IMutableObjects"/>.
        /// </returns>
        public IMutableObjects GetMaintainables(IComplexStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (structureQuery == null)
            {
                throw new ArgumentNullException("structureQuery");
            }

            IMutableObjects mutableObjects = new MutableObjectsImpl();

            var cachedRetrievalManager = this._retrievalFactory.GetRetrievalManager(this._retrievalManager);
            var crossReferenceMutableRetrievalManager = this._crossReferenceManagerFactory.GetCrossRetrievalManager(this._database, cachedRetrievalManager);

            this.PopulateMutables(cachedRetrievalManager, mutableObjects, structureQuery, allowedDataflows, crossReferenceMutableRetrievalManager);

            GetDetails(structureQuery, mutableObjects, crossReferenceMutableRetrievalManager, allowedDataflows);
            mutableObjects.DataStructures.NormalizeSdmxv20DataStructures();

            if (mutableObjects.AllMaintainables.Count == 0)
            {
                throw new SdmxNoResultsException("No structures found for the specific query");
            }

            return mutableObjects;
        }

        #endregion

        #region Methods

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
            IComplexStructureQuery structureQuery, 
            IMutableObjects mutableObjects, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflow)
        {
            _log.InfoFormat("Reference detail: {0}", structureQuery.StructureQueryMetadata.StructureReferenceDetail);

            bool returnStub = structureQuery.StructureQueryMetadata.StructureQueryDetail.EnumType != ComplexStructureQueryDetailEnumType.Full;
            StructureReferenceDetail referenceDetail = structureQuery.StructureQueryMetadata.StructureReferenceDetail ?? StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);
            var specificStructureTypes = structureQuery.StructureQueryMetadata.ReferenceSpecificStructures != null ? structureQuery.StructureQueryMetadata.ReferenceSpecificStructures.ToArray() : null;

            var resolver = _resolverFactory.GetResolver(referenceDetail, crossReferenceMutableRetrievalManager, specificStructureTypes);
            resolver.ResolveReferences(mutableObjects, returnStub, allowedDataflow);
        }

        #endregion
    }
}
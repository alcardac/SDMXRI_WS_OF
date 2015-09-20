// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthStructureRetrieverV21.cs" company="Eurostat">
//   Date Created : 2013-06-17
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <c>SDMX v2.1</c> mutable structure search manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    ///     The <c>SDMX v2.1</c> mutable structure search manager. Use with REST
    /// </summary>
    public class AuthStructureRetrieverV21 : AuthMutableStructureSearchManagerBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStructureRetrieverV21"/> class. 
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
        public AuthStructureRetrieverV21(
            IAuthAdvancedMutableRetrievalManagerFactory mutableRetrievalManagerFactory, IAuthCrossRetrievalManagerFactory crossReferenceManager, ConnectionStringSettings connectionStringSettings)
            : base(mutableRetrievalManagerFactory, crossReferenceManager, connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStructureRetrieverV21"/> class. 
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthStructureRetrieverV21(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStructureRetrieverV21"/> class. 
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
        public AuthStructureRetrieverV21(IAuthSdmxMutableObjectRetrievalManager fullRetrievalManager, IAuthCrossRetrievalManagerFactory crossReferenceManager)
            : base(fullRetrievalManager, crossReferenceManager)
        {
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
        public override IMaintainableMutableObject GetLatest(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows)
        {
            var maintainableMutableObject = base.GetLatest(maintainableObject, allowedDataflows);
            maintainableMutableObject.NormalizeSdmxv20DataStructure();

            return maintainableMutableObject;
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
        public override IMutableObjects GetMaintainables(IRestStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows)
        {
            var mutableObjects = base.GetMaintainables(structureQuery, allowedDataflows);
            mutableObjects.DataStructures.NormalizeSdmxv20DataStructures();
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
        public override IMutableObjects RetrieveStructures(IList<IStructureReference> queries, bool resolveReferences, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            var mutableObjects = base.RetrieveStructures(queries, resolveReferences, returnStub, allowedDataflows);
            mutableObjects.DataStructures.NormalizeSdmxv20DataStructures();
            return mutableObjects;
        }

        #endregion
    }
}
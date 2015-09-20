// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthMutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2013-07-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mutable structure search manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The mutable structure search manager implementation.
    /// </summary>
    public class AuthMutableStructureSearchManager : AuthMutableStructureSearchManagerBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManager"/> class.
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
        public AuthMutableStructureSearchManager(
            IAuthAdvancedMutableRetrievalManagerFactory mutableRetrievalManagerFactory, 
            IAuthCrossRetrievalManagerFactory crossReferenceManager, 
            ConnectionStringSettings connectionStringSettings)
            : base(mutableRetrievalManagerFactory, crossReferenceManager, connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public AuthMutableStructureSearchManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManager"/> class.
        /// </summary>
        /// <param name="fullRetrievalManager">
        /// The full retrieval manager. Used for <see cref="StructureQueryDetail.Full"/> or
        ///     <see cref="StructureQueryDetail.ReferencedStubs"/>
        /// </param>
        /// <param name="crossReferenceManager">
        /// The cross reference manager. Set it to be able to retrieve cross references. Used for
        ///     <see cref="StructureQueryDetail.Full"/>
        ///     .
        /// </param>
        public AuthMutableStructureSearchManager(IAuthSdmxMutableObjectRetrievalManager fullRetrievalManager, IAuthCrossRetrievalManagerFactory crossReferenceManager)
            : base(fullRetrievalManager, crossReferenceManager)
        {
        }

        #endregion
    }
}
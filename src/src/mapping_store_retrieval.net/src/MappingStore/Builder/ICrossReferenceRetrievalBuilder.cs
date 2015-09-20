// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceRetrievalBuilder.cs" company="Eurostat">
//   Date Created : 2013-03-04
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    /// The <see cref="ICrossReferenceMutableRetrievalManager"/> builder interface.
    /// </summary>
    public interface ICrossReferenceRetrievalBuilder : IBuilder<ICrossReferenceMutableRetrievalManager, ISdmxMutableObjectRetrievalManager>
    {
        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving stub artefacts
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        ICrossReferenceMutableRetrievalManager BuildStub(ISdmxMutableObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving stub artefacts
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="retrievalAuthManager">The authorization aware retrieval manager</param>
        /// <returns>
        /// The <see cref="IAuthCrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        IAuthCrossReferenceMutableRetrievalManager BuildStub(ISdmxMutableObjectRetrievalManager retrievalManager, IAuthSdmxMutableObjectRetrievalManager retrievalAuthManager);

        /// <summary>
        /// Build a <see cref="ICrossReferenceMutableRetrievalManager"/> from the specified <paramref name="retrievalManager"/> for retrieving full artefacts
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="retrievalAuthManager">The authorization aware retrieval manager</param>
        /// <returns>
        /// The <see cref="IAuthCrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        IAuthCrossReferenceMutableRetrievalManager Build(ISdmxMutableObjectRetrievalManager retrievalManager, IAuthSdmxMutableObjectRetrievalManager retrievalAuthManager);
    }
}
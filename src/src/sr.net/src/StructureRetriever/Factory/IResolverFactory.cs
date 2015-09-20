// -----------------------------------------------------------------------
// <copyright file="IResolverFactory.cs" company="Eurostat">
//   Date Created : 2013-09-16
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Factory
{
    using Estat.Nsi.StructureRetriever.Engines.Resolver;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The <see cref="IResolver"/> Factory interface.
    /// </summary>
    public interface IResolverFactory
    {
        /// <summary>
        /// Returns the <see cref="IResolver" /> for the specified <paramref name="referenceDetailType"/>.
        /// </summary>
        /// <param name="referenceDetailType">Type of the reference detail.</param>
        /// <param name="crossReferenceManager">The cross reference manager.</param>
        /// <param name="specificStructureTypes">The specific object structure types.</param>
        /// <returns>The <see cref="IResolver" /> for the specified <paramref name="referenceDetailType"/>; otherwise null</returns>
        IResolver GetResolver(StructureReferenceDetail referenceDetailType, IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, params SdmxStructureType[] specificStructureTypes);
    }
}
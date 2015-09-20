// -----------------------------------------------------------------------
// <copyright file="IExternalReferenceRetrievalManager.cs" company="Eurostat">
//   Date Created : 2014-03-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// This interface is responsible for providing the capabilities of resolving externally referenced structures
    /// </summary>
    public interface IExternalReferenceRetrievalManager
    {
        /// <summary>
        /// Resolves the full structure from the specified stub structure.
        /// </summary>
        /// <param name="externalStructure">The external structure.</param>
        /// <returns>The full structure from the stub structure</returns>
        /// <remarks>If the external structure has IsExternalReference.IsTrue == false, then it is not externally maintained and no 
        /// action will be taken, the same structure will be passed back.  Otherwise the external structure will be resolved using
        /// the StructureURL or ServiceURL obtained from the structure</remarks>
        /// <exception cref="SdmxNoResultsException">if the maintainable could not be resolved from the given endpoint</exception>
        IMaintainableObject ResolveFullStructure(IMaintainableObject externalStructure);
    }
}
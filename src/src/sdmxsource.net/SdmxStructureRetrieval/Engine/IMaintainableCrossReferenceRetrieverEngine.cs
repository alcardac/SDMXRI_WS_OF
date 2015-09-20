// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableCrossReferenceRetrieverEngine.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Given a maintainable structure, this will return a set of all the CrossReferenceBean structures that it uses to reference
//   other structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Engine
{
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    public interface IMaintainableCrossReferenceRetrieverEngine
    {
        /// <summary>
        /// Returns the set of cross referenced structures from this maintainable.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="maintainable">
        /// The maintainable object
        /// </param>
        /// <returns>
        /// The cross references set.
        /// </returns>
        ISet<ICrossReference> GetCrossReferences(ISdmxObjectRetrievalManager retrievalManager, IMaintainableObject maintainable);
    }
}

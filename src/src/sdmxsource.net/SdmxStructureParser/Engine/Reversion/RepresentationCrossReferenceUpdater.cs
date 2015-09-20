// -----------------------------------------------------------------------
// <copyright file="RepresentationCrossReferenceUpdater.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class RepresentationCrossReferenceUpdater
    {
        /// <summary>
        /// The update representation reference.
        /// </summary>
        /// <param name="representation">
        /// The representation.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        protected void UpdateRepresentationReference(IRepresentationMutableObject representation, IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (representation != null && representation.Representation != null)
            {
                IStructureReference mapTo = updateReferences[representation.Representation];
                if (mapTo != null)
                {
                    representation.Representation = mapTo;
                }
            }
        }
    }
}

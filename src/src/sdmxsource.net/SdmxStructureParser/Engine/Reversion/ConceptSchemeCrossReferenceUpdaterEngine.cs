// -----------------------------------------------------------------------
// <copyright file="ConceptSchemeCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ConceptSchemeCrossReferenceUpdaterEngine : RepresentationCrossReferenceUpdater,
                                                                IConceptSchemeCrossReferenceUpdaterEngine
    {
        /// <summary>
        /// The update references.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        /// <param name="newVersionNumber">
        /// The new version number.
        /// </param>
        /// <returns>
        /// The <see cref="IConceptSchemeObject"/>.
        /// </returns>
        public IConceptSchemeObject UpdateReferences(
            IConceptSchemeObject maintainable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IConceptSchemeMutableObject conceptScheme = maintainable.MutableInstance;
            if (!conceptScheme.Id.Equals(ConceptSchemeObject.DefaultSchemeId))
                conceptScheme.Version = newVersionNumber;
            foreach (IConceptMutableObject concept in conceptScheme.Items)
            {
                this.UpdateRepresentationReference(concept.CoreRepresentation, updateReferences);
            }
            return conceptScheme.ImmutableInstance;
        }
    }
}

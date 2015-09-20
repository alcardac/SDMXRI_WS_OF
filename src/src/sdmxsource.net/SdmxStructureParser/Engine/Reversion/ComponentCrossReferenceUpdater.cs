// -----------------------------------------------------------------------
// <copyright file="ComponentCrossReferenceUpdater.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public abstract class ComponentCrossReferenceUpdater : RepresentationCrossReferenceUpdater
    {
        /// <summary>
        /// The update component referneces.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        public void UpdateComponentReferneces<T>(
            IEnumerable<T> components, IDictionary<IStructureReference, IStructureReference> updateReferences) where T : IComponentMutableObject
        {
            foreach (T currentComponent in components)
            {
                UpdateComponentReferneces(currentComponent, updateReferences);
            }
        }

        /// <summary>
        /// The update component referneces.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        public void UpdateComponentReferneces(
            IComponentMutableObject component, IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            // Update the concept reference
            IStructureReference mapTo = updateReferences[component.ConceptRef];
            if (mapTo != null)
            {
                component.ConceptRef = mapTo;
            }
            // Update the codelist reference
            UpdateRepresentationReference(component.Representation, updateReferences);
        }
    }
}

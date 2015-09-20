// -----------------------------------------------------------------------
// <copyright file="HierarchicCodelistCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class HierarchicCodelistCrossReferenceUpdaterEngine : IHierarchicCodelistCrossReferenceUpdaterEngine
    {
        /// <summary>
        /// The update references.
        /// </summary>
        /// <param name="maintianable">
        /// The maintianable.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        /// <param name="newVersionNumber">
        /// The new version number.
        /// </param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistObject"/>.
        /// </returns>
        public IHierarchicalCodelistObject UpdateReferences(
            IHierarchicalCodelistObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IHierarchicalCodelistMutableObject codelistMutableObject = maintianable.MutableInstance;
            codelistMutableObject.Version = newVersionNumber;

            if (codelistMutableObject.CodelistRef != null)
            {
                foreach (ICodelistRefMutableObject codelistRef in codelistMutableObject.CodelistRef)
                {
                    if (codelistRef.CodelistReference != null)
                    {
                        IStructureReference structureReference = updateReferences[codelistRef.CodelistReference];
                        if (structureReference != null)
                        {
                            codelistRef.CodelistReference = structureReference;
                        }
                    }
                }
            }
            if (codelistMutableObject.Hierarchies != null)
            {
                foreach (IHierarchyMutableObject hierarchyMutable in codelistMutableObject.Hierarchies)
                {
                    this.UpdateCodeRefs(hierarchyMutable.HierarchicalCodeObjects, updateReferences);
                }
            }
            return codelistMutableObject.ImmutableInstance;
        }

        /// <summary>
        /// The update code refs.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        private void UpdateCodeRefs(
            IList<ICodeRefMutableObject> codeRef, IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (codeRef != null)
            {
                foreach (ICodeRefMutableObject currentCodeRef in codeRef)
                {
                    this.UpdateCodeRefs(currentCodeRef.CodeRefs, updateReferences);

                    if (currentCodeRef.CodeReference != null)
                    {
                        IStructureReference structureReference = updateReferences[currentCodeRef.CodeReference];
                        if (structureReference != null)
                        {
                            currentCodeRef.CodeReference = structureReference;
                        }
                    }
                }
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="MetadataStructureCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class MetadataStructureCrossReferenceUpdaterEngine : RepresentationCrossReferenceUpdater,
                                                                IMetadataStructureCrossReferenceUpdaterEngine
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
        /// The <see cref="IMetadataStructureDefinitionObject"/>.
        /// </returns>
        public IMetadataStructureDefinitionObject UpdateReferences(
            IMetadataStructureDefinitionObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IMetadataStructureDefinitionMutableObject msdMutable = maintianable.MutableInstance;
            msdMutable.Version = newVersionNumber;

            if (msdMutable.MetadataTargets != null)
            {
                foreach (IMetadataTargetMutableObject metadataTarget in msdMutable.MetadataTargets)
                {
                    if (metadataTarget.IdentifiableTarget != null)
                    {
                        foreach (
                            IIdentifiableTargetMutableObject identifiableTarget in
                                metadataTarget.IdentifiableTarget)
                        {
                            UpdateRepresentationReference(identifiableTarget.Representation, updateReferences);

                            if (identifiableTarget.ConceptRef != null)
                            {
                                IStructureReference structureReference = updateReferences[identifiableTarget.ConceptRef];
                                if (structureReference != null)
                                {
                                    identifiableTarget.ConceptRef = structureReference;
                                }
                            }
                        }
                    }
                }
            }
            if (msdMutable.ReportStructures != null)
            {
                foreach (IReportStructureMutableObject rs in msdMutable.ReportStructures)
                {
                    this.UpdateMetadataAttributes(rs.MetadataAttributes, updateReferences);
                }
            }
            return msdMutable.ImmutableInstance;
        }

        /// <summary>
        /// The update metadata attributes.
        /// </summary>
        /// <param name="metadataAttributes">
        /// The metadata attributes.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        private void UpdateMetadataAttributes(
            IList<IMetadataAttributeMutableObject> metadataAttributes,
            IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (metadataAttributes != null)
            {
                foreach (IMetadataAttributeMutableObject currentMa in metadataAttributes)
                {
                    UpdateRepresentationReference(currentMa.Representation, updateReferences);

                    if (currentMa.ConceptRef != null)
                    {
                        IStructureReference structureReference = updateReferences[currentMa.ConceptRef];
                        if (structureReference != null)
                        {
                            currentMa.ConceptRef = structureReference;
                        }
                    }
                }
            }
        }
    }
}

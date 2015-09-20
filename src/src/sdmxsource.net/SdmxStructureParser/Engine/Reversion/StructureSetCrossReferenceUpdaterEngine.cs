// -----------------------------------------------------------------------
// <copyright file="StructureSetCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class StructureSetCrossReferenceUpdaterEngine : IStructureSetCrossReferenceUpdaterEngine
    {
        public IStructureSetObject UpdateReferences(
            IStructureSetObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            String newVersionNumber)
        {
            IStructureSetMutableObject ss = maintianable.MutableInstance;
            ss.Version = newVersionNumber;

            this.UpdateSchemeMap(ss.CategorySchemeMapList, updateReferences);
            this.UpdateSchemeMap(ss.CodelistMapList, updateReferences);
            this.UpdateSchemeMap(ss.ConceptSchemeMapList, updateReferences);
            this.UpdateSchemeMap(ss.OrganisationSchemeMapList, updateReferences);
            this.UpdateSchemeMap(ss.StructureMapList, updateReferences);

            if (ss.RelatedStructures != null)
            {
                IRelatedStructuresMutableObject relatedStructures = ss.RelatedStructures;
                relatedStructures.CategorySchemeRef = this.UpdateRelatedStructures(
                    relatedStructures.CategorySchemeRef, updateReferences);
                relatedStructures.ConceptSchemeRef = this.UpdateRelatedStructures(
                    relatedStructures.ConceptSchemeRef, updateReferences);
                relatedStructures.HierCodelistRef = this.UpdateRelatedStructures(
                    relatedStructures.HierCodelistRef, updateReferences);
                relatedStructures.DataStructureRef = this.UpdateRelatedStructures(
                    relatedStructures.DataStructureRef, updateReferences);
                relatedStructures.MetadataStructureRef =
                    this.UpdateRelatedStructures(relatedStructures.MetadataStructureRef, updateReferences);
                relatedStructures.OrgSchemeRef = this.UpdateRelatedStructures(
                    relatedStructures.OrgSchemeRef, updateReferences);
            }

            return ss.ImmutableInstance;
        }

        private IList<IStructureReference> UpdateRelatedStructures(
            IList<IStructureReference> sRefList, IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            IList<IStructureReference> newReferences = new List<IStructureReference>();
            if (sRefList != null)
            {
                foreach (IStructureReference currentSRef in sRefList)
                {
                    IStructureReference updatedRef;
                    if (updateReferences.TryGetValue(currentSRef, out updatedRef))
                    {
                        newReferences.Add(updatedRef);
                    }
                    else
                    {
                        newReferences.Add(currentSRef);
                    }
                }
            }
            return newReferences;
        }

        private void UpdateSchemeMap<T>(
            IList<T> schemeMaps, IDictionary<IStructureReference, IStructureReference> updateReferences)
            where T : class, ISchemeMapMutableObject
        {
            if (schemeMaps != null)
            {
                foreach (T currentMap in schemeMaps)
                {
                    IStructureReference newTarget; 
                    if (updateReferences.TryGetValue(currentMap.SourceRef, out newTarget))
                    {
                        currentMap.SourceRef = newTarget;
                    }

                    if (updateReferences.TryGetValue(currentMap.TargetRef, out newTarget))
                    {
                        currentMap.TargetRef = newTarget;
                    }
                }
            }
        }
    }
}

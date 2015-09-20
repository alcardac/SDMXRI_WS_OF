// -----------------------------------------------------------------------
// <copyright file="ReportingTaxonomyBeanCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ReportingTaxonomyBeanCrossReferenceUpdaterEngine : IReportingTaxonomyBeanCrossReferenceUpdaterEngine
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
        /// The <see cref="IReportingTaxonomyObject"/>.
        /// </returns>
        public IReportingTaxonomyObject UpdateReferences(
            IReportingTaxonomyObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IReportingTaxonomyMutableObject reportingTaxonomy = maintianable.MutableInstance;
            reportingTaxonomy.Version = newVersionNumber;

            this.UpdateReportingCategories(reportingTaxonomy.Items, updateReferences);

            return reportingTaxonomy.ImmutableInstance;
        }

        /// <summary>
        /// The update reporting categories.
        /// </summary>
        /// <param name="reportingCategories">
        /// The reporting categories.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        private void UpdateReportingCategories(
            IList<IReportingCategoryMutableObject> reportingCategories,
            IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (reportingCategories != null)
            {
                foreach (IReportingCategoryMutableObject reportingCategory in reportingCategories)
                {
                    this.UpdateReportingCategories(reportingCategory.Items, updateReferences);
                    reportingCategory.ProvisioningMetadata =
                        this.UpdateRelatedStructures(reportingCategory.ProvisioningMetadata, updateReferences);
                    reportingCategory.StructuralMetadata =
                        this.UpdateRelatedStructures(reportingCategory.StructuralMetadata, updateReferences);
                }
            }
        }

        /// <summary>
        /// The update related structures.
        /// </summary>
        /// <param name="sRefList">
        /// The s ref list.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        private IList<IStructureReference> UpdateRelatedStructures(
            IList<IStructureReference> sRefList, IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            IList<IStructureReference> newReferences = new List<IStructureReference>();
            if (sRefList != null)
            {
                foreach (IStructureReference currentSRef in sRefList)
                {
                    IStructureReference updatedRef = updateReferences[currentSRef];
                    if (updatedRef != null)
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
    }
}

// -----------------------------------------------------------------------
// <copyright file="CrossReferenceReversionEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class CrossReferenceReversionEngine : ICrossReferenceReversionEngine
    {
        private ICategorisationCrossReferenceUpdaterEngine categorisationCrossReferenceUpdaterEngine = new CategorisationCrossReferenceUpdaterEngine();

        private IConceptSchemeCrossReferenceUpdaterEngine conceptSchemeCrossReferenceUpdaterEngine = new ConceptSchemeCrossReferenceUpdaterEngine();

        private IDataflowCrossReferenceUpdaterEngine dataflowCrossReferenceUpdaterEngine = new DataflowCrossReferenceUpdaterEngine();

        private IDataStructureCrossReferenceUpdaterEngine dataStructureCrossReferenceUpdaterEngine = new DataStructureCrossReferenceUpdaterEngine();

        private IHierarchicCodelistCrossReferenceUpdaterEngine hierarchicCodelistCrossReferenceUpdaterEngine = new HierarchicCodelistCrossReferenceUpdaterEngine();

        private IMetadataflowCrossReferenceUpdaterEngine metadataflowCrossReferenceUpdaterEngine = new MetadataflowCrossReferenceUpdaterEngine();

        private IMetadataStructureCrossReferenceUpdaterEngine metadataStructureCrossReferenceUpdaterEngine = new MetadataStructureCrossReferenceUpdaterEngine();

        private IProcessCrossReferenceUpdater processCrossReferenceUpdater = new ProcessCrossReferenceUpdater();

        private IProvisionCrossReferenceUpdaterEngine provisionCrossReferenceUpdaterEngine = new ProvisionCrossReferenceUpdaterEngine();

        private IStructureSetCrossReferenceUpdaterEngine structureSetCrossReferenceUpdaterEngine = new StructureSetCrossReferenceUpdaterEngine();

        private IReportingTaxonomyBeanCrossReferenceUpdaterEngine reportingTaxonomyBeanCrossReferenceUpdaterEngine = new ReportingTaxonomyBeanCrossReferenceUpdaterEngine();

        /// <summary>
        /// </summary>
        /// <param name="structures"> The structures</param>
        /// <param name="resolveAgencies"> Flag indicating resolve agencies .</param>
        /// <param name="resolutionDepth"> The resolution depth. </param>
        /// <param name="retrievalManager"> The retrieval manager </param>
        /// <returns> The references </returns>
        public IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> ResolveReferences(
            ISdmxObjects structures,
            bool resolveAgencies,
            int resolutionDepth,
            IIdentifiableRetrievalManager retrievalManager)
        {
            ICrossReferenceResolverEngine crossReferenceResolver = new CrossReferenceResolverEngineCore();
            return crossReferenceResolver.ResolveReferences(
                structures, resolveAgencies, resolutionDepth, retrievalManager);
        }

        public IMaintainableObject UdpateReferences(
            IMaintainableObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            switch (maintianable.StructureType.EnumType)
            {
                case SdmxStructureEnumType.AttachmentConstraint:
                    break;
                case SdmxStructureEnumType.Categorisation:
                    return
                        this.categorisationCrossReferenceUpdaterEngine.UpdateReferences(
                            (ICategorisationObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.ConceptScheme:
                    return
                        this.conceptSchemeCrossReferenceUpdaterEngine.UpdateReferences(
                            (IConceptSchemeObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.ContentConstraint:
                    break;
                case SdmxStructureEnumType.Dataflow:
                    return this.dataflowCrossReferenceUpdaterEngine.UpdateReferences(
                        (IDataflowObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.Dsd:
                    return
                        this.dataStructureCrossReferenceUpdaterEngine.UpdateReferences(
                            (IDataStructureObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return
                        this.hierarchicCodelistCrossReferenceUpdaterEngine.UpdateReferences(
                            (IHierarchicalCodelistObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.metadataflowCrossReferenceUpdaterEngine.UpdateReferences(
                        (IMetadataFlow)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.Msd:
                    return
                        this.metadataStructureCrossReferenceUpdaterEngine.UpdateReferences(
                            (IMetadataStructureDefinitionObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.Process:
                    return this.processCrossReferenceUpdater.UpdateReferences(
                        (IProcessObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return
                        this.provisionCrossReferenceUpdaterEngine.UpdateReferences(
                            (IProvisionAgreementObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return
                        this.reportingTaxonomyBeanCrossReferenceUpdaterEngine.UpdateReferences(
                            (IReportingTaxonomyObject)maintianable, updateReferences, newVersionNumber);
                case SdmxStructureEnumType.StructureSet:
                    return
                        this.structureSetCrossReferenceUpdaterEngine.UpdateReferences(
                            (IStructureSetObject)maintianable, updateReferences, newVersionNumber);
            }

            return maintianable;
        }
    }
}


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsV21Builder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx beans v 21 builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util;

    using DataStructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DataStructureType;
    using MetadataStructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataStructureType;
    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.StructureType;

    /// <summary>
    ///     The sdmx beans v 21 builder.
    /// </summary>
    public class SdmxObjectsV21Builder : AbstractSdmxObjectsBuilder
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //////////            PROCESS 2.1  MESSAGES                        ///////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// Build beans from a v2.1 Registry Document
        /// </summary>
        /// <param name="rid">
        /// The rid.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public ISdmxObjects Build(RegistryInterface rid)
        {
            RegistryInterfaceType rit = rid.Content;
            if (rit.SubmitStructureRequest != null)
            {
                if (rit.SubmitStructureRequest.Structures != null)
                {
                    DatasetAction action = null;
                    if (!string.IsNullOrEmpty(rit.SubmitStructureRequest.action))
                    {
                        switch (rit.SubmitStructureRequest.action)
                        {
                            case ActionTypeConstants.Append:
                                action = DatasetAction.GetFromEnum(DatasetActionEnumType.Append);
                                break;
                            case ActionTypeConstants.Replace:
                                action = DatasetAction.GetFromEnum(DatasetActionEnumType.Replace);
                                break;
                            default:
                                action = DatasetAction.GetFromEnum(DatasetActionEnumType.Append);
                                break;

                        }
                    }
                    
                    return this.Build(rit.SubmitStructureRequest.Structures.Content, ProcessHeader(rit.Header), action);
                }
            }

            return new SdmxObjectsImpl();
        }

        /// <summary>
        /// Build beans from a v2.1 Structure Document
        /// </summary>
        /// <param name="structuresDoc">
        /// The structures Doc.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public ISdmxObjects Build(Structure structuresDoc)
        {
            StructureType structures = structuresDoc.Content;
            if (structures != null && structures.Structures != null)
            {
                return this.Build(structures.Structures, ProcessHeader(structures.Header), null);
            }

            return new SdmxObjectsImpl();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process the header.
        /// </summary>
        /// <param name="baseHeaderType">
        /// The base header type.
        /// </param>
        /// <returns>
        /// The <see cref="IHeader"/>.
        /// </returns>
        private static IHeader ProcessHeader(BaseHeaderType baseHeaderType)
        {
            return new HeaderImpl(baseHeaderType);
        }

        /// <summary>
        /// Build SDMX objects from v2.1 structures
        /// </summary>
        /// <param name="structures">
        /// The structures
        /// </param>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <returns>
        /// beans container of all beans built
        /// </returns>
        private ISdmxObjects Build(StructuresType structures, IHeader header, DatasetAction action)
        {
            var beans = new SdmxObjectsImpl(header, action);
            this.ProcessOrganisationSchemes(structures.OrganisationSchemes, beans);
            this.ProcessDataflows(structures.Dataflows, beans);
            this.ProcessMetadataFlows(structures.Metadataflows, beans);
            this.ProcessCategorySchemes(structures.CategorySchemes, beans);
            this.ProcessCategorisations(structures.Categorisations, beans);
            this.ProcessCodelists(structures.Codelists, beans);
            this.ProcessHierarchicalCodelists(structures.HierarchicalCodelists, beans);
            this.ProcessConcepts(structures.Concepts, beans);
            this.ProcessMetadataStructures(structures.MetadataStructures, beans);
            this.ProcessDataStructures(structures.DataStructures, beans);
            this.ProcessStructureSets(structures.StructureSets, beans);
            this.ProcessReportingTaxonomies(structures.ReportingTaxonomies, beans);
            this.ProcessProcesses(structures.Processes, beans);
            this.ProcessConstraints(structures.Constraints, beans);
            this.ProcessProvisions(structures.ProvisionAgreements, beans);
            return beans;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        //////////            VERSION 2.1 METHODS FOR STRUCTURES          ///////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates categorisations based on the input categorisation schemes
        /// </summary>
        /// <param name="categorisations">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add categorisations to
        /// </param>
        private void ProcessCategorisations(CategorisationsType categorisations, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (categorisations != null && ObjectUtil.ValidCollection(categorisations.Categorisation))
            {
                /* foreach */
                foreach (CategorisationType currentCategorisation in categorisations.Categorisation)
                {
                    try
                    {
                        ICategorisationObject categorisationObject = new CategorisationObjectCore(currentCategorisation);
                        this.AddIfNotDuplicateURN(beans, urns, categorisationObject);
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation), 
                            currentCategorisation.agencyID, 
                            currentCategorisation.id, 
                            currentCategorisation.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates category schemes based on the input category schemes
        /// </summary>
        /// <param name="catSchemes">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add category schemes to
        /// </param>
        private void ProcessCategorySchemes(CategorySchemesType catSchemes, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (catSchemes != null && ObjectUtil.ValidCollection(catSchemes.CategoryScheme))
            {
                /* foreach */
                foreach (CategorySchemeType currentCatScheme in catSchemes.CategoryScheme)
                {
                    try
                    {
                        ICategorySchemeObject categorySchemeObject = new CategorySchemeObjectCore(currentCatScheme);
                        this.AddIfNotDuplicateURN(beans, urns, categorySchemeObject);
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme), 
                            currentCatScheme.agencyID, 
                            currentCatScheme.id, 
                            currentCatScheme.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates Codelist based on the input Codelist schemes
        /// </summary>
        /// <param name="codelists">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add codelist to
        /// </param>
        private void ProcessCodelists(CodelistsType codelists, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (codelists != null && codelists.Codelist != null)
            {
                /* foreach */
                foreach (CodelistType currentType in codelists.Codelist)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new CodelistObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates ConceptSchemes based on the input Concepts
        /// </summary>
        /// <param name="concepts">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessConcepts(ConceptsType concepts, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (concepts != null && concepts.ConceptScheme != null)
            {
                /* foreach */
                foreach (ConceptSchemeType currentType in concepts.ConceptScheme)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ConceptSchemeObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates Constraints based on the input Constraints
        /// </summary>
        /// <param name="constraints">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessConstraints(ConstraintsType constraints, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (constraints != null)
            {
                /* foreach */
                foreach (AttachmentConstraintType currentType in constraints.AttachmentConstraint)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new AttachmentConstraintObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }

                /* foreach */
                foreach (ContentConstraintType currentType0 in constraints.ContentConstraint)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ContentConstraintObjectCore(currentType0));
                    }
                    catch (Exception th1)
                    {
                        throw new MaintainableObjectException(
                            th1, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint), 
                            currentType0.agencyID, 
                            currentType0.id, 
                            currentType0.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates DataStructures based on the input DataStructures
        /// </summary>
        /// <param name="keyfamilies">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessDataStructures(DataStructuresType keyfamilies, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (keyfamilies != null && keyfamilies.DataStructure != null)
            {
                /* foreach */
                foreach (DataStructureType currentType in keyfamilies.DataStructure)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new DataStructureObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates dataflows and categorisations based on the input dataflows
        /// </summary>
        /// <param name="dataflowsType">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add dataflows to beans
        /// </param>
        private void ProcessDataflows(DataflowsType dataflowsType, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (dataflowsType != null && ObjectUtil.ValidCollection(dataflowsType.Dataflow))
            {
                /* foreach */
                foreach (DataflowType currentType in dataflowsType.Dataflow)
                {
                    try
                    {
                        IDataflowObject currentDataflow = new DataflowObjectCore(currentType);
                        this.AddIfNotDuplicateURN(beans, urns, currentDataflow);
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates HierarchicalCodelists based on the input HierarchicalCodelist schemes
        /// </summary>
        /// <param name="hcodelists">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add hierarchical codelists to
        /// </param>
        private void ProcessHierarchicalCodelists(HierarchicalCodelistsType hcodelists, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (hcodelists != null && hcodelists.HierarchicalCodelist != null)
            {
                /* foreach */
                foreach (HierarchicalCodelistType currentType in hcodelists.HierarchicalCodelist)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new HierarchicalCodelistObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates metadataflows on the input metadataflows
        /// </summary>
        /// <param name="mdfType">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add metadataflow to beans
        /// </param>
        private void ProcessMetadataFlows(MetadataflowsType mdfType, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (mdfType != null && ObjectUtil.ValidCollection(mdfType.Metadataflow))
            {
                /* foreach */
                foreach (MetadataflowType currentType in mdfType.Metadataflow)
                {
                    try
                    {
                        IMetadataFlow currentMetadataflow = new MetadataflowObjectCore(currentType);
                        this.AddIfNotDuplicateURN(beans, urns, currentMetadataflow);
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates MetadataStructures based on the input MetadataStructures
        /// </summary>
        /// <param name="metadataStructures">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessMetadataStructures(MetadataStructuresType metadataStructures, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (metadataStructures != null && metadataStructures.MetadataStructure != null)
            {
                /* foreach */
                foreach (MetadataStructureType currentType in metadataStructures.MetadataStructure)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new MetadataStructureDefinitionObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates organisation schemes and agencies based on the input organisation schemes
        /// </summary>
        /// <param name="orgSchemesType">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add organisation schemes and agencies to
        /// </param>
        private void ProcessOrganisationSchemes(OrganisationSchemesType orgSchemesType, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (orgSchemesType != null)
            {
                if (ObjectUtil.ValidCollection(orgSchemesType.AgencyScheme))
                {
                    /* foreach */
                    foreach (AgencySchemeType currentType in orgSchemesType.AgencyScheme)
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new AgencySchemeCore(currentType));
                        }
                        catch (Exception th)
                        {
                            throw new MaintainableObjectException(
                                th, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme), 
                                currentType.agencyID, 
                                currentType.id, 
                                currentType.version);
                        }
                    }
                }

                if (ObjectUtil.ValidCollection(orgSchemesType.DataProviderScheme))
                {
                    /* foreach */
                    foreach (DataProviderSchemeType currentType0 in orgSchemesType.DataProviderScheme)
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new DataProviderSchemeCore(currentType0));
                        }
                        catch (Exception th1)
                        {
                            throw new MaintainableObjectException(
                                th1, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme), 
                                currentType0.agencyID, 
                                currentType0.id, 
                                currentType0.version);
                        }
                    }
                }

                if (ObjectUtil.ValidCollection(orgSchemesType.DataConsumerScheme))
                {
                    /* foreach */
                    foreach (DataConsumerSchemeType currentType2 in orgSchemesType.DataConsumerScheme)
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new DataConsumerSchemeCore(currentType2));
                        }
                        catch (Exception th3)
                        {
                            throw new MaintainableObjectException(
                                th3, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme), 
                                currentType2.agencyID, 
                                currentType2.id, 
                                currentType2.version);
                        }
                    }
                }

                if (ObjectUtil.ValidCollection(orgSchemesType.OrganisationUnitScheme))
                {
                    /* foreach */
                    foreach (OrganisationUnitSchemeType currentType4 in orgSchemesType.OrganisationUnitScheme)
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new OrganisationUnitSchemeObjectCore(currentType4));
                        }
                        catch (Exception th5)
                        {
                            throw new MaintainableObjectException(
                                th5, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme), 
                                currentType4.agencyID, 
                                currentType4.id, 
                                currentType4.version);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates Processes based on the input Processes
        /// </summary>
        /// <param name="processes">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessProcesses(ProcessesType processes, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (processes != null && processes.Process != null)
            {
                /* foreach */
                foreach (ProcessType currentType in processes.Process)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ProcessObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the provisions.
        /// </summary>
        /// <param name="provisions">
        /// The provisions.
        /// </param>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        private void ProcessProvisions(ProvisionAgreementsType provisions, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (provisions != null && ObjectUtil.ValidCollection(provisions.ProvisionAgreement))
            {
                /* foreach */
                foreach (ProvisionAgreementType currentType in provisions.ProvisionAgreement)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ProvisionAgreementObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates ReportingTaxonomies based on the input ReportingTaxonomies
        /// </summary>
        /// <param name="reportingTaxonomies">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessReportingTaxonomies(ReportingTaxonomiesType reportingTaxonomies, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (reportingTaxonomies != null && reportingTaxonomies.ReportingTaxonomy != null)
            {
                /* foreach */
                foreach (ReportingTaxonomyType currentType in reportingTaxonomies.ReportingTaxonomy)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ReportingTaxonomyObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates StructureSets based on the input StructureSets
        /// </summary>
        /// <param name="structureSets">
        /// - if null will not add anything to the beans container
        /// </param>
        /// <param name="beans">
        /// - to add concepts to
        /// </param>
        private void ProcessStructureSets(StructureSetsType structureSets, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (structureSets != null && structureSets.StructureSet != null)
            {
                /* foreach */
                foreach (StructureSetType currentType in structureSets.StructureSet)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new StructureSetObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet), 
                            currentType.agencyID, 
                            currentType.id, 
                            currentType.version);
                    }
                }
            }
        }

        #endregion
    }
}
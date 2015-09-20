// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSdmxObjectsV2Builder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract sdmx sdmxObjects v 2 builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The abstract sdmx sdmxObjects v 2 builder.
    /// </summary>
    public abstract class AbstractSdmxObjectsV2Builder : AbstractSdmxObjectsBuilder
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //////////            VERSION 2.0 METHODS FOR STRUCTURES          ///////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// Create Categorisations from Version 2.0 categories, adds the categorisations to the sdmxObjects container
        /// </summary>
        /// <param name="beans">
        /// container to add to
        /// </param>
        /// <param name="categoryTypes">
        /// category types to filter on
        /// </param>
        /// <param name="categoryBeans">
        /// category sdmxObjects to process
        /// </param>
        protected internal void ProcessCategory(
            ISdmxObjects beans, IList<CategoryType> categoryTypes, IList<ICategoryObject> categoryBeans)
        {
            if (categoryTypes == null)
            {
                return;
            }

            // Note converted from recursive in Java 0.9.4 to iterative
            var stack = new Stack<KeyValuePair<IList<CategoryType>, IList<ICategoryObject>>>();
            stack.Push(new KeyValuePair<IList<CategoryType>, IList<ICategoryObject>>(categoryTypes, categoryBeans));
            while (stack.Count > 0)
            {
                KeyValuePair<IList<CategoryType>, IList<ICategoryObject>> pair = stack.Pop();
                IList<CategoryType> currentTypes = pair.Key;
                foreach (CategoryType cat in currentTypes)
                {
                    ICategoryObject processingCatBean = null;

                    IList<ICategoryObject> currentObjects = pair.Value;
                    foreach (ICategoryObject currentCatBean in currentObjects)
                    {
                        if (currentCatBean.Id.Equals(cat.id))
                        {
                            processingCatBean = currentCatBean;
                            break;
                        }
                    }

                    // TODO this check doesn't exist in Java 0.9.4
                    if (processingCatBean != null)
                    {
                        stack.Push(
                            new KeyValuePair<IList<CategoryType>, IList<ICategoryObject>>(
                                cat.Category, processingCatBean.Items));
                        this.ProcessCategory(beans, cat, processingCatBean);
                    }
                }
            }
        }

        /// <summary>
        /// Create Categorisations from Version 2.0 category, adds the categorisation to the sdmxObjects container
        /// </summary>
        /// <param name="beans">
        /// container to add to
        /// </param>
        /// <param name="categoryType">
        /// The category Type.
        /// </param>
        /// <param name="categoryBean">
        /// The category Bean.
        /// </param>
        protected internal void ProcessCategory(
            ISdmxObjects beans, CategoryType categoryType, ICategoryObject categoryBean)
        {
            if (categoryType.DataflowRef != null)
            {
                foreach (DataflowRefType dataflowRefType in categoryType.DataflowRef)
                {
                    // use mutable for now until the following issue is fixed. 
                    // http://www.metadatatechnology.com/mantis/view.php?id=1341
                    ICategorisationMutableObject mutable = new CategorisationMutableCore();
                    mutable.AgencyId = categoryBean.MaintainableParent.AgencyId;

                    mutable.CategoryReference = categoryBean.AsReference;

                    // TODO create specialized collections for TextTypeWrapperMutable and TextTypeWrapper 
                    foreach (ITextTypeWrapper name in categoryBean.Names)
                    {
                        mutable.Names.Add(new TextTypeWrapperMutableCore(name));
                    }

                    mutable.StructureReference = dataflowRefType.URN != null
                                                     ? new StructureReferenceImpl(dataflowRefType.URN)
                                                     : new StructureReferenceImpl(
                                                           dataflowRefType.AgencyID,
                                                           dataflowRefType.DataflowID,
                                                           dataflowRefType.Version,
                                                           SdmxStructureEnumType.Dataflow);
                    mutable.Id = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}_{1}",
                        mutable.CategoryReference.GetHashCode(),
                        mutable.StructureReference.GetHashCode());

                    // TODO use MT fix in java when is done. Mantis ticket:
                    // http://www.metadatatechnology.com/mantis/view.php?id=1341
                    // sdmxObjects.AddCategorisation(new CategorisationObjectCore(categoryBean, dataflowRefType));
                    beans.AddCategorisation(new CategorisationObjectCore(mutable));
                }
            }

            if (categoryType.MetadataflowRef != null)
            {
                foreach (var mdfRef in categoryType.MetadataflowRef)
                {
                    beans.AddCategorisation(new CategorisationObjectCore(categoryBean, mdfRef));
                }
            }
        }

        /// <summary>
        /// Creates category schemes and categorisations based on the input category schemes
        /// </summary>
        /// <param name="catSchemes">
        /// - if null will not add anything to the sdmxObjects container
        /// </param>
        /// <param name="beans">
        /// - to add category schemes and categorisations to
        /// </param>
        protected internal void ProcessCategorySchemes(CategorySchemesType catSchemes, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (catSchemes != null && ObjectUtil.ValidCollection(catSchemes.CategoryScheme))
            {
                /* foreach */
                foreach (CategorySchemeType currentType in catSchemes.CategoryScheme)
                {
                    try
                    {
                        ICategorySchemeObject categorySchemeObject = new CategorySchemeObjectCore(currentType);
                        this.AddIfNotDuplicateURN(beans, urns, categorySchemeObject);
                        if (currentType.Category != null)
                        {
                            this.ProcessCategory(beans, currentType.Category, categorySchemeObject.Items);
                        }
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme),
                            currentType.agencyID,
                            currentType.id,
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Creates dataflow and categorisations based on the input dataflow
        /// </summary>
        /// <param name="dataflowsType">
        /// - if null will not add anything to the sdmxObjects container
        /// </param>
        /// <param name="beans">
        /// - to add dataflow and categorisations to
        /// </param>
        protected internal void ProcessDataflows(DataflowsType dataflowsType, ISdmxObjects beans)
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

                        // CATEGORISATIONS FROM DATAFLOWS
                        if (currentType.CategoryRef != null)
                        {
                            /* foreach */
                            foreach (CategoryRefType cateogryRefType in currentType.CategoryRef)
                            {
                                // use mutable for now until the following issue is fixed. 
                                // http://www.metadatatechnology.com/mantis/view.php?id=1341
                                ICategorisationMutableObject mutable = new CategorisationMutableCore();
                                mutable.AgencyId = currentDataflow.AgencyId;
                                mutable.CategoryReference = RefUtil.CreateCategoryRef(cateogryRefType);

                                // TODO create specialized collections for TextTypeWrapperMutable and TextTypeWrapper 
                                foreach (ITextTypeWrapper name in currentDataflow.Names)
                                {
                                    mutable.Names.Add(new TextTypeWrapperMutableCore(name));
                                }

                                mutable.StructureReference = currentDataflow.AsReference;
                                mutable.Id = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}_{1}",
                                    mutable.CategoryReference.GetHashCode(),
                                    mutable.StructureReference.GetHashCode());

                                // TODO use MT fix in java when is done. Mantis ticket:
                                // http://www.metadatatechnology.com/mantis/view.php?id=1341
                                beans.AddCategorisation(new CategorisationObjectCore(mutable));

                                ////sdmxObjects.AddCategorisation(new CategorisationObjectCore(currentDataflow, cateogryRefType));
                            }
                        }
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
        /// Creates meta-dataflow and categorisations based on the input meta-dataflow
        /// </summary>
        /// <param name="metadataflowsType">
        /// - if null will not add anything to the sdmxObjects container
        /// </param>
        /// <param name="beans">
        /// - to add meta-dataflow and categorisations to
        /// </param>
        protected internal void ProcessMetadataFlows(MetadataflowsType metadataflowsType, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();
            if (metadataflowsType != null && ObjectUtil.ValidCollection(metadataflowsType.Metadataflow))
            {
                /* foreach */
                foreach (MetadataflowType currentType in metadataflowsType.Metadataflow)
                {
                    try
                    {
                        IMetadataFlow currentMetadataflow = new MetadataflowObjectCore(currentType);

                        this.AddIfNotDuplicateURN(beans, urns, currentMetadataflow);

                        // CATEGORISATIONS FROM METADATAFLOWS
                        if (currentType.CategoryRef != null)
                        {
                            /* foreach */
                            foreach (CategoryRefType cateogryRefType in currentType.CategoryRef)
                            {
                                beans.AddCategorisation(
                                    new CategorisationObjectCore(currentMetadataflow, cateogryRefType));
                            }
                        }
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
        /// Process the  metadata structure definitions.
        /// </summary>
        /// <param name="metadataStructureDefinitionsType">
        /// The metadata structure definitions type.
        /// </param>
        /// <exception cref="UnsupportedException">
        /// Metadata Structure Definition at SMDX v2.0 - please use SDMX v2.1
        /// </exception>
        protected internal void ProcessMetadataStructureDefinitions(
            MetadataStructureDefinitionsType metadataStructureDefinitionsType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (metadataStructureDefinitionsType != null
                && metadataStructureDefinitionsType.MetadataStructureDefinition != null)
            {
                foreach (MetadataStructureDefinitionType currentType in metadataStructureDefinitionsType.MetadataStructureDefinition)
                {
                    try
                    {
                        AddIfNotDuplicateURN(beans, urns, new MetadataStructureDefinitionObjectCore(currentType));
                    }
                    catch (Exception ex)
                    {
                        throw new MaintainableObjectException(ex, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd), currentType.agencyID, currentType.id, currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the codelist
        /// </summary>
        /// <param name="structures">
        /// The structures.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessCodelists(CodeListsType structures, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (structures != null && structures.CodeList != null)
            {
                /* foreach */
                foreach (CodeListType currentType in structures.CodeList)
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
        /// Process the  concept schemes.
        /// </summary>
        /// <param name="structures">
        /// The structures.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessConceptSchemes(ConceptsType structures, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (structures == null || structures.ConceptScheme == null)
            {
                return;
            }

            foreach (ConceptSchemeType currentType0 in structures.ConceptScheme)
            {
                try
                {
                    this.AddIfNotDuplicateURN(beans, urns, new ConceptSchemeObjectCore(currentType0));
                }
                catch (Exception th1)
                {
                    throw new MaintainableObjectException(
                        th1,
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme),
                        currentType0.agencyID,
                        currentType0.id,
                        currentType0.version);
                }
            }

            // CONCEPTS
            IDictionary<string, IList<ConceptType>> conceptAgencyMap =
                new Dictionary<string, IList<ConceptType>>(StringComparer.Ordinal);
            {
                /* foreach */
                foreach (ConceptType currentType2 in structures.Concept)
                {
                    IList<ConceptType> concepts;
                    if (!conceptAgencyMap.TryGetValue(currentType2.agencyID, out concepts))
                    {
                        concepts = new List<ConceptType>();
                        conceptAgencyMap.Add(currentType2.agencyID, concepts);
                    }

                    concepts.Add(currentType2);
                }
            }

            /* foreach */
            foreach (KeyValuePair<string, IList<ConceptType>> currentConceptAgency in conceptAgencyMap)
            {
                try
                {
                    this.AddIfNotDuplicateURN(
                        beans, urns, new ConceptSchemeObjectCore(currentConceptAgency.Value, currentConceptAgency.Key));
                }
                catch (Exception th3)
                {
                    throw new MaintainableObjectException(
                        th3,
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme),
                        currentConceptAgency.Key,
                        ConceptSchemeObject.DefaultSchemeId,
                        ConceptSchemeObject.DefaultSchemeVersion);
                }
            }
        }

        /// <summary>
        /// Process the hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistsType">
        /// The hierarchical codelist type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessHierarchicalCodelists(
            HierarchicalCodelistsType hierarchicalCodelistsType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (hierarchicalCodelistsType != null && hierarchicalCodelistsType.HierarchicalCodelist != null)
            {
                /* foreach */
                foreach (HierarchicalCodelistType currentType4 in hierarchicalCodelistsType.HierarchicalCodelist)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new HierarchicalCodelistObjectCore(currentType4));
                    }
                    catch (Exception th5)
                    {
                        throw new MaintainableObjectException(
                            th5,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist),
                            currentType4.agencyID,
                            currentType4.id,
                            currentType4.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the  key families.
        /// </summary>
        /// <param name="keyFamiliesType">
        /// The key families type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessKeyFamilies(KeyFamiliesType keyFamiliesType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (keyFamiliesType != null && keyFamiliesType.KeyFamily != null)
            {
                /* foreach */
                foreach (KeyFamilyType currentType in keyFamiliesType.KeyFamily)
                {
                    try
                    {
                        //// Reverted changes that were made during sync
                        //// Please do not change unless you are sure.
                        if (CrossSectionalUtil.IsCrossSectional(currentType))
                        {
                            ICrossSectionalDataStructureObject xsdBean = new CrossSectionalDataStructureObjectCore(currentType);
                            ICrossSectionalDataStructureMutableObject mutable = xsdBean.MutableInstance;
                            if (currentType.Components.CrossSectionalMeasure.Count > 0)
                            {
                                //Set the measure dimensions references
                                ICrossSectionalMeasureMutableObject xsMutable = mutable.CrossSectionalMeasures[0];
                                IStructureReference conceptRef = xsMutable.ConceptRef;

                                IDictionary<string, IStructureReference> mapping = mutable.MeasureDimensionCodelistMapping;
                                IStructureReference cocneptSchemeRef = new StructureReferenceImpl(conceptRef.MaintainableReference, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));

                                /* foreach */
                                foreach (IDimensionMutableObject dim in mutable.Dimensions)
                                {
                                    if (dim.MeasureDimension)
                                    {
                                        if (!mapping.ContainsKey(dim.Id))
                                        {
                                            mapping.Add(dim.Id, dim.Representation.Representation);
                                        }

                                        dim.Representation.Representation = cocneptSchemeRef;
                                    }
                                }
                            }

                            this.AddIfNotDuplicateURN(beans, urns, mutable.ImmutableInstance);
                        }
                        else
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new DataStructureObjectCore(currentType));
                        }
                    }
                    catch (Exception th7)
                    {
                        throw new MaintainableObjectException(
                            th7,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd),
                            currentType.agencyID,
                            currentType.id,
                            currentType.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the  organisation schemes.
        /// </summary>
        /// <param name="organisationSchemesType">
        /// The organisation schemes type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessOrganisationSchemes(
            OrganisationSchemesType organisationSchemesType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (organisationSchemesType != null)
            {
                /* foreach */
                foreach (OrganisationSchemeType currentType10 in organisationSchemesType.OrganisationScheme)
                {
                    if (ObjectUtil.ValidCollection(currentType10.Agencies))
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new AgencySchemeCore(currentType10));
                        }
                        catch (Exception th11)
                        {
                            throw new MaintainableObjectException(
                                th11,
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme),
                                currentType10.agencyID,
                                currentType10.id,
                                currentType10.version);
                        }
                    }

                    if (ObjectUtil.ValidCollection(currentType10.DataConsumers))
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new DataConsumerSchemeCore(currentType10));
                        }
                        catch (Exception th12)
                        {
                            throw new MaintainableObjectException(
                                th12,
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme),
                                currentType10.agencyID,
                                currentType10.id,
                                currentType10.version);
                        }
                    }

                    if (ObjectUtil.ValidCollection(currentType10.DataProviders))
                    {
                        try
                        {
                            this.AddIfNotDuplicateURN(beans, urns, new DataProviderSchemeCore(currentType10));
                        }
                        catch (Exception th13)
                        {
                            throw new MaintainableObjectException(
                                th13,
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme),
                                currentType10.agencyID,
                                currentType10.id,
                                currentType10.version);
                        }
                    }

                    // If the organisation scheme contains no elements, then this is an error
                    if (currentType10.Agencies.Count == 0 &&
                        currentType10.DataConsumers.Count == 0 &&
                        currentType10.DataProviders.Count == 0)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.StructureInvalidOrganisationSchemeNoContent, currentType10.agencyID, currentType10.id);
                    }
                }
            }
        }

        /// <summary>
        /// Process the processes.
        /// </summary>
        /// <param name="processesType">
        /// The processes type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessProcesses(ProcessesType processesType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (processesType != null && processesType.Process != null)
            {
                /* foreach */
                foreach (ProcessType currentType14 in processesType.Process)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ProcessObjectCore(currentType14));
                    }
                    catch (Exception th15)
                    {
                        throw new MaintainableObjectException(
                            th15,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process),
                            currentType14.agencyID,
                            currentType14.id,
                            currentType14.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the  reporting taxonomies.
        /// </summary>
        /// <param name="reportingTaxonomiesType">
        /// The reporting taxonomies type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessReportingTaxonomies(
            ReportingTaxonomiesType reportingTaxonomiesType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (reportingTaxonomiesType != null && reportingTaxonomiesType.ReportingTaxonomy != null)
            {
                /* foreach */
                foreach (ReportingTaxonomyType currentType16 in reportingTaxonomiesType.ReportingTaxonomy)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new ReportingTaxonomyObjectCore(currentType16));
                    }
                    catch (Exception th17)
                    {
                        throw new MaintainableObjectException(
                            th17,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy),
                            currentType16.agencyID,
                            currentType16.id,
                            currentType16.version);
                    }
                }
            }
        }

        /// <summary>
        /// Process the  structure sets.
        /// </summary>
        /// <param name="structureSetsType">
        /// The structure sets type.
        /// </param>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <param name="urns">
        /// The URN.
        /// </param>
        /// <exception cref="MaintainableObjectException">
        /// Duplicate URN
        /// </exception>
        protected void ProcessStructureSets(StructureSetsType structureSetsType, ISdmxObjects beans, ISet<Uri> urns)
        {
            if (structureSetsType != null && structureSetsType.StructureSet != null)
            {
                /* foreach */
                foreach (StructureSetType currentType18 in structureSetsType.StructureSet)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new StructureSetObjectCore(currentType18));
                    }
                    catch (Exception th19)
                    {
                        throw new MaintainableObjectException(
                            th19,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet),
                            currentType18.agencyID,
                            currentType18.id,
                            currentType18.version);
                    }
                }
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    using Xml.Schema.Linq;

    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.StructureType;

    /// <summary>
    ///     The structure xml bean builder.
    /// </summary>
    public class StructureXmlBuilder : IBuilder<Structure, ISdmxObjects>
    {
        #region Fields

        /// <summary>
        ///     The category scheme xml bean builder.
        /// </summary>
        private readonly CategorySchemeXmlBuilder _categorySchemeXmlBuilder = new CategorySchemeXmlBuilder();

        /// <summary>
        ///     The codelist xml bean builder.
        /// </summary>
        private readonly CodelistXmlBuilder _codelistXmlBuilder = new CodelistXmlBuilder();

        /// <summary>
        ///     The concept scheme xml bean builder.
        /// </summary>
        private readonly ConceptSchemeXmlBuilder _conceptSchemeXmlBuilder = new ConceptSchemeXmlBuilder();

        /// <summary>
        ///     The concept xml bean builder.
        /// </summary>
        private readonly ConceptXmlBuilder _conceptXmlBuilder = new ConceptXmlBuilder();

        /// <summary>
        ///     The data structure xml bean builder.
        /// </summary>
        private readonly DataStructureXmlBuilder _dataStructureXmlBuilder = new DataStructureXmlBuilder();

        /// <summary>
        ///     The dataflow xml bean builder.
        /// </summary>
        private readonly DataflowXmlBuilder _dataflowXmlBuilder = new DataflowXmlBuilder();

        /// <summary>
        ///     The hierarchical codelist xml bean builder.
        /// </summary>
        private readonly HierarchicalCodelistXmlBuilder _hierarchicalCodelistXmlBuilder =
            new HierarchicalCodelistXmlBuilder();

        /// <summary>
        ///     The metadata structure definition xml beans builder.
        /// </summary>
        private readonly MetadataStructureDefinitionXmlsBuilder _metadataStructureDefinitionXmlsBuilder =
            new MetadataStructureDefinitionXmlsBuilder();

        /// <summary>
        ///     The metadataflow xml bean builder.
        /// </summary>
        private readonly MetadataflowXmlBuilder _metadataflowXmlBuilder = new MetadataflowXmlBuilder();

        /// <summary>
        ///     The organisation scheme xml bean builder.
        /// </summary>
        private readonly OrganisationSchemeXmlBuilder _organisationSchemeXmlBuilder = new OrganisationSchemeXmlBuilder();

        /// <summary>
        ///     The process xml bean builder.
        /// </summary>
        private readonly ProcessXmlBuilder _processXmlBuilder = new ProcessXmlBuilder();

        /// <summary>
        ///     The reporting taxonomy xml bean builder.
        /// </summary>
        private readonly ReportingTaxonomyXmlBuilder _reportingTaxonomyXmlBuilder = new ReportingTaxonomyXmlBuilder();

        /// <summary>
        ///     The structure header xml bean builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder _structureHeaderXmlBuilder = new StructureHeaderXmlBuilder();

        /// <summary>
        ///     The structure set xml bean builder.
        /// </summary>
        private readonly StructureSetXmlBuilder _structureSetXmlBuilder = new StructureSetXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="Structure"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The source SDMX Object.
        /// </param>
        /// <returns>
        /// The <see cref="Structure"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual Structure Build(ISdmxObjects buildFrom)
        {
            var doc = new Structure();
            StructureType returnType = doc.Content;

            // HEADER
            HeaderType headerType;
            if (buildFrom.Header != null)
            {
                headerType = this._structureHeaderXmlBuilder.Build(buildFrom.Header);
                returnType.Header = headerType;
            }
            else
            {
                headerType = new HeaderType();
                returnType.Header = headerType;

                V2Helper.SetHeader(headerType, buildFrom);
            }

            // TOP LEVEL STRUCTURES ELEMENT
            ISet<ICategorisationObject> categorisations = buildFrom.Categorisations;

            // GET CATEGORY SCHEMES
            ISet<ICategorySchemeObject> categorySchemeObjects = buildFrom.CategorySchemes;
            if (categorySchemeObjects.Count > 0)
            {
                var catSchemesType = new CategorySchemesType();
                returnType.CategorySchemes = catSchemesType;

                /* foreach */
                foreach (ICategorySchemeObject cateogrySchemeBean in categorySchemeObjects)
                {
                    ISet<ICategorisationObject> matchingCategorisations = new HashSet<ICategorisationObject>();

                    /* foreach */
                    foreach (ICategorisationObject cat in categorisations)
                    {
                        if (cat.IsExternalReference.IsTrue)
                        {
                            continue;
                        }

                        if (MaintainableUtil<ICategorySchemeObject>.Match(cateogrySchemeBean, cat.CategoryReference))
                        {
                            matchingCategorisations.Add(cat);
                        }
                    }

                    catSchemesType.CategoryScheme.Add(this.Build(cateogrySchemeBean, categorisations));
                }
            }

            // GET CODELISTS
            ISet<ICodelistObject> codelistObjects = buildFrom.Codelists;
            if (codelistObjects.Count > 0)
            {
                var codeListsType = new CodeListsType();
                returnType.CodeLists = codeListsType;

                /* foreach */
                foreach (ICodelistObject codelistBean in codelistObjects)
                {
                    codeListsType.CodeList.Add(this.Build(codelistBean));
                }
            }

            // CONCEPT SCHEMES
            ISet<IConceptSchemeObject> conceptSchemeObjects = buildFrom.ConceptSchemes;
            if (conceptSchemeObjects.Count > 0)
            {
                var conceptsType = new ConceptsType();
                returnType.Concepts = conceptsType;

                /* foreach */
                foreach (IConceptSchemeObject conceptSchemeBean in conceptSchemeObjects)
                {
                    conceptsType.ConceptScheme.Add(this.Build(conceptSchemeBean));
                }
            }

            // DATAFLOWS
            ISet<IDataflowObject> dataflowObjects = buildFrom.Dataflows;
            if (dataflowObjects.Count > 0)
            {
                var dataflowsType = new DataflowsType();
                returnType.Dataflows = dataflowsType;

                /* foreach */
                foreach (IDataflowObject currentBean in dataflowObjects)
                {
                    dataflowsType.Dataflow.Add(
                        this.Build(currentBean, GetCategorisations(currentBean, categorisations)));
                }
            }

            // HIERARCIC CODELIST
            ISet<IHierarchicalCodelistObject> hierarchicalCodelistObjects = buildFrom.HierarchicalCodelists;
            if (hierarchicalCodelistObjects.Count > 0)
            {
                var hierarchicalCodelistsType = new HierarchicalCodelistsType();
                returnType.HierarchicalCodelists = hierarchicalCodelistsType;

                /* foreach */
                foreach (IHierarchicalCodelistObject currentBean0 in hierarchicalCodelistObjects)
                {
                    hierarchicalCodelistsType.HierarchicalCodelist.Add(this.Build(currentBean0));
                }
            }

            // KEY FAMILY
            ISet<IDataStructureObject> dataStructureObjects = buildFrom.DataStructures;
            if (dataStructureObjects.Count > 0)
            {
                var keyFamiliesType = new KeyFamiliesType();
                returnType.KeyFamilies = keyFamiliesType;

                /* foreach */
                foreach (IDataStructureObject currentBean1 in dataStructureObjects)
                {
                    keyFamiliesType.KeyFamily.Add(this.Build(currentBean1));
                }
            }

            // METADATA FLOW
            ISet<IMetadataFlow> metadataFlows = buildFrom.Metadataflows;
            if (metadataFlows.Count > 0)
            {
                var metadataflowsType = new MetadataflowsType();
                returnType.Metadataflows = metadataflowsType;

                /* foreach */
                foreach (IMetadataFlow currentBean2 in metadataFlows)
                {
                    metadataflowsType.Metadataflow.Add(
                        this.Build(currentBean2, GetCategorisations(currentBean2, categorisations)));
                }
            }

            // METADATA STRUCTURE
            ISet<IMetadataStructureDefinitionObject> metadataStructureDefinitionObjects = buildFrom.MetadataStructures;
            if (metadataStructureDefinitionObjects.Count > 0)
            {
                var msdsType = new MetadataStructureDefinitionsType();
                returnType.MetadataStructureDefinitions = msdsType;

                /* foreach */
                foreach (IMetadataStructureDefinitionObject currentBean3 in metadataStructureDefinitionObjects)
                {
                    msdsType.MetadataStructureDefinition.Add(this.Build(currentBean3));
                }
            }

            // ORGAISATION SCHEME
            ISet<IOrganisationUnitSchemeObject> organisationUnitSchemeObjects = buildFrom.OrganisationUnitSchemes;
            if (organisationUnitSchemeObjects.Count > 0)
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported,
                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme).StructureType);
            }

            OrganisationSchemesType orgSchemesType = null;

            // AGENCY SCHEMES
            ISet<IAgencyScheme> agenciesSchemes = buildFrom.AgenciesSchemes;
            if (agenciesSchemes.Count > 0)
            {
                orgSchemesType = new OrganisationSchemesType();
                returnType.OrganisationSchemes = orgSchemesType;

                /* foreach */
                foreach (IAgencyScheme currentBean4 in agenciesSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean4));
                }
            }

            // DATA CONSUMER SCHEMES
            ISet<IDataConsumerScheme> dataConsumerSchemes = buildFrom.DataConsumerSchemes;
            if (dataConsumerSchemes.Count > 0)
            {
                if (orgSchemesType == null)
                {
                    orgSchemesType = new OrganisationSchemesType();
                    returnType.OrganisationSchemes = orgSchemesType;
                }

                /* foreach */
                foreach (IDataConsumerScheme currentBean5 in dataConsumerSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean5));
                }
            }

            // DATA PROVIDER SCHEMES
            ISet<IDataProviderScheme> dataProviderSchemes = buildFrom.DataProviderSchemes;
            if (dataProviderSchemes.Count > 0)
            {
                if (orgSchemesType == null)
                {
                    orgSchemesType = new OrganisationSchemesType();
                    returnType.OrganisationSchemes = orgSchemesType;
                }

                /* foreach */
                foreach (IDataProviderScheme currentBean6 in dataProviderSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean6));
                }
            }

            // PROCESSES
            ISet<IProcessObject> processObjects = buildFrom.Processes;
            if (processObjects.Count > 0)
            {
                var processesType = new ProcessesType();
                returnType.Processes = processesType;

                /* foreach */
                foreach (IProcessObject currentBean7 in processObjects)
                {
                    processesType.Process.Add(this.Build(currentBean7));
                }
            }

            // STRUCTURE SETS
            ISet<IStructureSetObject> structureSetObjects = buildFrom.StructureSets;
            if (structureSetObjects.Count > 0)
            {
                var structureSetsType = new StructureSetsType();
                returnType.StructureSets = structureSetsType;

                /* foreach */
                foreach (IStructureSetObject currentBean8 in structureSetObjects)
                {
                    structureSetsType.StructureSet.Add(this.Build(currentBean8));
                }
            }

            // REPORTING TAXONOMIES
            ISet<IReportingTaxonomyObject> reportingTaxonomyObjects = buildFrom.ReportingTaxonomys;
            if (reportingTaxonomyObjects.Count > 0)
            {
                var reportingTaxonomiesType = new ReportingTaxonomiesType();
                returnType.ReportingTaxonomies = reportingTaxonomiesType;

                /* foreach */
                foreach (IReportingTaxonomyObject currentBean9 in reportingTaxonomyObjects)
                {
                    reportingTaxonomiesType.ReportingTaxonomy.Add(this.Build(currentBean9));
                }
            }

            ISet<IAttachmentConstraintObject> attachmentConstraintObjects = buildFrom.AttachmentConstraints;
            if (attachmentConstraintObjects.Count > 0)
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "Attachment Constraint at SMDX v2.0 - please use SDMX v2.1");
            }

            ISet<IContentConstraintObject> contentConstraintObjects = buildFrom.ContentConstraintObjects;
            if (contentConstraintObjects.Count > 0)
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "Content Constraint at SMDX v2.0 - please use SDMX v2.1");
            }

            return doc;
        }

        /// <summary>
        /// Build the XMLObject if Dataflow will return DataflowType
        /// </summary>
        /// <param name="bean">
        /// - the Bean to build the type for
        /// </param>
        /// <param name="categorisations">
        /// - optional (for v2 only if Maintainable is Dataflow, Metadataflow or Category)
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public XTypedElement Build(IMaintainableObject bean, ISet<ICategorisationObject> categorisations)
        {
            // FUNC 2.1 support IAgencyScheme, IDataConsumerScheme and IDataProviderScheme
            switch (bean.StructureType.EnumType)
            {
                case SdmxStructureEnumType.CategoryScheme:
                    return this.Build((ICategorySchemeObject)bean, categorisations);
                case SdmxStructureEnumType.CodeList:
                    return this.Build((ICodelistObject)bean);
                case SdmxStructureEnumType.ConceptScheme:
                    return this.Build((IConceptSchemeObject)bean);
                case SdmxStructureEnumType.Dataflow:
                    return this.Build((IDataflowObject)bean, categorisations);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return this.Build((IHierarchicalCodelistObject)bean);
                case SdmxStructureEnumType.Dsd:
                    return this.Build((IDataStructureObject)bean);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.Build((IMetadataFlow)bean, categorisations);
                case SdmxStructureEnumType.Msd:
                    return this.Build((IMetadataStructureDefinitionObject)bean);
                case SdmxStructureEnumType.Process:
                    return this.Build((IProcessObject)bean);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return this.Build((IReportingTaxonomyObject)bean);
                case SdmxStructureEnumType.StructureSet:
                    return this.Build((IStructureSetObject)bean);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, bean.StructureType);
            }
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <returns>
        /// The <see cref="CategorySchemeType"/>.
        /// </returns>
        public CategorySchemeType Build(ICategorySchemeObject bean, ISet<ICategorisationObject> categorisations)
        {
            return this._categorySchemeXmlBuilder.Build(bean, categorisations);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="CodeListType"/>.
        /// </returns>
        public CodeListType Build(ICodelistObject bean)
        {
            return this._codelistXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="ConceptSchemeType"/>.
        /// </returns>
        public ConceptSchemeType Build(IConceptSchemeObject bean)
        {
            return this._conceptSchemeXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="ConceptType"/>.
        /// </returns>
        public ConceptType Build(IConceptObject bean)
        {
            return this._conceptXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <returns>
        /// The <see cref="DataflowType"/>.
        /// </returns>
        public DataflowType Build(IDataflowObject bean, ISet<ICategorisationObject> categorisations)
        {
            return this._dataflowXmlBuilder.Build(bean, categorisations);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="HierarchicalCodelistType"/>.
        /// </returns>
        public HierarchicalCodelistType Build(IHierarchicalCodelistObject bean)
        {
            return this._hierarchicalCodelistXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="KeyFamilyType"/>.
        /// </returns>
        public KeyFamilyType Build(IDataStructureObject bean)
        {
            return this._dataStructureXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataflowType"/>.
        /// </returns>
        public MetadataflowType Build(IMetadataFlow bean, ISet<ICategorisationObject> categorisations)
        {
            return this._metadataflowXmlBuilder.Build(bean, categorisations);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataStructureDefinitionType"/>.
        /// </returns>
        public MetadataStructureDefinitionType Build(IMetadataStructureDefinitionObject bean)
        {
            return this._metadataStructureDefinitionXmlsBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="ProcessType"/>.
        /// </returns>
        public ProcessType Build(IProcessObject bean)
        {
            return this._processXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="ReportingTaxonomyType"/>.
        /// </returns>
        public ReportingTaxonomyType Build(IReportingTaxonomyObject bean)
        {
            return this._reportingTaxonomyXmlBuilder.Build(bean);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <returns>
        /// The <see cref="StructureSetType"/>.
        /// </returns>
        public StructureSetType Build(IStructureSetObject bean)
        {
            return this._structureSetXmlBuilder.Build(bean);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the categorisations.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <returns>
        /// the set of categorisations
        /// </returns>
        private static ISet<ICategorisationObject> GetCategorisations(
            IMaintainableObject maintainable, IEnumerable<ICategorisationObject> categorisations)
        {
            ISet<ICategorisationObject> returnSet = new HashSet<ICategorisationObject>();
            if (maintainable.IsExternalReference.IsTrue)
            {
                return returnSet;
            }

            /* foreach */
            foreach (ICategorisationObject cat in categorisations)
            {
                if (cat.IsExternalReference.IsTrue)
                {
                    continue;
                }

                if (cat.StructureReference.TargetReference.EnumType == maintainable.StructureType.EnumType)
                {
                    if (MaintainableUtil<IMaintainableObject>.Match(maintainable, cat.StructureReference))
                    {
                        returnSet.Add(cat);
                    }
                }
            }

            return returnSet;
        }

        #endregion
    }
}
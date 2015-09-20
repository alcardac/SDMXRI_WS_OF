// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Build a v2.1 SDMX Structure Document from SDMXObjects, by incorporating other builders for its parts
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
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
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.StructureType;

    /// <summary>
    ///     Build a v2.1 SDMX Structure Document from SDMXObjects, by incorporating other builders for its parts
    /// </summary>
    public class StructureXmlBuilder : IBuilder<Structure, ISdmxObjects>
    {
        #region Fields

        /// <summary>
        ///     The agency scheme xml bean builder.
        /// </summary>
        private readonly AgencySchemeXmlBuilder _agencySchemeXmlBuilder = new AgencySchemeXmlBuilder();

        /// <summary>
        ///     The attachment constraint xml bean builder.
        /// </summary>
        private readonly AttachmentConstraintXmlBuilder _attachmentConstraintXmlBuilder =
            new AttachmentConstraintXmlBuilder();

        /// <summary>
        ///     The categorisation xml bean builder.
        /// </summary>
        private readonly CategorisationXmlBuilder _categorisationXmlBuilder = new CategorisationXmlBuilder();

        /// <summary>
        ///     The category scheme xml bean builder bean.
        /// </summary>
        private readonly CategorySchemeXmlBuilder _categorySchemeXmlBuilderBean = new CategorySchemeXmlBuilder();

        /// <summary>
        ///     The codelist xml bean builder bean.
        /// </summary>
        private readonly CodelistXmlBuilder _codelistXmlBuilderBean = new CodelistXmlBuilder();

        /// <summary>
        ///     The concept scheme xml bean builder bean.
        /// </summary>
        private readonly ConceptSchemeXmlBuilder _conceptSchemeXmlBuilderBean = new ConceptSchemeXmlBuilder();

        /// <summary>
        ///     The content constraint xml bean builder.
        /// </summary>
        private readonly ContentConstraintXmlBuilder _contentConstraintXmlBuilder = new ContentConstraintXmlBuilder();

        /// <summary>
        ///     The data consumer scheme xml bean builder.
        /// </summary>
        private readonly DataConsumerSchemeXmlBuilder _dataConsumerSchemeXmlBuilder = new DataConsumerSchemeXmlBuilder();

        /// <summary>
        ///     The data provider scheme xml bean builder.
        /// </summary>
        private readonly DataProviderSchemeXmlBuilder _dataProviderSchemeXmlBuilder = new DataProviderSchemeXmlBuilder();

        /// <summary>
        ///     The data structure xml bean builder bean.
        /// </summary>
        private readonly DataStructureXmlBuilder _dataStructureXmlBuilderBean = new DataStructureXmlBuilder();

        /// <summary>
        ///     The dataflow xml bean builder bean.
        /// </summary>
        private readonly DataflowXmlBuilder _dataflowXmlBuilderBean = new DataflowXmlBuilder();

        /// <summary>
        ///     The hierarchical codelist xml builder bean.
        /// </summary>
        private readonly HierarchicalCodelistXmlBuilder _hierarchicalCodelistXmlBuilderBean =
            new HierarchicalCodelistXmlBuilder();

        /// <summary>
        ///     The metadata structure xml bean builder bean.
        /// </summary>
        private readonly MetadataStructureXmlBuilder _metadataStructureXmlBuilderBean =
            new MetadataStructureXmlBuilder();

        /// <summary>
        ///     The metadataflow xml bean builder bean.
        /// </summary>
        private readonly MetadataflowXmlBuilder _metadataflowXmlBuilderBean = new MetadataflowXmlBuilder();

        /// <summary>
        ///     The organisation unit scheme xml bean builder.
        /// </summary>
        private readonly OrganisationUnitSchemeXmlBuilder _organisationUnitSchemeXmlBuilder =
            new OrganisationUnitSchemeXmlBuilder();

        /// <summary>
        ///     The process xml bean builder bean.
        /// </summary>
        private readonly ProcessXmlBuilder _processXmlBuilderBean = new ProcessXmlBuilder();

        /// <summary>
        ///     The provision agreement xml bean builder.
        /// </summary>
        private readonly ProvisionAgreementXmlBuilder _provisionAgreementXmlBuilder = new ProvisionAgreementXmlBuilder();

        /// <summary>
        ///     The reporting taxonomy xml bean builder bean.
        /// </summary>
        private readonly ReportingTaxonomyXmlBuilder _reportingTaxonomyXmlBuilderBean =
            new ReportingTaxonomyXmlBuilder();

        /// <summary>
        ///     The structure header xml bean builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder<StructureHeaderType> _structureHeaderXmlBuilder =
            new StructureHeaderXmlBuilder<StructureHeaderType>();

        /// <summary>
        ///     The structure set xml bean builder bean.
        /// </summary>
        private readonly StructureSetXmlBuilder _structureSetXmlBuilderBean = new StructureSetXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="Structure"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="ISdmxObjects"/>
        /// </param>
        /// <returns>
        /// The <see cref="Structure"/>.
        /// </returns>
        public virtual Structure Build(ISdmxObjects buildFrom)
        {
            var doc = new Structure();
            StructureType structureType = doc.Content;

            // HEADER
            StructureHeaderType headerType;
            if (buildFrom.Header != null)
            {
                headerType = this._structureHeaderXmlBuilder.Build(buildFrom.Header);
                structureType.Header = headerType;
            }
            else
            {
                headerType = new StructureHeaderType();
                structureType.Header = headerType;
                V21Helper.SetHeader(headerType, buildFrom);
            }

            // TOP LEVEL STRUCTURES ELEMENT
            var structures = new StructuresType();
            structureType.Structures = structures;

            this.PopulateStructureType(buildFrom, structures);
            return doc;
        }

        /// <summary>
        /// The populate structure type.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="structures">
        /// The structures.
        /// </param>
        public void PopulateStructureType(ISdmxObjects buildFrom, StructuresType structures)
        {
            OrganisationSchemesType orgType = null;
            if (buildFrom.OrganisationUnitSchemes.Count > 0)
            {
                orgType = AddOrganisationSchemesType(structures);

                foreach (IOrganisationUnitSchemeObject currentBean in buildFrom.OrganisationUnitSchemes)
                {
                    orgType.OrganisationUnitScheme.Add(this._organisationUnitSchemeXmlBuilder.Build(currentBean));
                }
            }

            if (buildFrom.DataConsumerSchemes.Count > 0)
            {
                if (orgType == null)
                {
                    orgType = AddOrganisationSchemesType(structures);
                }

                //// TODO check latest java code. Unused array in Java 0.9.1

                /* foreach */
                foreach (IDataConsumerScheme currentBean2 in buildFrom.DataConsumerSchemes)
                {
                    orgType.DataConsumerScheme.Add(this._dataConsumerSchemeXmlBuilder.Build(currentBean2));
                }
            }

            if (buildFrom.DataProviderSchemes.Count > 0)
            {
                if (orgType == null)
                {
                    orgType = AddOrganisationSchemesType(structures);
                }

                /* foreach */
                foreach (IDataProviderScheme currentBean3 in buildFrom.DataProviderSchemes)
                {
                    orgType.DataProviderScheme.Add(this._dataProviderSchemeXmlBuilder.Build(currentBean3));
                }
            }

            if (buildFrom.AgenciesSchemes.Count > 0)
            {
                if (orgType == null)
                {
                    orgType = AddOrganisationSchemesType(structures);
                }

                /* foreach */
                foreach (IAgencyScheme currentBean4 in buildFrom.AgenciesSchemes)
                {
                    orgType.AgencyScheme.Add(this._agencySchemeXmlBuilder.Build(currentBean4));
                }
            }

            // CONSTRAINTS
            if (buildFrom.AttachmentConstraints.Count > 0 || buildFrom.ContentConstraintObjects.Count > 0)
            {
                var constraintsType = new ConstraintsType();
                structures.Constraints = constraintsType;

                /* foreach */
                foreach (IAttachmentConstraintObject currentBean5 in buildFrom.AttachmentConstraints)
                {
                    constraintsType.AttachmentConstraint.Add(this._attachmentConstraintXmlBuilder.Build(currentBean5));
                }

                /* foreach */
                foreach (IContentConstraintObject currentBean6 in buildFrom.ContentConstraintObjects)
                {
                    constraintsType.ContentConstraint.Add(this._contentConstraintXmlBuilder.Build(currentBean6));
                }
            }

            // DATAFLOWS
            if (buildFrom.Dataflows.Count > 0)
            {
                var dataflowsType = new DataflowsType();
                structures.Dataflows = dataflowsType;

                /* foreach */
                foreach (IDataflowObject currentBean7 in buildFrom.Dataflows)
                {
                    dataflowsType.Dataflow.Add(this._dataflowXmlBuilderBean.Build(currentBean7));
                }
            }

            // METADATAFLOWS
            if (buildFrom.Metadataflows.Count > 0)
            {
                var metadataflowsType = new MetadataflowsType();
                structures.Metadataflows = metadataflowsType;

                /* foreach */
                foreach (IMetadataFlow currentBean8 in buildFrom.Metadataflows)
                {
                    metadataflowsType.Metadataflow.Add(this._metadataflowXmlBuilderBean.Build(currentBean8));
                }
            }

            // CATEGORY SCHEMES
            if (buildFrom.CategorySchemes.Count > 0)
            {
                var catSchemesType = new CategorySchemesType();
                structures.CategorySchemes = catSchemesType;

                /* foreach */
                foreach (ICategorySchemeObject categorySchemeBean in buildFrom.CategorySchemes)
                {
                    catSchemesType.CategoryScheme.Add(this._categorySchemeXmlBuilderBean.Build(categorySchemeBean));
                }
            }

            // CATEGORISATIONS
            if (buildFrom.Categorisations.Count > 0)
            {
                var categorisations = new CategorisationsType();
                structures.Categorisations = categorisations;

                /* foreach */
                foreach (ICategorisationObject categorisationBean in buildFrom.Categorisations)
                {
                    categorisations.Categorisation.Add(this._categorisationXmlBuilder.Build(categorisationBean));
                }
            }

            // CODELISTS
            ISet<ICodelistObject> codelists = buildFrom.Codelists;
            if (codelists.Count > 0)
            {
                var codelistsType = new CodelistsType();
                structures.Codelists = codelistsType;

                foreach (ICodelistObject codelistBean in codelists)
                {
                    codelistsType.Codelist.Add(this._codelistXmlBuilderBean.Build(codelistBean));
                }
            }

            // HIERARCHICAL CODELISTS
            if (buildFrom.HierarchicalCodelists.Count > 0)
            {
                var hierarchicalCodelistsType = new HierarchicalCodelistsType();
                structures.HierarchicalCodelists = hierarchicalCodelistsType;

                /* foreach */
                foreach (IHierarchicalCodelistObject currentBean11 in buildFrom.HierarchicalCodelists)
                {
                    hierarchicalCodelistsType.HierarchicalCodelist.Add(
                        this._hierarchicalCodelistXmlBuilderBean.Build(currentBean11));
                }
            }

            // CONCEPTS
            if (buildFrom.ConceptSchemes.Count > 0)
            {
                var conceptsType = new ConceptsType();
                structures.Concepts = conceptsType;

                foreach (IConceptSchemeObject conceptSchemeBean in buildFrom.ConceptSchemes)
                {
                    conceptsType.ConceptScheme.Add(this._conceptSchemeXmlBuilderBean.Build(conceptSchemeBean));
                }
            }

            // METADATA STRUCTURE
            if (buildFrom.MetadataStructures.Count > 0)
            {
                var msdsType = new MetadataStructuresType();
                structures.MetadataStructures = msdsType;

                /* foreach */
                foreach (IMetadataStructureDefinitionObject currentBean12 in buildFrom.MetadataStructures)
                {
                    msdsType.MetadataStructure.Add(this._metadataStructureXmlBuilderBean.Build(currentBean12));
                }
            }

            // DATA STRUCTURE
            if (buildFrom.DataStructures.Count > 0)
            {
                var dataStructuresType = new DataStructuresType();
                structures.DataStructures = dataStructuresType;

                /* foreach */
                foreach (IDataStructureObject currentBean13 in buildFrom.DataStructures)
                {
                    dataStructuresType.DataStructure.Add(this._dataStructureXmlBuilderBean.Build(currentBean13));
                }
            }

            // STRUCTURE SETS
            if (buildFrom.StructureSets.Count > 0)
            {
                var structureSetsType = new StructureSetsType();
                structures.StructureSets = structureSetsType;

                /* foreach */
                foreach (IStructureSetObject currentBean14 in buildFrom.StructureSets)
                {
                    structureSetsType.StructureSet.Add(this._structureSetXmlBuilderBean.Build(currentBean14));
                }
            }

            // REPORTING TAXONOMIES
            if (buildFrom.ReportingTaxonomys.Count > 0)
            {
                var reportingTaxonomiesType = new ReportingTaxonomiesType();
                structures.ReportingTaxonomies = reportingTaxonomiesType;

                /* foreach */
                foreach (IReportingTaxonomyObject currentBean15 in buildFrom.ReportingTaxonomys)
                {
                    reportingTaxonomiesType.ReportingTaxonomy.Add(
                        this._reportingTaxonomyXmlBuilderBean.Build(currentBean15));
                }
            }

            // PROCESSES
            if (buildFrom.Processes.Count > 0)
            {
                var processesType = new ProcessesType();
                structures.Processes = processesType;

                /* foreach */
                foreach (IProcessObject currentBean16 in buildFrom.Processes)
                {
                    processesType.Process.Add(this._processXmlBuilderBean.Build(currentBean16));
                }
            }

            // PROVISION AGREEMENTS
            if (buildFrom.ProvisionAgreements.Count > 0)
            {
                var provisionAgreementsType = new ProvisionAgreementsType();
                structures.ProvisionAgreements = provisionAgreementsType;

                /* foreach */
                foreach (IProvisionAgreementObject currentBean17 in buildFrom.ProvisionAgreements)
                {
                    provisionAgreementsType.ProvisionAgreement.Add(
                        this._provisionAgreementXmlBuilder.Build(currentBean17));
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add the organisation schemes type.
        /// </summary>
        /// <param name="structures">
        /// The structures.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemesType"/>.
        /// </returns>
        private static OrganisationSchemesType AddOrganisationSchemesType(StructuresType structures)
        {
            var orgType = new OrganisationSchemesType();
            structures.OrganisationSchemes = orgType;
            return orgType;
        }

        #endregion
    }
}
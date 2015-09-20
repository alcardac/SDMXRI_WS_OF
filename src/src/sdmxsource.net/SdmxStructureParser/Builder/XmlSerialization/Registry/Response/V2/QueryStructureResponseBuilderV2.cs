// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
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
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///     The query structure response builder v 2.
    /// </summary>
    public class QueryStructureResponseBuilderV2 : AbstractResponseBuilder
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
        ///     The data structure xml bean builder.
        /// </summary>
        private readonly DataStructureXmlBuilder _dataStructureXmlBuilder = new DataStructureXmlBuilder();

        /// <summary>
        ///     The dataflow xml bean builder.
        /// </summary>
        private readonly DataflowXmlBuilder _dataflowXmlBuilder = new DataflowXmlBuilder();

        /// <summary>
        ///     The header xml beans builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder _headerXmlsBuilder = new StructureHeaderXmlBuilder();

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
        ///     The structure set xml bean builder.
        /// </summary>
        private readonly StructureSetXmlBuilder _structureSetXmlBuilder = new StructureSetXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception exception)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryStructureResponseType();
            regInterface.QueryStructureResponse = returnType;
            V2Helper.Header = regInterface;

            var statusMessage = new StatusMessageType();
            returnType.StatusMessage = statusMessage;

            statusMessage.status = StatusTypeConstants.Failure;

            var tt = new TextType();
            statusMessage.MessageText.Add(tt);

            var uncheckedException = exception as SdmxException;
            tt.TypedValue = uncheckedException != null ? uncheckedException.FullMessage : exception.Message;

            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ISdmxObjects beans)
        {
            return this.BuildSuccessResponse(beans, null);
        }

        /// <summary>
        /// Build success response.
        /// </summary>
        /// <param name="buildFrom">
        /// The source sdmx objects.
        /// </param>
        /// <param name="warningMessage">
        /// The warning message.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ISdmxObjects buildFrom, string warningMessage)
        {

            // PLEASE NOTE. The code here is slightly different than in Java.
            // That is because of the differences between Java XmlBeans and .NET Linq2Xsd generated classes.
            // Please consult GIT log before making any changes.
            var responseType = new RegistryInterface();

            RegistryInterfaceType regInterface = responseType.Content;
            HeaderType headerType;

            if (buildFrom.Header != null)
            {
                headerType = this._headerXmlsBuilder.Build(buildFrom.Header);
                regInterface.Header = headerType;
            }
            else
            {
                headerType = new HeaderType();
                regInterface.Header = headerType;
                V2Helper.SetHeader(headerType, buildFrom);
            }
            
            var returnType = new QueryStructureResponseType();
            regInterface.QueryStructureResponse = returnType;

            var statusMessage = new StatusMessageType();
            returnType.StatusMessage = statusMessage;

            if (!string.IsNullOrWhiteSpace(warningMessage) || !ObjectUtil.ValidCollection(buildFrom.GetAllMaintainables()))
            {
                statusMessage.status = StatusTypeConstants.Warning;
                var tt = new TextType();
                statusMessage.MessageText.Add(tt);
                tt.TypedValue = !string.IsNullOrWhiteSpace(warningMessage)
                                    ? warningMessage
                                    : "No Structures Match The Query Parameters";
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Success;
            }

            ISet<ICategorisationObject> categorisations = buildFrom.Categorisations;

            // GET CATEGORY SCHEMES
            if (buildFrom.CategorySchemes.Count > 0)
            {
                var catSchemesType = new CategorySchemesType();
                returnType.CategorySchemes = catSchemesType;

                /* foreach */
                foreach (ICategorySchemeObject cateogrySchemeBean in buildFrom.CategorySchemes)
                {
                    ISet<ICategorisationObject> matchingCategorisations = new HashSet<ICategorisationObject>();

                    /* foreach */
                    foreach (ICategorisationObject cat in categorisations)
                    {
                        if (MaintainableUtil<ICategorySchemeObject>.Match(cateogrySchemeBean, cat.CategoryReference))
                        {
                            matchingCategorisations.Add(cat);
                        }
                    }

                    catSchemesType.CategoryScheme.Add(
                        this._categorySchemeXmlBuilder.Build(cateogrySchemeBean, categorisations));
                }
            }

            // GET CODELISTS
            if (buildFrom.Codelists.Count > 0)
            {
                CodeListsType codeListsType = new CodeListsType();
                returnType.CodeLists = codeListsType;
                //CodeListsType codeListsType = returnType.CodeLists;

                /* foreach */
                foreach (ICodelistObject codelistBean in buildFrom.Codelists)
                {
                    codeListsType.CodeList.Add(this._codelistXmlBuilder.Build(codelistBean));
                }
            }

            // CONCEPT SCHEMES
            if (buildFrom.ConceptSchemes.Count > 0)
            {
                ConceptsType conceptsType =  new ConceptsType();
                returnType.Concepts = conceptsType;

                /* foreach */
                foreach (IConceptSchemeObject conceptSchemeBean in buildFrom.ConceptSchemes)
                {
                    conceptsType.ConceptScheme.Add(this._conceptSchemeXmlBuilder.Build(conceptSchemeBean));
                }
            }

            // DATAFLOWS
            if (buildFrom.Dataflows.Count > 0)
            {
                
                var dataflowsType =  new DataflowsType();
                returnType.Dataflows = dataflowsType;

                /* foreach */
                foreach (IDataflowObject currentBean in buildFrom.Dataflows)
                {
                    dataflowsType.Dataflow.Add(
                        this._dataflowXmlBuilder.Build(currentBean, GetCategorisations(currentBean, categorisations)));
                }
            }

            // HIERARCIC CODELIST
            if (buildFrom.HierarchicalCodelists.Count > 0)
            {
                HierarchicalCodelistsType hierarchicalCodelistsType = new HierarchicalCodelistsType();
                returnType.HierarchicalCodelists = hierarchicalCodelistsType;

                /* foreach */
                foreach (IHierarchicalCodelistObject currentBean0 in buildFrom.HierarchicalCodelists)
                {
                    hierarchicalCodelistsType.HierarchicalCodelist.Add(
                        this._hierarchicalCodelistXmlBuilder.Build(currentBean0));
                }
            }

            // KEY FAMILY
            if (buildFrom.DataStructures.Count > 0)
            {
                var keyFamiliesType = new KeyFamiliesType();
                returnType.KeyFamilies = keyFamiliesType;

                /* foreach */
                foreach (IDataStructureObject currentBean1 in buildFrom.DataStructures)
                {
                    keyFamiliesType.KeyFamily.Add(this._dataStructureXmlBuilder.Build(currentBean1));
                }
            }

            // METADATA FLOW
            if (buildFrom.Metadataflows.Count > 0)
            {
                var metadataflowsType = new MetadataflowsType();
                returnType.Metadataflows = metadataflowsType;

                /* foreach */
                foreach (IMetadataFlow currentBean2 in buildFrom.Metadataflows)
                {
                    metadataflowsType.Metadataflow.Add(
                        this._metadataflowXmlBuilder.Build(
                            currentBean2, GetCategorisations(currentBean2, categorisations)));
                }
            }

            // METADATA STRUCTURE
            if (buildFrom.MetadataStructures.Count > 0)
            {
                var msdsType = new MetadataStructureDefinitionsType();
                returnType.MetadataStructureDefinitions = msdsType;

                /* foreach */
                foreach (IMetadataStructureDefinitionObject currentBean3 in buildFrom.MetadataStructures)
                {
                    msdsType.MetadataStructureDefinition.Add(
                        this._metadataStructureDefinitionXmlsBuilder.Build(currentBean3));
                }
            }

            OrganisationSchemesType orgSchemesType = null;

            // AGENCY SCHEMES
            if (buildFrom.AgenciesSchemes.Count > 0)
            {
                orgSchemesType = new OrganisationSchemesType();
                returnType.OrganisationSchemes = orgSchemesType;

                /* foreach */
                foreach (IAgencyScheme currentBean4 in buildFrom.AgenciesSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean4));
                }
            }

            // DATA CONSUMER SCHEMES
            if (buildFrom.DataConsumerSchemes.Count > 0)
            {
                if (orgSchemesType == null)
                {
                    orgSchemesType = new OrganisationSchemesType();
                    returnType.OrganisationSchemes = orgSchemesType;
                }

                /* foreach */
                foreach (IDataConsumerScheme currentBean5 in buildFrom.DataConsumerSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean5));
                }
            }

            // DATA PROVIDER SCHEMES
            if (buildFrom.DataProviderSchemes.Count > 0)
            {
                if (orgSchemesType == null)
                {
                    orgSchemesType = new OrganisationSchemesType();
                    returnType.OrganisationSchemes = orgSchemesType;
                }

                /* foreach */
                foreach (IDataProviderScheme currentBean6 in buildFrom.DataProviderSchemes)
                {
                    orgSchemesType.OrganisationScheme.Add(this._organisationSchemeXmlBuilder.Build(currentBean6));
                }
            }

            // PROCESSES
            if (buildFrom.Processes.Count > 0)
            {
                var processesType = new ProcessesType();
                returnType.Processes = processesType;

                /* foreach */
                foreach (IProcessObject currentBean7 in buildFrom.Processes)
                {
                    processesType.Process.Add(this._processXmlBuilder.Build(currentBean7));
                }
            }

            // STRUCTURE SETS
            if (buildFrom.StructureSets.Count > 0)
            {
                var structureSetsType = new StructureSetsType();
                returnType.StructureSets = structureSetsType;

                /* foreach */
                foreach (IStructureSetObject currentBean8 in buildFrom.StructureSets)
                {
                    structureSetsType.StructureSet.Add(this._structureSetXmlBuilder.Build(currentBean8));
                }
            }

            // REPORTING TAXONOMIES
            if (buildFrom.ReportingTaxonomys.Count > 0)
            {
                var reportingTaxonomiesType = new ReportingTaxonomiesType();
                returnType.ReportingTaxonomies = reportingTaxonomiesType;

                /* foreach */
                foreach (IReportingTaxonomyObject currentBean9 in buildFrom.ReportingTaxonomys)
                {
                    reportingTaxonomiesType.ReportingTaxonomy.Add(this._reportingTaxonomyXmlBuilder.Build(currentBean9));
                }
            }

            if (buildFrom.AttachmentConstraints.Count > 0)
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "Attachment Constraint at SMDX v2.0 - please use SDMX v2.1");
            }

            if (buildFrom.ContentConstraintObjects.Count > 0)
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "Content Constraint at SMDX v2.0 - please use SDMX v2.1");
            }

            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the categorisations for <paramref name="maintainable"/>
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <returns>
        /// The set of <see cref="ICategorisationObject"/> for <paramref name="maintainable"/>
        /// </returns>
        private static ISet<ICategorisationObject> GetCategorisations(
            IMaintainableObject maintainable, IEnumerable<ICategorisationObject> categorisations)
        {
            ISet<ICategorisationObject> returnSet = new HashSet<ICategorisationObject>();

            /* foreach */
            foreach (ICategorisationObject cat in categorisations)
            {
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
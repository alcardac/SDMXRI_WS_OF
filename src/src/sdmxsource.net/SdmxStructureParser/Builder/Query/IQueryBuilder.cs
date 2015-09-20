// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds Query reference objects from SDMX query messages.
//   The SDMX queries can be structure queries, registration queries or provision queries
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    /// <summary>
    ///     Builds Query reference objects from SDMX query messages.
    ///     The SDMX queries can be structure queries, registration queries or provision queries
    /// </summary>
    public interface IQueryBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a list of structure references from a version 2.0 registry query structure request message
        /// </summary>
        /// <param name="queryStructureRequests">
        /// The Structure query
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(QueryStructureRequestType queryStructureRequests);

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query registration request message
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// The query Registration Request Type.
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.QueryRegistrationRequestType queryRegistrationRequestType);

        /// <summary>
        /// Builds a list of provision references from a version 2.1 registry query registration request message
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// Query registration request type
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(
            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryRegistrationRequestType queryRegistrationRequestType);

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query provision request message
        /// </summary>
        /// <param name="queryProvisionRequestType">
        /// The query for provision
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(QueryProvisioningRequestType queryProvisionRequestType);

        /// <summary>
        /// Builds a list of structure references from a version 1.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(QueryMessageType queryMessage);

        /// <summary>
        /// Builds a list of structure references from a version 2.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessageType queryMessage);

        /**
	     * Builds a {@link IComplexStructureQuery} from a version 2.1 Codelist query message
	     * @param codelistQueryMsg
	     * @return
	     */
        IComplexStructureQuery Build(CodelistQueryType codelistQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Dataflow query message
         * @param dataflowQueryMsg
         * @return
         */
        IComplexStructureQuery Build(DataflowQueryType dataflowQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Metadataflow query message
         * @param metadataflowQueryMsg
         * @return
         */
        IComplexStructureQuery Build(MetadataflowQueryType metadataflowQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Datastructure query message
         * @param metadataflowQueryMsg
         * @return
         */
        IComplexStructureQuery Build(DataStructureQueryType dataStructureQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Metadatastructure query message
         * @param metadataStructureQueryMsg
         * @return
         */
        IComplexStructureQuery Build(MetadataStructureQueryType metadataStructureQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 CategoryScheme query message
         * @param categorySchemeQueryMsg
         * @return
         */
        IComplexStructureQuery Build(CategorySchemeQueryType categorySchemeQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 ConceptScheme query message
         * @param conceptSchemeQueryMsg
         * @return
         */
        IComplexStructureQuery Build(ConceptSchemeQueryType conceptSchemeQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 HierarchicalCodelist query message
         * @param hierarchicalCodelistQueryMsg
         * @return
         */
        IComplexStructureQuery Build(HierarchicalCodelistQueryType hierarchicalCodelistQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 OrganisationScheme query message
         * @param organisationSchemeQueryMsg
         * @return
         */
        IComplexStructureQuery Build(OrganisationSchemeQueryType organisationSchemeQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 ReportingTaxonomy query message
         * @param reportingTaxonomyQueryMsg
         * @return
         */
        IComplexStructureQuery Build(ReportingTaxonomyQueryType reportingTaxonomyQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 StructureSet query message
         * @param structureSetQueryMsg
         * @return
         */
        IComplexStructureQuery Build(StructureSetQueryType structureSetQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Process query message
         * @param processQueryMsg
         * @return
         */
        IComplexStructureQuery Build(ProcessQueryType processQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Categorisation query message
         * @param categorisationQueryMsg
         * @return
         */
        IComplexStructureQuery Build(CategorisationQueryType categorisationQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 ProvisionAgreement query message
         * @param provisionAgreementQueryMsg
         * @return
         */
        IComplexStructureQuery Build(ProvisionAgreementQueryType provisionAgreementQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Constraint query message
         * @param constraintQueryMsg
         * @return
         */
        IComplexStructureQuery Build(ConstraintQueryType constraintQueryMsg);

        /**
         * Builds a {@link IComplexStructureQuery} from a version 2.1 Structures query message
         * @param structuresQueryMsg
         * @return
         */
        IComplexStructureQuery Build(StructuresQueryType structuresQueryMsg);

        #endregion
    }
}
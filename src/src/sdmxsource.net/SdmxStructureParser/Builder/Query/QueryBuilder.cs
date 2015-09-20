// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds query structure objects from SDMX query messages, this includes structure queries, registration queries and provision queries
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
    ///     Builds query structure objects from SDMX query messages, this includes structure queries, registration queries and provision queries
    /// </summary>
    public class QueryBuilder : IQueryBuilder
    {
        #region Fields

        /// <summary>
        ///     The query bean builder v 1.
        /// </summary>
        private readonly QueryBuilderV1 _queryBuilderV1 = new QueryBuilderV1();

        /// <summary>
        ///     The query bean builder v 2.
        /// </summary>
        private readonly QueryBuilderV2 _queryBuilderV2 = new QueryBuilderV2();

        /// <summary>
        ///     The query bean builder v 21.
        /// </summary>
        private readonly QueryBuilderV21 _queryBuilderV21 = new QueryBuilderV21();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        public QueryBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="queryBuilderV1">
        /// The query builder for SDMX v1.0. Use <c>null</c> to use the default.
        /// </param>
        /// <param name="queryBuilderV2">
        /// The query builder for SDMX v2.0. Use <c>null</c> to use the default.
        /// </param>
        /// <param name="queryBuilderV21">
        /// The query builder for SDMX v2.1. Use <c>null</c> to use the default.
        /// </param>
        public QueryBuilder(QueryBuilderV1 queryBuilderV1, QueryBuilderV2 queryBuilderV2, QueryBuilderV21 queryBuilderV21)
        {
            if (queryBuilderV1 != null)
            {
                this._queryBuilderV1 = queryBuilderV1;
            }

            if (queryBuilderV2 != null)
            {
                this._queryBuilderV2 = queryBuilderV2;
            }

            if (queryBuilderV21 != null)
            {
                this._queryBuilderV21 = queryBuilderV21;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// Builds a list of structure references from a version 2.0 registry query structure request message
        /// </summary>
        /// <param name="queryStructureRequests">
        /// The Query Structure Request (SDMX Registry Interface v2.0)
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        public virtual IList<IStructureReference> Build(QueryStructureRequestType queryStructureRequests)
        {
            return this._queryBuilderV2.Build(queryStructureRequests);
        }

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query registration request message
        ///     If only dataProviderRef is supplied then this is used and the flow type is assumed to be a dataflow
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// The query Registration Request Type.
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        public virtual IStructureReference Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.QueryRegistrationRequestType queryRegistrationRequestType)
        {
            return this._queryBuilderV2.Build(queryRegistrationRequestType);
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// The query registration request type.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        public virtual IStructureReference Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryRegistrationRequestType queryRegistrationRequestType)
        {
            return this._queryBuilderV21.Build(queryRegistrationRequestType);
        }

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query provision request message
        /// </summary>
        /// <param name="queryProvisionRequestType">
        /// The query Provision Request Type.
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        public virtual IStructureReference Build(QueryProvisioningRequestType queryProvisionRequestType)
        {
            return this._queryBuilderV2.Build(queryProvisionRequestType);
        }

        /// <summary>
        /// Builds a list of structure references from a version 1.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message SDMX v1.0
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        public virtual IList<IStructureReference> Build(QueryMessageType queryMessage)
        {
            return this._queryBuilderV1.Build(queryMessage);
        }

        /// <summary>
        /// Builds a list of structure references from a version 2.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message SDMX v2.0
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        public virtual IList<IStructureReference> Build(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessageType queryMessage)
        {
            return this._queryBuilderV2.Build(queryMessage);
        }


        public virtual IComplexStructureQuery Build(CodelistQueryType codelistQueryMessage)
        {
            return this._queryBuilderV21.Build(codelistQueryMessage);
        }

        public virtual IComplexStructureQuery Build(DataflowQueryType dataflowQueryMessage)
        {
            return this._queryBuilderV21.Build(dataflowQueryMessage);
        }

        public virtual IComplexStructureQuery Build(MetadataflowQueryType metadataflowQueryMessage)
        {
            return this._queryBuilderV21.Build(metadataflowQueryMessage);
        }

        public virtual IComplexStructureQuery Build(DataStructureQueryType dataStructureQueryMessage)
        {
            return this._queryBuilderV21.Build(dataStructureQueryMessage);
        }

        public virtual IComplexStructureQuery Build(MetadataStructureQueryType metadataStructureQueryMessage)
        {
            return this._queryBuilderV21.Build(metadataStructureQueryMessage);
        }

        public virtual IComplexStructureQuery Build(CategorySchemeQueryType categorySchemeQueryMessage)
        {
            return this._queryBuilderV21.Build(categorySchemeQueryMessage);
        }

        public virtual IComplexStructureQuery Build(ConceptSchemeQueryType conceptSchemeQueryMessage)
        {
            return _queryBuilderV21.Build(conceptSchemeQueryMessage);
        }

        public virtual IComplexStructureQuery Build(HierarchicalCodelistQueryType hierarchicalCodelistQueryMessage)
        {
            return _queryBuilderV21.Build(hierarchicalCodelistQueryMessage);
        }

        public virtual IComplexStructureQuery Build(OrganisationSchemeQueryType organisationSchemeQueryMessage)
        {
            return _queryBuilderV21.Build(organisationSchemeQueryMessage);
        }

        public virtual IComplexStructureQuery Build(ReportingTaxonomyQueryType reportingTaxonomyQueryMessage)
        {
            return _queryBuilderV21.Build(reportingTaxonomyQueryMessage);
        }

        public virtual IComplexStructureQuery Build(StructureSetQueryType structureSetQueryMessage)
        {
            return _queryBuilderV21.Build(structureSetQueryMessage);
        }

        public virtual IComplexStructureQuery Build(ProcessQueryType processQueryMessage)
        {
            return _queryBuilderV21.Build(processQueryMessage);
        }

        public virtual IComplexStructureQuery Build(CategorisationQueryType categorisationQueryMessage)
        {
            return _queryBuilderV21.Build(categorisationQueryMessage);
        }

        public virtual IComplexStructureQuery Build(ProvisionAgreementQueryType provisionAgreementQueryMessage)
        {
            return _queryBuilderV21.Build(provisionAgreementQueryMessage);
        }

        public virtual IComplexStructureQuery Build(ConstraintQueryType constraintQueryMessage)
        {
            return _queryBuilderV21.Build(constraintQueryMessage);
        }

        public virtual IComplexStructureQuery Build(StructuresQueryType structuresQueryMessage)
        {
            return _queryBuilderV21.Build(structuresQueryMessage);
        }

        #endregion
    }
}
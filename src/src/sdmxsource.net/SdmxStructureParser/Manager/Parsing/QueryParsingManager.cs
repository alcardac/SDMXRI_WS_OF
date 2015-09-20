// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query parsing manager
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;
    using Org.Sdmxsource.Sdmx.Util.Exception;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Log;

    using QueryMessage = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.QueryMessage;
    using QueryMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.QueryMessageType;
    using RegistryInterface = Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.RegistryInterface;

    /// <summary>
    ///     The query parsing manager
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="QueryParsingManager" />.
    ///     <code source="..\ReUsingExamples\StructureQuery\ReUsingQueryParsingManager.cs" lang="cs" />
    /// </example> 
    public class QueryParsingManager : BaseParsingManager, IQueryParsingManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(QueryParsingManager));

        #endregion

        #region Fields

        /// <summary>
        ///     The _query bean builder.
        /// </summary>
        private readonly IQueryBuilder _queryBuilder = new QueryBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <param name="queryBuilder">
        /// The query Builder. Overrides the default. Use <c>null</c> to use the default.
        /// </param>
        public QueryParsingManager(SdmxSchemaEnumType sdmxSchema, IQueryBuilder queryBuilder)
            : this(sdmxSchema)
        {
            if (queryBuilder != null)
            {
                this._queryBuilder = queryBuilder;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        public QueryParsingManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the SDMX at the given URI and returns a workspace containing the information on what was being queried.
        ///     <p/>
        ///     The Query parsing manager processes queries that are in a RegistryInterface document, this includes queries for
        ///     Provisions, Registrations and Structures.  It also processes Queries that are in a QueryMessage document
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the query
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/> from <paramref name="dataLocation"/>.
        /// </returns>
        public virtual IQueryWorkspace ParseQueries(IReadableDataLocation dataLocation)
        {
            LoggingUtil.Debug(_log, "Parse Structure request, for xml at location: " + dataLocation);
            using (ISdmxXmlStream xmlStream = new SdmxXmlStream(dataLocation))
            {
                return this.ParseQueries(xmlStream);
            }
        }

        /// <summary>
        /// Processes the SDMX at the given URI and returns a workspace containing the information on what was being queried.
        ///     <p/>
        ///     The Query parsing manager processes queries that are in a RegistryInterface document, this includes queries for
        ///     Provisions, Registrations and Structures.  It also processes Queries that are in a QueryMessage document
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the query
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/> from <paramref name="dataLocation"/>.
        /// </returns>
        public virtual IQueryWorkspace ParseQueries(ISdmxXmlStream dataLocation)
        {
            LoggingUtil.Debug(_log, "Parse Structure request, for xml at location: " + dataLocation);

            SdmxSchemaEnumType schemaVersion = dataLocation.SdmxVersion;

            // NOTE validation is performed by XmlReader from XMLParser.CreateSdmxMlReader
            ////XMLParser.ValidateXml(dataLocation, schemaVersion);
            LoggingUtil.Debug(_log, "XML VALID");
            MessageEnumType messageType = dataLocation.MessageType;
            using (XmlReader reader = dataLocation.Reader)
            {
                if (schemaVersion == SdmxSchemaEnumType.VersionOne || schemaVersion == SdmxSchemaEnumType.VersionTwo)
                {
                    switch (messageType)
                    {
                        case MessageEnumType.Query:
                            return this.ProcessQueryMessage(reader, schemaVersion);
                        case MessageEnumType.RegistryInterface:
                            {
                                RegistryMessageEnumType registryMessageType = dataLocation.RegistryType;
                                RegistryMessageType registryMessage = RegistryMessageType.GetFromEnum(registryMessageType);
                                if (registryMessage.IsQueryRequest())
                                {
                                    try
                                    {
                                        return this.ProcessRegistryQueryMessage(reader, schemaVersion, registryMessageType);
                                    }
                                    catch (Exception th)
                                    {
                                        throw new ParseException(th, DatasetActionEnumType.Information, false, registryMessage.ArtifactType);
                                    }
                                }

                                throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Expected query message - type found : " + registryMessageType);
                            }
                    }
                }
                else if (schemaVersion == SdmxSchemaEnumType.VersionTwoPointOne)
                {
                    if (messageType == MessageEnumType.RegistryInterface)
                    {
                        RegistryMessageEnumType registryMessageType = dataLocation.RegistryType;
                        return ProcessRegistryQueryMessage(reader, schemaVersion, registryMessageType);
                    }
                    else
                    {
                        QueryMessageEnumType queryMessageType = dataLocation.QueryMessageTypes.FirstOrDefault(); // Only one *Where element according to 2.1 Schema
                        if (messageType != MessageEnumType.Query)
                        {
                            //TODO should this be IllegalArgumentException?
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Not a structure query message:" + queryMessageType);
                        }
                        return ProcessQueryMessage(reader, queryMessageType);
                    }
                }
            }

            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, messageType);
        }

        #endregion

        #region Methods

        /// <summary>
        /// processes all v2.1 structure Query messages and build the {@link ComplexStructureQuery}
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="queryMessageType">Type of the query message.</param>
        /// <returns>a parsed {@link ComplexStructureQuery} set in the {@link QueryWorkspace} </returns>
        /// <exception cref="System.ArgumentException">Not a structure query message: + queryMessageType</exception>
        private IQueryWorkspace ProcessQueryMessage(XmlReader reader, QueryMessageEnumType queryMessageType)
        {
            IComplexStructureQuery complexQuery;
            switch (queryMessageType)
            {
                case QueryMessageEnumType.StructuresWhere:
                    StructuresQuery structuresQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<StructuresQuery, StructuresQueryType>(reader);
                    complexQuery = _queryBuilder.Build(structuresQuery.Content);
                    break;
                case QueryMessageEnumType.DataflowWhere:
                    DataflowQuery dataflowQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<DataflowQuery, DataflowQueryType>(reader);
                    complexQuery = _queryBuilder.Build(dataflowQuery.Content);
                    break;
                case QueryMessageEnumType.MetadataflowWhere:
                    MetadataflowQuery metadataflowQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<MetadataflowQuery, MetadataflowQueryType>(reader);
                    complexQuery = _queryBuilder.Build(metadataflowQuery.Content);
                    break;
                case QueryMessageEnumType.DsdWhere:
                    DataStructureQuery dataStructureQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<DataStructureQuery, DataStructureQueryType>(reader);
                    complexQuery = _queryBuilder.Build(dataStructureQuery.Content);
                    break;
                case QueryMessageEnumType.MdsWhere:
                    MetadataStructureQuery metadataStructureQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<MetadataStructureQuery, MetadataStructureQueryType>(reader);
                    complexQuery = _queryBuilder.Build(metadataStructureQuery.Content);
                    break;
                case QueryMessageEnumType.CategorySchemeWhere:
                    CategorySchemeQuery categorySchemeQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<CategorySchemeQuery, CategorySchemeQueryType>(reader);
                    complexQuery = _queryBuilder.Build(categorySchemeQuery.Content);
                    break;
                case QueryMessageEnumType.ConceptSchemeWhere:
                    ConceptSchemeQuery conceptSchemeQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<ConceptSchemeQuery, ConceptSchemeQueryType>(reader);
                    complexQuery = _queryBuilder.Build(conceptSchemeQuery.Content);
                    break;
                case QueryMessageEnumType.CodelistWhere:
                    CodelistQuery codelistQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<CodelistQuery, CodelistQueryType>(reader);
                    complexQuery = _queryBuilder.Build(codelistQuery.Content);
                    break;
                case QueryMessageEnumType.HclWhere:
                    HierarchicalCodelistQuery hierarchicalCodelistQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<HierarchicalCodelistQuery, HierarchicalCodelistQueryType>(reader);
                    complexQuery = _queryBuilder.Build(hierarchicalCodelistQuery.Content);
                    break;
                case QueryMessageEnumType.OrganisationSchemeWhere:
                    OrganisationSchemeQuery organisationSchemeQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<OrganisationSchemeQuery, OrganisationSchemeQueryType>(reader);
                    complexQuery = _queryBuilder.Build(organisationSchemeQuery.Content);
                    break;
                case QueryMessageEnumType.ReportingTaxonomyWhere:
                    ReportingTaxonomyQuery reportingTaxonomyQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<ReportingTaxonomyQuery, ReportingTaxonomyQueryType>(reader);
                    complexQuery = _queryBuilder.Build(reportingTaxonomyQuery.Content);
                    break;
                case QueryMessageEnumType.StructureSetWhere:
                    StructureSetQuery structureSetQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<StructureSetQuery, StructureSetQueryType>(reader);
                    complexQuery = _queryBuilder.Build(structureSetQuery.Content);
                    break;
                case QueryMessageEnumType.ProcessWhere:
                    ProcessQuery processQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<ProcessQuery, ProcessQueryType>(reader);
                    complexQuery = _queryBuilder.Build(processQuery.Content);
                    break;
                case QueryMessageEnumType.CategorisationWhere:
                    CategorisationQuery categorisationQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<CategorisationQuery, CategorisationQueryType>(reader);
                    complexQuery = _queryBuilder.Build(categorisationQuery.Content);
                    break;
                case QueryMessageEnumType.ProvisionAgreementWhere:
                    ProvisionAgreementQuery provisionAgreementQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<ProvisionAgreementQuery, ProvisionAgreementQueryType>(reader);
                    complexQuery = _queryBuilder.Build(provisionAgreementQuery.Content);
                    break;
                case QueryMessageEnumType.ConstraintWhere:
                    ConstraintQuery constraintQuery = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<ConstraintQuery, ConstraintQueryType>(reader);
                    complexQuery = _queryBuilder.Build(constraintQuery.Content);
                    break;
                default:
                    throw new ArgumentException("Not a structure query message:" + queryMessageType);
            }

            return new QueryWorkspace(complexQuery);
        }

        /// <summary>
        /// Processes a query message which is a QueryMessage Document
        /// </summary>
        /// <param name="reader">
        /// the input stream reader
        /// </param>
        /// <param name="schemaVersion">
        /// the SDMX schema version
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// The value of <paramref name="schemaVersion"/> is not supported.
        /// </exception>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        private IQueryWorkspace ProcessQueryMessage(XmlReader reader, SdmxSchemaEnumType schemaVersion)
        {
            IList<IStructureReference> structureReferences;

            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionOne:
                    QueryMessage docV1 = MessageFactory.Load<QueryMessage, QueryMessageType>(reader);
                    structureReferences = this._queryBuilder.Build(docV1.Content);
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessage docV2 =
                        Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessage.Load(reader);
                    structureReferences = this._queryBuilder.Build(docV2.Content);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }

            return new QueryWorkspace(null, null, structureReferences, false);
        }

        /// <summary>
        /// Processes a query message which is a RegistryInterface Document
        /// </summary>
        /// <param name="reader">
        /// - the stream containing the SDMX
        /// </param>
        /// <param name="schemaVersion">
        /// - the schema version that the SDMX is in
        /// </param>
        /// <param name="registryMessageType">
        /// - the type of query message, provision, registration or structure
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        private IQueryWorkspace ProcessRegistryQueryMessage(
            XmlReader reader, SdmxSchemaEnumType schemaVersion, RegistryMessageEnumType registryMessageType)
        {
            switch (registryMessageType)
            {
                case RegistryMessageEnumType.QueryProvisionRequest:
                    return this.ProcessRegistryQueryMessageForProvision(reader, schemaVersion);
                case RegistryMessageEnumType.QueryRegistrationRequest:
                    return this.ProcessRegistryQueryMessageForRegistration(reader, schemaVersion);
                case RegistryMessageEnumType.QueryStructureRequest:
                    return this.ProcessRegistryQueryMessageForStructures(reader, schemaVersion);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, registryMessageType);
            }
        }

        /// <summary>
        /// The process registry query message for provision.
        /// </summary>
        /// <param name="reader">
        /// The mask 0.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Unsupported value at <paramref name="schemaVersion"/>
        /// </exception>
        private IQueryWorkspace ProcessRegistryQueryMessageForProvision(
            XmlReader reader, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    RegistryInterface doc = RegistryInterface.Load(reader);
                    IStructureReference provisionReferences = this._queryBuilder.Build(doc.QueryProvisioningRequest);
                    return new QueryWorkspace(provisionReferences, null, null, false);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

        /// <summary>
        /// The process registry query message for registration.
        /// </summary>
        /// <param name="reader">
        /// The mask 0.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Unsupported value at <paramref name="schemaVersion"/>
        /// </exception>
        private IQueryWorkspace ProcessRegistryQueryMessageForRegistration(
            XmlReader reader, SdmxSchemaEnumType schemaVersion)
        {
            IStructureReference registrationReferences;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    RegistryInterface doc = RegistryInterface.Load(reader);
                    registrationReferences = this._queryBuilder.Build(doc.QueryRegistrationRequest);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface doc21 =
                        Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface.Load(reader);
                    registrationReferences = this._queryBuilder.Build(doc21.Content.QueryRegistrationRequest);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }

            return new QueryWorkspace(null, registrationReferences, null, false);
        }

        /// <summary>
        /// The process registry query message for structures.
        /// </summary>
        /// <param name="reader">
        /// The mask 0.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Unsupported value at <paramref name="schemaVersion"/>
        /// </exception>
        private IQueryWorkspace ProcessRegistryQueryMessageForStructures(
            XmlReader reader, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    RegistryInterface doc = RegistryInterface.Load(reader);
                    IList<IStructureReference> structureReferences = this._queryBuilder.Build(doc.QueryStructureRequest);
                    bool resolveRefernces = doc.QueryStructureRequest.resolveReferences;
                    return new QueryWorkspace(null, null, structureReferences, resolveRefernces);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

        #endregion
    }
}
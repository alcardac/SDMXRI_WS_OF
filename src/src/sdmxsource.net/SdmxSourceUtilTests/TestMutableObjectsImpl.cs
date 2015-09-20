// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMutableObjectsImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="MutableObjectsImpl" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     Test unit class for <see cref="MutableObjectsImpl" />
    /// </summary>
    [TestFixture]
    public class TestMutableObjectsImpl
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test method for <see cref="MutableObjectsImpl" />
        /// </summary>
        [Test]
        public void Test()
        {
            var s = new MutableObjectsImpl();
            var s3 = new SdmxObjectsImpl(DatasetAction.GetFromEnum(DatasetActionEnumType.Append));

            Assert.AreEqual(s3.Action.EnumType, DatasetActionEnumType.Append);

            Assert.IsNotNull(s.AllMaintainables);
            CollectionAssert.IsEmpty(s.AllMaintainables);
            Assert.AreNotSame(s.AllMaintainables, s.AllMaintainables);

            Assert.IsNotNull(s.Categorisations);
            CollectionAssert.IsEmpty(s.Categorisations);
            Assert.AreSame(s.Categorisations, s.Categorisations);

            Assert.IsNotNull(s.CategorySchemes);
            CollectionAssert.IsEmpty(s.CategorySchemes);
            Assert.AreSame(s.CategorySchemes, s.CategorySchemes);

            Assert.IsNotNull(s.Codelists);
            CollectionAssert.IsEmpty(s.Codelists);
            Assert.AreSame(s.Codelists, s.Codelists);

            Assert.IsNotNull(s.ConceptSchemes);
            CollectionAssert.IsEmpty(s.ConceptSchemes);
            Assert.AreSame(s.ConceptSchemes, s.ConceptSchemes);

            Assert.IsNotNull(s.DataStructures);
            CollectionAssert.IsEmpty(s.DataStructures);
            Assert.AreSame(s.DataStructures, s.DataStructures);

            Assert.IsNotNull(s.Dataflows);
            CollectionAssert.IsEmpty(s.Dataflows);
            Assert.AreSame(s.Dataflows, s.Dataflows);

            Assert.IsNotNull(s.HierarchicalCodelists);
            CollectionAssert.IsEmpty(s.HierarchicalCodelists);
            Assert.AreSame(s.HierarchicalCodelists, s.HierarchicalCodelists);

            Assert.IsNotNull(s.MetadataStructures);
            CollectionAssert.IsEmpty(s.MetadataStructures);
            Assert.AreSame(s.MetadataStructures, s.MetadataStructures);

            Assert.IsNotNull(s.Metadataflows);
            CollectionAssert.IsEmpty(s.Metadataflows);
            Assert.AreSame(s.Metadataflows, s.Metadataflows);

            Assert.IsNotNull(s.OrganisationUnitSchemes);
            CollectionAssert.IsEmpty(s.OrganisationUnitSchemes);
            Assert.AreSame(s.OrganisationUnitSchemes, s.OrganisationUnitSchemes);

            Assert.IsNotNull(s.Processes);
            CollectionAssert.IsEmpty(s.Processes);
            Assert.AreSame(s.Processes, s.Processes);

            Assert.IsNotNull(s.Registrations);
            CollectionAssert.IsEmpty(s.Registrations);
            Assert.AreSame(s.Registrations, s.Registrations);

            Assert.IsNotNull(s.ReportingTaxonomys);
            CollectionAssert.IsEmpty(s.ReportingTaxonomys);
            Assert.AreSame(s.ReportingTaxonomys, s.ReportingTaxonomys);

            Assert.IsNotNull(s.StructureSets);
            CollectionAssert.IsEmpty(s.StructureSets);
            Assert.AreSame(s.StructureSets, s.StructureSets);

            Assert.IsNotNull(s.Subscriptions);
            CollectionAssert.IsEmpty(s.Subscriptions);
            Assert.AreSame(s.Subscriptions, s.Subscriptions);

            var agencySchemeMock = new Mock<IAgencySchemeMutableObject>();
            agencySchemeMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme));

            agencySchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            agencySchemeMock.Setup(o => o.Id).Returns("ID_AGENCYSCHEME");
            agencySchemeMock.Setup(o => o.Version).Returns("1.2");
            agencySchemeMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme));
            IAgencySchemeMutableObject agencyScheme = agencySchemeMock.Object;
            s.AddAgencyScheme(agencyScheme);
            CollectionAssert.IsNotEmpty(s.AgencySchemeMutableObjects);
            s.RemoveAgencySchemeMutableObjects(agencyScheme);
            CollectionAssert.IsEmpty(s.AgencySchemeMutableObjects);
            s.AddIdentifiable(agencyScheme);
            CollectionAssert.IsNotEmpty(s.AgencySchemeMutableObjects);

            var categorisationObjectMock = new Mock<ICategorisationMutableObject>();
            categorisationObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation));

            categorisationObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            categorisationObjectMock.Setup(o => o.Id).Returns("ID_CATEGORISATIONOBJECT");
            categorisationObjectMock.Setup(o => o.Version).Returns("1.2");
            ICategorisationMutableObject categorisationObject = categorisationObjectMock.Object;
            s.AddCategorisation(categorisationObject);
            CollectionAssert.IsNotEmpty(s.Categorisations);
            s.RemoveCategorisation(categorisationObject);
            CollectionAssert.IsEmpty(s.Categorisations);
            s.AddIdentifiable(categorisationObject);
            CollectionAssert.IsNotEmpty(s.Categorisations);

            var categorySchemeObjectMock = new Mock<ICategorySchemeMutableObject>();
            categorySchemeObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));

            categorySchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            categorySchemeObjectMock.Setup(o => o.Id).Returns("ID_CATEGORYSCHEMEOBJECT");
            categorySchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            ICategorySchemeMutableObject categorySchemeObject = categorySchemeObjectMock.Object;
            s.AddCategoryScheme(categorySchemeObject);
            CollectionAssert.IsNotEmpty(s.CategorySchemes);
            s.RemoveCategoryScheme(categorySchemeObject);
            CollectionAssert.IsEmpty(s.CategorySchemes);
            s.AddIdentifiable(categorySchemeObject);
            CollectionAssert.IsNotEmpty(s.CategorySchemes);

            var codelistObjectMock = new Mock<ICodelistMutableObject>();
            codelistObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));

            codelistObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            codelistObjectMock.Setup(o => o.Id).Returns("ID_CODELISTOBJECT");
            codelistObjectMock.Setup(o => o.Version).Returns("1.2");
            ICodelistMutableObject codelistObject = codelistObjectMock.Object;
            s.AddCodelist(codelistObject);
            CollectionAssert.IsNotEmpty(s.Codelists);
            s.RemoveCodelist(codelistObject);
            CollectionAssert.IsEmpty(s.Codelists);
            s.AddIdentifiable(codelistObject);
            CollectionAssert.IsNotEmpty(s.Codelists);

            var conceptSchemeObjectMock = new Mock<IConceptSchemeMutableObject>();
            conceptSchemeObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));

            conceptSchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            conceptSchemeObjectMock.Setup(o => o.Id).Returns("ID_CONCEPTSCHEMEOBJECT");
            conceptSchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            IConceptSchemeMutableObject conceptSchemeObject = conceptSchemeObjectMock.Object;
            s.AddConceptScheme(conceptSchemeObject);
            CollectionAssert.IsNotEmpty(s.ConceptSchemes);
            s.RemoveConceptScheme(conceptSchemeObject);
            CollectionAssert.IsEmpty(s.ConceptSchemes);
            s.AddIdentifiable(conceptSchemeObject);
            CollectionAssert.IsNotEmpty(s.ConceptSchemes);

            var dataConsumerSchemeMock = new Mock<IDataConsumerSchemeMutableObject>();
            dataConsumerSchemeMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme));

            dataConsumerSchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataConsumerSchemeMock.Setup(o => o.Id).Returns("ID_DATACONSUMERSCHEME");
            dataConsumerSchemeMock.Setup(o => o.Version).Returns("1.2");
            IDataConsumerSchemeMutableObject dataConsumerScheme = dataConsumerSchemeMock.Object;
            s.AddDataConsumerScheme(dataConsumerScheme);
            CollectionAssert.IsNotEmpty(s.DataConsumberSchemeMutableObjects);
            s.RemoveDataConsumberSchemeMutableObjects(dataConsumerScheme);
            CollectionAssert.IsEmpty(s.DataConsumberSchemeMutableObjects);
            s.AddIdentifiable(dataConsumerScheme);
            CollectionAssert.IsNotEmpty(s.DataConsumberSchemeMutableObjects);

            var dataProviderSchemeMock = new Mock<IDataProviderSchemeMutableObject>();
            dataProviderSchemeMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme));

            dataProviderSchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataProviderSchemeMock.Setup(o => o.Id).Returns("ID_DATAPROVIDERSCHEME");
            dataProviderSchemeMock.Setup(o => o.Version).Returns("1.2");
            IDataProviderSchemeMutableObject dataProviderScheme = dataProviderSchemeMock.Object;
            s.AddDataProviderScheme(dataProviderScheme);
            CollectionAssert.IsNotEmpty(s.DataProviderSchemeMutableObjects);
            s.RemoveDataProviderSchemeMutableObjects(dataProviderScheme);
            CollectionAssert.IsEmpty(s.DataProviderSchemeMutableObjects);
            s.AddIdentifiable(dataProviderScheme);
            CollectionAssert.IsNotEmpty(s.DataProviderSchemeMutableObjects);

            var dataStructureObjectMock = new Mock<IDataStructureMutableObject>();
            dataStructureObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));

            dataStructureObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataStructureObjectMock.Setup(o => o.Id).Returns("ID_DATASTRUCTUREOBJECT");
            dataStructureObjectMock.Setup(o => o.Version).Returns("1.2");
            IDataStructureMutableObject dataStructureObject = dataStructureObjectMock.Object;
            s.AddDataStructure(dataStructureObject);
            CollectionAssert.IsNotEmpty(s.DataStructures);
            s.RemoveDataStructure(dataStructureObject);
            CollectionAssert.IsEmpty(s.DataStructures);
            s.AddIdentifiable(dataStructureObject);
            CollectionAssert.IsNotEmpty(s.DataStructures);

            var dataflowObjectMock = new Mock<IDataflowMutableObject>();
            dataflowObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));

            dataflowObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataflowObjectMock.Setup(o => o.Id).Returns("ID_DATAFLOWOBJECT");
            dataflowObjectMock.Setup(o => o.Version).Returns("1.2");
            IDataflowMutableObject dataflowObject = dataflowObjectMock.Object;
            s.AddDataflow(dataflowObject);
            CollectionAssert.IsNotEmpty(s.Dataflows);
            s.RemoveDataflow(dataflowObject);
            CollectionAssert.IsEmpty(s.Dataflows);
            s.AddIdentifiable(dataflowObject);
            CollectionAssert.IsNotEmpty(s.Dataflows);

            var hierarchicalCodelistObjectMock = new Mock<IHierarchicalCodelistMutableObject>();
            hierarchicalCodelistObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist));

            hierarchicalCodelistObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            hierarchicalCodelistObjectMock.Setup(o => o.Id).Returns("ID_HIERARCHICALCODELISTOBJECT");
            hierarchicalCodelistObjectMock.Setup(o => o.Version).Returns("1.2");
            IHierarchicalCodelistMutableObject hierarchicalCodelistObject = hierarchicalCodelistObjectMock.Object;
            s.AddHierarchicalCodelist(hierarchicalCodelistObject);
            CollectionAssert.IsNotEmpty(s.HierarchicalCodelists);
            s.RemoveHierarchicalCodelist(hierarchicalCodelistObject);
            CollectionAssert.IsEmpty(s.HierarchicalCodelists);
            s.AddIdentifiable(hierarchicalCodelistObject);
            CollectionAssert.IsNotEmpty(s.HierarchicalCodelists);

            var metadataFlowMock = new Mock<IMetadataFlowMutableObject>();
            metadataFlowMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));

            metadataFlowMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            metadataFlowMock.Setup(o => o.Id).Returns("ID_METADATAFLOW");
            metadataFlowMock.Setup(o => o.Version).Returns("1.2");
            IMetadataFlowMutableObject metadataFlow = metadataFlowMock.Object;
            s.AddMetadataFlow(metadataFlow);
            CollectionAssert.IsNotEmpty(s.Metadataflows);
            s.RemoveMetadataFlow(metadataFlow);
            CollectionAssert.IsEmpty(s.Metadataflows);
            s.AddIdentifiable(metadataFlow);
            CollectionAssert.IsNotEmpty(s.Metadataflows);

            var metadataStructureDefinitionObjectMock = new Mock<IMetadataStructureDefinitionMutableObject>();
            metadataStructureDefinitionObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));

            metadataStructureDefinitionObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            metadataStructureDefinitionObjectMock.Setup(o => o.Id).Returns("ID_METADATASTRUCTUREDEFINITIONOBJECT");
            metadataStructureDefinitionObjectMock.Setup(o => o.Version).Returns("1.2");
            IMetadataStructureDefinitionMutableObject metadataStructureDefinitionObject = metadataStructureDefinitionObjectMock.Object;
            s.AddMetadataStructure(metadataStructureDefinitionObject);
            CollectionAssert.IsNotEmpty(s.MetadataStructures);
            s.RemoveMetadataStructure(metadataStructureDefinitionObject);
            CollectionAssert.IsEmpty(s.MetadataStructures);
            s.AddIdentifiable(metadataStructureDefinitionObject);
            CollectionAssert.IsNotEmpty(s.MetadataStructures);

            var organisationUnitSchemeObjectMock = new Mock<IOrganisationUnitSchemeMutableObject>();
            organisationUnitSchemeObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme));

            organisationUnitSchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            organisationUnitSchemeObjectMock.Setup(o => o.Id).Returns("ID_ORGANISATIONUNITSCHEMEOBJECT");
            organisationUnitSchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            IOrganisationUnitSchemeMutableObject organisationUnitSchemeObject = organisationUnitSchemeObjectMock.Object;
            s.AddOrganisationUnitScheme(organisationUnitSchemeObject);
            CollectionAssert.IsNotEmpty(s.OrganisationUnitSchemes);
            s.RemoveOrganisationUnitScheme(organisationUnitSchemeObject);
            CollectionAssert.IsEmpty(s.OrganisationUnitSchemes);
            s.AddIdentifiable(organisationUnitSchemeObject);
            CollectionAssert.IsNotEmpty(s.OrganisationUnitSchemes);

            var processObjectMock = new Mock<IProcessMutableObject>();
            processObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process));

            processObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            processObjectMock.Setup(o => o.Id).Returns("ID_PROCESSOBJECT");
            processObjectMock.Setup(o => o.Version).Returns("1.2");
            IProcessMutableObject processObject = processObjectMock.Object;
            s.AddProcess(processObject);
            CollectionAssert.IsNotEmpty(s.Processes);
            s.RemoveProcess(processObject);
            CollectionAssert.IsEmpty(s.Processes);
            s.AddIdentifiable(processObject);
            CollectionAssert.IsNotEmpty(s.Processes);

            var provisionAgreementObjectMock = new Mock<IProvisionAgreementMutableObject>();
            provisionAgreementObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement));

            provisionAgreementObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            provisionAgreementObjectMock.Setup(o => o.Id).Returns("ID_PROVISIONAGREEMENTOBJECT");
            provisionAgreementObjectMock.Setup(o => o.Version).Returns("1.2");
            IProvisionAgreementMutableObject provisionAgreementObject = provisionAgreementObjectMock.Object;
            s.AddProvision(provisionAgreementObject);
            CollectionAssert.IsNotEmpty(s.Provisions);
            s.RemoveProvision(provisionAgreementObject);
            CollectionAssert.IsEmpty(s.Provisions);
            s.AddIdentifiable(provisionAgreementObject);
            CollectionAssert.IsNotEmpty(s.Provisions);

            var registrationObjectMock = new Mock<IRegistrationMutableObject>();
            registrationObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration));

            registrationObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            registrationObjectMock.Setup(o => o.Id).Returns("ID_REGISTRATIONOBJECT");
            registrationObjectMock.Setup(o => o.Version).Returns("1.2");
            IRegistrationMutableObject registrationObject = registrationObjectMock.Object;
            s.AddRegistration(registrationObject);
            CollectionAssert.IsNotEmpty(s.Registrations);
            s.RemoveRegistration(registrationObject);
            CollectionAssert.IsEmpty(s.Registrations);
            s.AddIdentifiable(registrationObject);
            CollectionAssert.IsNotEmpty(s.Registrations);

            var reportingTaxonomyObjectMock = new Mock<IReportingTaxonomyMutableObject>();
            reportingTaxonomyObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy));

            reportingTaxonomyObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            reportingTaxonomyObjectMock.Setup(o => o.Id).Returns("ID_REPORTINGTAXONOMYOBJECT");
            reportingTaxonomyObjectMock.Setup(o => o.Version).Returns("1.2");
            IReportingTaxonomyMutableObject reportingTaxonomyObject = reportingTaxonomyObjectMock.Object;
            s.AddReportingTaxonomy(reportingTaxonomyObject);
            CollectionAssert.IsNotEmpty(s.ReportingTaxonomys);
            s.RemoveReportingTaxonomy(reportingTaxonomyObject);
            CollectionAssert.IsEmpty(s.ReportingTaxonomys);
            s.AddIdentifiable(reportingTaxonomyObject);
            CollectionAssert.IsNotEmpty(s.ReportingTaxonomys);

            var structureSetObjectMock = new Mock<IStructureSetMutableObject>();
            structureSetObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet));

            structureSetObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            structureSetObjectMock.Setup(o => o.Id).Returns("ID_STRUCTURESETOBJECT");
            structureSetObjectMock.Setup(o => o.Version).Returns("1.2");
            IStructureSetMutableObject structureSetObject = structureSetObjectMock.Object;
            s.AddStructureSet(structureSetObject);
            CollectionAssert.IsNotEmpty(s.StructureSets);
            s.RemoveStructureSet(structureSetObject);
            CollectionAssert.IsEmpty(s.StructureSets);
            s.AddIdentifiable(structureSetObject);
            CollectionAssert.IsNotEmpty(s.StructureSets);

            var subscriptionObjectMock = new Mock<ISubscriptionMutableObject>();
            subscriptionObjectMock.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription));

            subscriptionObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            subscriptionObjectMock.Setup(o => o.Id).Returns("ID_SUBSCRIPTIONOBJECT");
            subscriptionObjectMock.Setup(o => o.Version).Returns("1.2");
            ISubscriptionMutableObject subscriptionObject = subscriptionObjectMock.Object;
            s.AddSubscription(subscriptionObject);
            CollectionAssert.IsNotEmpty(s.Subscriptions);
            s.RemoveSubscription(subscriptionObject);
            CollectionAssert.IsEmpty(s.Subscriptions);
            s.AddIdentifiable(subscriptionObject);
            CollectionAssert.IsNotEmpty(s.Subscriptions);

            var s5 = new MutableObjectsImpl(s.Dataflows);
            CollectionAssert.IsNotEmpty(s5.Dataflows);
            CollectionAssert.IsEmpty(s5.GetMaintainables(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)));
            CollectionAssert.IsNotEmpty(s5.GetMaintainables(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow)));
        }

        #endregion
    }
}
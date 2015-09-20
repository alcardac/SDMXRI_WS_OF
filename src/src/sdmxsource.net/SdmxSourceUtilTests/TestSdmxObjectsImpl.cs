// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxObjectsImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="SdmxObjectsImpl" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit for <see cref="SdmxObjectsImpl" />
    /// </summary>
    [TestFixture]
    public class TestSdmxObjectsImpl 
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test unit for <see cref="SdmxObjectsImpl" />
        /// </summary>
        [Test]
        public void Test()
        {
            var s = new SdmxObjectsImpl();
            var s2 = new SdmxObjectsImpl(new HeaderImpl("PAOK", "OLE"));
            Assert.IsFalse(s2.HasStructures());
            Assert.NotNull(s2.Header);
            var s3 = new SdmxObjectsImpl(DatasetAction.GetFromEnum(DatasetActionEnumType.Append));

            Assert.AreEqual(s.Action.EnumType, DatasetActionEnumType.Information);
            Assert.AreEqual(s3.Action.EnumType, DatasetActionEnumType.Append);
            Assert.IsNull(s.Header);
            Assert.IsNotNull(s.Agencies);
            CollectionAssert.IsEmpty(s.Agencies);
            Assert.AreNotSame(s.Agencies, s.Agencies);

            Assert.IsNotNull(s.AgenciesSchemes);
            CollectionAssert.IsEmpty(s.AgenciesSchemes);
            Assert.AreNotSame(s.AgenciesSchemes, s.AgenciesSchemes);

            Assert.IsNotNull(s.AllMaintainables);
            CollectionAssert.IsEmpty(s.AllMaintainables);
            Assert.AreNotSame(s.AllMaintainables, s.AllMaintainables);

            Assert.IsNotNull(s.AttachmentConstraints);
            CollectionAssert.IsEmpty(s.AttachmentConstraints);
            Assert.AreNotSame(s.AttachmentConstraints, s.AttachmentConstraints);

            Assert.IsNotNull(s.Categorisations);
            CollectionAssert.IsEmpty(s.Categorisations);
            Assert.AreNotSame(s.Categorisations, s.Categorisations);

            Assert.IsNotNull(s.CategorySchemes);
            CollectionAssert.IsEmpty(s.CategorySchemes);
            Assert.AreNotSame(s.CategorySchemes, s.CategorySchemes);

            Assert.IsNotNull(s.Codelists);
            CollectionAssert.IsEmpty(s.Codelists);
            Assert.AreNotSame(s.Codelists, s.Codelists);

            Assert.IsNotNull(s.ConceptSchemes);
            CollectionAssert.IsEmpty(s.ConceptSchemes);
            Assert.AreNotSame(s.ConceptSchemes, s.ConceptSchemes);

            Assert.IsNotNull(s.ContentConstraintObjects);
            CollectionAssert.IsEmpty(s.ContentConstraintObjects);
            Assert.AreNotSame(s.ContentConstraintObjects, s.ContentConstraintObjects);

            Assert.IsNotNull(s.DataConsumerSchemes);
            CollectionAssert.IsEmpty(s.DataConsumerSchemes);
            Assert.AreNotSame(s.DataConsumerSchemes, s.DataConsumerSchemes);

            Assert.IsNotNull(s.DataProviderSchemes);
            CollectionAssert.IsEmpty(s.DataProviderSchemes);
            Assert.AreNotSame(s.DataProviderSchemes, s.DataProviderSchemes);

            Assert.IsNotNull(s.DataStructures);
            CollectionAssert.IsEmpty(s.DataStructures);
            Assert.AreNotSame(s.DataStructures, s.DataStructures);

            Assert.IsNotNull(s.Dataflows);
            CollectionAssert.IsEmpty(s.Dataflows);
            Assert.AreNotSame(s.Dataflows, s.Dataflows);

            Assert.IsNotNull(s.HierarchicalCodelists);
            CollectionAssert.IsEmpty(s.HierarchicalCodelists);
            Assert.AreNotSame(s.HierarchicalCodelists, s.HierarchicalCodelists);

            Assert.IsNotNull(s.MetadataStructures);
            CollectionAssert.IsEmpty(s.MetadataStructures);
            Assert.AreNotSame(s.MetadataStructures, s.MetadataStructures);

            Assert.IsNotNull(s.Metadataflows);
            CollectionAssert.IsEmpty(s.Metadataflows);
            Assert.AreNotSame(s.Metadataflows, s.Metadataflows);

            Assert.IsNotNull(s.OrganisationUnitSchemes);
            CollectionAssert.IsEmpty(s.OrganisationUnitSchemes);
            Assert.AreNotSame(s.OrganisationUnitSchemes, s.OrganisationUnitSchemes);

            Assert.IsNotNull(s.Processes);
            CollectionAssert.IsEmpty(s.Processes);
            Assert.AreNotSame(s.Processes, s.Processes);

            Assert.IsNotNull(s.ProvisionAgreements);
            CollectionAssert.IsEmpty(s.ProvisionAgreements);
            Assert.AreNotSame(s.ProvisionAgreements, s.ProvisionAgreements);

            Assert.IsNotNull(s.Registrations);
            CollectionAssert.IsEmpty(s.Registrations);
            Assert.AreNotSame(s.Registrations, s.Registrations);

            Assert.IsNotNull(s.ReportingTaxonomys);
            CollectionAssert.IsEmpty(s.ReportingTaxonomys);
            Assert.AreNotSame(s.ReportingTaxonomys, s.ReportingTaxonomys);

            Assert.IsNotNull(s.StructureSets);
            CollectionAssert.IsEmpty(s.StructureSets);
            Assert.AreNotSame(s.StructureSets, s.StructureSets);

            Assert.IsNotNull(s.Subscriptions);
            CollectionAssert.IsEmpty(s.Subscriptions);
            Assert.AreNotSame(s.Subscriptions, s.Subscriptions);

            var agencySchemeMock = new Mock<IAgencyScheme>();
            agencySchemeMock.Setup(o => o.StructureType)
                            .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme));

            agencySchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            agencySchemeMock.Setup(o => o.Id).Returns("ID_AGENCYSCHEME");
            agencySchemeMock.Setup(o => o.Version).Returns("1.2");
            agencySchemeMock.Setup(o => o.StructureType)
                            .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme));
            IAgencyScheme agencyScheme = agencySchemeMock.Object;
            s.AddAgencyScheme(agencyScheme);
            CollectionAssert.IsNotEmpty(s.AgenciesSchemes);
            s.RemoveAgencyScheme(agencyScheme);
            CollectionAssert.IsEmpty(s.AgenciesSchemes);
            s.AddIdentifiable(agencyScheme);
            CollectionAssert.IsNotEmpty(s.AgenciesSchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.AgenciesSchemes);

            var attachmentConstraintObjectMock = new Mock<IAttachmentConstraintObject>();
            attachmentConstraintObjectMock.Setup(o => o.StructureType)
                                          .Returns(
                                              SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint));

            attachmentConstraintObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            attachmentConstraintObjectMock.Setup(o => o.Id).Returns("ID_ATTACHMENTCONSTRAINTOBJECT");
            attachmentConstraintObjectMock.Setup(o => o.Version).Returns("1.2");
            IAttachmentConstraintObject attachmentConstraintObject = attachmentConstraintObjectMock.Object;
            s.AddAttachmentConstraint(attachmentConstraintObject);
            CollectionAssert.IsNotEmpty(s.AttachmentConstraints);
            s.RemoveAttachmentConstraintObject(attachmentConstraintObject);
            CollectionAssert.IsEmpty(s.AttachmentConstraints);
            s.AddIdentifiable(attachmentConstraintObject);
            CollectionAssert.IsNotEmpty(s.AttachmentConstraints);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.AttachmentConstraints);

            var categorisationObjectMock = new Mock<ICategorisationObject>();
            categorisationObjectMock.Setup(o => o.StructureType)
                                    .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation));

            categorisationObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            categorisationObjectMock.Setup(o => o.Id).Returns("ID_CATEGORISATIONOBJECT");
            categorisationObjectMock.Setup(o => o.Version).Returns("1.2");
            ICategorisationObject categorisationObject = categorisationObjectMock.Object;
            s.AddCategorisation(categorisationObject);
            CollectionAssert.IsNotEmpty(s.Categorisations);
            s.RemoveCategorisation(categorisationObject);
            CollectionAssert.IsEmpty(s.Categorisations);
            s.AddIdentifiable(categorisationObject);
            CollectionAssert.IsNotEmpty(s.Categorisations);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Categorisations);

            var categorySchemeObjectMock = new Mock<ICategorySchemeObject>();
            categorySchemeObjectMock.Setup(o => o.StructureType)
                                    .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));

            categorySchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            categorySchemeObjectMock.Setup(o => o.Id).Returns("ID_CATEGORYSCHEMEOBJECT");
            categorySchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            ICategorySchemeObject categorySchemeObject = categorySchemeObjectMock.Object;
            s.AddCategoryScheme(categorySchemeObject);
            CollectionAssert.IsNotEmpty(s.CategorySchemes);
            s.RemoveCategoryScheme(categorySchemeObject);
            CollectionAssert.IsEmpty(s.CategorySchemes);
            s.AddIdentifiable(categorySchemeObject);
            CollectionAssert.IsNotEmpty(s.CategorySchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.CategorySchemes);

            var codelistObjectMock = new Mock<ICodelistObject>();
            codelistObjectMock.Setup(o => o.StructureType)
                              .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));

            codelistObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            codelistObjectMock.Setup(o => o.Id).Returns("ID_CODELISTOBJECT");
            codelistObjectMock.Setup(o => o.Version).Returns("1.2");
            ICodelistObject codelistObject = codelistObjectMock.Object;
            s.AddCodelist(codelistObject);
            CollectionAssert.IsNotEmpty(s.Codelists);
            s.RemoveCodelist(codelistObject);
            CollectionAssert.IsEmpty(s.Codelists);
            s.AddIdentifiable(codelistObject);
            CollectionAssert.IsNotEmpty(s.Codelists);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Codelists);

            var conceptSchemeObjectMock = new Mock<IConceptSchemeObject>();
            conceptSchemeObjectMock.Setup(o => o.StructureType)
                                   .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));

            conceptSchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            conceptSchemeObjectMock.Setup(o => o.Id).Returns("ID_CONCEPTSCHEMEOBJECT");
            conceptSchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            IConceptSchemeObject conceptSchemeObject = conceptSchemeObjectMock.Object;
            s.AddConceptScheme(conceptSchemeObject);
            CollectionAssert.IsNotEmpty(s.ConceptSchemes);
            s.RemoveConceptScheme(conceptSchemeObject);
            CollectionAssert.IsEmpty(s.ConceptSchemes);
            s.AddIdentifiable(conceptSchemeObject);
            CollectionAssert.IsNotEmpty(s.ConceptSchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.ConceptSchemes);

            var contentConstraintObjectMock = new Mock<IContentConstraintObject>();
            contentConstraintObjectMock.Setup(o => o.StructureType)
                                       .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint));

            contentConstraintObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            contentConstraintObjectMock.Setup(o => o.Id).Returns("ID_CONTENTCONSTRAINTOBJECT");
            contentConstraintObjectMock.Setup(o => o.Version).Returns("1.2");
            IContentConstraintObject contentConstraintObject = contentConstraintObjectMock.Object;
            s.AddContentConstraintObject(contentConstraintObject);
            CollectionAssert.IsNotEmpty(s.ContentConstraintObjects);
            s.RemoveContentConstraintObject(contentConstraintObject);
            CollectionAssert.IsEmpty(s.ContentConstraintObjects);
            s.AddIdentifiable(contentConstraintObject);
            CollectionAssert.IsNotEmpty(s.ContentConstraintObjects);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.ContentConstraintObjects);

            var dataConsumerSchemeMock = new Mock<IDataConsumerScheme>();
            dataConsumerSchemeMock.Setup(o => o.StructureType)
                                  .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme));

            dataConsumerSchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataConsumerSchemeMock.Setup(o => o.Id).Returns("ID_DATACONSUMERSCHEME");
            dataConsumerSchemeMock.Setup(o => o.Version).Returns("1.2");
            IDataConsumerScheme dataConsumerScheme = dataConsumerSchemeMock.Object;
            s.AddDataConsumerScheme(dataConsumerScheme);
            CollectionAssert.IsNotEmpty(s.DataConsumerSchemes);
            s.RemoveDataConsumerScheme(dataConsumerScheme);
            CollectionAssert.IsEmpty(s.DataConsumerSchemes);
            s.AddIdentifiable(dataConsumerScheme);
            CollectionAssert.IsNotEmpty(s.DataConsumerSchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.DataConsumerSchemes);

            var dataProviderSchemeMock = new Mock<IDataProviderScheme>();
            dataProviderSchemeMock.Setup(o => o.StructureType)
                                  .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme));

            dataProviderSchemeMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataProviderSchemeMock.Setup(o => o.Id).Returns("ID_DATAPROVIDERSCHEME");
            dataProviderSchemeMock.Setup(o => o.Version).Returns("1.2");
            IDataProviderScheme dataProviderScheme = dataProviderSchemeMock.Object;
            s.AddDataProviderScheme(dataProviderScheme);
            CollectionAssert.IsNotEmpty(s.DataProviderSchemes);
            s.RemoveDataProviderScheme(dataProviderScheme);
            CollectionAssert.IsEmpty(s.DataProviderSchemes);
            s.AddIdentifiable(dataProviderScheme);
            CollectionAssert.IsNotEmpty(s.DataProviderSchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.DataProviderSchemes);

            var dataStructureObjectMock = new Mock<IDataStructureObject>();
            dataStructureObjectMock.Setup(o => o.StructureType)
                                   .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));

            dataStructureObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataStructureObjectMock.Setup(o => o.Id).Returns("ID_DATASTRUCTUREOBJECT");
            dataStructureObjectMock.Setup(o => o.Version).Returns("1.2");
            IDataStructureObject dataStructureObject = dataStructureObjectMock.Object;
            s.AddDataStructure(dataStructureObject);
            CollectionAssert.IsNotEmpty(s.DataStructures);
            s.RemoveDataStructure(dataStructureObject);
            CollectionAssert.IsEmpty(s.DataStructures);
            s.AddIdentifiable(dataStructureObject);
            CollectionAssert.IsNotEmpty(s.DataStructures);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.DataStructures);

            var dataflowObjectMock = new Mock<IDataflowObject>();
            dataflowObjectMock.Setup(o => o.StructureType)
                              .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));

            dataflowObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            dataflowObjectMock.Setup(o => o.Id).Returns("ID_DATAFLOWOBJECT");
            dataflowObjectMock.Setup(o => o.Version).Returns("1.2");
            IDataflowObject dataflowObject = dataflowObjectMock.Object;
            s.AddDataflow(dataflowObject);
            CollectionAssert.IsNotEmpty(s.Dataflows);
            s.RemoveDataflow(dataflowObject);
            CollectionAssert.IsEmpty(s.Dataflows);
            s.AddIdentifiable(dataflowObject);
            CollectionAssert.IsNotEmpty(s.Dataflows);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Dataflows);

            var hierarchicalCodelistObjectMock = new Mock<IHierarchicalCodelistObject>();
            hierarchicalCodelistObjectMock.Setup(o => o.StructureType)
                                          .Returns(
                                              SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist));

            hierarchicalCodelistObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            hierarchicalCodelistObjectMock.Setup(o => o.Id).Returns("ID_HIERARCHICALCODELISTOBJECT");
            hierarchicalCodelistObjectMock.Setup(o => o.Version).Returns("1.2");
            IHierarchicalCodelistObject hierarchicalCodelistObject = hierarchicalCodelistObjectMock.Object;
            s.AddHierarchicalCodelist(hierarchicalCodelistObject);
            CollectionAssert.IsNotEmpty(s.HierarchicalCodelists);
            s.RemoveHierarchicalCodelist(hierarchicalCodelistObject);
            CollectionAssert.IsEmpty(s.HierarchicalCodelists);
            s.AddIdentifiable(hierarchicalCodelistObject);
            CollectionAssert.IsNotEmpty(s.HierarchicalCodelists);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.HierarchicalCodelists);

            var metadataFlowMock = new Mock<IMetadataFlow>();
            metadataFlowMock.Setup(o => o.StructureType)
                            .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));

            metadataFlowMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            metadataFlowMock.Setup(o => o.Id).Returns("ID_METADATAFLOW");
            metadataFlowMock.Setup(o => o.Version).Returns("1.2");
            IMetadataFlow metadataFlow = metadataFlowMock.Object;
            s.AddMetadataFlow(metadataFlow);
            CollectionAssert.IsNotEmpty(s.Metadataflows);
            s.RemoveMetadataFlow(metadataFlow);
            CollectionAssert.IsEmpty(s.Metadataflows);
            s.AddIdentifiable(metadataFlow);
            CollectionAssert.IsNotEmpty(s.Metadataflows);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Metadataflows);

            var metadataStructureDefinitionObjectMock = new Mock<IMetadataStructureDefinitionObject>();
            metadataStructureDefinitionObjectMock.Setup(o => o.StructureType)
                                                 .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));

            metadataStructureDefinitionObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            metadataStructureDefinitionObjectMock.Setup(o => o.Id).Returns("ID_METADATASTRUCTUREDEFINITIONOBJECT");
            metadataStructureDefinitionObjectMock.Setup(o => o.Version).Returns("1.2");
            IMetadataStructureDefinitionObject metadataStructureDefinitionObject =
                metadataStructureDefinitionObjectMock.Object;
            s.AddMetadataStructure(metadataStructureDefinitionObject);
            CollectionAssert.IsNotEmpty(s.MetadataStructures);
            s.RemoveMetadataStructure(metadataStructureDefinitionObject);
            CollectionAssert.IsEmpty(s.MetadataStructures);
            s.AddIdentifiable(metadataStructureDefinitionObject);
            CollectionAssert.IsNotEmpty(s.MetadataStructures);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.MetadataStructures);

            var organisationUnitSchemeObjectMock = new Mock<IOrganisationUnitSchemeObject>();
            organisationUnitSchemeObjectMock.Setup(o => o.StructureType)
                                            .Returns(
                                                SdmxStructureType.GetFromEnum(
                                                    SdmxStructureEnumType.OrganisationUnitScheme));

            organisationUnitSchemeObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            organisationUnitSchemeObjectMock.Setup(o => o.Id).Returns("ID_ORGANISATIONUNITSCHEMEOBJECT");
            organisationUnitSchemeObjectMock.Setup(o => o.Version).Returns("1.2");
            IOrganisationUnitSchemeObject organisationUnitSchemeObject = organisationUnitSchemeObjectMock.Object;
            s.AddOrganisationUnitScheme(organisationUnitSchemeObject);
            CollectionAssert.IsNotEmpty(s.OrganisationUnitSchemes);
            s.RemoveOrganisationUnitScheme(organisationUnitSchemeObject);
            CollectionAssert.IsEmpty(s.OrganisationUnitSchemes);
            s.AddIdentifiable(organisationUnitSchemeObject);
            CollectionAssert.IsNotEmpty(s.OrganisationUnitSchemes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.OrganisationUnitSchemes);

            var processObjectMock = new Mock<IProcessObject>();
            processObjectMock.Setup(o => o.StructureType)
                             .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process));

            processObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            processObjectMock.Setup(o => o.Id).Returns("ID_PROCESSOBJECT");
            processObjectMock.Setup(o => o.Version).Returns("1.2");
            IProcessObject processObject = processObjectMock.Object;
            s.AddProcess(processObject);
            CollectionAssert.IsNotEmpty(s.Processes);
            s.RemoveProcess(processObject);
            CollectionAssert.IsEmpty(s.Processes);
            s.AddIdentifiable(processObject);
            CollectionAssert.IsNotEmpty(s.Processes);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Processes);

            var provisionAgreementObjectMock = new Mock<IProvisionAgreementObject>();
            provisionAgreementObjectMock.Setup(o => o.StructureType)
                                        .Returns(
                                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement));

            provisionAgreementObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            provisionAgreementObjectMock.Setup(o => o.Id).Returns("ID_PROVISIONAGREEMENTOBJECT");
            provisionAgreementObjectMock.Setup(o => o.Version).Returns("1.2");
            IProvisionAgreementObject provisionAgreementObject = provisionAgreementObjectMock.Object;
            s.AddProvisionAgreement(provisionAgreementObject);
            CollectionAssert.IsNotEmpty(s.ProvisionAgreements);
            s.RemoveProvisionAgreement(provisionAgreementObject);
            CollectionAssert.IsEmpty(s.ProvisionAgreements);
            s.AddIdentifiable(provisionAgreementObject);
            CollectionAssert.IsNotEmpty(s.ProvisionAgreements);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.ProvisionAgreements);

            var registrationObjectMock = new Mock<IRegistrationObject>();
            registrationObjectMock.Setup(o => o.StructureType)
                                  .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration));

            registrationObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            registrationObjectMock.Setup(o => o.Id).Returns("ID_REGISTRATIONOBJECT");
            registrationObjectMock.Setup(o => o.Version).Returns("1.2");
            IRegistrationObject registrationObject = registrationObjectMock.Object;
            s.AddRegistration(registrationObject);
            CollectionAssert.IsNotEmpty(s.Registrations);
            s.RemoveRegistration(registrationObject);
            CollectionAssert.IsEmpty(s.Registrations);
            s.AddIdentifiable(registrationObject);
            CollectionAssert.IsNotEmpty(s.Registrations);
            Assert.IsFalse(s2.HasRegistrations());
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Registrations);
            Assert.IsTrue(s2.HasRegistrations());

            var reportingTaxonomyObjectMock = new Mock<IReportingTaxonomyObject>();
            reportingTaxonomyObjectMock.Setup(o => o.StructureType)
                                       .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy));

            reportingTaxonomyObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            reportingTaxonomyObjectMock.Setup(o => o.Id).Returns("ID_REPORTINGTAXONOMYOBJECT");
            reportingTaxonomyObjectMock.Setup(o => o.Version).Returns("1.2");
            IReportingTaxonomyObject reportingTaxonomyObject = reportingTaxonomyObjectMock.Object;
            s.AddReportingTaxonomy(reportingTaxonomyObject);
            CollectionAssert.IsNotEmpty(s.ReportingTaxonomys);
            s.RemoveReportingTaxonomy(reportingTaxonomyObject);
            CollectionAssert.IsEmpty(s.ReportingTaxonomys);
            s.AddIdentifiable(reportingTaxonomyObject);
            CollectionAssert.IsNotEmpty(s.ReportingTaxonomys);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.ReportingTaxonomys);

            var structureSetObjectMock = new Mock<IStructureSetObject>();
            structureSetObjectMock.Setup(o => o.StructureType)
                                  .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet));

            structureSetObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            structureSetObjectMock.Setup(o => o.Id).Returns("ID_STRUCTURESETOBJECT");
            structureSetObjectMock.Setup(o => o.Version).Returns("1.2");
            IStructureSetObject structureSetObject = structureSetObjectMock.Object;
            s.AddStructureSet(structureSetObject);
            CollectionAssert.IsNotEmpty(s.StructureSets);
            s.RemoveStructureSet(structureSetObject);
            CollectionAssert.IsEmpty(s.StructureSets);
            s.AddIdentifiable(structureSetObject);
            CollectionAssert.IsNotEmpty(s.StructureSets);
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.StructureSets);

            var subscriptionObjectMock = new Mock<ISubscriptionObject>();
            subscriptionObjectMock.Setup(o => o.StructureType)
                                  .Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription));

            subscriptionObjectMock.Setup(o => o.AgencyId).Returns("TEST_AGENCY");
            subscriptionObjectMock.Setup(o => o.Id).Returns("ID_SUBSCRIPTIONOBJECT");
            subscriptionObjectMock.Setup(o => o.Version).Returns("1.2");
            ISubscriptionObject subscriptionObject = subscriptionObjectMock.Object;
            s.AddSubscription(subscriptionObject);
            CollectionAssert.IsNotEmpty(s.Subscriptions);
            s.RemoveSubscription(subscriptionObject);
            CollectionAssert.IsEmpty(s.Subscriptions);
            s.AddIdentifiable(subscriptionObject);
            CollectionAssert.IsNotEmpty(s.Subscriptions);
            Assert.IsFalse(s2.HasSubscriptions());
            s2.Merge(s);
            CollectionAssert.IsNotEmpty(s2.Subscriptions);
            Assert.IsTrue(s2.HasSubscriptions());

            var wildCard = new MaintainableRefObjectImpl("TEST_AGENCY", null, "1.2");
            var nothing = new MaintainableRefObjectImpl("NOTHING", null, "1.0");

            CollectionAssert.IsNotEmpty(s2.GetAgenciesSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetAgenciesSchemes(nothing));
            CollectionAssert.IsNotEmpty(
                s2.GetAgenciesSchemes(
                    new MaintainableRefObjectImpl(agencyScheme.AgencyId, agencyScheme.Id, agencyScheme.Version)));
            CollectionAssert.IsNotEmpty(s2.GetAgenciesSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetAgenciesSchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetAttachmentConstraints("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetAttachmentConstraints("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetAttachmentConstraints(
                    new MaintainableRefObjectImpl(
                        attachmentConstraintObject.AgencyId, 
                        attachmentConstraintObject.Id, 
                        attachmentConstraintObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetAttachmentConstraints(wildCard));
            CollectionAssert.IsEmpty(s2.GetAttachmentConstraints(nothing));

            CollectionAssert.IsNotEmpty(s2.GetCategorisations("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetCategorisations("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetCategorisations(
                    new MaintainableRefObjectImpl(
                        categorisationObject.AgencyId, categorisationObject.Id, categorisationObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetCategorisations(wildCard));
            CollectionAssert.IsEmpty(s2.GetCategorisations(nothing));

            CollectionAssert.IsNotEmpty(s2.GetCategorySchemes("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetCategorySchemes("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetCategorySchemes(
                    new MaintainableRefObjectImpl(
                        categorySchemeObject.AgencyId, categorySchemeObject.Id, categorySchemeObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetCategorySchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetCategorySchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetCodelists("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetCodelists("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetCodelists(
                    new MaintainableRefObjectImpl(codelistObject.AgencyId, codelistObject.Id, codelistObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetCodelists(wildCard));
            CollectionAssert.IsEmpty(s2.GetCodelists(nothing));

            CollectionAssert.IsNotEmpty(s2.GetConceptSchemes("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetConceptSchemes("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetConceptSchemes(
                    new MaintainableRefObjectImpl(
                        conceptSchemeObject.AgencyId, conceptSchemeObject.Id, conceptSchemeObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetConceptSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetConceptSchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetContentConstraintObjects("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetContentConstraintObjects("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetContentConstraintObjects(
                    new MaintainableRefObjectImpl(
                        contentConstraintObject.AgencyId, contentConstraintObject.Id, contentConstraintObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetContentConstraintObjects(wildCard));
            CollectionAssert.IsEmpty(s2.GetContentConstraintObjects(nothing));

            CollectionAssert.IsNotEmpty(s2.GetDataConsumerSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataConsumerSchemes(nothing));
            CollectionAssert.IsNotEmpty(
                s2.GetDataConsumerSchemes(
                    new MaintainableRefObjectImpl(
                        dataConsumerScheme.AgencyId, dataConsumerScheme.Id, dataConsumerScheme.Version)));
            CollectionAssert.IsNotEmpty(s2.GetDataConsumerSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataConsumerSchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetDataProviderSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataProviderSchemes(nothing));
            CollectionAssert.IsNotEmpty(
                s2.GetDataProviderSchemes(
                    new MaintainableRefObjectImpl(
                        dataProviderScheme.AgencyId, dataProviderScheme.Id, dataProviderScheme.Version)));
            CollectionAssert.IsNotEmpty(s2.GetDataProviderSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataProviderSchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetDataStructures("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetDataStructures("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetDataStructures(
                    new MaintainableRefObjectImpl(
                        dataStructureObject.AgencyId, dataStructureObject.Id, dataStructureObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetDataStructures(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataStructures(nothing));

            CollectionAssert.IsNotEmpty(s2.GetDataflows("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetDataflows("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetDataflows(
                    new MaintainableRefObjectImpl(dataflowObject.AgencyId, dataflowObject.Id, dataflowObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetDataflows(wildCard));
            CollectionAssert.IsEmpty(s2.GetDataflows(nothing));

            CollectionAssert.IsNotEmpty(s2.GetHierarchicalCodelists("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetHierarchicalCodelists("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetHierarchicalCodelists(
                    new MaintainableRefObjectImpl(
                        hierarchicalCodelistObject.AgencyId, 
                        hierarchicalCodelistObject.Id, 
                        hierarchicalCodelistObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetHierarchicalCodelists(wildCard));
            CollectionAssert.IsEmpty(s2.GetHierarchicalCodelists(nothing));

            CollectionAssert.IsNotEmpty(s2.GetMetadataStructures("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetMetadataStructures("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetMetadataStructures(
                    new MaintainableRefObjectImpl(
                        metadataStructureDefinitionObject.AgencyId, 
                        metadataStructureDefinitionObject.Id, 
                        metadataStructureDefinitionObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetMetadataStructures(wildCard));
            CollectionAssert.IsEmpty(s2.GetMetadataStructures(nothing));

            CollectionAssert.IsNotEmpty(s2.GetMetadataflows("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetMetadataflows("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetMetadataflows(
                    new MaintainableRefObjectImpl(metadataFlow.AgencyId, metadataFlow.Id, metadataFlow.Version)));
            CollectionAssert.IsNotEmpty(s2.GetMetadataflows(wildCard));
            CollectionAssert.IsEmpty(s2.GetMetadataflows(nothing));

            CollectionAssert.IsNotEmpty(s2.GetOrganisationUnitSchemes("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetOrganisationUnitSchemes("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetOrganisationUnitSchemes(
                    new MaintainableRefObjectImpl(
                        organisationUnitSchemeObject.AgencyId, 
                        organisationUnitSchemeObject.Id, 
                        organisationUnitSchemeObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetOrganisationUnitSchemes(wildCard));
            CollectionAssert.IsEmpty(s2.GetOrganisationUnitSchemes(nothing));

            CollectionAssert.IsNotEmpty(s2.GetProcesses("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetProcesses("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetProcesses(
                    new MaintainableRefObjectImpl(processObject.AgencyId, processObject.Id, processObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetProcesses(wildCard));
            CollectionAssert.IsEmpty(s2.GetProcesses(nothing));

            CollectionAssert.IsNotEmpty(s2.GetProvisionAgreements("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetProvisionAgreements("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetProvisionAgreements(
                    new MaintainableRefObjectImpl(
                        provisionAgreementObject.AgencyId, provisionAgreementObject.Id, provisionAgreementObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetProvisionAgreements(wildCard));
            CollectionAssert.IsEmpty(s2.GetProvisionAgreements(nothing));

            CollectionAssert.IsNotEmpty(s2.GetRegistrations("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetRegistrations("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetRegistrations(
                    new MaintainableRefObjectImpl(
                        registrationObject.AgencyId, registrationObject.Id, registrationObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetRegistrations(wildCard));
            CollectionAssert.IsEmpty(s2.GetRegistrations(nothing));

            CollectionAssert.IsNotEmpty(s2.GetReportingTaxonomys("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetReportingTaxonomys("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetReportingTaxonomys(
                    new MaintainableRefObjectImpl(
                        reportingTaxonomyObject.AgencyId, reportingTaxonomyObject.Id, reportingTaxonomyObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetReportingTaxonomys(wildCard));
            CollectionAssert.IsEmpty(s2.GetReportingTaxonomys(nothing));

            CollectionAssert.IsNotEmpty(s2.GetStructureSets("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetStructureSets("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetStructureSets(
                    new MaintainableRefObjectImpl(
                        structureSetObject.AgencyId, structureSetObject.Id, structureSetObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetStructureSets(wildCard));
            CollectionAssert.IsEmpty(s2.GetStructureSets(nothing));

            CollectionAssert.IsNotEmpty(s2.GetSubscriptions("TEST_AGENCY"));
            CollectionAssert.IsEmpty(s2.GetSubscriptions("NOTHING"));
            CollectionAssert.IsNotEmpty(
                s2.GetSubscriptions(
                    new MaintainableRefObjectImpl(
                        subscriptionObject.AgencyId, subscriptionObject.Id, subscriptionObject.Version)));
            CollectionAssert.IsNotEmpty(s2.GetSubscriptions(wildCard));
            CollectionAssert.IsEmpty(s2.GetSubscriptions(nothing));

            Assert.IsTrue(s2.HasStructures());
            foreach (SdmxStructureType structureType in SdmxStructureType.Values)
            {
                if (structureType.IsMaintainable)
                {
                    CollectionAssert.IsNotEmpty(s2.GetMaintainables(structureType.EnumType));
                    CollectionAssert.IsEmpty(s3.GetMaintainables(structureType.EnumType));
                }
            }

            var mutableObjects = s2.MutableObjects;
            Assert.IsNotNull(mutableObjects);
            
            var s5 = new SdmxObjectsImpl(new HeaderImpl("PAOK", "OLE"), s2.Dataflows);
            CollectionAssert.IsNotEmpty(s5.Dataflows);
            CollectionAssert.IsNotEmpty(s5.GetAllMaintainables(SdmxStructureEnumType.HierarchicalCodelist));
            CollectionAssert.IsEmpty(s5.GetAllMaintainables(SdmxStructureEnumType.Dataflow));
        }

        #endregion
    }
}
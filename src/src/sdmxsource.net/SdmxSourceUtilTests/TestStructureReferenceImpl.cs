// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStructureReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="StructureReferenceImpl" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit for <see cref="StructureReferenceImpl" />
    /// </summary>
    [TestFixture]
    public class TestStructureReferenceImpl
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="CrossReferenceImpl"/>
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)")]
        public void TestCrossReference(string urn)
        {
            ICodelistObject immutableInstance = CreateSampleCodelist();
            var cross = new CrossReferenceImpl(immutableInstance, urn);
            Assert.IsTrue(immutableInstance.DeepEquals(cross.ReferencedFrom, true));
        }

        /// <summary>
        /// The test cross reference.
        /// </summary>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        [TestCase(null, null, null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, null, "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, null, " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, " ", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, " ", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, " ", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", null, null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", null, null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", null, "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", null, " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", " ", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", " ", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", " ", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", null, null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", null, null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", null, "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", null, " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", "", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", " ", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", "", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", "", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", " ", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", " ", " ", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, null, "1.0", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "", "1.0", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(" ", " ", "1.0", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "TEST", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "TEST", null, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("", "TEST", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "TEST", "", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, "TEST", "1.0", ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("AGENCY", "TEST", null)]
        [TestCase("AGENCY", "TEST", " ")]
        [TestCase("AGENCY", "TEST", "")]
        [TestCase("AGENCY", "TEST", "1.0")]
        [TestCase("AGENCY", "TST", "2.0")]
        [TestCase("AGNCY", "TEST", "2.0")]
        [TestCase("AGENCY", "TEST", "2.0")]
        public void TestCrossReference2(string agency, string id, string version)
        {
            ICodelistObject immutableInstance = CreateSampleCodelist();
            foreach (SdmxStructureType sdmxStructureType in SdmxStructureType.Values)
            {
                if (sdmxStructureType.IsMaintainable)
                {
                    string maintainableId = GetID(sdmxStructureType, id);
                    var cross = new CrossReferenceImpl(immutableInstance, agency, maintainableId, version, sdmxStructureType);
                    Assert.AreEqual(agency, cross.AgencyId);
                    Assert.AreEqual(maintainableId, cross.MaintainableId);
                    string versionToTest = string.IsNullOrWhiteSpace(version) ? "1.0" : version;
                    Assert.AreEqual(versionToTest, cross.Version);
                }
                else if (sdmxStructureType.IsIdentifiable && !sdmxStructureType.HasFixedId)
                {
                    var cross = new CrossReferenceImpl(
                        immutableInstance, agency, id, version, sdmxStructureType.EnumType, "ENA", "DYO", "TRIA");
                    Assert.AreEqual(agency, cross.AgencyId);
                    Assert.AreEqual(id, cross.MaintainableId);
                    string versionToTest = string.IsNullOrWhiteSpace(version) ? "1.0" : version;
                    Assert.AreEqual(versionToTest, cross.Version);
                    CollectionAssert.IsNotEmpty(cross.IdentifiableIds);
                }
            }
        }

        /// <summary>
        /// The test structure reference.
        /// </summary>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        [TestCase(null, null, null)]
        [TestCase(null, null, "")]
        [TestCase(null, null, " ")]
        [TestCase(null, "", null)]
        [TestCase(null, " ", null)]
        [TestCase(null, "", "")]
        [TestCase(null, "", " ")]
        [TestCase(null, " ", "")]
        [TestCase(null, " ", " ")]
        [TestCase("", null, null)]
        [TestCase("", null, null)]
        [TestCase("", null, "")]
        [TestCase("", null, " ")]
        [TestCase("", "", null)]
        [TestCase("", " ", null)]
        [TestCase("", "", "")]
        [TestCase("", "", " ")]
        [TestCase("", " ", "")]
        [TestCase("", " ", " ")]
        [TestCase(" ", null, null)]
        [TestCase(" ", null, null)]
        [TestCase(" ", null, "")]
        [TestCase(" ", null, " ")]
        [TestCase(" ", "", null)]
        [TestCase(" ", " ", null)]
        [TestCase(" ", "", "")]
        [TestCase(" ", "", " ")]
        [TestCase(" ", " ", "")]
        [TestCase(" ", " ", " ")]
        [TestCase(null, null, "1.0")]
        [TestCase("", "", "1.0")]
        [TestCase(" ", " ", "1.0")]
        [TestCase(null, "TEST", null)]
        [TestCase("", "TEST", null)]
        [TestCase("", "TEST", "")]
        [TestCase(null, "TEST", "")]
        [TestCase(null, "TEST", "1.0")]
        [TestCase("AGENCY", "TEST", null)]
        [TestCase("AGENCY", "TEST", " ")]
        [TestCase("AGENCY", "TEST", "")]
        [TestCase("AGENCY", "TEST", "1.0")]
        [TestCase("AGENCY", "TST", "2.0")]
        [TestCase("AGNCY", "TEST", "2.0")]
        [TestCase("AGENCY", "TEST", "2.0")]
        public void TestStructureReference(string agency, string id, string version)
        {
            foreach (SdmxStructureType sdmxStructureType in SdmxStructureType.Values)
            {
                if (sdmxStructureType.IsMaintainable)
                {
                    var cross = new StructureReferenceImpl(agency, id, version, sdmxStructureType);
                    Assert.AreEqual(agency, cross.AgencyId);
                    Assert.AreEqual(id, cross.MaintainableId);
                    string versionToTest = string.IsNullOrWhiteSpace(version) ? null : version;
                    Assert.AreEqual(versionToTest, cross.Version);
                }
                else if (sdmxStructureType.IsIdentifiable && !sdmxStructureType.HasFixedId)
                {
                    var cross = new StructureReferenceImpl(
                        agency, id, version, sdmxStructureType.EnumType, "ENA", "DYO", "TRIA");
                    Assert.AreEqual(agency, cross.AgencyId);
                    Assert.AreEqual(id, cross.MaintainableId);
                    string versionToTest = string.IsNullOrWhiteSpace(version) ? null : version;
                    Assert.AreEqual(versionToTest, cross.Version);
                    CollectionAssert.IsNotEmpty(cross.IdentifiableIds);
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureReferenceImpl"/>
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        /// <param name="hasChildReference">
        /// The has Child Reference.
        /// </param>
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", 
            SdmxStructureEnumType.Group, true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", SdmxStructureEnumType.Dsd, 
            false)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", 
            SdmxStructureEnumType.Category, true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).14.1440.STS.SSTSCONS", 
            SdmxStructureEnumType.Category, true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", SdmxStructureEnumType.Code, true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", 
            SdmxStructureEnumType.Dsd, false)]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2", 
            SdmxStructureEnumType.Category, true)]
        [TestCase(
            "urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME"
            , SdmxStructureEnumType.MetadataAttribute, true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)", 
            SdmxStructureEnumType.Dataflow, false)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", SdmxStructureEnumType.Agency, true)]
        public void TestStructureReference(string urn, SdmxStructureEnumType expectedResult, bool hasChildReference)
        {
            var impl = new StructureReferenceImpl(urn);
            Assert.AreEqual(urn, impl.TargetUrn.ToString());
            Assert.AreEqual(expectedResult, impl.TargetStructureType.EnumType);
            Assert.IsTrue(impl.HasAgencyId());
            Assert.IsTrue(impl.HasTargetUrn());
            Assert.IsTrue(impl.HasMaintainableId());
            Assert.AreEqual(hasChildReference, impl.HasChildReference());
            Assert.AreEqual(hasChildReference, impl.IdentifiableIds.Count > 0);
            Assert.AreEqual(impl.TargetUrn, impl.CreateCopy().TargetUrn);
            Assert.AreEqual(impl.TargetReference, impl.CreateCopy().TargetReference);
        }

        /// <summary>
        /// Test unit for <see cref="StructureReferenceImpl.IsMatch"/>
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)", true)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", false)]
        public void TestStructureReferenceIsMatch(string urn, bool expectedResult)
        {
            var df = new DataflowMutableCore { AgencyId = "TFFS.ABC", Id = "EXTERNAL_DEBT", Version = "1.0" };
            df.AddName("en", "Test");
            df.DataStructureRef = new StructureReferenceImpl("TEST_AG","TST", "1.0", SdmxStructureEnumType.Dsd);
            var impl = new StructureReferenceImpl(urn);
            IDataflowObject immutableInstance = df.ImmutableInstance;
            Assert.AreEqual(expectedResult, impl.IsMatch(immutableInstance));
            if (expectedResult)
            {
                Assert.IsTrue(immutableInstance.DeepEquals(impl.GetMatch(immutableInstance), true));
            }
            else
            {
                Assert.IsNull(impl.GetMatch(immutableInstance));
            }

            var cross = new CrossReferenceImpl(immutableInstance, impl);
            Assert.IsTrue(cross.ReferencedFrom.DeepEquals(immutableInstance, true));
            Assert.AreEqual(cross.CreateMutableInstance().TargetUrn, impl.TargetUrn);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <param name="targetObj">The target object.</param>
        /// <param name="id">The unique identifier.</param>
        /// <returns>
        /// The ID.
        /// </returns>
        private static string GetID(SdmxStructureType targetObj, string id)
        {
            return targetObj.HasFixedId ? targetObj.FixedId : id;
        }

        /// <summary>
        ///     The create sample codelist.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICodelistObject" />.
        /// </returns>
        private static ICodelistObject CreateSampleCodelist()
        {
            ICodelistMutableObject codelist = new CodelistMutableCore();
            codelist.Id = "CL_3166A2";
            codelist.AgencyId = "ISO";
            codelist.AddName("en", "Test CL");
            ICodeMutableObject code = new CodeMutableCore();
            code.Id = "AR";
            code.AddName("en", "Code " + code.Id);
            codelist.AddItem(code);
            ICodelistObject immutableInstance = codelist.ImmutableInstance;
            return immutableInstance;
        }

        #endregion
    }
}
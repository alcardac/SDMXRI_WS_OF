namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    /// Test unit for <see cref="UrnUtil"/>
    /// </summary>
    [TestFixture]
    public class TestUrnUtil
    {

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetIdentifiableType"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", SdmxStructureEnumType.Group)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", SdmxStructureEnumType.Dsd)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=IMF:BOP(1.0):compact", SdmxStructureEnumType.Dsd)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS",SdmxStructureEnumType.Category)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", SdmxStructureEnumType.Code)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd)]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2",SdmxStructureEnumType.Category)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME",SdmxStructureEnumType.MetadataAttribute)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)",SdmxStructureEnumType.Dataflow)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS",SdmxStructureEnumType.Agency)]
        public void TestGetIdentifiableType(string urn, SdmxStructureEnumType expectedResult)
        {
            SdmxStructureType sdmxStructureType = UrnUtil.GetIdentifiableType(urn);
            Assert.NotNull(sdmxStructureType);
            Assert.AreEqual(expectedResult, sdmxStructureType.EnumType);
        }
        
        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetUrnComponents"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", new[] { "ESTAT", "STS", "Sibling" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", new[] { "ESTAT", "STS" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", new [] { "ESTAT", "ESTAT_DATAFLOWS_SCHEME", "SSTSCONS" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).14.1440.STS.SSTSCONS", new [] { "ESTAT", "ESTAT_DATAFLOWS_SCHEME","14", "1440", "STS", "SSTSCONS" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", new [] { "ISO", "CL_3166A2", "AR" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", new [] { "TFFS", "CRED_EXT_DEBT" })]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2", new [] { "SDMX", "SUBJECT_MATTER_DOMAINS", "1", "2" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME", new [] { "SDMX", "CONTACT_METADATA", "CONTACT_REPORT", "CONTACT_DETAILS", "CONTACT_NAME" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)", new [] { "TFFS.ABC", "EXTERNAL_DEBT" })]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", new [] {AgencyScheme.DefaultScheme, AgencyScheme.FixedId, "TFFS"})]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS.ABC", new [] {"TFFS", AgencyScheme.FixedId, "ABC"})]
        public void TestGetUrnComponents(string urn, IList<string> expectedResult)
        {
            string[] urnComponents = UrnUtil.GetUrnComponents(urn);
            Assert.NotNull(urnComponents);
            CollectionAssert.IsNotEmpty(urnComponents);
            CollectionAssert.AllItemsAreNotNull(urnComponents);
            CollectionAssert.AreEqual(expectedResult, urnComponents, StringComparer.Ordinal);
           
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetUrnPostfix(string)"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling","ESTAT:STS(2.0).Sibling")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)","ESTAT:STS(2.0)")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", "ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", "ISO:CL_3166A2(1.0).AR")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", "TFFS:CRED_EXT_DEBT(1.0)")]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2", "SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME","SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)","TFFS.ABC:EXTERNAL_DEBT(1.0)")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS","TFFS")]
        public void TestGetUrnPostfix(string urn, string result)
        {
            string urnPostfix = UrnUtil.GetUrnPostfix(urn);
            Assert.AreEqual(result, urnPostfix);
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetUrnPostfix(string,string,string)"/> 
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        [Test]
        [TestCase("AGENCY", "ID", "1.2","AGENCY:ID(1.2)")]
        public void TestGetUrnPostfix(string agencyId, string id, string version, string result)
        {
            string urnPostfix = UrnUtil.GetUrnPostfix(agencyId, id, version);
            Assert.AreEqual(result, urnPostfix);
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetUrnPostfix(string,string,string,string[])"/> 
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="ids">
        /// The IDs.
        /// </param>
        [Test]
        [TestCase("AGENCY", "ID", "1.2", "AGENCY:ID(1.2).ENA", new[] { "ENA" })]
        [TestCase("AGENCY", "ID", "1.2", "AGENCY:ID(1.2).ENA.DYO",new[] { "ENA", "DYO"})]
        [TestCase("AGENCY", "ID", "1.2", "AGENCY:ID(1.2).ENA.DYO.TRIA",new[] { "ENA", "DYO","TRIA"})]
        public void TestGetUrnPostfixIds(string agencyId, string id, string version, string result, string[] ids)
        {
            string urnPostfix = UrnUtil.GetUrnPostfix(agencyId, id, version,ids);
            Assert.AreEqual(result, urnPostfix);
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetUrnPrefix"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", "urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", "urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", "urn:sdmx:org.sdmx.infomodel.categoryscheme.Category")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", "urn:sdmx:org.sdmx.infomodel.codelist.Code")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", "urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure")]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2","urn:sdmx:org.sdmx.infomodel.categoryscheme.Category")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME","urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)","urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", "urn:sdmx:org.sdmx.infomodel.base.Agency")]
        public void TestGetUrnPrefix(string urn, string expectedResult)
        {
            var prefix = UrnUtil.GetUrnPrefix(urn);
            Assert.AreEqual(expectedResult, prefix);
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.GetVersionFromUrn"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", "2.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", "2.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", "1.1")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR","1.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", "1.0")]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2","1.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME","1.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)","1.0")]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", null)]
        public void TestGetVersionFromUrn(string urn, string expectedResult)
        {
            var versionFromUrn = UrnUtil.GetVersionFromUrn(urn);
            Assert.AreEqual(expectedResult, versionFromUrn);
        }

        /// <summary>
        /// Test unit for <see cref="UrnUtil.ValidateURN"/> 
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="structure">
        /// The structure.
        /// </param>
        [Test]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", SdmxStructureEnumType.Group)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ESTAT:STS(2.0)", SdmxStructureEnumType.Dsd)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=ESTAT:ESTAT_DATAFLOWS_SCHEME(1.1).SSTSCONS", SdmxStructureEnumType.Category)]
        [TestCase("paok:ole", SdmxStructureEnumType.Category, ExpectedException = typeof(SdmxSyntaxException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0).Sibling", SdmxStructureEnumType.Category, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.GroupDimensionDescriptor=ESTAT:STS(2.0)", SdmxStructureEnumType.Category, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.codelist.Code=ISO:CL_3166A2(1.0).AR", SdmxStructureEnumType.Code)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd)]
        ////[TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.BAD=TFFS:CRED_EXT_DEBT(1.0)", SdmxStructureEnumType.Dsd, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("urn:sdmx:org.sdmx.infomodel.categoryscheme.Category=SDMX:SUBJECT_MATTER_DOMAINS(1.0).1.2", SdmxStructureEnumType.Category)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.metadatastructure.MetadataAttribute=SDMX:CONTACT_METADATA(1.0).CONTACT_REPORT.CONTACT_DETAILS.CONTACT_NAME", SdmxStructureEnumType.MetadataAttribute)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.datastructure.Dataflow=TFFS.ABC:EXTERNAL_DEBT(1.0)", SdmxStructureEnumType.Dataflow)]
        [TestCase("urn:sdmx:org.sdmx.infomodel.base.Agency=TFFS", SdmxStructureEnumType.Agency)]
        public void TestValidateURN(string urn, SdmxStructureEnumType structure)
        {
            UrnUtil.ValidateURN(urn, SdmxStructureType.GetFromEnum(structure));
        }
    }
}
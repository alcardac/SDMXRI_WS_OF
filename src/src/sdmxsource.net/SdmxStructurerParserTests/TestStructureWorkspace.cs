// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStructureWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="IStructureParsingManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureParserTests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     Test unit for <see cref="IStructureParsingManager" />
    /// </summary>
    [TestFixture]
    public class TestStructureWorkspace
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test the structure parsing manager.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/CR1_12_data_report.xml", ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", ExpectedException = typeof(MaintainableObjectException))]
        [TestCase("tests/v20/query.xml", ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryProvisioningRequest.xml")]
        [TestCase("tests/v20/QueryProvisioningResponse.xml")]
        [TestCase("tests/v20/QueryRegistrationRequest.xml")]
        //// TODO: BUG with the URI parsing [TestCase("tests/v20/QueryRegistrationResponse.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", ExpectedException = typeof(MaintainableObjectException))]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0-correct.xml")]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml")]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml")]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", ExpectedException = typeof(SdmxNotImplementedException))]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/SubmitStructureResponse.xml")]
        [TestCase("tests/v20/response.xml")]
        [TestCase("tests/v20/WRDOP+US_WAWA+1.7.xml")]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", ExpectedException = typeof(SdmxNotImplementedException))] // unsuported schema version 2
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", ExpectedException = typeof(SdmxNotImplementedException))] // unsuported schema version 2
        [TestCase("tests/v20/ESTAT+ESMS_MSD+2.2.xml")]
        public void TestIStructureParsingManagerV20(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }
        }

        /// <summary>
        /// Tests the i structure parsing manager V20 code time dimension.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedAgency">The expected agency.</param>
        /// <param name="expectedId">The expected identifier.</param>
        /// <param name="expectedVersion">The expected version.</param>
        [TestCase("tests/v20/WRDOP+US_WAWA+1.7.xml", "US_WAWA", "CL_ROK", "1.6")]
        public void TestIStructureParsingManagerV20CodeTimeDimension(string file, string expectedAgency, string expectedId, string expectedVersion)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
                var timeDimension = structureBeans.DataStructures.First().TimeDimension;
                Assert.IsTrue(timeDimension.HasCodedRepresentation());
                Assert.AreEqual(expectedAgency, timeDimension.Representation.Representation.AgencyId);
                Assert.AreEqual(expectedId, timeDimension.Representation.Representation.MaintainableId);
                Assert.AreEqual(expectedVersion, timeDimension.Representation.Representation.Version);
            }
        }


        /// <summary>
        /// The test i structure parsing manager v 21.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/demography2.xml")]
        [TestCase("tests/v21/structures.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        [TestCase("tests/v21/response_demo_stub.xml", ExpectedException = typeof(MaintainableObjectException))]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.0.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.1.xml")]
        [TestCase("tests/v21/21_IT1+SEP_IND_COSTR_PR+1.3.xml")]
        [TestCase("tests/v21/Structure/CNS_PR+UN+1.1.xml")]
        [TestCase("tests/v21/Structure/TEST_5+ESTAT+1.0.xml")]
        [TestCase("tests/v21/Structure/StructureSet-sdmxv2.1-ESTAT+STS+2.0.xml")]
        [TestCase("tests/v21/Structure/AGENCIES+ESTAT+1.0.xml")]
        public void TestIStructureParsingManagerV21(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }
        }

        [TestCase("tests/v21/Structure/StructureSet-sdmxv2.1-ESTAT+STS+2.0.xml", 2)]
        public void TestIStructureParsingManagerComponentMapV21(string file, int expectedComponentMap)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
                Assert.IsNotEmpty(structureBeans.StructureSets);
                var structureMap = structureBeans.StructureSets.First(o => o.StructureMapList.Count > 0).StructureMapList.First();
                Assert.AreEqual(expectedComponentMap, structureMap.Components.Count);
            }
        }


        /// <summary>
        /// The test i structure parsing manager v 21.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/demography2.xml")]
        [TestCase("tests/v21/structures.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        [TestCase("tests/v21/response_demo_stub.xml", ExpectedException = typeof(MaintainableObjectException))]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.0.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.1.xml")]
        [TestCase("tests/v21/21_IT1+SEP_IND_COSTR_PR+1.3.xml")]
        [TestCase("tests/v21/Structure/CNS_PR+UN+1.1.xml")]
        [TestCase("tests/v21/Structure/MSDv21-TEST.xml")]
        [TestCase("tests/v21/Structure/StructureSet-sdmxv2.1-ESTAT+STS+2.0.xml")]
        public void TestReadWriteSdmxV21(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            IStructureWriterManager writerManager = new StructureWriterManager();

            var outputFileName = file + "-out.xml";
            using (Stream outputStream = File.Create(outputFileName))
            {
                writerManager.WriteStructures(structureBeans, new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), outputStream);
                outputStream.Flush();
            }

            using (var stream = File.OpenRead(outputFileName))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwoPointOne);
            }
        }

        /// <summary>
        /// The test i structure parsing manager v 21.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/demography2.xml")]
        [TestCase("tests/v21/structures.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        [TestCase("tests/v21/response_demo_stub.xml", ExpectedException = typeof(MaintainableObjectException))]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.0.xml")]
        [TestCase("tests/v21/ESTAT_STS_3.1.xml")]
        [TestCase("tests/v21/21_IT1+SEP_IND_COSTR_PR+1.3.xml")]
        [TestCase("tests/v21/Structure/CNS_PR+UN+1.1.xml")]
        [TestCase("tests/v21/Structure/MSDv21-TEST.xml")]
        [TestCase("tests/v21/Structure/StructureSet-sdmxv2.1-ESTAT+STS+2.0.xml")]
        public void TestReadWriteSdmxV21ObjectCount(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            IStructureWriterManager writerManager = new StructureWriterManager();

            var outputFileName = file + "-out.xml";
            using (Stream outputStream = File.Create(outputFileName))
            {
                writerManager.WriteStructures(structureBeans, new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), outputStream);
                outputStream.Flush();
            }

            ISdmxObjects objects2;
            using (IReadableDataLocation location = new FileReadableDataLocation(outputFileName))
            {
                var structureWorkspace = parsingManager.ParseStructures(location);
                objects2 = structureWorkspace.GetStructureObjects(false);
            }

            Assert.AreEqual(structureBeans.GetAllMaintainables().Count, objects2.GetAllMaintainables().Count);
        }


        /// <summary>
        /// Tests the codelist with parent code.
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v21/CL_CODELIST_PARENT.xml")]
        [TestCase("tests/v21/CL_CODELIST_PARENT2.xml")]
        public void TestCodelistWithParentCode(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
                Assert.IsNotEmpty(structureBeans.Codelists);
                var codelist = structureBeans.Codelists.FirstOrDefault(o => o.Items.Any(code => !string.IsNullOrWhiteSpace(code.ParentCode)));
                Assert.NotNull(codelist);
            } 
        }

        /// <summary>
        /// The test write read big code list v 20.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        [TestCase(100)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000, Ignore = true)]
        public void TestWriteReadBigCodeListV20(int count)
        {
            string countStr = count.ToString(CultureInfo.InvariantCulture);
            ICodelistMutableObject codelist = new CodelistMutableCore();
            codelist.Id = "CL_K" + countStr;
            codelist.AgencyId = "TEST";
            codelist.AddName("en", "Test CL with " + countStr);
            for (int i = 0; i < count; i++)
            {
                ICodeMutableObject code = new CodeMutableCore();
                code.Id = i.ToString(CultureInfo.InvariantCulture);
                code.AddName("en", "Code " + code.Id);
                codelist.AddItem(code);
            }

            var sw = new Stopwatch();
            string output = string.Format(CultureInfo.InvariantCulture, "big-2.0-immutable-codelist-{0}.xml", countStr);
            var writingManager = new StructureWriterManager();
            sw.Start();
            ICodelistObject immutableInstance = codelist.ImmutableInstance;
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "To immutable {0} took {1}", countStr, sw.Elapsed));
            CollectionAssert.IsNotEmpty(immutableInstance.Items);
            sw.Restart();
            using (FileStream writer = File.Create(output))
            {
                writingManager.WriteStructure(immutableInstance, new HeaderImpl("ZZ9", "ZZ9"), new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument)), writer);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Writing {0} took {1}", countStr, sw.Elapsed));

            sw.Reset();
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
            sw.Start();
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(output))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Reading {0} took {1}", countStr, sw.Elapsed));
        }

        /// <summary>
        /// The test write read big code list v 21.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000, Ignore = true)]
        public void TestWriteReadBigCodeListV21(int count)
        {
            string countStr = count.ToString(CultureInfo.InvariantCulture);
            ICodelistMutableObject codelist = new CodelistMutableCore();
            codelist.Id = "CL_K" + countStr;
            codelist.AgencyId = "TEST";
            codelist.AddName("en", "Test CL with " + countStr);
            for (int i = 0; i < count; i++)
            {
                ICodeMutableObject code = new CodeMutableCore();
                code.Id = i.ToString(CultureInfo.InvariantCulture);
                code.AddName("en", "Code " + code.Id);
                codelist.AddItem(code);
            }

            var sw = new Stopwatch();
            string output = string.Format(CultureInfo.InvariantCulture, "big-2.1-immutable-codelist-{0}.xml", countStr);
            var writingManager = new StructureWriterManager();
            sw.Start();
            ICodelistObject immutableInstance = codelist.ImmutableInstance;
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "To immutable {0} took {1}", countStr, sw.Elapsed));
            CollectionAssert.IsNotEmpty(immutableInstance.Items);
            sw.Restart();
            using (FileStream writer = File.Create(output))
            {
                writingManager.WriteStructure(immutableInstance, new HeaderImpl("ZZ9", "ZZ9"), new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), writer);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Writing {0} took {1}", countStr, sw.Elapsed));

            sw.Reset();
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            sw.Start();
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(output))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Reading {0} took {1}", countStr, sw.Elapsed));
        }

        /// <summary>
        /// The test xxx.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        //// [TestCase("tests/v21/demography.xml")]
        //// [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        //// [TestCase("tests/v21/response_demo_stub.xml", ExpectedException = typeof(MaintainableBeanException))]
        [TestCase("tests/v21/response_esms_descendants.xml")]
        [Ignore("Missing file")]
        public void TestXXX(string file)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
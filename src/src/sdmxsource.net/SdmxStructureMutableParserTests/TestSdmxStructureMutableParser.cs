// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxStructureMutableParser.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureMutableParserTests
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using Estat.Sri.SdmxStructureMutableParser.Engine.V2;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     Test unit for
    /// </summary>
    [TestFixture]
    public class TestSdmxStructureMutableParser : TestSdmxStructureBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test RI reader v 2.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        public void TestRiReaderV2(string file)
        {
            var structureReader = new RegistryInterfaceReaderV2();
            var fileInfo = new FileInfo(file);
            IRegistryInfo mutableObjects;
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            ISdmxObjects structure = null;
            using (FileStream stream = fileInfo.OpenRead())
            {
                settings.NameTable = NameTableCache.Instance.NameTable;
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    mutableObjects = structureReader.Read(reader);
                    Assert.NotNull(mutableObjects);
                    Assert.IsTrue(mutableObjects.HasQueryStructureRequest || mutableObjects.HasQueryStructureResponse);
                    if (mutableObjects.HasQueryStructureRequest)
                    {
                        CollectionAssert.IsNotEmpty(mutableObjects.QueryStructureRequest.References);
                    }
                    else if (mutableObjects.HasQueryStructureResponse)
                    {
                        CollectionAssert.IsNotEmpty(mutableObjects.QueryStructureResponse.Structure.AllMaintainables);
                        structure = mutableObjects.QueryStructureResponse.Structure.ImmutableObjects;
                        Assert.IsNotNull(structure);
                    }
                }
            }

            //// mutableObjects.Header = new HeaderImpl("ZZ9", "ZZ9");
            string output = string.Format(CultureInfo.InvariantCulture, "output-{0}", fileInfo.Name);
            using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings { CloseOutput = true, Indent = true }))
            {
                var structureWriter = new RegistryInterfaceWriterV2(writer);
                structureWriter.WriteRegistryInterface(mutableObjects);
            }

            using (XmlReader reader = XmlReader.Create(output, settings))
            {
                mutableObjects = structureReader.Read(reader);
                Assert.NotNull(mutableObjects);
                Assert.IsTrue(mutableObjects.HasQueryStructureRequest || mutableObjects.HasQueryStructureResponse);
                if (mutableObjects.HasQueryStructureRequest)
                {
                    CollectionAssert.IsNotEmpty(mutableObjects.QueryStructureRequest.References);
                }
                else if (mutableObjects.HasQueryStructureResponse)
                {
                    CollectionAssert.IsNotEmpty(mutableObjects.QueryStructureResponse.Structure.AllMaintainables);
                    ISdmxObjects newStructure = mutableObjects.QueryStructureResponse.Structure.ImmutableObjects;
                    Assert.IsNotNull(newStructure);
                    if (structure != null)
                    {
                        Assert.AreEqual(newStructure.CategorySchemes.Count, structure.CategorySchemes.Count);
                        Assert.AreEqual(newStructure.Codelists.Count, structure.Codelists.Count);
                        Assert.AreEqual(newStructure.ConceptSchemes.Count, structure.ConceptSchemes.Count);
                        Assert.AreEqual(newStructure.Dataflows.Count, structure.Dataflows.Count);
                        Assert.AreEqual(newStructure.DataStructures.Count, structure.DataStructures.Count);
                        Assert.AreEqual(newStructure.HierarchicalCodelists.Count, structure.HierarchicalCodelists.Count);

                        CompareArtefacts(newStructure.CategorySchemes, structure.GetCategorySchemes);
                        CompareArtefacts(newStructure.Codelists, structure.GetCodelists);
                        CompareArtefacts(newStructure.ConceptSchemes, structure.GetConceptSchemes);
                        CompareArtefacts(newStructure.Dataflows, structure.GetDataflows);
                        CompareArtefacts(newStructure.DataStructures, structure.GetDataStructures);
                        CompareArtefacts(newStructure.HierarchicalCodelists, structure.GetHierarchicalCodelists);

                        // compare against StructureWorkspace from SdmxStructureParser
                        var parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
                        using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
                        {
                            IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                            Assert.NotNull(structureWorkspace);
                            ISdmxObjects sdmxObjects = structureWorkspace.GetStructureObjects(false);
                            Assert.NotNull(sdmxObjects);
                            Assert.AreEqual(sdmxObjects.CategorySchemes.Count, structure.CategorySchemes.Count);
                            Assert.AreEqual(sdmxObjects.Codelists.Count, structure.Codelists.Count);
                            Assert.AreEqual(sdmxObjects.ConceptSchemes.Count, structure.ConceptSchemes.Count);
                            Assert.AreEqual(sdmxObjects.Dataflows.Count, structure.Dataflows.Count);
                            Assert.AreEqual(sdmxObjects.DataStructures.Count, structure.DataStructures.Count);
                            Assert.AreEqual(sdmxObjects.HierarchicalCodelists.Count, structure.HierarchicalCodelists.Count);

                            CompareArtefacts(sdmxObjects.CategorySchemes, structure.GetCategorySchemes);
                            CompareArtefacts(sdmxObjects.Codelists, structure.GetCodelists);
                            CompareArtefacts(sdmxObjects.ConceptSchemes, structure.GetConceptSchemes);
                            CompareArtefacts(sdmxObjects.Dataflows, structure.GetDataflows);
                            CompareArtefacts(sdmxObjects.DataStructures, structure.GetDataStructures);
                            CompareArtefacts(sdmxObjects.HierarchicalCodelists, structure.GetHierarchicalCodelists);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureReaderV2" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedNumberOfMaintainable">The expected number of maintainable.</param>
        [Test]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", 12)]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml", 12)]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", 6)]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", 1)]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 11)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml",  1)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", 1)]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", 2)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 18)]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", 1)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 16)]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml", 22)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 14)]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0-correct.xml", 13)]
        [TestCase("tests/v20/ESTAT+ESMS_MSD+2.2.xml", 15)]
        [TestCase("tests/v20/validation-error-structure.xml", 0, ExpectedException = typeof(XmlSchemaValidationException))]
        [TestCase("tests/v20/not_well_formed-structure.xml", 0, ExpectedException = typeof(XmlException))]
        public void TestStructureReaderV2(string file, int expectedNumberOfMaintainable)
        {
            var structureReader = new StructureReaderV2();
            var fileInfo = new FileInfo(file);
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            settings.ValidationEventHandler += OnValidationEventHandler;
            using (FileStream stream = fileInfo.OpenRead())
            {
                settings.NameTable = NameTableCache.Instance.NameTable;
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    IMutableObjects mutableObjects = structureReader.Read(reader);
                    Assert.NotNull(mutableObjects);
                    ISdmxObjects structure = mutableObjects.ImmutableObjects;
                    Assert.NotNull(structure);
                    Assert.AreEqual(expectedNumberOfMaintainable, structure.GetAllMaintainables().Count);
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureReaderV2"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/validation-error-structure.xml", ExpectedException = typeof(XmlSchemaValidationException))]
        [TestCase("tests/v20/not_well_formed-structure.xml", ExpectedException = typeof(XmlException))]
        public void TestStructureReaderWriteV2(string file)
        {
            var structureReader = new StructureReaderV2();
            var fileInfo = new FileInfo(file);
            IMutableObjects mutableObjects;
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            settings.ValidationEventHandler += OnValidationEventHandler;
            ISdmxObjects structure;
            using (FileStream stream = fileInfo.OpenRead())
            {
                settings.NameTable = NameTableCache.Instance.NameTable;
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    mutableObjects = structureReader.Read(reader);
                    Assert.NotNull(mutableObjects);
                    structure = mutableObjects.ImmutableObjects;
                    Assert.NotNull(structure);
                }
            }

            string output = string.Format(CultureInfo.InvariantCulture, "output-{0}", fileInfo.Name);
            using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings { CloseOutput = true, Indent = true }))
            {
                var structureWriter = new StructureWriterV2(writer);
                structureWriter.WriteStructure(mutableObjects, new HeaderImpl("ZZ9", "ZZ9"));
            }

            using (XmlReader reader = XmlReader.Create(output, settings))
            {
                IMutableObjects newMutableObjects = structureReader.Read(reader);
                Assert.NotNull(newMutableObjects);
                ISdmxObjects newStructure = newMutableObjects.ImmutableObjects;
                Assert.NotNull(newStructure);
                Assert.AreEqual(newStructure.CategorySchemes.Count, structure.CategorySchemes.Count);
                Assert.AreEqual(newStructure.Codelists.Count, structure.Codelists.Count);
                Assert.AreEqual(newStructure.ConceptSchemes.Count, structure.ConceptSchemes.Count);
                Assert.AreEqual(newStructure.Dataflows.Count, structure.Dataflows.Count);
                Assert.AreEqual(newStructure.DataStructures.Count, structure.DataStructures.Count);
                Assert.AreEqual(newStructure.HierarchicalCodelists.Count, structure.HierarchicalCodelists.Count);

                CompareArtefacts(newStructure.CategorySchemes, structure.GetCategorySchemes);
                CompareArtefacts(newStructure.Codelists, structure.GetCodelists);
                CompareArtefacts(newStructure.ConceptSchemes, structure.GetConceptSchemes);
                CompareArtefacts(newStructure.Dataflows, structure.GetDataflows);
                CompareArtefacts(newStructure.DataStructures, structure.GetDataStructures);
                CompareArtefacts(newStructure.HierarchicalCodelists, structure.GetHierarchicalCodelists);
            }

            // compare against StructureWorkspace from SdmxStructureParser
            var parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                ISdmxObjects newStructure = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(newStructure);
                Assert.AreEqual(newStructure.CategorySchemes.Count, structure.CategorySchemes.Count);
                Assert.AreEqual(newStructure.Codelists.Count, structure.Codelists.Count);
                Assert.AreEqual(newStructure.ConceptSchemes.Count, structure.ConceptSchemes.Count);
                Assert.AreEqual(newStructure.Dataflows.Count, structure.Dataflows.Count);
                Assert.AreEqual(newStructure.DataStructures.Count, structure.DataStructures.Count);
                Assert.AreEqual(newStructure.HierarchicalCodelists.Count, structure.HierarchicalCodelists.Count);

                CompareArtefacts(newStructure.CategorySchemes, structure.GetCategorySchemes);
                CompareArtefacts(newStructure.Codelists, structure.GetCodelists);
                CompareArtefacts(newStructure.ConceptSchemes, structure.GetConceptSchemes);
                CompareArtefacts(newStructure.Dataflows, structure.GetDataflows);
                CompareArtefacts(newStructure.DataStructures, structure.GetDataStructures);
                CompareArtefacts(newStructure.HierarchicalCodelists, structure.GetHierarchicalCodelists);
            }
        }

        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        public void TestV20Annotations(string file)
        {
            var structureReader = new StructureReaderV2();
            var fileInfo = new FileInfo(file);
            IMutableObjects mutableObjects;
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            settings.ValidationEventHandler += OnValidationEventHandler;
            ISdmxObjects structure;
            using (FileStream stream = fileInfo.OpenRead())
            {
                settings.NameTable = NameTableCache.Instance.NameTable;
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    mutableObjects = structureReader.Read(reader);
                    Assert.NotNull(mutableObjects);
                }
            }

            foreach (var categoryScheme in mutableObjects.CategorySchemes)
            {
                Assert.IsNotEmpty(categoryScheme.Annotations, categoryScheme.Id);
            }

        }


        /// <summary>
        /// The test v 21.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        [Ignore("No 2.1 support yet")]
        public void TestV21(string file)
        {
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwoPointOne);
            settings.ValidationEventHandler += OnValidationEventHandler;
            var structureReader = new StructureReaderV2();
            settings.NameTable = NameTableCache.Instance.NameTable;
            using (XmlReader reader = XmlReader.Create(file, settings))
            {
                IMutableObjects res = structureReader.Read(reader);
                Assert.NotNull(res);
            }
        }

        /// <summary>
        /// The test write read big code list.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000, Ignore = true)]
        public void TestWriteReadBigCodeList(int count)
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

            IMutableObjects mutableObjects = new MutableObjectsImpl();
            mutableObjects.AddCodelist(codelist);
            var sw = new Stopwatch();
            string output = string.Format(CultureInfo.InvariantCulture, "big-codelist-{0}.xml", countStr);
            sw.Start();
            using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings { CloseOutput = true, Indent = true }))
            {
                var structureWriter = new StructureWriterV2(writer);
                structureWriter.WriteStructure(mutableObjects, new HeaderImpl("ZZ9", "ZZ9"));
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Writing {0} took {1}", countStr, sw.Elapsed));

            sw.Reset();
            sw.Start();
            XmlReaderSettings settings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            settings.ValidationEventHandler += OnValidationEventHandler;
            var structureReader = new StructureReaderV2();
            settings.NameTable = NameTableCache.Instance.NameTable;
            using (XmlReader reader = XmlReader.Create(output, settings))
            {
                IMutableObjects res = structureReader.Read(reader);
                Assert.NotNull(res);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Reading {0} took {1}", countStr, sw.Elapsed));
        }

        #endregion
    }
}
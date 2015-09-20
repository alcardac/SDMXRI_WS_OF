// -----------------------------------------------------------------------
// <copyright file="TestStructureWritingManager.cs" company="Eurostat">
//   Date Created : 2013-07-19
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxStructureParserTests
{
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Factory;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.Structureparser.Factory;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// Test unit for <see cref="StructureWriterManager"/>
    /// </summary>
    [TestFixture]
    public class TestStructureWritingManager
    {
        /// <summary>
        /// The _writer manager
        /// </summary>
        private IStructureWriterManager _writerManager;

        /// <summary>
        /// The _parsing manager
        /// </summary>
        private IStructureParsingManager _parsingManager;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this._writerManager = new StructureWriterManager();
            this._parsingManager = new StructureParsingManager();
        }

        /// <summary>
        /// Tests the write coded time dimension V20.
        /// </summary>
        [Test]
        public void TestWriteCodedTimeDimensionV20()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.TimeDimension = true;
            dimension.Id = DimensionObject.TimeDimensionFixedId;
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "TIME_PERIOD");
            var structureReference = new StructureReferenceImpl("TEST_AGENCY", "CL_TIME_PERIOD", "1.0", SdmxStructureEnumType.CodeList);
            dimension.Representation = new RepresentationMutableCore() { Representation = structureReference };
            var immutable = BuildDataStructureObject(dimension);
            using (var stream = new MemoryStream())
            {
                this._writerManager.WriteStructure(immutable, new HeaderImpl("TEST", "TEST"), new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument)), stream);

                stream.Position = 0;
                using (var location = new MemoryReadableLocation(stream.ToArray()))
                {
                    var workspace = _parsingManager.ParseStructures(location);
                    var dsd = workspace.GetStructureObjects(false).DataStructures.First();
                    var timeDimension = dsd.TimeDimension;
                    Assert.IsTrue(timeDimension.HasCodedRepresentation());
                    Assert.AreEqual(timeDimension.Representation.Representation.AgencyId, structureReference.AgencyId);
                    Assert.AreEqual(timeDimension.Representation.Representation.MaintainableId, structureReference.MaintainableId);
                    Assert.AreEqual(timeDimension.Representation.Representation.Version, structureReference.Version);
                }
            }
        }

        /// <summary>
        /// Tests the write coded time dimension V21.
        /// </summary>
        [Test]
        public void TestWriteCodedTimeDimensionV21()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.TimeDimension = true;
            dimension.Id = DimensionObject.TimeDimensionFixedId;
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "TIME_PERIOD");
            var structureReference = new StructureReferenceImpl("TEST_AGENCY", "CL_TIME_PERIOD", "1.0", SdmxStructureEnumType.CodeList);
            dimension.Representation = new RepresentationMutableCore() { Representation = structureReference };
            var immutable = BuildDataStructureObject(dimension);
            using (var stream = new MemoryStream())
            {
                this._writerManager.WriteStructure(immutable, new HeaderImpl("TEST", "TEST"), new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), stream);

                stream.Position = 0;
                using (var location = new MemoryReadableLocation(stream.ToArray()))
                {
                    var workspace = _parsingManager.ParseStructures(location);
                    var dsd = workspace.GetStructureObjects(false).DataStructures.First();

                    var timeDimension = dsd.TimeDimension;
                    Assert.IsFalse(timeDimension.HasCodedRepresentation());
                    Assert.IsTrue(ObjectUtil.ValidCollection(timeDimension.GetAnnotationsByTitle("CODED_TIME_DIMENSION")));
                   
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures" />
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        public void TestWriteStructures(string file)
        {
            ISdmxObjects objects;
            var fileInfo = new FileInfo(file);
            using (IReadableDataLocation location = new FileReadableDataLocation(fileInfo))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            StructureOutputFormat format = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument);
            var sdmxStructureFormat = new SdmxStructureFormat(format);
            var outputFileName = string.Format("{0}-output.xml", fileInfo.Name);
            using (var stream = File.Create(outputFileName))
            {
                this._writerManager.WriteStructures(objects, sdmxStructureFormat, stream);    
            }

            using (var stream = File.OpenRead(outputFileName))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures" />
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0-correct.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/response.xml")]
        [TestCase("tests/v20/ESTAT+ESMS_MSD+2.2.xml")]
        public void TestWriteStructuresV2(string file)
        {
            ISdmxObjects objects;
            var fileInfo = new FileInfo(file);
            using (IReadableDataLocation location = new FileReadableDataLocation(fileInfo))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            StructureOutputFormat format = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument);
            var sdmxStructureFormat = new SdmxStructureFormat(format);
            var outputFileName = string.Format("{0}-output.xml", fileInfo.Name);
            using (var stream = File.Create(outputFileName))
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    IStructureWriterManager structureWritingManager = new StructureWriterManager(new SdmxStructureWriterV2Factory(writer));
                    structureWritingManager.WriteStructures(objects, sdmxStructureFormat, null);
                    writer.Flush();
                }
            }

            using (var stream = File.OpenRead(outputFileName))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }
        /// <summary>
        /// Builds the data structure object.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <returns>
        /// The <see cref="IDataStructureObject"/>
        /// </returns>
        private static IDataStructureObject BuildDataStructureObject(IDimensionMutableObject dimension)
        {
            IDataStructureMutableObject dsd = new DataStructureMutableCore() { Id = "TEST_DSD", AgencyId = "TEST", Version = "1.0" };
            dsd.AddName("en", "TEST_DSD");
            dsd.AddPrimaryMeasure(new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));
            dsd.AddDimension(dimension);

            var immutable = dsd.ImmutableInstance;
            return immutable;
        }

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures" />
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategoriesNoMDF.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/response.xml")]
        public void TestWriteStructuresV2ObjectCount(string file)
        {
            ISdmxObjects objects;
            var fileInfo = new FileInfo(file);
            using (IReadableDataLocation location = new FileReadableDataLocation(fileInfo))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            StructureOutputFormat format = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument);
            var sdmxStructureFormat = new SdmxStructureFormat(format);
            var outputFileName = string.Format("{0}-output.xml", fileInfo.Name);
            using (var stream = File.Create(outputFileName))
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    IStructureWriterManager structureWritingManager = new StructureWriterManager(new SdmxStructureWriterV2Factory(writer));
                    structureWritingManager.WriteStructures(objects, sdmxStructureFormat, null);
                    writer.Flush();
                }
            }

            ISdmxObjects objects2;
            using (IReadableDataLocation location = new FileReadableDataLocation(outputFileName))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects2 = structureWorkspace.GetStructureObjects(false);
            }

            Assert.AreEqual(objects.GetAllMaintainables().Count, objects2.GetAllMaintainables().Count);
        }

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures" />
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0-correct.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/QueryProvisioningRequest.xml")]
        [TestCase("tests/v20/QueryProvisioningResponse.xml")]
        [TestCase("tests/v20/QueryRegistrationRequest.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml")]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml")]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/SubmitStructureResponse.xml")]
        [TestCase("tests/v20/response.xml")]
        [TestCase("tests/v20/ESTAT+ESMS_MSD+2.2.xml")]
        public void TestWriteStructuresV2Default(string file)
        {
            ISdmxObjects objects;
            var fileInfo = new FileInfo(file);
            using (IReadableDataLocation location = new FileReadableDataLocation(fileInfo))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            StructureOutputFormat format = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument);
            var sdmxStructureFormat = new SdmxStructureFormat(format);
            var outputFileName = string.Format("{0}-output.xml", fileInfo.Name);
            using (var stream = File.Create(outputFileName))
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    IStructureWriterManager structureWritingManager = new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    structureWritingManager.WriteStructures(objects, sdmxStructureFormat, null);
                    writer.Flush();
                }
            }

            using (var stream = File.OpenRead(outputFileName))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures" />
        /// </summary>
        /// <param name="file">The file.</param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME_annotations.xml")]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0-correct.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/QueryRegistrationRequest.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/SubmitStructureResponse.xml")]
        [TestCase("tests/v20/response.xml")]
        [TestCase("tests/v20/ESTAT+ESMS_MSD+2.2.xml")]
        public void TestWriteStructuresV2DefaultObjectCount(string file)
        {
            ISdmxObjects objects;
            var fileInfo = new FileInfo(file);
            using (IReadableDataLocation location = new FileReadableDataLocation(fileInfo))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            StructureOutputFormat format = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV2StructureDocument);
            var sdmxStructureFormat = new SdmxStructureFormat(format);
            var outputFileName = string.Format("{0}-output.xml", fileInfo.Name);
            using (var stream = File.Create(outputFileName))
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    IStructureWriterManager structureWritingManager = new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    structureWritingManager.WriteStructures(objects, sdmxStructureFormat, null);
                    writer.Flush();
                }
            }

            ISdmxObjects objects2;
            using (IReadableDataLocation location = new FileReadableDataLocation(outputFileName))
            {
                var structureWorkspace = this._parsingManager.ParseStructures(location);
                objects2 = structureWorkspace.GetStructureObjects(false);
            }

            Assert.AreEqual(objects.GetAllMaintainables().Count, objects2.GetAllMaintainables().Count);
        }


    }
}
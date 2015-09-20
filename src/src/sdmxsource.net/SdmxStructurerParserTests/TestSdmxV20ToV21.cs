// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxV20ToV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="StructureWritingManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureParserTests
{
    using System.Globalization;
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     Test unit for <see cref="StructureWriterManager" />
    /// </summary>
    [TestFixture]
    public class TestSdmxV20ToV21
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="StructureWriterManager.WriteStructures"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
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
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/ESTAT+STS+3.1-dependencies.xml")]
        [TestCase("tests/v20/allcodelists-for-hcl.xml")]
        public void TestWriteStructures(string file)
        {
            var structureReader = new StructureParsingManager();
            var fileInfo = new FileInfo(file);
            IStructureWorkspace structureWorkspace;
            using (var readable = new FileReadableDataLocation(fileInfo))
            {
                structureWorkspace = structureReader.ParseStructures(readable);
            }

            ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);

            string output = string.Format(CultureInfo.InvariantCulture, "test-sdmxv2.1-{0}", fileInfo.Name);
            var writtingManager = new StructureWriterManager();
            using (var outputStream = new FileStream(output, FileMode.Create))
            {
                writtingManager.WriteStructures(structureBeans, new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), outputStream);
            }

            using (var readable = new FileReadableDataLocation(output))
            {
                XMLParser.ValidateXml(readable, SdmxSchemaEnumType.VersionTwoPointOne);
                var structures = structureReader.ParseStructures(readable);
                Assert.NotNull(structures);
            }
        }

        #endregion
    }
}
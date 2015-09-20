// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStaxUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="StaxUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System.Xml;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Util.Xml;

    /// <summary>
    ///     Test unit for <see cref="StaxUtil" />
    /// </summary>
    [TestFixture]
    public class TestStaxUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="StaxUtil.SkipNode"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        public void TestSkipNode(string file)
        {
            using (XmlReader reader = XmlReader.Create(file))
            {
                reader.ReadToDescendant("Header", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message");
                StaxUtil.SkipNode(reader);
                reader.MoveToContent();
                Assert.AreEqual("QueryStructureResponse", reader.LocalName);
            }
        }

        /// <summary>
        /// Test unit for <see cref="StaxUtil.SkipToEndNode"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
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
        public void TestSkipToEndNode(string file)
        {
            using (XmlReader reader = XmlReader.Create(file))
            {
                Assert.IsTrue(StaxUtil.SkipToEndNode(reader, "Header"));
            }
        }

        /// <summary>
        /// Test unit for <see cref="StaxUtil.SkipToNode"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
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
        public void TestSkipToNode(string file)
        {
            using (XmlReader reader = XmlReader.Create(file))
            {
                Assert.IsTrue(StaxUtil.SkipToNode(reader, "Header"));
            }
        }

        #endregion
    }
}
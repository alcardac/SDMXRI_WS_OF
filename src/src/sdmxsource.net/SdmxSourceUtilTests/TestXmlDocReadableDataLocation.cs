namespace SdmxSourceUtilTests
{
    using System.Xml;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="XmlDocReadableDataLocation"/> </summary>
    [TestFixture]
    public class TestXmlDocReadableDataLocation
    {
        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation."/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/CR1_12_data_report.xml")]
        public void Test(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);
            using (var readable = new XmlDocReadableDataLocation(doc))
            {
                Assert.IsTrue(readable.InputStream.CanRead);
                Assert.IsTrue(readable.InputStream.ReadByte() > 0);
                Assert.AreNotSame(readable.InputStream, readable.InputStream);
            }
        }
    }
}
namespace SdmxSourceUtilTests
{
    using System.IO;
    using System.Xml;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="XmlDocReadableDataLocation"/> </summary>
    [TestFixture]
    public class TestSdmxXmlStream
    {
        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        public void TestXmlDocCtor(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);

            using (var readable = new SdmxXmlStream(doc, MessageEnumType.Structure, SdmxSchemaEnumType.VersionTwo, RegistryMessageEnumType.Null))
            {
                Assert.IsTrue(readable.HasReader);
                Assert.IsNotNull(readable.Reader);
                Assert.AreEqual(MessageEnumType.Structure, readable.MessageType);
                Assert.AreEqual(SdmxSchemaEnumType.VersionTwo, readable.SdmxVersion);
                Assert.AreEqual(RegistryMessageEnumType.Null, readable.RegistryType);
            }
        }

        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        public void TestXmlReaderCtor(string file)
        {
            using (var doc = XmlReader.Create(file))
            using (var readable = new SdmxXmlStream(doc, MessageEnumType.Structure, SdmxSchemaEnumType.VersionTwo, RegistryMessageEnumType.Null))
            {
                Assert.IsTrue(readable.HasReader);
                Assert.IsNotNull(readable.Reader);
                Assert.AreEqual(MessageEnumType.Structure, readable.MessageType);
                Assert.AreEqual(SdmxSchemaEnumType.VersionTwo, readable.SdmxVersion);
                Assert.AreEqual(RegistryMessageEnumType.Null, readable.RegistryType);
            }
        }


        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        public void TestReadableDataLocationCtor(string file)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                using (var sdmxXmlStream = new SdmxXmlStream(readable))
                {
                    Assert.IsTrue(sdmxXmlStream.HasReader);
                    Assert.IsNotNull(sdmxXmlStream.Reader);
                    Assert.AreEqual(MessageEnumType.Structure, sdmxXmlStream.MessageType);
                    Assert.AreEqual(SdmxSchemaEnumType.VersionTwo, sdmxXmlStream.SdmxVersion);
                    Assert.AreEqual(RegistryMessageEnumType.Null, sdmxXmlStream.RegistryType);
                }
            }
        }

        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        public void TestReadableDataLocationCtor2(string file)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                using (var xmlStream = new SdmxXmlStream(readable))
                using (var sdmxXmlStream = new SdmxXmlStream(xmlStream))
                {
                    Assert.IsTrue(sdmxXmlStream.HasReader);
                    Assert.IsNotNull(sdmxXmlStream.Reader);
                    Assert.AreEqual(MessageEnumType.Structure, sdmxXmlStream.MessageType);
                    Assert.AreEqual(SdmxSchemaEnumType.VersionTwo, sdmxXmlStream.SdmxVersion);
                    Assert.AreEqual(RegistryMessageEnumType.Null, sdmxXmlStream.RegistryType);
                }
            }
        }

        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        [Test]
        public void TestXmlWriterCtor()
        {
            using (var doc = XmlWriter.Create("TestXmlWriterCtor.xml"))
            using (var readable = new SdmxXmlStream(doc))
            {
                Assert.IsTrue(readable.HasWriter);
                Assert.IsNotNull(readable.Writer);
            }
        }


        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        [Test]
        public void TestOutStreamCtor()
        {
            using (var doc = new FileStream("TestXmlWriterCtor.xml", FileMode.Create))
            using (var readable = new SdmxXmlStream(doc, true))
            {
                Assert.IsTrue(readable.HasWriter);
                Assert.IsNotNull(readable.Writer);
            }
        }

        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        [Test]
        public void TestWritableCtor()
        {
            using (var doc = new WriteableDataLocationTmp())
            {
                using (var readable = new SdmxXmlStream(doc, true))
                {
                    Assert.IsTrue(readable.HasWriter);
                    Assert.IsNotNull(readable.Writer);
                }
            }
        }


        /// <summary>
        /// Test method for <see cref="XmlDocReadableDataLocation"/> 
        /// </summary>
        [Test]
        public void TestWritableCtor2()
        {
            using (var doc = new WriteableDataLocationTmp())
            {
                using (var b = new SdmxXmlStream(doc, true))
                using (var readable = new SdmxXmlStream(b, true))
                {
                    Assert.IsTrue(readable.HasWriter);
                    Assert.IsNotNull(readable.Writer);
                }
            }
        }
    }
}
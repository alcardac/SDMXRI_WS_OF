// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestURIUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="URIUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     Test unit class for <see cref="URIUtil" />
    /// </summary>
    [TestFixture]
    public class TestUriUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test method for <see cref="URIUtil.GetFileFromStream"/>
        /// </summary>
        /// <param name="testFile">
        /// The test File.
        /// </param>
        [TestCase("tests/TestFile.csv")]
        public void TestGetFileFromStream(string testFile)
        {
            URIUtil tempUriUtil = URIUtil.TempUriUtil;
            Assert.IsNotNull(tempUriUtil);
            FileInfo file = tempUriUtil.GetFileFromStream(File.OpenRead(testFile));
            Assert.NotNull(file);
            Assert.IsTrue(file.Exists);
            FileInfo file2 = tempUriUtil.GetFileFromStream(File.OpenRead(testFile));
            Assert.IsTrue(file2.Exists);
            Assert.AreNotEqual(file2.FullName, file.FullName);
        }

        /// <summary>
        /// Test method for <see cref="URIUtil.GetFileFromUri"/>
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        [TestCase("https://registry.sdmx.org/schemas/v2_0/SDMXMessage.xsd")]
        public void TestGetFileFromUri(string location)
        {
            URIUtil tempUriUtil = URIUtil.TempUriUtil;
            Assert.IsNotNull(tempUriUtil);
            FileInfo file = tempUriUtil.GetFileFromUri(new Uri(location));
            Assert.IsTrue(file.Exists);
            FileInfo file2 = tempUriUtil.GetFileFromUri(new Uri(location));
            Assert.IsTrue(file2.Exists);
            Assert.AreNotEqual(file2.FullName, file.FullName);
        }

        /// <summary>
        ///     Test method for <see cref="URIUtil.TempUriUtil" />
        /// </summary>
        [Test]
        public void TestTempUriUtil()
        {
            Assert.IsNotNull(URIUtil.TempUriUtil);
            FileInfo tempFile = URIUtil.TempUriUtil.GetTempFile();
            Assert.IsNotNull(tempFile);
            Assert.IsTrue(tempFile.Exists);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStreamUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="StreamUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     Test unit class for <see cref="StreamUtil" />
    /// </summary>
    [TestFixture]
    public class TestStreamUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test method for <see cref="StreamUtil.CloseStream"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/TestFile.csv")]
        public void TestCloseStream(string file)
        {
            var stream = new FileStream(file, FileMode.Open);
            Assert.IsTrue(stream.CanRead);
            StreamUtil.CloseStream(stream);
            Assert.IsFalse(stream.CanRead);
        }

        /// <summary>
        /// Test method for <see cref="StreamUtil.CopyFirstXLines"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="lines">
        /// The lines.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/TestFile.csv", 3, 3)]
        [TestCase("tests/TestFile.csv", 0, 0)]
        [TestCase("tests/TestFile.csv", 50, 41)]
        public void TestCopyFirstXLines(string file, int lines, int expectedResult)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                IList<string> copyFirstXLines = StreamUtil.CopyFirstXLines(stream, lines);
                Assert.IsNotNull(copyFirstXLines);
                CollectionAssert.AllItemsAreNotNull(copyFirstXLines);
                Assert.AreEqual(expectedResult, copyFirstXLines.Count);
            }
        }

        /// <summary>
        /// Test method for <see cref="StreamUtil.CopyStream"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/TestFile.csv")]
        public void TestCopyStream(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                using (var output = new FileStream("test-out.csv", FileMode.Create))
                {
                    StreamUtil.CopyStream(stream, output);
                }
            }
        }

        /// <summary>
        /// Test method for <see cref="StreamUtil.ToByteArray"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/TestFile.csv")]
        public void TestToByteArray(string file)
        {
            var fileInfo = new FileInfo(file);
            using (var stream = new FileStream(file, FileMode.Open))
            {
                byte[] byteArray = StreamUtil.ToByteArray(stream);
                Assert.IsNotEmpty(byteArray);
                Assert.AreEqual(fileInfo.Length, byteArray.Length);
            }
        }

        #endregion
    }
}
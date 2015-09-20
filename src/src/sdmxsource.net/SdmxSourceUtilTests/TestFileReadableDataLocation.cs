namespace SdmxSourceUtilTests
{
    using System.Diagnostics;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="FileReadableDataLocation"/> </summary>
    [TestFixture]
    public class TestFileReadableDataLocation
    {
        /// <summary>
        /// Test method for <see cref="FileReadableDataLocation.InputStream"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/TestFile.csv")]
        public void Test(string file)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                Assert.IsTrue(readable.InputStream.ReadByte() > 0);
                Assert.AreNotSame(readable.InputStream, readable.InputStream);
            }
        }
    }
}
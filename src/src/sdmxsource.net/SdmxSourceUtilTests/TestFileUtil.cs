namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="FileUtil"/> </summary>
    [TestFixture]
    public class TestFileUtil
    {
        /// <summary>Test method for <see cref="FileUtil.CreateTemporaryFile"/> </summary>
        [Test]
        public void Test()
        {
            var temporaryFile = FileUtil.CreateTemporaryFile("test", "tst");
            Assert.NotNull(temporaryFile);
            Assert.IsTrue(temporaryFile.Exists);
            FileUtil.DeleteFile(temporaryFile.FullName);
            temporaryFile.Refresh();
            Assert.IsFalse(temporaryFile.Exists);
        }
    }
}
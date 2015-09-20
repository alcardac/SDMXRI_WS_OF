namespace SdmxSourceUtilTests
{
    using System.Text;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="MemoryReadableLocation"/> </summary>
    [TestFixture]
    public class TestMemoryReadableLocation
    {
        /// <summary>Test method for <see cref="MemoryReadableLocation.InputStream"/> </summary>
        [Test]
        public void Test()
        {
            const string Buffer = "123456";

            using (var readable = new MemoryReadableLocation(Encoding.UTF8.GetBytes(Buffer)))
            {
                Assert.IsTrue(readable.InputStream.ReadByte() > 0);
                Assert.AreNotSame(readable.InputStream, readable.InputStream);
            }
        }
    }
}
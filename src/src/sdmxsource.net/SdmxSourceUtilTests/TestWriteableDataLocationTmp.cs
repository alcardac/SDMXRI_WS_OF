namespace SdmxSourceUtilTests
{
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;

    /// <summary> Test unit class for <see cref="WriteableDataLocationTmp"/> </summary>
    [TestFixture]
    public class TestWriteableDataLocationTmp
    {
        /// <summary>Test method for <see cref="WriteableDataLocationTmp"/> </summary>
        [Test]
        public void Test()
        {
            using (var writeable = new WriteableDataLocationTmp())
            {
                using (Stream outputStream = writeable.OutputStream)
                {
                    Assert.IsTrue(outputStream.CanWrite);
                    outputStream.WriteByte(0x1);
                    outputStream.Flush();
                }

                using (Stream inputStream = writeable.InputStream)
                {
                    Assert.IsTrue(inputStream.CanRead);
                    int readByte = inputStream.ReadByte();
                    Assert.AreEqual(readByte,0x1);
                }
            }
        }
    }
}
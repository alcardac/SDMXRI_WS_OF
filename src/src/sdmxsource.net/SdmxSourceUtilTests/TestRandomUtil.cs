namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Util.Random;

    /// <summary> Test unit class for <see cref="RandomUtil"/> </summary>
    [TestFixture]
    public class TestRandomUtil
    {
        /// <summary>
        /// Test method for <see cref="RandomUtil.GenerateRandomPassword"/> 
        /// </summary>
        /// <param name="len">
        /// The len.
        /// </param>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(15)]
        [TestCase(32)]
        [TestCase(1024)]
        public void TestGenerateRandomPassword(int len)
        {
            string generateRandomPassword = RandomUtil.GenerateRandomPassword(len);
            Assert.IsNotNull(generateRandomPassword);
            Assert.AreEqual(len, generateRandomPassword.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    Assert.AreNotEqual(generateRandomPassword, RandomUtil.GenerateRandomPassword(len));
                }
            }
        }

        /// <summary>Test method for <see cref="RandomUtil.GenerateRandomString"/> </summary>
        [Test]
        public void TestGenerateRandomString()
        {
            string generateRandomString = RandomUtil.GenerateRandomString();
            Assert.IsNotNullOrEmpty(generateRandomString);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreNotEqual(generateRandomString, RandomUtil.GenerateRandomString());
            }
        }
    }
}
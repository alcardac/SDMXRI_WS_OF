// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEmailValidation.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="EmailValidation" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Util.Email;

    /// <summary>
    ///     Test unit class for <see cref="EmailValidation" />
    /// </summary>
    [TestFixture]
    public class TestEmailValidation
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test method for <see cref="EmailValidation.ValidateEmail"/>
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("soemone@domain.com", true)]
        [TestCase("soemone@domain.foo.com", true)]
        [TestCase("soemone@domain.foo.bar.com", true)]
        [TestCase("name.soemone@domain.com", true)]
        [TestCase("n1ame.soem4one@domain.foo.com", true)]
        [TestCase("name.soemone@domain.foo.bar.com", true)]
        [TestCase("name.soemone@domai3n.foo.bar.com", true)]
        [TestCase("name.soe4mone@domain.eu", true)]
        [TestCase("name.soemo2ne@domain.int", true)]
        [TestCase("name.s1232oemone@do3main.org", true)]
        [TestCase("NAME.S1232OEMONE@DO3MAIn.org", true)]
        [TestCase("name.soemone-nodomain", false)]
        [TestCase("namesoemonenodomain", false)]
        [TestCase("name@192.168.1.1", true)]
        public void Test(string email, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, EmailValidation.ValidateEmail(email));
        }

        #endregion
    }
}
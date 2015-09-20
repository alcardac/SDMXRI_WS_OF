// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValidationUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for ValidationUtil
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     Test unit for <see cref="ValidationUtil"/>
    /// </summary>
    [TestFixture]
    public class TestValidationUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="ValidationUtil.CleanAndValidateId"/>
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="startWithInteger">
        /// The start With Integer.
        /// </param>
        [Test]
        [TestCase("PAOK", false)]
        [TestCase("PAOK1", false)]
        [TestCase("paok1", false)]
        [TestCase("paok$4", false)]
        [TestCase("paok@4", false)]
        [TestCase("paok@4_ole", false)]
        [TestCase("paok@4-*ole*", false)]
        [TestCase("paok_$4-@*ole*_1234_", false)]
        [TestCase("1233k_$4-@*ole*_1234_", false, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("1233", true)]
        [TestCase("123$3@", true)]
        [TestCase("123A-Z", false, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase(null, true)]
        [TestCase(null, false)]
        [TestCase("", true)]
        [TestCase("", false)]
        [TestCase("(1.0)", false, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("(1.0)", true, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("STS)", true, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("STS)", false, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("123STS)", true, ExpectedException = typeof(SdmxSemmanticException))]
        [TestCase("$@STS)", false, ExpectedException = typeof(SdmxSemmanticException))]
        public void TestCleanAndValidateId(string id, bool startWithInteger)
        {
            string validateId = ValidationUtil.CleanAndValidateId(id, startWithInteger);
            if (!string.IsNullOrWhiteSpace(id))
            {
                Assert.AreEqual(id, validateId);
            }
            else
            {
                Assert.IsNull(validateId);
            }
        }

        /// <summary>
        /// Test unit for <see cref="ValidationUtil.ValidateTextType"/>
        /// </summary>
        [Test]
        public void TestValidateTextType()
        {
            var textTypes = new ITextTypeWrapper[][]
                                                 {
                                                     new[] { new TextTypeWrapperImpl("en", "Test1", null) },
                                                     new[] { new TextTypeWrapperImpl("en", "Test1", null), new TextTypeWrapperImpl("el", "Test1", null) },
                                                     new[] { new TextTypeWrapperImpl("en", "This is a test.", null), new TextTypeWrapperImpl("el", "δοκιμή αλαλαλαλαα", null), new TextTypeWrapperImpl("de", "Teste. Straß. Können Sie nichts.", null) }
                                                 };

            foreach (var textTypeWrapper in textTypes)
            {
                ValidationUtil.ValidateTextType(textTypeWrapper, null);
            }

            textTypes = new ITextTypeWrapper[][]
                                                 {
                                                     new[] { new TextTypeWrapperImpl("en", "Test1", null), new TextTypeWrapperImpl("en", "Test1", null) },
                                                     new[] { new TextTypeWrapperImpl("en", "This is a test.", null), new TextTypeWrapperImpl("el", "δοκιμή αλαλαλαλαα", null), new TextTypeWrapperImpl("en", "Teste. Straß. Können Sie nichts.", null) },
                                                     new[] { new TextTypeWrapperImpl("en", "This is a test.", null), new TextTypeWrapperImpl("en", "δοκιμή αλαλαλαλαα", null), new TextTypeWrapperImpl("de", "Teste. Straß. Können Sie nichts.", null) }
                                                 };
            foreach (var textTypeWrapper in textTypes)
            {
                Assert.Throws<SdmxSemmanticException>(() => ValidationUtil.ValidateTextType(textTypeWrapper, null));
            }
        }

        #endregion
    }
}
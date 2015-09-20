namespace SdmxSourceUtilTests
{
    using System.Collections.Generic;
    using System.Globalization;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary> Test unit class for <see cref="LocaleUtil"/> </summary>
    [TestFixture]
    public class TestLocaleUtil
    {
        /// <summary>
        /// Test method for <see cref="LocaleUtil.BuildLocalMap"/> 
        /// </summary>
        /// <param name="locale">
        /// The locale.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        [TestCase("en","This is a test")]
        [TestCase("el", "Μια δοκιμή")]
        [TestCase("INVALID_LOCALE", "SHOULD FAIL", ExpectedException = typeof(SdmxSemmanticException))]
        public void Test(string locale, string text)
        {
            LocaleUtil.BuildLocalMap(new List<ITextTypeWrapper> { new TextTypeWrapperImpl(locale, text, null) });
        }
    }
}
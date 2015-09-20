// -----------------------------------------------------------------------
// <copyright file="TestSdmxMessageUtilExt.cs" company="EUROSTAT">
//   Date Created : 2015-02-02
//   Copyright (c) 2015 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using System.Linq;

    using Estat.Sdmxsource.Extension.Util;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="SdmxMessageUtilExt"/>
    /// </summary>
    [TestFixture]
    public class TestSdmxMessageUtilExt
    {

        /// <summary>
        /// Test unit for <see cref="SdmxMessageUtilExt.ParseSdmxFooterMessage" />
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="code">The code.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="firstText">The first text.</param>
        [TestCase(@"tests\V21\data-with-footer.xml", "510", Severity.Error, "Response size exceeds service limit : Reached configured limit : 10 observations")]
        public void TestParseSdmxFooterMessage(string fileName, string code, Severity severity, string firstText)
        {
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(fileName))
            {
                var sdmxFooterMessage = SdmxMessageUtilExt.ParseSdmxFooterMessage(dataLocation);
                Assert.IsNotNull(sdmxFooterMessage);
                Assert.AreEqual(sdmxFooterMessage.Code, code);
                Assert.AreEqual(sdmxFooterMessage.Severity, severity); 
                Assert.IsNotEmpty(sdmxFooterMessage.FooterText);
                Assert.AreEqual(firstText, sdmxFooterMessage.FooterText.First().Value);
            }
        }
    }
}
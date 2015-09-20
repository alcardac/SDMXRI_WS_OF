// -----------------------------------------------------------------------
// <copyright file="TestScanner.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxSourceUtilTests
{
    using System;
    using System.Globalization;
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Text;

    /// <summary>
    /// Test unit for <see cref="Scanner"/>
    /// </summary>
    [TestFixture]
    public class TestScanner
    {
        /// <summary>
        /// Test unit for <see cref="Scanner" />
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="input">The input.</param>
        [TestCase("'", "L0TEST'\nL1'L2'L3'L4'\nL5+dldldldlldl'")]
        [TestCase("'", "L0TEST'\nL1'L2'L3'L4'\nL5+dldldldlldl'\n")]
        [TestCase("'", "L0TEST'\nL1'L2'L3'L4'\nL5+dldldldlldl'L6\n")]
        public void Test(string delimiter, string input)
        {
            using (var reader = new StringReader(input))
            using (var scanner = new Scanner(reader))
            {
                scanner.UseDelimiter(delimiter);
                int index = 0;
                while (scanner.HasNext())
                {
                    string token = scanner.Next().Trim('\n');
                    if (token.Length > 0)
                    {
                        string expected = string.Format(CultureInfo.InvariantCulture, "L{0}", index++);
                        Assert.IsTrue(token.StartsWith(expected, StringComparison.Ordinal), "Expected to start with '{0}' got token '{1}'", expected, token);
                    }
                }
            }
        }
    }
}
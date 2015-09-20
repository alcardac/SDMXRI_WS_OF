// -----------------------------------------------------------------------
// <copyright file="TestVersionableUtil.cs" company="Eurostat">
//   Date Created : 2013-01-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Util;

    /// <summary>
    /// Test unit for <see cref="VersionableUtil"/>
    /// </summary>
    [TestFixture]
    public class TestVersionableUtil
    {

        /// <summary>
        /// Test unit for <see cref="VersionableUtil.IncrementVersion"/> 
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="majorInc">
        /// If major version should be incremented.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("1", true, "2.0")]
        [TestCase("1.0", true, "2.0")]
        [TestCase("1.4", true, "2.0")]
        [TestCase("1.4.3", true, "2.0")]
        [TestCase("2", true, "3.0")]
        [TestCase("2.0", true, "3.0")]
        [TestCase("2.1", true, "3.0")]
        [TestCase("2", false, "2.1")]
        [TestCase("2.0", false, "2.1")]
        [TestCase("2.0.0", false, "2.1")]
        [TestCase("2.0.1", false, "2.1")]
        public void TestIncrementVersion(string version, bool majorInc, string expectedResult)
        {
            Assert.AreEqual(expectedResult, VersionableUtil.IncrementVersion(version, majorInc));
        }

        /// <summary>
        /// Test unit for <see cref="VersionableUtil.IsHigherVersion"/> 
        /// </summary>
        /// <param name="versionA">
        /// The version A.
        /// </param>
        /// <param name="versionB">
        /// The version B.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("1", "2.0", false)]
        [TestCase("1.1", "2.0", false)]
        [TestCase("1.6", "2.0", false)]
        [TestCase("1.3.3", "2.0", false)]
        [TestCase("1", "2", false)]
        [TestCase("1.1", "2", false)]
        [TestCase("1.6", "2", false)]
        [TestCase("1.3.3", "2", false)]
        [TestCase("1", "2.2.3", false)]
        [TestCase("1.1", "2.2.3", false)]
        [TestCase("1.6", "2.9.9", false)]
        [TestCase("1.3.3", "2.9.3", false)]
        [TestCase("3", "2.0", true)]
        [TestCase("3.1", "2.0", true)]
        [TestCase("3.6", "2.0", true)]
        [TestCase("3.3.3", "2.0", true)]
        [TestCase("3", "2", true)]
        [TestCase("3.1", "2", true)]
        [TestCase("3.6", "2", true)]
        [TestCase("3.3.3", "2", true)]
        [TestCase("3", "2.2.3", true)]
        [TestCase("3.1", "2.2.3", true)]
        [TestCase("3.6", "2.9.9", true)]
        [TestCase("3.3.3", "2.9.3", true)]
        public void TestIsHigherVersion(string versionA, string versionB, bool expectedResult)
        {
            Assert.IsTrue(expectedResult == VersionableUtil.IsHigherVersion(versionA, versionB));
        }

        /// <summary>
        /// Test unit for <see cref="VersionableUtil.ValidVersion"/> 
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("1", true)]
        [TestCase("1.1", true)]
        [TestCase("1.6", true)]
        [TestCase("1.3.3", true)]
        [TestCase("1", true)]
        [TestCase("1.1", true)]
        [TestCase("1.6", true)]
        [TestCase("1.3.3", true)]
        [TestCase("1", true)]
        [TestCase("1.1", true)]
        [TestCase("1.6", true)]
        [TestCase("1.3.3", true)]
        [TestCase("3", true)]
        [TestCase("3.1", true)]
        [TestCase("3.6", true)]
        [TestCase("3.3.3", true)]
        [TestCase("3", true)]
        [TestCase("3.1", true)]
        [TestCase("3.6", true)]
        [TestCase("3.3.3", true)]
        [TestCase("3", true)]
        [TestCase("3.1", true)]
        [TestCase("3.6", true)]
        [TestCase("3.3.3", true)]
        [TestCase("1.o.1", false)]
        [TestCase("", false)]
        [TestCase("1.", false)]
        [TestCase("1.A", false)]
        [TestCase("0x02", false)]
        [TestCase("02", true)]
        [TestCase(".1", false)]
        [TestCase(".1.", false)]
        [TestCase("VERSION", false)]
        public void TestValidVersion(string version, bool expectedResult)
        {
            Assert.IsTrue(expectedResult == VersionableUtil.ValidVersion(version));
        }
    }
}
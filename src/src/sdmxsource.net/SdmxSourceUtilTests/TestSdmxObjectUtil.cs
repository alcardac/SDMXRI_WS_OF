// -----------------------------------------------------------------------
// <copyright file="TestSdmxObjectUtil.cs" company="Eurostat">
//   Date Created : 2013-01-17
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    /// Test unit for <see cref="SdmxObjectUtil"/>
    /// </summary>
    [TestFixture]
    public class TestSdmxObjectUtil
    {
        /// <summary>
        /// Test unit for <see cref="SdmxObjectUtil.CreateTertiary(bool,bool)"/> 
        /// </summary>
        /// <param name="isSet">
        /// The is Set.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="expectedValue">
        /// The expected Value.
        /// </param>
        [TestCase(false, false, TertiaryBoolEnumType.Unset)]
        [TestCase(false, true, TertiaryBoolEnumType.Unset)]
        [TestCase(true, false, TertiaryBoolEnumType.False)]
        [TestCase(true, true, TertiaryBoolEnumType.True)]
        public void TestCreateTertiary(bool isSet, bool value, TertiaryBoolEnumType expectedValue)
        {
           Assert.AreEqual(expectedValue, SdmxObjectUtil.CreateTertiary(isSet, value).EnumType); 
        }

        /// <summary>
        /// Test unit for <see cref="SdmxObjectUtil.CreateTertiary(bool,bool)"/> 
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="expectedValue">
        /// The expected Value.
        /// </param>
        [TestCase(null, TertiaryBoolEnumType.Unset)]
        [TestCase(false, TertiaryBoolEnumType.False)]
        [TestCase(true, TertiaryBoolEnumType.True)]
        public void TestCreateTertiary2(bool? value, TertiaryBoolEnumType expectedValue)
        {
            Assert.AreEqual(expectedValue, SdmxObjectUtil.CreateTertiary(value).EnumType);
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="TestDateUtil.cs" company="Eurostat">
//   Date Created : 2012-12-05
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// Test unit for <see cref="DateUtil"/> 
    /// </summary>
    [TestFixture]
    public class TestDateUtil
    {
        /// <summary>
        /// Test the <see cref="DateUtil.GetTimeFormatOfDate"/>
        /// </summary>
        /// <param name="dateStr">
        /// The date string.
        /// </param>
        /// <param name="timeFormat">
        /// The expected time Format.
        /// </param>
        [Test]
        [TestCase("2003", TimeFormatEnumType.Year)]
        [TestCase("1900", TimeFormatEnumType.Year)]
        [TestCase("2500", TimeFormatEnumType.Year)]
        [TestCase("2003-01", TimeFormatEnumType.Month)]
        [TestCase("2003-12", TimeFormatEnumType.Month)]
        [TestCase("2004-12", TimeFormatEnumType.Month)]
        [TestCase("2004-01", TimeFormatEnumType.Month)]
        [TestCase("2004-02", TimeFormatEnumType.Month)]
        [TestCase("2003-02", TimeFormatEnumType.Month)]
        [TestCase("2012-02", TimeFormatEnumType.Month)]
        [TestCase("2004-06", TimeFormatEnumType.Month)]
        [TestCase("2004-13", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-00", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-1", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-B3", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-Q5", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-Q0", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-Q", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("20040", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-W02", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-01-00", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-00-01", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-13-01", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-02-30", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-09-31", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-08-32", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        //// no validation takes place here[TestCase("2003-02-29", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2012-02-29T24:00:00", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2012-02-29 13:00:00", TimeFormatEnumType.Null, ExpectedException = typeof(Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException))]
        [TestCase("2004-Q2", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2004-Q1", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2004-Q2", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2004-Q3", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2012-Q2", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2012-Q3", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2004-Q4", TimeFormatEnumType.QuarterOfYear)]
        [TestCase("2004-B1", TimeFormatEnumType.HalfOfYear)]
        [TestCase("2004-B2", TimeFormatEnumType.HalfOfYear)]
        [TestCase("2004-W3", TimeFormatEnumType.Week)]
        [TestCase("2004-W33", TimeFormatEnumType.Week)]
        [TestCase("2004-W52", TimeFormatEnumType.Week)]
        [TestCase("2004-W53", TimeFormatEnumType.Week)]
        [TestCase("2003-W53", TimeFormatEnumType.Week)]
        //// TODO support 3rd of year.
        ////[TestCase("2003-T1", TimeFormatEnumType.ThirdOfYear)]
        ////[TestCase("2003-T2", TimeFormatEnumType.ThirdOfYear)]
        ////[TestCase("2003-T3", TimeFormatEnumType.ThirdOfYear)]
        [TestCase("2003-01-02", TimeFormatEnumType.Date)]
        [TestCase("2003-01-01", TimeFormatEnumType.Date)]
        [TestCase("2012-01-01", TimeFormatEnumType.Date)]
        [TestCase("2012-03-01", TimeFormatEnumType.Date)]
        [TestCase("2003-03-01", TimeFormatEnumType.Date)]
        [TestCase("2012-02-29", TimeFormatEnumType.Date)]
        [TestCase("2002-02-28", TimeFormatEnumType.Date)]
        [TestCase("2003-12-31", TimeFormatEnumType.Date)]
        [TestCase("2012-12-31", TimeFormatEnumType.Date)]
        [TestCase("2012-12-31T23:59:59", TimeFormatEnumType.Hour)]
        [TestCase("2012-12-31T00:00:00", TimeFormatEnumType.Hour)]
        [TestCase("2012-01-01T00:00:00", TimeFormatEnumType.Hour)]
        [TestCase("2012-01-01T13:00:00", TimeFormatEnumType.Hour)]
        [TestCase("2012-01-01T13:00:59", TimeFormatEnumType.Hour)]
        [TestCase("1920-01", TimeFormatEnumType.Month)]
        public void TestGetTimeFormatOfDate(string dateStr, TimeFormatEnumType timeFormat)
        {
            var timeFormatOfDate = DateUtil.GetTimeFormatOfDate(dateStr);
            Assert.NotNull(timeFormatOfDate);
            Assert.AreEqual(timeFormat, timeFormatOfDate.EnumType);
        }
    }
}
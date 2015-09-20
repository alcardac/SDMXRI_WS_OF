// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EDIDateUtil.cs" company="Eurostat">
//   Date Created : 2014-07-24
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI date utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Util
{
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     The EDI date utilities.
    /// </summary>
    public static class EDIDateUtil
    {
        #region Constants

        /// <summary>
        /// The half year.
        /// </summary>
        public const string HalfYear = YearLong + "[1-2]";

        /// <summary>
        /// The quarterly.
        /// </summary>
        public const string Quaterly = YearLong + "[1-4]";

        /// <summary>
        /// The hour minute.
        /// </summary>
        private const string HourMinute = "([0-1][0-9]|2[0-3])([0-5][0-9])";

        /// <summary>
        /// The monthly.
        /// </summary>
        private const string Monthly = YearLong + "(0[1-9]|1[1-2])";

        /// <summary>
        /// The weekly.
        /// </summary>
        private const string Weekly = YearLong + "[1-53]";

        /// <summary>
        /// The year long.
        /// </summary>
        private const string YearLong = "[0-9][0-9][0-9][0-9]";

        /// <summary>
        /// The year short.
        /// </summary>
        private const string YearShort = "[0-9][0-9]";

        /// <summary>
        /// The month day.
        /// </summary>
        private const string MonthDay = "((01|03|05|07|08|10|12)((0[1-9])|(1[0-9])|(2[0-9])|3[0-1])" + "|02-((0[1-9])|(1[0-9])|(2[0-9]))" + "|(04|06|09|11)((0[1-9])|(1[0-9])|(2[0-9])|30))";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _date format daily long year.
        /// </summary>
        private static readonly DateFormat _dateFormatDailyLongYear = DateUtil.GetDateFormatter("yyyyMMdd");

        /// <summary>
        /// The _date format daily short year.
        /// </summary>
        private static readonly DateFormat _dateFormatDailyShortYear = DateUtil.GetDateFormatter("yyMMdd");

        /// <summary>
        /// The _date format minute long year.
        /// </summary>
        private static readonly DateFormat _dateFormatMinuteLongYear = DateUtil.GetDateFormatter("yyyyMMddHHmm");

        /// <summary>
        /// The _date format minute short year.
        /// </summary>
        private static readonly DateFormat _dateFormatMinuteShortYear = DateUtil.GetDateFormatter("yyMMddHHmm");

        /// <summary>
        /// The _date format monthly.
        /// </summary>
        private static readonly DateFormat _dateFormatMonthly = DateUtil.GetDateFormatter("yyyyMM");

        /// <summary>
        /// The _date format weekly.
        /// </summary>
        private static readonly DateFormat _dateFormatWeekly = DateUtil.GetDateFormatter("yyyyww");

        /// <summary>
        /// The _date format yearly.
        /// </summary>
        private static readonly DateFormat _dateFormatYearly = DateUtil.GetDateFormatter("yyyy");

        /// <summary>
        /// The _pattern daily long year.
        /// </summary>
        private static readonly Regex _patternDailyLongYear = new Regex(YearLong + MonthDay, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern daily short year.
        /// </summary>
        private static readonly Regex _patternDailyShortYear = new Regex(YearShort + MonthDay, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern half year.
        /// </summary>
        private static readonly Regex _patternHalfYear = new Regex(HalfYear, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern minute long year.
        /// </summary>
        private static readonly Regex _patternMinuteLongYear = new Regex(YearLong + MonthDay + HourMinute, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern minute short year.
        /// </summary>
        private static readonly Regex _patternMinuteShortYear = new Regex(YearShort + MonthDay + HourMinute, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern monthly.
        /// </summary>
        private static readonly Regex _patternMonthly = new Regex(Monthly, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern quarterly.
        /// </summary>
        private static readonly Regex _patternQuaterly = new Regex(Quaterly, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern weekly.
        /// </summary>
        private static readonly Regex _patternWeekly = new Regex(Weekly, RegexOptions.Compiled);

        /// <summary>
        /// The _pattern yearly.
        /// </summary>
        private static readonly Regex _patternYearly = new Regex(YearLong, RegexOptions.Compiled);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the date format daily long year.
        /// </summary>
        public static DateFormat DateFormatDailyLongYear
        {
            get
            {
                return _dateFormatDailyLongYear;
            }
        }

        /// <summary>
        /// Gets the date format daily short year.
        /// </summary>
        public static DateFormat DateFormatDailyShortYear
        {
            get
            {
                return _dateFormatDailyShortYear;
            }
        }

        /// <summary>
        /// Gets the date format minute long year.
        /// </summary>
        public static DateFormat DateFormatMinuteLongYear
        {
            get
            {
                return _dateFormatMinuteLongYear;
            }
        }

        /// <summary>
        /// Gets the date format minute short year.
        /// </summary>
        public static DateFormat DateFormatMinuteShortYear
        {
            get
            {
                return _dateFormatMinuteShortYear;
            }
        }

        /// <summary>
        /// Gets the date format monthly.
        /// </summary>
        public static DateFormat DateFormatMonthly
        {
            get
            {
                return _dateFormatMonthly;
            }
        }

        /// <summary>
        /// Gets the date format weekly.
        /// </summary>
        public static DateFormat DateFormatWeekly
        {
            get
            {
                return _dateFormatWeekly;
            }
        }

        /// <summary>
        /// Gets the date format yearly.
        /// </summary>
        public static DateFormat DateFormatYearly
        {
            get
            {
                return _dateFormatYearly;
            }
        }

        /// <summary>
        /// Gets the pattern daily long year.
        /// </summary>
        public static Regex PatternDailyLongYear
        {
            get
            {
                return _patternDailyLongYear;
            }
        }

        /// <summary>
        /// Gets the pattern daily short year.
        /// </summary>
        public static Regex PatternDailyShortYear
        {
            get
            {
                return _patternDailyShortYear;
            }
        }

        /// <summary>
        /// Gets the pattern half year.
        /// </summary>
        public static Regex PatternHalfYear
        {
            get
            {
                return _patternHalfYear;
            }
        }

        /// <summary>
        /// Gets the pattern minute long year.
        /// </summary>
        public static Regex PatternMinuteLongYear
        {
            get
            {
                return _patternMinuteLongYear;
            }
        }

        /// <summary>
        /// Gets the pattern minute short year.
        /// </summary>
        public static Regex PatternMinuteShortYear
        {
            get
            {
                return _patternMinuteShortYear;
            }
        }

        /// <summary>
        /// Gets the pattern monthly.
        /// </summary>
        public static Regex PatternMonthly
        {
            get
            {
                return _patternMonthly;
            }
        }

        /// <summary>
        /// Gets the pattern quarterly.
        /// </summary>
        public static Regex PatternQuaterly
        {
            get
            {
                return _patternQuaterly;
            }
        }

        /// <summary>
        /// Gets the pattern weekly.
        /// </summary>
        public static Regex PatternWeekly
        {
            get
            {
                return _patternWeekly;
            }
        }

        /// <summary>
        /// Gets the pattern yearly.
        /// </summary>
        public static Regex PatternYearly
        {
            get
            {
                return _patternYearly;
            }
        }

        #endregion
    }
}
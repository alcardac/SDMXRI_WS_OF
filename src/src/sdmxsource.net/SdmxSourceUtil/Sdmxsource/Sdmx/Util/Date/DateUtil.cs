// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion


    /*
     * Class provided to perform date operations 
     */
    //TODO: Timezone + UTC
    /// <summary>
    /// The date util.
    /// </summary>
    public class DateUtil
    {
        // Regular expressions 
        #region Constants

        /// <summary>
        /// The xml date pattern string.
        /// </summary>
        private const string XMLDatePatternString =
            "^[0-9][0-9][0-9][0-9]-" + "((01|03|05|07|08|10|12)-((0[1-9])|(1[0-9])|(2[0-9])|3[0-1])"
            + "|02-((0[1-9])|(1[0-9])|(2[0-9]))" + "|(04|06|09|11)-((0[1-9])|(1[0-9])|(2[0-9])|30))";

        private const string XMLHourPatternString = "([0-1][0-9]|2[0-3])";

        /// <summary>
        /// The xml time pattern string.
        /// </summary>
        private const string XMLTimePatternString = XMLHourPatternString + ":([0-5][0-9])(:[0-5][0-9])?(.[0-9]*)?(Z|((\\+|-)([0-1][0-9]|2[0-3]):([0-5][0-9])))?";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _xml daily pattern.
        /// </summary>
        private static readonly Regex _xmlDailyPattern = new Regex(XMLDatePatternString + "$", RegexOptions.Compiled);

        /// <summary>
        /// The xml hourly pattern.
        /// </summary>
        private static readonly Regex _xmlHourlyPattern =
            new Regex(XMLDatePatternString + "T" + XMLHourPatternString + "$", RegexOptions.Compiled);

        /// <summary>
        /// The xml minutely pattern.
        /// </summary>
        private static readonly Regex _xmlMinutelyPattern =
            new Regex(XMLDatePatternString + "T" + XMLTimePatternString + "$", RegexOptions.Compiled);

        /// <summary>
        /// The _xml monthly pattern.
        /// </summary>
        private static readonly Regex _xmlMonthlyPattern = new Regex(
            "^[0-9][0-9][0-9][0-9]-(0[1-9]|1[0-2])$", RegexOptions.Compiled);

        /// <summary>
        /// The _xml quarterly pattern.
        /// </summary>
        private static readonly Regex _xmlQuarterlyPattern = new Regex(
            "^[0-9][0-9][0-9][0-9]-Q[1-4]$", RegexOptions.Compiled);

        /// <summary>
        /// The _xml semi annual pattern.
        /// </summary>
        private static readonly Regex _xmlSemiAnnualPattern = new Regex(
            "^[0-9][0-9][0-9][0-9]-B[1-2]$", RegexOptions.Compiled);

        /// <summary>
        /// The _xml weekly pattern.
        /// </summary>
        private static readonly Regex _xmlWeeklyPattern = new Regex(
            "^[0-9][0-9][0-9][0-9]-W([1-9]|[1-4][0-9]|5[0-3])$", RegexOptions.Compiled);

        /// <summary>
        /// The _xml yearly pattern.
        /// </summary>
        private static readonly Regex _xmlYearlyPattern = new Regex("^[0-9][0-9][0-9][0-9]$", RegexOptions.Compiled);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the date format.
        /// </summary>
        public static string DateFormat
        {
            get
            {
                return Daily.SdmxDateTimeFormat;
            }
        }

        /// <summary>
        /// Gets a DateFormat object that can be used to format a date 
        /// string as a date object. The hour only format: "yyyy-MM-dd'T'HH" 
        /// <p />
        /// For a String to be successfully formatted as a Date using the 
        /// returned DateFormat object, it must conform to the ISO8601 Standard Date/Time format.
        /// </summary>
        /// <value> DateFormat object </value>
        public static string HourDateTimeFormat
        {
            get
            {
                // ISO8601 Standard Date/Time format
                return Hourly.SdmxHourDateTimeFormat;
            }
        }

        /// <summary>
        /// Gets a DateFormat object that can be used to format a date 
        /// string as a date object. The hour:minute format: "yyyy-MM-dd'T'HH:mm"
        /// <p />
        /// For a String to be successfully formatted as a Date using the 
        /// returned DateFormat object, it must conform to the ISO8601 Standard Date/Time format.
        /// </summary>
        /// <value> DateFormat object </value>
        public static string HourMinuteDateTimeFormat
        {
            get
            {
                // ISO8601 Standard Date/Time format
                return Hourly.SdmxHourMinuteDateTimeFormat;
            }
        }

        /// <summary>
        /// Gets a DateFormat object that can be used to format a date 
        /// string as a date object.  
        /// <p />
        /// For a String to be successfully formatted as a Date using the 
        /// returned DateFormat object, it must conform to the ISO8601 Standard Date/Time format.
        /// </summary>
        /// <value> DateFormat object </value>
        public static string DateTimeFormat
        {
            get
            {
                // ISO8601 Standard Date/Time format
                return Hourly.SdmxDateTimeFormat;
            }
        }
        /// <summary>
        /// Gets the date time string now.
        /// </summary>
        public static string DateTimeStringNow
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.SSS");
            }
        }

        /// <summary>
        /// Gets the month format.
        /// </summary>
        public static string MonthFormat
        {
            get
            {
                return Monthly.SdmxDateTimeFormat;
            }
        }

        /// <summary>
        /// Gets the year format.
        /// </summary>
        public static string YearFormat
        {
            get
            {
                return Annual.SdmxDateTimeFormat;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create calendar.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="Calendar"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">Throws NotImplementedException
        /// </exception>
        public static Calendar CreateCalendar(DateTime date)
        {
            throw new NotImplementedException(
                "Java calendar is not the same as .NET calendar. We might not need to implement this.");

            ////Calendar cal = CultureInfo.InvariantCulture.Calendar;
            // . cal.setTime(date); // JAVA
            ////return cal;
        }

        /// <summary>
        /// Creates a list of all the date values between the from and to dates.  The dates are iterated by the 
        ///  time format.  The format of the dates is also dependant on the time format.
        /// </summary>
        /// <param name="dateFrom">Date From
        /// </param>
        /// <param name="dateTo">Date To
        /// </param>
        /// <param name="format">Date Format
        /// </param>
        /// <returns>
        /// The <see cref="IList{String}"/>.
        /// </returns>
        public static IList<string> CreateTimeValues(DateTime dateFrom, DateTime dateTo, TimeFormat format)
        {
            IList<string> returnList = new List<string>();

            switch (format.EnumType)
            {
                case TimeFormatEnumType.DateTime:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, format);

                // FUNC Support iteration of date time    
                case TimeFormatEnumType.Date:
                case TimeFormatEnumType.HalfOfYear:

                case TimeFormatEnumType.Hour:

                case TimeFormatEnumType.Month:

                case TimeFormatEnumType.QuarterOfYear:

                case TimeFormatEnumType.ThirdOfYear:

                case TimeFormatEnumType.Week:

                case TimeFormatEnumType.Year:
                    IterateDateValues(dateFrom, dateTo, returnList, format.EnumType);
                    break;

                default:
                    throw new Exception("Unsupported time format : " + format);
            }

            return returnList;
        }

        /* No uses found */
        /*
        public static XMLGregorianCalendar CreateXMLGregorianCalendar(DateTime date) {
            if (date == null) {
                return null;
            }
            try {
                Calendar now = ILOG.J2CsMapping.Util.Calendar.GetInstance();
                now.SetTime(date);
                GregorianCalendar cal = new GregorianCalendar(
                        now.Get(ILOG.J2CsMapping.Util.Calendar.YEAR), now.Get(ILOG.J2CsMapping.Util.Calendar.MONTH),
                        now.Get(ILOG.J2CsMapping.Util.Calendar.DATE), now.Get(ILOG.J2CsMapping.Util.Calendar.HOUR),
                        now.Get(ILOG.J2CsMapping.Util.Calendar.MINUTE), now.Get(ILOG.J2CsMapping.Util.Calendar.SECOND));
                return Javax.Xml.Datatype.DatatypeFactory.NewInstance().NewXMLGregorianCalendar(cal);
            } catch (Exception th) {
                throw new Exception(th.Message, th);
            }
        }
    */

        /// <summary>
        /// Formats a String as a date.
        /// <p/>
        /// The expected string must be in either of the two following formats.
        /// <ol>
        ///   <li>yyyy-MM-dd'T'HH:mm:ss.SSSz</li>
        ///   <li>yyyy-MM-dd</li>
        /// </ol>
        /// </summary>
        /// <param name="dateObject">Date object
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dateObject"/>
        /// is null
        /// </exception>
        /// <returns>
        /// The <see cref="DateTime"/> 
        /// </returns>
        public static DateTime FormatDate(object dateObject)
        {
            return FormatDate(dateObject, false);
        }

        /// <summary>
        /// Formats a String as a date.
        /// <p/>
        /// The expected string must be in either of the two following formats.
        /// <ol>
        ///   <li>yyyy-MM-dd'T'HH:mm:ss.SSSz</li>
        ///   <li>yyyy-MM-dd</li>
        /// </ol>
        /// </summary>
        /// <param name="dateObject">Date object
        /// </param>
        /// <param name="startOfPeriod">
        /// The start Of Period. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dateObject"/>
        /// is null
        /// </exception>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FormatDate(object dateObject, bool startOfPeriod)
        {
            if (dateObject == null)
            {
                throw new ArgumentNullException("dateObject");
            }

            if (dateObject is DateTime)
            {
                return (DateTime)dateObject;
            }

            /* TODO Those appear to be Java specific with no .NET equivelant */
            ////if (dateObject  is  XmlCalendar) {
            ////  return FormatDate(dateObject.ToString());
            ////}
            ////if (dateObject  is  XMLGregorianCalendar) {
            ////  XMLGregorianCalendar gregorianCal = (XMLGregorianCalendar) dateObject;
            ////  Calendar cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.YEAR, gregorianCal.GetYear());
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.MONTH, gregorianCal.GetMonth() - 1);
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.DATE, gregorianCal.GetDay());
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.AM_PM, 0);
            ////  if (gregorianCal.GetHour() > 0)
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.HOUR, gregorianCal.GetHour());
            ////  else
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.HOUR, 0);

            ////  if (gregorianCal.GetMinute() > 0)
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.MINUTE, gregorianCal.GetMinute());
            ////  else
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.MINUTE, 0);
            ////  if (gregorianCal.GetSecond() > 0)
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.SECOND, gregorianCal.GetSecond());
            ////  else
            ////      cal.Set(ILOG.J2CsMapping.Util.Calendar.SECOND, 0);
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.MILLISECOND, 0);

            ////  return cal.GetTime();
            ////}
            var dateString = dateObject as string;

            if (dateString == null)
            {
                throw new ArgumentException("Date type not recognised : " + dateObject.GetType().FullName);
            }

            if (dateString.Length == 0)
            {
                return DateTime.MinValue;
            }

            return FormatDate(dateString, startOfPeriod);
        }

        /// <summary>
        /// The format date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatDate(DateTime dt)
        {
            return dt.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return an SDMX-ML formatted time period, based on the frequency supplied. The value
        ///  returned from this method will be determined based on the frequency and the date value
        ///  supplied as follows:
        ///  Annual: The year of the date
        ///  Biannual: January 1 - June 30 = 1st half, July 1 - December 31 = 2nd half
        ///  Triannual: January 1 - April 30 = 1st third, May 1 - August 30 = 2nd third, September 1 - December 31 = 3rd third
        ///  Quarterly: January 1 - March 31 = 1st quarter, April 1 - June 30 = 2nd quarter, July 1 - September 30 = 3rd quarter, October 1 - December 31 = 4th quarter
        ///  Mothly: The month of the date
        ///  Weekly: The week of the year according to ISO 8601.
        ///  Daily: The date only
        ///  Hourly: The full date time representation.
        /// </summary>
        /// <param name="date">
        /// the date to convert 
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The date in SDMX-ML <c>TimePeriodType</c> format. 
        /// </returns>
        public static string FormatDate(DateTime date, TimeFormatEnumType format)
        {
            ////string df = null;
            ////string formatted = null;
            ////Calendar cal;
            IPeriodicity periodicity = PeriodicityFactory.Create(format);
            return periodicity.ToString(date);

            ////switch (format) {
            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.Date:
            ////  df = DateFormat;
            ////      return date.ToString(df, CultureInfo.InvariantCulture);

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.DateTime:
            ////  df = DateTimeFormat;
            ////  return date.ToString(df, CultureInfo.InvariantCulture);

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.Year:
            ////  df = YearFormat;
            ////  return date.ToString(df, CultureInfo.InvariantCulture);
            ////      // what is the point here ?
            ////  ////formatted = df.Format(date);
            ////  ////cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  ////cal.SetTime(date);
            ////  ////return formatted;

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.HalfOfYear:

            ////  df = YearFormat;
            ////  formatted = df.Format(date);
            ////  cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.SetTime(date);
            ////  if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 6) {
            ////      formatted += "-B1";
            ////  } else {
            ////      formatted += "-B2";
            ////  }
            ////  return formatted;

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.HOUR:
            ////  df = DateTimeFormat;
            ////  return df.Format(date);

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.MONTH:
            ////  df = MonthFormat;
            ////  return df.Format(date);

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.QUARTER_OF_YEAR:
            ////  df = YearFormat;
            ////  formatted = df.Format(date);
            ////  cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.SetTime(date);
            ////  if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 2) {
            ////      formatted += "-Q1";
            ////  } else if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 5) {
            ////      formatted += "-Q2";
            ////  } else if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 8) {
            ////      formatted += "-Q3";
            ////  } else {
            ////      formatted += "-Q4";
            ////  }
            ////  return formatted;

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.THIRD_OF_YEAR:
            ////  df = YearFormat;
            ////  formatted = df.Format(date);
            ////  cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.SetTime(date);
            ////  if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 3) {
            ////      formatted += "-T1";
            ////  } else if (cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH) <= 7) {
            ////      formatted += "-T2";
            ////  } else {
            ////      formatted += "-T3";
            ////  }
            ////  return formatted;

            ////  case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.WEEK:
            ////  cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.SetTime(date);
            ////  int weekNum = cal.Get(ILOG.J2CsMapping.Util.Calendar.WEEK_OF_YEAR);
            ////  int year = cal.Get(ILOG.J2CsMapping.Util.Calendar.YEAR);
            ////  int month = cal.Get(ILOG.J2CsMapping.Util.Calendar.MONTH);

            ////  //Due to a problem with the Java Calendar the year is wrong when parsing a date which corresponds to the 53rd week in a 53 week year
            ////  //Such as 2004-53, if the week is the 53rd but the month is Jan, then deduct one from the year to fix the problem
            ////  if (weekNum == 53 && month == 0) {
            ////      year--;
            ////  }
            ////  formatted = year + "-W" + weekNum;

            ////  return formatted;
            ////}
            ////return null;
        }

        /// <summary>
        /// Validates the date is in SDMX date/time format, and returns the Time Format for the date
        /// </summary>
        /// <param name="dateStr">Date string
        /// </param>
        /// <returns>
        /// The <see cref="TimeFormat"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// if the data format is invalid, with regards to the allowed SDMX date formats
        /// </exception>
        public static TimeFormat GetTimeFormatOfDate(string dateStr)
        {
            if (dateStr == null)
            {
                throw new ArgumentException("Could not determine date format, date null");
            }

            //// TODO Regex will be very slow.
            TimeFormatEnumType returnTimeFormatEnumType;
            if (dateStr.EndsWith("Z"))
            {
                dateStr = dateStr.Substring(0, dateStr.Length - 1);
            }

            if (_xmlYearlyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Year;
            }
            else if (_xmlSemiAnnualPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.HalfOfYear;
            }
            else if (_xmlQuarterlyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.QuarterOfYear;
            }
            else if (_xmlMonthlyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Month;
            }
            else if (_xmlWeeklyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Week;
            }
            else if (_xmlDailyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Date;
            }
            else if (_xmlHourlyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Hour;
            }
            else if (_xmlMinutelyPattern.IsMatch(dateStr))
            {
                returnTimeFormatEnumType = TimeFormatEnumType.Hour;
            }
            else
            {
                throw new SdmxSemmanticException(ExceptionCode.InvalidDateFormat, dateStr);
            }

            return TimeFormat.GetFromEnum(returnTimeFormatEnumType);
        }

        /// <summary>
        /// The week format.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string WeekFormat()
        {
            return Weekly.SdmxDateTimeFormat;
        }

        /// <summary>
        /// Gets the date formatter.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The date format.</returns>
        /// <remarks>Note the <see cref="DateFormat"/> is a very simple port from JDK DateFormat because no such class exists in .NET framework. It only supports Parse and Format.</remarks>
        public static DateFormat GetDateFormatter(string pattern)
        {
            return new DateFormat(pattern);
        }

        /// <summary>
        /// Moves to end of the specified period.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns></returns>
        public static DateTime MoveToEndofPeriod(DateTime date, TimeFormat timeFormat)
        {
            string dateAsString = FormatDate(date, timeFormat);
            return FormatDateEndPeriod(dateAsString);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The format date.
        /// </summary>
        /// <param name="valueRen">
        /// The value ren.
        /// </param>
        /// <param name="startOfPeriod">
        /// The start of period.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        private static DateTime FormatDate(string valueRen, bool startOfPeriod)
        {
            TimeFormat timeFormat = GetTimeFormatOfDate(valueRen);
            return PeriodicityFactory.Create(timeFormat.EnumType).ToDateTime(valueRen, startOfPeriod);

            ////string df = null;
            ////string formatValue = null;
            ////string[] split = null;
            ////int days = 0;
            ////int quarter = 0;
            ////Calendar cal = null;
            ////switch (timeFormat.EnumType) 
            ////{
            ////case TimeFormatEnumType.Date:
            ////  formatValue = value_ren;
            ////  df = "yyyy-MM-dd";
            ////  break;
            ////case TimeFormatEnumType.DateTime:
            ////  break;
            ////case TimeFormatEnumType.HalfOfYear:
            ////      split = value_ren.Split(new[] { "-B" }, StringSplitOptions.RemoveEmptyEntries);
            ////  quarter = int.Parse(split[1]);
            ////  switch (quarter) {
            ////  case 1:
            ////      formatValue = split[0] + "-06-30";
            ////      break;
            ////  case 2:
            ////      formatValue = split[0] + "-12-31";
            ////      break;
            ////  }
            ////  df = "yyyy-MM-dd";
            ////  break;
            ////case TimeFormatEnumType.Hour:
            ////  formatValue = value_ren;
            ////  df = "yyyy-MM-dd'T'HH:mm:ss";//.SSSz
            ////  break;
            ////case TimeFormatEnumType.Month:
            ////  split = value_ren.Split(new [] { "-" }, StringSplitOptions.RemoveEmptyEntries); 
            ////  days = DateTime.DaysInMonth(int.Parse(split[0]), int.Parse(split[1]));
            ////  formatValue = value_ren + "-" + days;
            ////  df = "yyyy-MM-dd";
            ////  break;
            ////case TimeFormatEnumType.QuarterOfYear:
            ////  split = value_ren.Split(new[] {"-Q"}, StringSplitOptions.RemoveEmptyEntries);
            ////  quarter = int.Parse(split[1]);
            ////  switch (quarter) {
            ////  case 1:
            ////      formatValue = split[0] + "-03-31";
            ////      break;
            ////  case 2:
            ////      formatValue = split[0] + "-06-30";
            ////      break;
            ////  case 3:
            ////      formatValue = split[0] + "-09-30";
            ////      break;
            ////  case 4:
            ////      formatValue = split[0] + "-12-31";
            ////      break;
            ////  }
            ////  df = "yyyy-MM-dd";
            ////  break;
            ////case TimeFormatEnumType.ThirdOfYear:
            ////  break;
            ////case Org.Sdmxsource.Sdmx.Api.Constants.TimeFormatEnumType.Week:

            ////  split = ILOG.J2CsMapping.Text.RegExUtil.Split(value_ren, "-W");
            ////  cal = ILOG.J2CsMapping.Util.Calendar.GetInstance();
            ////  cal.Clear();
            ////  cal.SetMinimalDaysInFirstWeek(4);
            ////  cal.SetFirstDayOfWeek(ILOG.J2CsMapping.Util.Calendar.MONDAY);
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.DAY_OF_WEEK, ILOG.J2CsMapping.Util.Calendar.SUNDAY);
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.YEAR, int.Parse(split[0]));
            ////  cal.Set(ILOG.J2CsMapping.Util.Calendar.WEEK_OF_YEAR, int.Parse(split[1]));
            ////  return cal.GetTime();
            ////case Org.Sdmxsource.Sdmx.Api.Constants.TIME_FORMAT.YEAR:
            ////  formatValue = value_ren + "-12-31";
            ////  df = new SimpleDateFormat("yyyy-MM-dd");
            ////  break;
            ////default:
            ////  throw new SdmxNotImplementedException(Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.UNSUPPORTED,
            ////          "formatting date of type " + timeFormat);
            ////}

            ////try {
            ////  return df.Parse(formatValue);
            ////} catch (ParseException e) {
            ////  throw new ArgumentException(e);
            ////}
        }

        /// <summary>
        /// The iterate date values.
        /// </summary>
        /// <param name="dateFrom">
        /// The date from.
        /// </param>
        /// <param name="dateTo">
        /// The date to.
        /// </param>
        /// <param name="returnList">
        /// The return list.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        private static void IterateDateValues(
            DateTime dateFrom, DateTime dateTo, ICollection<string> returnList, TimeFormatEnumType format)
        {
            IPeriodicity periodicity = PeriodicityFactory.Create(format);

            DateTime pointer = dateFrom;
            while ((pointer.Ticks / 10000) <= (dateTo.Ticks / 10000))
            {
                returnList.Add(FormatDate(pointer, format));
                pointer = periodicity.AddPeriod(pointer);
            }
        }

        #endregion

        /// <summary>
        /// Formats the date end period.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The end period</returns>
        private static DateTime FormatDateEndPeriod(string value)
        {
            TimeFormat timeFormat = GetTimeFormatOfDate(value);
            IPeriodicity periodicity = PeriodicityFactory.Create(timeFormat);
            return periodicity.ToDateTime(value, false);
        }
    }
}
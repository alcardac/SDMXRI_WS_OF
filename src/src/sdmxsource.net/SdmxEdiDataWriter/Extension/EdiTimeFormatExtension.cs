// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiTimeFormatExtension.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI time format extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Extension
{
    using System;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.EdiParser.Util;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     The EDI time format extension.
    /// </summary>
    public static class EdiTimeFormatExtension
    {
        #region Static Fields

        /// <summary>
        /// The _calendar.
        /// </summary>
        private static readonly Calendar _calendar;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="EdiTimeFormatExtension"/> class.
        /// </summary>
        static EdiTimeFormatExtension()
        {
            _calendar = CultureInfo.InvariantCulture.Calendar;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The EDI time format.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// Not implemented for value supplied in <paramref name="ediTimeFormat"/>
        /// </exception>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatDate(this EdiTimeFormat ediTimeFormat, DateTime date)
        {
            string formatted;

            var timeFormat = ConvertToSingleTimeFormat(ediTimeFormat);

            switch (timeFormat)
            {
                case EdiTimeFormat.DailyFourDigYear:
                    return EDIDateUtil.DateFormatDailyLongYear.Format(date);

                case EdiTimeFormat.DailyTwoDigYear:
                    return EDIDateUtil.DateFormatDailyShortYear.Format(date);

                case EdiTimeFormat.HalfOfYear:
                    formatted = EDIDateUtil.DateFormatYearly.Format(date);

                    if (date.Month <= 6)
                    {
                        formatted += "1";
                    }
                    else
                    {
                        formatted += "2";
                    }

                    return formatted;

                case EdiTimeFormat.MinuteFourDigYear:
                    return EDIDateUtil.DateFormatMinuteLongYear.Format(date);

                case EdiTimeFormat.MinuteTwoDigYear:
                    return EDIDateUtil.DateFormatMinuteShortYear.Format(date);

                case EdiTimeFormat.Month:
                    return EDIDateUtil.DateFormatMonthly.Format(date);

                case EdiTimeFormat.Week:

                    // See http://blogs.msdn.com/b/shawnste/archive/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net.aspx
                    DateTime time = date;
                    DayOfWeek day = _calendar.GetDayOfWeek(time);
                    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                    {
                        time = time.AddDays(3);
                    }

                    int weekNum = _calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                    int year = _calendar.GetYear(time) - ((weekNum >= 52) && (time.Month == 1) ? -1 : 0);

                    // The Edi Format is to always have a two digit week value, so append 0 to the week if required
                    return string.Format(CultureInfo.InvariantCulture, "{0:0000}{1:00}", year, weekNum);

                case EdiTimeFormat.Year:
                    return EDIDateUtil.DateFormatYearly.Format(date);

                case EdiTimeFormat.QuarterOfYear:
                    formatted = EDIDateUtil.DateFormatYearly.Format(date);

                    if (date.Month <= 2)
                    {
                        formatted += "1";
                    }
                    else if (date.Month <= 5)
                    {
                        formatted += "2";
                    }
                    else if (date.Month <= 8)
                    {
                        formatted += "3";
                    }
                    else
                    {
                        formatted += "4";
                    }

                    return formatted;

                default:
                    throw new SdmxNotImplementedException("Edi date format : " + ediTimeFormat);
            }
        }

        /// <summary>
        /// Gets the EDI value.
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The EDI time format.
        /// </param>
        /// <returns>
        /// The EDI value as text.
        /// </returns>
        public static string GetEdiValue(this EdiTimeFormat ediTimeFormat)
        {
            return ediTimeFormat.ToString("D");
        }

        /// <summary>
        /// Gets the expected length.
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The EDI time format.
        /// </param>
        /// <returns>
        /// the expected length.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Edi date format : <paramref name="ediTimeFormat"/>
        /// </exception>
        public static int GetExpectedLength(this EdiTimeFormat ediTimeFormat)
        {
            int dateLength;
            switch (ediTimeFormat)
            {
                case EdiTimeFormat.RangeDaily:
                    dateLength = 16;
                    break;
                case EdiTimeFormat.RangeHalfOfYear:
                    dateLength = 10;
                    break;
                case EdiTimeFormat.RangeMonthly:
                    dateLength = 12;
                    break;
                case EdiTimeFormat.RangeQuarterOfYear:
                    dateLength = 10;
                    break;
                case EdiTimeFormat.RangeWeekly:
                    dateLength = 12;
                    break;
                case EdiTimeFormat.RangeYear:
                    dateLength = 8;
                    break;
                case EdiTimeFormat.MinuteTwoDigYear:
                    dateLength = 10;
                    break;
                case EdiTimeFormat.MinuteFourDigYear:
                    dateLength = 12;
                    break;
                case EdiTimeFormat.DailyFourDigYear:
                    dateLength = 8;
                    break;
                case EdiTimeFormat.DailyTwoDigYear:
                    dateLength = 6;
                    break;
                case EdiTimeFormat.HalfOfYear:
                    dateLength = 5;
                    break;
                case EdiTimeFormat.Month:
                    dateLength = 6;
                    break;
                case EdiTimeFormat.QuarterOfYear:
                    dateLength = 5;
                    break;
                case EdiTimeFormat.Week:
                    dateLength = 6;
                    break;
                case EdiTimeFormat.Year:
                    dateLength = 4;
                    break;
                default:
                    throw new SdmxNotImplementedException("Edi date format : " + ediTimeFormat.ToString("D"));
            }

            return dateLength;
        }

        /// <summary>
        /// Gets the SDMX time format.
        /// </summary>
        /// <param name="ediFormat">
        /// The EDI time format.
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// Cannot convert EDI time format <paramref name="ediFormat"/> to SDMX
        /// </exception>
        /// <returns>
        /// The <see cref="TimeFormat"/>.
        /// </returns>
        public static TimeFormat GetSdmxTimeFormat(this EdiTimeFormat ediFormat)
        {
            TimeFormatEnumType timeFormat;
            switch (ediFormat)
            {
                case EdiTimeFormat.Year:
                case EdiTimeFormat.RangeYear:
                    timeFormat = TimeFormatEnumType.Year;
                    break;
                case EdiTimeFormat.Month:
                case EdiTimeFormat.RangeMonthly:
                    timeFormat = TimeFormatEnumType.Month;
                    break;
                case EdiTimeFormat.QuarterOfYear:
                case EdiTimeFormat.RangeQuarterOfYear:

                    timeFormat = TimeFormatEnumType.QuarterOfYear;
                    break;
                case EdiTimeFormat.HalfOfYear:
                case EdiTimeFormat.RangeHalfOfYear:
                    timeFormat = TimeFormatEnumType.HalfOfYear;
                    break;
                case EdiTimeFormat.Week:
                case EdiTimeFormat.RangeWeekly:
                    timeFormat = TimeFormatEnumType.Week;
                    break;
                case EdiTimeFormat.DailyFourDigYear:
                case EdiTimeFormat.DailyTwoDigYear:
                case EdiTimeFormat.RangeDaily:
                    timeFormat = TimeFormatEnumType.Date;
                    break;
                case EdiTimeFormat.MinuteFourDigYear:
                case EdiTimeFormat.MinuteTwoDigYear:
                    timeFormat = TimeFormatEnumType.DateTime;
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Cannot convert EDI time format " + ediFormat + "to SDMX");
            }

            return TimeFormat.GetFromEnum(timeFormat);
        }

        /// <summary>
        /// Determines whether the specified <see cref="EdiTimeFormat"/> is a range.
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The EDI time format.
        /// </param>
        /// <returns>
        /// True if the specified <see cref="EdiTimeFormat"/> is a range; otherwise false.
        /// </returns>
        public static bool IsRange(this EdiTimeFormat ediTimeFormat)
        {
            switch (ediTimeFormat)
            {
                case EdiTimeFormat.RangeDaily:
                case EdiTimeFormat.RangeMonthly:
                case EdiTimeFormat.RangeHalfOfYear:
                case EdiTimeFormat.RangeQuarterOfYear:
                case EdiTimeFormat.RangeWeekly:
                case EdiTimeFormat.RangeYear:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the start date of a range, or the only date in a non-range
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The EDI Time Format.
        /// </param>
        /// <param name="dateString">
        /// The date String.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ParseDate(this EdiTimeFormat ediTimeFormat, string dateString)
        {
            bool isRange = ediTimeFormat.IsRange();

            try
            {
                if (isRange)
                {
                    return ParseRange(ediTimeFormat, dateString, 0);
                }

                return ParseDate(dateString, ediTimeFormat);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Could not format date of type '" + ediTimeFormat + "' with date string '" + dateString + "'", e);
            }
        }

        /// <summary>
        /// Parses the end date of a range, -range
        /// </summary>
        /// <param name="timeFormat">
        /// The time Format.
        /// </param>
        /// <param name="dateString">
        /// The date String.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ParseEndDate(this EdiTimeFormat timeFormat, string dateString)
        {
            try
            {
                if (timeFormat.IsRange())
                {
                    return ParseRange(timeFormat, dateString, 1);
                }

                throw new ArgumentException("Attempting to parse non-range date as a range:  '" + dateString + "'");
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Could not format date of type '" + timeFormat + "' with date string '" + dateString + "'", e);
            }
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="ediString">
        /// The EDI string.
        /// </param>
        /// <returns>
        /// The <see cref="EdiTimeFormat"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Unknown time format :  <paramref name="ediString"/>
        /// </exception>
        public static EdiTimeFormat ParseString(string ediString)
        {
            foreach (EdiTimeFormat currentTimeFormat in Enum.GetValues(typeof(EdiTimeFormat)))
            {
                if (currentTimeFormat != EdiTimeFormat.None)
                {
                    if (currentTimeFormat.GetEdiValue().Equals(ediString))
                    {
                        return currentTimeFormat;
                    }
                }
            }

            throw new SdmxSemmanticException("Unknown time format : " + ediString);
        }

        /// <summary>
        /// Parses the time format.
        /// </summary>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <param name="isRange">
        /// if set to <c>true</c> return the ranged <see cref="EdiTimeFormat"/>.
        /// </param>
        /// <returns>
        /// The <see cref="EdiTimeFormat"/>.
        /// </returns>
        public static EdiTimeFormat ParseTimeFormat(this TimeFormat timeFormat, bool isRange)
        {
            IPeriodicity periodicity = PeriodicityFactory.Create(timeFormat);
            return isRange ? periodicity.Gesmes.RangeFormat : periodicity.Gesmes.DateFormat;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The convert to single time format.
        /// </summary>
        /// <param name="ediTimeFormat">
        /// The edi time format.
        /// </param>
        /// <returns>
        /// The <see cref="EdiTimeFormat"/>.
        /// </returns>
        private static EdiTimeFormat ConvertToSingleTimeFormat(EdiTimeFormat ediTimeFormat)
        {
            EdiTimeFormat timeFormat = ediTimeFormat;
            switch (ediTimeFormat)
            {
                case EdiTimeFormat.RangeDaily:
                    timeFormat = EdiTimeFormat.DailyFourDigYear;
                    break;
                case EdiTimeFormat.RangeMonthly:
                    timeFormat = EdiTimeFormat.Month;
                    break;
                case EdiTimeFormat.RangeHalfOfYear:
                    timeFormat = EdiTimeFormat.HalfOfYear;
                    break;
                case EdiTimeFormat.RangeQuarterOfYear:
                    timeFormat = EdiTimeFormat.QuarterOfYear;
                    break;
                case EdiTimeFormat.RangeWeekly:
                    timeFormat = EdiTimeFormat.Week;
                    break;
                case EdiTimeFormat.RangeYear:
                    timeFormat = EdiTimeFormat.Year;
                    break;
            }

            return timeFormat;
        }

        /// <summary>
        /// Parses the date.
        /// </summary>
        /// <param name="ediDateString">The EDI date string.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns>
        /// The <see cref="DateTime" />.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException"> <paramref name="timeFormat"/> value not supported. </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Could not parse date  in <paramref name="ediDateString"/> with EDI time format in <paramref name="timeFormat"/>  and is a range, and is therefore expected to provide a date with X characters  
        /// or
        /// Could not parse date  in <paramref name="ediDateString"/>  with EDI time format in <paramref name="timeFormat"/>' relates to  + timeFormat.GetSdmxTimeFormat().ReadableCode
        ///                     +  data is expected to provide a date with ' + expectedLength + ' characters 
        /// </exception>
        private static DateTime ParseDate(string ediDateString, EdiTimeFormat timeFormat)
        {
            try
            {
                switch (timeFormat)
                {
                    case EdiTimeFormat.DailyFourDigYear:
                        return EDIDateUtil.DateFormatDailyLongYear.Parse(ediDateString);

                    case EdiTimeFormat.DailyTwoDigYear:
                        return EDIDateUtil.DateFormatDailyShortYear.Parse(ediDateString);

                    case EdiTimeFormat.HalfOfYear:
                        return ParseHalfYear(ediDateString);

                    case EdiTimeFormat.MinuteFourDigYear:
                        return EDIDateUtil.DateFormatMinuteLongYear.Parse(ediDateString);

                    case EdiTimeFormat.MinuteTwoDigYear:
                        return EDIDateUtil.DateFormatMinuteShortYear.Parse(ediDateString);

                    case EdiTimeFormat.Month:
                        return EDIDateUtil.DateFormatMonthly.Parse(ediDateString);

                    case EdiTimeFormat.Week:
                        return EDIDateUtil.DateFormatWeekly.Parse(ediDateString);

                    case EdiTimeFormat.Year:
                        return EDIDateUtil.DateFormatYearly.Parse(ediDateString);

                    case EdiTimeFormat.QuarterOfYear:
                        return ParseQuarterYear(ediDateString);

                    default:
                        throw new SdmxNotImplementedException("Edi date format : " + timeFormat);
                }
            }
            catch (FormatException e)
            {
                int expectedLength = timeFormat.GetExpectedLength();
                if (timeFormat.IsRange())
                {
                    throw new SdmxSemmanticException(
                        "Could not parse date '" + ediDateString + "' edi time format '" + timeFormat.GetEdiValue() + "' relates to " + timeFormat.GetSdmxTimeFormat().ReadableCode
                        + " data, and is a range, and is therefore expected to provide a date with '" + expectedLength + "' characters  ", 
                        e);
                }

                throw new SdmxSemmanticException(
                    "Could not parse date '" + ediDateString + "' edi time format '" + timeFormat.GetEdiValue() + "' relates to " + timeFormat.GetSdmxTimeFormat().ReadableCode
                    + " data is expected to provide a date with '" + expectedLength + "' characters ", 
                    e);
            }
        }

        /// <summary>
        /// Parses the half year.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns>
        /// The <see cref="DateTime" />.
        /// </returns>
        private static DateTime ParseHalfYear(string dateString)
        {
            string yearHalf = dateString.Substring(4);
            string year = dateString.Substring(0, 4);
            int half = int.Parse(yearHalf);
            switch (half)
            {
                case 1:
                    year += "0630";
                    break;
                case 2:
                    year += "1231";
                    break;
            }

            return EDIDateUtil.DateFormatDailyLongYear.Parse(year);
        }

        /// <summary>
        /// Parses the quarter year.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns>
        /// The <see cref="DateTime" />.
        /// </returns>
        private static DateTime ParseQuarterYear(string dateString)
        {
            string yearQuater = dateString.Substring(4);
            string year = dateString.Substring(0, 4);
            int quarter = int.Parse(yearQuater);
            switch (quarter)
            {
                case 1:
                    year += "0331";
                    break;
                case 2:
                    year += "0630";
                    break;
                case 3:
                    year += "0930";
                    break;
                case 4:
                    year += "1231";
                    break;
            }

            return EDIDateUtil.DateFormatDailyLongYear.Parse(year);
        }

        /// <summary>
        /// Parses the range.
        /// </summary>
        /// <param name="ediTimeFormat">The EDI time format.</param>
        /// <param name="dateString">The date string.</param>
        /// <param name="range">The range.</param>
        /// <returns>
        /// The <see cref="DateTime" />.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">The <paramref name="ediTimeFormat" /> is not supported.</exception>
        /// <exception cref="SdmxSemmanticException">Time Period not consistent with time format code.</exception>
        private static DateTime ParseRange(EdiTimeFormat ediTimeFormat, string dateString, int range)
        {
            int dateLength;
            EdiTimeFormat tf;
            switch (ediTimeFormat)
            {
                case EdiTimeFormat.RangeDaily:
                    dateLength = 8;
                    tf = EdiTimeFormat.DailyFourDigYear;
                    break;
                case EdiTimeFormat.RangeHalfOfYear:
                    dateLength = 5;
                    tf = EdiTimeFormat.HalfOfYear;
                    break;
                case EdiTimeFormat.RangeMonthly:
                    dateLength = 6;
                    tf = EdiTimeFormat.Month;
                    break;
                case EdiTimeFormat.RangeQuarterOfYear:
                    dateLength = 5;
                    tf = EdiTimeFormat.QuarterOfYear;
                    break;
                case EdiTimeFormat.RangeWeekly:
                    dateLength = 6;
                    tf = EdiTimeFormat.Week;
                    break;
                case EdiTimeFormat.RangeYear:
                    dateLength = 4;
                    tf = EdiTimeFormat.Year;
                    break;
                default:
                    throw new SdmxNotImplementedException("Edi date format : " + ediTimeFormat.ToString("D"));
            }

            // NOTE (.NET). From what I understood here the range can either 1 or 0. When it is 0 we get the first half of the EDI time range(date string), i.e. the start date
            // When it is 1 we get the second half i.e. end date. 
            int startIdx = range * dateLength;
            int endIdx = startIdx + dateLength;

            if (dateString.Length >= endIdx)
            {
                string split = dateString.Substring(startIdx, dateLength);
                return ParseDate(split, tf);
            }

            string errorMessage = string.Format("Time Period not consistent with time format code. Time period \'{0}\'. Time format code \'{1}\'", dateString, ediTimeFormat);
            throw new SdmxSemmanticException(errorMessage);
        }

        #endregion
    }
}
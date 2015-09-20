// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hourly.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    using System;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Daily frequency
    /// </summary>
    public class Hourly : BasePeriodicity, IPeriodicity
    {
        #region Constants

        /// <summary>
        /// The SDMX just hour format
        /// </summary>
        public const string SdmxHourDateTimeFormat = "yyyy-MM-dd'T'HH";

        /// <summary>
        /// The SDMX minutes included format
        /// </summary>
        public const string SdmxHourMinuteDateTimeFormat = "yyyy-MM-dd'T'HH:mm";

        /// <summary>
        /// The SDMX date format
        /// </summary>
        public const string SdmxDateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";

        /// <summary>
        /// When the period digit starts
        /// </summary>
        private const byte DigitStartIndex = 0;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const byte NumberOfMonths = 0;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const short NumberOfPeriods = 8760;

        /// <summary>
        /// The SDMX period only format
        /// </summary>
        private const string SdmxFormat = "00";

        #endregion

        #region Static Fields

        /// <summary>
        /// Gesmes period information
        /// </summary>
        private  readonly GesmesPeriod _gesmesPeriod;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Hourly" /> class from being created.
        /// </summary>
        public Hourly()
            : base(TimeFormatEnumType.Hour)
        {
            this._gesmesPeriod = new GesmesPeriod(this)
                                {
                                    DateFormat = EdiTimeFormat.MinuteFourDigYear,
                                    RangeFormat = EdiTimeFormat.None,
                                    PeriodFormat = SdmxFormat,
                                    PeriodMax = 31 // TODO
                                };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets when the digit starts inside a period time SDMX Time. (For SDMX TIme Period)
        /// e.g. in Quarterly the digit x is second character "Qx" so starting from 0 it is 1
        /// </summary>
        public byte DigitStart
        {
            get
            {
                return DigitStartIndex;
            }
        }

        /// <summary>
        /// Gets the format of the period for using it with int.ToString(string format,NumberFormatInfo) <see cref="System.Int32" />
        /// E.g. for monthly is "00" for quarterly is "\\Q0" (For SDMX TIme Period)
        /// </summary>
        public string Format
        {
            get
            {
                return SdmxFormat;
            }
        }

        /// <summary>
        /// Gets the frequency code
        /// </summary>
        public char FrequencyCode
        {
            get
            {
                return FrequencyConstants.Daily;
            }
        }

        /// <summary>
        /// Gets the <see cref="GesmesPeriod" />
        /// </summary>
        public GesmesPeriod Gesmes
        {
            get
            {
                return _gesmesPeriod;
            }
        }

        /// <summary>
        /// Gets the number of months in a period e.g. 1 for Monthly, 3 for quarterly (For SDMX TIme Period)
        /// </summary>
        public byte MonthsPerPeriod
        {
            get
            {
                return NumberOfMonths;
            }
        }

        /// <summary>
        /// Gets the number of periods in a period e.g. 12 for monthly (For SDMX TIme Period)
        /// </summary>
        public short PeriodCount
        {
            get
            {
                return NumberOfPeriods;
            }
        }

        /// <summary>
        /// Gets the period prefix if any
        /// </summary>
        public char PeriodPrefix
        {
            get
            {
                return FrequencyConstants.Null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add period to the specified <paramref name="dateTime"/> and returns the result.
        /// </summary>
        /// <param name="dateTime">
        /// The date time. 
        /// </param>
        /// <returns>
        /// The result of adding period to the specified <paramref name="dateTime"/> 
        /// </returns>
        public DateTime AddPeriod(DateTime dateTime)
        {
            return Calendar.AddHours(dateTime, 1);
        }

        /// <summary>
        /// Convert an SDMX Time Period type to a System.DateTime object
        /// </summary>
        /// <param name="sdmxPeriod">
        /// A string with the SDMX Time period 
        /// </param>
        /// <param name="start">
        /// Not used in this implementation 
        /// </param>
        /// <returns>
        /// A DateTime object 
        /// </returns>
        public DateTime ToDateTime(string sdmxPeriod, bool start)
        {
            return DateTime.Parse(sdmxPeriod, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert the specified <paramref name="time"/> to SDMX Time Period type representation
        /// </summary>
        /// <param name="time">
        /// The <see cref="DateTime"/> object to convert 
        /// </param>
        /// <param name="format">
        /// The <see cref="string"/> time format to convert to
        /// </param>
        /// <returns>
        /// A string with the SDMX Time Period 
        /// </returns>
        public string ToString(DateTime time, string format)
        {
            switch (format)
            {
                case SdmxHourDateTimeFormat:
                case SdmxHourMinuteDateTimeFormat:
                case SdmxDateTimeFormat:
                    return time.ToString(format, CultureInfo.InvariantCulture);
                default:
                    return time.ToString(SdmxDateTimeFormat, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Convert the specified <paramref name="time"/> to SDMX Time Period type representation
        /// </summary>
        /// <param name="time">
        /// The <see cref="DateTime"/> object to convert 
        /// </param>
        /// <returns>
        /// A string with the SDMX Time Period 
        /// </returns>
        public string ToString(DateTime time)
        {
            return time.ToString(SdmxDateTimeFormat, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
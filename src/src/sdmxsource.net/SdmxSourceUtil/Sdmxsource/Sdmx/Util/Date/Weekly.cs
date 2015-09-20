// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Weekly.cs" company="Eurostat">
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
    /// Weekly frequency
    /// </summary>
    public class Weekly : BasePeriodicity, IPeriodicity
    {
        #region Constants

        /// <summary>
        /// The SDMX date format
        /// </summary>
        public const string SdmxDateTimeFormat = "{0:yyyy}-W{1}";

        /// <summary>
        /// When the period digit starts
        /// </summary>
        private const byte DigitStartIndex = 1;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const byte NumberOfMonths = 0;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const short NumberOfPeriods = 52;

        /// <summary>
        /// The SDMX period only format
        /// </summary>
        private const string SdmxFormat = "\\W0";

        #endregion

        #region Static Fields

        /// <summary>
        /// GESMES period information
        /// </summary>
        private readonly GesmesPeriod _gesmesPeriod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Weekly" /> class
        /// </summary>
        public Weekly()
            : base(TimeFormatEnumType.Week)
        {
            this._gesmesPeriod = new GesmesPeriod(this)
                                {
                                    DateFormat = EdiTimeFormat.Week, PeriodFormat = "0", PeriodMax = NumberOfPeriods + 1 
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
        /// Gets the the frequency code
        /// </summary>
        public char FrequencyCode
        {
            get
            {
                return FrequencyConstants.Weekly;
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
                return this.FrequencyCode;
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
            return Calendar.AddWeeks(dateTime, 1);
        }

        /// <summary>
        /// Convert an SDMX Time Period type to a System.DateTime object
        /// </summary>
        /// <param name="sdmxPeriod">
        /// A string with the SDMX Time period 
        /// </param>
        /// <param name="start">
        /// If it is true it will expand to the start of the period else towards the end e.g. if it is true 2001-04 will become 2001-04-01 else it will become 2001-04-30 
        /// </param>
        /// <returns>
        /// A DateTime object 
        /// </returns>
        public DateTime ToDateTime(string sdmxPeriod, bool start)
        {
            var ret = new DateTime();
            if (!string.IsNullOrEmpty(sdmxPeriod))
            {
                string[] dateFields = sdmxPeriod.Split(new[] { '-' }, 3);
                if (dateFields.Length == 2)
                {
                    int period = Convert.ToInt32(dateFields[1].Substring(1), CultureInfo.InvariantCulture);
                    int year = Convert.ToInt32(dateFields[0].Substring(0, 4), CultureInfo.InvariantCulture);
                    ret = new DateTime(year, 1, 1).AddDays(period * 7);
                }
            }

            return ret;
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
            IFormatProvider fmt = CultureInfo.InvariantCulture;
            return string.Format(
                fmt, SdmxDateTimeFormat, time, Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }

        #endregion
    }
}
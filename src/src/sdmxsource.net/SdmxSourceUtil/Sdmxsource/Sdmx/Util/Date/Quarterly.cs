// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Quarterly.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The quarterly.
    /// </summary>
    public class Quarterly : BasePeriodicity, IPeriodicity
    {
        #region Constants

        /// <summary>
        /// When the period digit starts
        /// </summary>
        private const byte DigitStartIndex = 1;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const byte NumberOfMonths = 3;

        /// <summary>
        /// The number of months in a period
        /// </summary>
        private const short NumberOfPeriods = 4;

        /// <summary>
        /// The SDMX period only format
        /// </summary>
        private const string SdmxFormat = "\\Q0";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _gesmes period.
        /// </summary>
        private  readonly GesmesPeriod _gesmesPeriod;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Quarterly" /> class
        /// </summary>
        public Quarterly()
            : base(TimeFormatEnumType.QuarterOfYear)
        {
            this._gesmesPeriod = new GesmesPeriod(this)
                                {
                                    DateFormat = EdiTimeFormat.QuarterOfYear, 
                                    RangeFormat = EdiTimeFormat.RangeQuarterOfYear, 
                                    PeriodFormat = "0", 
                                    PeriodMax = 4
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
                return FrequencyConstants.Quarterly;
            }
        }

        /// <summary>
        /// Gets the Gesmes Period
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
            return Calendar.AddMonths(dateTime, NumberOfMonths);
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
            return PeriodicityHelper.ConvertToDateTime(sdmxPeriod, this.MonthsPerPeriod, start, this.DigitStart);
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
            return PeriodicityHelper.ConvertToString(time, this.MonthsPerPeriod, this.PeriodPrefix);
        }

        #endregion
    }
}
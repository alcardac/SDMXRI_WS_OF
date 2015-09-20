// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeFormat.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    ///     Defines different time formats supported by SDMX-ML
    /// </summary>
    public enum TimeFormatEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The year.
        /// </summary>
        Year, 

        /// <summary>
        ///     The half of year.
        /// </summary>
        HalfOfYear, 

        /// <summary>
        ///     The third of year.
        /// </summary>
        ThirdOfYear, 

        /// <summary>
        ///     The quarter of year.
        /// </summary>
        QuarterOfYear, 

        /// <summary>
        ///     The month.
        /// </summary>
        Month, 

        /// <summary>
        ///     The week.
        /// </summary>
        Week, 

        /// <summary>
        ///     The date.
        /// </summary>
        Date, 

        /// <summary>
        ///     The hour.
        /// </summary>
        Hour, 

        /// <summary>
        ///     The date time.
        /// </summary>
        DateTime
    }

    /// <summary>
    ///     Defines different time formats supported by SDMX-ML
    /// </summary>
    public class TimeFormat : BaseConstantType<TimeFormatEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<TimeFormatEnumType, TimeFormat> _instances =
            new Dictionary<TimeFormatEnumType, TimeFormat>
                {
                    {
                        TimeFormatEnumType.Year, 
                        new TimeFormat(TimeFormatEnumType.Year, "A", "Yearly")
                    }, 
                    {
                        TimeFormatEnumType.HalfOfYear, 
                        new TimeFormat(
                        TimeFormatEnumType.HalfOfYear, "S", "Half Yearly")
                    }, 
                    {
                        TimeFormatEnumType.ThirdOfYear, 
                        new TimeFormat(
                        TimeFormatEnumType.ThirdOfYear, 
                        "T", 
                        "Trimesterly = new TimeFormat(Third of Year)")
                    }, 
                    {
                        TimeFormatEnumType.QuarterOfYear, 
                        new TimeFormat(
                        TimeFormatEnumType.QuarterOfYear, "Q", "Quarterly")
                    }, 
                    {
                        TimeFormatEnumType.Month, 
                        new TimeFormat(
                        TimeFormatEnumType.Month, "M", "Monthly")
                    }, 
                    {
                        TimeFormatEnumType.Week, 
                        new TimeFormat(TimeFormatEnumType.Week, "W", "Weekly")
                    }, 
                    {
                        TimeFormatEnumType.Date, 
                        new TimeFormat(TimeFormatEnumType.Date, "D", "Daily")
                    }, 
                    {
                        TimeFormatEnumType.Hour, 
                        new TimeFormat(TimeFormatEnumType.Hour, "H", "Hourly")
                    }, 
                    {
                        TimeFormatEnumType.DateTime, 
                        new TimeFormat(
                        TimeFormatEnumType.DateTime, "I", "Date Time")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _frequency code.
        /// </summary>
        private readonly string _frequencyCode;

        /// <summary>
        ///     The _readable code.
        /// </summary>
        private readonly string _readableCode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeFormat"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="frequencyCode">
        /// The frequency code.
        /// </param>
        /// <param name="readableCode">
        /// The readable code.
        /// </param>
        private TimeFormat(TimeFormatEnumType enumType, string frequencyCode, string readableCode)
            : base(enumType)
        {
            this._frequencyCode = frequencyCode;
            this._readableCode = readableCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<TimeFormat> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///     Gets an Id reprentation of this TIME_FORMAT:
        ///     <ul>
        ///         <li>A = TIME_FORMAT.YEAR</li>
        ///         <li>S = TIME_FORMAT.HALF_OF_YEAR</li>
        ///         <li>T = TIME_FORMAT.THIRD_OF_YEAR</li>
        ///         <li>Q = TIME_FORMAT.QUARTER_OF_YEAR</li>
        ///         <li>M = TIME_FORMAT.MONTH</li>
        ///         <li>W = TIME_FORMAT.WEEK</li>
        ///         <li>D = TIME_FORMAT.DATE</li>
        ///         <li>H = TIME_FORMAT.HOUR</li>
        ///         <li>I = TIME_FORMAT.DATE_TIME</li>
        ///     </ul>
        /// </summary>
        public string FrequencyCode
        {
            get
            {
                return this._frequencyCode;
            }
        }

        /// <summary>
        ///     Gets a human readable string representation of this time format
        /// </summary>
        public string ReadableCode
        {
            get
            {
                return this._readableCode;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="TimeFormat"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="TimeFormat"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static TimeFormat GetFromEnum(TimeFormatEnumType enumType)
        {
            TimeFormat output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Gets the time format from the code Id (case sensitive) - the code ids are listed in the getFrequencyCodeId() method
        /// </summary>
        /// <param name="codeId">Code Id </param>
        /// <returns>
        /// The <see cref="TimeFormat"/> .
        /// </returns>
        public static TimeFormat GetTimeFormatFromCodeId(string codeId)
        {
            foreach (TimeFormat currentTimeFormat in Values)
            {
                if (currentTimeFormat.FrequencyCode.Equals(codeId))
                {
                    return currentTimeFormat;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;
            foreach (TimeFormat currentTimeFormat in Values)
            {
                sb.Append(concat + currentTimeFormat.FrequencyCode);
                concat = ",";
            }

            throw new ArgumentException(
                "Time format can not be found for code id : " + codeId + " allowed values are : " + sb);
        }

        #endregion
    }
}
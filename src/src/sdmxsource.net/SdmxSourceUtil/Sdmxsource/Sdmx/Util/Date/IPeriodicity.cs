// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPeriodicity.cs" company="Eurostat">
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
    /// Interface for various frequencies
    /// </summary>
    public interface IPeriodicity
    {
        #region Public Properties

        /// <summary>
        /// Gets when the digit starts inside a period time SDMX Time. (For SDMX Time Period)
        /// e.g. in Quarterly the digit x is second character "Qx" so starting from 0 it is 1
        /// </summary>
        byte DigitStart { get; }

        /// <summary>
        /// Gets the format of the period for using it with int.ToString(string format,NumberFormatInfo) <see cref="System.Int32" />
        /// E.g. for monthly is "00" for quarterly is "\\Q0" (For SDMX TIme Period)
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Gets the frequency code
        /// </summary>
        char FrequencyCode { get; }

        /// <summary>
        /// Gets the <see cref="GesmesPeriod" />
        /// </summary>
        GesmesPeriod Gesmes { get; }

        /// <summary>
        /// Gets the number of months in a period e.g. 1 for Monthly, 3 for quarterly (For SDMX Time Period)
        /// </summary>
        byte MonthsPerPeriod { get; }

        /// <summary>
        /// Gets the number of periods in a period e.g. 12 for monthly (For SDMX Time Period)
        /// </summary>
        short PeriodCount { get; }

        /// <summary>
        /// Gets the period prefix if any
        /// </summary>
        char PeriodPrefix { get; }

        /// <summary>
        /// Gets the time format
        /// </summary>
        TimeFormat TimeFormat { get; }

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
        DateTime AddPeriod(DateTime dateTime);

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
        DateTime ToDateTime(string sdmxPeriod, bool start);

        /// <summary>
        /// Convert the specified <paramref name="time"/> to SDMX Time Period type representation
        /// </summary>
        /// <param name="time">
        /// The <see cref="DateTime"/> object to convert 
        /// </param>
        /// <returns>
        /// A string with the SDMX Time Period 
        /// </returns>
        string ToString(DateTime time);

        #endregion
    }
}
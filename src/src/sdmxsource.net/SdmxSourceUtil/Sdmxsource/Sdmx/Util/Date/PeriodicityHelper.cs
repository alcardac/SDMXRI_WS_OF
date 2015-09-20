// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeriodicityHelper.cs" company="Eurostat">
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

    /// <summary>
    /// Utils for periodicity
    /// </summary>
    public static class PeriodicityHelper
    {
        #region Constants

        /// <summary>
        /// The format used in frequencies with prefix and contain two or more months in each period
        /// </summary>
        private const string PrefixFormatString = "{0:yyyy}-{1}{2}";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Convert an SDMX Time Period type to a System.DateTime object
        /// </summary>
        /// <param name="sdmxPeriod">
        /// A string with the SDMX Time period 
        /// </param>
        /// <param name="months">
        /// The number of months in a period 
        /// </param>
        /// <param name="start">
        /// If it is true it will expand to the start of the period else towards the end e.g. if it is true 2001-04 will become 2001-04-01 else it will become 2001-04-30 
        /// </param>
        /// <param name="digitStart">
        /// The first digit in the period 
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> object 
        /// </returns>
        public static DateTime ConvertToDateTime(string sdmxPeriod, byte months, bool start, byte digitStart)
        {
            var ret = new DateTime();
            if (!string.IsNullOrEmpty(sdmxPeriod))
            {
                string[] dateFields = sdmxPeriod.Split(new[] { '-' }, 3);
                if (dateFields.Length == 2)
                {
                    short period = Convert.ToInt16(dateFields[1].Substring(digitStart), CultureInfo.InvariantCulture);
                    short year = Convert.ToInt16(dateFields[0].Substring(0, 4), CultureInfo.InvariantCulture);
                    int day = 1;
                    int endMonth = 0;
                    if (start)
                    {
                        checked
                        {
                            endMonth = months - 1;
                        }
                    }

                    int month = (period * months) - endMonth;
                    if (!start)
                    {
                        day = DateTime.DaysInMonth(year, month);
                    }

                    ret = new DateTime(year, month, day);
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
        /// <param name="months">
        /// The number of months in a period 
        /// </param>
        /// <param name="prefix">
        /// The periodicity prefix 
        /// </param>
        /// <returns>
        /// A string with the SDMX Time Period 
        /// </returns>
        public static string ConvertToString(DateTime time, int months, char prefix)
        {
            IFormatProvider fmt = CultureInfo.InvariantCulture;
            return string.Format(fmt, PrefixFormatString, time, prefix, ((time.Month - 1) / months) + 1);
        }

        #endregion
    }
}
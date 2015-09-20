// -----------------------------------------------------------------------
// <copyright file="SdmxDateExtensions.cs" company="Eurostat">
//   Date Created : 2013-09-27
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// The SDMX date extensions.
    /// </summary>
    public static class SdmxDateExtensions
    {
        /// <summary>
        /// The _invariant culture
        /// </summary>
        private static readonly CultureInfo _invariantCulture;

        /// <summary>
        /// Initializes static members of the <see cref="SdmxDateExtensions"/> class.
        /// </summary>
        static SdmxDateExtensions()
        {
            _invariantCulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Check if the <paramref name="thisSdmxDate"/> starts before <paramref name="otherSdmxDate"/>.
        /// </summary>
        /// <param name="thisSdmxDate">The this SDMX date.</param>
        /// <param name="otherSdmxDate">The other SDMX date.</param>
        /// <returns><c>true</c> is the <paramref name="thisSdmxDate"/> starts before <paramref name="otherSdmxDate"/>; otherwise false.</returns>
        public static bool StartsBefore(this ISdmxDate thisSdmxDate, ISdmxDate otherSdmxDate)
        {
            DateTime thisDate = DateUtil.FormatDate(thisSdmxDate.DateInSdmxFormat, true);
            DateTime otherDate = DateUtil.FormatDate(otherSdmxDate.DateInSdmxFormat, true);

            return thisDate.CompareTo(otherDate) < 0;
        }

        /// <summary>
        /// Check if the <paramref name="thisSdmxDate"/> ends after <paramref name="otherSdmxDate"/>.
        /// </summary>
        /// <param name="thisSdmxDate">The this SDMX date.</param>
        /// <param name="otherSdmxDate">The other SDMX date.</param>
        /// <returns><c>true</c> is the <paramref name="thisSdmxDate"/> ends after <paramref name="otherSdmxDate"/>; otherwise false.</returns>
        public static bool EndsAfter(this ISdmxDate thisSdmxDate, ISdmxDate otherSdmxDate)
        {
            DateTime thisDate = DateUtil.FormatDate(thisSdmxDate.DateInSdmxFormat, false);
            DateTime otherDate = DateUtil.FormatDate(otherSdmxDate.DateInSdmxFormat, false);

            return thisDate.CompareTo(otherDate) > 0;
        }

        /// <summary>
        /// Formats the <paramref name="sdmxDate"/> as a date string.
        /// </summary>
        /// <param name="sdmxDate">The SDMX date.</param>
        /// <param name="startOfPeriod">if set to <c>true</c> [start of period].</param>
        /// <returns>the <paramref name="sdmxDate"/> as a date string.</returns>
        public static string FormatAsDateString(this ISdmxDate sdmxDate, bool startOfPeriod)
        {
            var date = DateUtil.FormatDate(sdmxDate.DateInSdmxFormat, startOfPeriod);
            return DateUtil.FormatDate(date, TimeFormatEnumType.Date);
        }

        /// <summary>
        /// Translates the specified <paramref name="sdmxDate"/> to <see cref="SdmxQueryPeriod"/>
        /// </summary>
        /// <param name="sdmxDate">The SDMX date.</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <returns>
        /// The <see cref="SdmxQueryPeriod" />.
        /// </returns>
        public static SdmxQueryPeriod ToQueryPeriod(this ISdmxDate sdmxDate, IPeriodicity periodicity)
        {
            if (sdmxDate == null)
            {
                return null;
            }

            if (periodicity.TimeFormat.EnumType != sdmxDate.TimeFormatOfDate)
            {
                sdmxDate = new SdmxDateCore(sdmxDate.Date, periodicity.TimeFormat);
            }

            var time = new SdmxQueryPeriod();

            string[] startTime = sdmxDate.DateInSdmxFormat.Split('-');
            var startYear = Convert.ToInt32(startTime[0].Substring(0, 4), _invariantCulture);
            time.Year = startYear;
            if (startTime.Length >= 2)
            {
                int startPeriod;
                if (int.TryParse(startTime[1].Substring(periodicity.DigitStart), NumberStyles.None, _invariantCulture, out startPeriod))
                { 
                    time.HasPeriod = true;
                    time.Period = startPeriod;
                }
            }

            return time;
        }
    }
}
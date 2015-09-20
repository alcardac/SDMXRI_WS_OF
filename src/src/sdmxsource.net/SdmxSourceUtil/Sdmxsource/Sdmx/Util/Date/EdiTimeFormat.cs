// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesPeriodCode.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    /// <summary>
    /// The GESMES (EDI) period code.
    /// </summary>
    public enum EdiTimeFormat
    {
        /// <summary>
        /// None (default)
        /// </summary>
        None = 0, 

        /// <summary>
        /// Monthly CCYYMM
        /// </summary>
        Month = 610, 

        /// <summary>
        /// Annual CCYY
        /// </summary>
        Year = 602, 

        /// <summary>
        /// Quarterly CCYYQ
        /// </summary>
        QuarterOfYear = 608, 

        /// <summary>
        /// Biannual CCYYB
        /// </summary>
        HalfOfYear = 604,

        /// <summary>
        /// Daily YYMMDD
        /// </summary>
        DailyTwoDigYear = 101, 

        /// <summary>
        /// Daily CCYYMMDD
        /// </summary>
        DailyFourDigYear = 102, 

        /// <summary>
        /// Weekly CCYYW
        /// </summary>
        Week = 616, 

        /// <summary>
        /// Up to minute detail YYDDHHMM
        /// </summary>
        MinuteTwoDigYear = 201,

        /// <summary>
        /// Up to minute detail CCYYMMDDHHMM (used in DTM+242 tag)
        /// </summary>
        MinuteFourDigYear = 203, 

        /// <summary>
        /// Monthly CCYYMM
        /// </summary>
        RangeMonthly = 710, 

        /// <summary>
        /// Annual CCYY
        /// </summary>
        RangeYear = 702, 

        /// <summary>
        /// Quarterly CCYYQ
        /// </summary>
        RangeQuarterOfYear = 708, 

        /// <summary>
        /// Biannual CCYYB
        /// </summary>
        RangeHalfOfYear = 704, 

        /// <summary>
        /// Daily CCYYMMDD
        /// </summary>
        RangeDaily = 711, 

        /// <summary>
        /// Weekly CCYYW
        /// </summary>
        RangeWeekly = 716
    }
}
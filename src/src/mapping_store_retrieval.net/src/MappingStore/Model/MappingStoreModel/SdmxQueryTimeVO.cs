// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxQueryTimeVO.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A Value Object used when transcoding SDMX Query Time period
//   to Dissemination database time period to store SDMX Query Time element
//   year and periods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// A Value Object used when transcoding SDMX Query Time period 
    /// to Dissemination database time period to store SDMX Query Time element
    /// year and periods
    /// </summary>
    public class SdmxQueryTimeVO
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the period part of EndTime Element e.g. the "1" from 2003-Q1
        /// </summary>
        public int EndPeriod { get; set; }

        /// <summary>
        /// Gets or sets the year part of EndTime Element e.g. the "2003" from 2003-Q1
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDMX Query Time.EndTime as a period. e.g. "2003" hasn't but "2004-01" has
        /// </summary>        
        public bool HasEndPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  the SDMX Query Time.StartTime has a period. e.g. "2003" hasn't but "2004-01" has
        /// </summary>
        public bool HasStartPeriod { get; set; }

        /// <summary>
        /// Gets or sets the period part of StartTime Element e.g. the "1" from 2003-Q1
        /// </summary>
        public int StartPeriod { get; set; }

        /// <summary>
        /// Gets or sets the year part of StartTime Element e.g. the "2003" from 2003-Q1
        /// </summary>
        public int StartYear { get; set; }

        #endregion
    }
}
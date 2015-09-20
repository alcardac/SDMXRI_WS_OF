// -----------------------------------------------------------------------
// <copyright file="SdmxQueryPeriod.cs" company="Eurostat">
//   Date Created : 2013-10-30
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    /// <summary>
    /// The SDMX query period.
    /// </summary>
    public class SdmxQueryPeriod
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [has period].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has period]; otherwise, <c>false</c>.
        /// </value>
        public bool HasPeriod { get; set; }
    }
}
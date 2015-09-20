// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeRange.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITimeRange
    {
        /**
	 * If true then start date and end date both have a value, and the range is between the start and end dates.
	 * <p/>
	 * If false, then only the start date or end date will be populated, if the start date is populated then it this period refers
	 * to dates before the start date. If the end date is populated then it refers to dates after the end date.
	 * @return
	 */
        /// <summary>
        /// If true then start date and end date both have a value, and the range is between the start and end dates.
        /// 
        /// </summary>
        bool IsRange{ get; }

        /**
         * Returns the Start Date - if range is true, or the Before date if range is false
         * @return
         */
        /// <summary>
        /// Returns the Start Date - if range is true, or the Before date if range is false
        /// </summary>
        ISdmxDate StartDate{ get; }

        /**
         * Returns the End Date - if range is true, or the After date if range is false
         * @return
         */
        ISdmxDate EndDate{ get; }

        /**
         * Returns true if the start date is included in the range
         * @return
         */
        /// <summary>
        /// 
        /// </summary>
        bool IsStartInclusive{ get; }

        /**
         * Returns true if the end date is included in the range
         * @return
         */
        /// <summary>
        /// 
        /// </summary>
        bool IsEndInclusive{ get; }
    }
}

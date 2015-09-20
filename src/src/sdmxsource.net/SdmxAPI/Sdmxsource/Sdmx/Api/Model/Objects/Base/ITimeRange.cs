// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeRange.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Time Range
    /// </summary>
    public interface ITimeRange : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the End Date - if range is true, or the After date if range is false
        /// </summary>
        /// <value> </value>
        ISdmxDate EndDate { get; }

        /// <summary>
        ///     Gets a value indicating whether the the end date is included in the range
        /// </summary>
        /// <value> </value>
        bool EndInclusive { get; }

        /// <summary>
        ///      Gets a value indicating whether the start date and end date both have a value, and the range is between the start and end dates.
        ///     <p />
        ///     If false, then only the start date or end date will be populated, if the start date is populated then it this period refers
        ///     to dates before the start date. If the end date is populated then it refers to dates after the end date.
        /// </summary>
        bool Range { get; }

        /// <summary>
        ///     Gets the Start Date - if range is true, or the Before date if range is false
        /// </summary>
        /// <value> </value>
        ISdmxDate StartDate { get; }

        /// <summary>
        ///     Gets a value indicating whether the the start date is included in the range
        /// </summary>
        /// <value> </value>
        bool StartInclusive { get; }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeRangeMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The TimeRangeMutableObject interface.
    /// </summary>
    public interface ITimeRangeMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the end date.
        /// </summary>
        DateTime? EndDate { get; set; }

        /// <summary>
        ///      Gets or sets a value indicating whether the date is included in the range.
        /// </summary>
        bool IsEndInclusive { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is start inclusive.
        /// </summary>
        bool IsStartInclusive { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the start date and end date both have a value, and the range is between the start and end dates.
        ///     <p />
        ///     If false, then only the start date or end date will be populated, if the start date is populated then it this period refers
        ///     to dates before the start date. If the end date is populated then it refers to dates after the end date.
        /// </summary>
        /// <value> </value>
        bool IsRange { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        DateTime? StartDate { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create immutable instance.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <returns>
        /// The <see cref="ITimeRange"/> .
        /// </returns>
        ITimeRange CreateImmutableInstance(ISdmxStructure parent);

        #endregion
    }
}
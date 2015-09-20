// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexVersionReference.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IComplexVersionReference
    {
        /// <summary>
        /// If true then start date and end date both have a value, and the range is between the start and end dates.
        /// If false, then only the start date or end date will be populated, if the start date is populated then it this period refers
        /// to dates before the start date. If the end date is populated then it refers to dates after the end date.
        /// </summary>
        TertiaryBool IsReturnLatest{ get; }
       
        /// <summary>
        /// Gets the version that is being reference, this can be null
        /// </summary>
        string Version{ get; }

        /// <summary>
        /// Gets the period from which the version should be valid from - this can be null
        /// </summary>
        ITimeRange VersionValidFrom{ get; }

        
        /// <summary>
        /// Gets the period from which the version should be valid to - this can be null
        /// </summary>
        ITimeRange VersionValidTo{ get; }
    }
}

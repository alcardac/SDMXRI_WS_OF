// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContentConstraintMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The ContentConstraintMutableObject interface.
    /// </summary>
    public interface IContentConstraintMutableObject : IConstraintMutableObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the metadata target region
        /// </summary>
        IMetadataTargetRegionMutableObject MetadataTargetRegion { get; set; }

        /// <summary>
        ///     Gets or sets the excluded cube region.
        /// </summary>
        ICubeRegionMutableObject ExcludedCubeRegion { get; set; }

        /// <summary>
        ///     Gets the immutable instance.
        /// </summary>
        new IContentConstraintObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the included cube region.
        /// </summary>
        ICubeRegionMutableObject IncludedCubeRegion { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is defining actual data present.
        /// </summary>
        bool IsDefiningActualDataPresent { get; set; }

        /// <summary>
        ///     Gets or sets the reference period.
        /// </summary>
        IReferencePeriodMutableObject ReferencePeriod { get; set; }

        /// <summary>
        ///     Gets or sets the release calendar.
        /// </summary>
        IReleaseCalendarMutableObject ReleaseCalendar { get; set; }

        #endregion
    }
}
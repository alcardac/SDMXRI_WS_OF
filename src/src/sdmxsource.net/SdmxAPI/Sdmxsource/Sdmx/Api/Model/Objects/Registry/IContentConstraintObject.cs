// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContentConstraintObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    #endregion

    /// <summary>
    ///     The ContentConstraintObject interface.
    /// </summary>
    public interface IContentConstraintObject : IConstraintObject
    {
        #region Public Properties

        /// <summary>
        /// Gets the metadata target region, or null if this is undefined.
        /// </summary>
        IMetadataTargetRegion MetadataTargetRegion { get; }

        /// <summary>
        ///     Gets the constraint attachment.
        /// </summary>
        new IConstraintAttachment ConstraintAttachment { get; }

        /// <summary>
        ///     Gets a value indicating whether the constraint is defining the actual data present for this constraint, false if it is defining the
        ///     allowed data
        /// </summary>
        /// <value> </value>
        bool IsDefiningActualDataPresent { get; }

        /// <summary>
        ///     Gets a CubeRegion where the Keys and Attributes are defining data is is not present or disallowed (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        ICubeRegion ExcludedCubeRegion { get; }

        /// <summary>
        ///     Gets the series keys that this constraint defines at ones that either do not have data, or are not allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        new IConstraintDataKeySet ExcludedSeriesKeys { get; }

        /// <summary>
        ///     Gets a CubeRegion where the Keys and Attributes are defining data that is present or allowed (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        ICubeRegion IncludedCubeRegion { get; }

        /// <summary>
        ///     Gets the series keys that this constraint defines at ones that either have data, or are allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        new IConstraintDataKeySet IncludedSeriesKeys { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IContentConstraintMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets the reference period.
        /// </summary>
        IReferencePeriod ReferencePeriod { get; }

        /// <summary>
        ///     Gets the release calendar.
        /// </summary>
        IReleaseCalendar ReleaseCalendar { get; }

        #endregion
    }
}
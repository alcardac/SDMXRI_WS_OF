// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ConstraintObject interface.
    /// </summary>
    public interface IConstraintObject : IMaintainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the structure(s) that this contraint is constraining
        /// </summary>
        /// <value> </value>
        IConstraintAttachment ConstraintAttachment { get; }

        /// <summary>
        ///     Gets the series keys that this constraint defines at ones that either do not have data, or are not allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        IConstraintDataKeySet ExcludedSeriesKeys { get; }

        /// <summary>
        ///     Gets the series keys that this constraint defines at ones that either have data, or are allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        /// <value> </value>
        IConstraintDataKeySet IncludedSeriesKeys { get; }

        /// <summary>
        /// Gets the Metadata keys that this constraint defines as ones that either 
        /// have data, or are allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        IConstraintDataKeySet IncludedMetadataKeys { get; }

        /// <summary>
        /// Gets the Metadata keys that this constraint defines as ones that either 
        /// do not have data, or are not allowed to have data (depending on isDefiningActualDataPresent() value)
        /// </summary>
        IConstraintDataKeySet ExcludedMetadataKeys { get; }

        #endregion
    }
}
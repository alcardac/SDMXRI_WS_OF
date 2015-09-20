// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ConstraintMutableObject interface.
    /// </summary>
    public interface IConstraintMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the constraint attachment.
        /// </summary>
        IConstraintAttachmentMutableObject ConstraintAttachment { get; set; }

        /// <summary>
        ///     Gets or sets the excluded series keys.
        /// </summary>
        IConstraintDataKeySetMutableObject ExcludedSeriesKeys { get; set; }

        /// <summary>
        ///     Gets or sets the included series keys.
        /// </summary>
        IConstraintDataKeySetMutableObject IncludedSeriesKeys { get; set; }

        /// <summary>
        /// Gets or sets the metadata keys that this constraint defines at ones that either have data, or are allowed to have data (depending on IsDefiningActualDataPresent value)
        /// </summary>
        IConstraintDataKeySetMutableObject IncludedMetadataKeys { get; set; }

        /// <summary>
        /// Gets or sets the metadata keys that this constraint defines at ones that either do not have data, or are not allowed to have data (depending on IsDefiningActualDataPresent value)
        /// </summary>
        IConstraintDataKeySetMutableObject ExcludedMetadataKeys { get; set; }

        #endregion
    }
}
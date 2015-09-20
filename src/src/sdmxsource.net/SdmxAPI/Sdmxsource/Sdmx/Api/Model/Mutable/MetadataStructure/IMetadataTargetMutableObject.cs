// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataTargetMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The MetadataTargetMutableObject interface.
    /// </summary>
    public interface IMetadataTargetMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the constraint content target.
        /// </summary>
        IConstraintContentTargetMutableObject ConstraintContentTarget { get; set; }

        /// <summary>
        ///     Gets or sets the data set target.
        /// </summary>
        IDataSetTargetMutableObject DataSetTarget { get; set; }

        /// <summary>
        ///     Gets the identifiable target.
        /// </summary>
        IList<IIdentifiableTargetMutableObject> IdentifiableTarget { get; }

        /// <summary>
        ///     Gets or sets the key descriptor values target.
        /// </summary>
        IKeyDescriptorValuesTargetMutableObject KeyDescriptorValuesTarget { get; set; }

        /// <summary>
        ///     Gets or sets the report period target.
        /// </summary>
        IReportPeriodTargetMutableObject ReportPeriodTarget { get; set; }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataAttributeMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The MetadataAttributeMutableObject interface.
    /// </summary>
    public interface IMetadataAttributeMutableObject : IComponentMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the max occurs.
        /// </summary>
        int? MaxOccurs { get; set; }

        /// <summary>
        ///     Gets the metadata attributes.
        /// </summary>
        IList<IMetadataAttributeMutableObject> MetadataAttributes { get; }

        /// <summary>
        ///     Gets or sets the min occurs.
        /// </summary>
        int? MinOccurs { get; set; }

        /// <summary>
        ///     Gets or sets the presentational.
        /// </summary>
        TertiaryBool Presentational { get; set; }

        #endregion
    }
}
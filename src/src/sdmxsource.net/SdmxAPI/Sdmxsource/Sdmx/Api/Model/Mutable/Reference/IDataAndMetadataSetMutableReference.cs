// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataAndMetadataSetMutableReference.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The DataAndMetadataSetMutableReference interface.
    /// </summary>
    public interface IDataAndMetadataSetMutableReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the dataset referenced by data flow/provider/provision
        /// </summary>
        /// <value>
        ///     The <see cref="IStructureReference" />
        /// </value>
        IStructureReference DataSetReference { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this is a dataset reference, false if it is a metadata set reference
        /// </summary>
        /// <value> </value>
        bool IsDataSetReference { get; set; }

        /// <summary>
        ///     Gets or sets the set id.
        /// </summary>
        string SetId { get; set; }

        #endregion
    }
}
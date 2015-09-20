// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataAndMetadataSetReference.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;

    #endregion

    /// <summary>
    ///     The DataAndMetadataSetReference interface.
    /// </summary>
    public interface IDataAndMetadataSetReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets the dataset referenced by data flow/provider/provision
        /// </summary>
        /// <value> </value>
        ICrossReference DataSetReference { get; }

        /// <summary>
        ///     Gets a value indicating whether the is a dataset reference, false if it is a metadata set reference
        /// </summary>
        /// <value> </value>
        bool IsDataSetReference { get; }

        /// <summary>
        ///     Gets the id of the data or metadata set
        /// </summary>
        /// <value> </value>
        string SetId { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a mutable version
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataAndMetadataSetMutableReference" /> .
        /// </returns>
        IDataAndMetadataSetMutableReference CreateMutableInstance();

        #endregion
    }
}
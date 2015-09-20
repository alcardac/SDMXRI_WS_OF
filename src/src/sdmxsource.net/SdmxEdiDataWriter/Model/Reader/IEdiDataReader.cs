// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiDataReader.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EdiDataReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    ///     The EdiDataReader interface.
    /// </summary>
    public interface IEdiDataReader : IEdiAbstractPositionalReader 
    {
        #region Public Properties

        /// <summary>
        ///     Gets the data set header for the data that this reader is reading.
        /// </summary>
        /// <value>
        ///     The data set header object.
        /// </value>
        IDatasetHeader DataSetHeaderObject { get; }

        /// <summary>
        ///     Gets the dataset attributes.
        /// </summary>
        /// <value>
        ///     The dataset attributes.
        /// </value>
        IList<IKeyValue> DatasetAttributes { get; }

        /// <summary>
        ///     Gets the missing value.
        /// </summary>
        /// <value>
        ///     The missing value.
        /// </value>
        string MissingValue { get; }

        #endregion
    }
}
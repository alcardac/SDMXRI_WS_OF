// -----------------------------------------------------------------------
// <copyright file="IMetadataFormat.cs" company="Eurostat">
//   Date Created : 2014-03-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Model.Format
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The MetadataFormat interface.
    /// </summary>
    public interface IMetadataFormat
    {
        /// <summary>
        /// Gets the SDMX format.
        /// </summary>
        /// <value>
        /// The SDMX format.
        /// </value>
         SdmxSchema SdmxFormat { get; }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataFormat.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Data format is an Interface which wrappers a strongly typed and non-extensible <see cref="DataType"/> enumeration.  The purpose of the wrapper is to 
    /// allow for additional implementations to be provided which may describe non-sdmx formats which are not supported by the <see cref="DataType"/> enumeration.
    /// </summary>
    public interface IDataFormat
    {
        /// <summary>
        /// Gets the sdmx data format that this interface is describing.
        /// If this is not describing an SDMX message then this will return null and the implementation class will be expected to expose additional methods to understand the data
        /// </summary>
        DataType SdmxDataFormat { get; }

        /// <summary>
        /// Gets a string representation of the format, that can be used for auditing and debugging purposes.
        /// <p/>
        /// This is expected to return a not null response.
        /// </summary>
        string FormatAsString { get; }
    }
}
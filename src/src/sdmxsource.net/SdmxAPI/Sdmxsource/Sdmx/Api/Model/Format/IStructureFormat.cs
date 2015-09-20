// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureFormat.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Format
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// IStructureFormat is an Interface which wrappers a strongly typed and non-extensible <see cref="DataType"/> enumeration.  The purpose of the wrapper is to
    /// allow for additional implementations to be provided which may describe non-sdmx formats which are not supported by the <see cref="DataType"/> enum.
    /// </summary>
    public interface IStructureFormat
    {
        /// <summary>
        /// Gets the SDMX Structure Output Type that this interface is describing.
        /// If this is not describing an SDMX message then this will return null and the implementation class will be expected to expose additional methods
        /// to describe the output format
        /// </summary>
        StructureOutputFormat SdmxOutputFormat { get; }

        /// <summary>
        /// Gets a string representation of the format, that can be used for auditing and debugging purposes.
        /// <p/>
        /// This is expected to return a not null response.
        /// </summary>
        string FormatAsString { get; }
    }
}

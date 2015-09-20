// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaFormat.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Format
{
    /// <summary>
    /// Defines a Schema format 
    /// </summary>
    public interface ISchemaFormat
    { 
        /// <summary>
        /// Gets a string representation of the format, that can be used for auditing and debugging purposes.
        /// <p/>
        /// This is expected to return a not null response.
        /// </summary>
        string FormatAsString { get; }
    }
}

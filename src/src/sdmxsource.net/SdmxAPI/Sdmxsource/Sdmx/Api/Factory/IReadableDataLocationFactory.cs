// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadableDataLocationFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using System;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// Used to create ReadableDataLocations from various sources of information
    /// </summary>
    public interface IReadableDataLocationFactory
    {
        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="uriStr"></param>
        /// <returns></returns>
        IReadableDataLocation GetReadableDataLocation(String uriStr);
        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        IReadableDataLocation GetReadableDataLocation(byte[] bytes);
        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL)
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        IReadableDataLocation GetReadableDataLocation(FileInfo file);

        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IReadableDataLocation GetReadableDataLocation(Uri url);
        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        IReadableDataLocation GetReadableDataLocation(Stream streamReader);
    }
}

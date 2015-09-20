// -----------------------------------------------------------------------
// <copyright file="ReadableDataLocationFactory.cs" company="Eurostat">
//   Date Created : 2014-04-16
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Util;

    public class ReadableDataLocationFactory : IReadableDataLocationFactory
    {
        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="uriStr"></param>
        /// <returns></returns>
        public IReadableDataLocation GetReadableDataLocation(string uriStr)
        {
            return new ReadableDataLocationTmp(uriStr);
        }

        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public IReadableDataLocation GetReadableDataLocation(byte[] bytes)
        {
            return new MemoryReadableLocation(bytes);
        }

        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IReadableDataLocation GetReadableDataLocation(FileInfo file)
        {
            return new FileReadableDataLocation(file);
        }

        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IReadableDataLocation GetReadableDataLocation(Uri url)
        {
            return new ReadableDataLocationTmp(url);
        }

        /// <summary>
        /// Create a readable data location from a String, this may represent a URI (file or URL) 
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        public IReadableDataLocation GetReadableDataLocation(Stream streamReader)
        {
            return new ReadableDataLocationTmp(streamReader);
        }
    }
}
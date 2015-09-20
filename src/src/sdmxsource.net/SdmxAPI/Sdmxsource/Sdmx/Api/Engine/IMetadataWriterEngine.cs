// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Engine
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;

    /// <summary>
    /// Writes a MetadataSet to the output stream
    /// </summary>
    public interface IMetadataWriterEngine
    {
        /// <summary>
        /// Writes one or more meta-data sets to the output stream
        /// </summary>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="metadataSet">The meta-data sets</param>
        void WriteMetadataSet(Stream outputStream, params IMetadataSet[] metadataSet);
    }
}

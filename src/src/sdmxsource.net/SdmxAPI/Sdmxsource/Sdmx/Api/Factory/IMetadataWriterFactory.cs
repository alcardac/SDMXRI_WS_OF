// -----------------------------------------------------------------------
// <copyright file="IMetadataWriterFactory.cs" company="Eurostat">
//   Date Created : 2014-03-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A MetadataWriterFactory operates as a plug-in to a Manager which can request a MetadataWriterEngine, to which the implementation will
    ///  respond with an appropriate MetadataWriterEngine if it is able, otherwise it will return null
    /// </summary>
    public interface IMetadataWriterFactory
    {
        /// <summary>
        /// Gets the metadata writer engine.
        /// </summary>
        /// <param name="outputFormat">The output format.</param>
        /// <returns> a MetadataWriterEngine if the output format is understood, if it is not known then NULL is returned</returns>
        IMetadataWriterEngine GetMetadataWriterEngine(IMetadataFormat outputFormat);

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWriterFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A StructureWriterFactory operates as a plugin to a Manager which can request a StructureWritingEngine, to which the implementation will
    /// respond with an appropriate StructureWritingEngine if it is able, otherwise it will return null
    /// </summary>
    public interface IStructureWriterFactory
    {
        /// <summary>
        /// Obtains a StructureWritingEngine engine for the given output format
        /// </summary>
        /// <param name="structureFormat">An implementation of the StructureFormat to describe the output format for the structures (required)</param>
        /// <param name="streamWriter">The output stream to write to (can be null if it is not required)</param>
        /// <returns>Null if this factory is not capable of creating a data writer engine in the requested format</returns>
        IStructureWriterEngine GetStructureWriterEngine(IStructureFormat structureFormat, Stream streamWriter);
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataWriterFactory.cs" company="Eurostat">
//   Date Created : 2013-04-01
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
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    /// <summary>
    /// The DataWriterFactory interface. A DataWriterFactory operates as a plugin to a manager which can request a DataWriterEngine, to which the implementation will
    /// respond with an appropriate DataWriterEngine if it is able, otherwise it will return null
    /// </summary>
    public interface IDataWriterFactory
    {
        /// <summary>
        /// Gets the data writer engine.
        /// </summary>
        /// <param name="dataFormat">
        /// The data format.
        /// </param>
        /// <param name="outStream">
        /// The output stream.
        /// </param>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        IDataWriterEngine GetDataWriterEngine(IDataFormat dataFormat, Stream outStream);
    }
}
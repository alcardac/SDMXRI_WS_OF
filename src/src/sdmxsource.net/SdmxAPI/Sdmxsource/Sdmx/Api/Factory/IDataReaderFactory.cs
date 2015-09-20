// -----------------------------------------------------------------------
// <copyright file="IDataReaderFactory.cs" company="Eurostat">
//   Date Created : 2014-05-15
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// Creates <see cref="IDataReaderEngine"/> from Source data, DataStructure (or <see cref="ISdmxObjectRetrievalManager"/> used to get DataStructure(s))
    /// </summary>
    public interface IDataReaderFactory
    {
        /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
        /// </summary>
        /// <param name="sourceData">The source data, giving access to an InputStream of the data.</param>
        /// <param name="dsd">The Data Structure Definition, describes the data in terms of the dimensionality.</param>
        /// <param name="dataflow">The dataflow (optional). Provides further information about the data.</param>
        /// <returns>The <see cref="IDataReaderEngine"/>; otherwise null if the <paramref name="sourceData"/> cannot be read.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sourceData"/> is null -or- <paramref name="dsd"/> is null</exception>
        IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, IDataStructureObject dsd, IDataflowObject dataflow);

        /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
        /// </summary>
        /// <param name="sourceData">
        /// The source data, giving access to an InputStream of the data.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager. It is used to obtain the <see cref="IDataStructureObject"/> that describe the data.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/>; otherwise null if the <paramref name="sourceData"/> cannot be read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceData"/> is null -or- <paramref name="retrievalManager"/> is null
        /// </exception>
        IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, ISdmxObjectRetrievalManager retrievalManager);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiWorkspace.cs" company="Eurostat">
//   Date Created : 2014-05-15
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EdiWorkspace interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;

    /// <summary>
    /// The EdiWorkspace interface.
    /// </summary>
    public interface IEdiWorkspace : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets the dataset headers.
        /// </summary>
        /// <value>
        ///     The dataset headers.
        /// </value>
        IList<IDatasetHeader> DatasetHeaders { get; }

        /// <summary>
        ///     Gets a value indicating whether any <see cref="IEdiDataDocument"/> exist in this workspace.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [has data]; otherwise, <c>false</c>.
        /// </value>
        bool HasData { get; }

        /// <summary>
        ///     Gets a value indicating whether any <see cref="ISdmxObjects"/> exist in this workspace.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [has structures]; otherwise, <c>false</c>.
        /// </value>
        bool HasStructures { get; }

        /// <summary>
        ///     Gets the header.
        /// </summary>
        /// <value>
        ///     The header.
        /// </value>
        IHeader Header { get; }

        /// <summary>
        ///     Gets the merged objects.
        /// </summary>
        /// <value>
        ///     The merged objects.
        /// </value>
        ISdmxObjects MergedObjects { get; }

        /// <summary>
        ///     Gets the objects.
        /// </summary>
        /// <value>
        ///     The objects.
        /// </value>
        IList<ISdmxObjects> Objects { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/> if this <see cref="HasData"/> is <c>True</c>; otherwise null.
        /// </returns>
        IDataReaderEngine GetDataReader(ISdmxObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="keyFamily">
        /// The key family.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow (Optional).
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/> if this <see cref="HasData"/> is <c>True</c>; otherwise null.
        /// </returns>
        IDataReaderEngine GetDataReader(IDataStructureObject keyFamily, IDataflowObject dataflow);

        #endregion
    }
}
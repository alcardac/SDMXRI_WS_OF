// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Engine
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The DataReaderEngine engine is capable of reading Data Sets in an iterative way.
    ///     <p />
    ///     The DataReaderEngine may contain the dataStructureObject that it is reading data for, and the data being read is not necessarily time series.
    ///     <p />
    /// </summary>
    public interface IDataReaderEngine : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets the header information for the current dataset.  This may contain references to the data structure, dataflow, or provision agreement that this data is for
        /// </summary>
        /// <value> </value>
        IDatasetHeader CurrentDatasetHeader { get; }

        /// <summary>
        ///     Gets the current Keyable entry in the dataset, if there has been no initial call to moveNextKeyable, then null will be returned.
        /// </summary>
        /// <value> </value>
        IKeyable CurrentKey { get; }

        /// <summary>
        /// Gets the dataflow that this reader engine is currently reading data for.
        /// This is not guaranteed to return a DataflowBean, as it may be unknown or not applicable, in this case null will be returned
        /// Note this will return null unless there has been a call to moveNextDataset(), this Dataflow returned by this method call may change when reading a new dataset
        /// </summary>
        IDataflowObject Dataflow { get; }

        /// <summary>
        ///     Gets the current Observation for the current Key-able.
        ///     <p />
        ///     Gets null if any of the following conditions are met:
        ///     <ul>
        ///         <li><see cref="CurrentKey"/> returns null</li>
        ///         <li><see cref="CurrentKey"/> returns a Key-able which defines a GroupKey</li>
        ///         <li><see cref="MoveNextKeyable"/> has been called with no subsequent call to <see cref="MoveNextObservation"/></li>
        ///         <li><see cref="MoveNextObservation"/> was called and returned false</li>
        ///     </ul>
        /// </summary>
        /// <value> the next observation value </value>
        IObservation CurrentObservation { get; }

        /// <summary>
        ///     Gets the data structure definition that this reader engine is currently reading data for
        ///     <p />
        ///     Note this will return null unless there has been a call to moveNextDataset(), this KeyFamily returned by this method call may change when reading a new dataset
        /// </summary>
        /// <value> </value>
        IDataStructureObject DataStructure { get; }

        /// <summary>
        ///     Gets the attributes available for the current dataset
        /// </summary>
        /// <value> a copy of the list, returns an empty list if there are no dataset attributes </value>
        IList<IKeyValue> DatasetAttributes { get; }

        /// <summary>
        ///     Gets the current dataset index the iterator position is at within the data source.
        ///     <p />
        ///     Index starts at -1, (no datasets have been read)
        /// </summary>
        /// <value> </value>
        int DatasetPosition { get; }

        /// <summary>
        ///     Gets the header of the datasource that this reader engine is reading data for.  The header is related to the message and not an individual dataset
        /// </summary>
        /// <value> </value>
        IHeader Header { get; }

        /// <summary>
        ///     Gets the current Keyable index the iterator position is at within the Data Set
        ///     <p />
        ///     Index starts at -1 - (no Keys have been Read)
        /// </summary>
        int KeyablePosition { get; }

        /// <summary>
        ///     Gets the current Observation index the iterator position is at within the current Keyable being read.
        ///     <p />
        ///     Index starts at -1 (no observations have been read - meaning getCurrentObservation() will return null
        /// </summary>
        /// <value> </value>
        int ObsPosition { get; }

        /// <summary>
        /// Gets the provision agreement that this data is for.
        /// </summary>
        /// <value>
        /// The provision agreement.
        /// </value>
        /// <remarks>This is not guaranteed to return a ProvisionAgreementBean, as it may be unknown or not applicable, in this case null will be returned
        /// Note this will return null unless there has been a call to <see cref="MoveNextDataset"/>, this Provision Agreement returned by this method call may change when reading a new dataset
        /// </remarks>
        IProvisionAgreementObject ProvisionAgreement { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes the reader engine, and releases all resources.
        /// </summary>
        void Close();

        /// <summary>
        /// Copies the entire dataset that the reader is reading, to the output stream (irrespective of current position)
        /// </summary>
        /// <param name="outputStream">
        /// output stream to copy data to
        /// </param>
        void CopyToOutputStream(Stream outputStream);

        /// <summary>
        ///     Creates a copy of this data reader engine, the copy is another iterator over the same source data
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataReaderEngine" /> .
        /// </returns>
        IDataReaderEngine CreateCopy();

        /// <summary>
        ///     Gets a value indicating whether the there are any more datasets in the data source
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool MoveNextDataset();

        /// <summary>
        ///     Gets a value indicating whether the there are any more keys in the dataset
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool MoveNextKeyable();

        /// <summary>
        ///     If this reader is in a series, this will return true if the series has any more observation values.
        /// </summary>
        /// <returns> true if series has more observation values </returns>
        bool MoveNextObservation();

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="KeyablePosition"/> moved back to -1)
        /// </summary>
        void Reset();

        #endregion
    }
}
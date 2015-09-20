// -----------------------------------------------------------------------
// <copyright file="AbstractWrapperedDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-06-17
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    /// Acts a a direct wrapper on top of a <see cref="IDataReaderEngine"/>.  The intention is to override this class if any specific methods are to be overridden.
    /// </summary>
    public class AbstractWrapperedDataReaderEngine : IDataReaderEngine
    {
        /// <summary>
        /// The data reader engine
        /// </summary>
        private readonly IDataReaderEngine _dataReaderEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWrapperedDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine.</param>
        public AbstractWrapperedDataReaderEngine(IDataReaderEngine dataReaderEngine)
        {
            if (dataReaderEngine == null)
            {
                throw new ArgumentNullException("dataReaderEngine");
            }

            this._dataReaderEngine = dataReaderEngine;
        }

        /// <summary>
        ///     Gets the header information for the current dataset.  This may contain references to the data structure, dataflow, or provision agreement that this data is for
        /// </summary>
        /// <value> </value>
        public virtual IDatasetHeader CurrentDatasetHeader
        {
            get
            {
                return this._dataReaderEngine.CurrentDatasetHeader;
            }
        }

        /// <summary>
        ///     Gets the current Keyable entry in the dataset, if there has been no initial call to moveNextKeyable, then null will be returned.
        /// </summary>
        /// <value> </value>
        public virtual IKeyable CurrentKey
        {
            get
            {
                return this._dataReaderEngine.CurrentKey;
            }
        }

        /// <summary>
        /// Gets the dataflow that this reader engine is currently reading data for.
        /// This is not guaranteed to return a DataflowBean, as it may be unknown or not applicable, in this case null will be returned
        /// Note this will return null unless there has been a call to moveNextDataset(), this Dataflow returned by this method call may change when reading a new dataset
        /// </summary>
        public virtual IDataflowObject Dataflow
        {
            get
            {
                return this._dataReaderEngine.Dataflow;
            }
        }

        /// <summary>
        ///     Gets the current Observation for the current Key-able.
        ///     <p />
        ///     Gets null if any of the following conditions are met:
        ///     <ul>
        ///         <li><see cref="IDataReaderEngine.CurrentKey"/> returns null</li>
        ///         <li><see cref="IDataReaderEngine.CurrentKey"/> returns a Key-able which defines a GroupKey</li>
        ///         <li><see cref="IDataReaderEngine.MoveNextKeyable"/> has been called with no subsequent call to <see cref="IDataReaderEngine.MoveNextObservation"/></li>
        ///         <li><see cref="IDataReaderEngine.MoveNextObservation"/> was called and returned false</li>
        ///     </ul>
        /// </summary>
        /// <value> the next observation value </value>
        public virtual IObservation CurrentObservation
        {
            get
            {
                return this._dataReaderEngine.CurrentObservation;
            }
        }

        /// <summary>
        ///     Gets the data structure definition that this reader engine is currently reading data for
        ///     <p />
        ///     Note this will return null unless there has been a call to moveNextDataset(), this KeyFamily returned by this method call may change when reading a new dataset
        /// </summary>
        /// <value> </value>
        public virtual IDataStructureObject DataStructure
        {
            get
            {
                return this._dataReaderEngine.DataStructure;
            }
        }

        /// <summary>
        ///     Gets the attributes available for the current dataset
        /// </summary>
        /// <value> a copy of the list, returns an empty list if there are no dataset attributes </value>
        public virtual IList<IKeyValue> DatasetAttributes
        {
            get
            {
                return this._dataReaderEngine.DatasetAttributes;
            }
        }

        /// <summary>
        ///     Gets the current dataset index the iterator position is at within the data source.
        ///     <p />
        ///     Index starts at -1, (no datasets have been read)
        /// </summary>
        /// <value> </value>
        public virtual int DatasetPosition
        {
            get
            {
                return this._dataReaderEngine.DatasetPosition;
            }
        }

        /// <summary>
        ///     Gets the header of the datasource that this reader engine is reading data for.  The header is related to the message and not an individual dataset
        /// </summary>
        /// <value> </value>
        public virtual IHeader Header
        {
            get
            {
                return this._dataReaderEngine.Header;
            }
        }

        /// <summary>
        ///     Gets the current Keyable index the iterator position is at within the Data Set
        ///     <p />
        ///     Index starts at -1 - (no Keys have been Read)
        /// </summary>
        public virtual int KeyablePosition
        {
            get
            {
                return this._dataReaderEngine.KeyablePosition;
            }
        }

        /// <summary>
        ///     Gets the current Observation index the iterator position is at within the current Keyable being read.
        ///     <p />
        ///     Index starts at -1 (no observations have been read - meaning getCurrentObservation() will return null
        /// </summary>
        /// <value> </value>
        public virtual int ObsPosition
        {
            get
            {
                return this._dataReaderEngine.ObsPosition;
            }
        }

        /// <summary>
        /// Gets the provision agreement that this data is for.
        /// </summary>
        /// <value>
        /// The provision agreement.
        /// </value>
        /// <remarks>This is not guaranteed to return a ProvisionAgreementBean, as it may be unknown or not applicable, in this case null will be returned
        /// Note this will return null unless there has been a call to <see cref="IDataReaderEngine.MoveNextDataset"/>, this Provision Agreement returned by this method call may change when reading a new dataset
        /// </remarks>
        public virtual IProvisionAgreementObject ProvisionAgreement
        {
            get
            {
                return this._dataReaderEngine.ProvisionAgreement;
            }
        }

        /// <summary>
        ///     Closes the reader engine, and releases all resources.
        /// </summary>
        public virtual void Close()
        {
            this._dataReaderEngine.Close();
        }

        /// <summary>
        /// Copies the entire dataset that the reader is reading, to the output stream (irrespective of current position)
        /// </summary>
        /// <param name="outputStream">
        /// output stream to copy data to
        /// </param>
        public virtual void CopyToOutputStream(Stream outputStream)
        {
            this._dataReaderEngine.CopyToOutputStream(outputStream);
        }

        /// <summary>
        ///     Creates a copy of this data reader engine, the copy is another iterator over the same source data
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataReaderEngine" /> .
        /// </returns>
        public virtual IDataReaderEngine CreateCopy()
        {
            return this._dataReaderEngine.CreateCopy();
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more datasets in the data source
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public virtual bool MoveNextDataset()
        {
            return this._dataReaderEngine.MoveNextDataset();
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more keys in the dataset
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public virtual bool MoveNextKeyable()
        {
            return this._dataReaderEngine.MoveNextKeyable();
        }

        /// <summary>
        ///     If this reader is in a series, this will return true if the series has any more observation values.
        /// </summary>
        /// <returns> true if series has more observation values </returns>
        public virtual bool MoveNextObservation()
        {
            return this._dataReaderEngine.MoveNextObservation();
        }

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="IDataReaderEngine.KeyablePosition"/> moved back to -1)
        /// </summary>
        public virtual void Reset()
        {
            this._dataReaderEngine.Reset();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this._dataReaderEngine.Dispose();
        }
    }
}
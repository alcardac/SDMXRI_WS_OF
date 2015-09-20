// -----------------------------------------------------------------------
// <copyright file="DelayedCrossWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-11-18
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.Ws.Controllers.Extension;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    /// A wrapper <see cref="ICrossSectionalWriterEngine"/> that writes <see cref="IWriterEngine.WriteHeader"/> and <see cref="ICrossSectionalWriterEngine.StartDataset"/> after the first data is written.
    /// </summary>
    public class DelayedCrossWriterEngine : ICrossSectionalWriterEngine
    {
        /// <summary>
        /// The _actions
        /// </summary>
        private readonly Queue<Action> _actions;

        /// <summary>
        /// The _data writer engine
        /// </summary>
        private readonly ICrossSectionalWriterEngine _dataWriterEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayedCrossWriterEngine"/> class.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <param name="dataWriterEngine">The data writer engine.</param>
        public DelayedCrossWriterEngine(Queue<Action> actions, ICrossSectionalWriterEngine dataWriterEngine)
        {
            this._actions = actions;
            this._dataWriterEngine = dataWriterEngine;
        }

        /// <summary>
        /// (Optional) Writes a header to the message, any missing mandatory attributes are defaulted.  If a header is required for a message, then this
        /// call should be the first call on a WriterEngine
        /// <p />
        /// If this method is not called, and a message requires a header, a default header will be generated.
        /// </summary>
        /// <param name="header">The header.</param>
        public void WriteHeader(IHeader header)
        {
            this._actions.Enqueue(() => this._dataWriterEngine.WriteHeader(header));
        }

        /// <summary>
        /// Starts a dataset with the data conforming to the <paramref name="dsd" />
        /// </summary>
        /// <param name="dataflow">The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure.IDataflowObject" /></param>
        /// <param name="dsd">The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure.ICrossSectionalDataStructureObject" /></param>
        /// <param name="header">The Dataset attributes</param>
        /// <exception cref="T:System.ArgumentNullException">if the <paramref name="dsd" /> is null</exception>
        public void StartDataset(IDataflowObject dataflow, ICrossSectionalDataStructureObject dsd, IDatasetHeader header)
        {
            this._actions.Enqueue(() => this._dataWriterEngine.StartDataset(dataflow, dsd, header));
        }

        /// <summary>
        /// Start a Cross Sectional Section
        /// </summary>
        public void StartSection()
        {
           this.RunQueue();
           this._dataWriterEngine.StartSection();
        }

        /// <summary>
        /// Start a Cross Sectional group
        /// </summary>
        public void StartXSGroup()
        {
           this.RunQueue();
           this._dataWriterEngine.StartXSGroup();
        }

        /// <summary>
        /// Write a Cross Sectional Measure with <paramref name="measure" /> and <paramref name="value" />
        /// </summary>
        /// <param name="measure">The measure code</param>
        /// <param name="value">The measure value</param>
        public void StartXSObservation(string measure, string value)
        {
           this.RunQueue();
           this._dataWriterEngine.StartXSObservation(measure, value);
        }

        /// <summary>
        /// Write an <paramref name="attribute" /> and the <paramref name="value" />
        /// </summary>
        /// <param name="attribute">The attribute concept id</param>
        /// <param name="value">The value</param>
        public void WriteAttributeValue(string attribute, string value)
        {
           this.RunQueue();
           this._dataWriterEngine.WriteAttributeValue(attribute, value);
        }

        /// <summary>
        /// Write a Cross Sectional section <paramref name="key" /> and the <paramref name="value" />
        /// </summary>
        /// <param name="key">The key. i.e. the dimension</param>
        /// <param name="value">The value</param>
        public void WriteSectionKeyValue(string key, string value)
        {
           this.RunQueue();
            this._dataWriterEngine.WriteSectionKeyValue(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional DataSet <paramref name="key" /> and the <paramref name="value" />
        /// </summary>
        /// <param name="key">The key. i.e. the dimension</param>
        /// <param name="value">The value</param>
        public void WriteDataSetKeyValue(string key, string value)
        {
           this.RunQueue();
           this._dataWriterEngine.WriteDataSetKeyValue(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional Group <paramref name="key" /> and the <paramref name="value" />
        /// </summary>
        /// <param name="key">The key. i.e. the dimension</param>
        /// <param name="value">The value</param>
        public void WriteXSGroupKeyValue(string key, string value)
        {
           this.RunQueue();
           this._dataWriterEngine.WriteXSGroupKeyValue(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional measure <paramref name="key" /> and the <paramref name="value" />
        /// </summary>
        /// <param name="key">The key. i.e. the dimension</param>
        /// <param name="value">The value</param>
        public void WriteXSObservationKeyValue(string key, string value)
        {
           this.RunQueue();
           this._dataWriterEngine.WriteXSObservationKeyValue(key, value);
        }

        /// <summary>
        /// Closes the writer. Makes to close any opened XSObservation/Section/XSGroup and write the closing dataset/message elements.
        /// </summary>
        public void Close()
        {
           this.RunQueue();
            this._dataWriterEngine.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
           this.Dispose(true); 
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._dataWriterEngine.Dispose();
            }
        }

        /// <summary>
        /// Runs the queue.
        /// </summary>
        private void RunQueue()
        {
            this._actions.RunAll();
        }
    }
}
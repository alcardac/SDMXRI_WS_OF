// -----------------------------------------------------------------------
// <copyright file="DelayedDataWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-11-15
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// A wrapper <see cref="IDataWriterEngine"/> that writes <see cref="IWriterEngine.WriteHeader"/> and <see cref="IDataWriterEngine.StartDataset"/> after the first data is written.
    /// </summary>
    public class DelayedDataWriterEngine : IDataWriterEngine
    {
        /// <summary>
        /// The _actions
        /// </summary>
        private readonly Queue<Action> _actions;

        /// <summary>
        /// The _data writer engine
        /// </summary>
        private readonly IDataWriterEngine _dataWriterEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayedDataWriterEngine" /> class.
        /// </summary>
        /// <param name="dataWriterEngine">The data writer engine.</param>
        /// <param name="actions">The actions.</param>
        public DelayedDataWriterEngine(IDataWriterEngine dataWriterEngine, Queue<Action> actions)
        {
            if (dataWriterEngine == null)
            {
                throw new ArgumentNullException("dataWriterEngine");
            }

            this._dataWriterEngine = dataWriterEngine;
            this._actions = actions ?? new Queue<Action>();
        }

        /// <summary>
        /// (Optional) Writes a header to the message, any missing mandatory attributes are defaulted.  If a header is required for a message, then this
        /// call should be the first call on a WriterEngine
        /// <p/>
        /// If this method is not called, and a message requires a header, a default header will be generated.
        /// </summary>
        /// <param name="header">The header.
        /// </param>
        public void WriteHeader(IHeader header)
        {
            this._actions.Enqueue(() => this._dataWriterEngine.WriteHeader(header));
        }

        /// <summary>
        /// Writes the footer message (if supported by the writer implementation), and then completes the XML document, closes off all the tags, closes any resources.
        /// <b>NOTE</b> It is very important to close off a completed DataWriterEngine,
        /// as this ensures any output is written to the given location, and any resources are closed.  If this call
        /// is not made, the output document may be incomplete.
        /// <p><b>NOTE: </b> If writing a footer is not supported, then the footer will be silently ignored
        /// </p>
        /// Close also validates that the last series key or group key has been completed.
        /// </summary>
        /// <param name="footer">The footer.</param>
        public void Close(params IFooterMessage[] footer)
        {
            this.RunQueue();
            this._dataWriterEngine.Close();
        }

        /// <summary>
        /// Starts a dataset with the data conforming to the DSD
        /// </summary>
        /// <param name="dataflow">Optional. The dataflow can be provided to give extra information about the dataset.</param>
        /// <param name="dsd">The data structure is used to know the dimensionality of the data</param>
        /// <param name="header">Dataset header containing, amongst others, the dataset action, reporting dates,
        /// dimension at observation if null then the dimension at observation is assumed to be TIME_PERIOD and the dataset action is assumed to be INFORMATION</param>
        /// <param name="annotations">Any additional annotations that are attached to the dataset, can be null if no annotations exist</param>
        /// <exception cref="T:System.ArgumentException">if the DSD is null</exception>
        public void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {
            this._actions.Enqueue(() => this._dataWriterEngine.StartDataset(dataflow, dsd, header, annotations));
        }

        /// <summary>
        /// Starts a group with the given id, the subsequent calls to <c>writeGroupKeyValue</c> will write the id/values to this group.  After
        /// the group key is complete <c>writeAttributeValue</c> may be called to add attributes to this group.
        /// <p /><b>Example Usage</b>
        /// A group 'demo' is made up of 3 concepts (Country/Sex/Status), and has an attribute (Collection).
        /// <code>
        /// DataWriterEngine dre = //Create instance
        /// dre.startGroup("demo");
        /// dre.writeGroupKeyValue("Country", "FR");
        /// dre.writeGroupKeyValue("Sex", "M");
        /// dre.writeGroupKeyValue("Status", "U");
        /// dre.writeAttributeValue("Collection", "A");
        /// </code>
        /// Any subsequent calls, for example to start a series, or to start a new group, will close off this exiting group.
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="annotations">Annotations any additional annotations that are attached to the group, can be null if no annotations exist</param>
        public void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            this.RunQueue();
            this._dataWriterEngine.StartGroup(groupId, annotations);
        }

        /// <summary>
        /// Starts a new series, closes off any existing series keys or attribute/observations.
        /// </summary>
        /// <param name="annotations">Any additional annotations that are attached to the series, can be null if no annotations exist </param>
        public void StartSeries(params IAnnotation[] annotations)
        {
            this.RunQueue();
            this._dataWriterEngine.StartSeries(annotations);
        }

        /// <summary>
        /// Writes an attribute value for the given id.
        /// <p />
        /// If this method is called immediately after a <c>writeSeriesKeyValue</c> method call then it will write
        /// the attribute at the series level.  If it is called after a <c>writeGroupKeyValue</c> it will write the attribute against the group.
        /// <p />
        /// If this method is called immediately after a <c>writeObservation</c> method call then it will write the attribute at the observation level.
        /// </summary>
        /// <param name="id">the id of the given value for example 'OBS_STATUS'</param>
        /// <param name="value">The value.</param>
        public void WriteAttributeValue(string id, string value)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteAttributeValue(id, value);
        }

        /// <summary>
        /// Writes a group key value, for example 'Country' is 'France'.  A group may contain multiple id/value pairs in the key, so this method may be called consecutively with an id / value for each key item.
        /// <p />
        /// If this method is called consecutively multiple times and a duplicate id is passed in, then an exception will be thrown, as a group can only declare one value for a given id.
        /// <p />
        /// The <c>startGroup</c> method must be called before calling this method to add the first id/value, as the WriterEngine needs to know what group to assign the id/values to.
        /// </summary>
        /// <param name="id">the id of the concept or dimension</param>
        /// <param name="value">The value.</param>
        public void WriteGroupKeyValue(string id, string value)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteGroupKeyValue(id, value);
        }

        /// <summary>
        /// Writes an observation, the observation concept is assumed to be that which has been defined to be at the observation level (as declared in the start dataset method DatasetHeaderObject).
        /// </summary>
        /// <param name="obsConceptValue">May be the observation time, or the cross section value </param><param name="obsValue">The observation value - can be numerical</param><param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        public void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteObservation(obsConceptValue, obsValue, annotations);
        }

        /// <summary>
        /// Writes an Observation value against the current series.
        /// <p/>
        /// The current series is determined by the latest writeKeyValue call,
        /// If this is a cross sectional dataset, then the obsConcept is expected to be the cross sectional concept value - for example if this is cross sectional on Country the id may be "FR"
        /// If this is a time series dataset then the obsConcept is expected to be the observation time - for example 2006-12-12
        /// <p/>
        /// Validates the following:
        /// <ul><li>The obsTime string format is one of an allowed SDMX time format</li><li>The obsTime does not replicate a previously reported obsTime for the current series</li></ul>
        /// </summary>
        /// <param name="observationConceptId">the concept id for the observation, for example 'COUNTRY'.  If this is a Time Series, then the id will be DimensionBean.TIME_DIMENSION_FIXED_ID. </param><param name="obsConceptValue">may be the observation time, or the cross section value </param><param name="obsValue">The observation value - can be numerical
        /// </param><param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist </param>
        public void WriteObservation(string observationConceptId, string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteObservation(observationConceptId, obsConceptValue, obsValue, annotations);
        }

        /// <summary>
        /// Writes an Observation value against the current series
        /// <p />
        /// The date is formatted as a string, the format rules are determined by the TIME_FORMAT argument
        /// <p />
        /// Validates the following:
        /// <ul><li>The obsTime does not replicate a previously reported obsTime for the current series</li></ul>
        /// </summary>
        /// <param name="obsTime">the time of the observation</param>
        /// <param name="obsValue">the observation value - can be numerical</param>
        /// <param name="sdmxSwTimeFormat">the time format to format the obsTime in when converting to a string</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist </param>
        public void WriteObservation(DateTime obsTime, string obsValue, TimeFormat sdmxSwTimeFormat, params IAnnotation[] annotations)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteObservation(obsTime, obsValue, sdmxSwTimeFormat, annotations);
        }

        /// <summary>
        /// Writes a series key value.  This will have the effect of closing off any observation, or attribute values if they are any present
        /// <p/>
        /// If this method is called after calling writeAttribute or writeObservation, then the engine will start a new series.
        /// </summary>
        /// <param name="id">the id of the value for example 'Country'
        /// </param><param name="value">The value.
        /// </param>
        public void WriteSeriesKeyValue(string id, string value)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteSeriesKeyValue(id, value);
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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSdmxDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-06-17
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The abstract SDMX data reader engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants.Builder;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;

    /// <summary>
    ///     The abstract SDMX data reader engine.
    /// </summary>
    public abstract class AbstractSdmxDataReaderEngine : AbstractDataReaderEngine
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AbstractSdmxDataReaderEngine));

        #endregion

        #region Fields

        /// <summary>
        /// The XML reader builder
        /// </summary>
        private readonly IXmlReaderBuilder _xmlReaderBuilder = new XmlReaderBuilder();

        /// <summary>
        ///     The concept automatic component unique identifier
        /// </summary>
        private readonly IDictionary<string, string> _conceptToComponentId = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _is two point one
        /// </summary>
        private readonly bool _isTwoPointOne;

        /// <summary>
        ///     The _flat observation
        /// </summary>
        private IObservation _flatObs;

        /// <summary>
        ///     The _group unique identifier
        /// </summary>
        private string _groupId;

        /// <summary>
        ///     The _last call.
        /// </summary>
        private LastCall _lastCall;

        /// <summary>
        ///     The _no series
        /// </summary>
        private bool _noSeries;

        /// <summary>
        ///     The _parse input stream
        /// </summary>
        private Stream _parseInputStream;

        /// <summary>
        ///     The _parser
        /// </summary>
        private XmlReader _parser;

        /// <summary>
        ///     The _run ahead parser
        /// </summary>
        private XmlReader _runAheadParser;

        /// <summary>
        ///     The _run ahead parser input stream
        /// </summary>
        private Stream _runAheadParserInputStream;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSdmxDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataLocation">
        /// The data Location.
        /// </param>
        /// <param name="objectRetrieval">
        /// The SDMX Object Retrieval. giving the ability to retrieve DSDs for the datasets this
        ///     reader engine is reading.  This can be null if there is only one relevant DSD - in which case the
        ///     <paramref name="defaultDsd"/> should be provided.
        /// </param>
        /// <param name="defaultDataflow">
        /// The default Dataflow. (Optional)
        /// </param>
        /// <param name="defaultDsd">
        /// The default DSD. The default DSD to use if the <paramref name="objectRetrieval"/> is null, or
        ///     if the bean retrieval does not return the DSD for the given dataset.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// AbstractDataReaderEngine expects either a ISdmxObjectRetrievalManager or a
        ///     IDataStructureObject to be able to interpret the structures
        /// </exception>
        protected AbstractSdmxDataReaderEngine(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager objectRetrieval, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
            : base(dataLocation, objectRetrieval, defaultDataflow, defaultDsd)
        {
            this._isTwoPointOne = SdmxMessageUtil.GetSchemaVersion(dataLocation) == SdmxSchemaEnumType.VersionTwoPointOne;
        }

        #endregion

        #region Enums

        /// <summary>
        ///     The last call.
        /// </summary>
        private enum LastCall
        {
            /// <summary>
            ///     The null.
            /// </summary>
            Null, 

            /// <summary>
            ///     The has next data set.
            /// </summary>
            HasNextDataSet, 

            /// <summary>
            ///     The has next OBS.
            /// </summary>
            HasNextObs, 

            /// <summary>
            ///     The has next series.
            /// </summary>
            HasNextSeries, 

            /// <summary>
            ///     The next OBS.
            /// </summary>
            NextObs, 

            /// <summary>
            ///     The next series.
            /// </summary>
            NextSeries
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the group identifier.
        /// </summary>
        /// <value>
        ///     The group identifier.
        /// </value>
        protected string GroupId
        {
            get
            {
                return this._groupId;
            }

            set
            {
                this._groupId = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is SDMXv2.1.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is SDMXv2.1; otherwise, <c>false</c>.
        /// </value>
        protected bool IsTwoPointOne
        {
            get
            {
                return this._isTwoPointOne;
            }
        }

        /// <summary>
        ///     Gets the parser.
        /// </summary>
        /// <value>
        ///     The parser.
        /// </value>
        protected XmlReader Parser
        {
            get
            {
                return this._parser;
            }
        }

        /// <summary>
        ///     Gets the run ahead parser.
        /// </summary>
        /// <value>
        ///     The run ahead parser.
        /// </value>
        protected XmlReader RunAheadParser
        {
            get
            {
                return this._runAheadParser;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [no series].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no series]; otherwise, <c>false</c>.
        /// </value>
        protected bool NoSeries
        {
            get
            {
                return this._noSeries;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="IDataReaderEngine.KeyablePosition" /> moved
        ///     back to -1)
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.CloseStreams();
            this._lastCall = LastCall.Null;
            this.GroupId = null;
            this._flatObs = null;
            try
            {
                this._parseInputStream = this.DataLocation.InputStream;
                this._parser = this._xmlReaderBuilder.Build(this._parseInputStream);
                this._runAheadParserInputStream = this.DataLocation.InputStream;
                this._runAheadParser = this._xmlReaderBuilder.Build(this._runAheadParserInputStream);
                IHeaderRetrievalManager headerRetrievalManager = new DataHeaderRetrievalManager(this._parser, this.DefaultDsd);
                this.Header = headerRetrievalManager.Header;
                string dimensionAll = DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;

                // .NET Implementation note. Because we need to share the ProcessHeader with CrossSectionalDataReaderEngine
                this._noSeries = this.Header.Structures.Any(reference => string.Equals(reference.DimensionAtObservation, dimensionAll));
            }
            catch (XmlException e)
            {
                _log.Error("While reseting the reader and processing the header", e);
                this.Close();
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the component identifier.
        /// </summary>
        /// <param name="componentId">
        /// The component identifier.
        /// </param>
        /// <returns>
        /// The component ID.
        /// </returns>
        protected string GetComponentId(string componentId)
        {
            if (this.IsTwoPointOne)
            {
                return componentId;
            }

            string actualComponentId;
            if (this._conceptToComponentId.TryGetValue(componentId, out actualComponentId))
            {
                return actualComponentId;
            }

            return componentId;
        }

        /// <summary>
        ///     The lazy load key.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected override IKeyable LazyLoadKey()
        {
            try
            {
                // If the last call was to read the next series, then we are reading the one after that, so move on,
                // Otherwise, if we're not on a series, group, or obs as series, move on
                if (this._lastCall == LastCall.NextSeries
                    || (this.DatasetPositionInternal != Api.Constants.DatasetPosition.Series && this.DatasetPositionInternal != Api.Constants.DatasetPosition.Group
                        && this.DatasetPositionInternal != Api.Constants.DatasetPosition.ObservationAsSeries))
                {
                    if (!this.MoveNextKeyable())
                    {
                        return null;
                    }
                }

                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Group)
                {
                    IKeyable key = this.ProcessGroupNode();
                    if (_log.IsDebugEnabled)
                    {
                        _log.DebugFormat("Read Key {0}", key);
                    }

                    return key;
                }
                else if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Series || this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
                {
                    var key = this.ProcessSeriesNode();
                    this._flatObs = this.CurrentObs; // STORE THE OBSERVATION LOCALLY IN THIS CLASS THAT WAS CREATED AS PART OF GENERATING THE SERIES
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug(key);
                    }

                    return key;
                }

                return null;
            }
            catch (XmlException e)
            {
                throw new SdmxException("Unrecoverable error while reading SDMX data", e);
            }
            finally
            {
                this._lastCall = LastCall.NextSeries;
            }
        }

        /// <summary>
        ///     The lazy load observation.
        /// </summary>
        /// <returns>
        ///     The <see cref="IObservation" />.
        /// </returns>
        protected override IObservation LazyLoadObservation()
        {
            try
            {
                // There are no more observations, or even series return null
                if (!this.HasNext)
                {
                    return null;
                }

                // Series and Observation are on the same node (2.1
                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
                {
                    // The current node is a series and obs node as one, the last call was to get the next observation, there is only one so this time we return null
                    if (this._lastCall == LastCall.NextObs)
                    {
                        return null;
                    }

                    // The current node is a series and obs node as one, the last call was not to process the next series but to move onto it, so we will process it now, to get the next observation
                    if (this._lastCall == LastCall.HasNextSeries)
                    {
                        this.ProcessSeriesNode();
                    }

                    // The series has now been processed, return the observation that resulted from that reading
                    return this._flatObs;
                }

                // If the user has not yet called has next observation, then make a call now to check and return the next obs, if there is one
                if (this._lastCall != LastCall.HasNextObs)
                {
                    if (this.MoveNextObservation())
                    {
                        return this.ProcessObsNode(this.Parser);
                    }

                    return null;
                }

                // The last call was has next observation, so make sure, the dataset position is on the observation node (i.e - there was a next one), and process it
                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Observation)
                {
                    return this.ProcessObsNode(this.Parser);
                }

                // The last call was has next obs, and it returned false, so this call returns null
                return null;
            }
            catch (XmlException e)
            {
                throw new SdmxException("Unrecoverable error while reading SDMX data", e);
            }
            finally
            {
                this._lastCall = LastCall.NextObs;
            }
        }

        /// <summary>
        ///     Moves the next dataset internal.
        /// </summary>
        /// <returns>
        ///     True if there is another <c>Dataset</c>; otherwise false.
        /// </returns>
        protected override bool MoveNextDatasetInternal()
        {
            try
            {
                // If a check was made to hasNextSeries, the parser may have moved forward to the next dataset,
                if (this._lastCall != LastCall.HasNextDataSet && this.DatasetPositionInternal == Api.Constants.DatasetPosition.Dataset)
                {
                    return true;
                }

                // Set the dataset to null at this point, as it may be recreated
                this.DatasetHeader = null;
                while (this.Next(false))
                {
                    if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Dataset)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (XmlException e)
            {
                throw new SdmxException("Unrecoverable error while reading SDMX data", e);
            }
            finally
            {
                this._lastCall = LastCall.HasNextDataSet;
            }
        }

        /// <summary>
        ///     Moves the next key-able (internal).
        /// </summary>
        /// <returns>
        ///     True if there is another <c>Key-able</c>; otherwise false.
        /// </returns>
        protected override bool MoveNextKeyableInternal()
        {
            try
            {
                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Dataset && this._lastCall != LastCall.HasNextDataSet)
                {
                    return false;
                }

                // If a check was made to hasNextObservation, the parser may have moved forward to the next series as obs,
                // and this may not yet have been processed, in which case check to see if a call has been made to hasNextKeyable since the last 
                // call to hasNextObservation
                if (this._lastCall == LastCall.HasNextObs
                    && (this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries || this.DatasetPositionInternal == Api.Constants.DatasetPosition.Series
                        || this.DatasetPositionInternal == Api.Constants.DatasetPosition.Group))
                {
                    return true;
                }

                while (this.Next(false))
                {
                    if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Series || this.DatasetPositionInternal == Api.Constants.DatasetPosition.Group
                        || this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
                    {
                        return true;
                    }

                    if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Dataset)
                    {
                        return false;
                    }
                }

                return false;
            }
            catch (XmlException e)
            {
                throw new SdmxException(innerException: e, errorMessage: "Unrecoverable error while reading SDMX data");
            }
            catch (SdmxException e)
            {
                throw new SdmxException(innerException: e, errorMessage: "Error while attempting to read key");
            }
            finally
            {
                this._lastCall = LastCall.HasNextSeries;
            }
        }

        /// <summary>
        ///     Moves the next observation internal.
        /// </summary>
        /// <returns>
        ///     True if there is another <c>observation</c>; otherwise false.
        /// </returns>
        protected override bool MoveNextObservationInternal()
        {
            try
            {
                this.CurrentObs = null; // Set the current observation to null, this is so when the user reads the observation it can generate it on demand 

                // If we are at the end of the file, then return false, there is no point in processing anything
                if (!this.HasNext)
                {
                    return false;
                }

                // If the dataset position is an observation that is also acting as a series, then the following rules apply;
                // 1. The user has made a call on hasNextSeries, this has returned true, but they have not processed the series, in this case, we need to process the series to extract the observation
                // 2. The user's last call was readNextSeries, in which case there will be an observation, as the series IS also the observation
                // 3. The user's last call was hasNextObservation, or readNextObs in both cases we return false, as there is only one observation here 
                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
                {
                    if (this._lastCall == LastCall.HasNextSeries)
                    {
                        this.ProcessSeriesNode();
                        return true;
                    }

                    if (this._lastCall == LastCall.NextSeries)
                    {
                        return true;
                    }

                    // We have read the next obs, or already made this call, set the position to null and return false
                    this.DatasetPositionInternal = Api.Constants.DatasetPosition.Null;
                    return false;
                }

                if (this.Next(true))
                {
                    return this.DatasetPositionInternal == Api.Constants.DatasetPosition.Observation;
                }

                return false;
            }
            catch (XmlException e)
            {
                throw new SdmxException("Unrecoverable error while reading SDMX data", e);
            }
            catch (SdmxException e)
            {
                throw new SdmxException("Error while attempting to read observation", e);
            }
            finally
            {
                this._lastCall = LastCall.HasNextObs;
            }
        }

        /// <summary>
        /// Move to the next OBS.
        /// </summary>
        /// <param name="includeObs">
        /// if set to <c>true</c> [include OBS].
        /// </param>
        /// <returns>
        /// True if it successfully moves to the next OBS; otherwise false;
        /// </returns>
        protected abstract bool Next(bool includeObs);

        /// <summary>
        ///     Processes the group node.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected abstract IKeyable ProcessGroupNode();

        /// <summary>
        /// Processes the OBS node.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IObservation"/>.
        /// </returns>
        protected abstract IObservation ProcessObsNode(XmlReader parser);

        /// <summary>
        ///     Processes the series node.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected abstract IKeyable ProcessSeriesNode();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.</param>
        protected override void Dispose(bool managed)
        {
            if (managed)
            {
                this.CloseStreams();
                if (this.DataLocation != null)
                {
                    this.DataLocation.Close();
                    this.DataLocation = null;
                }
            }

            base.Dispose(managed);
        }

        /// <summary>
        /// Sets the current DSD.
        /// </summary>
        /// <param name="currentDsd">
        /// The current DSD.
        /// </param>
        protected override void SetCurrentDsd(IDataStructureObject currentDsd)
        {
            base.SetCurrentDsd(currentDsd);
            foreach (var component in currentDsd.Components)
            {
                this._conceptToComponentId[component.ConceptRef.FullId] = component.Id;
            }
        }

        /// <summary>
        ///     Closes the streams.
        /// </summary>
        private void CloseStreams()
        {
            if (this.Parser != null)
            {
                this.Parser.Close();
            }

            if (this._parseInputStream != null)
            {
                this._parseInputStream.Close();
            }

            if (this.RunAheadParser != null)
            {
                this.RunAheadParser.Close();
            }

            if (this._runAheadParserInputStream != null)
            {
                this._runAheadParserInputStream.Close();
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-07-16
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Data reader of data in Cross Sectional format.
//   The Reader is expected to be used by calling first
//   <see cref="MoveNextDataSet"/>(), then <see cref="MoveNextKeyable"/>() , getNextKey(), then for each <c>Keyable</c> <see cref="MoveNextObservation"/>(),
//   getNextObservation()
//   e.g.
//   <code lang="csharp">
//   IDataReaderEngine reader = new CrossSectionalDataReaderEngine(datalocation, dsd, dataflow);
//   reader.MoveNextDataSet();
//   while (reader.MoveNextKeyable()) {
//   IKeyable key = GetNextKey();
//   use key ...
//   while (reader.MoveNextObservation()) {
//   IObservation obs = reader.GetNextObservation();
//   use observation ...
//   }
//   }
//   </code>
//   Ported/Based on the Java version CrossSectionalDataReaderEngine written by Mihaela Munteanu.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;
    using Estat.Sri.SdmxXmlConstants.Builder;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     Data reader of data in Cross Sectional format.
    ///     The Reader is expected to be used by calling first
    ///     <see cref="MoveNextDataset"/>(), then <see cref="MoveNextKeyable"/>() , getNextKey(), then for each <see cref="IKeyable"/> <see cref="MoveNextObservation"/>(),
    ///     getNextObservation()
    ///     e.g.
    ///     <code lang="csharp">
    ///       IDataReaderEngine reader = new CrossSectionalDataReaderEngine(datalocation, dsd, dataflow);
    ///       reader.MoveNextDataSet();
    ///       while (reader.MoveNextKeyable()) {
    ///        IKeyable key = GetNextKey();
    ///        //use key ...
    ///        while (reader.MoveNextObservation()) {
    ///           IObservation obs = reader.GetNextObservation();
    ///           //use observation ...
    ///        }
    ///     }  
    ///  </code>
    ///     Ported/Based on the Java version CrossSectionalDataReaderEngine written by <c>Mihaela Munteanu</c>. 
    /// </summary>
    public class CrossSectionalDataReaderEngine : IDataReaderEngine
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(CrossSectionalDataReaderEngine));

        #endregion

        #region Fields
        /// <summary> The measure dimensions concepts available from DSD</summary>
        private readonly List<string> _availableCrossSectionalMeasures = new List<string>();

        /// <summary>
        ///     The concept automatic component unique identifier
        /// </summary>
        private readonly IDictionary<string, string> _conceptToComponentId = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///     List with available dimensions from the DSD.
        ///     If an attribute from item tag is not in this list it is considered an attribute of the artefact.
        /// </summary>
        private readonly List<string> _availableDimensions = new List<string>();

        /// <summary> The measure dimensions concepts available from DSD</summary>
        private readonly List<string> _availableMeasureDimensions = new List<string>();

        /// <summary> Dictionary with DataSet Attributes e.g. ("TAB_NUM","RQFI05V1"), ("REV_NUM", "1") </summary>
        private readonly Dictionary<string, string> _dataSetAttributes = new Dictionary<string, string>();

        /// <summary> Dictionary with DataSet concepts and values if there are any </summary>
        private readonly Dictionary<string, string> _dataSetConcepts = new Dictionary<string, string>();

        /// <summary> Dictionary with Group Attributes if there are any </summary>
        private readonly Dictionary<string, string> _groupAttributes = new Dictionary<string, string>();

        /// <summary> Dictionary with Group concepts and values e.g. ("FREQ","A"), ("COUNTRY","DK") </summary>
        private readonly Dictionary<string, string> _groupConcepts = new Dictionary<string, string>();

        /// <summary> Dictionary with Observation Attributes e.g. ("OBS_STATUS","A") </summary>
        private readonly Dictionary<string, string> _observationAttributes = new Dictionary<string, string>();

        /// <summary> Dictionary with Observation concepts and values e.g. ("SEX", "F"), ("CAS", "003" ) </summary>
        private readonly Dictionary<string, string> _observationConcepts = new Dictionary<string, string>();

        /// <summary>
        /// The _XML builder
        /// </summary>
        private readonly IXmlReaderBuilder _xmlBuilder;

        /// <summary> Dictionary with Section Attributes e.g. (UNIT_MULT,"0"), (DECI,"0"), (UNIT,"PERS") </summary>
        private readonly Dictionary<string, string> _sectionAttributes = new Dictionary<string, string>();

        /// <summary> Dictionary with Section concepts and values e.g. ("FREQ","A"), ("COUNTRY","DK") </summary>
        private readonly Dictionary<string, string> _sectionConcepts = new Dictionary<string, string>();

        /// <summary> The given Dataflow </summary>
        private readonly IDataflowObject _dataflow;

        /// <summary> The given Data Structure </summary>
        private readonly ICrossSectionalDataStructureObject _defaultDsd;

        /// <summary>
        ///     The _object retrieval.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _objectRetrieval;

        /// <summary> Stream Reader used for getting the current Items </summary>
        private XmlReader _parser;

        /// <summary> Input Stream used for checking ahead moveNext Items </summary>
        private Stream _runAheadInputStream;

        /// <summary> Stream Reader for checking ahead moveNext Items </summary>
        private XmlReader _runAheadParser;

        /// <summary> The primary measure dimension </summary>
        private string _primaryMeasure;

        /// <summary>
        ///     The header bean.
        /// </summary>
        private IHeader _headerObject;

        /// <summary> Input Stream used for getting the current Items </summary>
        private Stream _inputStream;

        /// <summary>
        ///     If there are Cross Sectional measures then observation tag looks like this: <PJANT value="34444" SEX="F" />
        /// </summary>
        private string _crossSectionalObservation;

        /// <summary>
        ///     The current key.
        /// </summary>
        private IKeyable _currentKey;

        /// <summary>
        /// The _current observation.
        /// </summary>
        private IObservation _currentObs;

        /// <summary> The Readable source to be parsed </summary>
        private IReadableDataLocation _dataLocation;

        /// <summary> Flag to see if the Observation is built when <c>Keyable</c> is processed </summary>
        private bool _observationSetFromKeyable;

        /// <summary> The value of observation as it is read from parser </summary>
        private string _observationValue;

        /// <summary>
        /// The _time concept.
        /// </summary>
        private string _timeDimensionId = string.Empty;

        /// <summary>
        /// The OBS time
        /// </summary>
        private string _obsTime;

        /// <summary>
        /// The _disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The _dataset header
        /// </summary>
        private IDatasetHeader _datasetHeader;

        /// <summary>
        /// The _current DSD
        /// </summary>
        private ICrossSectionalDataStructureObject _currentDsd;

        /// <summary>
        /// The _current dataflow
        /// </summary>
        private IDataflowObject _currentDataflow;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <param name="dsd">
        /// The DSD. giving the ability to retrieve DSDs for the datasets this reader engine is reading.  This
        ///     can be null if there is only one relevant DSD - in which case the default DSD should be provided
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        public CrossSectionalDataReaderEngine(IReadableDataLocation dataLocation, ICrossSectionalDataStructureObject dsd, IDataflowObject dataflow)
            : this(dataLocation, null, dsd, dataflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <param name="objectRetrieval">
        /// The object retrieval.
        /// </param>
        /// <param name="defaultDsd">
        /// The DSD. giving the ability to retrieve DSDs for the datasets this reader engine is reading.  This
        ///     can be null if there is only one relevant DSD - in which case the default DSD should be provided
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        public CrossSectionalDataReaderEngine(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager objectRetrieval, ICrossSectionalDataStructureObject defaultDsd, IDataflowObject dataflow)
        {
            this._xmlBuilder = new XmlReaderBuilder();
            if (objectRetrieval == null && defaultDsd == null)
            {
                throw new ArgumentException("AbstractDataReaderEngine expects either a ISdmxObjectRetrievalManager or a IDataStructureObject to be able to interpret the structures");
            }

            this._objectRetrieval = objectRetrieval;
            this._dataLocation = dataLocation;
            this._defaultDsd = defaultDsd;
            this._dataflow = dataflow;

            this.Reset();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the header information for the current dataset.  This may contain references to the data structure, dataflow,
        ///     or provision agreement that this data is for
        /// </summary>
        /// <value> </value>
        public IDatasetHeader CurrentDatasetHeader
        {
            get
            {
                return this._datasetHeader;
            }
        }

        /// <summary>
        ///     Gets the current <see cref="IKeyable" /> entry in the dataset, if there has been no initial call to
        ///     <see cref="MoveNextKeyable"/>, then null will be returned.
        /// </summary>
        public IKeyable CurrentKey
        {
            get
            {
                return this._currentKey;
            }
        }

        /// <summary>
        ///     Gets the current Observation for the current Key-able.
        ///     <p />
        ///     Gets null if any of the following conditions are met:
        ///     <ul>
        ///         <li><see cref="IDataReaderEngine.CurrentKey" /> returns null</li>
        ///         <li><see cref="IDataReaderEngine.CurrentKey" /> returns a Key-able which defines a GroupKey</li>
        ///         <li>
        ///             <see cref="IDataReaderEngine.MoveNextKeyable" /> has been called with no subsequent call to
        ///             <see cref="IDataReaderEngine.MoveNextObservation" />
        ///         </li>
        ///         <li><see cref="IDataReaderEngine.MoveNextObservation" /> was called and returned false</li>
        ///     </ul>
        /// </summary>
        /// <value> the next observation value </value>
        public IObservation CurrentObservation
        {
            get
            {
                return this._currentObs;
            }
        }

        /// <summary>
        ///     Gets the data structure definition that this reader engine is currently reading data for
        ///     <p />
        ///     Note this will return null unless there has been a call to <see cref="MoveNextDataset"/>(), this KeyFamily returned by this
        ///     method call may change when reading a new dataset
        /// </summary>
        /// <value> </value>
        public IDataStructureObject DataStructure
        {
            get
            {
                return this._currentDsd;
            }
        }

        /// <summary>
        ///     Gets the dataflow that this reader engine is currently reading data for.
        ///     This is not guaranteed to return a DataflowBean, as it may be unknown or not applicable, in this case null will be
        ///     returned
        ///     Note this will return null unless there has been a call to <see cref="MoveNextDataset"/>(), this Dataflow returned by this method
        ///     call may change when reading a new dataset
        /// </summary>
        public IDataflowObject Dataflow
        {
            get
            {
                return this._currentDataflow;
            }
        }

        /// <summary>
        ///     Gets the attributes available for the current dataset
        /// </summary>
        /// <value> a copy of the list, returns an empty list if there are no dataset attributes </value>
        public IList<IKeyValue> DatasetAttributes
        {
            get
            {
                return new List<IKeyValue>(this._dataSetAttributes.Select(pair => new KeyValueImpl(pair.Value, pair.Key)));
            }
        }

        /// <summary>
        ///     Gets the current dataset index the iterator position is at within the data source.
        ///     <p />
        ///     Index starts at -1, (no datasets have been read)
        /// </summary>
        /// <value> </value>
        public int DatasetPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets the header of the data-source that this reader engine is reading data for.  The header is related to the
        ///     message and not an individual dataset
        /// </summary>
        /// <value> </value>
        public IHeader Header
        {
            get
            {
                return this._headerObject;
            }
        }

        /// <summary>
        ///     Gets the current <c>Keyable</c> index the iterator position is at within the Data Set
        ///     <p />
        ///     Index starts at -1 - (no Keys have been Read)
        /// </summary>
        public int KeyablePosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets the current Observation index the iterator position is at within the current <c>Keyable</c> being read.
        ///     <p />
        ///     Index starts at -1 (no observations have been read - meaning getCurrentObservation() will return null
        /// </summary>
        /// <value> </value>
        public int ObsPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets the provision agreement that this data is for.
        /// </summary>
        /// <value>
        ///     The provision agreement.
        /// </value>
        /// <remarks>
        ///     This is not guaranteed to return a ProvisionAgreementBean, as it may be unknown or not applicable, in this case
        ///     null will be returned
        ///     Note this will return null unless there has been a call to <see cref="IDataReaderEngine.MoveNextDataset" />, this
        ///     Provision Agreement returned by this method call may change when reading a new dataset
        /// </remarks>
        public IProvisionAgreementObject ProvisionAgreement
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes the reader engine, and releases all resources.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Copies the entire dataset that the reader is reading, to the output stream (irrespective of current position)
        /// </summary>
        /// <param name="outputStream">
        /// output stream to copy data to
        /// </param>
        public void CopyToOutputStream(Stream outputStream)
        {
            StreamUtil.CopyStream(this._dataLocation.InputStream, outputStream);
        }

        /// <summary>
        ///     Creates a copy of this data reader engine, the copy is another iterator over the same source data
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataReaderEngine" /> .
        /// </returns>
        public IDataReaderEngine CreateCopy()
        {
            return new CrossSectionalDataReaderEngine(this._dataLocation, this._defaultDsd, this._dataflow);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more datasets in the data source
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool MoveNextDataset()
        {
            return true;
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more keys in the dataset
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool MoveNextKeyable()
        {
            // TODO inherit AbstractDataReaderEngine
            try
            {
                this.ResetObservationFields();
                return this.MoveNextKeyableInternal();
            }
            catch (XmlException e)
            {
                throw new SdmxSyntaxException("Error while trying to get Next <c>Keyable</c> from data source", e);
            }
        }

        /// <summary>
        ///     If this reader is in a series, this will return true if the series has any more observation values.
        /// </summary>
        /// <returns> true if series has more observation values </returns>
        public bool MoveNextObservation()
        {
            try
            {
                return this.MoveNextObservationInternal();
            }
            catch (XmlException e)
            {
                throw new SdmxException("Error while trying to get Next Observation from data source", e);
            }
        }

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="IDataReaderEngine.KeyablePosition" /> moved
        ///     back to -1)
        /// </summary>
        public void Reset()
        {
            try
            {
                this._inputStream = this._dataLocation.InputStream;
                this._parser = this._xmlBuilder.Build(this._inputStream);
                this._runAheadInputStream = this._dataLocation.InputStream;
                this._runAheadParser = this._xmlBuilder.Build(this._runAheadInputStream);

                IHeaderRetrievalManager headerRetrievalManager = new DataHeaderRetrievalManager(this._parser, this._defaultDsd);
                this._headerObject = headerRetrievalManager.Header;
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool managed)
        {
            if (!this._disposed)
            {
                return;
            }

            if (managed)
            {
                this.CloseStreams();
                if (this._dataLocation != null)
                {
                    this._dataLocation.Close();
                    this._dataLocation = null;
                }
            }

            this._disposed = true;
        }

        /// <summary>
        ///     Moves the next <see cref="IKeyable"/>.
        /// </summary>
        /// <returns>true if there is a next <see cref="IKeyable"/>.</returns>
        protected bool MoveNextKeyableInternal()
        {
            while (this._runAheadParser.Read())
            {
                switch (this._runAheadParser.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string nodeName = this._runAheadParser.LocalName;

                            if (ElementNameTable.DataSet.Is(nodeName))
                            {
                                this._datasetHeader = new DatasetHeaderCore(this._runAheadParser, this.Header);
                                this.ProcessAttributes(this._dataSetConcepts, this._dataSetAttributes);
                                this.DetermineCurrentDataStructure();
                            }
                            else if (ElementNameTable.Group.Is(nodeName))
                            {
                                this.ProcessAttributes(this._groupConcepts, this._groupAttributes);
                            }
                            else if (ElementNameTable.Section.Is(nodeName))
                            {
                                this.ProcessAttributes(this._sectionConcepts, this._sectionAttributes);
                            }
                            else if (this.IsMeasureNode(nodeName))
                            {
                                this.ProcessObservation(nodeName);

                                this.BuildCurrentKey();
                                this.BuildCurrentObservation();
                                this._observationSetFromKeyable = true;
                                return true;
                            }
                        }

                        break;

                    case XmlNodeType.EndElement:
                        {
                            string nodeName = this._runAheadParser.LocalName;
                            if (this._observationValue != null)
                            {
                                if (ElementNameTable.Section.Is(nodeName) || ElementNameTable.Group.Is(nodeName) || ElementNameTable.DataSet.Is(nodeName))
                                {
                                    return false;
                                }
                            }

                            if (ElementNameTable.CrossSectionalData.Is(nodeName))
                            {
                                return false;
                            }
                        }

                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the current DSD.
        /// </summary>
        /// <param name="currentDsd">
        /// The current DSD.
        /// </param>
        protected void SetCurrentDsd(ICrossSectionalDataStructureObject currentDsd)
        {
            this._currentDsd = currentDsd;
            foreach (var component in currentDsd.Components)
            {
                this._conceptToComponentId[component.ConceptRef.FullId] = component.Id;
            }

            this._availableDimensions.AddRange(currentDsd.GetDimensions(SdmxStructureEnumType.Dimension).Select(dimension => dimension.Id));
            this._availableMeasureDimensions.AddRange(currentDsd.GetDimensions(SdmxStructureEnumType.MeasureDimension).Select(dimension => dimension.Id));
            this._availableCrossSectionalMeasures.AddRange(currentDsd.CrossSectionalMeasures.Select(measure => measure.Id));
            this._primaryMeasure = currentDsd.PrimaryMeasure.Id;
            if (currentDsd.TimeDimension != null)
            {
                this._timeDimensionId = currentDsd.TimeDimension.Id;
            }

            // If the current dataset header does not reference a DSD then amend it
            var datasetHeader = this._datasetHeader;
            if (datasetHeader != null && datasetHeader.DataStructureReference == null)
            {
                IDatasetStructureReference datasetStructureReference = new DatasetStructureReferenceCore(currentDsd.AsReference);
                this._datasetHeader = this._datasetHeader.ModifyDataStructureReference(datasetStructureReference);
            }
        }

        /// <summary>
        ///     The move next observation internal.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected bool MoveNextObservationInternal()
        {
            if (this._observationSetFromKeyable)
            {
                this._observationSetFromKeyable = false;
                return true;
            }

            if (this._crossSectionalObservation == null)
            {
                return false;
            }

            this.ResetObservationFields();
            while (this._runAheadParser.Read())
            {
                switch (this._runAheadParser.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string nodeName = this._runAheadParser.LocalName;
                            if (this.IsMeasureNode(nodeName))
                            {
                                this.ProcessObservation(nodeName);

                                this.BuildCurrentObservation();

                                return true;
                            }

                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            string nodeName = this._runAheadParser.LocalName;
                            if (this._observationValue != null && this.IsMeasureNode(nodeName))
                            {
                                return false;
                            }

                            if (ElementNameTable.Section.Is(nodeName) || ElementNameTable.Group.Is(nodeName) || ElementNameTable.DataSet.Is(nodeName)
                                || ElementNameTable.CrossSectionalData.Is(nodeName))
                            {
                                return false;
                            }
                        }

                        break;
                }

                this.BuildCurrentObservation();
            }

            return false;
        }

        /// <summary>
        ///     Determines the current data structure.
        /// </summary>
        /// <exception cref="SdmxNoResultsException">
        ///     Could not read dataset, the data set references dataflow '+errorString+' which could not be resolved
        ///     or
        ///     Could not read dataset, the data set references provision '+errorString+' which could not be resolved
        ///     or
        ///     Could not read dataset, the data set references dataflow '+errorString+' which could not be resolved
        ///     or
        ///     Can not read dataset, the data set references the DSD ' + errorString + ' which could not be resolved
        ///     or
        ///     Can not read dataset, the data set does no reference any data structures, and there was no default data structure
        ///     definition provided
        /// </exception>
        /// <exception cref="SdmxNotImplementedException">Can not write dataset for structure of type:  + sRef</exception>
        private void DetermineCurrentDataStructure()
        {
            // 1. Set the current DSD to null before trying to resolve it
            this._currentDsd = null;
            this._currentDataflow = null;

            IDatasetStructureReference datasetStructureReference = null;
            if (this._datasetHeader != null)
            {
                datasetStructureReference = this._datasetHeader.DataStructureReference;
                if (datasetStructureReference == null && this._headerObject != null && this._headerObject.Structures.Count == 1)
                {
                    datasetStructureReference = this._headerObject.Structures[0];
                }
            }

            if (datasetStructureReference != null)
            {
                var structureReference = datasetStructureReference.StructureReference;

                string errorString = structureReference.MaintainableReference.ToString();

                // Provision, Flow, DSD or MSD
                switch (structureReference.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.Dsd:
                        if (this._defaultDsd != null && structureReference.IsMatch(this._defaultDsd))
                        {
                            this.SetCurrentDsd(this._defaultDsd);
                        }
                        else if (this._objectRetrieval != null)
                        {
                            this.SetCurrentDsd(structureReference.MaintainableReference);
                        }

                        break;
                    case SdmxStructureEnumType.Dataflow:
                        if (this._objectRetrieval != null)
                        {
                            this._currentDataflow = this._objectRetrieval.GetMaintainableObject<IDataflowObject>(structureReference.MaintainableReference);
                            if (this._currentDataflow == null)
                            {
                                throw new SdmxNoResultsException("Could not read dataset, the data set references dataflow '" + errorString + "' which could not be resolved");
                            }

                            this.SetCurrentDsd(this._currentDataflow.DataStructureRef.MaintainableReference);
                        }
                        else
                        {
                            // Use the "header" values 
                            this.SetCurrentDsd(this._defaultDsd);
                            this._currentDataflow = this._dataflow;
                        }

                        break;
                    default:
                        throw new SdmxNotImplementedException("Can not write dataset for structure of type: " + structureReference);
                }

                if (this._currentDsd == null)
                {
                    throw new SdmxNoResultsException("Can not read dataset, the data set references the DSD '" + errorString + "' which could not be resolved");
                }
            }
            else if (this._defaultDsd == null)
            {
                throw new SdmxNoResultsException("Can not read dataset, the data set does no reference any data structures, and there was no default data structure definition provided");
            }
            else
            {
                if (this._dataflow != null)
                {
                    this.SetCurrentDataflow(this._dataflow);
                }
                else
                {
                    this.SetCurrentDsd(this._defaultDsd);
                }
            }
        }

        /// <summary>
        /// Sets the current DSD.
        /// </summary>
        /// <param name="dsdRef">
        /// The DSD preference.
        /// </param>
        /// <exception cref="SdmxNoResultsException">
        /// Can not read dataset, the data set references the DSD which could
        ///     not be resolved
        /// </exception>
        private void SetCurrentDsd(IMaintainableRefObject dsdRef)
        {
            var dsd = this._objectRetrieval.GetMaintainableObject<IDataStructureObject>(dsdRef) as ICrossSectionalDataStructureObject;
            if (dsd == null)
            {
                throw new SdmxNoResultsException("Can not read dataset, the data set references the DSD '" + dsdRef + "' which could not be resolved");
            }

            this.SetCurrentDsd(dsd);
        }

        /// <summary>
        /// Sets the current dataflow.
        /// </summary>
        /// <param name="currentDataflow">
        /// The current dataflow.
        /// </param>
        private void SetCurrentDataflow(IDataflowObject currentDataflow)
        {
            this._currentDataflow = currentDataflow;

            // If the current dataset header does not reference a Dataflow then amend it
            if (this._datasetHeader == null)
            {
                this._datasetHeader = new DatasetHeaderCore(Guid.NewGuid().ToString(), DatasetActionEnumType.Information, new DatasetStructureReferenceCore(currentDataflow.AsReference));
            }
            else if (this._datasetHeader.DataProviderReference == null)
            {
                IDatasetStructureReference datasetStructureReference = new DatasetStructureReferenceCore(currentDataflow.AsReference);
                this._datasetHeader = this._datasetHeader.ModifyDataStructureReference(datasetStructureReference);
            }

            if (this._defaultDsd != null && this._defaultDsd.Urn.Equals(currentDataflow.DataStructureRef.TargetUrn))
            {
                this.SetCurrentDsd(this._defaultDsd);
            }
            else if (this._objectRetrieval != null)
            {
                this.SetCurrentDsd(this._objectRetrieval.GetMaintainableObject<IDataStructureObject>(currentDataflow.DataStructureRef.MaintainableReference) as ICrossSectionalDataStructureObject);
            }
        }

        /// <summary>
        ///     Builds the current key.
        /// </summary>
        private void BuildCurrentKey()
        {
            var conceptKeys = new List<IKeyValue>();
            foreach (var key in this._dataSetConcepts)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                conceptKeys.Add(keyValue);
            }

            foreach (var key in this._groupConcepts)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                conceptKeys.Add(keyValue);
            }

            foreach (var key in this._sectionConcepts)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                conceptKeys.Add(keyValue);
            }

            foreach (var key in this._observationConcepts)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                conceptKeys.Add(keyValue);
            }

            var attributeKeys = new List<IKeyValue>();
            foreach (var key in this._dataSetAttributes)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                attributeKeys.Add(keyValue);
            }

            foreach (var key in this._groupAttributes)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                attributeKeys.Add(keyValue);
            }

            foreach (var key in this._sectionAttributes)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                attributeKeys.Add(keyValue);
            }

            if (!string.IsNullOrWhiteSpace(this._timeDimensionId) && !string.IsNullOrWhiteSpace(this._obsTime))
            {
                TimeFormat timeFormat = DateUtil.GetTimeFormatOfDate(this._obsTime);
                this._currentKey = new KeyableImpl(this._dataflow, this._currentDsd, conceptKeys, attributeKeys, timeFormat, this._timeDimensionId, this._obsTime);
            }
            else
            {
                this._currentKey = new KeyableImpl(this._dataflow, this._currentDsd, conceptKeys, attributeKeys, string.Empty);
            }
        }

        /// <summary>
        ///     Builds the current observation.
        /// </summary>
        private void BuildCurrentObservation()
        {
            var attributes = new List<IKeyValue>();

            foreach (var key in this._observationAttributes)
            {
                IKeyValue keyValue = new KeyValueImpl(key.Value, key.Key);
                attributes.Add(keyValue);
            }

            IKeyValue crossSectionValue = null;
            if (this._crossSectionalObservation != null)
            {
                crossSectionValue = new KeyValueImpl(this._crossSectionalObservation, this._availableMeasureDimensions[0]);
            }

            this._currentObs = new ObservationImpl(this._currentKey, this._currentKey.ObsTime, this._observationValue, attributes, crossSectionValue);
        }

        /// <summary>
        ///     Closes the streams.
        /// </summary>
        private void CloseStreams()
        {
            if (this._parser != null)
            {
                this._parser.Close();
            }

            if (this._inputStream != null)
            {
                this._inputStream.Close();
            }

            if (this._runAheadParser != null)
            {
                this._runAheadParser.Close();
            }

            if (this._runAheadInputStream != null)
            {
                this._runAheadInputStream.Close();
            }
        }

        /// <summary>
        /// Determines whether the specified node name is a measure node.
        /// </summary>
        /// <param name="nodeName">
        /// Name of the node.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsMeasureNode(string nodeName)
        {
            return nodeName.Equals(this._primaryMeasure) || this._availableCrossSectionalMeasures.Contains(nodeName);
        }

        /// <summary>
        /// Processes the attributes.
        /// </summary>
        /// <param name="setConcepts">
        /// The set concepts.
        /// </param>
        /// <param name="setAttributes">
        /// The set attributes.
        /// </param>
        private void ProcessAttributes(IDictionary<string, string> setConcepts, IDictionary<string, string> setAttributes)
        {
            int attributesNumber = this._runAheadParser.AttributeCount;
            for (int i = 0; i < attributesNumber; i++)
            {
                this._runAheadParser.MoveToAttribute(i);
                var attribName = this.GetComponentId(this._runAheadParser.LocalName);
                var attribValue = this._runAheadParser.Value;
                if (this._availableDimensions.Contains(attribName))
                {
                    setConcepts.Add(attribName, attribValue);
                }
                else if (string.Equals(attribName, this._timeDimensionId))
                {
                    this._obsTime = attribValue;
                }
                else
                {
                    // TODO FIXME no check if it is an attribute
                    setAttributes.Add(attribName, attribValue);
                }
            }
        }

        /// <summary>
        /// Processes the observation.
        /// </summary>
        /// <param name="nodeName">
        /// Name of the node.
        /// </param>
        private void ProcessObservation(string nodeName)
        {
            int attributesNumber = this._runAheadParser.AttributeCount;
            for (int i = 0; i < attributesNumber; i++)
            {
                this._runAheadParser.MoveToAttribute(i);
                var attribName = this.GetComponentId(this._runAheadParser.LocalName);
                var attribValue = this._runAheadParser.Value;
                if (!AttributeNameTable.value.FastToString().Equals(attribName))
                {
                    if (this._availableDimensions.Contains(attribName))
                    {
                        this._observationConcepts.Add(attribName, attribValue);
                    }
                    else if (string.Equals(attribName, this._timeDimensionId))
                    {
                        this._obsTime = attribValue;
                    }
                    else
                    {
                        // TODO FIXME no check if it is an attribute
                        this._observationAttributes.Add(attribName, attribValue);
                    }
                }
                else
                {
                    // the value should be added as obs value
                    this._observationValue = attribValue;
                }
            }

            if (!nodeName.Equals(this._primaryMeasure))
            {
                this._observationConcepts.Add(this._availableMeasureDimensions[0], nodeName);
                this._crossSectionalObservation = nodeName;
            }
        }

        /// <summary>
        ///     Resets the observation fields.
        /// </summary>
        private void ResetObservationFields()
        {
            this._observationValue = null;
            this._crossSectionalObservation = null;
            this._observationConcepts.Clear();
            this._observationAttributes.Clear();
        }

        /// <summary>
        /// Gets the component identifier.
        /// </summary>
        /// <param name="componentId">
        /// The component identifier.
        /// </param>
        /// <returns>
        /// The component ID.
        /// </returns>
        private string GetComponentId(string componentId)
        {
            string actualComponentId;
            if (this._conceptToComponentId.TryGetValue(componentId, out actualComponentId))
            {
                return actualComponentId;
            }

            return componentId;
        }

        #endregion
    }
}
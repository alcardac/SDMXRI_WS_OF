// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-07-01
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The abstract data reader engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The abstract data reader engine.
    /// </summary>
    [Serializable]
    public abstract class AbstractDataReaderEngine : IDataReaderEngine
    {
        #region Fields

        /// <summary>
        ///     The default dataflow.
        /// </summary>
        private readonly IDataflowObject _defaultDataflow;

        /// <summary>
        ///     The default DSD.
        /// </summary>
        private readonly IDataStructureObject _defaultDsd;

        /// <summary>
        ///     The _object retrieval.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _objectRetrieval;

        /// <summary>
        ///     The _current dataflow.
        /// </summary>
        private IDataflowObject _currentDataflow;

        /// <summary>
        ///     The _current DSD.
        /// </summary>
        private IDataStructureObject _currentDsd;

        /// <summary>
        ///     The current key.
        /// </summary>
        private IKeyable _currentKey;

        /// <summary>
        ///     The current OBS.
        /// </summary>
        private IObservation _currentObs;

        /// <summary>
        ///     The current provision agreement.
        /// </summary>
        private IProvisionAgreementObject _currentProvisionAgreement;

        /// <summary>
        ///     The _data location.
        /// </summary>
        private IReadableDataLocation _dataLocation;

        /// <summary>
        ///     The _dataset header.
        /// </summary>
        private IDatasetHeader _datasetHeader;

        /// <summary>
        ///     The dataset index.
        /// </summary>
        private int _datasetIndex = -1;

        /// <summary>
        ///     The dataset position.
        /// </summary>
        private DatasetPosition _datasetPosition;

        /// <summary>
        ///     The has next.
        /// </summary>
        private bool _hasNext = true; // End of File

        /// <summary>
        ///     The has next OBS.
        /// </summary>
        private bool _hasNextObs = true; // End of Obs for current Key

        /// <summary>
        ///     The header bean.
        /// </summary>
        private IHeader _headerObject;

        /// <summary>
        ///     The key-able index.
        /// </summary>
        private int _keyableIndex = -1;

        /// <summary>
        ///     The OBS index.
        /// </summary>
        private int _obsIndex = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDataReaderEngine"/> class.
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
        protected AbstractDataReaderEngine(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager objectRetrieval, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
        {
            this._dataLocation = dataLocation;
            this._objectRetrieval = objectRetrieval;
            this._defaultDataflow = defaultDataflow;
            this._defaultDsd = defaultDsd;
            if (objectRetrieval == null && defaultDsd == null)
            {
                throw new ArgumentException("AbstractDataReaderEngine expects either a ISdmxObjectRetrievalManager or a IDataStructureObject to be able to interpret the structures");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the cross section concept.
        /// </summary>
        public string CrossSectionConcept
        {
            get
            {
                if (this._datasetHeader == null)
                {
                    return DimensionObject.TimeDimensionFixedId;
                }

                return this._datasetHeader.DataStructureReference.DimensionAtObservation;
            }
        }

        /// <summary>
        ///     Gets the header information for the current dataset.  This may contain references to the data structure,
        ///     dataflow, or provision agreement that this data is for
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
        ///     Gets the current Key-able entry in the dataset, if there has been no initial call to <see cref="MoveNextKeyable" />
        ///     , then
        ///     null will be returned.
        /// </summary>
        /// <value> </value>
        public IKeyable CurrentKey
        {
            get
            {
                if (this._currentKey != null)
                {
                    return this._currentKey;
                }

                if (this._keyableIndex < 0)
                {
                    return null;
                }

                this._currentKey = this.LazyLoadKey();
                return this._currentKey;
            }
        }

        /// <summary>
        ///     Gets the current Observation for the current Key-able.
        ///     <p />
        ///     Gets null if any of the following conditions are met:
        ///     <ul>
        ///         <li><see cref="CurrentKey" /> returns null</li>
        ///         <li><see cref="CurrentKey" /> returns a Key-able which defines a GroupKey</li>
        ///         <li>
        ///             <see cref="MoveNextKeyable" /> has been called with no subsequent call to
        ///             <see cref="MoveNextObservation" />
        ///         </li>
        ///         <li><see cref="MoveNextObservation" /> was called and returned false</li>
        ///     </ul>
        /// </summary>
        /// <value> the next observation value </value>
        public IObservation CurrentObservation
        {
            get
            {
                if (this._currentObs != null)
                {
                    return this._currentObs;
                }

                if (this._obsIndex < 0)
                {
                    return null;
                }

                this._currentObs = this.LazyLoadObservation();
                return this._currentObs;
            }
        }

        /// <summary>
        ///     Gets the data structure definition that this reader engine is currently reading data for
        ///     <p />
        ///     Note this will return null unless there has been a call to moveNextDataset(), this KeyFamily returned by this
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
        ///     Note this will return null unless there has been a call to moveNextDataset(), this Dataflow returned by this method
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
        public abstract IList<IKeyValue> DatasetAttributes { get; }

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
                return this._datasetIndex;
            }
        }

        /// <summary>
        ///     Gets or sets the header of the data source that this reader engine is reading data for.  The header is related to
        ///     the
        ///     message and not an individual dataset
        /// </summary>
        /// <value> </value>
        public IHeader Header
        {
            get
            {
                return this._headerObject;
            }

            protected set
            {
                this._headerObject = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether it is time series.
        /// </summary>
        public bool IsTimeSeries
        {
            get
            {
                return this.CrossSectionConcept.Equals(DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.Time).Value)
                       || this.CrossSectionConcept.Equals(DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value);
            }
        }

        /// <summary>
        ///     Gets the current Key-able index the iterator position is at within the Data Set
        ///     <p />
        ///     Index starts at -1 - (no Keys have been Read)
        /// </summary>
        public int KeyablePosition
        {
            get
            {
                return this._keyableIndex;
            }
        }

        /// <summary>
        ///     Gets the current Observation index the iterator position is at within the current Key-able being read.
        ///     <p />
        ///     Index starts at -1 (no observations have been read - meaning getCurrentObservation() will return null
        /// </summary>
        /// <value> </value>
        public int ObsPosition
        {
            get
            {
                return this._obsIndex;
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
                return this._currentProvisionAgreement;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current dataflow internal.
        /// </summary>
        /// <value>
        ///     The current dataflow internal.
        /// </value>
        protected IDataflowObject CurrentDataflowInternal
        {
            get
            {
                return this._currentDataflow;
            }
        }

        /// <summary>
        ///     Gets the current DSD internal.
        /// </summary>
        /// <value>
        ///     The current DSD internal.
        /// </value>
        protected IDataStructureObject CurrentDsdInternal
        {
            get
            {
                return this._currentDsd;
            }
        }

        /// <summary>
        ///     Gets or sets the current key value.
        /// </summary>
        /// <value>
        ///     The current key value.
        /// </value>
        protected IKeyable CurrentKeyValue
        {
            get
            {
                return this._currentKey;
            }

            set
            {
                this._currentKey = value;
            }
        }

        /// <summary>
        ///     Gets or sets the current observation.
        /// </summary>
        /// <value>
        ///     The current observation.
        /// </value>
        protected IObservation CurrentObs
        {
            get
            {
                return this._currentObs;
            }

            set
            {
                this._currentObs = value;
            }
        }

        /// <summary>
        ///     Gets or sets the data location.
        /// </summary>
        /// <value>
        ///     The data location.
        /// </value>
        protected IReadableDataLocation DataLocation
        {
            get
            {
                return this._dataLocation;
            }

            set
            {
                this._dataLocation = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset header.
        /// </summary>
        /// <value>
        ///     The dataset header.
        /// </value>
        protected IDatasetHeader DatasetHeader
        {
            get
            {
                return this._datasetHeader;
            }

            set
            {
                this._datasetHeader = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset position internal.
        /// </summary>
        /// <value>
        ///     The dataset position internal.
        /// </value>
        protected DatasetPosition DatasetPositionInternal
        {
            get
            {
                return this._datasetPosition;
            }

            set
            {
                this._datasetPosition = value;
            }
        }

        /// <summary>
        ///     Gets the default dataflow.
        /// </summary>
        /// <value>
        ///     The default dataflow.
        /// </value>
        protected IDataflowObject DefaultDataflow
        {
            get
            {
                return this._defaultDataflow;
            }
        }

        /// <summary>
        ///     Gets the default DSD.
        /// </summary>
        /// <value>
        ///     The default DSD.
        /// </value>
        protected IDataStructureObject DefaultDsd
        {
            get
            {
                return this._defaultDsd;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [has next].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [has next]; otherwise, <c>false</c>.
        /// </value>
        protected bool HasNext
        {
            get
            {
                return this._hasNext;
            }

            set
            {
                this._hasNext = value;
            }
        }

        /// <summary>
        ///     Gets the object retrieval.
        /// </summary>
        /// <value>
        ///     The object retrieval.
        /// </value>
        protected ISdmxObjectRetrievalManager ObjectRetrieval
        {
            get
            {
                return this._objectRetrieval;
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
        public abstract IDataReaderEngine CreateCopy();

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more datasets in the data source
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool MoveNextDataset()
        {
            this._currentKey = null;
            this._hasNextObs = true;
            this._obsIndex = -1;
            this._keyableIndex = -1;

            bool moveSuccessful = this.MoveNextDatasetInternal();
            if (moveSuccessful)
            {
                this._datasetIndex++;
                this.DetermineCurrentDataStructure();
            }

            return moveSuccessful;
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more keys in the dataset
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool MoveNextKeyable()
        {
            // If the dataset has not been read, then read it!
            if (this._datasetIndex == -1)
            {
                this.MoveNextDataset();
            }

            this._currentKey = null; // Set the current key to null, this is so when the user reads the observation it can generate it on demand 

            // If there is no more information left at all, then we return false
            if (!this._hasNext)
            {
                return false;
            }

            this._hasNextObs = true;
            this._obsIndex = -1;
            this._keyableIndex++;
            return this.MoveNextKeyableInternal();
        }

        /// <summary>
        ///     If this reader is in a series, this will return true if the series has any more observation values.
        /// </summary>
        /// <returns> true if series has more observation values </returns>
        public bool MoveNextObservation()
        {
            this._currentObs = null;

            // If we are at the end of the file, or the observations for the key, then return false, there is no point in processing anything
            if (!this._hasNext && !this._hasNextObs)
            {
                return false;
            }

            this._obsIndex++;

            if (this._currentKey == null)
            {
                this._currentKey = this.CurrentKey;
            }

            this._hasNextObs = this.MoveNextObservationInternal();
            return this._hasNextObs;
        }

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="IDataReaderEngine.KeyablePosition" /> moved
        ///     back to -1)
        /// </summary>
        public virtual void Reset()
        {
            this._hasNext = true;
            this._keyableIndex = -1;
            this._datasetIndex = -1;
            this._obsIndex = -1;
            this._currentObs = null;
            this._currentKey = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the <see cref="IKeyable"/> with the current dataflow and DSD.
        /// </summary>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <returns>
        /// The <see cref="IKeyable"/>.
        /// </returns>
        protected IKeyable CreateKeyable(IList<IKeyValue> keyValues, IList<IKeyValue> attributes, TimeFormat timeFormat)
        {
            return new KeyableImpl(this._currentDataflow, this._currentDsd, keyValues, attributes, timeFormat);
        }

        /// <summary>
        /// Creates the <see cref="IKeyable"/> with the current dataflow and DSD.
        /// </summary>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        /// <param name="id">
        /// The unique identifier.
        /// </param>
        /// <returns>
        /// The <see cref="IKeyable"/>.
        /// </returns>
        protected IKeyable CreateKeyable(IList<IKeyValue> keyValues, IList<IKeyValue> attributes, string id)
        {
            return new KeyableImpl(this._currentDataflow, this._currentDsd, keyValues, attributes, id);
        }

        /// <summary>
        /// Creates the <see cref="IKeyable"/> with the current dataflow and DSD.
        /// </summary>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <param name="timeValue">
        /// The time value.
        /// </param>
        /// <returns>
        /// The <see cref="IKeyable"/>.
        /// </returns>
        protected IKeyable CreateKeyable(IList<IKeyValue> keyValues, IList<IKeyValue> attributes, TimeFormat timeFormat, string timeValue)
        {
            return new KeyableImpl(this._currentDataflow, this._currentDsd, keyValues, attributes, timeFormat, this.CrossSectionConcept, timeValue);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool managed)
        {
        }

        /// <summary>
        ///     The lazy load key.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected abstract IKeyable LazyLoadKey();

        /// <summary>
        ///     The lazy load observation.
        /// </summary>
        /// <returns>
        ///     The <see cref="IObservation" />.
        /// </returns>
        protected abstract IObservation LazyLoadObservation();

        /// <summary>
        ///     Moves the next dataset internal.
        /// </summary>
        /// <returns>
        ///     True if there is another <c>Dataset</c>; otherwise false.
        /// </returns>
        protected abstract bool MoveNextDatasetInternal();

        /// <summary>
        ///     Moves the next key-able (internal).
        /// </summary>
        /// <returns>
        ///     True if there is another <c>Key-able</c>; otherwise false.
        /// </returns>
        protected abstract bool MoveNextKeyableInternal();

        /// <summary>
        ///     Moves the next observation internal.
        /// </summary>
        /// <returns>
        ///     True if there is another <c>observation</c>; otherwise false.
        /// </returns>
        protected abstract bool MoveNextObservationInternal();

        /// <summary>
        /// Sets the current DSD.
        /// </summary>
        /// <param name="currentDsd">
        /// The current DSD.
        /// </param>
        protected virtual void SetCurrentDsd(IDataStructureObject currentDsd)
        {
            this._currentDsd = currentDsd;

            // If the current dataset header does not reference a DSD then amend it
            if (this._datasetHeader.DataStructureReference == null)
            {
                IDatasetStructureReference datasetStructureReference = new DatasetStructureReferenceCore(currentDsd.AsReference);
                this._datasetHeader = this._datasetHeader.ModifyDataStructureReference(datasetStructureReference);
            }
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
            this._currentProvisionAgreement = null;

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
                        else if (this.ObjectRetrieval != null)
                        {
                            this.SetCurrentDsd(structureReference.MaintainableReference);
                        }

                        break;
                    case SdmxStructureEnumType.Dataflow:
                        if (this.ObjectRetrieval != null)
                        {
                            this._currentDataflow = this.ObjectRetrieval.GetMaintainableObject<IDataflowObject>(structureReference.MaintainableReference);
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
                            this._currentDataflow = this._defaultDataflow;
                        }

                        break;
                    case SdmxStructureEnumType.ProvisionAgreement:
                        if (this.ObjectRetrieval != null)
                        {
                            this._currentProvisionAgreement = this.ObjectRetrieval.GetMaintainableObject<IProvisionAgreementObject>(structureReference.MaintainableReference);
                            if (this._currentProvisionAgreement == null)
                            {
                                throw new SdmxNoResultsException("Could not read dataset, the data set references provision '" + errorString + "' which could not be resolved");
                            }

                            this._currentDataflow = this.ObjectRetrieval.GetMaintainableObject<IDataflowObject>(this._currentProvisionAgreement.StructureUseage.MaintainableReference);
                            if (this._currentDataflow == null)
                            {
                                throw new SdmxNoResultsException("Could not read dataset, the data set references dataflow '" + errorString + "' which could not be resolved");
                            }

                            this.SetCurrentDsd(this._currentDataflow.DataStructureRef.MaintainableReference);
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
                if (this._defaultDataflow != null)
                {
                    this.SetCurrentDataflow(this._defaultDataflow);
                }
                else
                {
                    this.SetCurrentDsd(this._defaultDsd);
                }
            }
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
            else if (this.ObjectRetrieval != null)
            {
                this.SetCurrentDsd(this.ObjectRetrieval.GetMaintainableObject<IDataStructureObject>(currentDataflow.DataStructureRef.MaintainableReference));
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
            var dsd = this.ObjectRetrieval.GetMaintainableObject<IDataStructureObject>(dsdRef);
            if (dsd == null)
            {
                throw new SdmxNoResultsException("Can not read dataset, the data set references the DSD '" + dsdRef + "' which could not be resolved");
            }

            this.SetCurrentDsd(dsd);
        }

        #endregion
    }
}
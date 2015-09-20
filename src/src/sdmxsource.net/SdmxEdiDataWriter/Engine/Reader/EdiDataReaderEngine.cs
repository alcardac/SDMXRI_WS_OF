// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX-EDI (aka GESMES/TS) data reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Extension;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;
    using Org.Sdmxsource.Sdmx.EdiParser.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The SDMX-EDI (aka GESMES/TS) data reader.
    /// </summary>
    public class EdiDataReaderEngine : IDataReaderEngine
    {
        #region Fields

        /// <summary>
        /// The bean retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _objectRetrievalManager;

        /// <summary>
        /// The dataflow bean.
        /// </summary>
        private readonly IDataflowObject _dataflowObject;

        /// <summary>
        /// The date map.
        /// </summary>
        private readonly IDictionary<string, string> _dateMap = new Dictionary<string, string>(StringComparer.Ordinal); // EDI date string to date

        /// <summary>
        /// The edi data readers.
        /// </summary>
        private readonly IList<IEdiDataReader> _ediDataReaders = new List<IEdiDataReader>();

        /// <summary>
        /// The header.
        /// </summary>
        private readonly IHeader _header;

        /// <summary>
        /// The time range cache.
        /// </summary>
        private readonly IDictionary<TimeFormat, CachedTimeRanges> _timeRangeCache = new Dictionary<TimeFormat, CachedTimeRanges>();

        /// <summary>
        ///     The _disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     The observation confidentiality concept
        /// </summary>
        private string _obsConf = EdiConstants.ObsConf;

        /// <summary>
        ///     The sibling group
        /// </summary>
        private string _siblingGroup = EDIUtil.SiblingGroupId;

        /// <summary>
        /// The current key.
        /// </summary>
        private IKeyable _currentKey;

        /// <summary>
        /// The current OBS.
        /// </summary>
        private IObservation _currentObs;

        /// <summary>
        /// The current OBS position.
        /// </summary>
        private int _currentObsPos = -1;

        /// <summary>
        /// The currently processing reader index.
        /// </summary>
        private int _currentlyProcessingReaderIndex = -1;

        /// <summary>
        /// The data structure object.
        /// </summary>
        private IDataStructureObject _dataStructureObject;

        /// <summary>
        /// The dataset attributes.
        /// </summary>
        private IList<IKeyValue> _datasetAttributes = new List<IKeyValue>();

        /// <summary>
        /// The dimensions.
        /// </summary>
        private string[] _dimensions;

        /// <summary>
        /// The EDI data reader.
        /// </summary>
        private IEdiDataReader _ediDataReader;

        /// <summary>
        /// The EDI time format.
        /// </summary>
        private EdiTimeFormat _ediTimeFormat;

        /// <summary>
        /// The has current key.
        /// </summary>
        private bool _hasCurrentKey;

        /// <summary>
        /// The has OBS confidentiality flag.
        /// </summary>
        private bool _hasObsConf;

        /// <summary>
        /// The has OBS PRE beak.
        /// </summary>
        private bool _hasObsPreBeak;

        /// <summary>
        /// The has OBS status.
        /// </summary>
        private bool _hasObsStatus;

        /// <summary>
        /// The in foot note section.
        /// </summary>
        private bool _inFootNoteSection;

        /// <summary>
        /// The key-able position.
        /// </summary>
        private int _keyablePosition = -1;

        /// <summary>
        ///     This is true when the reader has read the next key to see if it was the same as the previous
        ///     In this instance we do not want the user's call to <see cref="MoveNextKeyable"/> to actually do anything other then set this
        ///     back to false, as the next key has already been read
        /// </summary>
        private bool _lookedAhead;

        /// <summary>
        /// The missing value.
        /// </summary>
        private string _missingValue;

        /// <summary>
        /// The OBS dates.
        /// </summary>
        private IList<string> _obsDates;

        /// <summary>
        /// The observations.
        /// </summary>
        private string[] _observations;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDataReaderEngine"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="dataStructureObject">
        /// The data structure bean.
        /// </param>
        /// <param name="ediDataReaders">
        /// The EDI data readers.
        /// </param>
        public EdiDataReaderEngine(IHeader header, IDataflowObject dataflow, IDataStructureObject dataStructureObject, IList<IEdiDataReader> ediDataReaders)
        {
            this._header = header;
            this._dataStructureObject = dataStructureObject;
            this._dataflowObject = dataflow;
            this._ediDataReaders = ediDataReaders;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDataReaderEngine"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="dataStructureObject">
        /// The data Structure Bean.
        /// </param>
        /// <param name="ediDataReader">
        /// The edi Data Reader.
        /// </param>
        public EdiDataReaderEngine(IHeader header, IDataflowObject dataflow, IDataStructureObject dataStructureObject, IEdiDataReader ediDataReader)
        {
            this._header = header;
            this._dataStructureObject = dataStructureObject;
            this._dataflowObject = dataflow;
            this._ediDataReaders.Add(ediDataReader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDataReaderEngine"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="objectRetrievalManager">
        /// The bean retrieval manager.
        /// </param>
        /// <param name="ediDataReaders">
        /// The edi data readers.
        /// </param>
        public EdiDataReaderEngine(IHeader header, ISdmxObjectRetrievalManager objectRetrievalManager, IList<IEdiDataReader> ediDataReaders)
        {
            this._objectRetrievalManager = objectRetrievalManager;
            this._header = header;
            this._ediDataReaders = ediDataReaders;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the cross sectional concept.
        /// </summary>
        /// <value>
        ///     The cross sectional concept.
        /// </value>
        public string CrossSectionalConcept
        {
            get
            {
                return DimensionObject.TimeDimensionFixedId;
            }
        }

        /// <summary>
        ///     Gets the header information for the current dataset.  This may contain references to the data structure, dataflow,
        ///     or provision agreement that this data is for
        /// </summary>
        public IDatasetHeader CurrentDatasetHeader
        {
            get
            {
                if (this._ediDataReader == null)
                {
                    throw new InvalidOperationException("MoveNextDataset() not called");
                }

                return this._ediDataReader.DataSetHeaderObject;
            }
        }

        /// <summary>
        /// Gets the current Key-able entry in the dataset, if there has been no initial call to <see cref="MoveNextKeyable"/>, then null will
        /// be returned.
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
        ///     Note this will return null unless there has been a call to moveNextDataset(), this KeyFamily returned by this
        ///     method call may change when reading a new dataset
        /// </summary>
        /// <value> </value>
        public IDataStructureObject DataStructure
        {
            get
            {
                return this._dataStructureObject;
            }
        }

        /// <summary>
        ///     Gets the type of the data.
        /// </summary>
        /// <value>
        ///     The type of the data.
        /// </value>
        public DataType DataType
        {
            get
            {
                return DataType.GetFromEnum(DataEnumType.EdiTs);
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
                return this._dataflowObject;
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
                return new List<IKeyValue>(this._datasetAttributes);
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
                return this._currentlyProcessingReaderIndex;
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
                return this._header;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is time series.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is time series; otherwise, <c>false</c>.
        /// </value>
        public bool IsTimeSeries
        {
            get
            {
                return true;
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
                return this._keyablePosition;
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
                return this._currentObsPos;
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
            this._ediDataReader.CopyToStream(outputStream);
        }

        /// <summary>
        ///     Creates a copy of this data reader engine, the copy is another iterator over the same source data
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataReaderEngine" /> .
        /// </returns>
        public IDataReaderEngine CreateCopy()
        {
            if (this._objectRetrievalManager != null)
            {
                return new EdiDataReaderEngine(this._header, this._objectRetrievalManager, this._ediDataReaders);
            }

            return new EdiDataReaderEngine(this._header, this._dataflowObject, this._dataStructureObject, this._ediDataReaders);
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
            this._currentlyProcessingReaderIndex++;
            if (this._ediDataReaders.Count > this._currentlyProcessingReaderIndex)
            {
                this._ediDataReader = this._ediDataReaders[this._currentlyProcessingReaderIndex];
                this.Start();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating whether the there are any more keys in the dataset
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool MoveNextKeyable()
        {
            if (this._currentlyProcessingReaderIndex < 0)
            {
                if (!this.MoveNextDataset())
                {
                    return false;
                }
            }

            if (this._lookedAhead)
            {
                this._lookedAhead = false;
                return this._hasCurrentKey;
            }

            try
            {
                while (this._ediDataReader.MoveNext())
                {
                    this._keyablePosition++;
                    switch (this._ediDataReader.LineType)
                    {
                        case EdiPrefix.DatasetData:
                            this.ProcessEdiDataLine();
                            this._hasCurrentKey = true;
                            return true;
                        case EdiPrefix.DatasetFootnoteSection:
                            this._inFootNoteSection = true;
                            this.AssertMoveNext();
                            if (this.ProcessEDIAttributeSegment())
                            {
                                this._hasCurrentKey = true;
                                return true;
                            }

                            break;

                        case EdiPrefix.DatasetDataAttribute:
                            if (!this._inFootNoteSection)
                            {
                                throw new SdmxSemmanticException("Can not process attributes, no foot note section declared (FNS)");
                            }

                            this.ProcessEDIAttributeSegment();
                            this._hasCurrentKey = true;
                            return true;
                    }
                }

                this._hasCurrentKey = false;
                return false;
            }
            catch (Exception e)
            {
                if (this._ediDataReader.LineType == EdiPrefix.Null)
                {
                    throw new SdmxException("Error while processing EDI Segment (unknown prefix):  " + this._ediDataReader.CurrentLine, e);
                }

                throw new SdmxException("Error while processing EDI Segment:  " + this._ediDataReader.LineType.GetPrefix() + this._ediDataReader.CurrentLine, e);
            }
        }

        /// <summary>
        ///     If this reader is in a series, this will return true if the series has any more observation values.
        /// </summary>
        /// <returns> true if series has more observation values </returns>
        public bool MoveNextObservation()
        {
            if (this._inFootNoteSection)
            {
                return false;
            }

            this.ProcessEDIObservation();
            return this._currentObs != null;
        }

        /// <summary>
        ///     Moves the read position back to the start of the Data Set (<see cref="IDataReaderEngine.KeyablePosition" /> moved
        ///     back to -1)
        /// </summary>
        public void Reset()
        {
            this._currentlyProcessingReaderIndex = -1;
            this._ediDataReader = null;
            this._currentObsPos = -1;
            this._keyablePosition = -1;
            this._currentKey = null;
            this._currentObs = null;
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
                if (managed)
                {
                    foreach (var dataReader in this._ediDataReaders)
                    {
                        dataReader.Close();
                    }
                }

                this._disposed = true;
            }
        }

        /// <summary>
        ///     Asserts the move next.
        /// </summary>
        /// <exception cref="SdmxSyntaxException">Unexpected end of file <c>ediDataReader.CurrentLine</c> </exception>
        private void AssertMoveNext()
        {
            if (!this._ediDataReader.MoveNext())
            {
                throw new SdmxSyntaxException("Unexpected end of file" + this._ediDataReader.CurrentLine);
            }
        }

        /// <summary>
        /// Checks the attribute value validity.
        /// </summary>
        /// <param name="attributeConceptId">
        /// The attribute concept identifier.
        /// </param>
        /// <param name="attributeValue">
        /// The attribute value.
        /// </param>
        /// <param name="ediPrefix">
        /// The EDI prefix.
        /// </param>
        /// <exception cref="SdmxSyntaxException">
        /// Processing Un-coded Dataset Attribute (IDE+Z11) encountered illegal empty value for attribute:  + attributeConceptId
        ///     or
        ///     Processing Coded Dataset Attribute (IDE+Z10) encountered illegal empty code for attribute:  + attributeConceptId
        /// </exception>
        private void CheckAttributeValueValidity(string attributeConceptId, string attributeValue, EdiPrefix ediPrefix)
        {
            DatasetAction action = null;
            if (this.CurrentDatasetHeader != null)
            {
                action = this.CurrentDatasetHeader.Action;
            }

            if (action == null && this._header != null && this._header.Action != null)
            {
                action = this._header.Action;
            }

            // If the action is Not delete then the value must be legal.
            // Spaces are considered legal values 
            if (action != DatasetActionEnumType.Delete)
            {
                if (string.IsNullOrEmpty(attributeValue))
                {
                    if (ediPrefix.Equals(EdiPrefix.DatasetAttributeUncoded))
                    {
                        throw new SdmxSyntaxException("Processing Uncoded Dataset Attribute (IDE+Z11) encountered illegal empty value for attribute: " + attributeConceptId);
                    }

                    throw new SdmxSyntaxException("Processing Coded Dataset Attribute (IDE+Z10) encountered illegal empty code for attribute: " + attributeConceptId);
                }
            }
        }

        /// <summary>
        ///     Determines if a DSD has OBS status, observation confidentiality and
        ///     observation pre-break attributes present as observation level attributes
        /// </summary>
        private void DetermineObsAttributes()
        {
            this._hasObsStatus = this._dataStructureObject.GetObservationAttribute(EdiConstants.ObsStatus) != null;
            this._hasObsConf = this._dataStructureObject.GetObservationAttribute(this._obsConf) != null;
            this._hasObsPreBeak = this._dataStructureObject.GetObservationAttribute(EdiConstants.ObsPreBreak) != null;
        }

        /// <summary>
        ///     Obtains the data structure information.
        /// </summary>
        private void ObtainDataStructureInformation()
        {
            var dimList = new List<string>();
            foreach (var currentDimension in this._dataStructureObject.GetDimensions(SdmxStructureEnumType.Dimension))
            {
                dimList.Add(currentDimension.Id);
            }

            foreach (var group in this._dataStructureObject.Groups)
            {
                if (group.Id.StartsWith("sibling", StringComparison.OrdinalIgnoreCase))
                {
                    this._siblingGroup = group.Id;
                }
            }

            this._dimensions = dimList.ToArray();
            this.DetermineObsAttributes();
        }

        /// <summary>
        ///     Processes the attribute values.
        /// </summary>
        /// <returns>The list of attribute values.</returns>
        private IList<IKeyValue> ProcessAttributeValues()
        {
            var returnList = new List<IKeyValue>();
            while (true)
            {
                // Move to the coded (IDE+Z10)/Uncoded(IDE+Z11) attribute line
                this.AssertMoveNext();
                if (this._ediDataReader.LineType != EdiPrefix.DatasetAttributeCoded && this._ediDataReader.LineType != EdiPrefix.DatasetAttributeUncoded)
                {
                    this._ediDataReader.MoveBackLine();
                    returnList.Sort((keyValue1, keyValue2) => string.Compare(keyValue1.Concept, keyValue2.Concept, StringComparison.Ordinal));
                    return returnList;
                }

                string attributeConceptId = this._ediDataReader.CurrentLine;
                string attributeValue = null;
                if (this._ediDataReader.LineType == EdiPrefix.DatasetAttributeCoded)
                {
                    // Move to the code value Line
                    this.AssertMoveNext();

                    // If the current line is the attribute value then store it, otherwise
                    if (EDIUtil.AssertPrefix(this._ediDataReader, EdiPrefix.CodeValue, false))
                    {
                        attributeValue = this._ediDataReader.CurrentLine;
                    }
                    else
                    {
                        this._ediDataReader.MoveBackLine();
                    }

                    this.CheckAttributeValueValidity(attributeConceptId, attributeValue, EdiPrefix.DatasetAttributeCoded);
                }
                else if (this._ediDataReader.LineType == EdiPrefix.DatasetAttributeUncoded)
                {
                    string compositeValue = string.Empty;
                    while (true)
                    {
                        // Move to the next line and see if it is Free Text
                        this.AssertMoveNext();
                        if (EDIUtil.AssertPrefix(this._ediDataReader, EdiPrefix.String, false))
                        {
                            compositeValue += this._ediDataReader.ParseTextString();
                        }
                        else
                        {
                            break;
                        }
                    }

                    this.CheckAttributeValueValidity(attributeConceptId, compositeValue, EdiPrefix.DatasetAttributeUncoded);

                    attributeValue = compositeValue;
                    this._ediDataReader.MoveBackLine();
                }

                returnList.Add(new KeyValueImpl(attributeValue, attributeConceptId));
            }
        }

        /// <summary>
        ///     Processes the attribute segment
        /// </summary>
        /// <returns>
        ///     true if the attribute was not a dataset attribute.  If the attribute was a dataset attribute, then it can be
        ///     retrieved from the getDatasetAttribute method
        /// </returns>
        private bool ProcessEDIAttributeSegment()
        {
            // TODO Process FNS+ - current line, does this need any processing?

            // Move to the attribute scope line
            if (EDIUtil.AssertPrefix(this._ediDataReader, EdiPrefix.DatasetAttributeScope, false))
            {
                int scope = int.Parse(this._ediDataReader.CurrentLine, CultureInfo.InvariantCulture);

                // 1 = dataset, 4=mix of dimensions, 5=observation
                if (scope == 1)
                {
                    // This is a Dataset attribute which we want to ignore because this reader is passed all the dataset attribtues up front.  
                    // Move to the next line and return false to ensure this is not processed
                    this._ediDataReader.MoveNext();
                    EDIUtil.AssertPrefix(this._ediDataReader, EdiPrefix.DatasetDataAttribute, true);
                    return false;
                }
            }
            else
            {
                this._ediDataReader.MoveBackLine();
            }

            // Move to the dimension/key pointer line
            this.AssertMoveNext();
            EDIUtil.AssertPrefix(this._ediDataReader, EdiPrefix.DatasetDataAttribute, true);
            string currentLine = this._ediDataReader.CurrentLine;
            string[] posKeySplit = EDIUtil.SplitOnPlus(currentLine);

            if (posKeySplit.Length != 2)
            {
                // TODO Exception should be caught and full line + line position put on
                throw new SdmxSyntaxException("Can not parse current line '" + currentLine + "' expecting integer+key example 5+A:B:C:D");
            }

            // TODO These two attributes gives the key of the attribute attachment, if the key is not a full key then it is a group key and the group muse be 
            // determined
            // string lastDimensionPosition = posKeySplit[0];  //TODO What does this mean?  
            string key = posKeySplit[1];

            string[] keySplit = EDIUtil.SplitOnColon(key);
            IList<IKeyValue> keyableKey = new List<IKeyValue>();
            if (keySplit.Length < this._dimensions.Length)
            {
                throw new SdmxSemmanticException("Reported attributes '" + key + "' are less then key length '" + this._dimensions.Length + "' ");
            }

            bool isGroup = false;
            for (int i = 0; i < this._dimensions.Length; i++)
            {
                string val = keySplit[i];
                if (val.Length == 0)
                {
                    isGroup = true;
                }
                else
                {
                    keyableKey.Add(new KeyValueImpl(val, this._dimensions[i]));
                }
            }

            IList<IKeyValue> attributes = this.ProcessAttributeValues();
            this._currentObs = null;

            var obsDateAsSdmxString = this.BuildObsDateAsSdmxString(keySplit, key, isGroup);

            if (isGroup)
            {
                if (obsDateAsSdmxString != null)
                {
                    this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, keyableKey, null, this._siblingGroup);
                }
                else
                {
                    this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, keyableKey, attributes, this._siblingGroup);
                }
            }
            else
            {
                if (obsDateAsSdmxString != null)
                {
                    // TODO It is not 100% sure what ctor is used in Java. There is a warning there also.
                    this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, keyableKey, null);
                    this._currentObs = new ObservationImpl(this._currentKey, obsDateAsSdmxString, null, attributes);
                }
                else
                {
                    // TODO It is not 100% sure what ctor is used in Java. There is a warning there also.
                    this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, keyableKey, attributes);
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the OBS date as SDMX string.
        /// </summary>
        /// <param name="keySplit">The key split.</param>
        /// <param name="key">The key.</param>
        /// <param name="isGroup">if set to <c>true</c> is group.</param>
        /// <returns>the OBS date as SDMX string.</returns>
        /// <exception cref="SdmxSemmanticException">
        /// Reported attributes ' <c> key </c> ' unexpected information, expecting length ' <c> (this._dimensions.Length + 2 </c>' 
        /// or
        /// Illegal observation level attribute reported against a wild carded series key ' <c> key </c> '.  Observation attributes must be reported against the full key.
        /// </exception>
        private string BuildObsDateAsSdmxString(IList<string> keySplit, string key, bool isGroup)
        {
            string obsDateAsSdmxString = null;
            if (keySplit.Count > this._dimensions.Length)
            {
                if (keySplit.Count != this._dimensions.Length + 2)
                {
                    throw new SdmxSemmanticException("Reported attributes '" + key + "' unexpected information, expecting length '" + (this._dimensions.Length + 2) + "' ");
                }

                if (isGroup)
                {
                    throw new SdmxSemmanticException(
                        "Illegal observation level attribute reported against a wild carded series key '" + key + "'.  Observation attributes must be reported against the full key.");
                }

                string timeFormatString = keySplit[this._dimensions.Length + 1];
                string dateString = keySplit[this._dimensions.Length];
                EdiTimeFormat timeFormat = EdiTimeFormatExtension.ParseString(timeFormatString);
                DateTime obsDate = timeFormat.ParseDate(dateString);
                obsDateAsSdmxString = DateUtil.FormatDate(obsDate, timeFormat.GetSdmxTimeFormat());
            }

            return obsDateAsSdmxString;
        }

        /// <summary>
        ///     Processes the EDI observation.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        ///     No observation attribute ' <c> GesmesConstants.ObsStatus </c> ' present on DSD, but the data contains a value for
        ///     this attribute
        ///     or
        ///     No observation attribute ' <c> _obsConf </c> ' present on DSD, but the data contains a value for this attribute
        ///     or
        ///     No observation attribute ' <c> GesmesConstants.ObsPreBreak </c> ' present on DSD, but the data contains a value for
        ///     this attribute
        /// </exception>
        private void ProcessEDIObservation()
        {
            while (true)
            {
                this._currentObsPos++;
                this._currentObs = null;
                if (this._observations == null)
                {
                    if (this._obsDates.Count > this._currentObsPos)
                    {
                        // Observations is null, but this does not mean that there was not a reported date, or dates.  
                        // In this case, the observation is just a date, with no reported value.  This can be the case in delete messages
                        string obsDate = this._obsDates.Count > this._currentObsPos ? this._obsDates[this._currentObsPos] : null;
                        this._currentObs = new ObservationImpl(this._currentKey, obsDate, null, null);
                    }
                }
                else if (this._observations.Length > this._currentObsPos)
                {
                    string currentObsLine = this._observations[this._currentObsPos];

                    // 1. Set up variables
                    string obsDate = this._obsDates.Count > this._currentObsPos ? this._obsDates[this._currentObsPos] : null;
                    IList<IKeyValue> attributes = new List<IKeyValue>();

                    // If there is no observation, and no date, don't output anything.
                    if (!ObjectUtil.ValidString(currentObsLine))
                    {
                        if (!ObjectUtil.ValidString(obsDate))
                        {
                            this._currentObsPos++;
                            continue;
                        }
                    }

                    string[] obsArr = EDIUtil.SplitOnColon(currentObsLine);

                    // 0 = ObsValue
                    // 1 = ObsStatus
                    // 2 = ObsConf
                    // 3 = ObsPreBreak
                    string obsVal = obsArr[0];

                    bool obsStatusPresent = obsArr.Length > 1 && ObjectUtil.ValidString(obsArr[1]);
                    bool obsConfPresent = obsArr.Length > 2 && ObjectUtil.ValidString(obsArr[2]);
                    bool obsPreBreakPresent = obsArr.Length > 3 && ObjectUtil.ValidString(obsArr[3]);

                    if (obsStatusPresent)
                    {
                        if (this._hasObsStatus)
                        {
                            attributes.Add(new KeyValueImpl(obsArr[1], EdiConstants.ObsStatus));
                        }
                        else
                        {
                            throw new ArgumentException("No observation attribute '" + EdiConstants.ObsStatus + "' present on Dsd, but the data contains a value for this attribute");
                        }
                    }
                    else
                    {
                        // If there is no ObsStatus and no ObsValue skip to the next entry
                        if (!ObjectUtil.ValidString(obsVal))
                        {
                            continue;
                        }
                    }

                    if (obsConfPresent)
                    {
                        if (this._hasObsConf)
                        {
                            attributes.Add(new KeyValueImpl(obsArr[2], this._obsConf));
                        }
                        else
                        {
                            throw new ArgumentException("No observation attribute '" + this._obsConf + "' present on Dsd, but the data contains a value for this attribute");
                        }
                    }

                    if (obsPreBreakPresent)
                    {
                        if (this._hasObsPreBeak)
                        {
                            string obsAttr = obsArr[3].Equals(this._missingValue) ? SdmxConstants.MissingDataValue : obsArr[3];
                            attributes.Add(new KeyValueImpl(obsAttr, EdiConstants.ObsPreBreak));
                        }
                        else
                        {
                            throw new ArgumentException("No observation attribute '" + EdiConstants.ObsPreBreak + "' present on Dsd, but the data contains a value for this attribute");
                        }
                    }

                    if (obsVal == null)
                    {
                        obsVal = string.Empty;
                    }

                    if (obsVal.Equals(this._missingValue))
                    {
                        obsVal = SdmxConstants.MissingDataValue;
                    }

                    this._currentObs = new ObservationImpl(this._currentKey, obsDate, obsVal, attributes);
                }
                else
                {
                    // Check if the next key is the same as the last key
                    IKeyable prevKey = this._currentKey;
                    if (this.MoveNextKeyable())
                    {
                        if (this._inFootNoteSection)
                        {
                            this._lookedAhead = true;
                            break;
                        }

                        if (this._currentKey.Equals(prevKey))
                        {
                            continue;
                        }

                        this._lookedAhead = true;
                    }
                }

                break;
            }
        }

        /// <summary>
        ///     Processes the EDI data line.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">
        ///     Reported key ' <c> firstDataLine </c> ' is less then key length '
        ///     <c> dimensions.Length </c> '
        /// </exception>
        private void ProcessEdiDataLine()
        {
            this._currentObsPos = -1;

            // Observations becomes all of the observations for the series, unfortunately the first item in the array is the key & observation value
            // So we need to extract the first value and process it, and then replace the first value with the observation minus the key
            this._observations = EDIUtil.SplitOnPlus(this._ediDataReader.CurrentLine);
            string firstDataLine = this._observations[0];

            string[] dataLineSplit = EDIUtil.SplitOnColon(firstDataLine);

            // Get the series key from the ARR segment. 
            // Note: if this is a partial key, then it is assumed to be the deletion of the sibling group, as per the EDI Specification:
            // 2520 Deletion of a group of sibling series;
            // 2521 Rule: dates/periods/time ranges cannot be present in segment;
            // 2522 e.g. if :BE:XXX:YYY is the key of the sibling group (second position: frequency wildcarded: for
            // 2523 any frequency), then the segment
            // 2524 ARR++:BE:XXX:YYY'
            // 2525 implies the deletion of all series of the group (e.g. Q:BE:XXX:YYY and M:BE:XXX:YYY) and all
            // 2526 their attribute values at all
            var seriesKey = new List<IKeyValue>();
            if (dataLineSplit.Length < this._dimensions.Length)
            {
                throw new SdmxSemmanticException("Reported key '" + firstDataLine + "' is less then key length '" + this._dimensions.Length + "' ");
            }

            bool isGroup = false;
            for (int i = 0; i < this._dimensions.Length; i++)
            {
                string val = dataLineSplit[i];
                if (val.Length == 0)
                {
                    isGroup = true;
                }
                else
                {
                    seriesKey.Add(new KeyValueImpl(val, this._dimensions[i]));
                }
            }

            // observations[0] contained both the series key (or partial key) and the first observation, we want to extract from this ONLY the observation (not the series Key)
            // and store the in the first observation location observations[0] 
            this._observations[0] = null;
            string firstObservation = string.Empty;
            for (int i = this._dimensions.Length + 2; i < dataLineSplit.Length; i++)
            {
                firstObservation += dataLineSplit[i];
                if (dataLineSplit.Length > (i + 1))
                {
                    firstObservation += ":";
                }
            }

            if (ObjectUtil.ValidString(firstObservation))
            {
                this._observations[0] = firstObservation;
            }
            else
            {
                this._observations = null;
            }

            // At this point the observations array will have been populated. Validate it.
            this.ValidateObservation(this._observations);

            this.ProcessObservations(dataLineSplit);

            TimeFormat timeFormat = this._ediTimeFormat == EdiTimeFormat.None ? null : this._ediTimeFormat.GetSdmxTimeFormat();
            if (isGroup)
            {
                this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, seriesKey, null, this._siblingGroup);
            }
            else
            {
                this._currentKey = new KeyableImpl(this._dataflowObject, this._dataStructureObject, seriesKey, null, timeFormat);
            }
        }

        /// <summary>
        /// Processes the observation range.
        /// </summary>
        /// <param name="reportedDate">
        /// The reported date.
        /// </param>
        /// <exception cref="SdmxSyntaxException">
        /// More than one observation found for a ARR segment declaring a single time period
        ///     or
        ///     Expecting ' <c> obsDates.Count </c> ' observations for time range ' <c> reportedDate </c> ' in format '
        ///     <c> ediTimeFormat </c> ' but got ' <c> observations.Length </c> '
        ///     or
        ///     ARR segment declares time range requiring  <c> obsDates.Count </c>  observations, only
        ///     <c> observations.Length </c>  observations reported
        /// </exception>
        private void ProcessObservationRange(string reportedDate)
        {
            if (!this._ediTimeFormat.IsRange())
            {
                throw new SdmxSyntaxException("More than one observation found for a Arr segment declaring a single time period");
            }

            CachedTimeRanges timeRanges;
            TimeFormat sdmxTimeFormat = this._ediTimeFormat.GetSdmxTimeFormat();
            if (!this._timeRangeCache.TryGetValue(sdmxTimeFormat, out timeRanges))
            {
                timeRanges = new CachedTimeRanges();
                this._timeRangeCache.Add(sdmxTimeFormat, timeRanges);
            }

            IDictionary<string, IList<string>> timeRangeMap = timeRanges.TimeRangeMap;
            if (!timeRangeMap.TryGetValue(reportedDate, out this._obsDates))
            {
                DateTime startDate = this._ediTimeFormat.ParseDate(reportedDate);
                DateTime endDate = this._ediTimeFormat.ParseEndDate(reportedDate);
                this._obsDates = DateUtil.CreateTimeValues(startDate, endDate, sdmxTimeFormat);
                if (this._obsDates.Count != this._observations.Length)
                {
                    throw new SdmxSyntaxException(
                        "Expecting '" + this._obsDates.Count + "' observations for time range '" + reportedDate + "' in format '" + this._ediTimeFormat + "' but got '" + this._observations.Length + "'");
                }

                timeRangeMap.Add(reportedDate, this._obsDates);
            }

            if (this._obsDates.Count != this._observations.Length)
            {
                throw new SdmxSyntaxException("Arr segment declares time range requiring " + this._obsDates.Count + " observations, only " + this._observations.Length + " observations reported");
            }
        }

        /// <summary>
        /// Processes the observations by storing the observation dates in a list in the order in which the observations are
        ///     reported.
        ///     <p/>
        ///     If there are no observations to process (as this could be a delete message deleting a series, or a group of series)
        ///     then the <c>obsDates</c> list will be empty
        /// </summary>
        /// <param name="dataLineSplit">
        /// The data line split.
        /// </param>
        private void ProcessObservations(IList<string> dataLineSplit)
        {
            this._obsDates = new List<string>();
            if (dataLineSplit.Count > this._dimensions.Length)
            {
                string reportedDate = dataLineSplit[this._dimensions.Length];
                string dateFormat = dataLineSplit[this._dimensions.Length + 1];
                this._ediTimeFormat = EdiTimeFormatExtension.ParseString(dateFormat);

                // If there are no observations but we have a time range and this is a delete message,
                // then create observations with no value for each of the entries in the time range.
                if (this._observations == null && this._ediTimeFormat.IsRange() && this.CurrentDatasetHeader.Action.EnumType == DatasetActionEnumType.Delete)
                {
                    var startDate = this._ediTimeFormat.ParseDate(reportedDate);
                    var endDate = this._ediTimeFormat.ParseEndDate(reportedDate);
                    this._obsDates = DateUtil.CreateTimeValues(startDate, endDate, this._ediTimeFormat.GetSdmxTimeFormat());
                }
                else
                {
                    if (this._observations != null && this._observations.Length > 1)
                    {
                        this.ProcessObservationRange(reportedDate);
                    }
                    else
                    {
                        this.ProcessSingleObservation(reportedDate);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the single observation.
        /// </summary>
        /// <param name="reportedDate">
        /// The reported date.
        /// </param>
        private void ProcessSingleObservation(string reportedDate)
        {
            string mapKey = this._ediTimeFormat.GetEdiValue() + ":" + reportedDate;
            string item;
            if (this._dateMap.TryGetValue(mapKey, out item))
            {
                this._obsDates.Add(item);
            }
            else
            {
                DateTime startDate = this._ediTimeFormat.ParseDate(reportedDate);
                string parsedDate = DateUtil.FormatDate(startDate, this._ediTimeFormat.GetSdmxTimeFormat());
                this._obsDates.Add(parsedDate);
                this._dateMap.Add(mapKey, parsedDate);
            }
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        /// <exception cref="SdmxNoResultsException">
        ///     Can not read EDI Data File - Data Structure can not be found: +
        ///     maintainableRefObject
        /// </exception>
        private void Start()
        {
            this._ediDataReader = this._ediDataReaders[this._currentlyProcessingReaderIndex];
            this._ediDataReader.ResetReader();
            this._inFootNoteSection = false;
            this._missingValue = this._ediDataReader.MissingValue;
            IMaintainableRefObject maintainableRefObject = this._ediDataReader.DataSetHeaderObject.DataStructureReference.StructureReference.MaintainableReference;
            if (this._objectRetrievalManager != null)
            {
                this._dataStructureObject = this._objectRetrievalManager.GetMaintainableObject<IDataStructureObject>(maintainableRefObject);
            }

            if (this._dataStructureObject == null)
            {
                throw new SdmxNoResultsException("Can not read EDI Data File - Data Structure can not be found:" + maintainableRefObject);
            }

            // metadata specific convention ?
            if (this._dataStructureObject.GetAttribute(this._obsConf) == null)
            {
                if (this._dataStructureObject.GetAttribute("CONF_STATUS") != null)
                {
                    this._obsConf = "CONF_STATUS";
                }
            }

            this._datasetAttributes = this._ediDataReader.DatasetAttributes;
            this.ObtainDataStructureInformation();
        }

        /// <summary>
        /// Validates the observation.
        /// </summary>
        /// <param name="observations">
        /// The observations.
        /// </param>
        /// <exception cref="SdmxSyntaxException">
        /// Illegal observation value - it exceeds 15 characters. Observation value:  <c> aVal </c> . Line being processed:
        ///     <c> ediDataReader.CurrentLine </c>  within  currentKey
        ///     or
        ///     Illegal observation value. The observation:  <c> aVal </c>  is invalid. Observations must be valid numeric. Line
        ///     being processed: ediDataReader.CurrentLine
        /// </exception>
        private void ValidateObservation(IEnumerable<string> observations)
        {
            if (observations == null)
            {
                return;
            }

            foreach (var observation in observations)
            {
                // The values being passed in here are of the format:
                // 123:A
                // So everything after the first colon MUST be removed
                string value = EDIUtil.SplitOnColon(observation)[0];
                if (value.Length > 15)
                {
                    throw new SdmxSyntaxException(
                        "Illegal observation value - it exceeds 15 characters. Observation value: " + value + ". Line being processed: " + this._ediDataReader.CurrentLine + " within " + this._currentKey);
                }

                // Ensure that the value passed in is legal.
                // Legal values are: blank; a dash (indicates the absence of a value (EDI guide line 2035)); or a legal numeric.
                // Legal numerics means catering for decimals, negatives and the use of exponent (E)
                decimal obsValue;
                if (!(value.Equals("-") || value.Equals(string.Empty)) && !decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out obsValue))
                {
                    throw new SdmxSyntaxException(
                        "Illegal observation value. The observation: " + value + " is invalid. Observations must be valid numerics. Line being processed: " + this._ediDataReader.CurrentLine);
                }
            }
        }

        #endregion

        /// <summary>
        /// The cached time ranges.
        /// </summary>
        private class CachedTimeRanges
        {
            #region Fields

            /// <summary>
            /// The time range map.
            /// </summary>
            private readonly IDictionary<string, IList<string>> _timeRangeMap = new Dictionary<string, IList<string>>(); // Edi Time period map to dates

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the time range map.
            /// </summary>
            public IDictionary<string, IList<string>> TimeRangeMap
            {
                get
                {
                    return this._timeRangeMap;
                }
            }

            #endregion
        }
    }
}
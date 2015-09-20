// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesTimeSeriesWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Implementation of the DataWriter interface for writing SDMX-EDI data messages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Estat.Sri.SdmxEdiDataWriter.Constants;
    using Estat.Sri.SdmxEdiDataWriter.Helper;
    using Estat.Sri.SdmxEdiDataWriter.Model;
    using Estat.Sri.SdmxParseBase.Engine;
    using Estat.Sri.SdmxParseBase.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     Implementation of the DataWriter interface for writing SDMX-EDI data messages.
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="GesmesTimeSeriesWriter" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingGesmesWriter.cs" lang="cs" />
    /// </example>
    public class GesmesTimeSeriesWriter : IDataWriterEngine
    {
        #region Fields
        /// <summary>
        ///     A value indicating whether time range method should be used.
        /// </summary>
        private readonly bool _useTimeRange;

        /// <summary>
        ///     The writer used for output
        /// </summary>
        private readonly TextWriter _writer;

        /// <summary>
        /// The close text writer
        /// </summary>
        private readonly bool _closeTextWriter;

        /// <summary>
        ///     The set of coded attributes
        /// </summary>
        private readonly Dictionary<string, object> _codedAttributes =
            new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        ///     GESMES compatible TS groups.
        /// </summary>
        private readonly Dictionary<string, object> _siblingGroups =
            new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        ///     The dataset attribute stream
        /// </summary>
        private AttributeTempFile _dataSetStream;

        /// <summary>
        ///     The current GESMES array
        /// </summary>
        private GesmesArrayCell _gesmesArrayCell;

        /// <summary>
        ///     The sibling attribute stream
        /// </summary>
        private AttributeTempFile _groupStream;

        /// <summary>
        ///     The dimension position in ARR map
        /// </summary>
        private GesmesKeyMap _map;

        /// <summary>
        ///     The observation attribute stream
        /// </summary>
        private AttributeTempFile _observationStream;

        /// <summary>
        ///     The series attribute stream
        /// </summary>
        private AttributeTempFile _seriesStream;

        /// <summary>
        ///     The collection of temporary files
        /// </summary>
        private AttributeTempFile[] _tempFiles;

        /// <summary>
        ///     The temporary path
        /// </summary>
        private string _tempPath;

        /// <summary>
        ///     The default observation value
        /// </summary>
        private string _defaultObs = EdiConstants.MissingVal;

        /// <summary>
        ///     A file indicating whether this instance has been disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     The Header
        /// </summary>
        private IHeader _header;

        /// <summary>
        ///     The GESMES Header and Footer writer
        /// </summary>
        private GesmesHeaderWriter _headerWriter;

        /// <summary>
        ///     A value indicating whether this is a delete message
        /// </summary>
        private bool _isDeleteMessage;

        /// <summary>
        ///     The current periodicity
        /// </summary>
        private IPeriodicity _periodicity;

        /// <summary>
        ///     The internal field used to store the number of segments
        /// </summary>
        private int _segmentCount;

        /// <summary>
        ///     This field holds a value that indicates whether the dataset element is open.
        /// </summary>
        private bool _startedDataSet;

        /// <summary>
        ///     This field holds a value that indicates whether a group element is open.
        /// </summary>
        private bool _startedGroup;

        /// <summary>
        ///     This field holds a value that indicates whether a Observation element is open.
        /// </summary>
        private bool _startedObservation;

        /// <summary>
        ///     This field holds a value that indicates whether a series element is open.
        /// </summary>
        private bool _startedSeries;

        /// <summary>
        ///     This field holds the total number of groups written to <see cref="Writer.SdmxMLWriter" />
        /// </summary>
        private int _totalGroupsWritten;

        /// <summary>
        ///     This field holds the total number of observations written to <see cref="Writer.SdmxMLWriter" />
        /// </summary>
        private int _totalObservationsWritten;

        /// <summary>
        ///     This field holds the total number of series written to <see cref="Writer.SdmxMLWriter" />
        /// </summary>
        private int _totalSeriesWritten;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesTimeSeriesWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="useTimeRange">
        /// A value indicating whether time range method should be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        public GesmesTimeSeriesWriter(Stream writer, bool useTimeRange)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this._writer = new StreamWriter(writer);
            this._closeTextWriter = true;
            this._useTimeRange = useTimeRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesTimeSeriesWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="useTimeRange">
        /// A value indicating whether time range method should be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        public GesmesTimeSeriesWriter(TextWriter writer, bool useTimeRange)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this._writer = writer;
            this._useTimeRange = useTimeRange;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GesmesTimeSeriesWriter"/> class.
        /// </summary>
        ~GesmesTimeSeriesWriter()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the total groups written
        /// </summary>
        public int TotalGroupsWritten
        {
            get
            {
                return this._totalGroupsWritten;
            }
        }

        /// <summary>
        ///     Gets the total number of observation written
        /// </summary>
        public int TotalObservationsWritten
        {
            get
            {
                return this._totalObservationsWritten;
            }
        }

        /// <summary>
        ///     Gets the total Series Written.
        /// </summary>
        public int TotalSeriesWritten
        {
            get
            {
                return this._totalSeriesWritten;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Close writer
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Close(params IFooterMessage[] messages)
        {
           this.Dispose(); 
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Starts a dataset with the data conforming to the DSD
        /// </summary>
        /// <param name="dataflow">dataflow (optional) The dataflow can be provided to give extra information about the dataset </param>
        /// <param name="dsd">
        /// The data structure object.
        /// </param>
        /// <param name="header">
        /// Dataset header
        /// </param>
        /// <param name="annotations"> (optional) any additional annotations that are attached to the dataset, can be null if no annotations exist</param>
        /// <exception cref="System.ArgumentException">
        /// if the <paramref name="dsd"/> is null
        /// </exception>
        public void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {
            if (dsd == null)
            {
                throw new ArgumentNullException("dsd");
            }

            this.SetupKeyFamily(dsd);
            this.ParseAction(this._header.Action ?? header.Action);
            this._headerWriter = new GesmesHeaderWriter(this._writer, dsd, this._header);
            this._headerWriter.WriteHeader(this._isDeleteMessage);
            this._segmentCount = this._headerWriter.SegmentCount;
            this.StartDataSet();
        }

        /// <summary>
        /// Start a group with <paramref name="groupId" />
        /// </summary>
        /// <param name="groupId">The group id and the element name</param>
        /// <param name="annotations">Annotations any additional annotations that are attached to the group, can be null if no annotations exist</param>
        public void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            this.EndGroup();
            if (this._siblingGroups.ContainsKey(groupId))
            {
                this._startedGroup = true;
            }
        }

        /// <summary>
        /// Start a series
        /// </summary>
        /// <param name="annotations">Any additional annotations that are attached to the series, can be null if no annotations exist</param>
        public void StartSeries(params IAnnotation[] annotations)
        {
            this.StartDataSet();
            this.EndGroup();

            // check if there is an already open series element
            this.EndSeries();

            this._startedSeries = true;
        }

        /// <summary>
        /// Write an <paramref name="attribute"/> and the <paramref name="valueRen"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute concept id
        /// </param>
        /// <param name="valueRen">
        /// The value
        /// </param>
        public void WriteAttributeValue(string attribute, string valueRen)
        {
            string escapedValue = GesmesHelper.EdiEscape(valueRen);
            GesmesAttributeGroup targetGroup = null;
            if (this._startedObservation)
            {
                switch (attribute)
                {
                    case EdiConstants.ObsStatus:
                        this._gesmesArrayCell.Observation.ObservationStatus = escapedValue;
                        break;
                    case EdiConstants.ObsConf:
                        this._gesmesArrayCell.Observation.ObservationConf = escapedValue;
                        break;
                    case EdiConstants.ObsPreBreak:
                        this._gesmesArrayCell.Observation.ObservationPreBreak = escapedValue;
                        break;
                    default:
                        targetGroup = this._observationStream.CurrentGroup;
                        break;
                }
            }
            else if (this._startedSeries)
            {
                targetGroup = this._seriesStream.CurrentGroup;
            }
            else if (this._startedGroup)
            {
                targetGroup = this._groupStream.CurrentGroup;
            }
            else if (this._startedDataSet)
            {
                targetGroup = this._dataSetStream.CurrentGroup;
            }

            if (targetGroup != null)
            {
                // We don't escape now because it might be an uncoded value
                targetGroup.AddAttributeValue(attribute, valueRen);
            }
        }

        /// <summary>
        /// Write a group <paramref name="key"/> and the <paramref name="valueRen"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="valueRen">
        /// The value
        /// </param>
        public void WriteGroupKeyValue(string key, string valueRen)
        {
            if (this._startedGroup)
            {
                this._groupStream.CurrentGroup.AddDimensionValue(key, valueRen);
            }
        }

        /// <summary>
        /// Write the specified <paramref name="header"/>
        /// </summary>
        /// <param name="header">
        /// The SDMX action.
        /// </param>
        public void WriteHeader(IHeader header)
        {
            this._header = header;
        }

        /// <summary>
        /// Writes an Observation value against the current series.
        ///     <p/>
        ///     The current series is determined by the latest writeKeyValue call,
        ///     If this is a cross sectional dataset, then the observation Concept is expected to be the cross sectional concept value - for example if this is cross sectional on Country the id may be "FR"
        ///     If this is a time series dataset then the observation Concept is expected to be the observation time - for example 2006-12-12
        ///     <p/>
        ///     Validates the following:
        ///     <ul>
        ///         <li>The observation Time string format is one of an allowed SDMX time format</li>
        ///         <li>The observation Time does not replicate a previously reported observation Time for the current series</li>
        ///     </ul>
        /// </summary>
        /// <param name="observationConceptId">the concept id for the observation, for example 'COUNTRY'.  If this is a Time Series, then the id will be DimensionBean.TIME_DIMENSION_FIXED_ID. </param>
        /// <param name="obsConceptValue">may be the observation time, or the cross section value </param>
        /// <param name="obsValue"> The observation value - can be numerical
        /// </param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist </param>
        public void WriteObservation(string observationConceptId, string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
           throw new NotImplementedException("GESMES/TS does not support this method.");
        }

        /// <summary>
        /// Writes an Observation value against the current series
        ///     <p/>
        ///     The date is formatted as a string, the format rules are determined by the TIME_FORMAT argument
        ///     <p/>
        ///     Validates the following:
        ///     <ul>
        ///         <li>
        ///             The <paramref name="obsTime"/> does not replicate a previously reported <paramref name="obsTime"/> for the current series
        ///         </li>
        ///     </ul>
        /// </summary>
        /// <param name="obsTime">
        /// the time of the observation
        /// </param>
        /// <param name="obsValue">
        /// the observation value - can be numerical
        /// </param>
        /// <param name="sdmxSwTimeFormat">
        /// the time format to format the <paramref name="obsTime"/> in when converting to a string
        /// </param>
        /// <param name="annotations">
        /// The annotations.
        /// </param>
        public void WriteObservation(DateTime obsTime, string obsValue, TimeFormat sdmxSwTimeFormat, params IAnnotation[] annotations)
        {
            this.WriteObservation(DateUtil.FormatDate(obsTime, sdmxSwTimeFormat.EnumType), obsValue, annotations);
        }

        /// <summary>
        /// Writes an observation, the observation concept is assumed to be that which has been defined to be at the observation level (as declared in the start dataset method DatasetHeaderObject).
        /// </summary>
        /// <param name="obsConceptValue">May be the observation time, or the cross section value </param>
        /// <param name="obsValue">The observation value - can be numerical</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        public void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            this.EndObservation();

            this._startedObservation = true;
            if (string.IsNullOrEmpty(this._gesmesArrayCell.Frequency))
            {
                throw new InvalidOperationException("Frequency value not set");
            }

            if (this._periodicity == null)
            {
                TimeFormat timeFormatFromCodeId = TimeFormat.GetTimeFormatFromCodeId(this._gesmesArrayCell.Frequency);
                this._periodicity = PeriodicityFactory.Create(timeFormatFromCodeId.EnumType);
            }

            this._gesmesArrayCell.Observation.TimePeriod = GesmesHelper.GetPeriodValue(obsConceptValue);
            this._gesmesArrayCell.Observation.ObservationValue = GesmesHelper.GetObsValue(
                obsValue, this._defaultObs);
        }

        /// <summary>
        /// Write a series <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteSeriesKeyValue(string key, string value)
        {
            this._gesmesArrayCell.AddDimensionValue(key, GesmesHelper.EdiEscape(value));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.EndObservation();
                this.EndSeries();
                this.EndGroup();
                this.EndDataSet();

                for (int i = 0; i < this._tempFiles.Length; i++)
                {
                    AttributeTempFile attributeTempFile = this._tempFiles[i];
                    if (attributeTempFile != null)
                    {
                        attributeTempFile.Dispose();
                    }
                }

                this._writer.Flush();

                if (this._closeTextWriter)
                {
                    this._writer.Close();
                }
            }

            this.RemoveTempDir();

            this._disposed = true;
        }

        /// <summary>
        /// Setups the key family.
        /// </summary>
        /// <param name="keyFamily">The key family.</param>
        private void SetupKeyFamily(IDataStructureObject keyFamily)
        {
            WriterHelper.ValidateErrors(Validator.ValidateForCompact(keyFamily));

            this._map = new GesmesKeyMap(keyFamily);
            this._gesmesArrayCell = new GesmesArrayCell(this._map);
            IDimension frequencyDimension = keyFamily.FrequencyDimension;

            this.FindSiblingGroup(keyFamily, frequencyDimension);
            RelStatus levels = this.GetLevels(keyFamily);

            if (levels != RelStatus.None)
            {
                this._tempPath = GesmesHelper.TempPath;
                Directory.CreateDirectory(this._tempPath);
                this._seriesStream = this.CreateTempStream(levels, RelStatus.Series);

                this._groupStream = this.CreateTempStream(levels, RelStatus.Sibling);

                this._observationStream = this.CreateTempStream(levels, RelStatus.Observation);

                this._dataSetStream = this.CreateTempStream(levels, RelStatus.DataSet);
            }

            this._tempFiles = new[] { this._dataSetStream, this._groupStream, this._seriesStream, this._observationStream };
        }

        /// <summary>
        ///     Buffer observation attributes
        /// </summary>
        private void BufferObservationAttributes()
        {
            if (this._observationStream != null && this._observationStream.CurrentGroup.AttributeValues.Count > 0)
            {
                this._gesmesArrayCell.DimensionValues.CopyTo(this._observationStream.CurrentGroup.DimensionValues, 0);
                this._observationStream.CurrentGroup.AddTimeValue(
                    this._gesmesArrayCell.Observation.TimePeriod, this._gesmesArrayCell.TimeFormat);
                this._observationStream.WriteCurrentGroup();
            }
        }

        /// <summary>
        ///     Buffer series attributes
        /// </summary>
        private void BufferSeriesAttributes()
        {
            if (this._seriesStream != null && this._seriesStream.CurrentGroup.AttributeValues.Count > 0)
            {
                this._gesmesArrayCell.DimensionValues.CopyTo(this._seriesStream.CurrentGroup.DimensionValues, 0);
                this._seriesStream.WriteCurrentGroup();
            }
        }

        /// <summary>
        /// Create a <see cref="AttributeTempFile"/> if <paramref name="targetLevel"/> in <paramref name="levels"/>
        /// </summary>
        /// <param name="levels">
        /// The available levels
        /// </param>
        /// <param name="targetLevel">
        /// The target level
        /// </param>
        /// <returns>
        /// a <see cref="AttributeTempFile"/> if <paramref name="targetLevel"/> in <paramref name="levels"/> ; otherwise null
        /// </returns>
        private AttributeTempFile CreateTempStream(RelStatus levels, RelStatus targetLevel)
        {
            if ((levels & targetLevel) == targetLevel)
            {
                return new AttributeTempFile(this._tempPath, this._map, targetLevel);
            }

            return null;
        }

        /// <summary>
        ///     Conditionally end DataSet element
        /// </summary>
        private void EndDataSet()
        {
            if (this._startedDataSet)
            {
                if (this._dataSetStream != null && this._dataSetStream.CurrentGroup.AttributeValues.Count > 0)
                {
                    this._dataSetStream.WriteCurrentGroup();
                }

                this.WriteAttributes();

                if (this._headerWriter != null)
                {
                    this._headerWriter.WriteGesmesFooter(this._segmentCount);
                }

                this._startedDataSet = false;
            }
        }

        /// <summary>
        ///     Conditionally end group element
        /// </summary>
        private void EndGroup()
        {
            if (this._startedGroup && this._groupStream.CurrentGroup.AttributeValues.Count > 0)
            {
                this._startedGroup = false;
                this._totalGroupsWritten++;
                this._groupStream.WriteCurrentGroup();
            }
        }

        /// <summary>
        ///     Conditionally end observation
        /// </summary>
        private void EndObservation()
        {
            if (this._startedObservation)
            {
                this._startedObservation = false;
                if (string.IsNullOrEmpty(this._gesmesArrayCell.TimeFormat))
                {
                    this._gesmesArrayCell.TimeFormat =
                        GesmesHelper.GetTimeFormatCode(this._periodicity.Gesmes.DateFormat);
                }

                this.BufferObservationAttributes();
                if (this._useTimeRange)
                {
                    if (string.IsNullOrEmpty(this._gesmesArrayCell.TimeFormatRange))
                    {
                        this._gesmesArrayCell.TimeFormatRange =
                            GesmesHelper.GetTimeFormatCode(this._periodicity.Gesmes.RangeTimeFormat);
                    }

                    this._gesmesArrayCell.PushObservation();
                }
                else
                {
                    this._gesmesArrayCell.StreamToGesmes(this._writer);
                    this._gesmesArrayCell.ClearObservation();
                    this._segmentCount++;
                }

                this._totalObservationsWritten++;
            }
        }

        /// <summary>
        ///     Conditionally end series
        /// </summary>
        private void EndSeries()
        {
            if (this._startedSeries)
            {
                this.EndObservation();

                // in which case close it
                this._startedSeries = false;

                if (this._useTimeRange)
                {
                    this._gesmesArrayCell.StreamToGesmesTimeRange(this._writer, this._periodicity.Gesmes);
                    this._segmentCount++;
                }

                this._periodicity = null;
                this.BufferSeriesAttributes();

                this._gesmesArrayCell.Clear();
                this._totalSeriesWritten++;
            }
        }

        /// <summary>
        /// Find sibling compatible groups. This is created to replicate the behavior of the non-streaming GESMES writer.
        /// </summary>
        /// <param name="keyFamily">
        /// The key family
        /// </param>
        /// <param name="frequencyDimension">
        /// The frequency dimension
        /// </param>
        private void FindSiblingGroup(IDataStructureObject keyFamily, IIdentifiableObject frequencyDimension)
        {
            foreach (IGroup groupBean in keyFamily.Groups)
            {
                if (groupBean.DimensionRefs.Count == (keyFamily.DimensionList.Dimensions.Count - 1)
                    && !groupBean.DimensionRefs.Contains(frequencyDimension.Id))
                {
                    this._siblingGroups.Add(groupBean.Id, null);
                }
            }
        }

        /// <summary>
        /// Get available attachment levels from all attributes
        /// </summary>
        /// <param name="keyFamily">
        /// The key family
        /// </param>
        /// <returns>
        /// The attachment levels
        /// </returns>
        private RelStatus GetLevels(IDataStructureObject keyFamily)
        {
            var levels = RelStatus.None;
            for (int i = 0; i < keyFamily.Attributes.Count; i++)
            {
                IAttributeObject attributeBean = keyFamily.Attributes[i];
                switch (attributeBean.AttachmentLevel)
                {
                    case AttributeAttachmentLevel.DimensionGroup:
                        levels |= RelStatus.Series;
                        break;
                    case AttributeAttachmentLevel.Observation:
                        switch (attributeBean.Id)
                        {
                            case EdiConstants.ObsStatus:
                            case EdiConstants.ObsConf:
                            case EdiConstants.ObsPreBreak:
                                break;
                            default:
                                levels |= RelStatus.Observation;
                                break;
                        }

                        break;
                    case AttributeAttachmentLevel.Group:
                        if (attributeBean.AttachmentGroup != null)
                        {
                            if (this._siblingGroups.ContainsKey(attributeBean.AttachmentGroup))
                            {
                                levels |= RelStatus.Sibling;
                            }
                        }

                        break;
                    case AttributeAttachmentLevel.DataSet:
                        levels |= RelStatus.DataSet;
                        break;
                }

                if (attributeBean.HasCodedRepresentation())
                {
                    this._codedAttributes.Add(attributeBean.Id, null);
                }
            }

            return levels;
        }

        /// <summary>
        /// Parse the Header.DataSetAction and update the <see cref="_isDeleteMessage"/>
        /// </summary>
        /// <param name="action">
        /// The SDMX Header
        /// </param>
        private void ParseAction(BaseConstantType<DatasetActionEnumType> action)
        {
            this._isDeleteMessage = action != null && action.EnumType == DatasetActionEnumType.Delete;
        }

        /// <summary>
        ///     Remove temporary directory <see cref="_tempPath" /> if it exists
        /// </summary>
        private void RemoveTempDir()
        {
            if (this._tempPath != null)
            {
                if (Directory.Exists(this._tempPath))
                {
                    Directory.Delete(this._tempPath, true);
                }
            }
        }

        /// <summary>
        ///     Conditionally start the DataSet if <see cref="_startedDataSet" /> is false
        /// </summary>
        private void StartDataSet()
        {
            if (this._startedDataSet)
            {
                return;
            }

            this._defaultObs = this._isDeleteMessage ? EdiConstants.MissingVal : null;

            this._startedDataSet = true;
        }

        /// <summary>
        ///     Write attributes if any
        /// </summary>
        private void WriteAttributes()
        {
            bool wroteFns = false;
            for (int i = 0; i < this._tempFiles.Length; i++)
            {
                AttributeTempFile attributeTempFile = this._tempFiles[i];
                if (attributeTempFile != null && attributeTempFile.TotalAttributeWritten > 0)
                {
                    if (!wroteFns)
                    {
                        wroteFns = true;
                        this._writer.WriteLine(EdiConstants.FnsAttributes);
                        this._segmentCount++;
                    }

                    this._segmentCount += attributeTempFile.StreamToGesmes(this._writer, this._codedAttributes);
                }
            }
        }

        #endregion
    }
}
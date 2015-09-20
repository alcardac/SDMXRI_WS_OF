// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompactDataWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Engine;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Properties;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     This class is responsible for writing Compact SDMX-ML data messages
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="CompactDataWriterEngine" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingCompactWriter.cs" lang="cs" />
    /// </example>
    public class CompactDataWriterEngine : DataWriterEngineBase
    {
        #region Fields

        /// <summary>
        /// A flag indicating whether the writer should be closed.
        /// </summary>
        private readonly bool _closeXmlWriter;

        /// <summary>
        ///     The component values
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _componentVals = new List<KeyValuePair<string, string>>();

        /// <summary>
        ///     The namespace used in <c>DataSet</c>, <c>Series</c> and <c>Obs</c> elements. It depends on
        ///     <see cref="IoBase.TargetSchema" />
        /// </summary>
        private NamespacePrefixPair _compactNs;

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

        /// <summary>
        /// The observation concept. It is affected by SDMX version and the <see cref="DataWriterEngineBase.IsCrossSectional"/>
        /// </summary>
        private string _obsConcept;

        /// <summary>
        ///     The dataset annotations
        /// </summary>
        private IAnnotation[] _datasetAnnotations;

        /// <summary>
        ///     The series annotations
        /// </summary>
        private IAnnotation[] _seriesAnnotations;

        /// <summary>
        ///     The group annotations
        /// </summary>
        private IAnnotation[] _groupAnnotations;

        /// <summary>
        ///     The OBS annotations
        /// </summary>
        private IAnnotation[] _obsAnnotations;

        /// <summary>
        ///     The primary measure concept
        /// </summary>
        private string _primaryMeasureConcept;

        /// <summary>
        /// The _disposing
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDataWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public CompactDataWriterEngine(XmlWriter writer, SdmxSchema schema)
            : base(writer, schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDataWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public CompactDataWriterEngine(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(writer, namespaces, schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDataWriterEngine"/> class.
        /// </summary>
        /// <param name="outStream">The out stream.</param>
        /// <param name="schemaVersion">The schema version.</param>
        public CompactDataWriterEngine(Stream outStream, SdmxSchema schemaVersion)
            : this(XmlWriter.Create(outStream), schemaVersion)
        {
            this._closeXmlWriter = true;
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

        #region Properties

        /// <summary>
        ///     Gets the data format namespace Suffix e.g. compact
        /// </summary>
        protected override BaseDataFormatEnumType DataFormatType
        {
            get
            {
                return BaseDataFormatEnumType.Compact;
            }
        }

        /// <summary>
        ///     Gets the Message element tag
        /// </summary>
        protected override string MessageElement
        {
            get
            {
                return NameTableCache.GetElementName(this.IsTwoPointOne ? ElementNameTable.StructureSpecificData : ElementNameTable.CompactData);
            }
        }

        /// <summary>
        ///   Gets the default namespace
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this.Namespaces.DataSetStructureSpecific;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Starts a dataset with the data conforming to the DSD
        /// </summary>
        /// <param name="dataflow">Optional. The dataflow can be provided to give extra information about the dataset.</param>
        /// <param name="dsd">The <see cref="IDataStructureObject" /> for which the dataset will be created</param>
        /// <param name="header">The <see cref="IHeader" /> of the dataset</param>
        /// <param name="annotations">Any additional annotations that are attached to the dataset, can be null if no annotations exist</param>
        /// <exception cref="System.ArgumentNullException">if the <paramref name="dsd" /> is null</exception>
        public override void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {
            base.StartDataset(dataflow, dsd, header, annotations);
            this._primaryMeasureConcept = this.GetComponentId(dsd.PrimaryMeasure);
            this._compactNs = this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne
                                  ? this.Namespaces.DataSetStructureSpecific
                                  : NamespacePrefixPair.Empty;
            this._obsConcept = this.IsTwoPointOne
                                   ? this.DimensionAtObservation
                                   : ConceptRefUtil.GetConceptId(this.KeyFamily.TimeDimension.ConceptRef);
            if (this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._datasetAnnotations = annotations;
            }
        }

        /// <summary>
        /// Start a group with <paramref name="groupId" />
        /// </summary>
        /// <param name="groupId">The group id and the element name</param>
        /// <param name="annotations">Annotations any additional annotations that are attached to the group, can be null if no annotations exist</param>
        /// <exception cref="System.InvalidOperationException">Started a group after starting series.</exception>
        /// <exception cref="InvalidOperationException">Cannot call a StartGroup after StartSeries</exception>
        public override void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            base.StartGroup(groupId, annotations);
            if (this._startedSeries)
            {
                throw new InvalidOperationException(Resources.ErrorStartGroupAfterStartSeries);
            }

            this.EndGroup();

            // TODO check java for namespace in 2.0 and 2.1
            if (this.TargetSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                this.WriteStartElement(ElementNameTable.Group);
                string format = string.Format(
                    CultureInfo.InvariantCulture, "{0}:{1}", this.Namespaces.DataSetStructureSpecific.Prefix, groupId);
                this.WriteAttributeString(this.Namespaces.Xsi, AttributeNameTable.type, format);
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._groupAnnotations = annotations;
                this.WriteStartElement(this.Namespaces.DataSetStructureSpecific, groupId);
            }

            this._startedGroup = true;
            this._totalGroupsWritten++;
        }

        /// <summary>
        /// Start a series
        /// </summary>
        /// <param name="annotations">Any additional annotations that are attached to the series, can be null if no annotations exist</param>
        public override void StartSeries(params IAnnotation[] annotations)
        {
            this.EndGroup();

            // check if there is an already open series element
            this.EndSeries();
            base.StartSeries(annotations);
            this._startedSeries = true;
            if (this.IsFlat)
            {
                this._componentVals.Clear();
            }
            else
            {
                // write the series element
                this.WriteStartElement(this._compactNs, ElementNameTable.Series);

                this._totalSeriesWritten++;
                this._seriesAnnotations = annotations;
            }
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
        public override void WriteAttributeValue(string attribute, string valueRen)
        {
            attribute = this.GetComponentId(attribute);
            base.WriteAttributeValue(attribute, valueRen);
            if (this.IsFlat && this._startedSeries && !this._startedObservation)
            {
                this._componentVals.Add(new KeyValuePair<string, string>(attribute, valueRen));
            }
            else
            {
                this.WriteAttributeString(attribute, valueRen);
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
        public override void WriteGroupKeyValue(string key, string valueRen)
        {
            key = this.GetComponentId(key);
            base.WriteGroupKeyValue(key, valueRen);
            if (this._startedGroup)
            {
                this.WriteAttributeString(key, valueRen);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrorStartGroupNotCalledOrStartSeriesStarted);
            }
        }

        /// <summary>
        /// Writes an observation, the observation concept is assumed to be that which has been defined to be at the observation level (as declared in the start dataset method DatasetHeaderObject).
        /// </summary>
        /// <param name="obsConceptValue">May be the observation time, or the cross section value</param>
        /// <param name="obsValue">The observation value - can be numerical</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        /// <exception cref="System.ArgumentNullException">Can not write observation, as no observation concept id was given, and this is writing a flat dataset.  +
        /// Please use the method: <c>WriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue, params IAnnotation[] annotations)</c></exception>
        public override void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            if (this.IsFlat)
            {
                throw new ArgumentNullException("Can not write observation, as no observation concept id was given, and this is writing a flat dataset. " +
                        "Please use the method: WriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue, params IAnnotation[] annotations)");
            }
            
            this.WriteObservation(this._obsConcept, obsConceptValue, obsValue, annotations);
        }

        /// <summary>
        /// Write the <paramref name="observationConceptId" /> and the <paramref name="primaryMeasureValue" />
        /// </summary>
        /// <param name="observationConceptId">The observation component ID</param>
        /// <param name="obsConceptValue">The co</param>
        /// <param name="primaryMeasureValue">The primary measure value</param>
        /// <param name="annotations">The annotations.</param>
        /// <exception cref="System.InvalidOperationException"><see cref="StartSeries"/> not called for SDMX v2.0 dataset</exception>
        public override void WriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue, params IAnnotation[] annotations)
        {
            if (this._seriesAnnotations != null && this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, this._seriesAnnotations);
                this._seriesAnnotations = null;
            }

            observationConceptId = this.GetComponentId(observationConceptId);
            base.WriteObservation(observationConceptId, obsConceptValue, primaryMeasureValue, annotations);
            
            if (!this._startedSeries && this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne)
            {
                throw new InvalidOperationException(Resources.ErrorStartSeriesNotCalled);
            }

            this.EndObservation();

            this.WriteStartElement(this._compactNs, ElementNameTable.Obs);
            if (this.IsFlat)
            {
                foreach (var keyValuePair in this._componentVals)
                {
                    this.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
                }
            }

            this.WriteAttributeString(observationConceptId, obsConceptValue);
            string obsValue = string.IsNullOrEmpty(primaryMeasureValue) ? this.DefaultObs : primaryMeasureValue;
            this.TryWriteAttribute(this._primaryMeasureConcept, obsValue);
            this._startedObservation = true;
            this._totalObservationsWritten++;

            if (this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._obsAnnotations = annotations;  // STORE THE ANNOTATIONS
            }
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
        public override void WriteSeriesKeyValue(string key, string value)
        {
            key = this.GetComponentId(key);
            base.WriteSeriesKeyValue(key, value);
            if (this.IsFlat)
            {
                this._componentVals.Add(new KeyValuePair<string, string>(key, value));
            }
            else
            {
                if (!this._startedSeries)
                {
                    this.StartSeries();
                }

                this.WriteAttributeString(key, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposable)
        {
            if (!this._disposed)
            {
                if (disposable)
                {
                    this.EndGroup();
                    this.EndObservation();
                    this.EndSeries();
                    this.EndDataSet();
                    if (this.IsTwoPointOne)
                    {
                        this.WriteFooter(this.FooterMessage);
                    }

                    this.CloseMessageTag();

                    if (this._closeXmlWriter)
                    {
                        this.SdmxMLWriter.Close();
                    }
                }

                this._disposed = true;
            }

            base.Dispose(disposable);
        }

        /// <summary>
        ///     Conditionally end DataSet element
        /// </summary>
        protected override void EndDataSet()
        {
            if (!this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, this._datasetAnnotations);
                this._datasetAnnotations = null;
            }

            base.EndDataSet();
            if (this._startedDataSet)
            {
                this._startedDataSet = false;
            }
        }

        /// <summary>
        /// Conditionally start the DataSet if <see cref="_startedDataSet"/> is false
        /// </summary>
        /// <param name="header">
        /// The dataset header
        /// </param>
        protected override void WriteFormatDataSet(IDatasetHeader header)
        {
            if (this._startedDataSet)
            {
                return;
            }

            NamespacePrefixPair dataSetNS = this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne
                                                ? this.Namespaces.DataSetStructureSpecific
                                                : this.Namespaces.Message;
            this.WriteStartElement(dataSetNS, ElementNameTable.DataSet);
            this.WriteDataSetHeader(header);
            this._startedDataSet = true;
        }

        /// <summary>
        ///     Conditionally end group element
        /// </summary>
        private void EndGroup()
        {
            if (this._startedGroup)
            {
               if (!this.IsTwoPointOne)
               {
                   this.WriteAnnotations(ElementNameTable.AnnotationType, this._groupAnnotations);
                   this._groupAnnotations = null;
               }

               this.WriteEndElement();
               this._startedGroup = false;
            }
        }

        /// <summary>
        ///     Conditionally end observation
        /// </summary>
        private void EndObservation()
        {
            if (this._startedObservation)
            {
                if (!this.IsTwoPointOne)
                {
                    this.WriteAnnotations(ElementNameTable.AnnotationType, this._obsAnnotations);
                    this._obsAnnotations = null;
                }

                this.WriteEndElement();
                this._startedObservation = false;
            }
        }

        /// <summary>
        ///     Conditionally end series
        /// </summary>
        private void EndSeries()
        {
            this.EndObservation();

            if (this._startedSeries)
            {
                if (!this.IsTwoPointOne)
                {
                    this.WriteAnnotations(ElementNameTable.AnnotationType, this._seriesAnnotations);
                    this._seriesAnnotations = null;
                }

                if (!this.IsFlat)
                {
                    // in which case close it
                    this.WriteEndElement();
                }

                this._startedSeries = false;
            }
        }

        #endregion
    }
}
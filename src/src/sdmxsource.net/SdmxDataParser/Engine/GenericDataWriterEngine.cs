// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericDataWriterEngine.cs" company="Eurostat">
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
    ///     The generic stream data writer.
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="GenericDataWriterEngine" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingGenericWriter.cs" lang="cs" />
    /// </example>
    public class GenericDataWriterEngine : DataWriterEngineBase
    {
        #region Fields

        /// <summary>
        ///     The concept attribute
        /// </summary>
        private readonly AttributeNameTable _conceptAttribute;

        /// <summary>
        /// A flag indicating whether the writer should be closed.
        /// </summary>
        private readonly bool _closeXmlWriter;

        /// <summary>
        ///     The component values
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _componentVals = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// The _dimension at observation
        /// </summary>
        private string _dimensionAtObservation;

        /// <summary>
        ///     The write observation method.
        /// </summary>
        private Action<string, string> _writeObservationMethod;

        /// <summary>
        ///     This field holds a value that indicates whether the attribute element is open.
        /// </summary>
        private bool _startedAttributes;

        /// <summary>
        ///     This field holds a value that indicates whether the dataset element is open.
        /// </summary>
        private bool _startedDataSet;

        /// <summary>
        ///     This field holds a value that indicates whether a group element is open.
        /// </summary>
        private bool _startedGroup;

        /// <summary>
        ///     This field holds a value that indicates whether the *key element is open.
        /// </summary>
        private bool _startedKey;

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
        /// The _disposed
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public GenericDataWriterEngine(Stream writer, SdmxSchema schema)
            : this(XmlWriter.Create(writer), schema)
        {
            this._closeXmlWriter = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public GenericDataWriterEngine(XmlWriter writer, SdmxSchema schema)
            : base(writer, schema)
        {
            if (this.IsTwoPointOne)
            {
                this._conceptAttribute = AttributeNameTable.id;
                this._writeObservationMethod = this.WriteObservation21;
            }
            else
            {
                this._conceptAttribute = AttributeNameTable.concept;
                this._writeObservationMethod = this.WriteObservation20;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataWriterEngine"/> class.
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
        public GenericDataWriterEngine(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(writer, namespaces, schema)
        {
            if (this.IsTwoPointOne)
            {
                this._conceptAttribute = AttributeNameTable.id;
                this._writeObservationMethod = this.WriteObservation21;
            }
            else
            {
                this._conceptAttribute = AttributeNameTable.concept;
                this._writeObservationMethod = this.WriteObservation20;
            }
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
        ///   Gets the default namespace
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this.Namespaces.Generic;
            }
        }

        /// <summary>
        ///     Gets DataFormatType.
        /// </summary>
        protected override BaseDataFormatEnumType DataFormatType
        {
            get
            {
                return BaseDataFormatEnumType.Generic;
            }
        }

        /// <summary>
        ///     Gets MessageElement.
        /// </summary>
        protected override string MessageElement
        {
            get
            {
                return NameTableCache.GetElementName(ElementNameTable.GenericData);
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
            if (this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._datasetAnnotations = annotations;
            }

            if (this.IsFlat)
            {
                this._writeObservationMethod = this.WriteObservation21Flat;
            }
        }

        /// <summary>
        /// Start a group with <paramref name="groupId" />
        /// </summary>
        /// <param name="groupId">The group id and the element name</param>
        /// <param name="annotations">Annotations any additional annotations that are attached to the group, can be null if no annotations exist</param>
        /// <exception cref="System.InvalidOperationException">Called after <see cref="StartSeries"/></exception>
        public override void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            base.StartGroup(groupId, annotations);
            if (this._startedSeries)
            {
                throw new InvalidOperationException(Resources.ErrorStartGroupAfterStartSeries);
            }

            this.EndAttribute();
            this.EndKey();
            this.EndGroup();

            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Group);
            string type = !string.IsNullOrEmpty(groupId) ? groupId : "Sibling";
            this.WriteAttributeString(AttributeNameTable.type, type);
            this._totalGroupsWritten++;
            this._startedGroup = true;

            if (this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._groupAnnotations = annotations;
            }
        }

        /// <summary>
        /// Start a series
        /// </summary>
        /// <param name="annotations">Any additional annotations that are attached to the series, can be null if no annotations exist</param>
        public override void StartSeries(params IAnnotation[] annotations)
        {
            base.StartSeries(annotations);
            this.EndAttribute();
            this.EndKey();
            this.EndGroup();
            this.EndObservation();
            this.EndSeries();
            if (this.IsFlat)
            {
                this._componentVals.Clear();
            }
            else
            {
                // <generic:Series>
                this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Series);
                this._totalSeriesWritten++;
                if (this.IsTwoPointOne)
                {
                    this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
                }
                else
                {
                    this._seriesAnnotations = annotations;
                }
            }

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
        public override void WriteAttributeValue(string attribute, string valueRen)
        {
            attribute = this.GetComponentId(attribute);
            base.WriteAttributeValue(attribute, valueRen);
            this.EndKey();
            if (this.IsFlat && this._startedSeries && !this._startedObservation)
            {
                this._componentVals.Add(new KeyValuePair<string, string>(attribute, valueRen));
            }
            else
            {
                this.StartAttribute();
                this.WriteConceptValue(attribute, valueRen);
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
                this.StartKey(ElementNameTable.GroupKey);
                this.WriteConceptValue(key, valueRen);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrorStartGroupNotCalledOrStartSeriesStarted);
            }
        }

        /// <summary>
        /// Writes an observation, the observation concept is assumed to be that which has been defined to be at the observation level (as declared in the start dataset method DatasetHeaderObject).
        /// </summary>
        /// <param name="obsConceptValue">May be the observation time, or the cross section value </param>
        /// <param name="obsValue">The observation value - can be numerical</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        public override void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            if (this.IsFlat)
            {
                throw new ArgumentNullException("Can not write observation, as no observation concept id was given, and this is writing a flat dataset. " +
                        "Please use the method: WriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue, params IAnnotation[] annotations)");
            }

            string obsConcept = this.IsTwoPointOne ? this._dimensionAtObservation : ConceptRefUtil.GetConceptId(this.KeyFamily.TimeDimension.ConceptRef);

            this.WriteObservation(obsConcept, obsConceptValue, obsValue, annotations);
        }

        /// <summary>
        /// Writes an Observation value against the current series.
        ///     <p/>
        ///     The current series is determined by the latest writeKeyValue call,
        ///     If this is a cross sectional dataset, then the <paramref name="observationConceptId"/> is expected to be the cross sectional concept value - for example if this is cross sectional on Country the id may be "FR"
        ///     If this is a time series dataset then the <paramref name="observationConceptId"/> is expected to be the observation time - for example 2006-12-12
        ///     <p/>
        /// </summary>
        /// <param name="observationConceptId"> the observation dimension id </param>
        /// <param name="obsConceptValue"> the observation dimension value </param>
        /// <param name="primaryMeasureValue">
        /// the observation value - can be numerical
        /// </param>
        /// <param name="annotations">The observation annotations </param>
        public override void WriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue, params IAnnotation[] annotations)
        {
            observationConceptId = this.GetComponentId(observationConceptId); 
            if (observationConceptId == null)
            {
                throw new ArgumentNullException("observationConceptId");
            }

            base.WriteObservation(observationConceptId, obsConceptValue, primaryMeasureValue, annotations);

            this.EndAttribute();
            this.EndKey();
            this.EndObservation();

            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Obs);
            string obsValue = string.IsNullOrEmpty(primaryMeasureValue) ? this.DefaultObs : primaryMeasureValue;
            this._writeObservationMethod(observationConceptId, obsConceptValue);

            if (!string.IsNullOrEmpty(obsValue))
            {
                // <generic:ObsValue value="3.14"/>
                this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.ObsValue);
                this.WriteAttributeString(AttributeNameTable.value, obsValue);
                this.WriteEndElement();
            }

            this._startedObservation = true;
            this._totalObservationsWritten++;
            if (this.IsTwoPointOne)
            {
                this.WriteAnnotations(ElementNameTable.AnnotationType, annotations);
            }
            else
            {
                this._obsAnnotations = annotations;
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

                this.StartKey(ElementNameTable.SeriesKey);
                this.WriteConceptValue(key, value);
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
                    this.EndKey();
                    this.EndAttribute();
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

            this._dimensionAtObservation = this.GetDimensionAtObservation(header);

            this.WriteStartElement(this.Namespaces.Message, ElementNameTable.DataSet);
            this.WriteDataSetHeader(header);

            if (this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne)
            {
                this.TryToWriteElement(this.Namespaces.Generic, ElementNameTable.KeyFamilyRef, this.KeyFamily.Id);
            }

            this._startedDataSet = true;
        }

        /// <summary>
        ///     End a <see cref="ElementNameTable.Attributes" /> is <see cref="_startedAttributes" /> is true
        /// </summary>
        private void EndAttribute()
        {
            if (!this._startedAttributes)
            {
                return;
            }

            this.WriteEndElement();
            this._startedAttributes = false;
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
        ///     End element if <see cref="_startedKey" /> is true
        /// </summary>
        private void EndKey()
        {
            if (!this._startedKey)
            {
                return;
            }

            this.WriteEndElement();
            this._startedKey = false;
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
            if (IsFlat)
            {
                return;
            }

            if (this._startedSeries)
            {
                if (!this.IsTwoPointOne)
                {
                    this.WriteAnnotations(ElementNameTable.AnnotationType, this._seriesAnnotations);
                    this._seriesAnnotations = null;
                }
                
                // in which case close it
                this.WriteEndElement();
                this._startedSeries = false;
            }
        }

        /// <summary>
        ///     Start a <see cref="ElementNameTable.Attributes" /> is <see cref="_startedAttributes" /> is false
        /// </summary>
        private void StartAttribute()
        {
            if (this._startedAttributes)
            {
                return;
            }

            // <generic:GroupKey>
            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Attributes);
            this._startedAttributes = true;
        }

        /// <summary>
        /// Start a <see cref="ElementNameTable.SeriesKey"/> or a <see cref="ElementNameTable.GroupKey"/> is
        ///     <see cref="_startedKey"/>
        ///     is false
        /// </summary>
        /// <param name="keyTag">
        /// The  a <see cref="ElementNameTable.SeriesKey"/> or a <see cref="ElementNameTable.GroupKey"/>
        /// </param>
        private void StartKey(ElementNameTable keyTag)
        {
            if (this._startedKey)
            {
                return;
            }

            // <generic:GroupKey>
            this.WriteStartElement(this.Namespaces.Generic, keyTag);
            this._startedKey = true;
        }

        /// <summary>
        /// Write <see cref="ElementNameTable.Value"/> with <paramref name="concept"/> and <paramref name="value"/>
        /// </summary>
        /// <param name="concept">
        /// The concept id
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        private void WriteConceptValue(string concept, string value)
        {
            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Value);

            this.WriteAttributeString(this._conceptAttribute, concept);
            this.WriteAttributeString(AttributeNameTable.value, value);

            this.WriteEndElement();
        }

        /// <summary>
        /// Write the SDMX 2.0 part of the observation .
        /// </summary>
        /// <param name="obsConcept">
        /// The observation concept.
        /// </param>
        /// <param name="obsConceptValue">The observation concept value.</param>
        private void WriteObservation20(string obsConcept, string obsConceptValue)
        {
            if (!this._startedSeries)
            {
                throw new InvalidOperationException(Resources.ErrorStartSeriesNotCalled);
            }

            // <generic:Time>2000-01</generic:Time>
            this.WriteElement(this.Namespaces.Generic, ElementNameTable.Time, obsConceptValue);
        }

        /// <summary>
        /// Write the SDMX 2.1 part of the observation .
        /// </summary>
        /// <param name="obsConcept">The observation concept.</param>
        /// <param name="obsConceptValue">The observation concept value.</param>
        private void WriteObservation21(string obsConcept, string obsConceptValue)
        {
            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.ObsDimension);
            //// if (this.IsCrossSectional)
            //// {
            this.WriteAttributeString(AttributeNameTable.id, obsConcept);
            //// }

            this.WriteAttributeString(AttributeNameTable.value, obsConceptValue);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the SDMX 2.1 (flat) part of the observation .
        /// </summary>
        /// <param name="obsConcept">this.DimensionAtObservation
        /// The observation concept.</param>
        /// <param name="obsConceptValue">The observation concept value.</param>
        private void WriteObservation21Flat(string obsConcept, string obsConceptValue)
        {
            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.ObsKey);

            this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Value);
            this.WriteAttributeString(AttributeNameTable.id, obsConcept);
            this.WriteAttributeString(AttributeNameTable.value, obsConceptValue);

            this.WriteEndElement(); //END Value

            foreach (var componentId in this._componentVals)
            {
               this.WriteStartElement(this.Namespaces.Generic, ElementNameTable.Value);
               this.WriteAttributeString(AttributeNameTable.id, componentId.Key);
               this.WriteAttributeString(AttributeNameTable.value, componentId.Value);
               this.WriteEndElement(); //END Value
            }

            this.WriteEndElement(); //END ObsKey
        }

        #endregion
    }
}
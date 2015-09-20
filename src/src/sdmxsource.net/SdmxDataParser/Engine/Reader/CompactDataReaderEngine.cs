// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompactDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-06-17
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The compact data reader engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Xml;

    /// <summary>
    ///     The compact data reader engine.
    /// </summary>
    public sealed class CompactDataReaderEngine : AbstractSdmxDataReaderEngine
    {
        #region Fields

        /// <summary>
        ///     The group attribute concepts.
        /// </summary>
        private readonly IDictionary<string, ISet<string>> _groupAttributeConcepts = new Dictionary<string, ISet<string>>(StringComparer.Ordinal);

        /// <summary>
        ///     The _attribute values.
        /// </summary>
        private IDictionary<string, string> _attributeValues = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _attributes.
        /// </summary>
        private List<IKeyValue> _attributes = new List<IKeyValue>();

        /// <summary>
        ///     The _attributes on dataset node.
        /// </summary>
        private IDictionary<string, string> _attributesOnDatasetNode = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _cross section.
        /// </summary>
        private IKeyValue _crossSection;

        /// <summary>
        ///     The _dataset attributes.
        /// </summary>
        private ISet<string> _datasetAttributes = new HashSet<string>(StringComparer.Ordinal);

        //// CONCEPTS

        /// <summary>
        ///     The _dimension concepts.
        /// </summary>
        private List<string> _dimensionConcepts = new List<string>();

        /// <summary>
        ///     The _group concepts.
        /// </summary>
        private IDictionary<string, List<string>> _groupConcepts = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        /// <summary>
        ///     The _groups
        /// </summary>
        private IList<string> _groups = new List<string>();

        /// <summary>
        ///     The _key values.
        /// </summary>
        private IDictionary<string, string> _keyValues = new Dictionary<string, string>(StringComparer.Ordinal);

        //// OBS INFO

        /// <summary>
        ///     The OBS time.
        /// </summary>
        private string _obsTime;

        /// <summary>
        ///     The OBS value.
        /// </summary>
        private string _obsValue;

        /// <summary>
        ///     The _observation attributes.
        /// </summary>
        private ISet<string> _observationAttributes = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _primary measure concept.
        /// </summary>
        private string _primaryMeasureConcept;

        /// <summary>
        ///     The _rolled up attributes.
        /// </summary>
        private IDictionary<string, string> _rolledUpAttributes = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _series attributes.
        /// </summary>
        private ISet<string> _seriesAttributes = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>
        ///     The _time concept.
        /// </summary>
        private string _timeConcept;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataLocation">
        /// The data Location.
        /// </param>
        /// <param name="defaultDataflow">
        /// The default Dataflow. (Optional)
        /// </param>
        /// <param name="defaultDsd">
        /// The default DSD. The default DSD to use if the
        ///     <paramref>
        ///         <name>objectRetrieval</name>
        ///     </paramref>
        ///     is null, or
        ///     if the bean retrieval does not return the DSD for the given dataset.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// AbstractDataReaderEngine expects either a ISdmxObjectRetrievalManager or a
        ///     IDataStructureObject to be able to interpret the structures
        /// </exception>
        public CompactDataReaderEngine(IReadableDataLocation dataLocation, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
            : this(dataLocation, null, defaultDataflow, defaultDsd)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDataReaderEngine"/> class.
        ///     Initializes a new instance of the <see cref="AbstractSdmxDataReaderEngine"/> class.
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
        public CompactDataReaderEngine(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager objectRetrieval, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
            : base(dataLocation, objectRetrieval, defaultDataflow, defaultDsd)
        {
            this.Reset();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the attributes available for the current dataset
        /// </summary>
        /// <value> a copy of the list, returns an empty list if there are no dataset attributes </value>
        public override IList<IKeyValue> DatasetAttributes
        {
            get
            {
                var returnList = new List<IKeyValue>();
                foreach (var datasetAttribute in this.CurrentDsdInternal.DatasetAttributes)
                {
                    string attributeValue;
                    if (this._attributesOnDatasetNode.TryGetValue(datasetAttribute.Id, out attributeValue))
                    {
                        returnList.Add(new KeyValueImpl(attributeValue, datasetAttribute.Id));
                    }
                }

                return returnList;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a copy of this data reader engine, the copy is another iterator over the same source data
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataReaderEngine" /> .
        /// </returns>
        public override IDataReaderEngine CreateCopy()
        {
            return new CompactDataReaderEngine(this.DataLocation, this.ObjectRetrieval, this.DefaultDataflow, this.DefaultDsd);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Next the specified include observation.
        /// </summary>
        /// <param name="includeObs">
        /// if set to <c>true</c> [include observation].
        /// </param>
        /// <returns>
        /// True is there is a next observation.
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// Unexpected Node in XML: + nodeName
        /// </exception>
        protected override bool Next(bool includeObs)
        {
            while (this.Parser.Read())
            {
                switch (this.Parser.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string nodeName = this.Parser.LocalName;

                            if (ElementNameTable.DataSet.Is(nodeName))
                            {
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Dataset;
                                this.ProcessDataSetNode();
                                return true;
                            }

                            if (ElementNameTable.Series.Is(nodeName))
                            {
                                StaxUtil.JumpToNode(this.RunAheadParser, ElementNameTable.Series.FastToString(), null);
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Series;
                                return true;
                            }

                            if (!this.IsTwoPointOne && this._groups.Contains(nodeName))
                            {
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Group;
                                this.GroupId = nodeName;
                                return true;
                            }

                            if (this.IsTwoPointOne && ElementNameTable.Group.Is(nodeName))
                            {
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Group;
                                this.GroupId = this.Parser.GetAttribute("type", XmlConstants.XmlSchemaNS);
                                return true;
                            }

                            if (ElementNameTable.Obs.Is(nodeName))
                            {
                                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Series || this.DatasetPositionInternal == Api.Constants.DatasetPosition.Observation)
                                {
                                    if (includeObs)
                                    {
                                        this.DatasetPositionInternal = Api.Constants.DatasetPosition.Observation;
                                        return true;
                                    }

                                    continue;
                                }

                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.ObservationAsSeries;
                                return true;
                            }

                            if (ElementNameTable.Annotations.Is(nodeName))
                            {
                                this.Parser.Skip();
                            }
                            else
                            {
                                throw new SdmxSyntaxException("Unexpected Node in XML:" + nodeName);
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        {
                            string nodeName = this.Parser.LocalName;
                            if (ElementNameTable.Series.Is(nodeName) || ElementNameTable.Group.Is(nodeName) || this._groups.Contains(nodeName))
                            {
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Null;
                            }
                        }

                        break;
                }
            }

            this.DatasetPositionInternal = Api.Constants.DatasetPosition.Null;
            this.HasNext = false;
            return false;
        }

        /// <summary>
        ///     Processes the group node.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected override IKeyable ProcessGroupNode()
        {
            // clear values
            this._keyValues.Clear();
            this._attributes.Clear();

            for (int i = 0; i < this.Parser.AttributeCount; i++)
            {
                this.Parser.MoveToAttribute(i);
                string attributeName = this.GetComponentId(this.Parser.LocalName);
                string attributeValue = this.Parser.Value;
                string ns = this.Parser.NamespaceURI;

                if (XmlConstants.XmlSchemaNS.Equals(ns) && "type".Equals(attributeName))
                {
                    this.GroupId = attributeValue.Contains(":") ? attributeValue.Split(':')[1] : attributeValue;
                }
                else
                {
                    if (this._dimensionConcepts.Contains(attributeName))
                    {
                        this._keyValues.Add(attributeName, attributeValue);
                    }
                    else
                    {
                        this._attributeValues.Add(attributeName, attributeValue);
                    }
                }
            }

            var key = new List<IKeyValue>();
            var attributes = new List<IKeyValue>();

            List<string> conceptList;
            if (!this._groupConcepts.TryGetValue(this.GroupId, out conceptList))
            {
                throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "Data Structure '{0}' does not contain group '{1}'", this.CurrentDsdInternal, this.GroupId));
            }

            foreach (var groupConcept in conceptList)
            {
                string conceptValue;
                if (!this._keyValues.TryGetValue(groupConcept, out conceptValue) && !this._rolledUpAttributes.TryGetValue(groupConcept, out conceptValue))
                {
                    throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "No value found in data for group '{0}' and concept '{1}'.  ", this.GroupId, groupConcept));
                }

                var kv = new KeyValueImpl(conceptValue, groupConcept);
                key.Add(kv);
            }

            try
            {
                this.ProcessAttributes(this._groupAttributeConcepts[this.GroupId], attributes);
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(string.Format("Error while processing group attributes for group '{0}' ", this.GroupId), e);
            }

            this._keyValues.Clear();
            this._attributeValues.Clear();

            return this.CurrentKeyValue = this.CreateKeyable(key, attributes, this.GroupId);
        }

        /// <summary>
        /// Processes the observation node.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IObservation"/>.
        /// </returns>
        protected override IObservation ProcessObsNode(XmlReader parser)
        {
            this.ClearObsInformation();
            this.ProcessObservation(parser);

            try
            {
                if (this.IsTimeSeries)
                {
                    return new ObservationImpl(this.CurrentKeyValue, this._obsTime, this._obsValue, this._attributes);
                }

                return new ObservationImpl(this.CurrentKeyValue, this.CurrentKeyValue.ObsTime, this._obsValue, this._attributes, this._crossSection);
            }
            catch (Exception e)
            {
                if (this.CurrentKeyValue != null)
                {
                    throw new SdmxSemmanticException(string.Format("Error while processing observation for key {0}", this.CurrentKeyValue));
                }

                throw new SdmxSemmanticException("Error while processing observation", e);
            }
            finally
            {
                this._attributes.Clear();
            }
        }

        /// <summary>
        ///     Processes the series node.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Missing series key value</exception>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected override IKeyable ProcessSeriesNode()
        {
            // clear values
            this._keyValues.Clear();
            this._attributeValues.Clear();
            TimeFormat timeFormat = null;
            string timeValue = null;
            ISet<string> unknownConcepts = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < this.Parser.AttributeCount; i++)
            {
                this.Parser.MoveToAttribute(i);
                string attributeId = this.GetComponentId(this.Parser.LocalName);
                string attributeValue = this.Parser.Value;
                if (string.Equals(attributeId, this._timeConcept))
                {
                    timeValue = attributeValue;
                    timeFormat = DateUtil.GetTimeFormatOfDate(timeValue);
                }
                else if (this._dimensionConcepts.Contains(attributeId))
                {
                    this._keyValues.Add(attributeId, attributeValue);
                }
                else if (this._seriesAttributes.Contains(attributeId))
                {
                    this._attributeValues.Add(attributeId, attributeValue);
                }
                else if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
                {
                    // This attribute was not found as a series level attribute, it could still be an observation level attribute
                    // But this is only allowed if we are processing this series node as a FLAT obs node which is both an obs and key
                    // In which case this attributeName could be an observation attribute, the primary measure, or the time concept
                    if (!this._observationAttributes.Contains(attributeId) && !attributeId.Equals(this._primaryMeasureConcept) && !attributeId.Equals(this._timeConcept))
                    {
                        unknownConcepts.Add(attributeId);
                    }
                }
                else
                {
                    unknownConcepts.Add(attributeId);
                }
            }

            var key = new List<IKeyValue>();
            foreach (var dimensionConcept in this._dimensionConcepts)
            {
                string conceptValue;
                if (!this._keyValues.TryGetValue(dimensionConcept, out conceptValue) && !this._rolledUpAttributes.TryGetValue(dimensionConcept, out conceptValue))
                {
                    if (this.DatasetHeader.Action != DatasetActionEnumType.Delete)
                    {
                        if (this.IsTimeSeries || !this.CrossSectionConcept.Equals(dimensionConcept))
                        {
                            throw new SdmxSemmanticException(string.Format("Missing series key value for concept: {0}", dimensionConcept));
                        }
                    }
                }
                else
                {
                    IKeyValue kv = new KeyValueImpl(conceptValue, dimensionConcept);
                    key.Add(kv);
                }
            }

            if (unknownConcepts.Count > 0)
            {
                // NOTE this is a simplified version of the corresponding Java code. 
                string series = string.Join(", ", key.Select(kv => kv.Code));
                string unknownConcept = string.Join(", ", unknownConcepts);
                throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "Unknown concept(s) '{0}' reported for series : {1}", unknownConcept, series));
            }

            var attributes = new List<IKeyValue>();
            try
            {
                this.ProcessAttributes(this._seriesAttributes, attributes);
            }
            catch (SdmxException e)
            {
                throw new SdmxException("Error while processing series attributes", e);
            }

            // Clear values
            this._keyValues.Clear();
            this._attributeValues.Clear();

            if (this.IsTimeSeries)
            {
                if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.Series)
                {
                    // NOTE Java has a try catch because it needs change the exception to a runtime exception. In .NET we should not do this because 
                    // all exceptions in C# are run time exceptions. Doing it makes it harder to diagnose problems. 
                    while (this.RunAheadParser.Read())
                    {
                        var nodeType = this.RunAheadParser.NodeType;
                        string localName = this.RunAheadParser.LocalName;
                        if (nodeType == XmlNodeType.Element)
                        {
                            if (ElementNameTable.Obs.Is(localName))
                            {
                                this.ProcessObservation(this.RunAheadParser);
                                timeFormat = DateUtil.GetTimeFormatOfDate(this._obsTime);
                                break;
                            }
                        }
                        else if (nodeType == XmlNodeType.EndElement)
                        {
                            if (ElementNameTable.Series.Is(localName))
                            {
                                break;
                            }
                        }
                    }
                }

                this.CurrentKeyValue = this.CreateKeyable(key, attributes, timeFormat);
            }
            else
            {
                this.CurrentKeyValue = this.CreateKeyable(key, attributes, timeFormat, timeValue);
            }

            if (this.DatasetPositionInternal == Api.Constants.DatasetPosition.ObservationAsSeries)
            {
                this.CurrentObs = this.ProcessObsNode(this.Parser);

                // unused variable in java 1.1.4. Possibly a bug in Java SdmxSource 1.1.4?
                timeFormat = this.CurrentObs.ObsTimeFormat;
            }

            this._attributeValues.Clear();
            return this.CurrentKeyValue;
        }

        /// <summary>
        /// Sets the current DSD.
        /// </summary>
        /// <param name="currentDsd">
        /// The current DSD.
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// Time series without time dimension
        /// </exception>
        protected override void SetCurrentDsd(IDataStructureObject currentDsd)
        {
            base.SetCurrentDsd(currentDsd);

            // reset all maps
            this._dimensionConcepts = new List<string>();
            this._datasetAttributes = new HashSet<string>(StringComparer.Ordinal);
            this._seriesAttributes = new HashSet<string>(StringComparer.Ordinal);
            this._observationAttributes = new HashSet<string>(StringComparer.Ordinal);
            this._groups = new List<string>();
            this._groupConcepts = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            if (this.DatasetHeader.DataStructureReference != null)
            {
                this.SetDimensionAtObservation(this.DatasetHeader.DataStructureReference.DimensionAtObservation);
            }
            else
            {
                this.SetDimensionAtObservation(DimensionObject.TimeDimensionFixedId);
            }

            // Roll up any attribute values
            foreach (var valuePair in this._attributesOnDatasetNode)
            {
                var component = currentDsd.GetComponent(valuePair.Key);
                if (component != null)
                {
                    this._rolledUpAttributes.Add(valuePair);
                }
            }

            // Create a list of dimension concepts
            foreach (var dimension in currentDsd.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension))
            {
                this._dimensionConcepts.Add(dimension.Id);
            }

            // Create a list of dataset attribute concepts
            foreach (var datasetAttribute in currentDsd.DatasetAttributes)
            {
                this._datasetAttributes.Add(datasetAttribute.Id);
            }

            // Create a list of dimension group attribute concepts
            foreach (var dimensionGroupAttribute in currentDsd.DimensionGroupAttributes)
            {
                this._seriesAttributes.Add(dimensionGroupAttribute.Id);
            }

            // Create a list of observation attribute concepts
            foreach (var observationAttribute in currentDsd.ObservationAttributes)
            {
                this._observationAttributes.Add(observationAttribute.Id);
            }

            this._primaryMeasureConcept = currentDsd.PrimaryMeasure.Id;

            if (currentDsd.TimeDimension == null)
            {
                throw new SdmxNotImplementedException(string.Format(CultureInfo.InvariantCulture, "The DSD: {0} has no time dimension. This is unsupported!", currentDsd.Id));
            }

            this._timeConcept = currentDsd.TimeDimension.Id;

            foreach (var dsdGroup in currentDsd.Groups)
            {
                string groupId = dsdGroup.Id;
                this._groups.Add(groupId);

                ISet<string> groupAttributes = new HashSet<string>(currentDsd.GetGroupAttributes(groupId, true).Select(o => o.Id), StringComparer.Ordinal);
                this._groupAttributeConcepts.Add(groupId, groupAttributes);

                var groups = new List<string>();
                PopulateGroupContents(dsdGroup, groups);
                this._groupConcepts.Add(groupId, groups);
            }
        }

        /// <summary>
        /// Populates the group contents.
        /// </summary>
        /// <param name="groupObject">
        /// The group object.
        /// </param>
        /// <param name="groups">
        /// The groups.
        /// </param>
        private static void PopulateGroupContents(IGroup groupObject, ICollection<string> groups)
        {
            foreach (var dimensionId in groupObject.DimensionRefs)
            {
                groups.Add(dimensionId);
            }
        }

        /// <summary>
        ///     Clears the OBS information.
        /// </summary>
        private void ClearObsInformation()
        {
            // Clear values
            this._attributeValues.Clear();

            // Clear the current Obs information
            this._obsTime = null;
            this._obsValue = null;
            this._attributes = new List<IKeyValue>();
            this._crossSection = null;
        }

        /// <summary>
        /// Gets the component unique identifier.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <returns>
        /// the component unique identifier.
        /// </returns>
        private string GetComponentId(IComponent component)
        {
            if (component == null)
            {
                return null;
            }

            if (this.IsTwoPointOne)
            {
                return component.Id;
            }

            return ConceptRefUtil.GetConceptId(component.ConceptRef);
        }

        /// <summary>
        /// Processes the attributes.
        /// </summary>
        /// <param name="attributeConcepts">
        /// The attribute concepts.
        /// </param>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Unknown attribute ' + attribute + ' reported in the data.  This attribute is
        ///     not defined by the data structure definition
        /// </exception>
        private void ProcessAttributes(ICollection<string> attributeConcepts, ICollection<IKeyValue> attributes)
        {
            foreach (var attributeConcept in attributeConcepts)
            {
                string conceptValue;
                if ((this._attributeValues.TryGetValue(attributeConcept, out conceptValue) || this._rolledUpAttributes.TryGetValue(attributeConcept, out conceptValue)) && conceptValue != null)
                {
                    IKeyValue kv = new KeyValueImpl(conceptValue, attributeConcept);
                    attributes.Add(kv);
                }
            }

            if (this._attributeValues.Count != attributes.Count)
            {
                string attribute = this._attributeValues.Keys.FirstOrDefault(s => !attributeConcepts.Contains(s));
                if (attribute != null)
                {
                    throw new SdmxSemmanticException("Unknown attribute '" + attribute + "' reported in the data.  This attribute is not defined by the data structure definition");
                }
            }
        }

        /// <summary>
        ///     Processes the data set node.
        /// </summary>
        private void ProcessDataSetNode()
        {
            this._rolledUpAttributes = new Dictionary<string, string>(StringComparer.Ordinal);
            this._keyValues = new Dictionary<string, string>(StringComparer.Ordinal);
            this._attributeValues = new Dictionary<string, string>(StringComparer.Ordinal);
            this._attributesOnDatasetNode = new Dictionary<string, string>(StringComparer.Ordinal);

            this.DatasetHeader = new DatasetHeaderCore(this.Parser, this.Header);

            for (int i = 0; i < this.Parser.AttributeCount; i++)
            {
                // WARN different behavior than in Java
                this.Parser.MoveToAttribute(i);
                this._attributesOnDatasetNode.Add(GetComponentId(this.Parser.LocalName), this.Parser.Value);
            }
        }

        /// <summary>
        /// Processes the observation.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        private void ProcessObservation(XmlReader parser)
        {
            this.ClearObsInformation();

            for (int i = 0; i < parser.AttributeCount; i++)
            {
                parser.MoveToAttribute(i);
                string attributeId = this.GetComponentId(parser.LocalName);
                string attributeValue = parser.Value;
                if (!this.IsTimeSeries && attributeId.Equals(this.CrossSectionConcept))
                {
                    this._crossSection = new KeyValueImpl(attributeValue, attributeId);
                }
                else if (this._observationAttributes.Contains(attributeId))
                {
                    this._attributeValues.Add(attributeId, attributeValue);
                }
                else if (attributeId.Equals(this._primaryMeasureConcept))
                {
                    this._obsValue = attributeValue;
                }
                else if (attributeId.Equals(this._timeConcept))
                {
                    this._obsTime = attributeValue;
                }
            }

            try
            {
                this.ProcessAttributes(this._observationAttributes, this._attributes);
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException("Error while processing observation attributes", e);
            }

            // NOTE the current code in Java SdmxSource v1.1.4 possibly needs to be revisit
            try
            {
                if (!this.IsTimeSeries && this._crossSection == null)
                {
                    throw new SdmxSemmanticException(
                        string.Format("Error while processing observation for series '{0}' , missing required concept '{1}'", this.CurrentKeyValue, this.CrossSectionConcept));
                }
            }
            catch (Exception e)
            {
                if (this.CurrentKeyValue != null)
                {
                    throw new SdmxSemmanticException(string.Format("Error while processing observation for key {0}", this.CurrentKeyValue), e);
                }

                throw new SdmxSemmanticException("Error while processing observation", e);
            }

            // clear values
            this._attributeValues.Clear();
        }

        /// <summary>
        /// Sets the dimension attribute observation.
        /// </summary>
        /// <param name="dimensionAtObservation">
        /// The dimension attribute observation.
        /// </param>
        private void SetDimensionAtObservation(string dimensionAtObservation)
        {
            this._observationAttributes = new HashSet<string>(StringComparer.Ordinal);
            this._seriesAttributes = new HashSet<string>(StringComparer.Ordinal);
            foreach (var attributeObject in this.CurrentDsdInternal.GetSeriesAttributes(dimensionAtObservation))
            {
                this._seriesAttributes.Add(this.GetComponentId(attributeObject));
            }

            foreach (var attributeObject in this.CurrentDsdInternal.GetObservationAttributes(dimensionAtObservation))
            {
                this._observationAttributes.Add(this.GetComponentId(attributeObject));
            }
        }

        #endregion
    }
}
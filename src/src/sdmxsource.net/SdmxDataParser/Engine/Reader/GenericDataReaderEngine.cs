// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-07-01
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The generic data reader engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Xml;

    /// <summary>
    /// The generic data reader engine.
    /// </summary>
    public class GenericDataReaderEngine : AbstractSdmxDataReaderEngine
    {
        #region Fields

        /// <summary>
        ///     The attributes on dataset node
        /// </summary>
        private IDictionary<string, string> _attributesOnDatasetNode = new Dictionary<string, string>(StringComparer.Ordinal);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataReaderEngine"/> class.
        /// </summary>
        /// <param name="dataLocation">
        /// The data Location.
        /// </param>
        /// <param name="defaultDataflow">
        /// The default Dataflow. (Optional)
        /// </param>
        /// <param name="defaultDsd">
        /// The default DSD. The default DSD to use.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// AbstractDataReaderEngine expects either a ISdmxObjectRetrievalManager or a
        ///     IDataStructureObject to be able to interpret the structures
        /// </exception>
        public GenericDataReaderEngine(IReadableDataLocation dataLocation, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
            : this(dataLocation, null, defaultDataflow, defaultDsd)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataReaderEngine"/> class.
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
        public GenericDataReaderEngine(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager objectRetrieval, IDataflowObject defaultDataflow, IDataStructureObject defaultDsd)
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
            return new GenericDataReaderEngine(this.DataLocation, this.ObjectRetrieval, this.CurrentDataflowInternal, this.CurrentDsdInternal);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Move to the next OBS.
        /// </summary>
        /// <param name="includeObs">
        /// if set to <c>true</c> [include OBS].
        /// </param>
        /// <returns>
        /// True if it successfully moves to the next OBS; otherwise false;
        /// </returns>
        protected override bool Next(bool includeObs)
        {
            while (this.Parser.Read())
            {
                var nodeType = this.Parser.NodeType;
                string nodeName;
                if (nodeType == XmlNodeType.Element)
                {
                    // TODO check why java uses ignore case for DataSet check since both SDMX v2.0 and v2.1 use the same casing.
                    nodeName = this.Parser.LocalName;
                    ElementNameTable elementName;
                    if (Enum.TryParse(nodeName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.DataSet:
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Dataset;
                                this._attributesOnDatasetNode = new Dictionary<string, string>(StringComparer.Ordinal);
                                this.DatasetHeader = new DatasetHeaderCore(this.Parser, this.Header);
                                string dsdId = this.ProcessDatasetNode();
                                IDatasetStructureReference dataStructureReference = this.DatasetHeader.DataStructureReference;
                                IStructureReference structureReference = null;
                                string id = null;
                                Uri serviceUrl = null;
                                Uri structureUrl = null;
                                string dimensionAtObservation = null;
                                if (dataStructureReference != null)
                                {
                                    id = dataStructureReference.Id;
                                    serviceUrl = dataStructureReference.ServiceUrl;
                                    structureUrl = dataStructureReference.StructureUrl;
                                    dimensionAtObservation = dataStructureReference.DimensionAtObservation;
                                    structureReference = dataStructureReference.StructureReference;
                                }

                                if (structureReference == null)
                                {
                                    if (this.DefaultDsd != null && this.DefaultDsd.Id.Equals(dsdId))
                                    {
                                        structureReference = this.DefaultDsd.AsReference;
                                    }
                                    else
                                    {
                                        structureReference = new StructureReferenceImpl(null, dsdId, MaintainableObject.DefaultVersion, SdmxStructureEnumType.Dsd);
                                    }
                                }

                                dataStructureReference = new DatasetStructureReferenceCore(id, structureReference, serviceUrl, structureUrl, dimensionAtObservation);
                                this.DatasetHeader = this.DatasetHeader.ModifyDataStructureReference(dataStructureReference);
                                return true;
                            case ElementNameTable.Series:
                                StaxUtil.SkipToEndNode(this.RunAheadParser, ElementNameTable.SeriesKey.FastToString());
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Series;
                                return true;
                            case ElementNameTable.Group:
                                StaxUtil.SkipToEndNode(this.RunAheadParser, ElementNameTable.GroupKey.FastToString());
                                this.DatasetPositionInternal = Api.Constants.DatasetPosition.Group;

                                // TODO test in Java it is possible to ignore the namespace. 
                                this.GroupId = this.Parser.GetAttribute(AttributeNameTable.type);
                                return true;
                            case ElementNameTable.Obs:
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
                            case ElementNameTable.Annotations:
                            case ElementNameTable.Attributes:
                                this.Parser.Skip();
                                break;
                            case ElementNameTable.KeyFamilyRef:
                            case ElementNameTable.Time:
                            case ElementNameTable.ObsValue:
                            case ElementNameTable.ObsDimension:
                            case ElementNameTable.SeriesKey:
                            case ElementNameTable.Value:
                            case ElementNameTable.GroupKey:
                                break;
                            default:
                                throw new SdmxSyntaxException("Unexpected Node in XML: " + nodeName);
                        }
                    }
                }
                else if (nodeType == XmlNodeType.EndElement)
                {
                    nodeName = this.Parser.LocalName;
                    if (ElementNameTable.Series.Is(nodeName) || ElementNameTable.Group.Is(nodeName))
                    {
                        this.DatasetPositionInternal = Api.Constants.DatasetPosition.Null;
                    }
                }
            }

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
            var key = this.GetKeyValues(ElementNameTable.GroupKey);
            IList<IKeyValue> attributes = null;

            // Send Run ahead parser to check the next node, if it is the attributes node then process it, otherwise stop
            while (this.RunAheadParser.Read())
            {
                var nodeType = this.RunAheadParser.NodeType;
                if (nodeType == XmlNodeType.Element)
                {
                    if (ElementNameTable.Attributes.Is(this.RunAheadParser.LocalName))
                    {
                        attributes = this.GetKeyValues(ElementNameTable.Attributes);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            this.CurrentKeyValue = this.CreateKeyable(key, attributes, this.GroupId);
            return this.CurrentKeyValue;
        }

        /// <summary>
        /// Processes the OBS node.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IObservation"/>.
        /// </returns>
        protected override IObservation ProcessObsNode(XmlReader parser)
        {
            string obsDimension = null;
            string obsValue = null;
            IList<IKeyValue> attributes = null;
            string text = null;
            while (parser.Read())
            {
                var nodeType = parser.NodeType;
                if (nodeType == XmlNodeType.Element)
                {
                    string nodeName = parser.LocalName;
                    if (ElementNameTable.ObsDimension.Is(nodeName))
                    {
                        obsDimension = parser.GetAttribute(AttributeNameTable.value);
                    }
                    else if (ElementNameTable.ObsValue.Is(nodeName))
                    {
                        obsValue = parser.GetAttribute(AttributeNameTable.value);
                    }
                    else if (ElementNameTable.Attributes.Is(nodeName))
                    {
                        attributes = this.GetKeyValues(ElementNameTable.Attributes);
                    }
                }
                else if (nodeType == XmlNodeType.Text)
                {
                    text = parser.Value;
                }
                else if (nodeType == XmlNodeType.EndElement)
                {
                    string nodeName = parser.LocalName;
                    if (ElementNameTable.Time.Is(nodeName))
                    {
                        obsDimension = text;
                    }
                    else if (ElementNameTable.Obs.Is(nodeName))
                    {
                        break;
                    }
                }
            }

            obsDimension = this.GetComponentId(obsDimension);

            try
            {
                if (this.IsTimeSeries)
                {
                    return new ObservationImpl(this.CurrentKeyValue, obsDimension, obsValue, attributes);
                }

                if (obsDimension == null)
                {
                    throw new SdmxSemmanticException(
                        string.Format("Error while processing observation for series '{0}' , missing required cross sectional concept value '{1}'", this.CurrentKey, this.CrossSectionConcept));
                }

                IKeyValue crossSection = new KeyValueImpl(obsDimension, this.CrossSectionConcept);
                return new ObservationImpl(this.CurrentKeyValue, this.CurrentKeyValue.ObsTime, obsValue, attributes, crossSection);
            }
            catch (SdmxException)
            {
                throw;
            }
            catch (Exception th)
            {
                if (this.CurrentKey != null)
                {
                    throw new SdmxException("Error while processing observation for key " + this.CurrentKeyValue, th);
                }

                throw new SdmxException("Error while processing observation");
            }
        }

        /// <summary>
        ///     Processes the series node.
        /// </summary>
        /// <returns>
        ///     The <see cref="IKeyable" />.
        /// </returns>
        protected override IKeyable ProcessSeriesNode()
        {
            IList<IKeyValue> key = this.GetKeyValues(this.NoSeries ? ElementNameTable.ObsKey : ElementNameTable.SeriesKey);
            IList<IKeyValue> attributes = null;
            TimeFormat timeFormat = null;
            string obsValue = null;
            string obsConcept = null;

            // Send Run ahead parser to check the next node, if it is the attributes node then process it, 
            // Also move forward to the first obs and get the time format, if we hit the end series node, without finding any obs then stop
            bool inSeries = true;
            string text = null;
            while (this.RunAheadParser.Read())
            {
                XmlNodeType xmlNodeType = this.RunAheadParser.NodeType;
                if (xmlNodeType == XmlNodeType.Element)
                {
                    string nodeName = this.RunAheadParser.LocalName;

                    if (inSeries && ElementNameTable.Attributes.Is(nodeName))
                    {
                        attributes = this.GetKeyValues(ElementNameTable.Attributes);
                    }
                    else if (ElementNameTable.Obs.Is(nodeName))
                    {
                        if (!this.NoSeries)
                        {
                            // We are no longer in a series, so we don't want to process obs attributes here
                            inSeries = false;
                        }
                    }
                }
                else if (xmlNodeType == XmlNodeType.Text)
                {
                    text = this.RunAheadParser.Value;
                }
                else if (xmlNodeType == XmlNodeType.EndElement)
                {
                    string nodeName = this.RunAheadParser.LocalName;
                    if (ElementNameTable.Time.Is(nodeName) || ElementNameTable.ObsDimension.Is(nodeName))
                    {
                        obsConcept = text;
                        break;
                    }

                    if (ElementNameTable.ObsValue.Is(nodeName))
                    {
                        obsValue = text;
                    }
                    else if ((this.NoSeries && ElementNameTable.Obs.Is(nodeName)) || ElementNameTable.Series.Is(nodeName))
                    {
                        break;
                    }
                }
            }

            if (inSeries)
            {
                var seriesKeyValue = new List<IKeyValue>();
                string obsTime = null;
                foreach (var currentKeyValue in key)
                {
                    if (currentKeyValue.Concept.Equals(DimensionObject.TimeDimensionFixedId))
                    {
                        obsTime = currentKeyValue.Code;
                    }
                    else
                    {
                        seriesKeyValue.Add(currentKeyValue);
                    }
                }

                var seriesAttributes = new List<IKeyValue>();
                var obsAttributes = new List<IKeyValue>();
                var obsAttributeIds = new HashSet<string>(StringComparer.Ordinal);

                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (obsAttributeIds.Contains(attribute.Concept))
                        {
                            obsAttributes.Add(attribute);
                        }
                        else
                        {
                            seriesAttributes.Add(attribute);
                        }
                    }
                }

                timeFormat = DateUtil.GetTimeFormatOfDate(obsTime);
                this.CurrentKeyValue = CreateKeyable(seriesKeyValue, seriesAttributes, timeFormat);
                this.CurrentObs = new ObservationImpl(this.CurrentKeyValue, obsTime, obsValue, obsAttributes);
            }
            else
            {
                if (this.IsTimeSeries)
                {
                    if (obsConcept != null)
                    {
                        timeFormat = DateUtil.GetTimeFormatOfDate(obsConcept);
                    }

                    this.CurrentKeyValue = CreateKeyable(key, attributes, timeFormat);
                }
                else
                {
                    var seriesKeyValue = new List<IKeyValue>();
                    string crossSectionTime = null;
                    foreach (var keyValue in key)
                    {
                        if (keyValue.Concept.Equals(DimensionObject.TimeDimensionFixedId))
                        {
                            crossSectionTime = keyValue.Code;
                            timeFormat = DateUtil.GetTimeFormatOfDate(crossSectionTime);
                        }
                        else
                        {
                            seriesKeyValue.Add(keyValue);
                        }
                    }

                    this.CurrentKeyValue = CreateKeyable(seriesKeyValue, attributes, timeFormat, crossSectionTime);
                }
            }

            return this.CurrentKeyValue;
        }

        /// <summary>
        /// The get key values.
        /// </summary>
        /// <param name="endElement">
        /// The end element.
        /// </param>
        /// <returns>
        /// The <see cref="IList{IKeyValue}"/>.
        /// </returns>
        private IList<IKeyValue> GetKeyValues(ElementNameTable endElement)
        {
            IList<IKeyValue> returnList = new List<IKeyValue>();
            while (this.Parser.Read())
            {
                var nodeType = this.Parser.NodeType;
                switch (nodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string nodeName = this.Parser.LocalName;
                            if (ElementNameTable.Value.Is(nodeName))
                            {
                                string componentVal = this.Parser.GetAttribute(AttributeNameTable.value);
                                if (this.IsTwoPointOne)
                                {
                                    returnList.Add(new KeyValueImpl(componentVal, this.Parser.GetAttribute(AttributeNameTable.id)));
                                }
                                else
                                {
                                    string componentId = this.GetComponentId(this.Parser.GetAttribute(AttributeNameTable.concept));
                                    returnList.Add(new KeyValueImpl(componentVal, componentId));
                                }
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (endElement.Is(this.Parser.LocalName))
                        {
                            return returnList;
                        }

                        break;
                }
            }

            return returnList;
        }

        /// <summary>
        ///     Sends the runAheadParser ahead to see if there are any dataset attributes, will stop running ahead if it finds a
        ///     series, group or end of dataset element
        /// </summary>
        /// <returns>the DSD id referenced.</returns>
        private string ProcessDatasetNode()
        {
            string text = null;
            while (this.RunAheadParser.Read())
            {
                var nodeType = this.RunAheadParser.NodeType;
                switch (nodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string nodeName = this.RunAheadParser.LocalName;
                            if (ElementNameTable.Attributes.Is(nodeName))
                            {
                                IList<IKeyValue> attributes = this.GetKeyValues(ElementNameTable.Attributes);
                                foreach (var attribute in attributes)
                                {
                                    this._attributesOnDatasetNode.Add(attribute.Concept, attribute.Code);
                                }
                            }
                            else if (this.IsTwoPointOne)
                            {
                                if (ElementNameTable.Series.Is(nodeName) || ElementNameTable.Group.Is(nodeName))
                                {
                                    return null;
                                }
                            }
                        }

                        break;
                    case XmlNodeType.Text:
                        text = this.RunAheadParser.Value;
                        break;
                    case XmlNodeType.EndElement:
                        string localName = this.RunAheadParser.LocalName;
                        if (ElementNameTable.KeyFamilyRef.Is(localName))
                        {
                            return text;
                        }

                        if (ElementNameTable.DataSet.Is(localName))
                        {
                            return null;
                        }

                        break;
                }
            }

            return null;
        }

        #endregion
    }
}
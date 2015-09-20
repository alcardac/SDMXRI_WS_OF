// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappedValues.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Holds the mapped and transcoded values following the series structure
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DsplDataFormat.Retriever.Model
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// Holds the mapped and transcoded values following the series structure
    /// </summary>
    internal class MappedValues : MappedValuesBase, IMappedValues
    {
        #region Constants and Fields

        /// <summary>
        ///   dataset attribute values
        /// </summary>
        private readonly List<ComponentValue> _attributeDataSetValues = new List<ComponentValue>();

        /// <summary>
        ///   group attribute values. It should contain only the attributes of a specific group.
        /// </summary>
        private readonly List<ComponentValue> _attributeGroupValues = new List<ComponentValue>();

        /// <summary>
        ///   OBS attribute values
        /// </summary>
        private readonly List<ComponentValue> _attributeObservationValues = new List<ComponentValue>();

        /// <summary>
        ///   series attribute values
        /// </summary>
        private readonly List<ComponentValue> _attributeSeriesValues = new List<ComponentValue>();

        /// <summary>
        ///   All dimension values
        /// </summary>
        private readonly List<ComponentValue> _dimensionValues = new List<ComponentValue>();

        /// <summary>
        ///   The dimension key values
        /// </summary>
        private readonly List<ComponentValue> _keyValues = new List<ComponentValue>();

        /// <summary>
        ///   The previous key values
        /// </summary>
        private readonly string[] _previousKeyValues;

        /// <summary>
        ///   The list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        private readonly List<ComponentValue> _xsMeasureCachedSeriesAttribute = new List<ComponentValue>();

        /// <summary>
        ///   The list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        private readonly List<ComponentValue> _xsMeasureCachedSeriesKey = new List<ComponentValue>();

        /// <summary>
        ///   XS Measure cache. It is used only when XS measures are mapped.
        /// </summary>
        private readonly IDictionary<ComponentEntity, XsMeasureCache> _xsMeasureCaches =
            new Dictionary<ComponentEntity, XsMeasureCache>();

        /// <summary>
        ///   CrossSectional measure values
        /// </summary>
        private readonly List<ComponentValue> _xsMeasures = new List<ComponentValue>();

        /// <summary>
        ///   A value indicating whether the SDMX DSD Attributes attached to DataSet level have been processed.
        /// </summary>
        private bool _startedDataSet;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValues"/> class.
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <param name="componentMappings">
        /// The component Mappings. 
        /// </param>
        public MappedValues(DataRetrievalInfo info, IEnumerable<IComponentMapping> componentMappings)
        {
            this.SetComponentOrder(componentMappings, info);
            this.SetTimeDimensionComponent(info.TimeTranscoder);
            if (info.TimeTranscoder != null && !info.IsTimePeriodAtObservation &&
                    info.TimeTranscoder.Component.ComponentType == SdmxComponentType.TimeDimension)
                this._keyValues.Add(this.TimeDimensionValue);
            this._previousKeyValues = new string[this._keyValues.Count];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValues"/> class.
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        public MappedValues(DataRetrievalInfo info)
            : this(info, info.AllComponentMappings)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attributes attached to DataSet level
        /// </summary>
        public IEnumerable<ComponentValue> AttributeDataSetValues
        {
            get
            {
                return this._attributeDataSetValues;
            }
        }

        /// <summary>
        ///   Gets group attribute values. To avoid extra checks, only attributes and dimensions of a specific group should be added at <see
        ///    cref="MappedValuesBase.Add" /> . So this should contain only the attributes of a specific group.
        /// </summary>
        public IEnumerable<ComponentValue> AttributeGroupValues
        {
            get
            {
                return this._attributeGroupValues;
            }
        }

        /// <summary>
        ///   Gets the Observation attribute values
        /// </summary>
        public IEnumerable<ComponentValue> AttributeObservationValues
        {
            get
            {
                return this._attributeObservationValues;
            }
        }

        /// <summary>
        ///   Gets the series attribute values
        /// </summary>
        public IEnumerable<ComponentValue> AttributeSeriesValues
        {
            get
            {
                return this._attributeSeriesValues;
            }
        }

        /// <summary>
        ///   Gets all dimension values. This means either the series dimensions (all of them except time) or the dimensions of a specific group.
        /// </summary>
        public IList<ComponentValue> DimensionValues
        {
            get
            {
                return this._dimensionValues;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the SDMX DSD Attributes attached to DataSet level have been processed.
        /// </summary>
        public bool StartedDataSet
        {
            get
            {
                return this._startedDataSet;
            }

            set
            {
                this._startedDataSet = value;
            }
        }

        /// <summary>
        ///   Gets the list of XS Measure buffer. It is used only when XS measures are mapped.
        /// </summary>
        public IEnumerable<KeyValuePair<ComponentEntity, XsMeasureCache>> XSMeasureCaches
        {
            get
            {
                return this._xsMeasureCaches;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add <paramref name="xsComponent"/> to buffer
        /// </summary>
        /// <param name="xsComponent">
        /// The XS Measure to add to buffer 
        /// </param>
        public void AddToBuffer(ComponentEntity xsComponent)
        {
            XsMeasureCache xsMeasureCache;
            if (!this._xsMeasureCaches.TryGetValue(xsComponent, out xsMeasureCache))
            {
                xsMeasureCache = new XsMeasureCache(
                    this._xsMeasureCachedSeriesKey,
                    this._xsMeasureCachedSeriesAttribute,
                    xsComponent.CrossSectionalMeasureCode);
                this._xsMeasureCaches.Add(xsComponent, xsMeasureCache);
            }

            xsMeasureCache.XSMeasureCachedObservations.Add(
                new KeyValuePair<string, string>(this.TimeValue, this.GetXSMeasureValue(xsComponent)));
            xsMeasureCache.Attributes.Add(new List<ComponentValue>(this._attributeObservationValues));
        }

        /// <summary>
        /// Initialize the buffers used for mappings with XS Measures mapped.
        /// </summary>
        public void InitXsBuffer()
        {
            this._xsMeasureCaches.Clear();

            // we need to copy to avoid overwriting with new values
            CopyComponentValues(this._dimensionValues, this._xsMeasureCachedSeriesKey);
            CopyComponentValues(this._attributeSeriesValues, this._xsMeasureCachedSeriesAttribute);
        }

        /// <summary>
        /// Gets a value indicating whether this is a new key or it is the same as the last one.
        /// </summary>
        /// <returns>
        /// a value indicating whether this is a new key or it is the same as the last one. 
        /// </returns>
        public bool IsNewKey()
        {
            bool ret = !EqualKeyValues(this._keyValues, this._previousKeyValues);
            if (ret)
            {
                CopyValues(this._keyValues, this._previousKeyValues);
            }

            return ret;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear the <paramref name="destination"/> and copy all items from <paramref name="source"/> to <paramref name="destination"/>
        /// </summary>
        /// <param name="source">The source list of <see cref="ComponentValue"/></param>
        /// <param name="destination">The destination list of <see cref="ComponentValue"/></param>
        private static void CopyComponentValues(IEnumerable<ComponentValue> source, ICollection<ComponentValue> destination)
        {
            destination.Clear();
            foreach (var componentValue in source)
            {
                destination.Add(new ComponentValue(componentValue.Key) { Value = componentValue.Value });
            }
        }

        /// <summary>
        /// Gets the value for the specified <paramref name="component"/>
        /// </summary>
        /// <param name="component">
        /// The CrossSectional measure 
        /// </param>
        /// <returns>
        /// the value for the specified <paramref name="component"/> 
        /// </returns>
        private string GetXSMeasureValue(ComponentEntity component)
        {
            return this._xsMeasures.Find(x => x.Key.Equals(component)).Value;
        }

        /// <summary>
        /// Initialize the internal order of the components based on the specified <paramref name="componentMappings"/>
        /// </summary>
        /// <param name="componentMappings">
        /// The component mappings 
        /// </param>
        /// /// <param name="info">
        /// The data retrieval info 
        /// </param>
        private void SetComponentOrder(IEnumerable<IComponentMapping> componentMappings, DataRetrievalInfo info)
        {
            foreach (IComponentMapping componentMapping in componentMappings)
            {
                var componentValue = new ComponentValue(componentMapping.Component);
                this.ComponentValues.Add(componentValue);
                switch (componentMapping.Component.ComponentType)
                {
                    case SdmxComponentType.Dimension:
                        this._dimensionValues.Add(componentValue);
                        if (!componentValue.Key.Id.Equals(info.DimensionAtObservation))
                            this._keyValues.Add(componentValue);
                        break;
                    case SdmxComponentType.Attribute:
                        switch (componentMapping.Component.AttributeAttachmentLevel)
                        {
                            case AttachmentLevel.DataSet:
                                this._attributeDataSetValues.Add(componentValue);
                                break;
                            case AttachmentLevel.Group:
                                // NOTE we expect only the attributes of a specific group to be in _attributeGroupValues
                                this._attributeGroupValues.Add(componentValue);
                                break;
                            case AttachmentLevel.Series:
                                if (IsDimensionObsReference(componentValue, info))
                                    this._attributeObservationValues.Add(componentValue);
                                else
                                    this._attributeSeriesValues.Add(componentValue);
                                break;
                            case AttachmentLevel.Observation:
                                this._attributeObservationValues.Add(componentValue);
                                break;
                        }

                        break;
                    case SdmxComponentType.PrimaryMeasure:
                        this.PrimaryMeasureValue = componentValue;
                        break;
                    case SdmxComponentType.CrossSectionalMeasure:
                        this._xsMeasures.Add(componentValue);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks if the dimension at observation is in the dimension references of the component <paramref name="componentMappings"/>
        /// </summary>
        /// <param name="componentValue">
        /// The component value 
        /// </param>
        /// /// <param name="info">
        /// The data retrieval info 
        /// </param>
        private bool IsDimensionObsReference(ComponentValue componentValue, DataRetrievalInfo info)
        {
            IBaseDataQuery baseDataQuery = (IBaseDataQuery)info.ComplexQuery ?? info.Query;

            foreach (IAttributeObject attr in baseDataQuery.DataStructure.AttributeList.Attributes)
            {
                if (attr.Id.Equals(componentValue.Key.Id))
                {
                    if (attr.DimensionReferences.Contains(info.DimensionAtObservation))
                        return true;
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}
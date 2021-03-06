// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrievalInfo.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds the current state of the data retrieval
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    /// This class holds the current state of the data retrieval
    /// </summary>
    internal class DataRetrievalInfo
    {
        #region Constants and Fields

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DataRetrievalInfo));

        /// <summary>
        /// The _all dimensions
        /// </summary>
        private static readonly string _allDimensions;

        /// <summary>
        ///   Holds the map between component concept id and the ComponentEntity. Used in getting the ComponentEntity from SDMX-ML Query clauses
        /// </summary>
        private readonly Dictionary<string, IComponentMapping> _componentIdMap =
            new Dictionary<string, IComponentMapping>(StringComparer.Ordinal);

        /// <summary>
        ///   A dictionary to find the mapping of each DSD component
        /// </summary>
        private readonly Dictionary<ComponentEntity, MappingEntity> _componentMapping =
            new Dictionary<ComponentEntity, MappingEntity>();

        /// <summary>
        ///   The Mapping Store connection string settings
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        /// The _schema version
        /// </summary>
        private readonly SdmxSchemaEnumType _schemaVersion;

        /// <summary>
        ///   Cross Sectional measures
        /// </summary>
        private readonly List<MappingEntity> _crossSectionalMeasureMappings = new List<MappingEntity>();

        /// <summary>
        ///   The mapping set of the dataflow found in the SDMX query
        /// </summary>
        private readonly MappingSetEntity _mappingSet;

        /// <summary>
        ///   The Measure dimension SDMX Query values if any
        /// </summary>
        private readonly List<string> _measureDimensionQueryValues = new List<string>();

        /// <summary>
        ///   The current SDMX Query object
        /// </summary>
        private readonly IDataQuery _query;

        /// <summary>
        ///   The current SDMX complex Query object
        /// </summary>
        private readonly IComplexDataQuery _complexQuery;

        /// <summary>
        ///   This field holds the XSMeasureMapping which are used in case they are mapped instead of Measure Dimension
        /// </summary>
        private readonly Dictionary<string, MappingEntity> _xsMeasureMappings = new Dictionary<string, MappingEntity>();

        /// <summary>
        /// The _dimension at observation
        /// </summary>
        private readonly string _dimensionAtObservation;

        /// <summary>
        /// A value indicating whether the time period is the dimension at observation.
        /// </summary>
        private readonly bool _isTimePeriodAtObservation;

        /// <summary>
        ///   The list of component mappings
        /// </summary>
        private IComponentMapping[] _componentMappingList;

        /// <summary>
        ///   This field holds if the current dataset is truncated
        /// </summary>
        private bool _isTruncated;

        /// <summary>
        ///   This field holds if the no of records read from DDB
        /// </summary>
        private int _recordsRead;

        /*
        /// <summary>
        /// The "Data Structure Definition" used for the produced Dataset model.
        /// It is a <see cref="Estat.Sdmx.Model.Structure.KeyFamilyBean"/> object
        /// as defined in the package sdmx_model.
        /// </summary>
        private KeyFamilyBean _keyFamilyBean;
*/

        /// <summary>
        ///   The current limit
        /// </summary>
        private int _limit;

        /// <summary>
        ///   Holds the final SQL string
        /// </summary>
        private string _sqlString;

        /// <summary>
        ///   The object used to store the mapping of the time dimension
        /// </summary>
        private MappingEntity _timeMapping;

        /// <summary>
        ///   The object that is used for transcoding the time fields
        /// </summary>
        private ITimeDimension _timeTranscoder;

        /// <summary>
        /// The _dimension at observation mapping
        /// </summary>
        private IComponentMapping _dimensionAtObservationMapping;

        private readonly string _buildEffectiveDimensionAtObservation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="DataRetrievalInfo"/> class.
        /// </summary>
        static DataRetrievalInfo()
        {
            _allDimensions = Org.Sdmxsource.Sdmx.Api.Constants.DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfo"/> class.
        /// </summary>
        /// <param name="mappingSet">
        /// The mapping set of the dataflow found in the sdmx query 
        /// </param>
        /// <param name="query">
        /// The current SDMX Query object 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings 
        /// </param>
        public DataRetrievalInfo(MappingSetEntity mappingSet, IDataQuery query, ConnectionStringSettings connectionStringSettings)
            : this(mappingSet, query, connectionStringSettings, SdmxSchemaEnumType.VersionTwo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfo" /> class.
        /// </summary>
        /// <param name="mappingSet">The mapping set of the dataflow found in the SDMX query</param>
        /// <param name="query">The current SDMX Query object</param>
        /// <param name="connectionStringSettings">The Mapping Store connection string settings</param>
        /// <param name="schemaVersion">The schema version.</param>
        /// <exception cref="System.ArgumentNullException">
        /// mappingSet
        /// or
        /// connectionStringSettings
        /// </exception>
        public DataRetrievalInfo(MappingSetEntity mappingSet, IDataQuery query, ConnectionStringSettings connectionStringSettings, SdmxSchemaEnumType schemaVersion)
        {
            if (mappingSet == null)
            {
                throw new ArgumentNullException("mappingSet");
            }

            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._mappingSet = mappingSet;
            this._query = query;
            this._connectionStringSettings = connectionStringSettings;
            this._schemaVersion = schemaVersion;

            if (schemaVersion == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                this._isTimePeriodAtObservation = DimensionObject.TimeDimensionFixedId.Equals(query.DimensionAtObservation);
                if (!this._isTimePeriodAtObservation)
                {
                    this._dimensionAtObservation = query.DimensionAtObservation;
                }

                this._limit = 0; // REST does not support default limit.
            }
            else
            {
                this._limit = query.FirstNObservations.HasValue ? query.FirstNObservations.Value : 0;
                this._isTimePeriodAtObservation = true;
            }

            this.BuildMappings();
            this._buildEffectiveDimensionAtObservation = this.BuildEffectiveDimensionAtObservation();
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfo"/> class.
        /// </summary>
        /// <param name="mappingSet">
        /// The mapping set of the dataflow found in the SDMX query 
        /// </param>
        /// <param name="query">
        /// The current SDMX Query object 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings 
        /// </param>
        public DataRetrievalInfo(MappingSetEntity mappingSet, IComplexDataQuery query, ConnectionStringSettings connectionStringSettings)
        {
            if (mappingSet == null)
            {
                throw new ArgumentNullException("mappingSet");
            }

            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._schemaVersion = SdmxSchemaEnumType.VersionTwoPointOne;
            this._mappingSet = mappingSet;
            this._complexQuery = query;
            this._connectionStringSettings = connectionStringSettings;
            this._limit = query.DefaultLimit.HasValue ? query.DefaultLimit.Value : 0;
            this._isTimePeriodAtObservation = DimensionObject.TimeDimensionFixedId.Equals(query.DimensionAtObservation);
            if (!this._isTimePeriodAtObservation)
            {
                this._dimensionAtObservation = query.DimensionAtObservation;
            }

            this.BuildMappings();
            this._buildEffectiveDimensionAtObservation = this.BuildEffectiveDimensionAtObservation();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the dimension at observation mapping
        /// </summary>
        public IComponentMapping DimensionAtObservationMapping
        {
            get
            {
                return this._dimensionAtObservationMapping;
            }
        }

        /// <summary>
        ///   Gets the main component
        /// </summary>
        public virtual IList<IComponentMapping> AllComponentMappings
        {
            get
            {
                return this._componentMappingList;
            }
        }

        /// <summary>
        ///   Gets the map between component concept id and the ComponentEntity. Used in getting the ComponentEntity from SDMX-ML Query clauses
        /// </summary>
        public Dictionary<string, IComponentMapping> ComponentIdMap
        {
            get
            {
                return this._componentIdMap;
            }
        }

        /// <summary>
        ///   Gets a dictionary to find the mapping of each DSD component
        /// </summary>
        public Dictionary<ComponentEntity, MappingEntity> ComponentMapping
        {
            get
            {
                return this._componentMapping;
            }
        }

        /// <summary>
        ///   Gets the Mapping Store DataBase Connection String Settings.
        /// </summary>
        public ConnectionStringSettings ConnectionStringSettings
        {
            get
            {
                return this._connectionStringSettings;
            }
        }

        /// <summary>
        ///   Gets the current Cross Sectional Measure mappings
        /// </summary>
        public IList<MappingEntity> CrossSectionalMeasureMappings
        {
            get
            {
                return this._crossSectionalMeasureMappings;
            }
        }

        /// <summary>
        ///   Gets or sets the default SDMX Header
        /// </summary>
        public IHeader DefaultHeader { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the current dataset is truncated
        /// </summary>
        /// <remarks>
        ///   TODO This is not currently supported.
        /// </remarks>
        public bool IsTruncated
        {
            get
            {
                return this._isTruncated;
            }

            set
            {
                this._isTruncated = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the no of records read from DDB
        /// </summary>
        /// <remarks>
        ///   TODO This is not currently supported.
        /// </remarks>
        public int RecordsRead
        {
            get
            {
                return this._recordsRead;
            }

            set
            {
                this._recordsRead = value;
            }
        }

        /// <summary>
        ///   Gets or sets the current limit. Defaults to <see cref="Query" /> 's <see cref="IComplexDataQuery.DefaultLimit" />
        /// </summary>
        public int Limit
        {
            get
            {
                return this._limit;
            }

            set
            {
                this._limit = value;
            }
        }

        /// <summary>
        ///   Gets the mapping set of the dataflow found in the sdmx query
        /// </summary>
        public MappingSetEntity MappingSet
        {
            get
            {
                return this._mappingSet;
            }
        }

        /// <summary>
        ///   Gets the Measure Component
        /// </summary>
        public ComponentEntity MeasureComponent { get; private set; }

        /// <summary>
        /// Gets the effective dimension at observation.
        /// </summary>
        /// <value>
        /// The effective dimension at observation.
        /// </value>
        public string EffectiveDimensionAtObservation
        {
            get
            {
                return this._buildEffectiveDimensionAtObservation;
            }
        }

        /// <summary>
        /// Gets the first N observations per series.
        /// </summary>
        /// <value>
        /// The first N observations per series.
        /// </value>
        public int FirstNObservations
        {
            get
            {
                return this.BaseDataQuery.FirstNObservations.HasValue ? this.BaseDataQuery.FirstNObservations.Value : -1;
            }
        }

        /// <summary>
        /// Gets the last N observations per series.
        /// </summary>
        /// <value>
        /// The last N observations per series.
        /// </value>
        public int LastNObservations
        {
            get
            {
                return this.BaseDataQuery.LastNObservations.HasValue ? this.BaseDataQuery.LastNObservations.Value : -1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has last n observations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has last n observations; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasLastNObservations
        {
            get
            {
                return this._schemaVersion == SdmxSchemaEnumType.VersionTwoPointOne && this.LastNObservations > -1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has first n observations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has first n observations; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasFirstNObservations
        {
            get
            {
                return this._schemaVersion == SdmxSchemaEnumType.VersionTwoPointOne && this.FirstNObservations > -1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has first and last n observations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has first and last n observations; otherwise, <c>false</c>.
        /// </value>
        public bool HasFirstAndLastNObservations
        {
            get
            {
                return this.HasFirstNObservations && this.HasLastNObservations;
            } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance has max OBS per series.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has max OBS per series; otherwise, <c>false</c>.
        /// </value>
        public bool HasMaxObsPerSeries
        {
            get
            {
                return this.HasFirstNObservations || this.HasLastNObservations;
            }
        }

        /// <summary>
        /// Gets the base data query.
        /// </summary>
        /// <value>
        /// The base data query.
        /// </value>
        public IBaseDataQuery BaseDataQuery
        {
            get
            {
                return (IBaseDataQuery)this._query ?? this._complexQuery;
            }
        }

        /// <summary>
        ///   Gets the current SDMX Query object
        /// </summary>
        public IDataQuery Query
        {
            get
            {
                return this._query;
            }
        }

        /// <summary>
        ///   Gets the current SDMX Query object
        /// </summary>
        public IComplexDataQuery ComplexQuery
        {
            get
            {
                return this._complexQuery;
            }
        }

        /// <summary>
        ///   Gets or sets the final SQL string
        /// </summary>
        public string SqlString
        {
            get
            {
                return this._sqlString;
            }

            set
            {
                _logger.DebugFormat("DDB SQL Set to: {0}", value);
                this._sqlString = value;
            }
        }

        /// <summary>
        ///   Gets the object used to store the mapping of the time dimension
        /// </summary>
        public MappingEntity TimeMapping
        {
            get
            {
                return this._timeMapping;
            }
        }

        /// <summary>
        ///   Gets the object that is used for transcoding the time fields
        /// </summary>
        public ITimeDimension TimeTranscoder
        {
            get
            {
                return this._timeTranscoder;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the time period is the dimension at observation.
        /// </summary>
        public bool IsTimePeriodAtObservation
        {
            get
            {
                return this._isTimePeriodAtObservation || this._buildEffectiveDimensionAtObservation.Equals(DimensionObject.TimeDimensionFixedId);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is all dimensions.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is all dimensions; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllDimensions
        {
            get
            {
                return _allDimensions.Equals(this._dimensionAtObservation);
            }
        }

        /// <summary>
        /// Gets the dimension at observation
        /// </summary>
        public string DimensionAtObservation
        {
            get
            {
                return this._dimensionAtObservation;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Getter for the list of available XS Measures in case the XS Measures are mapped
        /// </summary>
        /// <returns>
        /// A list of available XS Measures Mappings or null in case they were not mapped 
        /// </returns>
        public IList<MappingEntity> BuildXSMeasures()
        {
            if (this._crossSectionalMeasureMappings.Count > 0)
            {
                return this._crossSectionalMeasureMappings;
            }

            List<MappingEntity> collection = null;
            if (this.MeasureComponent != null)
            {
                if (this._measureDimensionQueryValues.Count == 0)
                {
                    this._measureDimensionQueryValues.AddRange(
                        GetFromQueryDimensionValues(this._query, this.MeasureComponent));
                }

                collection = new List<MappingEntity>();
                if (this._measureDimensionQueryValues.Count > 0)
                {
                    // only in case the measure dimension is not mapped and there are conditions for measure dimension
                    foreach (string queryValue in this._measureDimensionQueryValues)
                    {
                        MappingEntity mapping;
                        if (this._xsMeasureMappings.TryGetValue(queryValue, out mapping))
                        {
                            collection.Add(mapping);
                        }
                    }
                }
                else
                {
                    collection.AddRange(this._xsMeasureMappings.Values);
                }

                // cache results
                this._crossSectionalMeasureMappings.AddRange(collection);
            }

            return collection;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the effective dimension at observation.
        /// </summary>
        /// <returns>The effective dimension at observation.</returns>
        private string BuildEffectiveDimensionAtObservation()
        {
            if (this._isTimePeriodAtObservation)
            {
                return DimensionObject.TimeDimensionFixedId;
            }

            if (_allDimensions.Equals(this._dimensionAtObservation))
            {
                if (this.TimeMapping != null)
                {
                    return DimensionObject.TimeDimensionFixedId;
                }

                return this._mappingSet.Dataflow.Dsd.Dimensions.Last().Id;
            }

            return this._dimensionAtObservation;
        }

        /// <summary>
        /// Gets the list of <paramref name="dimension"/> values inside a <paramref name="query"/>
        /// </summary>
        /// <param name="query">
        /// The SDMX Model query 
        /// </param>
        /// <param name="dimension">
        /// The <see cref="ComponentEntity"/> of a dimension 
        /// </param>
        /// <returns>
        /// the list of <paramref name="dimension"/> values inside a <paramref name="query"/> 
        /// </returns>
        private static IEnumerable<string> GetFromQueryDimensionValues(IDataQuery query, ComponentEntity dimension)
        {
            var xsMeasureValues = new List<string>();

            foreach (IDataQuerySelectionGroup sg in query.SelectionGroups)
            {
                if (sg.HasSelectionForConcept(dimension.Id))
                {
                    var selection = sg.GetSelectionsForConcept(dimension.Id);
                    if (selection.HasMultipleValues)
                    {
                        xsMeasureValues.AddRange(selection.Values);
                    }
                    else
                    {
                        xsMeasureValues.Add(selection.Value);
                    }
                }
            }

            return xsMeasureValues;
        }

        /// <summary>
        /// Builds the component to Mapping dictionary. Before building it the methods make sure that the dictionary is empty
        /// </summary>
        private void BuildMappings()
        {
            // do some safety work
            bool measureDimensionMapped = false;
            IComponentMapping frequencyMapping = null;

            // fill the dictionaries and the fields
            foreach (MappingEntity mapping in this._mappingSet.Mappings)
            {
                if (this.IsTimeDimensionMapping(mapping))
                {
                    // find the mapping that contains the timedimension
                    this._timeMapping = mapping;
                    this._componentMapping.Add(mapping.Components[0], mapping);
                }
                else
                {
                    foreach (ComponentEntity component in mapping.Components)
                    {
                        this._componentMapping.Add(component, mapping);

                        IComponentMapping componentMapping = Sri.MappingStoreRetrieval.Engine.Mapping.ComponentMapping.CreateComponentMapping(component, mapping);

                        //// TODO REMOVE this._componentMappingType.Add(component, componentMapping);
                        this._componentIdMap.Add(component.Id, componentMapping);

                        switch (component.ComponentType)
                        {
                            case SdmxComponentType.Dimension:
                                if (component.MeasureDimension)
                                {
                                    measureDimensionMapped = true;
                                }

                                if (component.FrequencyDimension)
                                {
                                    frequencyMapping = componentMapping;
                                }

                                if (component.Id.Equals(this._dimensionAtObservation))
                                {
                                    this._dimensionAtObservationMapping = componentMapping;
                                }

                                break;
                            case SdmxComponentType.CrossSectionalMeasure:
                                this._xsMeasureMappings.Add(component.CrossSectionalMeasureCode, mapping);
                                break;
                        }
                    }
                }
            }

            this._componentMappingList = new IComponentMapping[this._componentIdMap.Count];
            this._componentIdMap.Values.CopyTo(this._componentMappingList, 0);
            if (this._timeMapping != null)
            {
                    this._timeTranscoder = TimeDimensionMapping.Create(this._timeMapping, frequencyMapping, this._mappingSet.DataSet.Connection.DBType);
            }

            if (!measureDimensionMapped)
            {
                foreach (ComponentEntity component in this._mappingSet.Dataflow.Dsd.Dimensions)
                {
                    if (component.MeasureDimension)
                    {
                        this.MeasureComponent = component;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether the <paramref name="mapping"/> is for a TimeDimension
        /// </summary>
        /// <param name="mapping">
        /// The <see cref="MappingEntity"/> to check 
        /// </param>
        /// <returns>
        /// A value indicating whether the <paramref name="mapping"/> is for a TimeDimension 
        /// </returns>
        private bool IsTimeDimensionMapping(MappingEntity mapping)
        {
            return this._mappingSet.Dataflow.Dsd.TimeDimension != null && mapping.Components.Count == 1
                   && mapping.Components[0].SysId == this._mappingSet.Dataflow.Dsd.TimeDimension.SysId;
        }

        #endregion
    }
}
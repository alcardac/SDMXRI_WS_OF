// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRetrievalInfo.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds the current StructureRetrieval state
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Model
{
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    /// This class holds the current StructureRetrieval state
    /// </summary>
    internal class StructureRetrievalInfo
    {
        #region Constants and Fields

        /// <summary>
        ///   Get or Set the collection of allowed dataflows
        /// </summary>
        private readonly IList<IMaintainableRefObject> _allowedDataflows; //DataflowRefBean = IMaintainableMutableObject

        /// <summary>
        ///   The MADB connection settings
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        ///   This field holds the dataflowRef constrains
        /// </summary>
        private readonly List<IKeyValues> _criteria = new List<IKeyValues>();

        /// <summary>
        ///   The logger object to be used
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        ///   The list of XS measures at the constraint or all XS measures.
        /// </summary>
        private readonly Dictionary<string, ComponentEntity> _xsMeasureDimensionConstraints =
            new Dictionary<string, ComponentEntity>();

        /// <summary>
        ///   This field holds the mapping between the component name and the ComponentInfo
        /// </summary>
        private readonly Dictionary<string, ComponentInfo> _componentMapping = new Dictionary<string, ComponentInfo>();

        /// <summary>
        ///  Gets or sets the requested codelist reference.
        /// </summary>
        private IMaintainableRefObject _codelistRef;

        /// <summary>
        ///   The dataset SQL Query
        /// </summary>
        private string _innerSqlQuery;

        /// <summary>
        ///   This field holds the mappingSet of the dataflow specified in the constructor
        /// </summary>
        private MappingSetEntity _mappingSet;

        /// <summary>
        ///   The Structure access object used to get structural metadata from Mapping Store
        /// </summary>
        private SpecialMutableObjectRetrievalManager _mastoreAccess;

        /// <summary>
        ///   This field holds the measure component name in case it is not mapped. Otherwise it is null.
        /// </summary>
        private string _measureComponent;

        /// <summary>
        ///   This field holds the ReferencePeriod
        /// </summary>
        private IReferencePeriodMutableObject _referencePeriod; //ReferencePeriodBean = IReferencePeriodMutableObject

        /// <summary>
        ///   The field contains the requested component
        /// </summary>
        private string _requestedComponent;

        /// <summary>
        ///   The <see cref="_sqlQuery" /> to execute on DDB
        /// </summary>
        private string _sqlQuery;

        /// <summary>
        ///   This field holds the name of the TimeDimension
        /// </summary>
        private string _timeDimension;

        /// <summary>
        ///   This field holds the mapping used by the time dimension
        /// </summary>
        private MappingEntity _timeMapping;

        /// <summary>
        ///   This field holds the transcoder used by the time dimension
        /// </summary>
        private ITimeDimension _timeTranscoder;

        /// <summary>
        /// The requested component <see cref="ComponentInfo"/>
        /// </summary>
        private ComponentInfo _requestedComponentInfo;

        /// <summary>
        /// Gets or sets the frequency mapping
        /// </summary>
        private ComponentInfo _frequencyInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrievalInfo"/> class. 
        /// Initializes a new instance of the <see cref="StructureRetrievalInfo"/> class. Initializes a new instance of the <see cref="StructureRetrievalInfo"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger. 
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection String Settings. 
        /// </param>
        public StructureRetrievalInfo(
            ILog logger, IList<IMaintainableRefObject> allowedDataflows, ConnectionStringSettings connectionStringSettings)
        {
            this._logger = logger;
            this._allowedDataflows = allowedDataflows;
            this._connectionStringSettings = connectionStringSettings;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the collection of allowed dataflows
        /// </summary>
        public IList<IMaintainableRefObject> AllowedDataflows
        {
            get
            {
                return this._allowedDataflows;
            }
        }

        /// <summary>
        ///   Gets the mapping between the component name and the ComponentInfo
        /// </summary>
        public IDictionary<string, ComponentInfo> ComponentMapping
        {
            get
            {
                return this._componentMapping;
            }
        }

        /// <summary>
        ///   Gets the MADB connection settings
        /// </summary>
        public ConnectionStringSettings ConnectionStringSettings
        {
            get
            {
                return this._connectionStringSettings;
            }
        }

        /// <summary>
        ///  Gets or sets the requested codelist reference.
        /// </summary>
        public IMaintainableRefObject CodelistRef
        {
            get
            {
                return this._codelistRef;
            }

            set
            {
                this._codelistRef = value;
            }
        }

        /// <summary>
        ///   Gets the dataflowRef constrains
        /// </summary>
        public List<IKeyValues> Criteria
        {
            get
            {
                return this._criteria;
            }
        }

        /// <summary>
        ///   Gets or sets the dataset SQL Query
        /// </summary>
        public string InnerSqlQuery
        {
            get
            {
                return this._innerSqlQuery;
            }

            set
            {
                this._innerSqlQuery = value;
            }
        }

        /// <summary>
        ///   Gets the logger object to be used
        /// </summary>
        public ILog Logger
        {
            get
            {
                return this._logger;
            }
        }

        /// <summary>
        ///   Gets or sets the mappingSet of the dataflow specified in the constructor
        /// </summary>
        public MappingSetEntity MappingSet
        {
            get
            {
                return this._mappingSet;
            }

            set
            {
                this._mappingSet = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Structure access object used to get structural metadata from Mapping Store
        /// </summary>
        public SpecialMutableObjectRetrievalManager MastoreAccess
        {
            get
            {
                return this._mastoreAccess;
            }

            set
            {
                this._mastoreAccess = value;
            }
        }

        /// <summary>
        ///   Gets or sets the measure component name in case it is not mapped. Otherwise it is null.
        /// </summary>
        public string MeasureComponent
        {
            get
            {
                return this._measureComponent;
            }

            set
            {
                this._measureComponent = value;
            }
        }

        /// <summary>
        ///   Gets or sets the ReferencePeriod
        /// </summary>
        public IReferencePeriodMutableObject ReferencePeriod
        {
            get
            {
                return this._referencePeriod;
            }

            set
            {
                this._referencePeriod = value;
            }
        }

        /// <summary>
        ///   Gets or sets the requested component
        /// </summary>
        public string RequestedComponent
        {
            get
            {
                return this._requestedComponent;
            }

            set
            {
                this._requestedComponent = value;
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref="_sqlQuery" /> to execute on DDB
        /// </summary>
        public string SqlQuery
        {
            get
            {
                return this._sqlQuery;
            }

            set
            {
                this._sqlQuery = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the TimeDimension
        /// </summary>
        public string TimeDimension
        {
            get
            {
                return this._timeDimension;
            }

            set
            {
                this._timeDimension = value;
            }
        }

        /// <summary>
        ///   Gets or sets the mapping used by the time dimension
        /// </summary>
        public MappingEntity TimeMapping
        {
            get
            {
                return this._timeMapping;
            }

            set
            {
                this._timeMapping = value;
            }
        }

        /// <summary>
        ///   Gets or sets the transcoder used by the time dimension
        /// </summary>
        public ITimeDimension TimeTranscoder
        {
            get
            {
                return this._timeTranscoder;
            }

            set
            {
                this._timeTranscoder = value;
            }
        }

        /// <summary>
        ///   Gets the list of XS measures at the constraint or all XS measures.
        /// </summary>
        public IDictionary<string, ComponentEntity> XSMeasureDimensionConstraints
        {
            get
            {
                return this._xsMeasureDimensionConstraints;
            }
        }

        /// <summary>
        /// Gets or sets the Requested Component <see cref="ComponentInfo"/>.
        /// </summary>
        public ComponentInfo RequestedComponentInfo
        {
            get
            {
                return this._requestedComponentInfo;
            }

            set
            {
                this._requestedComponentInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets the frequency mapping
        /// </summary>
        public ComponentInfo FrequencyInfo
        {
            get
            {
                return this._frequencyInfo;
            }

            set
            {
                this._frequencyInfo = value;
            }
        }

        #endregion
    }
}
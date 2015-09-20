// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrievalInfoSeries.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The current data retrieval state for Time Series output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DsplDataFormat.Retriever.Model
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using DsplDataFormat.Engine;

    /// <summary>
    /// The current data retrieval state for Time Series output
    /// </summary>
    internal class DataRetrievalInfoSeries : DataRetrievalInfo
    {
        #region Constants and Fields

        /// <summary>
        ///   The dataSet attributes
        /// </summary>
        private readonly IList<IComponentMapping> _dataSetAttributes;

        /// <summary>
        ///   The <see cref="NameTable" /> used to store atomized strings from group key values
        /// </summary>
        private readonly NameTable _groupNameTable = new NameTable();

        /// <summary>
        ///   A dictionary between a <see cref="GroupEntity" /> and <see cref="GroupInformation" />
        /// </summary>
        private readonly IDictionary<GroupEntity, GroupInformation> _groups;

        /// <summary>
        ///   Hold all series and observation components
        /// </summary>
        private readonly List<IComponentMapping> _seriesObsComponents;

        /// <summary>
        ///   Writer provided that is based on the series model to write the retrieved data.
        /// </summary>
        private readonly IDsplDataWriterEngine _seriesWriter;

        /// <summary>
        ///   A value indicating whether values for attributes attached to dataset level should be retrieved from DDB using a separate SQL query.
        /// </summary>
        private readonly bool _useDataSetSqlQuery;

        /// <summary>
        /// The component mappings for series. It is filled by <see cref="_seriesObsComponents"/>
        /// </summary>
        private IComponentMapping[] _componentMappings;

        /// <summary>
        ///   Holds the SQL string for getting the dataset attributes
        /// </summary>
        private string _dataSetSqlString;

        /// <summary>
        ///   Holds the cached where clauses which is useful for TimeSeries with Groups and/or DataSet attributes
        /// </summary>
        private string _sqlWhereCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfoSeries" /> class.
        /// </summary>
        /// <param name="mappingSet">The mapping set of the dataflow found in the sdmx query</param>
        /// <param name="query">The current SDMX Query object</param>
        /// <param name="connectionStringSettings">The Mapping Store connection string settings</param>
        /// <param name="seriesWriter">The series Writer.</param>
        /// <param name="sdmxSchemaVersion">The SDMX schema version.</param>
        public DataRetrievalInfoSeries(MappingSetEntity mappingSet, IDataQuery query, ConnectionStringSettings connectionStringSettings, IDsplDataWriterEngine seriesWriter, SdmxSchemaEnumType sdmxSchemaVersion)
            : base(mappingSet, query, connectionStringSettings, sdmxSchemaVersion)
        {
            this._seriesWriter = seriesWriter;
            this._seriesObsComponents = new List<IComponentMapping>();
            this._dataSetAttributes = new List<IComponentMapping>();
            this._groups = new Dictionary<GroupEntity, GroupInformation>();
            this._useDataSetSqlQuery = mappingSet.Dataflow.Dsd.Groups.Count > 0;
            this.BuildSeriesMappings();

            // add dimension mappings to groups
            this.BuildTimeSeriesGroupMappings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfoSeries"/> class.
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
        /// <param name="seriesWriter">
        /// The series Writer. 
        /// </param>
        public DataRetrievalInfoSeries(
            MappingSetEntity mappingSet,
            IComplexDataQuery query,
            ConnectionStringSettings connectionStringSettings,
            IDsplDataWriterEngine seriesWriter)
            : base(mappingSet, query, connectionStringSettings)
        {
            this._seriesWriter = seriesWriter;
            this._seriesObsComponents = new List<IComponentMapping>();
            this._dataSetAttributes = new List<IComponentMapping>();
            this._groups = new Dictionary<GroupEntity, GroupInformation>();
            this._useDataSetSqlQuery = mappingSet.Dataflow.Dsd.Groups.Count > 0;
            this.BuildSeriesMappings();

            // add dimension mappings to groups
            this.BuildTimeSeriesGroupMappings();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets series and observation components
        /// </summary>
        public override IList<IComponentMapping> AllComponentMappings
        {
            get
            {
                return this._componentMappings ?? (this._componentMappings = this._seriesObsComponents.ToArray());
            }
        }

        /// <summary>
        ///   Gets the DataSet level Attribute mappings
        /// </summary>
        public IList<IComponentMapping> DataSetAttributes
        {
            get
            {
                return this._dataSetAttributes;
            }
        }

        /// <summary>
        ///   Gets or sets the SQL string for getting the dataset attributes
        /// </summary>
        public string DataSetSqlString
        {
            get
            {
                return this._dataSetSqlString;
            }

            set
            {
                this._dataSetSqlString = value;
            }
        }

        /// <summary>
        ///   Gets the <see cref="NameTable" /> used to store atomized strings from group key values
        /// </summary>
        public NameTable GroupNameTable
        {
            get
            {
                return this._groupNameTable;
            }
        }

        /// <summary>
        ///   Gets a dictionary between a <see cref="GroupEntity" /> and <see cref="GroupInformation" />
        /// </summary>
        public IEnumerable<KeyValuePair<GroupEntity, GroupInformation>> Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        ///   Gets writer provided that is based on the series model to write the retrieved data. If null the XS writer should be set instead.
        /// </summary>
        public IDsplDataWriterEngine SeriesWriter
        {
            get
            {
                return this._seriesWriter;
            }
        }

        /// <summary>
        ///   Gets or sets the cached where clauses which is useful for TimeSeries with Groups and/or DataSet attributes
        /// </summary>
        public string SqlWhereCache
        {
            get
            {
                return this._sqlWhereCache;
            }

            set
            {
                this._sqlWhereCache = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add <paramref name="componentMapping"/> to <see cref="_dataSetAttributes"/>
        /// </summary>
        /// <param name="componentMapping">
        /// The dataset level attribute <see cref="IComponentMapping"/> 
        /// </param>
        private void AddDataSetAttributes(IComponentMapping componentMapping)
        {
            if (this._useDataSetSqlQuery)
            {
                this._dataSetAttributes.Add(componentMapping);
            }
            else
            {
                this._seriesObsComponents.Add(componentMapping);
            }
        }

        /// <summary>
        /// Adds series OBS mapping.
        /// </summary>
        /// <param name="componentMapping">
        /// The component mapping. 
        /// </param>
        private void AddSeriesObsMapping(IComponentMapping componentMapping)
        {
            this._seriesObsComponents.Add(componentMapping);
        }

        /// <summary>
        /// Add <paramref name="componentMapping"/> to <see cref="_groups"/>
        /// </summary>
        /// <param name="componentMapping">
        /// The group level attribute <see cref="IComponentMapping"/> 
        /// </param>
        private void AddTimeSeriesGroups(IComponentMapping componentMapping)
        {
            foreach (var groupEntity in componentMapping.Component.AttAssignmentGroups)
            {
                if (!this._groups.ContainsKey(groupEntity))
                {
                    var information = new GroupInformation(groupEntity);
                    this._groups.Add(groupEntity, information);
                    information.ComponentMappings.Add(componentMapping);
                }
            }
        }

        /// <summary>
        /// Build Time series mappings
        /// </summary>
        private void BuildSeriesMappings()
        {
            foreach (var componentMap in this.ComponentIdMap.Values)
            {
                IComponentMapping componentMapping = componentMap;
                ComponentEntity component = componentMapping.Component;

                switch (component.ComponentType)
                {
                    case SdmxComponentType.Dimension:
                        this.AddSeriesObsMapping(componentMapping);
                        break;
                    case SdmxComponentType.Attribute:
                        switch (component.AttributeAttachmentLevel)
                        {
                            case AttachmentLevel.Group:
                                this.AddTimeSeriesGroups(componentMapping);

                                break;
                            case AttachmentLevel.DataSet:
                                this.AddDataSetAttributes(componentMapping);
                                break;
                            case AttachmentLevel.Series:
                            case AttachmentLevel.Observation:
                                this.AddSeriesObsMapping(componentMapping);
                                break;
                        }

                        break;
                    case SdmxComponentType.PrimaryMeasure:
                        this.AddSeriesObsMapping(componentMapping);
                        break;
                    case SdmxComponentType.CrossSectionalMeasure:
                        this.AddSeriesObsMapping(componentMapping);
                        break;
                }
            }
        }

        /// <summary>
        /// Build Time series group mappings
        /// </summary>
        private void BuildTimeSeriesGroupMappings()
        {
            foreach (var valuePair in this._groups)
            {
                foreach (var dim in valuePair.Key.Dimensions)
                {
                    IComponentMapping componentMapping;
                    if (this.ComponentIdMap.TryGetValue(dim.Id, out componentMapping))
                    {
                        valuePair.Value.ComponentMappings.Add(componentMapping);
                    }
                    else if (dim.Equals(this.MeasureComponent))
                    {
                        valuePair.Value.MeasureComponent = dim;
                    }
                }
            }
        }

        #endregion
    }
}
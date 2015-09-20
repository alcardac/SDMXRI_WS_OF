// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Org.Sdmxsource.Sdmx.Api.Model.Query;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;

    /// <summary>
    ///   The data query impl.
    /// </summary>
    [Serializable]
    public class DataQueryImpl : BaseDataQuery, IDataQuery
    {
        #region Fields

        /// <summary>
        ///   The _data providers.
        /// </summary>
        private readonly ISet<IDataProvider> _dataProviders = new HashSet<IDataProvider>();

        /// <summary>
        ///   The _data query selection groups.
        /// </summary>
        private readonly IList<IDataQuerySelectionGroup> _dataQuerySelectionGroups = new List<IDataQuerySelectionGroup>();

        /// <summary>
        ///   The _last updated.
        /// </summary>
        private readonly ISdmxDate _lastUpdated;

        /// <summary>
        ///   The _order asc.
        /// </summary>
        private readonly bool _orderAsc;

        /// <summary>
        ///   The _data query detail.
        /// </summary>
        private readonly DataQueryDetail _dataQueryDetail;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Build from a REST query and a bean retrival manager
        /// </summary>
        /// <param name="dataQuery"></param>
        /// <param name="retrievalManager"></param>
        /// <exception cref="SdmxSemmanticException"></exception>
        public DataQueryImpl(IRestDataQuery dataQuery, ISdmxObjectRetrievalManager retrievalManager)
        {
            this._lastUpdated = dataQuery.UpdatedAfter;
            this._dataQueryDetail = dataQuery.QueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

            base.FirstNObservations = dataQuery.FirstNObservations;
            base.LastNObservations = dataQuery.LastNObsertations;
            if (ObjectUtil.ValidString(dataQuery.DimensionAtObservation))
            {
                this.DimensionAtObservation = dataQuery.DimensionAtObservation;
            }

            base.Dataflow = retrievalManager.GetMaintainableObject<IDataflowObject>(dataQuery.FlowRef.MaintainableReference);
            if (base.Dataflow == null)
            {
                throw new SdmxNoResultsException("Data Flow could not be found for query : " + dataQuery.FlowRef);
            }

            base.DataStructure =
                retrievalManager.GetMaintainableObject<IDataStructureObject>(base.Dataflow.DataStructureRef.MaintainableReference);
            if (base.DataStructure == null)
            {
                throw new SdmxNoResultsException("DSD could not be found for query : " + base.Dataflow.DataStructureRef);
            }

            ISet<IDataProvider> dataProviders = new HashSet<IDataProvider>();
            if (dataQuery.ProviderRef != null)
            {
                ISet<IDataProviderScheme> dataProviderSchemes =
                    retrievalManager.GetMaintainableObjects<IDataProviderScheme>(dataQuery.ProviderRef.MaintainableReference);
                foreach (IDataProviderScheme currentDpScheme in dataProviderSchemes)
                {
                    foreach (IDataProvider dataProvider in currentDpScheme.Items)
                    {
                        if (dataProvider.Id.Equals(dataQuery.ProviderRef.ChildReference.Id))
                        {
                            dataProviders.Add(dataProvider);
                        }
                    }
                }
            }
            ISet<IDataQuerySelection> selections = new HashSet<IDataQuerySelection>();
            if (dataQuery.QueryList.Count > 0)
            {
                int i = 0;
                foreach (IDimension dimension in base.DataStructure.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension).OrderBy(dimension => dimension.Position))
                {
                    if (dataQuery.QueryList.Count <= i)
                    {
                        throw new SdmxSemmanticException(
                            "Not enough key values in query, expecting "
                            + base.DataStructure.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension).Count + " got "
                            + dataQuery.QueryList.Count);
                    }
                    ISet<string> queriesForDimension = dataQuery.QueryList[i];
                    if (queriesForDimension != null && queriesForDimension.Count > 0)
                    {
                        IDataQuerySelection selectionsForDimension =
                            new DataQueryDimensionSelectionImpl(
                                dimension.Id,
                                new HashSet<string>(queriesForDimension));
                        selections.Add(selectionsForDimension);
                    }
                    i++;
                }
            }

            if (ObjectUtil.ValidCollection(selections) || dataQuery.StartPeriod != null || dataQuery.EndPeriod != null)
            {
                _dataQuerySelectionGroups.Add(
                    new DataQuerySelectionGroupImpl(selections, dataQuery.StartPeriod, dataQuery.EndPeriod));
            }
            ValidateQuery();
        }

        private DataQueryImpl(
            IDataStructureObject dataStructure, IDataflowObject dataflow, DataQueryDetail dataQueryDetail)
        {
            base.DataStructure = dataStructure;
            this._dataQueryDetail = dataQueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
            base.Dataflow = dataflow;
            ValidateQuery();
        }

        public static IDataQuery BuildEmptyQuery(
            IDataStructureObject dataStructure, IDataflowObject dataflow, DataQueryDetail dataQueryDetail)
        {
            return new DataQueryImpl(dataStructure, dataflow, dataQueryDetail);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryImpl"/> class.
        /// </summary>
        /// <param name="dataStructure">
        /// The data structure.
        /// </param>
        /// <param name="lastUpdated">
        /// The last updated.
        /// </param>
        /// <param name="dataQueryDetail">
        /// The data query detail.
        /// </param>
        /// <param name="firstNObs">
        /// The first n obs.
        /// </param>
        /// <param name="lastNObs">
        /// The last n obs.
        /// </param>
        /// <param name="dataProviders">
        /// The data providers.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="dimensionAtObservation">
        /// The dimension at observation.
        /// </param>
        /// <param name="selections">
        /// The selections.
        /// </param>
        /// <param name="dateFrom">
        /// The date from.
        /// </param>
        /// <param name="dateTo">
        /// The date to.
        /// </param>
        public DataQueryImpl(
            IDataStructureObject dataStructure,
            ISdmxDate lastUpdated,
            DataQueryDetail dataQueryDetail,
            int? firstNObs,
            int? lastNObs,
            ISet<IDataProvider> dataProviders,
            IDataflowObject dataflow,
            string dimensionAtObservation,
            ISet<IDataQuerySelection> selections,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            base.DataStructure = dataStructure;
            if (dataProviders != null)
            {
                this._dataProviders = new HashSet<IDataProvider>(dataProviders);
            }
            if (ObjectUtil.ValidCollection(selections) || dateFrom != null || dateTo != null)
            {
                ISdmxDate sdmxDateFrom = null;
                if (dateFrom != null)
                {
                    sdmxDateFrom = new SdmxDateCore(dateFrom, TimeFormatEnumType.Date);
                }
                ISdmxDate sdmxDateTo = null;
                if (dateTo != null)
                {
                    sdmxDateTo = new SdmxDateCore(dateTo, TimeFormatEnumType.Date);
                }
                this._dataQuerySelectionGroups.Add(
                    new DataQuerySelectionGroupImpl(selections, sdmxDateFrom, sdmxDateTo));
            }
            base.Dataflow = dataflow;
            this._lastUpdated = lastUpdated;
            this._dataQueryDetail = dataQueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
            this.DimensionAtObservation = dimensionAtObservation;
            base.FirstNObservations = firstNObs;
            base.LastNObservations = lastNObs;
            ValidateQuery();
        }

        public DataQueryImpl(
            IDataStructureObject dataStructure,
            ISdmxDate lastUpdated,
            DataQueryDetail dataQueryDetail,
            int maxObs,
            bool orderAsc,
            ISet<IDataProvider> dataProviders,
            IDataflowObject dataflow,
            string dimensionAtObservation,
            ISet<IDataQuerySelection> selections,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            base.DataStructure = dataStructure;
            this._lastUpdated = lastUpdated;
            this._dataQueryDetail = dataQueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
            if (orderAsc)
            {
                base.FirstNObservations = maxObs;
            }
            else
            {
                base.LastNObservations = maxObs;
            }
            if (dataProviders != null)
            {
                this._dataProviders = new HashSet<IDataProvider>(dataProviders);
            }
            base.Dataflow = dataflow;
            this.DimensionAtObservation = dimensionAtObservation;

            if (ObjectUtil.ValidCollection(selections) || dateFrom != null || dateTo != null)
            {
                ISdmxDate sdmxDateFrom = null;
                if (dateFrom != null)
                {
                    sdmxDateFrom = new SdmxDateCore(dateFrom, TimeFormatEnumType.Date);
                }
                ISdmxDate sdmxDateTo = null;
                if (dateTo != null)
                {
                    sdmxDateTo = new SdmxDateCore(dateTo, TimeFormatEnumType.Date);
                }
                //TODO: move to fluent interface
                this._dataQuerySelectionGroups.Add(new DataQuerySelectionGroupImpl(selections, sdmxDateFrom, sdmxDateTo));
            }
            ValidateQuery();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryImpl" /> class.
        /// </summary>
        /// <param name="dataStructure">The data structure.</param>
        /// <param name="lastUpdated">The last updated.</param>
        /// <param name="dataQueryDetail">The data query detail.</param>
        /// <param name="firstNObs">The first asynchronous obs.</param>
        /// <param name="lastNObs">The last asynchronous obs.</param>
        /// <param name="dataProviders">The data providers.</param>
        /// <param name="dataflow">The dataflow.</param>
        /// <param name="dimensionAtObservation">The dimension at observation.</param>
        /// <param name="selectionGroup">The selection group.</param>
        public DataQueryImpl(
            IDataStructureObject dataStructure,
            ISdmxDate lastUpdated,
            DataQueryDetail dataQueryDetail,
            int? firstNObs,
            int? lastNObs,
            IEnumerable<IDataProvider> dataProviders,
            IDataflowObject dataflow,
            string dimensionAtObservation,
            IEnumerable<IDataQuerySelectionGroup> selectionGroup)
        {
            base.DataStructure = dataStructure;
            this._lastUpdated = lastUpdated;
            this._dataQueryDetail = dataQueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
            base.FirstNObservations = firstNObs;
            base.LastNObservations = lastNObs;
            if (dataProviders != null)
            {
                this._dataProviders = new HashSet<IDataProvider>(dataProviders);
            }
            base.Dataflow = dataflow;
            this.DimensionAtObservation = dimensionAtObservation;

            if (selectionGroup != null)
            {
                foreach (IDataQuerySelectionGroup dqsg in selectionGroup)
                {
                    if (dqsg != null)
                    {
                        this._dataQuerySelectionGroups.Add(dqsg);
                    }
                }
            }
            ValidateQuery();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryImpl"/> class.
        /// </summary>
        /// <param name="dataStructure">
        /// The data structure.
        /// </param>
        /// <param name="lastUpdated">
        /// The last updated.
        /// </param>
        /// <param name="dataQueryDetail">
        /// The data query detail.
        /// </param>
        /// <param name="maxObs">
        /// The max obs.
        /// </param>
        /// <param name="orderAsc">
        /// If the order is ascending.
        /// </param>
        /// <param name="dataProviders">
        /// The data providers.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="dimensionAtObservation">
        /// The dimension at observation.
        /// </param>
        /// <param name="selectionGroup">
        /// The selection group.
        /// </param>
        public DataQueryImpl(
            IDataStructureObject dataStructure,
            ISdmxDate lastUpdated,
            DataQueryDetail dataQueryDetail,
            int maxObs,
            bool orderAsc,
            ISet<IDataProvider> dataProviders,
            IDataflowObject dataflow,
            string dimensionAtObservation,
            ICollection<IDataQuerySelectionGroup> selectionGroup)
        {
            base.DataStructure = dataStructure;
            this._lastUpdated = lastUpdated;
            this._dataQueryDetail = dataQueryDetail ?? DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
            if (orderAsc)
            {
                base.FirstNObservations = maxObs;
            }
            else
            {
                base.LastNObservations = maxObs;
            }
            if (dataProviders != null)
            {
                this._dataProviders = new HashSet<IDataProvider>(dataProviders);
            }
            base.Dataflow = dataflow;
            this.DimensionAtObservation = dimensionAtObservation;

            if (selectionGroup != null)
            {
                foreach (IDataQuerySelectionGroup dqsg in selectionGroup)
                {
                    if (dqsg != null)
                    {
                        this._dataQuerySelectionGroups.Add(dqsg);
                    }
                }
            }
            ValidateQuery();
        }

        #endregion


        #region Public Properties

        /// <summary>
        ///   Gets the data provider.
        /// </summary>
        public virtual ISet<IDataProvider> DataProvider
        {
            get
            {
                return new HashSet<IDataProvider>(this._dataProviders);
            }
        }

        /// <summary>
        ///   Gets the data query detail.
        /// </summary>
        public virtual DataQueryDetail DataQueryDetail
        {
            get
            {
                return this._dataQueryDetail;
            }
        }

        /// <summary>
        ///   Gets the last updated date.
        /// </summary>
        public virtual ISdmxDate LastUpdatedDate
        {
            get
            {
                return this._lastUpdated;
            }
        }

        /// <summary>
        ///   Gets the selection groups.
        /// </summary>
        public virtual IList<IDataQuerySelectionGroup> SelectionGroups
        {
            get
            {
                return new List<IDataQuerySelectionGroup>(this._dataQuerySelectionGroups);
            }
        }

        #endregion

        #region Public Methods and Operators

        protected override ISet<string> GetQueryComponentIds()
        {
            ISet<string> returnSet = new HashSet<string>();
            foreach (IDataQuerySelectionGroup dqsg in SelectionGroups)
            {
                foreach (IDataQuerySelection dqs in dqsg.Selections)
                {
                    returnSet.Add(dqs.ComponentId);
                }
            }
            return returnSet;
        }

        /// <summary>
        ///   The has selections.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasSelections()
        {
            return this._dataQuerySelectionGroups.Count > 0;
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            string newLine = Environment.NewLine;
            if (base.DataStructure != null)
            {
                sb.Append(newLine).Append("Data Structure : " ).Append(base.DataStructure.Urn);
            }

            if (base.Dataflow != null)
            {
                sb.Append(newLine).Append("Dataflow : ").Append(base.Dataflow.Urn);
            }

            if(_dataProviders != null) 
            {
		       foreach(IDataProvider dataProvider in _dataProviders) 
               {
			      sb.Append(newLine).Append("Data Provider  : ").Append(dataProvider.Urn);
		       }
 		    } 

            // ADD SELECTION INFORMATION
            if (this.HasSelections())
            {
                string concat = string.Empty;

                foreach (IDataQuerySelectionGroup selectionGroup in this._dataQuerySelectionGroups)
                {
                    sb.Append(concat).Append("(").Append(selectionGroup).Append(")");
                    concat = "OR";
                }
            }

            return sb.ToString();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
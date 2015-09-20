// -----------------------------------------------------------------------
// <copyright file="DataQueryFluentBuilder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataQueryFluentBuilder : IDataQueryFluentBuilder
    {
        private IDataQuery _dataQuery;

        private ISet<IDataProvider> _dataProviders = new HashSet<IDataProvider>();

        private ISet<IDataQuerySelection> _dataQuerySelections = null;

        private ICollection<IDataQuerySelectionGroup> _dataQuerySelectionGroup = null;

        private string _dimensionAtObservation;

        private ISdmxDate _lastUpdated;

        private int? _maxObs;

        private bool _orderAsc;

        private DataQueryDetail _dataQueryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

        private IDataStructureObject _dataStructure = null;

        private IDataflowObject _dataflow = null;

        private int? _firstNObs;

        private int _lastNObs;

        private DateTime? _dateFrom; //value type
        
        private DateTime? _dateTo;

        public IDataQueryFluentBuilder Initialize(IDataStructureObject dataStructure, IDataflowObject dataflow)
        {
            this._dataStructure = dataStructure;
            this._dataflow = dataflow;
            return this;
        }

        public IDataQueryFluentBuilder WithLastUpdated(ISdmxDate lastUpdated)
        {
            this._lastUpdated = lastUpdated;
            return this;
        }

        public IDataQueryFluentBuilder WithDataQueryDetail(DataQueryDetail dataQueryDetail)
        {
            if(dataQueryDetail != null)
                this._dataQueryDetail = dataQueryDetail;
            return this;
        }

        public IDataQueryFluentBuilder WithDataProviders(ISet<IDataProvider> dataProviders)
        {
            this._dataProviders = dataProviders;
            return this;
        }

        public IDataQueryFluentBuilder WithDateFrom(DateTime dateFrom)
        {
            this._dateFrom = dateFrom;
            return this;
        }

        public IDataQueryFluentBuilder WithDateTo(DateTime dateTo)
        {
            this._dateTo = dateTo;
            return this;
        } 

        public IDataQueryFluentBuilder WithMaxObservations(int maxObs)
        {
            this._maxObs = maxObs;
            return this;
        }

        public IDataQueryFluentBuilder WithOrderAsc(bool orderAsc)
        {
            this._orderAsc = orderAsc;
            return this;
        }

        public IDataQueryFluentBuilder WithFirstNObs(int firstNObs)
        {
            this._firstNObs = firstNObs;
            return this;
        }

        public IDataQueryFluentBuilder WithLastNObs(int lastNObs)
        {
            this._lastNObs = lastNObs;
            return this;
        }

        public IDataQueryFluentBuilder WithDimensionAtObservation(string dimensionAtObservation)
        {
            this._dimensionAtObservation = dimensionAtObservation;
            return this;
        }

        public IDataQueryFluentBuilder WithDataQuerySelections(ISet<IDataQuerySelection> dataQuerySelections)
        {
            this._dataQuerySelections = dataQuerySelections;
            return this;
        }

        public IDataQueryFluentBuilder WithDataQuerySelectionGroup(ICollection<IDataQuerySelectionGroup> dataQuerySelectionGroups)
        {
            this._dataQuerySelectionGroup = dataQuerySelectionGroups;
            return this;
        }

        public IDataQuery Build()
        {
            if (this._dataQuerySelectionGroup != null)
            {
                return new DataQueryImpl(
                   this._dataStructure,
                   this._lastUpdated,
                   this._dataQueryDetail,
                   this._maxObs?? default(int),
                   this._orderAsc,
                   this._dataProviders,
                   this._dataflow,
                   this._dimensionAtObservation,
                   this._dataQuerySelectionGroup);
            }

            if (this._firstNObs.HasValue)
            {
                return new DataQueryImpl(
                    this._dataStructure,
                    this._lastUpdated,
                    this._dataQueryDetail,
                    this._firstNObs.Value,
                    this._lastNObs,
                    this._dataProviders,
                    this._dataflow,
                    this._dimensionAtObservation,
                    this._dataQuerySelections, 
                    this._dateFrom, 
                    this._dateTo);
            }
            
            if (this._maxObs.HasValue)
            {
                return new DataQueryImpl(
                   this._dataStructure,
                   this._lastUpdated,
                   this._dataQueryDetail,
                   this._maxObs.Value,
                   this._orderAsc,
                   this._dataProviders,
                   this._dataflow,
                   this._dimensionAtObservation,
                   this._dataQuerySelections,
                   this._dateFrom,
                   this._dateTo);
            }

            return 
                DataQueryImpl.BuildEmptyQuery(this._dataStructure, this._dataflow, this._dataQueryDetail);

        }
    }
}

using System;
using System.Collections.Generic;
using Org.Sdmxsource.Sdmx.Api.Exception;
using Org.Sdmxsource.Util;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
using Org.Sdmxsource.Sdmx.Api.Model.Base;
using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

using ITimeRange = Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex.ITimeRange;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex
{
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    public class ComplexDataQueryImpl : BaseDataQuery, IComplexDataQuery
    {
        #region Fields

        //private static sealed long serialVersionUID = 2107508015963353910L;

	    private int? _defaultLimit;

	    private ObservationAction _obsAction;

	    private bool _hasExplicitMeasures = false;

	    private DataQueryDetail _queryDetail;

	    private string _datasetId;

	    private TextSearch _datasetIdOperator;
	
	    private ISet<IDataProvider> _dataProviders = new HashSet<IDataProvider>();

	    private IProvisionAgreementObject _provisionAgreement;

	    private IList<ITimeRange> _lastUpdatedDate = new List<ITimeRange>();

	    private IList<IComplexDataQuerySelectionGroup> _complexDataQuerySelectionGroups = new List<IComplexDataQuerySelectionGroup>();

        private static readonly string _allDimensions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ComplexDataQueryImpl"/> class.
        /// </summary>
        static ComplexDataQueryImpl()
        {
            _allDimensions = Api.Constants.DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;
        }

	    public ComplexDataQueryImpl(string datasetId, 
								TextSearch datasetIdOperator, 
								ISet<IDataProvider> dataProviders, 
								IDataStructureObject dataStructure, 
								IDataflowObject dataFlow,
								IProvisionAgreementObject provisionAgreement,  
								IList<ITimeRange> lastUpdatedDate, 
								int? maxObs, 
								int? defaultLimit, 
								bool orderAsc, 
								ObservationAction obsAction, 
								string dimensionAtObservation,
								bool hasExplicitMeasures, 
								DataQueryDetail queryDetail, 
								ICollection<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroup)
        {

		    this._datasetId = datasetId;

		    if (datasetIdOperator != null)
			    this._datasetIdOperator = datasetIdOperator;
		    else
			    this._datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		
            if (dataProviders != null) 
            {
			    this._dataProviders = new HashSet<IDataProvider>(dataProviders);
		    }

		    base.DataStructure = dataStructure;
		    base.Dataflow = dataFlow;
		    this._provisionAgreement = provisionAgreement;

		    if(lastUpdatedDate != null) 
            {
			    this._lastUpdatedDate = new List<ITimeRange>(lastUpdatedDate);
		    }

		    if (orderAsc) 
			    base.FirstNObservations = maxObs;
		    else 
			    base.LastNObservations = maxObs;

		    this._defaultLimit = defaultLimit;

		    if (obsAction!=null)
			    this._obsAction = obsAction;
		    else 
			    this._obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);

		    this.DimensionAtObservation = dimensionAtObservation;

		    if (dimensionAtObservation != null)
            {
			    //the values: 'AllDimensions' and 'TIME_PERIOD' are valid values.
			    if (dimensionAtObservation.Equals(_allDimensions) || dimensionAtObservation.Equals(DimensionAtObservationEnumType.Time.ToString()))
				    this.DimensionAtObservation = dimensionAtObservation;
			    else //check if the value is a dimension Value
				    CheckDimensionExistence(dimensionAtObservation, dataStructure);
		    }
            else
            {
			    this.DimensionAtObservation = GetDimensionAtObservationLevel(dataStructure);
		    }

		    this._hasExplicitMeasures = hasExplicitMeasures;

		    if (queryDetail != null)
			    this._queryDetail = queryDetail;
		    else
			    this._queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

		    if (complexDataQuerySelectionGroup != null) 
            {
			    foreach (IComplexDataQuerySelectionGroup cdqsg in complexDataQuerySelectionGroup) 
                {
				    if (cdqsg != null) 
                    {
					    this._complexDataQuerySelectionGroups.Add(cdqsg);
				    }
			    }
		    }

		    //perform validation 	
		    ValidateQuery();
		    ValidateProvisionAgreement();
	    }

	    public ComplexDataQueryImpl(string datasetId, TextSearch datasetIdOperator, 
                                ISet<IDataProvider> dataProviders, 
			                    IDataStructureObject dataStructure, 
                                IDataflowObject dataFlow,
			                    IProvisionAgreementObject provisionAgreement, 
                                IList<ITimeRange> lastUpdatedDate,  
			                    int? firstNObs, int? lastNObs, int? defaultLimit, 
                                ObservationAction obsAction, string dimensionAtObservation,
			                    bool hasExplicitMeasures, DataQueryDetail queryDetail, 
                                ICollection<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroup)
        {

		    this._datasetId = datasetId;
		    if (datasetIdOperator!=null) 
            {
			    this._datasetIdOperator = datasetIdOperator;
		    }
		    else 
            {
			    this._datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		    }

		    if (dataProviders != null) 
            {
			    this._dataProviders = new HashSet<IDataProvider>(dataProviders);
		    }

		    if (lastUpdatedDate != null) 
            {
			    this._lastUpdatedDate = new List<ITimeRange>(lastUpdatedDate);
		    }

		    this.DimensionAtObservation = dimensionAtObservation;
		    base.DataStructure = dataStructure;
		    base.Dataflow = dataFlow;
		    this._provisionAgreement = provisionAgreement;
		    base.FirstNObservations = firstNObs;
		    base.LastNObservations = lastNObs;
		    this._defaultLimit = defaultLimit;

		    if (obsAction != null)
            {
			    this._obsAction = obsAction;
		    }
		    else 
            {
			    this._obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);
		    }
		
		    if (dimensionAtObservation != null)
            {
			    //the values: 'AllDimensions' and 'TIME_PERIOD' are valid values.
			    if ( dimensionAtObservation.Equals(_allDimensions) || dimensionAtObservation.Equals(DimensionAtObservationEnumType.Time.ToString()))
				    this.DimensionAtObservation = dimensionAtObservation;
			    else//check if the value is a dimension Value
				    CheckDimensionExistence(dimensionAtObservation, dataStructure);
		    }
            else
            {
			    this.DimensionAtObservation = GetDimensionAtObservationLevel(dataStructure);
		    }
		
		    this._hasExplicitMeasures = hasExplicitMeasures;

		    if (queryDetail != null)
			    this._queryDetail = queryDetail;
		    else
			    this._queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

		    if(complexDataQuerySelectionGroup != null)
            {
			    foreach (IComplexDataQuerySelectionGroup cdqsg in complexDataQuerySelectionGroup) 
                {
				    if(cdqsg != null)
                    {
					    this._complexDataQuerySelectionGroups.Add(cdqsg);
				    }
			    }
		    }
		    //perform validation 	
		    ValidateQuery();
		    ValidateProvisionAgreement();
	    }

	    public ComplexDataQueryImpl(string datasetId, TextSearch datasetIdOperator, 
            ISet<IDataProvider> dataProviders, 
			IDataStructureObject dataStructure, 
            IDataflowObject dataFlow,
			IProvisionAgreementObject provisionAgreement, 
            IList<ITimeRange> lastUpdatedDate,   
			int? maxObs, bool orderAsc, int? defaultLimit, 
            ObservationAction obsAction, string dimensionAtObservation,
			bool hasExplicitMeasures, DataQueryDetail queryDetail, 
            IList<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroup)
        {
		    this._datasetId = datasetId;
		    if (datasetIdOperator!=null)
            {
			    this._datasetIdOperator = datasetIdOperator;
		    }
		    else 
            {
			    this._datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		    }

		    if (dataProviders != null) 
            {
			    this._dataProviders = new HashSet<IDataProvider>(dataProviders);
		    }

		    if (lastUpdatedDate != null) 
            {
			    this._lastUpdatedDate = new List<ITimeRange>(lastUpdatedDate);
		    }

		    this.DimensionAtObservation = dimensionAtObservation;
		    base.DataStructure = dataStructure;
		    base.Dataflow = dataFlow;
		    this._provisionAgreement = provisionAgreement;
		    if( orderAsc) 
            {
			    base.FirstNObservations = maxObs;
		    } 
		    else 
            {
			    base.LastNObservations = maxObs;
		    }

		    this._defaultLimit = defaultLimit;

		    if (obsAction!=null) 
            {
			    this._obsAction = obsAction;
		    }
		    else 
            {
			    this._obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);
		    }

		    if (dimensionAtObservation != null) 
            {
			    //the values: 'AllDimensions' and 'TIME_PERIOD' are valid values.
			    if ( dimensionAtObservation.Equals(_allDimensions) || dimensionAtObservation.Equals(DimensionAtObservationEnumType.Time.ToString()))
				    this.DimensionAtObservation = dimensionAtObservation;
			    else //check if the value is a dimension Value
				    CheckDimensionExistence(dimensionAtObservation, dataStructure);
		    }
            else
            {
			    this.DimensionAtObservation = GetDimensionAtObservationLevel(dataStructure);
		    }
		
		    this._hasExplicitMeasures = hasExplicitMeasures;
		
            if (queryDetail != null) 
            {
			    this._queryDetail = queryDetail;
		    }
		    else 
            {
			    this._queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
		    }

		    if (complexDataQuerySelectionGroup != null) 
            {
			    foreach (IComplexDataQuerySelectionGroup cdqsg in complexDataQuerySelectionGroup) 
                {
				    if( cdqsg != null) 
                    {
					    this._complexDataQuerySelectionGroups.Add(cdqsg);
				    }
			    }
		    }
		    //perform validation 	
		    ValidateQuery();
		    ValidateProvisionAgreement();
	    }

	    public ComplexDataQueryImpl(string datasetId, TextSearch datasetIdOperator, 
                                    ISet<IDataProvider> dataProviders, 
			                        IDataStructureObject dataStructure, IDataflowObject dataFlow,
			                        IProvisionAgreementObject provisionAgreement, 
                                    IList<ITimeRange> lastUpdatedDate,  
			                        int? maxObs, bool orderAsc, int? defaultLimit, 
                                    ObservationAction obsAction, 
			                        string dimensionAtObservation, bool hasExplicitMeasures, 
                                    DataQueryDetail queryDetail,  
			                        ISet<IComplexDataQuerySelection> complexSelections, 
			                        DateTime dateFrom, OrderedOperator dateFromOperator, 
                                    DateTime dateTo,  OrderedOperator dateToOperator, 
                                    ISet<IComplexComponentValue> primaryMeasureValues)
        {
		    this._datasetId = datasetId;
		    if (datasetIdOperator != null) 
            {
			    this._datasetIdOperator = datasetIdOperator;
		    }
		    else 
            {
			    this._datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		    }
		
		    if (dataProviders != null) 
            {
			    this._dataProviders = new HashSet<IDataProvider>(dataProviders);
		    }

		    if (lastUpdatedDate != null) 
            {
			    this._lastUpdatedDate = new List<ITimeRange>(lastUpdatedDate);
		    }
		

		    this.DimensionAtObservation = dimensionAtObservation;
		    base.DataStructure = dataStructure;
		    base.Dataflow = dataFlow;
		    this._provisionAgreement = provisionAgreement;

		    if (orderAsc) 
            {
			    base.FirstNObservations = maxObs;
		    }
		    else 
            {
			    base.LastNObservations = maxObs;
		    }

		    this._defaultLimit = defaultLimit;

		    if (obsAction != null)
			    this._obsAction = obsAction;
		    else 
			    this._obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);

		    if (dimensionAtObservation != null)
            {
			    //the values: 'AllDimensions' and 'TIME_PERIOD' are valid values.
			    if ( dimensionAtObservation.Equals(_allDimensions) || dimensionAtObservation.Equals(DimensionAtObservationEnumType.Time.ToString()))
                {
				    this.DimensionAtObservation = dimensionAtObservation; 
			    }
			    else 
                {
				    //check if the value is a dimension Value
				    CheckDimensionExistence(dimensionAtObservation, dataStructure);
			    }
		    } else 
            {
			    this.DimensionAtObservation = GetDimensionAtObservationLevel(dataStructure);
		    }
		
		    this._hasExplicitMeasures = hasExplicitMeasures;
		    if (queryDetail!=null) {
			    this._queryDetail = queryDetail;
		    }
		    else 
            {
			    this._queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);
		    }

		    if(ObjectUtil.ValidCollection(complexSelections) || dateFrom != null || dateTo != null) 
            {
			    ISdmxDate sdmxDateFrom = null;
			    if(dateFrom != null) 
                {
				    sdmxDateFrom = new SdmxDateCore(dateFrom, TimeFormatEnumType.Date);
			    }

			    ISdmxDate sdmxDateTo = null;
			    if(dateFrom != null)
                {
				    sdmxDateTo = new SdmxDateCore(dateTo, TimeFormatEnumType.Date);
			    }
			    this._complexDataQuerySelectionGroups.Add(new ComplexDataQuerySelectionGroupImpl(complexSelections, sdmxDateFrom, dateFromOperator, sdmxDateTo,  dateToOperator, primaryMeasureValues));
		    }

		    //perform validation 	
		    ValidateQuery();	
		    ValidateProvisionAgreement();
	    }

	    public ComplexDataQueryImpl(string datasetId, TextSearch datasetIdOperator, 
                                    ISet<IDataProvider> dataProviders, 
			                        IDataStructureObject dataStructure, IDataflowObject dataFlow,
			                        IProvisionAgreementObject provisionAgreement, 
                                    IList<ITimeRange> lastUpdatedDate,  
			                        int? firstNObs, int? lastNObs, int? defaultLimit, 
                                    ObservationAction obsAction, string dimensionAtObservation,
			                        bool hasExplicitMeasures, DataQueryDetail queryDetail, 
                                    ISet<IComplexDataQuerySelection> complexSelections, 
			                        DateTime dateFrom, OrderedOperator dateFromOperator, 
                                    DateTime dateTo,  OrderedOperator dateToOperator,
                                    ISet<IComplexComponentValue> primaryMeasureValues)
        {
		    this._datasetId = datasetId;
		    if (datasetIdOperator != null)
			    this._datasetIdOperator = datasetIdOperator;
		    else
			    this._datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);

		    this._dataProviders = dataProviders;
		    base.DataStructure = dataStructure;
		    base.Dataflow = dataFlow;
		    this._provisionAgreement = provisionAgreement;
		    this._lastUpdatedDate = lastUpdatedDate;
		    base.FirstNObservations = firstNObs;
		    base.LastNObservations = lastNObs;
		    this._defaultLimit = defaultLimit;

		    if (obsAction != null)
			    this._obsAction = obsAction;
		    else 
			    this._obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);

		    this.DimensionAtObservation = dimensionAtObservation;
		    if (dimensionAtObservation != null)
            {
			    //the values: 'AllDimensions' and 'TIME_PERIOD' are valid values.
			    if (dimensionAtObservation.Equals(_allDimensions) || dimensionAtObservation.Equals(DimensionAtObservationEnumType.Time.ToString()))
				    this.DimensionAtObservation = dimensionAtObservation;
			    else//check if the value is a dimension Value
				    CheckDimensionExistence(dimensionAtObservation, dataStructure);
		    }
            else
            {
			    this.DimensionAtObservation = GetDimensionAtObservationLevel(dataStructure);
		    }
		
		    this._hasExplicitMeasures = hasExplicitMeasures;
		    if (queryDetail != null)
			    this._queryDetail = queryDetail;
		    else
			    this._queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

		    if(ObjectUtil.ValidCollection(complexSelections) || dateFrom != null || dateTo != null) 
            {
			    ISdmxDate sdmxDateFrom = null;
			    if(dateFrom != null) 
                {
				    sdmxDateFrom = new SdmxDateCore(dateFrom, TimeFormatEnumType.Date);
			    }
			    ISdmxDate sdmxDateTo = null;
			    if(dateFrom != null) {
				    sdmxDateTo = new SdmxDateCore(dateTo, TimeFormatEnumType.Date);
			    }
			    this._complexDataQuerySelectionGroups.Add(new ComplexDataQuerySelectionGroupImpl(complexSelections, sdmxDateFrom, dateFromOperator, sdmxDateTo,  dateToOperator, primaryMeasureValues));
		    }

		    //perform validation 	
		    ValidateQuery();
		    ValidateProvisionAgreement();
	    }

        #endregion


        #region Public Methods and Operators

        protected override ISet<string> GetQueryComponentIds()
        {
            ISet<string> returnSet = new HashSet<string>();
		    foreach (IComplexDataQuerySelectionGroup dqsg in SelectionGroups)
            {
			    foreach (IComplexDataQuerySelection dqs in dqsg.Selections) 
                {
				    returnSet.Add(dqs.ComponentId);
			    }
		    }
		    return returnSet;
        }

        /**
	     * It performs validation upon the provision agreement information. If the current dataflow is not referenced
	     * by the provision agreement then an exception occurs.
	     */
	    private void ValidateProvisionAgreement()
        {
		    if (_provisionAgreement != null)
            {
			    //get the dataflow id, version and agency 
			    ICrossReference dataflowReference = _provisionAgreement.StructureUseage;
			    if (dataflowReference == null)
				    throw new SdmxException("Can not create DataQuery, Dataflow is required",
                        SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError));
			    else
                {
				    if (!dataflowReference.MaintainableReference.MaintainableId.Equals(base.Dataflow.Id) 
                            || !dataflowReference.MaintainableReference.Version.Equals(base.Dataflow.Version)
                            || !dataflowReference.MaintainableReference.AgencyId.Equals(base.Dataflow.AgencyId))
                    {
					    throw new SdmxException("Can not create DataQuery, Dataflow provided is not referenced by the Provision Agreement",
                            SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError));				
				    }
			    }

		    }		
	    }

	    /**
	     * It checks for the existence of the provided dimensionAtObs in the data structure definition.
	     * If it is not included an error is thrown.
	     * @param dimensionAtObs
	     * @param dataStructure
	     */
	    private void CheckDimensionExistence(string dimensionAtObs, IDataStructureObject dataStructure) 
        {
		    IDimension dimension = dataStructure.GetDimension(dimensionAtObs);
	        if (dimension == null)
	        {
                // Changed from ArgumentException in order for Web Service to produce the correct error (semantic).
                // This is different than in SdmxSource Java where it throws the equivalent of ArgumentException. 
	            throw new SdmxSemmanticException("Can not create DataQuery, The dimension at observation is not included in the Dimension list of the DSD ");
	        }
        }

        /**
	     * It returns the default value for the dimension at observation in accordance with the DSD. <br>
	     * For a time series DSD, the return value Equals to the Time Dimension. For a crossX DSD it returns the <br>
	     * measure dimension value
	     * @param dataStructure
	     * @return
	     */
	    private string GetDimensionAtObservationLevel(IDataStructureObject dataStructure) 
        {
            //// Change from the SdmxSource Java. 
            //// MAT-675 : Not all DSD have either time or measure dimension. The standard says in that case 
            //// use all (flat).
		    if (dataStructure.TimeDimension != null)
                return Api.Constants.DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.Time).Value;

	        var measureDimension = dataStructure.GetDimensions(SdmxStructureEnumType.MeasureDimension).FirstOrDefault();
	        if (measureDimension != null)
	        {
	            return measureDimension.Id;
	        }

	        return Api.Constants.DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;
        }

        #endregion

        #region Public Properties

        public int?  DefaultLimit
        {
	        get { return _defaultLimit; }
        }

        public ObservationAction  ObservationAction
        {
	        get { return _obsAction; }
        }

        public bool  HasExplicitMeasures()
        {
 	        return _hasExplicitMeasures;
        }

        public string  DatasetId
        {
	        get { return _datasetId; }
        }

        public TextSearch  DatasetIdOperator
        {
	        get { return _datasetIdOperator; }
        }

        IList<ITimeRange> IComplexDataQuery.LastUpdatedDateTimeRange
        {
            get { return new List<ITimeRange>(_lastUpdatedDate); }
        }

        public IList<IComplexDataQuerySelectionGroup>  SelectionGroups
        {
	        get { return new List<IComplexDataQuerySelectionGroup>(_complexDataQuerySelectionGroups); }
        }

        public IProvisionAgreementObject  ProvisionAgreement
        {
	        get { return _provisionAgreement; }
        }

        public bool  HasSelections()
        {
 	        return _complexDataQuerySelectionGroups.Count > 0;
        }

        public ISet<IDataProvider>  DataProvider
        {
            get { return new HashSet<IDataProvider>(_dataProviders); }
        }

        public DataQueryDetail DataQueryDetail
        {
            get { return this._queryDetail; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}

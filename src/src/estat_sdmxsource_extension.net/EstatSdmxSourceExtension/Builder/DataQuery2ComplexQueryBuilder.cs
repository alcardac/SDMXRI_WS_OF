// -----------------------------------------------------------------------
// <copyright file="DataQuery2ComplexQueryBuilder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sdmxsource.Extension.Builder
{
    using System;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///Builder class of IComplexDataQuery object from IDataQuery object.
    ///Used for transforming messages in SDMX 2.0 version to SDMX 2.1 Messages. 
    /// </summary>
    public class DataQuery2ComplexQueryBuilder : IBuilder<IComplexDataQuery, IDataQuery>
    {
     
      private bool soapV20;

	  public DataQuery2ComplexQueryBuilder(bool soapV20)
      {
		this.soapV20 = soapV20;
	  }
	
	  public DataQuery2ComplexQueryBuilder() 
      {
		this.soapV20 = false;
	  }
	
	  public virtual IComplexDataQuery Build(IDataQuery dataQuery) 
      {

		  string datasetId = null;
          TextSearch datasetIdOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		  List<ITimeRange> lastUpdatedDate = null;
		  ProvisionAgreementObjectCore provisionAgreement = null;
		
		  int? defaultLimit = null;
		  int? firstNObs = null;
		  if (soapV20)
          {
			  defaultLimit = dataQuery.FirstNObservations;
		  }
          else 
          {
			  firstNObs = dataQuery.FirstNObservations;	
		  }
		  int? lastNObs = dataQuery.LastNObservations;

		  ObservationAction obsAction = ObservationAction.GetFromEnum(ObservationActionEnumType.Active);
	  	  string dimensionAtObservation = null;
		  bool hasExplicitMeasures = false;
          DataQueryDetail queryDetail = dataQuery.DataQueryDetail;
		
		  //map all DataQuerySelectionGroups on complexDataQuerySelectionGroups
		  ISet<IComplexDataQuerySelectionGroup> complexSelectionGroups = null;
		  if (dataQuery.SelectionGroups != null) 
          {
			 complexSelectionGroups = new HashSet<IComplexDataQuerySelectionGroup>();
			
			 foreach (IDataQuerySelectionGroup selectionGroup in dataQuery.SelectionGroups)
             {				
				//mapping selections
				ISet<IComplexDataQuerySelection> complexSelections = null;
				if (selectionGroup.Selections != null) {
					complexSelections = new HashSet<IComplexDataQuerySelection>();
					foreach (IDataQuerySelection querySelection in selectionGroup.Selections)
                    {
						String componentId = querySelection.ComponentId;
						ISet<IComplexComponentValue> complexComponentValues = new HashSet<IComplexComponentValue>();
						
						if (querySelection.Values != null)
                        {
							foreach (string value in querySelection.Values)
                            {
								IComplexComponentValue complexComponentValue = new ComplexComponentValueImpl(
										value, OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), 
                                        SdmxStructureEnumType.Dimension);
								complexComponentValues.Add(complexComponentValue);
							}
						}
						IComplexDataQuerySelection complexDataQuerySelection = new ComplexDataQuerySelectionImpl(componentId, complexComponentValues);
						complexSelections.Add(complexDataQuerySelection);
					}
				}
				
				//Time dimension Value
				ISdmxDate dateFrom = (selectionGroup.DateFrom != null) ? selectionGroup.DateFrom : null;
				OrderedOperator orderedOperatorFrom = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.GreaterThanOrEqual); 
				
				ISdmxDate dateTo = (selectionGroup.DateTo != null) ? selectionGroup.DateTo : null;
                OrderedOperator orderedOperatorTo = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.LessThanOrEqual); 
				
				IComplexDataQuerySelectionGroup complexSelectionGroup = new ComplexDataQuerySelectionGroupImpl(
						complexSelections, dateFrom, orderedOperatorFrom, 
						dateTo, orderedOperatorTo, null);
				
				complexSelectionGroups.Add(complexSelectionGroup);
			}
		 }
		
		  IComplexDataQuery complexDataQuery = new ComplexDataQueryImpl(datasetId, datasetIdOperator, 
				dataQuery.DataProvider, dataQuery.DataStructure, dataQuery.Dataflow, 
				provisionAgreement, lastUpdatedDate, firstNObs, lastNObs, defaultLimit, obsAction, 
				dimensionAtObservation, hasExplicitMeasures, queryDetail, complexSelectionGroups);
		   return complexDataQuery;
	  }
   }
}

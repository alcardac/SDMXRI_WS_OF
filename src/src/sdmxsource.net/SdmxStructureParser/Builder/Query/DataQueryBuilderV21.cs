// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query builder for <c>SDMX</c> v2.1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using DataStructureRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.DataStructureRequestType;
    using ITimeRange = Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex.ITimeRange;
    using TimeRangeCore = Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex.TimeRangeCore;

    class DataQueryBuilderV21
    {
        /// <summary>
        /// Processes an XML return details to return the detail parameters. DEFAULT set to FULL
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// The data query detail.
        /// </returns>
        private DataQueryDetail GetReturnDetailsDetail(DataReturnDetailsType returnDetails)
        {
            if (returnDetails.detail != null)
                return DataQueryDetail.ParseString(returnDetails.detail);
            else
                return DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

        }

        /// <summary>
        /// Processes an XML return details to return the first N observations
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// First N observations.
        /// </returns>
        private int? GetReturnDetailsFirstNobs(DataReturnDetailsType returnDetails)
        {
            return returnDetails.FirstNObservations;
        }

        /// <summary>
        /// Processes an XML return details to return the last N observations
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// Last N observations.
        /// </returns>
        private int? GetReturnDetailsLastNobs(DataReturnDetailsType returnDetails)
        {
            return returnDetails.LastNObservations;
        }

        /// <summary>
        /// Processes an XML return details to return the default limit value
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// The default limit value.
        /// </returns>
        private int GetReturnDetailsDefaultLimit(DataReturnDetailsType returnDetails)
        {
            if (returnDetails.defaultLimit != null)//.isSetDefaultLimit())
                return (int)returnDetails.defaultLimit.Value;
            else
                return 0;
        }

        /// <summary>
        /// Processes an XML return details to return the observation Action. Default to ACTIVE
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// The observation Action.
        /// </returns>
        private ObservationAction GetReturnDetailsObsAction(DataReturnDetailsType returnDetails)
        {
            if (returnDetails.observationAction != null)//.isSetObservationAction())
                return ObservationAction.ParseString(returnDetails.observationAction);
            else
                return ObservationAction.GetFromEnum(ObservationActionEnumType.Active);
        }

        /// <summary>
        /// Processes an XML return details to return the structure ref elements. 
        /// </summary>
        /// <param name="returnDetails">
        /// The return details.
        /// </param>
        /// <returns>
        /// A List with the structure ref details, first element is the dimensionAtObservation
        /// and second element of the list the hasExplicitMeasures value if exists. Can be NULL if a structure Ref is not specified
        /// </returns>
        private IList<object> GetStructureRefDetails(DataReturnDetailsType returnDetails)
        {
            IList<object> structureRefDetails = null;
            //string dimensionAtObservation = null;
            if (returnDetails.Structure != null && returnDetails.Structure.Count > 0)
            {
                structureRefDetails = new List<object>();
                IList<DataStructureRequestType> structureList = returnDetails.Structure;
                DataStructureRequestType structure = structureList[0];
                structureRefDetails.Add(structure.dimensionAtObservation);
                if (structure.explicitMeasures != null)//.isSetExplicitMeasures())
                    structureRefDetails.Add(structure.explicitMeasures);
            }
            return structureRefDetails;
        }

        /// <summary>
        /// Builds a data provider bean from DataProviderReferenceType from XML 
        /// </summary>
        /// <param name="dataProviderRef">
        /// The data provider reference.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The data provider.
        /// </returns>
        private IDataProvider ProcessDataProviderType(DataProviderReferenceType dataProviderRef, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            var dataProviderRefType = dataProviderRef.GetTypedRef<DataProviderRefType>();
            string agencyId = dataProviderRefType.agencyID;
            string id = dataProviderRefType.maintainableParentID;
            string version = dataProviderRefType.maintainableParentVersion;

            IMaintainableRefObject orgSchemeRef = new MaintainableRefObjectImpl(agencyId, id, version);
            IDataProviderScheme dataProviderScheme = structureRetrievalManager.GetMaintainableObject<IDataProviderScheme>(orgSchemeRef);
            foreach (IDataProvider dp in dataProviderScheme.Items)
            {
                if (dp.Id.Equals(dataProviderRefType.id))
                {
                    return dp;
                }
            }
            return null;
        }

        /// <summary>
        /// Processes an XML data where element to return the data providers.
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The data providers, an empty list if unspecified in the data query.
        /// </returns>
        private ISet<IDataProvider> GetDataWhereDataProviders(DataParametersAndType dataWhere, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            ISet<IDataProvider> dataProviders = new HashSet<IDataProvider>();
            if (dataWhere.DataProvider != null && dataWhere.DataProvider.Count > 0)
            {
                foreach (DataProviderReferenceType dataProviderRefType in dataWhere.DataProvider)
                {
                    IDataProvider dataProviderBean = ProcessDataProviderType(dataProviderRefType, structureRetrievalManager);
                    dataProviders.Add(dataProviderBean);
                }
            }
            return dataProviders;
        }

        /// <summary>
        /// Processes an XML data where element to return the data set identification and its operator.
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <returns>
        /// The data set identification and its operator. Can be NULL, if not specified in the xml data query.
        /// </returns>
        private string[] GetDataWhereDatasetId(DataParametersAndType dataWhere)
        {
            string[] datasetArray = null;
            string datasetId = null;
            string datasetIdOperator = null;
            if (dataWhere.DataSetID != null && dataWhere.DataSetID.Count > 0)
            {
                datasetArray = new string[2];
                datasetId = dataWhere.DataSetID[0].TypedValue;
                datasetArray[0] = datasetId;
                var @operator = dataWhere.DataSetID[0].@operator;
                if (@operator != null)
                {
                    string sOperator = @operator.ToString();
                    datasetIdOperator = sOperator;
                }
                else
                {
                    datasetIdOperator = "equals";
                }

                datasetArray[1] = datasetIdOperator;
            }
            return datasetArray;
        }

        /// <summary>
        /// Processes the dataWhere element to get the dataflow reference and retrieve the respective dataflow.
        /// It throws an exception if the dataflow retrieved is null
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The data flow object.
        /// </returns>
        private IDataflowObject GetDataWhereDataFlow(DataParametersAndType dataWhere, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            IDataflowObject dataFlow = null;
            if (dataWhere.Dataflow != null && dataWhere.Dataflow.Count > 0)
            {
                var dataflowRefType = dataWhere.Dataflow[0].GetTypedRef<DataflowRefType>();
                string dataFlowAgency = dataflowRefType.agencyID;
                string dataFlowId = dataflowRefType.id;
                string dataFlowVersion = null; // null if not specified so as to get the latest

                if (dataflowRefType.version != null)
                {
                    dataFlowVersion = dataflowRefType.version;
                }

                IMaintainableRefObject flowRef = new MaintainableRefObjectImpl(dataFlowAgency, dataFlowId, dataFlowVersion);
                dataFlow = structureRetrievalManager.GetMaintainableObject<IDataflowObject>(flowRef);
                if (dataFlow == null)
                {
                    throw new SdmxNoResultsException("Dataflow not found: " + flowRef);
                }
            }
            else
            {
                throw new ArgumentException("Can not create DataQuery, Dataflow is required");
            }
            return dataFlow;
        }

        /// <summary>
        /// Processes the dataWhere element to get the DSD reference and retrieve the respective DSD.
        /// It throws an exception if the DSD cannot be retrieved.
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <param name="dataFlow">
        /// The data flow.
        /// </param>
        /// <returns>
        /// The data structure object.
        /// </returns>
        private IDataStructureObject GetDataWhereDataStrucuture(DataParametersAndType dataWhere,
                                            ISdmxObjectRetrievalManager structureRetrievalManager,
                                            IDataflowObject dataFlow)
        {
            IDataStructureObject dataStructure = null;

            if (dataWhere.DataStructure != null && dataWhere.DataStructure.Count > 0)
            {
                var refBaseType = dataWhere.DataStructure[0].GetTypedRef<DataStructureRefType>();
                string dataStructureAgency = refBaseType.agencyID;
                string dataStructureId = refBaseType.id;
                string dataStructureVersion = null;
                if (refBaseType.version != null)
                    dataStructureVersion = refBaseType.version;

                IMaintainableRefObject dsdRef = new MaintainableRefObjectImpl(dataStructureAgency, dataStructureId, dataStructureVersion);
                dataStructure = structureRetrievalManager.GetMaintainableObject<IDataStructureObject>(dsdRef);
                if (dataStructure == null)
                {
                    throw new SdmxNoResultsException("DSD not found: " + dsdRef);
                }
            }
            else
            {
                IMaintainableRefObject dsdRef = dataFlow.DataStructureRef.MaintainableReference;
                dataStructure = structureRetrievalManager.GetMaintainableObject<IDataStructureObject>(dsdRef);
                if (dataStructure == null)
                {
                    throw new SdmxNoResultsException("Data Structure not found: " + dsdRef);
                }
            }
            return dataStructure;
        }

        /// <summary>
        /// Processes the dataWhere element to get the Provisionn Agreeement reference and retrieve the respective artefact bean.
        /// If the provision agreement cannot be found it throws an exception.
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The provision agreement object.
        /// </returns>
        private IProvisionAgreementObject GetProvisionAgreement(DataParametersAndType dataWhere, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            IProvisionAgreementObject provisionAgreement = null;

            if (dataWhere.ProvisionAgreement != null && dataWhere.ProvisionAgreement.Count > 0)
            {
                var refBaseType = dataWhere.ProvisionAgreement[0].GetTypedRef<ProvisionAgreementRefType>();
                string praAgency = refBaseType.agencyID;
                string praId = refBaseType.id;
                string praVersion = null;
                if (refBaseType.version != null)
                    praVersion = refBaseType.version;
                IMaintainableRefObject praRef = new MaintainableRefObjectImpl(praAgency, praId, praVersion);
                provisionAgreement = structureRetrievalManager.GetMaintainableObject<IProvisionAgreementObject>(praRef);
                if (provisionAgreement == null)
                {
                    throw new SdmxNoResultsException("Provision Agreement not found: " + praRef);
                }
            }
            return provisionAgreement;
        }

        /// <summary>
        /// Builds a time range from time range value from XML.
        /// </summary>
        /// <param name="timeRangeValueType">
        /// The time range value type.
        /// </param>
        /// <returns>
        /// The time range object.
        /// </returns>
        private ITimeRange BuildTimeRange(TimeRangeValueType timeRangeValueType)
        {
            bool range = false;
            ISdmxDate startDate = null;
            ISdmxDate endDate = null;
            bool endInclusive = false;
            bool startInclusive = false;

            if (timeRangeValueType.AfterPeriod != null)
            {
                TimePeriodRangeType afterPeriod = timeRangeValueType.AfterPeriod;
                startDate = new SdmxDateCore(afterPeriod.TypedValue.ToString());
                startInclusive = afterPeriod.isInclusive;
            }
            else if (timeRangeValueType.BeforePeriod != null)
            {
                TimePeriodRangeType beforePeriod = timeRangeValueType.BeforePeriod;
                endDate = new SdmxDateCore(beforePeriod.TypedValue.ToString());
                endInclusive = beforePeriod.isInclusive;
            }
            else
            { //case that range is set
                range = true;
                TimePeriodRangeType startPeriod = timeRangeValueType.StartPeriod;
                startDate = new SdmxDateCore(startPeriod.TypedValue.ToString());
                startInclusive = startPeriod.isInclusive;

                TimePeriodRangeType endPeriod = timeRangeValueType.EndPeriod;
                endDate = new SdmxDateCore(endPeriod.TypedValue.ToString());
                endInclusive = endPeriod.isInclusive;
            }
            return new TimeRangeCore(range, startDate, endDate, startInclusive, endInclusive);
        }

        /// <summary>
        /// Process the datawhere updated element to get the last updated information.
        /// </summary>
        /// <param name="dataWhere">
        /// The data where.
        /// </param>
        /// <returns>
        /// A list of time range objects.
        /// </returns>
        private IList<ITimeRange> GetDataWhereUpdatedDates(DataParametersAndType dataWhere)
        {
            IList<ITimeRange> updatedDates = new List<ITimeRange>();
            if (dataWhere.Updated.Count > 0)
            {
                //there should exist one or 2 time range values
                for (int i = 0; i < dataWhere.Updated.Count; i++)
                {
                    TimeRangeValueType timeRangeValueType = dataWhere.Updated[i];
                    updatedDates.Add(BuildTimeRange(timeRangeValueType));
                }
            }
            return updatedDates;
        }

        /// <summary>
        /// Parses the value types of the incoming component value type, and returns the complex component value to construct in the selection
        /// </summary>
        /// <param name="compValue">
        /// The component value.
        /// </param>
        /// <param name="compType">
        /// The component type : either a dimension, time dimension, attribute or primary measure.
        /// </param>
        /// <returns>
        /// The complex component value.
        /// </returns>
        private IComplexComponentValue GetComplexComponentValue(DataStructureComponentValueQueryType compValue, SdmxStructureEnumType compType)
        {
            IComplexComponentValue comValue = null;

            //Numeric Values
            if (compValue.NumericValue != null)
            {
                foreach (NumericValue numerValue in compValue.NumericValue)
                {
                    OrderedOperator orderedOperator = OrderedOperator.ParseString(numerValue.@operator.ToString());
                    comValue = new ComplexComponentValueImpl(numerValue.TypedValue.ToString(CultureInfo.InvariantCulture), orderedOperator, compType);
                }
            }
            //Time Value
            if (compValue.TimeValue != null)
            {
                foreach (TimeValue timeValue in compValue.TimeValue)
                {
                    OrderedOperator orderedOperator = OrderedOperator.ParseString(timeValue.@operator.ToString());
                    comValue = new ComplexComponentValueImpl(timeValue.TypedValue.ToString(), orderedOperator, compType);
                }
            }
            //Text Value applicable only for attribute and primary measure
            if (compType.Equals(SdmxStructureEnumType.PrimaryMeasure) || compType.Equals(SdmxStructureEnumType.DataAttribute))
            {
                //It's a generic type but the type will always be one
                if (compValue.TextValue != null)
                {
                    foreach (TextValue textValue in compValue.TextValue)
                    {
                        TextSearch textOperator = TextSearch.ParseString(textValue.@operator.ToString());
                        comValue = new ComplexComponentValueImpl(textValue.Content.TypedValue, textOperator, compType);
                    }
                }
            }
            //Value
            if (compValue.Value != null)
            {
                OrderedOperator orderedOperator = OrderedOperator.ParseString(compValue.Value.@operator.ToString());
                comValue = new ComplexComponentValueImpl(compValue.Value.TypedValue, orderedOperator, compType);
            }
            return comValue;
        }

        /// <summary>
        /// Parses the value types of the incoming component value type, and returns the complex component value to construct in the selection.
        /// </summary>
        /// <param name="compValue">
        /// The component value.
        /// </param>
        /// <param name="compType">
        /// The component type : either a dimension, time dimension, attribute or primary measure.
        /// </param>
        /// <returns>
        /// The set with the ComplexCompoentValue objects.
        /// </returns>
        private IList<IComplexComponentValue> GetComplexComponentValues(DataStructureComponentValueQueryType compValue, SdmxStructureEnumType compType)
        {
            IList<IComplexComponentValue> comValues = new List<IComplexComponentValue>();
            IComplexComponentValue comValue = null;

            //Numeric Values
            if (compValue.NumericValue != null)
            {
                foreach (NumericValue numerValue in compValue.NumericValue)
                {
                    OrderedOperator orderedOperator = OrderedOperator.ParseString(numerValue.@operator.ToString());
                    comValue = new ComplexComponentValueImpl(numerValue.TypedValue.ToString(CultureInfo.InvariantCulture), orderedOperator, compType);
                    comValues.Add(comValue);
                }
            }

            //Time Value
            if (compValue.TimeValue != null)
            {
                foreach (TimeValue timeValue in compValue.TimeValue)
                {
                    OrderedOperator orderedOperator = OrderedOperator.ParseString(timeValue.@operator.ToString());
                    comValue = new ComplexComponentValueImpl(timeValue.TypedValue.ToString(), orderedOperator, compType);
                    comValues.Add(comValue);
                }
            }

            //Text Value applicable only for attribute and primary measure
            if (compType.Equals(SdmxStructureEnumType.PrimaryMeasure) || compType.Equals(SdmxStructureEnumType.DataAttribute))
            {
                //It's a generic type but the type will always be one
                if (compValue.TextValue != null)
                {
                    foreach (TextValue textValue in compValue.TextValue)
                    {
                        TextSearch textOperator = TextSearch.ParseString(textValue.@operator.ToString());
                        comValue = new ComplexComponentValueImpl(textValue.ToString(), textOperator, compType);
                        comValues.Add(comValue);
                    }
                }
            }

            //Value
            if (compValue.Value != null)
            {
                OrderedOperator orderedOperator = OrderedOperator.ParseString(compValue.Value.@operator.ToString());
                comValue = new ComplexComponentValueImpl(compValue.Value.ToString(), orderedOperator, compType);
                comValues.Add(comValue);
            }

            return comValues;
        }

        /// <summary>
        /// Adds a selection value, either into an existing ComplexDataQuerySelection with the given concept, or a new DataQuerySelection if none exist with the given concept.
        /// </summary>
        /// <param name="complexSelections">
        /// The complex selections.
        /// </param>
        /// <param name="conceptId">
        /// The concept id.
        /// </param>
        /// <param name="value">
        /// The component value.
        /// </param>
        private void AddComponentSelection(ISet<IComplexDataQuerySelection> complexSelections, string conceptId, IComplexComponentValue value)
        {
            foreach (IComplexDataQuerySelection selection in complexSelections)
            {
                if (selection.ComponentId.Equals(conceptId))
                {
                    ((ComplexDataQuerySelectionImpl)selection).Values.Add(value);
                    return;
                }
            }

            IList<IComplexComponentValue> values = new List<IComplexComponentValue>();
            values.Add(value);
            IComplexDataQuerySelection newSelection = new ComplexDataQuerySelectionImpl(conceptId, values);
            complexSelections.Add(newSelection);
        }

        /// <summary>
        /// Adds a selection value, either into an existing ComplexDataQuerySelection with the given concept, or a new DataQuerySelection if none exist with the given concept.
        /// </summary>
        /// <param name="complexSelections">
        /// The complex selections.
        /// </param>
        /// <param name="conceptId">
        /// The concept id.
        /// </param>
        /// <param name="values">
        /// The components values.
        /// </param>
        private void AddComponentSelection(ISet<IComplexDataQuerySelection> complexSelections, String conceptId, ISet<IComplexComponentValue> values)
        {
            foreach (IComplexDataQuerySelection selection in complexSelections)
            {
                if (selection.ComponentId.Equals(conceptId))
                {
                    foreach (IComplexComponentValue value in values)
                    {
                        ((ComplexDataQuerySelectionImpl)selection).AddValue(value);
                    }
                    return;
                }
            }
            IComplexDataQuerySelection newSelection = new ComplexDataQuerySelectionImpl(conceptId, values);
            complexSelections.Add(newSelection);
        }

        /// <summary>
        /// Builds complex selections from DataParametersOr from XML.
        /// </summary>
        /// <param name="orType">
        /// The OrType.
        /// </param>
        /// <param name="complexSelections">
        /// The selections.
        /// </param>
        private void ProcessParametersOrType(DataParametersOrType orType,
                                                ISet<IComplexDataQuerySelection> complexSelections,
                                                ISdmxObjectRetrievalManager structureRetrievalManager,
                                                ISet<IDataProvider> dataProviders)
        {
            //data provider
            if (orType.DataProvider != null && orType.DataProvider.Count > 0)
            {
                foreach (DataProviderReferenceType dataProviderRefType in orType.DataProvider)
                {
                    IDataProvider dataProviderBean = ProcessDataProviderType(dataProviderRefType, structureRetrievalManager);
                    dataProviders.Add(dataProviderBean);
                }
            }
            //add a complex selection for dimensions
            if (orType.DimensionValue != null && orType.DimensionValue.Count > 0)
            {
                foreach (DimensionValueType dimValue in orType.DimensionValue)
                {
                    IComplexComponentValue comValue = GetComplexComponentValue(dimValue, SdmxStructureEnumType.Dimension);
                    AddComponentSelection(complexSelections, dimValue.ID, comValue);
                }
            }
            //add complex selections for attributes
            if (orType.AttributeValue != null && orType.AttributeValue.Count > 0)
            {
                foreach (AttributeValueType attrValue in orType.AttributeValue)
                {
                    IComplexComponentValue comValue = GetComplexComponentValue(attrValue, SdmxStructureEnumType.DataAttribute);
                    AddComponentSelection(complexSelections, attrValue.ID, comValue);
                }
            }
            //add complex selections for measure
            if (orType.PrimaryMeasureValue != null && orType.PrimaryMeasureValue.Count > 0)
            {
                foreach (PrimaryMeasureValueType measureValue in orType.PrimaryMeasureValue)
                {
                    IList<IComplexComponentValue> comValues = GetComplexComponentValues(measureValue, SdmxStructureEnumType.PrimaryMeasure);
                    this.AddComponentSelection(complexSelections, measureValue.ID, comValues);
                }
            }
            //add complex selections for time dimension
            if (orType.TimeDimensionValue != null && orType.TimeDimensionValue.Count > 0)
            {
                foreach (TimeDimensionValueType timeValue in orType.TimeDimensionValue)
                {
                    IList<IComplexComponentValue> comValues = GetComplexComponentValues(timeValue, SdmxStructureEnumType.TimeDimension);
                    AddComponentSelection(complexSelections, timeValue.ID, comValues);
                }
            }
            //TODO analyse to support in the future 
            //processParametersAndType(orType.getAndList(), complexSelections);		
        }

        private void AddComponentSelection(ISet<IComplexDataQuerySelection> complexSelections, string conceptId, IList<IComplexComponentValue> comValue)
        {
            foreach (IComplexDataQuerySelection selection in complexSelections)
            {
                if (selection.ComponentId.Equals(conceptId))
                {
                    foreach (var value in comValue)
                    {
                        // TODO fix this. We need mutable IComplexDataQuerySelection...
                        ((ComplexDataQuerySelectionImpl)selection).AddValue(value);
                    }

                    return;
                }
            }

            IComplexDataQuerySelection newSelection = new ComplexDataQuerySelectionImpl(conceptId, comValue);
            complexSelections.Add(newSelection);
        }

        /// <summary>
        /// Iterates through an unbound number of DataParametersOrType and call the respective method to process the OrType
        /// </summary>
        /// <param name="orTypeList">
        /// The OrTypes list.
        /// </param>
        /// <param name="complexSelections">
        /// The selections.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <param name="dataProviders">
        /// The data providers.
        /// </param>
        private void ProcessParametersOrType(IList<DataParametersOrType> orTypeList,
                                            ISet<IComplexDataQuerySelection> complexSelections,
                                            ISdmxObjectRetrievalManager structureRetrievalManager,
                                            ISet<IDataProvider> dataProviders)
        {
            if (orTypeList != null)
            {
                foreach (DataParametersOrType orType in orTypeList)
                {
                    ProcessParametersOrType(orType, complexSelections, structureRetrievalManager, dataProviders);
                }
            }
        }

        /// <summary>
        /// Id adds a selection group only if it has at least one of the following date from, date to, valid collection of primary measure values, valid collection of complex selections. <br>
        /// A collection is considered valid when it is not null and contains at least one element.
        /// </summary>
        /// <param name="complexSelections">
        /// The selections.
        /// </param>
        /// <param name="dateFrom">
        /// The date from.
        /// </param>
        /// <param name="dateFromOperator">
        /// The date from operator.
        /// </param>
        /// <param name="dateTo">
        /// The date to.
        /// </param>
        /// <param name="dateToOperator">
        /// The date to operator.
        /// </param>
        /// <param name="primaryMeasureValues">
        /// The primary measure values.
        /// </param>
        /// <param name="complexDataQuerySelectionGroups">
        /// The complex data query selection groups.
        /// </param>
        private void AddGroupIfSelectionsExist(ISet<IComplexDataQuerySelection> complexSelections,
                                                ISdmxDate dateFrom, OrderedOperator dateFromOperator,
                                                ISdmxDate dateTo, OrderedOperator dateToOperator,
                                                ISet<IComplexComponentValue> primaryMeasureValues,
                                                ISet<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroups)
        {
            if (ObjectUtil.ValidCollection(complexSelections) || dateFrom != null || dateTo != null || ObjectUtil.ValidCollection(primaryMeasureValues))
            {
                complexDataQuerySelectionGroups.Add(new ComplexDataQuerySelectionGroupImpl(complexSelections, dateFrom, dateFromOperator, dateTo, dateToOperator, primaryMeasureValues));
            }
        }

        /// <summary>
        /// Builds the complex data query groups, by processing the or types, dimension/primary measure/attribute/time dimension values.
        ///  It then calls the 'addGroupIfSelectionsExist' method in order to add the newly built-in ComplexDataQuerySelectionGroup. It returns
        /// a set of Complex Data Query Selection Groups.
        /// Build complex data queries from the specified <paramref name="dataWhere"/>
        /// </summary>
        /// <param name="dataWhere">
        /// The data query type.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <param name="dataProviders">
        /// The data providers.
        /// </param>
        /// <returns>
        /// The complex data query groups.
        /// </returns>
        private ISet<IComplexDataQuerySelectionGroup> BuildComplexDataQueryGroups(DataParametersAndType dataWhere, ISdmxObjectRetrievalManager structureRetrievalManager, ISet<IDataProvider> dataProviders)
        {
            ISet<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroups = new HashSet<IComplexDataQuerySelectionGroup>();
            ISet<IComplexDataQuerySelection> complexSelections = new HashSet<IComplexDataQuerySelection>();
            ISet<IComplexComponentValue> primaryMeasureValues = new HashSet<IComplexComponentValue>();
            OrderedOperator dateFromOperator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal);
            OrderedOperator dateToOperator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal);
            ISdmxDate dateFrom = null;
            ISdmxDate dateTo = null;

            //primary measure
            if (dataWhere.PrimaryMeasureValue != null && (dataWhere.PrimaryMeasureValue.Count > 0))
            {
                PrimaryMeasureValueType primaryMeasure = dataWhere.PrimaryMeasureValue[0];
                var complexValues = GetComplexComponentValues(primaryMeasure, SdmxStructureEnumType.PrimaryMeasure);
                foreach (var complexValue in complexValues)
                {
                    primaryMeasureValues.Add(complexValue);
                }
            }
            //time dimension
            if (dataWhere.TimeDimensionValue != null && dataWhere.TimeDimensionValue.Count > 0)
            {
                TimeDimensionValueType timeValue = dataWhere.TimeDimensionValue[0];
                var complexValues = GetComplexComponentValues(timeValue, SdmxStructureEnumType.TimeDimension);

                if (complexValues != null && complexValues.Count > 0)
                {
                    var complexValue = complexValues[0];
                    switch (complexValue.OrderedOperator.EnumType)
                    {
                        case OrderedOperatorEnumType.GreaterThan:
                        case OrderedOperatorEnumType.GreaterThanOrEqual:
                        case OrderedOperatorEnumType.Equal:
                            dateFromOperator = complexValue.OrderedOperator;
                            dateFrom = new SdmxDateCore(complexValue.Value);
                            if (complexValues.Count == 2)
                            {
                                dateTo = new SdmxDateCore(complexValues[1].Value);
                                dateToOperator = complexValues[1].OrderedOperator;
                            }

                            break;
                        default:
                            dateToOperator = complexValue.OrderedOperator;
                            dateTo = new SdmxDateCore(complexValue.Value);
                            if (complexValues.Count == 2)
                            {
                                dateFrom = new SdmxDateCore(complexValues[1].Value);
                                dateFromOperator = complexValues[1].OrderedOperator;
                            }

                            break;
                    }

                    if (complexValues.Count == 2)
                    {
                        if (dateFrom != null && dateFrom.IsLater(dateTo))
                        {
                            // interchange dates if not the correct order
                            var tempDate = dateTo;
                            dateTo = dateFrom;
                            dateFrom = tempDate;
                            var tempOperator = dateToOperator;
                            dateToOperator = dateFromOperator;
                            dateFromOperator = tempOperator;
                        }

                        // cases when same operator is used
                        if (dateToOperator.Equals(dateFromOperator))
                        {
                            switch (dateToOperator.EnumType)
                            {
                                case OrderedOperatorEnumType.GreaterThan:
                                case OrderedOperatorEnumType.GreaterThanOrEqual:

                                    // only the greatest date is considered
                                    dateFrom = dateTo;
                                    dateTo = null;
                                    break;
                                case OrderedOperatorEnumType.LessThan:
                                case OrderedOperatorEnumType.LessThanOrEqual:

                                    // only the lowest date is considered
                                    dateTo = dateFrom;
                                    dateFrom = null;
                                    break;
                            }
                        }
                    }
                }
            }
            //dimensions
            if (dataWhere.DimensionValue != null && dataWhere.DimensionValue.Count > 0)
            {
                foreach (DimensionValueType dimValue in dataWhere.DimensionValue)
                {
                    IComplexComponentValue comValue = GetComplexComponentValue(dimValue, SdmxStructureEnumType.Dimension);
                    AddComponentSelection(complexSelections, dimValue.ID, comValue);
                }
            }
            //attributes
            if (dataWhere.AttributeValue != null && dataWhere.AttributeValue.Count > 0)
            {
                foreach (AttributeValueType attrValue in dataWhere.AttributeValue)
                {
                    IComplexComponentValue comValue = GetComplexComponentValue(attrValue, SdmxStructureEnumType.DataAttribute);
                    AddComponentSelection(complexSelections, attrValue.ID, comValue);
                }
            }
            //DataParametersOrType
            ProcessParametersOrType(dataWhere.Or, complexSelections, structureRetrievalManager, dataProviders);
            AddGroupIfSelectionsExist(complexSelections, dateFrom, dateFromOperator, dateTo, dateToOperator, primaryMeasureValues, complexDataQuerySelectionGroups);
            return complexDataQuerySelectionGroups;
        }

        /// <summary>
        /// Build complex data queries from the specified <paramref name="dataQueryType"/>
        /// </summary>
        /// <param name="dataQueryType">
        /// The data query type.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The list of complex data query from the specified <paramref name="dataQueryType"/>
        /// </returns>
        public IList<IComplexDataQuery> BuildComplexDataQuery(DataQueryType dataQueryType, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            if (structureRetrievalManager == null)
            {
                throw new SystemException("ComplexDataQueryBuilder expectes a ISdmxObjectRetrievalManager");
            }

            IList<IComplexDataQuery> returnList = new List<IComplexDataQuery>();
            DataReturnDetailsType returnDetails = dataQueryType.ReturnDetails;
            DataParametersAndType dataWhere = dataQueryType.DataWhere;

            /**process the DataReturnDetailsType*/
            DataQueryDetail queryDetail = GetReturnDetailsDetail(returnDetails);
            int? firstNObs = GetReturnDetailsFirstNobs(returnDetails);
            int? lastNObs = GetReturnDetailsLastNobs(returnDetails);
            int defaultLimit = GetReturnDetailsDefaultLimit(returnDetails);
            ObservationAction obsAction = GetReturnDetailsObsAction(returnDetails);
            IList<object> structureReferenceDetails = GetStructureRefDetails(returnDetails);
            string dimensionAtObservation = (structureReferenceDetails != null) ? (string)structureReferenceDetails[0] : null;
            bool hasExplicitMeasures = (structureReferenceDetails != null && structureReferenceDetails.Count > 1) ? (bool)structureReferenceDetails[1] : false;
            /**process the DataParametersAndType*/
            //Get the Data Providers
            ISet<IDataProvider> dataProviders = GetDataWhereDataProviders(dataWhere, structureRetrievalManager);
            //Get the DatasetId
            string[] datasetArray = GetDataWhereDatasetId(dataWhere);
            string datasetId = (datasetArray != null) ? datasetArray[0] : null;
            TextSearch datasetIdOper = (datasetArray != null) ? TextSearch.ParseString(datasetArray[1]) : null;
            //Get the DataFlow
            IDataflowObject dataFlow = GetDataWhereDataFlow(dataWhere, structureRetrievalManager);
            //Get the Data Structure Definition
            IDataStructureObject dataStructure = GetDataWhereDataStrucuture(dataWhere, structureRetrievalManager, dataFlow);
            //Get the Provision Agreement
            IProvisionAgreementObject provisionAgreement = GetProvisionAgreement(dataWhere, structureRetrievalManager);
            //Get the Updated dates
            IList<ITimeRange> updatedDates = GetDataWhereUpdatedDates(dataWhere);
            //Get the ComplexDataQueryGoups
            ISet<IComplexDataQuerySelectionGroup> complexDataQuerySelectionGroups = BuildComplexDataQueryGroups(dataWhere, structureRetrievalManager, dataProviders);
            //Build the complex Query
            IComplexDataQuery complexQuery = new ComplexDataQueryImpl(datasetId, datasetIdOper, dataProviders, dataStructure, dataFlow, provisionAgreement, updatedDates, firstNObs, lastNObs, defaultLimit, obsAction, dimensionAtObservation, hasExplicitMeasures, queryDetail, complexDataQuerySelectionGroups);
            returnList.Add(complexQuery);
            return returnList;
        }

        /// <summary>
        /// Processes the ParametersAndType, to be supported in the future
        /// </summary>
        /// <param name="andType">
        /// The and type.
        /// </param>
        /// <param name="complexSelections">
        /// The complex selections.
        /// </param>
        private void ProcessParametersAndType(DataParametersAndType andType,
                                                ISet<IComplexDataQuerySelection> complexSelections)
        {
            //ProcessParametersOrType(andType.getOrList(),complexSelections);
        }

        /// <summary>
        /// Iterates through an unbound number of DataParametersAndType and call the respective method to process the AndType
        /// </summary>
        /// <param name="andTypeList">
        /// The and type list.
        /// </param>
        /// <param name="complexSelections">
        /// The complex selections.
        /// </param>
        private void ProcessParametersAndType(IList<DataParametersAndType> andTypeList,
                                                    ISet<IComplexDataQuerySelection> complexSelections)
        {
            if (andTypeList != null)
            {
                foreach (DataParametersAndType andType in andTypeList)
                {
                    ProcessParametersAndType(andType, complexSelections);
                }
            }
        }
    }
}
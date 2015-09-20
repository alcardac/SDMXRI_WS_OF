// -----------------------------------------------------------------------
// <copyright file="ComplexDataQueryCoreBuilderV21.cs" company="Eurostat">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.CustomRequests.Extension;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    using DataStructureRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataStructureRequestType;
    using SimpleValueType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.SimpleValueType;

    /// <summary>
    /// Class that contain common implementation for DataSet Request:
    /// GetGenericData, GetGenericTimeSeriesData, GetStructureSpecificData, GetStructureSpecificTimeSeriesData
    /// </summary>
    public class ComplexDataQueryCoreBuilderV21
    {
        /// <summary>
        /// The default structure unique identifier
        /// </summary>
        private const string DefaultStructureId = "StructureId";

        /// <summary>
        /// The default equal operator
        /// </summary>
        private const string DefaultEqualOperator = "equal";

        /// <summary>
        /// The query component value type
        /// </summary>
        private enum ValueType
        {
            /// <summary>
            /// The numeric value
            /// </summary>
            NumericValue,

            /// <summary>
            /// The text value
            /// </summary>
            TextValue,

            /// <summary>
            /// The time value
            /// </summary>
            TimeValue,

            /// <summary>
            /// The value
            /// </summary>
            Value
        }

        /// <summary>
        /// Fills the type of the data query.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxException">Too many Selection Groups in ComplexDataQuery max one supported.</exception>
        public void FillDataQueryType(DataQueryType queryType, IComplexDataQuery complexDataQuery)
        {
            var dataReturnDetailsType = new DataReturnDetailsType();
            queryType.ReturnDetails = dataReturnDetailsType;

            // TODO: check null in java
            if (complexDataQuery.DefaultLimit.HasValue && complexDataQuery.DefaultLimit.Value != 0)
            {
                dataReturnDetailsType.defaultLimit = complexDataQuery.DefaultLimit;
            }

            if (complexDataQuery.FirstNObservations.HasValue)
            {
                dataReturnDetailsType.FirstNObservations = complexDataQuery.FirstNObservations;
            }

            if (complexDataQuery.LastNObservations.HasValue)
            {
                dataReturnDetailsType.LastNObservations = complexDataQuery.LastNObservations;
            }
            if (complexDataQuery.DataQueryDetail != null)
            {
                dataReturnDetailsType.detail = complexDataQuery.DataQueryDetail.EnumType.ToString();
            }

            if (complexDataQuery.ObservationAction != null)
            {
                dataReturnDetailsType.observationAction = complexDataQuery.ObservationAction.EnumType.ToString();
            }

            HandleStructure(complexDataQuery, dataReturnDetailsType);
            var whereType = new DataParametersAndType();
            queryType.DataWhere = whereType;

            //DataSetId at DataWhere Leveln - Optional
            if (complexDataQuery.DatasetId != null)
            {
                var datasetIdType = new QueryIDType { TypedValue = complexDataQuery.DatasetId, @operator = complexDataQuery.DatasetIdOperator.Operator };

                whereType.DataSetID.Add(datasetIdType);

            }

            //DataProvider at DataWhere Level - Optional; 
            if (complexDataQuery.DataProvider != null && complexDataQuery.DataProvider.Count != 0)
            {
                HandleDataProvider(complexDataQuery, whereType);
            }

            //Dataflow at DataWhere Level - Optional
            if (complexDataQuery.Dataflow != null)
            {

                HandleDataflow(complexDataQuery, whereType);
            }

            //ProvisionAgreement at DataWhere Level - Optional  
            if (complexDataQuery.ProvisionAgreement != null)
            {

                HandleProvisionAgreement(complexDataQuery, whereType);
            }

            //Updated tag that can have 0 to 2 repetitions
            if (complexDataQuery.LastUpdatedDateTimeRange != null)
            {
                HandleLastUpdatedTime(complexDataQuery, whereType);
            }

            //Selection Groups - and clause (DataWhere)
            if (complexDataQuery.SelectionGroups != null && complexDataQuery.SelectionGroups.Count > 1)
            {
                throw new SdmxException("Too many Selection Groups in ComplexDataQuery max one supported.", SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError));
            }

            if (complexDataQuery.SelectionGroups != null && complexDataQuery.SelectionGroups.Count != 0)
            {
                HandleSelectionGroups(complexDataQuery, whereType);
            }
        }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="complexComponentValue">The complex component value.</param>
        /// <returns>The value type</returns>
        private static ValueType GetValueType(IComponent component, IComplexComponentValue complexComponentValue)
        {
            if (component.HasCodedRepresentation())
            {
                return ValueType.Value;
            }

            var defaultValueType = ValueType.TextValue;
            if (complexComponentValue.TextSearchOperator != null)
            {
                return ValueType.TextValue;
            }
            
            if (complexComponentValue.OrderedOperator != null)
            {
                switch (complexComponentValue.OrderedOperator.EnumType)
                {
                    case OrderedOperatorEnumType.GreaterThanOrEqual:
                    case OrderedOperatorEnumType.LessThanOrEqual:
                    case OrderedOperatorEnumType.LessThan:
                    case OrderedOperatorEnumType.GreaterThan:
                        defaultValueType = ValueType.NumericValue;
                        break;
                    default:
                        defaultValueType = ValueType.Value;
                        break;
                }
            }

            if (component.Representation != null && component.Representation.TextFormat != null)
            {
                if (component.Representation.TextFormat.TextType != null)
                {
                    switch (component.Representation.TextFormat.TextType.EnumType)
                    {
                        case TextEnumType.BasicTimePeriod:
                        case TextEnumType.DateTime:
                        case TextEnumType.Date:
                        case TextEnumType.Time:
                        case TextEnumType.Year:
                        case TextEnumType.Month:
                        case TextEnumType.Day:
                        case TextEnumType.MonthDay:
                        case TextEnumType.YearMonth:
                        case TextEnumType.Duration:
                        case TextEnumType.Timespan:
                        case TextEnumType.TimePeriod:
                        case TextEnumType.ObservationalTimePeriod:
                        case TextEnumType.GregorianDay:
                        case TextEnumType.GregorianTimePeriod:
                        case TextEnumType.GregorianYear:
                        case TextEnumType.GregorianYearMonth:
                        case TextEnumType.ReportingDay:
                        case TextEnumType.ReportingMonth:
                        case TextEnumType.ReportingQuarter:
                        case TextEnumType.ReportingSemester:
                        case TextEnumType.ReportingTimePeriod:
                        case TextEnumType.ReportingTrimester:
                        case TextEnumType.ReportingWeek:
                        case TextEnumType.ReportingYear:
                        case TextEnumType.StandardTimePeriod:
                        case TextEnumType.TimesRange:
                            return ValueType.TimeValue;
                        case TextEnumType.BigInteger:
                        case TextEnumType.Integer:
                        case TextEnumType.Long:
                        case TextEnumType.Short:
                        case TextEnumType.Decimal:
                        case TextEnumType.Float:
                        case TextEnumType.Double:
                        case TextEnumType.Count:
                        case TextEnumType.Numeric:
                        case TextEnumType.InclusiveValueRange:
                        case TextEnumType.ExclusiveValueRange:
                            return ValueType.NumericValue;
                        case TextEnumType.Boolean:
                            return ValueType.Value;
                    }
                }

                return defaultValueType;
            }

            // try guessing type from component type.
            switch (component.StructureType.EnumType)
            {
                case SdmxStructureEnumType.Dimension:
                    return ValueType.Value;
                case SdmxStructureEnumType.PrimaryMeasure:
                    return ValueType.NumericValue;
                case SdmxStructureEnumType.TimeDimension:
                    return ValueType.TimeValue;
            }

            return defaultValueType;
        }

        /// <summary>
        /// Handles the selection groups.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        private static void HandleSelectionGroups(IComplexDataQuery complexDataQuery, DataParametersAndType whereType)
        {
            // TODO support for multiple selection groups. 
            IComplexDataQuerySelectionGroup complexDataQuerySelectionGroup = complexDataQuery.SelectionGroups.First();

            //Selections in SelectionGroups - at DataWhere Level
            if (complexDataQuerySelectionGroup.Selections != null)
            {
                HandleSelections(complexDataQuery, whereType, complexDataQuerySelectionGroup.Selections.Where(selection => selection.ShouldUseAnd()), new ParameterBuilderAnd());
            }

            //Time Dimension clause at DataWhere Level for Date From/To - optional
            if (complexDataQuerySelectionGroup.DateFrom != null || complexDataQuerySelectionGroup.DateTo != null)
            {
                HandleTime(whereType, complexDataQuerySelectionGroup);
            }

            //Primary Measure Value at DataWhere Level - Zero to more repetitions
            if (complexDataQuerySelectionGroup.PrimaryMeasureValue != null)
            {
                HandlePrimaryMeasure(whereType, complexDataQuerySelectionGroup, complexDataQuery);
            }

            //Selections in SelectionGroups - OR clauses at DataWhere Level
            if (complexDataQuerySelectionGroup.Selections != null)
            {
                HandleSelections(complexDataQuery, whereType, complexDataQuerySelectionGroup.Selections.Where(selection => !selection.ShouldUseAnd()), new ParameterBuilderOr());
            }
        }

        /// <summary>
        /// Handles the time.
        /// </summary>
        /// <param name="whereType">Type of the where.</param>
        /// <param name="complexDataQuerySelectionGroup">The complex data query selection group.</param>
        private static void HandleTime(DataParametersType whereType, IComplexDataQuerySelectionGroup complexDataQuerySelectionGroup)
        {
            var timeDimensionValueType = new TimeDimensionValueType();

            if (complexDataQuerySelectionGroup.DateFrom != null)
            {
                var timePeriodValueType = new TimeValue();

                timePeriodValueType.TypedValue = complexDataQuerySelectionGroup.DateFrom.DateInSdmxFormat;
                timePeriodValueType.@operator = complexDataQuerySelectionGroup.DateFromOperator.OrdOperator;
                timeDimensionValueType.TimeValue.Add(timePeriodValueType);
            }

            if (complexDataQuerySelectionGroup.DateTo != null)
            {
                var timePeriodValueType = new TimeValue();
                timePeriodValueType.TypedValue = complexDataQuerySelectionGroup.DateTo.DateInSdmxFormat;
                timePeriodValueType.@operator = complexDataQuerySelectionGroup.DateToOperator.OrdOperator;
                timeDimensionValueType.TimeValue.Add(timePeriodValueType);
            }
            whereType.TimeDimensionValue.Add(timeDimensionValueType);
        }

        /// <summary>
        /// Handles the primary measure.
        /// </summary>
        /// <param name="whereType">Type of the where.</param>
        /// <param name="complexDataQuerySelectionGroup">The complex data query selection group.</param>
        /// <param name="complexDataQuery">The complex data query.</param>
        private static void HandlePrimaryMeasure(DataParametersType whereType, IComplexDataQuerySelectionGroup complexDataQuerySelectionGroup, IComplexDataQuery complexDataQuery)
        {
            var primaryMeasure = complexDataQuery.DataStructure.PrimaryMeasure;
            foreach (IComplexComponentValue primaryMeasureValue in complexDataQuerySelectionGroup.PrimaryMeasureValue)
            {
                var primaryMeasureValueType = new PrimaryMeasureValueType();
                SetOperatorAndValue(primaryMeasure, primaryMeasureValue, primaryMeasureValueType);

                whereType.PrimaryMeasureValue.Add(primaryMeasureValueType);
            }
        }

        /// <summary>
        /// Sets the operator and value.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="complexComponentValue">The complex component value.</param>
        /// <param name="dataStructureComponentValueQueryType">Type of the data structure component value query.</param>
        private static void SetOperatorAndValue(IComponent component, IComplexComponentValue complexComponentValue, DataStructureComponentValueQueryType dataStructureComponentValueQueryType)
        {
            var valueType = GetValueType(component, complexComponentValue);
            switch (valueType)
            {
                case ValueType.NumericValue:
                    SetNumericOperatorAndValue(dataStructureComponentValueQueryType, complexComponentValue);
                    break;
                case ValueType.TextValue:
                    SetTextOperatorAndValue(dataStructureComponentValueQueryType, complexComponentValue);
                    break;
                case ValueType.TimeValue:
                    SetTimeOperatorAndValue(dataStructureComponentValueQueryType, complexComponentValue);
                    break;
                case ValueType.Value:
                    SetSimpleOperatorAndValue(dataStructureComponentValueQueryType, complexComponentValue);
                    break;
            }
        }

        /// <summary>
        /// Handles the selections.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        /// <param name="complexDataQuerySelections"></param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">
        /// Invalid structure type for component and 
        /// </exception>
        private static void HandleSelections(IComplexDataQuery complexDataQuery, DataParametersAndType whereType, IEnumerable<IComplexDataQuerySelection> complexDataQuerySelections, IParameterBuilder builder)
        {
            foreach (IComplexDataQuerySelection complexDataQuerySelection in complexDataQuerySelections)
            {

                // Dimension Values inside an OR -or- AND clause
                foreach (IComplexComponentValue complexComponentValue in complexDataQuerySelection.Values)
                {
                    var component = complexDataQuery.DataStructure.GetComponent(complexDataQuerySelection.ComponentId);
                    if (component == null)
                    {
                        throw new SdmxSemmanticException(string.Format("Component with ID {0} does not exist in the DSD {1}", complexDataQuerySelection.ComponentId, complexDataQuery.DataStructure));
                    }

                    switch (component.StructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Dimension:
                        case SdmxStructureEnumType.MeasureDimension:
                            {
                                var dimensionValueType = new DimensionValueType { ID = complexDataQuerySelection.ComponentId };
                                SetOperatorAndValue(component, complexComponentValue, dimensionValueType);
                                builder.AddDimension(dimensionValueType);
                            }

                            break;
                        case SdmxStructureEnumType.DataAttribute:
                            {
                                var attributeValueType = new AttributeValueType { ID = complexDataQuerySelection.ComponentId };
                                SetOperatorAndValue(component, complexComponentValue, attributeValueType);

                                builder.AddAttribute(attributeValueType);
                            }

                            break;
                        default:
                            throw new SdmxSemmanticException(string.Format("Invalid structure type for component ID : {1} Type {0} ", component.StructureType.EnumType, component.Urn));
                    }
                }

                builder.PopulateAndParameter(whereType);
            }
        }

        /// <summary>
        /// Gets the parameter builder.
        /// </summary>
        /// <param name="complexDataQuerySelection">The complex data query selection.</param>
        /// <returns>The <see cref="IParameterBuilder"/></returns>
        private static IParameterBuilder GetParameterBuilder(IComplexDataQuerySelection complexDataQuerySelection)
        {
            IParameterBuilder builder;
            if (complexDataQuerySelection.ShouldUseAnd())
            {
                builder = new ParameterBuilderAnd();
            }
            else
            {
                builder = new ParameterBuilderOr();
            }

            return builder;
        }

        /// <summary>
        /// Handles the last updated time.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        private static void HandleLastUpdatedTime(IComplexDataQuery complexDataQuery, DataParametersType whereType)
        {
            foreach (var timeRange in complexDataQuery.LastUpdatedDateTimeRange)
            {
                var timeRangeValueType = new TimeRangeValueType();

                var startTimePeriodRangeType = new TimePeriodRangeType();

                startTimePeriodRangeType.isInclusive = timeRange.IsStartInclusive;
                startTimePeriodRangeType.TypedValue = timeRange.StartDate.DateInSdmxFormat;

                timeRangeValueType.StartPeriod = startTimePeriodRangeType;

                var endTimePeriodRangeType = new TimePeriodRangeType();

                endTimePeriodRangeType.isInclusive = timeRange.IsEndInclusive;
                endTimePeriodRangeType.TypedValue = timeRange.StartDate.DateInSdmxFormat;

                timeRangeValueType.EndPeriod = endTimePeriodRangeType;

                whereType.Updated.Add(timeRangeValueType);
            }
        }

        /// <summary>
        /// Handles the provision agreement.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        private static void HandleProvisionAgreement(IComplexDataQuery complexDataQuery, DataParametersAndType whereType)
        {
            RefBaseType provisionReferenceType = new ProvisionAgreementRefType();

            provisionReferenceType.id = complexDataQuery.ProvisionAgreement.Id;
            provisionReferenceType.agencyID = complexDataQuery.ProvisionAgreement.AgencyId;
            provisionReferenceType.version = complexDataQuery.ProvisionAgreement.Version;

            var provisionAgreementReferenceType = new ProvisionAgreementReferenceType { Ref = provisionReferenceType };
            whereType.ProvisionAgreement.Add(provisionAgreementReferenceType);
        }

        /// <summary>
        /// Handles the dataflow.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        private static void HandleDataflow(IComplexDataQuery complexDataQuery, DataParametersAndType whereType)
        {
            var dataflowReferenceType = new DataflowReferenceType();

            RefBaseType dataflowRefType = new DataflowRefType();

            dataflowRefType.agencyID = complexDataQuery.Dataflow.AgencyId;
            dataflowRefType.id = complexDataQuery.Dataflow.Id;
            dataflowRefType.version = complexDataQuery.Dataflow.Version;
            dataflowReferenceType.Ref = dataflowRefType;
            whereType.Dataflow.Add(dataflowReferenceType);
        }

        /// <summary>
        /// Handles the data provider.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="whereType">Type of the where.</param>
        private static void HandleDataProvider(IComplexDataQuery complexDataQuery, DataParametersAndType whereType)
        {
            //if only one Data provider let alone otherwise encapsulate it in an Or clause
            if (complexDataQuery.DataProvider.Count == 1)
            {
                IDataProvider dataProvider = complexDataQuery.DataProvider.First();
                var dataProviderReferenceType = new DataProviderReferenceType();

                RefBaseType dataProviderRefType = new DataProviderRefType();

                dataProviderRefType.agencyID = dataProvider.CrossReferences.First().MaintainableReference.AgencyId;
                dataProviderRefType.id = dataProvider.Id;
                dataProviderRefType.maintainableParentID = dataProvider.MaintainableParent.Id;
                dataProviderRefType.maintainableParentVersion = dataProvider.MaintainableParent.Version;

                dataProviderReferenceType.Ref = dataProviderRefType;
                whereType.DataProvider.Add(dataProviderReferenceType);
            }
            else
            {
                var orForDataProviderType = new DataParametersOrType();

                foreach (IDataProvider dataProvider in complexDataQuery.DataProvider)
                {
                    RefBaseType dataProviderRefType = new DataProviderRefType();
                    dataProviderRefType.agencyID = dataProvider.CrossReferences.First().MaintainableReference.AgencyId;
                    dataProviderRefType.id = dataProvider.Id;
                    dataProviderRefType.maintainableParentID = dataProvider.MaintainableParent.Id;
                    dataProviderRefType.maintainableParentVersion = dataProvider.MaintainableParent.Version;
                    var dataProviderReferenceType = new DataProviderReferenceType { Ref = dataProviderRefType };
                    orForDataProviderType.DataProvider.Add(dataProviderReferenceType);
                }

                whereType.Or.Add(orForDataProviderType);
            }
        }

        /// <summary>
        /// Handles the structure request type
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="dataReturnDetails">The data return details.</param>
        private static void HandleStructure(IComplexDataQuery complexDataQuery, DataReturnDetailsType dataReturnDetails)
        {
            var structure = new DataStructureRequestType();
            dataReturnDetails.Structure.Add(structure);
            if (!string.IsNullOrWhiteSpace(complexDataQuery.DimensionAtObservation))
            {
                structure.dimensionAtObservation = complexDataQuery.DimensionAtObservation;
            }
            else
            {
                structure.dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
            }

            HandleExplicit(complexDataQuery, structure);
            structure.structureID = DefaultStructureId;
            
            if (complexDataQuery.DataStructure != null)
            {
                RefBaseType structureRefernce = new StructureRefType();
                structureRefernce.agencyID = complexDataQuery.DataStructure.AgencyId;
                structureRefernce.id = complexDataQuery.DataStructure.Id;
                structureRefernce.version = complexDataQuery.DataStructure.Version;
                var structureType = new StructureReferenceType();
                structureType.SetTypedRef(structureRefernce);
                structure.Structure = structureType;
            }
            else if (complexDataQuery.ProvisionAgreement != null)
            {
                RefBaseType provisionAgreementReference = new ProvisionAgreementRefType();
                provisionAgreementReference.id = complexDataQuery.ProvisionAgreement.Id;
                provisionAgreementReference.agencyID = complexDataQuery.ProvisionAgreement.AgencyId;
                provisionAgreementReference.version = complexDataQuery.ProvisionAgreement.Version;
                var provisionAgreementReferenceType = new ProvisionAgreementReferenceType();
                provisionAgreementReferenceType.SetTypedRef(provisionAgreementReference);
                structure.ProvisionAgrement = provisionAgreementReferenceType;
            }
        }

        /// <summary>
        /// Handles the explicit.
        /// </summary>
        /// <param name="complexDataQuery">The complex data query.</param>
        /// <param name="structure">The structure.</param>
        private static void HandleExplicit(IComplexDataQuery complexDataQuery, DataStructureRequestType structure)
        {
            if (complexDataQuery.HasExplicitMeasures())
            {
                structure.explicitMeasures = complexDataQuery.HasExplicitMeasures();
            }
        }

       /// <summary>
       /// Sets the numeric operator and value.
       /// </summary>
       /// <param name="rootObject">The root object.</param>
       /// <param name="complexComponentValue">The complex component value.</param>
        private static void SetNumericOperatorAndValue(DataStructureComponentValueQueryType rootObject, IComplexComponentValue complexComponentValue)
       {
            var numericValue = new NumericValue { TypedValue = decimal.Parse(complexComponentValue.Value, CultureInfo.InvariantCulture), @operator = complexComponentValue.OrderedOperator.OrdOperator };
            rootObject.NumericValue.Add(numericValue);
       }

       /// <summary>
       /// Sets the simple operator and value.
       /// </summary>
       /// <param name="rootObject">The root object.</param>
       /// <param name="complexComponentValue">The complex component value.</param>
       private static void SetTextOperatorAndValue(DataStructureComponentValueQueryType rootObject, IComplexComponentValue complexComponentValue)
       {
           string queryOperator = complexComponentValue.TextSearchOperator != null ? complexComponentValue.TextSearchOperator.Operator : DefaultEqualOperator;
           var queryTextType = new QueryTextType { @operator = queryOperator, TypedValue = complexComponentValue.Value };
           var textValue = new TextValue(queryTextType);
           rootObject.TextValue.Add(textValue);
       }

       /// <summary>
       /// Sets the time operator and value.
       /// </summary>
       /// <param name="rootObject">The root object.</param>
       /// <param name="complexComponentValue">The complex component value.</param>
       private static void SetTimeOperatorAndValue(DataStructureComponentValueQueryType rootObject, IComplexComponentValue complexComponentValue)
       {
           string queryOperator = complexComponentValue.OrderedOperator != null ? complexComponentValue.OrderedOperator.OrdOperator : DefaultEqualOperator;
           var timePeriodValueType = new TimePeriodValueType { @operator = queryOperator, TypedValue = complexComponentValue.Value };
           rootObject.TimeValue.Add(new TimeValue(timePeriodValueType));
       }

       /// <summary>
       /// Sets the simple operator and value.
       /// </summary>
       /// <param name="rootObject">The root object.</param>
       /// <param name="complexComponentValue">The complex component value.</param>
       private static void SetSimpleOperatorAndValue(DataStructureComponentValueQueryType rootObject, IComplexComponentValue complexComponentValue)
       {
           string queryOperator = complexComponentValue.OrderedOperator != null ? complexComponentValue.OrderedOperator.OrdOperator : DefaultEqualOperator;
           var simpleValueType = new SimpleValueType { @operator = queryOperator, TypedValue = complexComponentValue.Value };
           rootObject.Value = new Value(simpleValueType);
       }
    }
}

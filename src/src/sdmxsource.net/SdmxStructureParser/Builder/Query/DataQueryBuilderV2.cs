// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query builder for <c>SDMX</c> v2.0.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System;
    using System.Collections.Generic;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.query;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The data query builder for <c>SDMX</c> v2.0.
    /// </summary>
    public class DataQueryBuilderV2
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build data query from the specified <paramref name="queryType"/>
        /// </summary>
        /// <param name="queryType">
        /// The query type.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The data query from the specified <paramref name="queryType"/>
        /// </returns>
        public IList<IDataQuery> BuildDataQuery(
            QueryType queryType, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            IList<IDataQuery> returnList = new List<IDataQuery>();

            foreach (DataWhereType dataWhere in queryType.DataWhere)
            {
                // TODO java 0.9.9 no support for default limit. Opened : http://www.metadatatechnology.com/mantis/view.php?id=1427
                DataQueryProcessor dataQueryProcessor = null;

                if (queryType.defaultLimit.HasValue && queryType.defaultLimit.Value < int.MaxValue)
                {
                    int limit = decimal.ToInt32(queryType.defaultLimit.Value);
                    dataQueryProcessor = new DataQueryProcessor(limit);
                }
                else
                {
                    dataQueryProcessor = new DataQueryProcessor();
                }

                returnList.Add(dataQueryProcessor.BuildDataQuery(dataWhere, structureRetrievalManager));
            }

            return returnList;
        }

        /// <summary>
        /// Build data query from the specified <paramref name="dataWhereType"/>
        /// </summary>
        /// <param name="dataWhereType">
        /// The data where type.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <returns>
        /// The data query from the specified <paramref name="dataWhereType"/>
        /// </returns>
        public IDataQuery BuildDataQuery(
            DataWhereType dataWhereType, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            return new DataQueryProcessor().BuildDataQuery(dataWhereType, structureRetrievalManager);
        }

        /// <summary>
        /// Build data query from the specified <paramref name="dataWhereType"/>
        /// </summary>
        /// <param name="dataWhereType">
        /// The data where type.
        /// </param>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        /// <param name="defaultLimit"> The default limit</param>
        /// <returns>
        /// The data query from the specified <paramref name="dataWhereType"/>
        /// </returns>
        public IDataQuery BuildDataQuery(
            DataWhereType dataWhereType, ISdmxObjectRetrievalManager structureRetrievalManager, int defaultLimit)
        {
            return new DataQueryProcessor(defaultLimit).BuildDataQuery(dataWhereType, structureRetrievalManager);
        }

        #endregion

        /// <summary>
        ///     The data query processor.
        /// </summary>
        private class DataQueryProcessor
        {
            #region Fields

            /// <summary>
            ///     The data query selection groups.
            /// </summary>
            private readonly ISet<IDataQuerySelectionGroup> _dataQuerySelectionGroups;

            /// <summary>
            /// The default limit
            /// </summary>
            private readonly int _defaultLimit = -1;

            /// <summary>
            ///     The agency id.
            /// </summary>
            private string _agencyId;

            /// <summary>
            ///     The dataflow id.
            /// </summary>
            private string _dataflowId;

            /// <summary>
            ///     The key family id.
            /// </summary>
            private string _keyFamilyId;

            /// <summary>
            ///     The version.
            /// </summary>
            private string _version;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="DataQueryProcessor" /> class.
            /// </summary>
            public DataQueryProcessor()
            {
                this._dataQuerySelectionGroups = new HashSet<IDataQuerySelectionGroup>();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DataQueryProcessor"/> class.
            /// </summary>
            /// <param name="defaultLimit">
            /// The default Limit.
            /// </param>
            public DataQueryProcessor(int defaultLimit)
                : this()
            {
                this._defaultLimit = defaultLimit;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Builds a data query from <paramref name="dataWhereType"/>
            /// </summary>
            /// <param name="dataWhereType">
            /// The data Where Type.
            /// </param>
            /// <param name="structureRetrievalManager">
            /// The structure Retrieval Manager.
            /// </param>
            /// <returns>
            /// The <see cref="IDataQuery"/>.
            /// </returns>
            public IDataQuery BuildDataQuery(
                DataWhereType dataWhereType, ISdmxObjectRetrievalManager structureRetrievalManager)
            {
                if (structureRetrievalManager == null)
                {
                    throw new ArgumentNullException("structureRetrievalManager");
                }

                this.ProcessDataWhere(dataWhereType);
                this.ProcessAnd(dataWhereType.And);

                IMaintainableRefObject flowRef = new MaintainableRefObjectImpl(
                    this._agencyId, this._dataflowId, this._version);
                IDataflowObject dataflow = structureRetrievalManager.GetMaintainableObject<IDataflowObject>(flowRef);
                if (dataflow == null)
                {
                    throw new SdmxNoResultsException("Dataflow not found: " + flowRef);
                }

                IMaintainableRefObject dsdRef = dataflow.DataStructureRef.MaintainableReference;
                IDataStructureObject dataStructureBean = structureRetrievalManager.GetMaintainableObject<IDataStructureObject>(dsdRef);
                if (dataStructureBean == null)
                {
                    throw new SdmxNoResultsException("Data Structure not found: " + dsdRef);
                }

                // TODO check if DSD is v2.0 compatible
                if (!dataStructureBean.IsCompatible(SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
                {
                    throw new SdmxSemmanticException("DataStructure used for this dataflow is not SDMX v2.0 compatible.");
                }

                var convertedDataQuerySelectionGroups = ConvertConceptIdToComponentId(this._dataQuerySelectionGroups, dataStructureBean);

                // FUNC Data Provider
                return new DataQueryImpl(
                    dataStructureBean,
                    null,
                    null,
                    this._defaultLimit /* was null TODO */,
                    true,
                    null,
                    dataflow,
                    null,
                    convertedDataQuerySelectionGroups);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Converts the concept identifier to component identifier.
            /// </summary>
            /// <param name="dataQuerySelectionGroups">The data query selection groups.</param>
            /// <param name="dataStructureBean">The data structure bean.</param>
            /// <returns>The selections groups with concept i</returns>
            private static ISet<IDataQuerySelectionGroup> ConvertConceptIdToComponentId(IEnumerable<IDataQuerySelectionGroup> dataQuerySelectionGroups, IDataStructureObject dataStructureBean)
            {
                // build
                IDictionary<string, string> idDictionary = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (var component in dataStructureBean.Components)
                {
                    if (!string.Equals(component.Id, component.ConceptRef.ChildReference.Id))
                    {
                        idDictionary.Add(component.ConceptRef.ChildReference.Id, component.Id);
                    }
                }

                ISet<IDataQuerySelectionGroup> newDataQuerySelectionGroups = new HashSet<IDataQuerySelectionGroup>();

                foreach (var dataQuerySelectionGroup in dataQuerySelectionGroups)
                {
                    ISet<IDataQuerySelection> selections = new HashSet<IDataQuerySelection>();
                    foreach (var dataQuerySelection in dataQuerySelectionGroup.Selections)
                    {
                        string componentId;
                        if (idDictionary.TryGetValue(dataQuerySelection.ComponentId, out componentId))
                        {
                            IDataQuerySelection newSelection = dataQuerySelection.HasMultipleValues ? new DataQueryDimensionSelectionImpl(componentId, dataQuerySelection.Values) : new DataQueryDimensionSelectionImpl(componentId, dataQuerySelection.Value);
                            selections.Add(newSelection);
                        }
                        else
                        {
                            selections.Add(dataQuerySelection);
                        }
                    }

                    newDataQuerySelectionGroups.Add(new DataQuerySelectionGroupImpl(selections, dataQuerySelectionGroup.DateFrom, dataQuerySelectionGroup.DateTo));
                }

                return newDataQuerySelectionGroups;
            }


            /// <summary>
            /// Adds a selection value, either into an existing DataQuerySelection with the given concept, or a new DataQuerySelection if none exist with the given concept.
            /// </summary>
            /// <param name="selections">
            /// The selections
            /// </param>
            /// <param name="conceptId">
            /// The concept id
            /// </param>
            /// <param name="valueren">
            /// The value
            /// </param>
            private static void AddComponentSelection(
                ISet<IDataQuerySelection> selections, string conceptId, string valueren)
            {
                /* foreach */
                foreach (IDataQuerySelection selection in selections)
                {
                    if (selection.ComponentId.Equals(conceptId))
                    {
                        ((DataQueryDimensionSelectionImpl)selection).AddValue(valueren);
                        return;
                    }
                }

                IDataQuerySelection newSelection = new DataQueryDimensionSelectionImpl(conceptId, valueren);
                selections.Add(newSelection);
            }

            /// <summary>
            /// Add to <see cref="_dataQuerySelectionGroups"/> if selections exist.
            /// </summary>
            /// <param name="selections">
            /// The selections.
            /// </param>
            /// <param name="dateFrom">
            /// The date from.
            /// </param>
            /// <param name="dateTo">
            /// The date to.
            /// </param>
            private void AddGroupIfSelectionsExist(
                ISet<IDataQuerySelection> selections, ISdmxDate dateFrom, ISdmxDate dateTo)
            {
                if (ObjectUtil.ValidCollection(selections) || dateFrom != null || dateTo != null)
                {
                    this._dataQuerySelectionGroups.Add(new DataQuerySelectionGroupImpl(selections, dateFrom, dateTo));
                }
            }

            /// <summary>
            /// The process and.
            /// </summary>
            /// <param name="andType">
            /// The and type.
            /// </param>
            /// <exception cref="SdmxNotImplementedException">
            /// DataWhere.And followed by AND is not supported
            ///     -or-
            ///     Multiple Time selection on DataWhere.And not supported
            /// </exception>
            /// <exception cref="SdmxSemmanticException">
            /// Query Selection Illegal And Codes In Same Dimensions
            /// </exception>
            /// <exception cref="SdmxSemmanticException">
            /// Multiple Data Structure Ids not supported on DataWhere
            /// </exception>
            private void ProcessAnd(AndType andType)
            {
                ISet<IDataQuerySelection> selections = new HashSet<IDataQuerySelection>();
                ISdmxDate dateFrom = null;
                ISdmxDate dateTo = null;

                if (andType != null)
                {
                    if (ObjectUtil.ValidCollection(andType.And))
                    {
                        throw new SdmxNotImplementedException("DataWhere.And followed by AND is not supported");
                    }

                    if (andType.Dimension != null)
                    {
                        /* foreach */
                        foreach (DimensionType currentDimension in andType.Dimension)
                        {
                            /* foreach */
                            foreach (IDataQuerySelection selection in selections)
                            {
                                // CAN NOT AND TWO DIMENSIONS WITH DIFFERNT VALUES
                                if (selection.ComponentId.Equals(currentDimension.id))
                                {
                                    throw new SdmxSemmanticException(
                                        ExceptionCode.QuerySelectionIllegalAndCodesInSameDimension, currentDimension.id);
                                }
                            }

                            AddComponentSelection(selections, currentDimension.id, currentDimension.TypedValue);
                        }
                    }

                    if (andType.Attribute != null)
                    {
                        /* foreach */
                        foreach (AttributeType currentDimension in andType.Attribute)
                        {
                            /* foreach */
                            foreach (IDataQuerySelection selection in selections)
                            {
                                // CAN NOT AND TWO DIMENSIONS WITH DIFFERNT VALUES
                                if (selection.ComponentId.Equals(currentDimension.id))
                                {
                                    throw new SdmxSemmanticException(
                                        ExceptionCode.QuerySelectionIllegalAndCodesInSameDimension, currentDimension.id);
                                }
                            }

                            AddComponentSelection(selections, currentDimension.id, currentDimension.TypedValue);
                        }
                    }

                    if (andType.Time != null)
                    {
                        if (andType.Time.Count > 1)
                        {
                            throw new SdmxNotImplementedException("Multiple Time selection on DataWhere.And not supported");
                        }

                        /* foreach */
                        foreach (TimeType time in andType.Time)
                        {
                            var t = new Time(time);
                            dateFrom = t.DateFrom;
                            dateTo = t.DateTo;
                        }
                    }

                    if (ObjectUtil.ValidCollection(andType.AgencyID))
                    {
                        if (andType.AgencyID.Count > 1)
                        {
                            throw new SdmxSemmanticException(ExceptionCode.QuerySelectionIllegalAndAgencyId);
                        }

                        if (this._agencyId != null && !andType.AgencyID[0].Equals(this._agencyId))
                        {
                            throw new SdmxSemmanticException(
                                "Multiple agency Ids not supported on DataWhere - got '" + this._agencyId + "' and '"
                                + andType.AgencyID[0] + "'");
                        }

                        this._agencyId = andType.AgencyID[0];
                    }

                    if (ObjectUtil.ValidCollection(andType.KeyFamily))
                    {
                        if (andType.KeyFamily.Count > 1)
                        {
                            throw new SdmxSemmanticException(ExceptionCode.QuerySelectionIllegalAndKeyfamily);
                        }

                        if (this._keyFamilyId != null && !andType.KeyFamily[0].Equals(this._keyFamilyId))
                        {
                            throw new SdmxSemmanticException(
                                "Multiple Data Structure Ids not supported on DataWhere - got '" + this._keyFamilyId
                                + "' and '" + andType.KeyFamily[0] + "'");
                        }

                        this._keyFamilyId = andType.KeyFamily[0];
                    }

                    if (ObjectUtil.ValidCollection(andType.Dataflow))
                    {
                        if (andType.Dataflow.Count > 1)
                        {
                            throw new SdmxNotImplementedException("Multiple Dataflow Ids not supported in an AND operation");
                        }

                        if (this._dataflowId != null && !andType.Dataflow[0].Equals(this._dataflowId))
                        {
                            throw new SdmxSemmanticException(
                                "Multiple Dataflow Ids not supported on DataWhere - got '" + this._dataflowId
                                + "' and '" + andType.Dataflow[0] + "'");
                        }

                        this._dataflowId = andType.Dataflow[0];
                    }

                    this.ProcessOr(andType.Or, selections);
                    this.AddGroupIfSelectionsExist(selections, dateFrom, dateTo);
                }
            }

            /// <summary>
            /// The process and.
            /// </summary>
            /// <param name="andTypes">
            /// The and types.
            /// </param>
            private void ProcessAnd(IEnumerable<AndType> andTypes)
            {
                if (andTypes != null)
                {
                    foreach (AndType andType in andTypes)
                    {
                        this.ProcessAnd(andType);
                    }
                }
            }

            /// <summary>
            /// Process the <paramref name="dataWhereType"/>.
            /// </summary>
            /// <param name="dataWhereType">
            /// The data where type.
            /// </param>
            private void ProcessDataWhere(DataWhereType dataWhereType)
            {
                ISet<IDataQuerySelection> selections = new HashSet<IDataQuerySelection>();
                ISdmxDate dateFrom = null;
                ISdmxDate dateTo = null;

                this._dataflowId = dataWhereType.Dataflow;
                this._keyFamilyId = dataWhereType.KeyFamily;
                this._version = dataWhereType.Version;

                if (dataWhereType.Time != null)
                {
                    var t = new Time(dataWhereType.Time);
                    dateFrom = t.DateFrom;
                    dateTo = t.DateTo;
                }

                if (dataWhereType.Dimension != null)
                {
                    IDataQuerySelection newSelection = new DataQueryDimensionSelectionImpl(
                        dataWhereType.Dimension.id, dataWhereType.Dimension.TypedValue);

                    selections.Add(newSelection);
                }

                if (dataWhereType.Attribute != null)
                {
                    IDataQuerySelection newSelection0 = new DataQueryDimensionSelectionImpl(
                        dataWhereType.Attribute.id, dataWhereType.Attribute.TypedValue);

                    selections.Add(newSelection0);
                }

                this.ProcessOr(dataWhereType.Or, selections);
                this.AddGroupIfSelectionsExist(selections, dateFrom, dateTo);
            }

            /// <summary>
            /// The process or.
            /// </summary>
            /// <param name="orTypes">
            /// The or types.
            /// </param>
            /// <param name="selections">
            /// The selections.
            /// </param>
            private void ProcessOr(IEnumerable<OrType> orTypes, ISet<IDataQuerySelection> selections)
            {
                if (orTypes != null)
                {
                    foreach (OrType orType in orTypes)
                    {
                        this.ProcessOr(orType, selections);
                    }
                }
            }

            /// <summary>
            /// The process or.
            /// </summary>
            /// <param name="orType">
            /// The or type.
            /// </param>
            /// <param name="selections">
            /// The selections.
            /// </param>
            /// <exception cref="SdmxNotImplementedException">
            /// Key Family not supported in the IDataQuery.Or, please put in IDataQuery.And
            ///     -or-
            ///     Dataflow not supported in the IDataQuery.Or, please put in IDataQuery.And
            /// </exception>
            private void ProcessOr(OrType orType, ISet<IDataQuerySelection> selections)
            {
                if (orType != null)
                {
                    if (orType.Dimension != null)
                    {
                        foreach (DimensionType currentDimension in orType.Dimension)
                        {
                            AddComponentSelection(selections, currentDimension.id, currentDimension.TypedValue);
                        }
                    }

                    if (orType.Attribute != null)
                    {
                        foreach (AttributeType currentDimension in orType.Attribute)
                        {
                            AddComponentSelection(selections, currentDimension.id, currentDimension.TypedValue);
                        }
                    }

                    if (ObjectUtil.ValidCollection(orType.KeyFamily))
                    {
                        throw new SdmxNotImplementedException(
                            "Key Family not supported in the IDataQuery.Or, please put in IDataQuery.And");
                    }

                    if (ObjectUtil.ValidCollection(orType.Dataflow))
                    {
                        throw new SdmxNotImplementedException(
                            "Dataflow not supported in the IDataQuery.Or, please put in IDataQuery.And");
                    }

                    this.ProcessAnd(orType.And);
                }
            }

            #endregion
        }

        /// <summary>
        ///     The time.
        /// </summary>
        private class Time
        {
            #region Fields

            /// <summary>
            ///     The date from.
            /// </summary>
            internal readonly ISdmxDate DateFrom;

            /// <summary>
            ///     The date to.
            /// </summary>
            internal readonly ISdmxDate DateTo;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Time"/> class.
            /// </summary>
            /// <param name="timeType">
            /// The time type.
            /// </param>
            /// <exception cref="SdmxSemmanticException">
            /// Query Selection Multiple Date From/To
            /// </exception>
            public Time(TimeType timeType)
            {
                this.DateFrom = null;
                this.DateTo = null;
                if (timeType.Time != null)
                {
                    if (this.DateFrom != null)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMultipleDateFrom);
                    }

                    if (this.DateTo != null)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMultipleDateTo);
                    }

                    this.DateFrom = ParseDate(timeType.Time);
                    this.DateTo = ParseDate(timeType.Time);
                }

                if (timeType.StartTime != null)
                {
                    if (this.DateFrom != null)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMultipleDateFrom);
                    }

                    this.DateFrom = ParseDate(timeType.StartTime);
                }

                if (timeType.EndTime != null)
                {
                    if (this.DateTo != null)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMultipleDateTo);
                    }

                    this.DateTo = ParseDate(timeType.EndTime);
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Parse <paramref name="obj"/> <c>ToString()</c> method and return it as <see cref="ISdmxDate"/>
            /// </summary>
            /// <param name="obj">
            /// The input object.
            /// </param>
            /// <returns>
            /// The <see cref="ISdmxDate"/>.
            /// </returns>
            private static ISdmxDate ParseDate(object obj)
            {
                return new SdmxDateCore(obj.ToString());
            }

            #endregion
        }
    }
}
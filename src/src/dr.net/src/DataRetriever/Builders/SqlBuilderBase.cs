// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlBuilderBase.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sql builder base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Builders
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// The SQL builder base.
    /// </summary>
    internal class SqlBuilderBase
    {
        #region Constants and Fields

        /// <summary>
        ///   THe comma separator followed by space.
        /// </summary>
        private const string CommaSeparator = ", ";

        /// <summary>
        /// The _logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SqlBuilderBase));

        #endregion

        #region Methods

        /// <summary>
        /// Convert <paramref name="componentMappings"/> to a ICollection(MappingEntity) collection
        /// </summary>
        /// <param name="componentMappings">
        /// The component mappings. 
        /// </param>
        /// <returns>
        /// a ICollection(MappingEntity) collection 
        /// </returns>
        protected static ICollection<MappingEntity> ConvertToMapping(ICollection<IComponentMapping> componentMappings)
        {
            var mappingEntities = new List<MappingEntity>(componentMappings.Count);
            foreach (var componentMapping in componentMappings)
            {
                mappingEntities.Add(componentMapping.Mapping);
            }

            return mappingEntities;
        }

        /// <summary>
        /// This method generates the FROM part of the query.
        /// </summary>
        /// <param name="mappingSet">
        /// The <see cref="MappingSetEntity"/> that contains the mappings for the Dataflow of the query 
        /// </param>
        /// <returns>
        /// A string containing the FROM part 
        /// </returns>
        protected static string GenerateFrom(MappingSetEntity mappingSet)
        {
            _logger.Info(Resources.DataRetriever_GenerateFrom_Begin_GenerateFrom____);

            var from = new StringBuilder(" from ");
            from.AppendFormat(CultureInfo.InvariantCulture, "( {0} ) virtualDataset ", mappingSet.DataSet.Query);

            // log for easy debug
            _logger.Info(
                string.Format(
                    CultureInfo.InvariantCulture, Resources.DataRetriever_GenerateFrom_Generated_FROM___0_, from));
            _logger.Info(Resources.DataRetriever_GenerateFrom_End_GenerateFrom____);

            return from.ToString();
        }

        /// <summary>
        /// This method generates the ORDER BY part of the query for the specified <paramref name="orderComponents"/>
        /// </summary>
        /// <param name="info">
        /// The current Data retrieval state 
        /// </param>
        /// <param name="orderComponents">
        /// The order components. 
        /// </param>
        /// <returns>
        /// the ORDER BY part of the query for the specified <paramref name="orderComponents"/> 
        /// </returns>
        protected static string GenerateOrderBy(DataRetrievalInfo info, IEnumerable<ComponentEntity> orderComponents)
        {
            _logger.Info(Resources.InfoGenerateOrderBy);
            var orderBy = new StringBuilder("order by ");

            var orderColumns = new List<string>();

            var effectiveDimensionAtObs = info.EffectiveDimensionAtObservation;
            IEnumerable<ComponentEntity> componentEntities = orderComponents as ComponentEntity[] ?? orderComponents.ToArray();

            // TODO there might be an issue with dim at obs sharing a column with another obs. 
            // add each components contribution to the order by of the query
            foreach (ComponentEntity component in componentEntities)
            {
                MappingEntity mapping;
                if (info.ComponentMapping.TryGetValue(component, out mapping))
                {
                    foreach (DataSetColumnEntity column in mapping.Columns)
                    {
                        if (!orderColumns.Contains(column.Name))
                        {
                            orderColumns.Add(column.Name);
                        }
                    }
                }
            }

            bool hasLastObs = !info.HasFirstAndLastNObservations && info.HasLastNObservations;
            if (hasLastObs)
            {
                var dimAtObsComponent = componentEntities.First(entity => entity.Id.Equals(effectiveDimensionAtObs));
                var mappingEntity = info.ComponentMapping[dimAtObsComponent];
                for (int i = 0; i < orderColumns.Count; i++)
                {
                    var column = orderColumns[i];
                    if (mappingEntity.Columns.Any(entity => entity.Name.Equals(column)))
                    {
                        orderColumns[i] += " DESC";
                    }
                }
            }

            orderBy.Append(string.Join(CommaSeparator, orderColumns));

            // log for easy debug
            _logger.Info(
                string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedOrderByFormat1, orderBy));
            _logger.Info(Resources.InfoEndGenerateOrderBy);
            return orderBy.ToString();
        }

        /// <summary>
        /// This method generates the SELECT part of the query with the columns inside <paramref name="mappings"/>
        /// </summary>
        /// <param name="isDistinct">
        /// Whether the distinct keyword should be included 
        /// </param>
        /// <param name="mappings">
        /// The component mappings that contain the columns to be included at the select statement 
        /// </param>
        /// <returns>
        /// The SQL string.
        /// </returns>
        protected static string GenerateSelect(
            bool isDistinct, IEnumerable<MappingEntity> mappings)
        {
            string sql = string.Empty;
            StringBuilder sb = new StringBuilder(sql);

            _logger.Info(Resources.InfoBeginGenerateSelect);

            sb.Append("select ");
            sb.Append(isDistinct ? " distinct " : string.Empty);

            var selectArray = new List<string>();

            foreach (var componentMapping in mappings)
            {
                foreach (DataSetColumnEntity column in componentMapping.Columns)
                {
                    if (!selectArray.Contains(column.Name))
                    {
                        selectArray.Add(column.Name);
                    }
                }
            }

            sb.Append(string.Join(CommaSeparator, selectArray.ToArray()));

            sql = sb.ToString();

            // log for easy debug
            _logger.Info(string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedSelectFormat1, sql));
            _logger.Info(Resources.InfoEndGenerateSelect);

            return sql;
        }

        /// <summary>
        /// This method generates the WHERE part of the query
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// The string containing the required SQL part. For example, "where (FREQ='M')" 
        /// </returns>
        protected static string GenerateWhere(DataRetrievalInfo info)
        {
            IDataQuery query = info.Query;
            _logger.Info(Resources.InfoBeginGenerateWhere);

            var whereBuilder = new StringBuilder();

            if (query != null)
            {
                if (query.HasSelections())
                {
                    string dimId = string.Empty;
                    if (query.DataStructure.TimeDimension != null)
                    {
                        IList<IDimension> dimLst = query.DataStructure.DimensionList.Dimensions;
                        foreach (IDimension dim in dimLst)
                        {
                            if (dim.FrequencyDimension)
                            {
                                dimId = dim.Id;
                                break; // only one dimension has FrequencyDimension = true so no need to look further
                            }
                        }
                    }

                    IList<IDataQuerySelectionGroup> selGrps = query.SelectionGroups;
                    foreach (IDataQuerySelectionGroup sg in selGrps)
                    {
                        // AND has precedence over OR
                        if (whereBuilder.Length > 0)
                        {
                            whereBuilder.Append(" OR ");
                        }

                        IList<string> freqs = new List<string>();
                        if (sg.HasSelectionForConcept(dimId))
                        {
                            IDataQuerySelection selConcept = sg.GetSelectionsForConcept(dimId);
                            if (selConcept.HasMultipleValues)
                            {
                                foreach (string val in selConcept.Values)
                                {
                                    freqs.Add(val);
                                }
                            }
                            else
                            {
                                freqs.Add(selConcept.Value);
                            }
                        }
                        else
                        {
                            // HACK FIXME TODO
                            freqs.Add(null);
                        }

                        string sqlWhere = string.Empty;
                        foreach (string freqVal in freqs)
                        {
                            if (sqlWhere != string.Empty)
                            {
                                whereBuilder.Append(" AND ");
                            }

                            sqlWhere = GenerateWhereClause(sg, info, freqVal);
                            whereBuilder.Append(sqlWhere);
                        }

                        foreach (IDataQuerySelection sel in sg.Selections)
                        {
                            if (sqlWhere != string.Empty)
                            {
                                whereBuilder.Append(" AND ");
                            }

                            if (sel.HasMultipleValues)
                            {
                                int contor = 0;

                                whereBuilder.Append("(");
                                foreach (string val in sel.Values)
                                {
                                    if (contor > 0)
                                    {
                                        whereBuilder.Append(" OR ");
                                    }

                                    sqlWhere = GenerateComponentWhere(sel.ComponentId, val, info);
                                    whereBuilder.Append(sqlWhere);
                                    contor++;
                                }

                                whereBuilder.Append(")");
                            }
                            else
                            {
                                sqlWhere = GenerateComponentWhere(sel.ComponentId, sel.Value, info);
                                whereBuilder.Append(sqlWhere);
                            }
                        }
                    }
                }

                // MAT-274
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.Insert(0, "where ");
                }
            }

            // log for easy debug
            _logger.Info(string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedWhereFormat1, whereBuilder));
            _logger.Info(Resources.InfoEndGenerateWhere);

            return whereBuilder.ToString();
        }

        /// <summary>
        /// This method generates the WHERE part of the complex query
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// The string containing the required SQL part. For example, "where (FREQ='M')" 
        /// </returns>
        protected static string GenerateComplexWhere(DataRetrievalInfo info)
        {
            IComplexDataQuery query = info.ComplexQuery;
            _logger.Info(Resources.InfoBeginGenerateWhere);

            var whereBuilder = new StringBuilder();

            if (query != null)
            {
                if (query.HasSelections())
                {
                    string dimId = string.Empty;
                    if (query.DataStructure.TimeDimension != null)
                    {
                        IList<IDimension> dimLst = query.DataStructure.DimensionList.Dimensions;
                        foreach (IDimension dim in dimLst)
                        {
                            if (dim.FrequencyDimension)
                            {
                                dimId = dim.Id;
                                break; // only one dimension has FrequencyDimension = true so no need to look further
                            }
                        }
                    }

                    IList<IComplexDataQuerySelectionGroup> selGrps = query.SelectionGroups;
                    foreach (IComplexDataQuerySelectionGroup sg in selGrps)
                    {
                        // AND has precedence over OR
                        if (whereBuilder.Length > 0)
                        {
                            whereBuilder.Append(" OR ");
                        }

                        IList<string> freqs = new List<string>();
                        if (sg.HasSelectionForConcept(dimId))
                        {
                            IComplexDataQuerySelection selConcept = sg.GetSelectionsForConcept(dimId);
                            if (selConcept.HasMultipleValues())
                            {
                                foreach (IComplexComponentValue val in selConcept.Values)
                                {
                                    freqs.Add(val.Value);
                                }
                            }
                            else
                            {
                                freqs.Add(selConcept.Value.Value);
                            }
                        }
                        else
                        {
                            // HACK FIX ME TODO
                            freqs.Add(null);
                        }

                        string sqlWhere = string.Empty;
                        foreach (string freqVal in freqs)
                        {
                            if (sqlWhere != string.Empty)
                            {
                                whereBuilder.Append(" AND ");
                            }

                            sqlWhere = GenerateWhereClause(sg, info, freqVal);
                            whereBuilder.Append(sqlWhere);
                        }

                        if (sg.PrimaryMeasureValue != null && sg.PrimaryMeasureValue.Count > 0)
                        {
                            if (sqlWhere != string.Empty)
                            {
                                whereBuilder.Append(" AND ");
                            }

                            foreach (IComplexComponentValue complexValue in sg.PrimaryMeasureValue)
                            {
                                sqlWhere = GenerateComponentWhere(PrimaryMeasure.FixedId, complexValue, info);
                                whereBuilder.Append(sqlWhere);
                            }
                        }

                        foreach (IComplexDataQuerySelection sel in sg.Selections)
                        {
                            if (sqlWhere != string.Empty)
                            {
                                whereBuilder.Append(" AND ");
                            }

                            if (sel.HasMultipleValues())
                            {
                                int contor = 0;

                                whereBuilder.Append("(");
                                var componentValuesOperator = GetComponentValuesOperator(sel);

                                foreach (IComplexComponentValue val in sel.Values)
                                {
                                    if (contor > 0)
                                    {
                                        whereBuilder.Append(componentValuesOperator);
                                    }

                                    sqlWhere = GenerateComponentWhere(sel.ComponentId, val, info);
                                    whereBuilder.Append(sqlWhere);
                                    contor++;
                                }

                                whereBuilder.Append(")");
                            }
                            else
                            {
                                sqlWhere = GenerateComponentWhere(sel.ComponentId, sel.Value, info);
                                whereBuilder.Append(sqlWhere);
                            }
                        }
                    }
                }

                // MAT-274
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.Insert(0, "where ");
                }
            }

            // log for easy debug
            _logger.Info(string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedWhereFormat1, whereBuilder));
            _logger.Info(Resources.InfoEndGenerateWhere);

            return whereBuilder.ToString();
        }

        /// <summary>
        /// Gets the component values operator.
        /// </summary>
        /// <param name="sel">The values selection of a component.</param>
        /// <returns>The operator to use for the values in <paramref name="sel"/></returns>
        private static string GetComponentValuesOperator(IComplexDataQuerySelection sel)
        {
            string componentValuesOperator = " OR ";

            var orderedOperatorEnumTypes = sel.Values.Where(value => value.OrderedOperator != null).Select(value => value.OrderedOperator.EnumType).Distinct().ToArray();
            if (orderedOperatorEnumTypes.Length == 1)
            {
                switch (orderedOperatorEnumTypes[0])
                {
                    case OrderedOperatorEnumType.NotEqual:
                        componentValuesOperator = " AND ";
                        break;
                }
            }
            else if (orderedOperatorEnumTypes.Length == 0)
            {
                var textOperatorEnumTypes = sel.Values.Where(value => value.TextSearchOperator != null).Select(value => value.TextSearchOperator.EnumType).Distinct().ToArray();
                if (textOperatorEnumTypes.Length == 1)
                {
                    switch (textOperatorEnumTypes[0])
                    {
                        case TextSearchEnumType.NotEqual:
                            componentValuesOperator = " AND ";
                            break;
                    }
                }
            }

            return componentValuesOperator;
        }

        /// <summary>
        /// Maps a component to one or more local columns and it's value to one or more local codes
        /// </summary> 
        /// <param name="id">
        /// The DSD Component identifier e.g. FREQ 
        /// </param>
        /// <param name="conditionValue">
        /// The DSD Component condition value (from the SDMX Query) 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval status 
        /// </param>
        /// <returns>
        /// An string containing the SQL query where condition 
        /// </returns>
        private static string GenerateComponentWhere(string id, string conditionValue, DataRetrievalInfo info)
        {
            var ret = new StringBuilder();

            // MappingEntity mapping;
            // check if there is a mapping for this component
            if (id != null && conditionValue != null)
            {
                IComponentMapping componentMappingType;
                if (info.ComponentIdMap.TryGetValue(id, out componentMappingType))
                {
                    ret.Append(componentMappingType.GenerateComponentWhere(conditionValue));
                }
                else if (info.MeasureComponent == null || !id.Equals(info.MeasureComponent.Id))
                {
                    // component is not in the mapping
                    ret.Append(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            " ('{0} is not mapped' = '{1}') ",
                            id.Replace("'", "''"),
                            conditionValue.Replace("'", "''")));
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// Maps a component to one or more local columns and it's value to one or more local codes
        /// </summary>
        /// <param name="id">
        /// The DSD Component identifier e.g. FREQ 
        /// </param>
        /// <param name="conditionValue">
        /// The DSD Component condition value (from the SDMX Query) 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval status 
        /// </param>
        /// <returns>
        /// An string containing the SQL query where condition 
        /// </returns>
        private static string GenerateComponentWhere(string id, IComplexComponentValue conditionValue, DataRetrievalInfo info)
        {
            var ret = new StringBuilder();

            // MappingEntity mapping;
            // check if there is a mapping for this component
            if (id != null && conditionValue != null)
            {
                string operatorValue = "=";
                if (conditionValue.OrderedOperator != null)
                {
                    operatorValue = GetSqlOrderedOperator(conditionValue.OrderedOperator);
                }
                else if (conditionValue.TextSearchOperator != null)
                {
                    operatorValue = GetSqlTextSearchOperator(conditionValue.TextSearchOperator);
                }

                IComponentMapping componentMappingType;
                if (info.ComponentIdMap.TryGetValue(id, out componentMappingType))
                {
                    ret.Append(componentMappingType.GenerateComponentWhere(conditionValue.Value, operatorValue));
                }
                else if (info.MeasureComponent == null || !id.Equals(info.MeasureComponent.Id))
                {
                    // component is not in the mapping
                    ret.Append(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            " ('{0} is not mapped' " + operatorValue + " '{1}') ",
                            id.Replace("'", "''"),
                            conditionValue.Value.Replace("'", "''")));
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// Gets SQL operator from the given SDMX OrderedOperator
        /// </summary>
        /// <param name="ord">
        /// The OrderedOperator
        /// </param>
        /// <returns>
        /// An string containing the SQL query ordered operator 
        /// </returns>
        private static string GetSqlOrderedOperator(OrderedOperator ord)
        {
            switch (ord.EnumType)
            {
                case OrderedOperatorEnumType.Equal:
                    return "=";
                case OrderedOperatorEnumType.GreaterThan:
                    return ">";
                case OrderedOperatorEnumType.GreaterThanOrEqual:
                    return ">=";
                case OrderedOperatorEnumType.LessThan:
                    return "<";
                case OrderedOperatorEnumType.LessThanOrEqual:
                    return "<=";
                case OrderedOperatorEnumType.NotEqual:
                    return "!=";
            }

            return "=";
        }

        /// <summary>
        /// Gets SQL operator from the given SDMX OrderedOperator
        /// </summary>
        /// <param name="ord">
        /// The OrderedOperator
        /// </param>
        /// <returns>
        /// An string containing the SQL query ordered operator 
        /// </returns>
        private static string GetSqlTextSearchOperator(TextSearch ord)
        {
            switch (ord.EnumType)
            {
                case TextSearchEnumType.Contains:
                    return "LIKE %value%";
                case TextSearchEnumType.DoesNotContain:
                    return "NOT LIKE %value%";
                case TextSearchEnumType.DoesNotEndWith:
                    return "NOT LIKE %value";
                case TextSearchEnumType.DoesNotStartWith:
                    return "NOT LIKE value%";
                case TextSearchEnumType.EndsWith:
                    return "LIKE %value";
                case TextSearchEnumType.Equal:
                    return "=";
                case TextSearchEnumType.NotEqual:
                    return "!=";
                case TextSearchEnumType.StartsWith:
                    return "LIKE value%";
            }

            return "=";
        }

        /// <summary>
        /// Generates SQL where clauses from <paramref name="time"/>
        /// </summary>
        /// <param name="time">
        /// The <see cref="IDataQuerySelectionGroup"/> containing the time.
        /// </param>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <param name="freqValue">
        /// The frequency value 
        /// </param>
        /// <returns>
        /// The string containing the time part of the WHERE in an SQL query. 
        /// </returns>
        private static string GenerateWhereClause(IDataQuerySelectionGroup time, DataRetrievalInfo info, string freqValue)
        {
            return info.TimeTranscoder != null ? info.TimeTranscoder.GenerateWhere(time.DateFrom, time.DateTo, freqValue) : string.Empty;
        }

        /// <summary>
        /// Generates SQL where clauses from <paramref name="time"/>
        /// </summary>
        /// <param name="time">
        /// The <see cref="IComplexDataQuerySelectionGroup"/> 
        /// </param>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <param name="freqValue">
        /// The frequency value 
        /// </param>
        /// <returns>
        /// The string containing the time part of the WHERE in an SQL query. 
        /// </returns>
        private static string GenerateWhereClause(IComplexDataQuerySelectionGroup time, DataRetrievalInfo info, string freqValue)
        {
            return info.TimeTranscoder != null ? info.TimeTranscoder.GenerateWhere(time.DateFrom, time.DateTo, freqValue) : string.Empty;
        }

        #endregion
    }
}
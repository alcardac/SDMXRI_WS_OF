// -----------------------------------------------------------------------
// <copyright file="ComplexDataQuery2DataQueryBuilder.cs" company="Eurostat">
//   Date Created : 2014-01-22
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Builder
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;

    /// <summary>
    /// The complex data query 2 data query builder.
    /// </summary>
    public class ComplexDataQuery2DataQueryBuilder : IBuilder<IDataQuery, IComplexDataQuery>
    {
        /// <summary>
        /// Builds an <see cref="IDataQuery"/> from a <see cref="IComplexDataQuery"/>.
        /// Only the subset supported from <see cref="IDataQuery"/> is converted. The rest is ignored.
        /// The supported subset is operator "equal" and for time less or greater or equal
        /// </summary>
        /// <param name="buildFrom">An Object to build the output object from</param>
        /// <returns>
        /// A <see cref="IDataQuery"/>
        /// </returns>
        /// <exception cref="T:Org.Sdmxsource.Sdmx.Api.Exception.SdmxException">- If anything goes wrong during the build process</exception>
        public IDataQuery Build(IComplexDataQuery buildFrom)
        {
            ISet<IDataQuerySelectionGroup> selectionGroups = new HashSet<IDataQuerySelectionGroup>();
            foreach (var complexDataQuerySelectionGroup in buildFrom.SelectionGroups)
            {
                ISdmxDate dateFrom = null;
                ISdmxDate dateTo = null;
                if (complexDataQuerySelectionGroup.DateFromOperator == null || complexDataQuerySelectionGroup.DateFromOperator == OrderedOperatorEnumType.GreaterThanOrEqual)
                {
                    dateFrom = complexDataQuerySelectionGroup.DateFrom;
                }

                if (complexDataQuerySelectionGroup.DateToOperator == null || complexDataQuerySelectionGroup.DateToOperator == OrderedOperatorEnumType.LessThanOrEqual)
                {
                    dateTo = complexDataQuerySelectionGroup.DateTo;
                }

                ISet<IDataQuerySelection> selections = new HashSet<IDataQuerySelection>();
                foreach (var complexDataQuerySelection in complexDataQuerySelectionGroup.Selections)
                {
                    var values = new HashSet<string>();
                    foreach (var complexComponentValue in complexDataQuerySelection.Values)
                    {
                        if ((complexComponentValue.OrderedOperator == null || complexComponentValue.OrderedOperator == OrderedOperatorEnumType.Equal)
                            && (complexComponentValue.TextSearchOperator == null || complexComponentValue.TextSearchOperator == TextSearchEnumType.Equal))
                        {
                            values.Add(complexComponentValue.Value);
                        }
                    }

                    if (values.Count > 0)
                    {
                        selections.Add(new DataQueryDimensionSelectionImpl(complexDataQuerySelection.ComponentId, values));
                    }
                }

                selectionGroups.Add(new DataQuerySelectionGroupImpl(selections, dateFrom, dateTo));
            }

            return new DataQueryImpl(buildFrom.DataStructure, null, buildFrom.DataQueryDetail, buildFrom.FirstNObservations, buildFrom.LastNObservations, buildFrom.DataProvider, buildFrom.Dataflow, buildFrom.DimensionAtObservation, selectionGroups);
        }
    }
}
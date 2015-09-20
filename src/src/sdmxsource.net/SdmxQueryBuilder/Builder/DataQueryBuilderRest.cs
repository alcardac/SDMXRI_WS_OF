// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryBuilderRest.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query builder implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Builder
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Date;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public class DataQueryBuilderRest : IDataQueryBuilder<string>
    {

        #region Public Methods and Operators

        /// <summary>
        /// Get a string based DataQuery
        /// </summary>
        /// <param name="dataQuery">
        /// The data query
        /// </param>
        /// <returns></returns>
        public string BuildDataQuery(IDataQuery dataQuery)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            IDataStructureObject keyFamily = dataQuery.DataStructure;

            StringBuilder sb = new StringBuilder();

            sb.Append("data/");

            if (dataQuery.Dataflow != null)
            {
                IDataflowObject dataflow = dataQuery.Dataflow;
                sb.Append(dataflow.AgencyId);
                sb.Append(",");
                sb.Append(dataflow.Id);
                sb.Append(",");
                sb.Append(dataflow.Version);
                sb.Append("/");
            }
            else
            {
                throw new ArgumentNullException("dataQuery");
            }

            IDictionary<string, ISet<string>> selections = new Dictionary<string, ISet<string>>();

            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (dataQuery.HasSelections())
            {
                IDataQuerySelectionGroup dataQuerySelectionGroup = dataQuery.SelectionGroups[0];
                if (dataQuerySelectionGroup.DateFrom != null)
                {
                    dateFrom = dataQuerySelectionGroup.DateFrom.Date;
                }
                if (dataQuerySelectionGroup.DateTo != null)
                {
                    dateTo = dataQuerySelectionGroup.DateTo.Date;
                }
                foreach (IDataQuerySelection currentSelection in dataQuerySelectionGroup.Selections)
                {
                    selections.Add(currentSelection.ComponentId, currentSelection.Values);
                }
            }

            if (selections.Count == 0)
            {
                sb.Append("all");
            }
            else
            {
                string concatPeriod = "";

                foreach (IDimension dim in keyFamily.GetDimensions(SdmxStructureEnumType.Dimension))
                {
                    sb.Append(concatPeriod);
                    string conceptId = dim.Id;
                    if (selections.ContainsKey(conceptId))
                    {
                        string concatPlus = "";

                        foreach (string currentSelection in selections[conceptId])
                        {
                            sb.Append(concatPlus);
                            sb.Append(currentSelection);
                            concatPlus = "+";
                        }
                    }
                    concatPeriod = ".";
                }
            }

            string providerAgency = null;
            string providerId = null;

            if (dataQuery.DataProvider != null)
            {
                foreach (IDataProvider currentProvider in dataQuery.DataProvider)
                {
                    if (providerAgency != null && !providerAgency.Equals(currentProvider.MaintainableParent.AgencyId))
                    {
                        providerAgency = "*";
                    }
                    else
                    {
                        providerAgency = currentProvider.MaintainableParent.AgencyId;
                    }
                    if (providerId != null && !providerId.Equals(currentProvider.Id))
                    {
                        providerId = "ALL";
                    }
                    else
                    {
                        providerId = currentProvider.Id;
                    }
                }
            }

            if (!"all".Equals(providerId) && (providerId != null))
            {
                if (!"all".Equals(providerAgency) && (providerAgency != null))
                {
                    sb.Append("/" + providerAgency + "," + providerId + "/");
                }
                else
                {
                    sb.Append("/" + providerId + "/");
                }
            }
            else
            {
                sb.Append("/ALL/");
            }



            bool firstAppend = true;
            if (dataQuery.FirstNObservations != null && dataQuery.FirstNObservations != 0)
            {
                AppendParam(firstAppend, "firstNObservations", dataQuery.FirstNObservations, sb);
                firstAppend = false;
            }
            if (dataQuery.LastNObservations != null && dataQuery.LastNObservations != 0)
            {
                AppendParam(firstAppend, "lastNObservations", dataQuery.LastNObservations, sb);
                firstAppend = false;
            }
            if (dataQuery.DataQueryDetail != null)
            {
                AppendParam(firstAppend, "detail", dataQuery.DataQueryDetail.RestParam, sb);
                firstAppend = false;
            }
            if (dateFrom != null)
            {
                AppendParam(firstAppend, "startPeriod", DateUtil.FormatDate(dateFrom.Value, TimeFormatEnumType.Date), sb);
                firstAppend = false;
            }
            if (dateTo != null)
            {
                AppendParam(firstAppend, "endPeriod", DateUtil.FormatDate(dateTo.Value, TimeFormatEnumType.Date), sb);
                firstAppend = false;
            }
            if (dataQuery.DimensionAtObservation != null)
            {
                AppendParam(firstAppend, "dimensionAtObservation", dataQuery.DimensionAtObservation, sb);
                firstAppend = false;
            }
            return sb.ToString();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Append param and value to stringbuilder
        /// </summary>
        /// <param name="firstAppend">
        /// The first append
        /// </param>
        /// <param name="param">
        /// The param
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        /// <param name="sb">
        /// The stringbuilder
        /// </param>
        private void AppendParam(bool firstAppend, string param, object value, StringBuilder sb)
        {
            if (firstAppend)
            {
                sb.Append("?");
            }
            else
            {
                sb.Append("&");
            }
            sb.Append(param + "=" + value);
        }

        #endregion

    }
}

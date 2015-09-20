// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDocumentDataQueryBuilderV2.cs" company="Eurostat">
//   Date Created : 2013-04-02
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data query builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Builder
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.query;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    using QueryMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessageType;

    #endregion

    /// <summary>
    /// The data query builder.
    /// </summary>
    public class XDocumentDataQueryBuilderV2 : IDataQueryBuilder<XDocument>
    {
        #region Constants

        /// <summary>
        ///     The unknown id.
        /// </summary>
        private const string UnknownId = "UNKNOWN";

        #endregion


        #region Fields

        /// <summary>
        ///     The _header.
        /// </summary>
        private readonly IHeader _header;

        /// <summary>
        ///     The _header xml builder.
        /// </summary>
        private readonly IBuilder<HeaderType, IHeader> _headerXmlBuilder;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentDataQueryBuilderV2"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="headerXmlBuilder">
        /// The header Xml Builder.
        /// </param>
        public XDocumentDataQueryBuilderV2(IHeader header, IBuilder<HeaderType, IHeader> headerXmlBuilder)
        {
            this._headerXmlBuilder = headerXmlBuilder;
            this._header = header;
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Builds a DataQuery that matches the passed in format
        /// </summary>
        /// <param name="query">
        /// The query
        /// </param>
        /// <returns>
        /// The <see cref="XDocument"/> containing the <paramref name="query"/>.
        /// </returns>
        public XDocument BuildDataQuery(IDataQuery query)
        {
            var queryMessage = new QueryMessageType();
            queryMessage.Header = this._headerXmlBuilder.Build(this._header);

            var queryType = new QueryType();
            ProcessDataWhere(queryType, query);

            queryMessage.Query = queryType;
            return new XDocument(new QueryMessage(queryMessage).Untyped);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Builds the attribute multiple criteria.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="or">
        /// The or.
        /// </param>
        private static void BuildAttributeMultipleCriteria(string id, string value, OrType or)
        {
            or.Attribute.Add(new AttributeType { id = id, TypedValue = value });
        }

        /// <summary>
        /// Builds the attribute single criteria.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="and">
        /// The and.
        /// </param>
        private static void BuildAttributeSingleCriteria(string id, string value, AndType and)
        {
            and.Attribute.Add(new AttributeType { id = id, TypedValue = value });
        }

        /// <summary>
        /// Build Dimension/Attribute based criteria
        /// </summary>
        /// <param name="selections">
        /// The selections.
        /// </param>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <param name="sdmxStructureEnumType">
        /// The <see cref="SdmxStructureEnumType"/> of the Dimension or Attribute.
        /// </param>
        private static void BuildComponentCriteria(IDataQuerySelection selections, AndType root, SdmxStructureEnumType sdmxStructureEnumType)
        {
            Action<string, string, OrType> addCriteria = GetActionForMultipleValues(sdmxStructureEnumType);
            Action<string, string, AndType> addCriteriaSingle = GetActionForSingleValues(sdmxStructureEnumType);

            if (addCriteria == null || addCriteriaSingle == null)
            {
                return;
            }

            if (selections.HasMultipleValues)
            {
                var orType = new OrType();
                root.Or.Add(orType);
                foreach (var value in selections.Values)
                {
                    addCriteria(selections.ComponentId, value, orType);
                }
            }
            else
            {
                addCriteriaSingle(selections.ComponentId, selections.Value, root);
            }
        }

        /// <summary>
        /// Builds the dimension multiple criteria.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="or">
        /// The or.
        /// </param>
        private static void BuildDimensionMultipleCriteria(string id, string value, OrType or)
        {
            or.Dimension.Add(new DimensionType { id = id, TypedValue = value });
        }

        /// <summary>
        /// Builds the dimension single criteria.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="and">
        /// The and.
        /// </param>
        private static void BuildDimensionSingleCriteria(string id, string value, AndType and)
        {
            and.Dimension.Add(new DimensionType { id = id, TypedValue = value });
        }

        /// <summary>
        /// Build time criteria.
        /// </summary>
        /// <param name="dataQuerySelectionGroup">
        /// The data query selection group.
        /// </param>
        /// <param name="root">
        /// The root And
        /// </param>
        private static void BuildTime(IDataQuerySelectionGroup dataQuerySelectionGroup, AndType root)
        {
            var dateFrom = dataQuerySelectionGroup.DateFrom;
            var dateTo = dataQuerySelectionGroup.DateTo;
            if (dateFrom != null)
            {
                var timeType = new TimeType();
                root.Time.Add(timeType);

                // check if there is DateTo
                if (dateTo != null)
                {
                    if (string.Equals(dateFrom.DateInSdmxFormat, dateTo.DateInSdmxFormat))
                    {
                        //// if they are  equal use the Time.Time
                        timeType.Time = dateFrom.DateInSdmxFormat;
                    }
                    else
                    {
                        //// if they are not equal use the StartTime and EndTime
                        timeType.StartTime = dateFrom.DateInSdmxFormat;
                        timeType.EndTime = dateTo.DateInSdmxFormat;
                    }
                }
                else
                {
                    //// if only start time is specified use only that.
                    timeType.StartTime = dateFrom.DateInSdmxFormat;
                }
            }
        }

        /// <summary>
        /// Returns the action for multiple values.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Action<string, string, OrType> GetActionForMultipleValues(SdmxStructureEnumType structure)
        {
            switch (structure)
            {
                case SdmxStructureEnumType.Dimension:
                case SdmxStructureEnumType.MeasureDimension:
                    return BuildDimensionMultipleCriteria;
                case SdmxStructureEnumType.DataAttribute:
                    return BuildAttributeMultipleCriteria;
            }

            return null;
        }

        /// <summary>
        /// Returns the action for single values.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Action<string, string, AndType> GetActionForSingleValues(SdmxStructureEnumType structure)
        {
            switch (structure)
            {
                case SdmxStructureEnumType.Dimension:
                case SdmxStructureEnumType.MeasureDimension:
                    return BuildDimensionSingleCriteria;
                case SdmxStructureEnumType.DataAttribute:
                    return BuildAttributeSingleCriteria;
            }

            return null;
        }

        /// <summary>
        /// Process <see cref="DataWhereType"/>.
        /// </summary>
        /// <param name="queryType">
        /// The query type.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        private static void ProcessDataWhere(QueryType queryType, IDataQuery query)
        {
            queryType.DataWhere = new List<DataWhereType>();
            var dataWhereType = new DataWhereType();
            queryType.DataWhere.Add(dataWhereType);

            // add default limit. TODO change this when http://www.metadatatechnology.com/mantis/view.php?id=1506 is solved.
            if (query.FirstNObservations > 0)
            {
                queryType.defaultLimit = query.FirstNObservations;
            }

            var root = new AndType();
            dataWhereType.And = root;

            // set dataflow
            root.Dataflow.Add(query.Dataflow.Id);

            if (query.HasSelections())
            {
                // if we have multiple IDataQuerySelectionGroup then we need to include them in a <Or>
                OrType rootOr = null;
                if (query.SelectionGroups.Count > 1)
                {
                    rootOr = new OrType();
                    root.Or.Add(rootOr);
                }

                foreach (IDataQuerySelectionGroup dataQuerySelectionGroup in query.SelectionGroups)
                {
                    BuildTime(dataQuerySelectionGroup, root);

                    BuildSelections(query, dataQuerySelectionGroup, root);

                    // if we have multiple IDataQuerySelectionGroup then we need to include them in a <Or>
                    if (rootOr != null)
                    {
                        root = new AndType();
                        rootOr.And.Add(root);
                    }
                }
            }
        }

        /// <summary>
        /// Builds the selections.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="dataQuerySelectionGroup">
        /// The data query selection group.
        /// </param>
        /// <param name="root">
        /// The root.
        /// </param>
        private static void BuildSelections(IDataQuery query, IDataQuerySelectionGroup dataQuerySelectionGroup, AndType root)
        {
            foreach (IDataQuerySelection selections in dataQuerySelectionGroup.Selections)
            {
                var component = query.DataStructure.GetComponent(selections.ComponentId);
                if (component != null)
                {
                    BuildComponentCriteria(selections, root, component.StructureType.EnumType);
                }
            }
        }

        #endregion
    }
}

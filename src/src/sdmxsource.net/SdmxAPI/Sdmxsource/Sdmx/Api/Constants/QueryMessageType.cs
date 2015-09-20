// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryMessageType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Contains a list of Query message types.
    ///     <p />
    ///     This Enum offers the ability to retrieve the underlying <b>SWBASE_QUERY_MESSAGE_TYPE</b>
    /// </summary>
    /// <seealso cref="T:Org.Sdmxsource.Sdmx.Api.Constants.SWBASE_QUERY_MESSAGE_TYPE" />
    public enum QueryMessageEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     The data where.
        /// </summary>
        DataWhere,

        /// <summary>
        ///     The generic data query.
        /// </summary>
        GenericDataQuery,

        /// <summary>
        ///     The structure specific data query.
        /// </summary>
        StructureSpecificDataQuery,

        /// <summary>
        ///     The generic time series data query.
        /// </summary>
        GenericTimeseriesDataQuery,

        /// <summary>
        ///     The structure specific time series data query.
        /// </summary>
        StructureSpecificTimeseriesDataQuery,

        /// <summary>
        ///     The metadata where.
        /// </summary>
        MetadataWhere,

        /// <summary>
        ///     The generic metadata query.
        /// </summary>
        GenericMetadataQuery,

        /// <summary>
        ///     The structure specific metadata query.
        /// </summary>
        StructureSpecificMetadataQuery,

        /// <summary>
        ///     The dsd where.
        /// </summary>
        DsdWhere,

        /// <summary>
        ///     The mds where.
        /// </summary>
        MdsWhere,

        /// <summary>
        ///     The codelist where.
        /// </summary>
        CodelistWhere,

        /// <summary>
        ///     The concept where.
        /// </summary>
        ConceptWhere,

        /// <summary>
        ///     The agency where.
        /// </summary>
        AgencyWhere,

        /// <summary>
        ///     The data provider where.
        /// </summary>
        DataProviderWhere,

        /// <summary>
        ///     The hcl where.
        /// </summary>
        HclWhere,

        /// <summary>
        ///     The reporting taxonomy where.
        /// </summary>
        ReportingTaxonomyWhere,

        /// <summary>
        ///     The dataflow where.
        /// </summary>
        DataflowWhere,

        /// <summary>
        ///     The metadataflow where.
        /// </summary>
        MetadataflowWhere,

        /// <summary>
        ///     The structure set where.
        /// </summary>
        StructureSetWhere,

        /// <summary>
        ///     The process where.
        /// </summary>
        ProcessWhere,

        /// <summary>
        ///     The organisation scheme where.
        /// </summary>
        OrganisationSchemeWhere,

        /// <summary>
        ///     The concept scheme where.
        /// </summary>
        ConceptSchemeWhere,

        /// <summary>
        ///     The category scheme where.
        /// </summary>
        CategorySchemeWhere,

        /// <summary>
        ///     The categorisation where.
        /// </summary>
        CategorisationWhere,

        /// <summary>
        ///     The provision agreement where.
        /// </summary>
        ProvisionAgreementWhere,

        /// <summary>
        ///     The constraint where.
        /// </summary>
        ConstraintWhere,

        /// <summary>
        ///     The structures where.
        /// </summary>
        StructuresWhere
    }

    /// <summary>
    ///     The query message type.
    /// </summary>
    public class QueryMessageType : BaseConstantType<QueryMessageEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<QueryMessageEnumType, QueryMessageType> _instances =
            new Dictionary<QueryMessageEnumType, QueryMessageType>
                {
                    {
                        QueryMessageEnumType.DataWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.DataWhere, 
                        "DataWhere", 
                        BaseQueryMessageEnumType.Data)
                    }, 
                    {
                        QueryMessageEnumType.GenericDataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.GenericDataQuery, 
                        "GenericDataQuery", 
                        BaseQueryMessageEnumType.Data)
                    },
                    {
                        QueryMessageEnumType.StructureSpecificDataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.StructureSpecificDataQuery, 
                        "StructureSpecificDataQuery", 
                        BaseQueryMessageEnumType.Data)
                    },
                    {
                        QueryMessageEnumType.GenericTimeseriesDataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.GenericTimeseriesDataQuery, 
                        "GenericTimeSeriesDataQuery", 
                        BaseQueryMessageEnumType.Data)
                    },
                    {
                        QueryMessageEnumType.StructureSpecificTimeseriesDataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.StructureSpecificTimeseriesDataQuery, 
                        "StructureSpecificTimeSeriesDataQuery", 
                        BaseQueryMessageEnumType.Data)
                    },
                    {
                        QueryMessageEnumType.MetadataWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.MetadataWhere, 
                        "MetadataWhere", 
                        BaseQueryMessageEnumType.Metadata)
                    }, 
                    {
                        QueryMessageEnumType.GenericMetadataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.GenericMetadataQuery, 
                        "GenericMetadataQuery", 
                        BaseQueryMessageEnumType.Metadata)
                    }, 
                    {
                        QueryMessageEnumType.StructureSpecificMetadataQuery, 
                        new QueryMessageType(
                        QueryMessageEnumType.StructureSpecificMetadataQuery, 
                        "StructureSpecificMetadataQuery", 
                        BaseQueryMessageEnumType.Metadata)
                    }, 
                    {
                        QueryMessageEnumType.DsdWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.DsdWhere, 
                        "KeyFamilyWhere", 
                        BaseQueryMessageEnumType.Structure) // also DataStructureWhere mapped in parseString()
                    },
                    {
                        QueryMessageEnumType.MdsWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.MdsWhere, 
                        "MetadataStructureWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.CodelistWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.CodelistWhere, 
                        "CodelistWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ConceptWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ConceptWhere, 
                        "ConceptWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.AgencyWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.AgencyWhere, 
                        "AgencyWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.DataProviderWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.DataProviderWhere, 
                        "DataProviderWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.HclWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.HclWhere, 
                        "HierarchicalCodelistWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ReportingTaxonomyWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ReportingTaxonomyWhere, 
                        "ReportingTaxonomyWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.DataflowWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.DataflowWhere, 
                        "DataflowWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.MetadataflowWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.MetadataflowWhere, 
                        "MetadataflowWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.StructureSetWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.StructureSetWhere, 
                        "StructureSetWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ProcessWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ProcessWhere, 
                        "ProcessWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.OrganisationSchemeWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.OrganisationSchemeWhere, 
                        "OrganisationSchemeWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ConceptSchemeWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ConceptSchemeWhere, 
                        "ConceptSchemeWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.CategorySchemeWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.CategorySchemeWhere, 
                        "CategorySchemeWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.CategorisationWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.CategorisationWhere, 
                        "CategorisationWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ProvisionAgreementWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ProvisionAgreementWhere, 
                        "ProvisionAgreementWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.ConstraintWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.ConstraintWhere, 
                        "ConstraintWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }, 
                    {
                        QueryMessageEnumType.StructuresWhere, 
                        new QueryMessageType(
                        QueryMessageEnumType.StructuresWhere, 
                        "StructuresWhere", 
                        BaseQueryMessageEnumType.Structure)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _base query message type.
        /// </summary>
        private readonly BaseQueryMessageEnumType _baseQueryMessageType;

        /// <summary>
        ///     The _node name.
        /// </summary>
        private readonly string _nodeName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryMessageType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="nodeName">
        /// The node name.
        /// </param>
        /// <param name="baseQueryMessageType">
        /// The base query message type.
        /// </param>
        private QueryMessageType(
            QueryMessageEnumType enumType, string nodeName, BaseQueryMessageEnumType baseQueryMessageType)
            : base(enumType)
        {
            this._nodeName = nodeName;
            this._baseQueryMessageType = baseQueryMessageType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<QueryMessageType> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///     Gets the base query message type.
        /// </summary>
        public BaseQueryMessageType BaseQueryMessageType
        {
            get
            {
                return BaseQueryMessageType.GetFromEnum(this._baseQueryMessageType);
            }
        }

        /// <summary>
        ///     Gets the node name.
        /// </summary>
        public string NodeName
        {
            get
            {
                return this._nodeName;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="QueryMessageType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="QueryMessageType"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static QueryMessageType GetFromEnum(QueryMessageEnumType enumType)
        {
            QueryMessageType output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The parse string.
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="QueryMessageType"/> .
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Threows ArgumentException. 
        /// </exception>
        public static QueryMessageType ParseString(string messageType)
        {
            if (messageType == null)
            {
                throw new ArgumentException("QUERY_MESSAGE_TYPE.parseString can not parse null");
            }

            if ("DataStructureWhere".Equals(messageType, StringComparison.OrdinalIgnoreCase))
            { // this 2.1. In 2.0 it has KeyFamilyWhere
                return GetFromEnum(QueryMessageEnumType.DsdWhere);
            }

            foreach (QueryMessageType currentType in Values)
            {
                if (currentType.NodeName.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                {
                    return currentType;
                }
            }

            throw new ArgumentException("QUERY_MESSAGE_TYPE.parseString unknown message type : " + messageType);
        }

        #endregion
    }
}
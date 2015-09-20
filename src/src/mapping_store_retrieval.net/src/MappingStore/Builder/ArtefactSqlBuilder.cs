// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactSqlBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-09
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The artefact SQL builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    ///     The artefact SQL builder.
    /// </summary>
    internal class ArtefactSqlBuilder : ISqlQueryInfoBuilder<TableInfo>
    {
        #region Constants

        /// <summary>
        ///     The SQL query format to retrieve artefacts.
        /// </summary>
        private const string SqlQueryFormat =
            "SELECT T.{0} as SYSID, A.ID,  A.AGENCY, dbo.versionToString(A.VERSION1, A.VERSION2, A.VERSION3) as VERSION, A.VALID_FROM, A.VALID_TO, A.IS_FINAL, LN.TEXT, LN.LANGUAGE, LN.TYPE {2} FROM {1} T INNER JOIN ARTEFACT A ON T.{0} = A.ART_ID LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = A.ART_ID";

        /// <summary>
        ///     The SQL query format to retrieve the latest versions of artefact.
        /// </summary>
        private const string SqlLatestQueryFormat =
            SqlQueryFormat + " WHERE (SELECT COUNT(*) FROM ARTEFACT A2 INNER JOIN {1} T2 ON A2.ART_ID = T2.{0} where A2.ID=A.ID AND A2.AGENCY=A.AGENCY AND dbo.isGreaterVersion(A2.VERSION1, A2.VERSION2, A2.VERSION3, A.VERSION1, A.VERSION2, A.VERSION3)=1 {3} ) = 0 ";

        private const string OrderBy = " ORDER BY A.ART_ID ";
        #endregion

        #region Fields

        /// <summary>
        /// The _order by.
        /// </summary>
        private readonly string _orderBy;

        /// <summary>
        /// The latest Constraint. Additional SQL WHERE clause which is used only with <see cref="SqlLatestQueryFormat"/> . 
        /// Needs to start with the operator <c>AND</c>.
        /// </summary>
        private readonly string _latestConstraint;

        /// <summary>
        /// The query format.
        /// </summary>
        private readonly string _queryFormat = SqlQueryFormat;

        /// <summary>
        /// The where state depends whether <see cref="SqlQueryFormat"/> or <see cref="SqlLatestQueryFormat"/> is used.
        /// </summary>
        private readonly WhereState _whereStatus;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactSqlBuilder"/> class.
        /// </summary>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="versionQueryType">
        /// The version Query Type.
        /// </param>
        /// <param name="latestConstraint">
        /// The latest Constraint. Additional SQL WHERE clause which is used only when <paramref name="versionQueryType"/> equals <see cref="VersionQueryType.Latest"/>. 
        /// Needs to start with the operator <c>AND</c>.
        /// </param>
        public ArtefactSqlBuilder(string orderBy = null, VersionQueryType versionQueryType = VersionQueryType.All, string latestConstraint = null)
        {
            this._orderBy = orderBy ?? OrderBy;
            this._latestConstraint = latestConstraint;
            switch (versionQueryType)
            {
                case VersionQueryType.All:
                    this._queryFormat = SqlQueryFormat;
                    this._whereStatus = WhereState.Nothing;
                    break;
                case VersionQueryType.Latest:
                    this._queryFormat = SqlLatestQueryFormat;
                    this._whereStatus = WhereState.And;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("versionQueryType");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="SqlQueryInfo"/> from the specified <paramref name="tableInfo"/>
        /// </summary>
        /// <param name="tableInfo">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="SqlQueryInfo"/> build from the specified <paramref name="tableInfo"/>
        /// </returns>
        public SqlQueryInfo Build(TableInfo tableInfo)
        {
            string queryFormat = string.Format(CultureInfo.InvariantCulture, this._queryFormat, tableInfo.PrimaryKey, tableInfo.Table, tableInfo.ExtraFields, this._latestConstraint);
            var queryInfo = new SqlQueryInfo { QueryFormat = queryFormat, OrderBy = this._orderBy, WhereStatus = this._whereStatus };
            return queryInfo;
        }

        #endregion
    }
}
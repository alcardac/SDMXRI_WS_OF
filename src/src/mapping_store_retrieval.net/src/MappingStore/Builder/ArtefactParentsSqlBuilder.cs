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
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    ///     The artefact SQL builder for finding parents of an artefact.
    /// </summary>
    internal class ArtefactParentsSqlBuilder : ISqlQueryInfoBuilder<string>
    {
        #region Constants

        /// <summary>
        ///     The SQL query format to retrieve artefacts.
        /// </summary>
        private const string SqlQueryFormat =
            "SELECT distinct P.ART_ID as SYSID, P.ID, P.AGENCY, dbo.versionToString(P.VERSION1, P.VERSION2, P.VERSION3) AS VERSION, P.VALID_FROM, P.VALID_TO, P.IS_FINAL, LN.TEXT, LN.LANGUAGE, LN.TYPE {1} FROM ARTEFACT P LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = P.ART_ID {0} ";

        /// <summary>
        /// The SQL Order By clause
        /// </summary>
        private const string OrderBy = " ORDER BY P.ART_ID ";

        #endregion
        /// <summary>
        /// The table info
        /// </summary>
        private readonly TableInfo _tableInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactParentsSqlBuilder"/> class.
        /// </summary>
        /// <param name="tableInfo">The table info.</param>
        public ArtefactParentsSqlBuilder(TableInfo tableInfo)
        {
            this._tableInfo = tableInfo;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="SqlQueryInfo"/> from the specified <paramref name="innerJoins"/>
        /// </summary>
        /// <param name="innerJoins">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="SqlQueryInfo"/> build from the specified <paramref name="innerJoins"/>
        /// </returns>
        public SqlQueryInfo Build(string innerJoins)
        {
            string queryFormat = string.Format(CultureInfo.InvariantCulture, SqlQueryFormat, innerJoins, this._tableInfo.ExtraFields);
            var queryInfo = new SqlQueryInfo { QueryFormat = queryFormat, OrderBy = OrderBy, WhereStatus = WhereState.Nothing };
            return queryInfo;
        }

        #endregion
    }
}
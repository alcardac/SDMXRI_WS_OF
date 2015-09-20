// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferencedSqlQueryBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The item SQL query builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    /// The item SQL query builder.
    /// </summary>
    internal class ReferencedSqlQueryBuilder : ISqlQueryInfoBuilder<string>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        ///     The _order by.
        /// </summary>
        private readonly string _orderBy;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedSqlQueryBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="orderBy">
        /// The order by
        /// </param>
        public ReferencedSqlQueryBuilder(Database mappingStoreDb, string orderBy)
        {
            this._mappingStoreDb = mappingStoreDb;
            this._orderBy = orderBy;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="SqlQueryInfo"/> from the specified <paramref name="queryFormat"/>
        /// </summary>
        /// <param name="queryFormat">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="SqlQueryInfo"/> build from the specified <paramref name="queryFormat"/>
        /// </returns>
        public SqlQueryInfo Build(string queryFormat)
        {
            string paramId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            string query = string.Format(CultureInfo.InvariantCulture, queryFormat, paramId);
            return new SqlQueryInfo { QueryFormat = query, OrderBy = this._orderBy };
        }

        #endregion
    }
}
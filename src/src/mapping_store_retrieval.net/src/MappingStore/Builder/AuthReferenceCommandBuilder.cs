// -----------------------------------------------------------------------
// <copyright file="AuthReferenceCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-07-01
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The authorization reference command builder.
    /// </summary>
    internal class AuthReferenceCommandBuilder : IAuthCommandBuilder<ReferenceSqlQuery>
    {
        /// <summary>
        /// The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        /// The  dataflow filter.
        /// </summary>
        private readonly DataflowFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthReferenceCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The dataflow filter.
        /// </param>
        public AuthReferenceCommandBuilder(Database mappingStoreDb, DataflowFilter filter)
        {
            this._mappingStoreDb = mappingStoreDb;
            this._filter = filter;
        }

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="SqlQueryBase"/> based class to build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand Build(ReferenceSqlQuery buildFrom, IList<IMaintainableRefObject> allowedDataflows)
        {
            IList<DbParameter> parameters = new List<DbParameter>();
            var sqlCommand = new StringBuilder();
            var whereState = WhereState.Nothing;
            if (this._filter == DataflowFilter.Production)
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, CategorisationConstant.ProductionWhereClause);
                whereState = WhereState.And;
            }

            SecurityHelper.AddWhereClauses(null, this._mappingStoreDb, sqlCommand, parameters, allowedDataflows, whereState);

            var sqlQuery = string.Format(CultureInfo.InvariantCulture, buildFrom.QueryInfo.QueryFormat, sqlCommand);
            return this._mappingStoreDb.GetSqlStringCommand(sqlQuery, parameters);
        }
    }
}
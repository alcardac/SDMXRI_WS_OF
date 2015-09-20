// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthArtefactCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The auth artefact command builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The authorization aware artefact command builder.
    /// </summary>
    internal abstract class AuthArtefactCommandBuilder : ArtefactCommandBuilder, IAuthCommandBuilder<ArtefactSqlQuery>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthArtefactCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        protected AuthArtefactCommandBuilder(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand Build(ArtefactSqlQuery buildFrom, IList<IMaintainableRefObject> allowedDataflows)
        {
            string sqlQuery = this.GetSqlQuery(buildFrom);
            var sqlCommand = new StringBuilder(sqlQuery);
            IList<DbParameter> parameters = this.CreateArtefactWhereClause(buildFrom.MaintainableRef, sqlCommand, buildFrom.QueryInfo.WhereStatus, allowedDataflows);

            if (!string.IsNullOrWhiteSpace(buildFrom.QueryInfo.OrderBy))
            {
                sqlCommand.Append(buildFrom.QueryInfo.OrderBy);
            }

            return this.MappingStoreDB.GetSqlStringCommand(sqlCommand.ToString(), parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The SQL query.
        /// </returns>
        protected virtual string GetSqlQuery(ArtefactSqlQuery query)
        {
            return query.QueryInfo.QueryFormat;
        }

        /// <summary>
        /// Create the WHERE clause from the <paramref name="maintainableRef"/>  and write it to <paramref name="sqlCommand"/>
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable Ref.
        /// </param>
        /// <param name="sqlCommand">
        /// The output string buffer
        /// </param>
        /// <param name="whereState">
        /// the current state of the WHERE clause in <paramref name="sqlCommand"/>
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The list of <see cref="DbParameter"/>
        /// </returns>
        protected abstract IList<DbParameter> CreateArtefactWhereClause(
            IMaintainableRefObject maintainableRef, StringBuilder sqlCommand, WhereState whereState, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}
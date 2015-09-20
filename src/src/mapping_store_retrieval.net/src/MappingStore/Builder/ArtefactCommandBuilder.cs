// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-08
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The artefact command builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The artefact command builder.
    /// </summary>
    internal class ArtefactCommandBuilder : ICommandBuilder<ArtefactSqlQuery>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public ArtefactCommandBuilder(Database mappingStoreDb)
        {
            if (mappingStoreDb == null)
            {
                throw new ArgumentNullException("mappingStoreDb");
            }

            this._mappingStoreDb = mappingStoreDb;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the mapping store DB.
        /// </summary>
        protected Database MappingStoreDB
        {
            get
            {
                return this._mappingStoreDb;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="DbCommand"/> from <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand Build(ArtefactSqlQuery buildFrom)
        {
            var sqlCommand = new StringBuilder(buildFrom.QueryInfo.QueryFormat);
            IList<DbParameter> parameters = this.CreateArtefactWhereClause(buildFrom.MaintainableRef, sqlCommand, buildFrom.QueryInfo.WhereStatus);

            if (!string.IsNullOrWhiteSpace(buildFrom.QueryInfo.OrderBy))
            {
                sqlCommand.Append(buildFrom.QueryInfo.OrderBy);
            }

            return this._mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString(), parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create the WHERE clause from the <paramref name="maintainableRef"/>  and write it to <paramref name="sqlCommand"/>
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable Ref.
        /// </param>
        /// <param name="sqlCommand">
        ///     The output string buffer
        /// </param>
        /// <param name="whereState">the current state of the WHERE clause in <paramref name="sqlCommand"/></param>
        /// <returns>
        /// The list of <see cref="DbParameter"/>
        /// </returns>
        protected virtual IList<DbParameter> CreateArtefactWhereClause(IMaintainableRefObject maintainableRef, StringBuilder sqlCommand, WhereState whereState)
        {
            var parameters = new List<DbParameter>();
            if (maintainableRef == null)
            {
                return parameters;
            }

            if (maintainableRef.HasMaintainableId())
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, " A.ID = {0} ", this._mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter));
                whereState = WhereState.And;
                parameters.Add(this._mappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.AnsiString, maintainableRef.MaintainableId));
            }

            if (maintainableRef.HasVersion())
            {
                var versionParameters = maintainableRef.GenerateVersionParameters(this._mappingStoreDb, parameters, "A.VERSION", versionNumber => string.Format("{0}{1}", ParameterNameConstants.VersionParameter, versionNumber));
                SqlHelper.AddWhereClause(sqlCommand, whereState, versionParameters);
            }

            if (maintainableRef.HasAgencyId())
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, " A.AGENCY = {0} ", this._mappingStoreDb.BuildParameterName(ParameterNameConstants.AgencyParameter));
                parameters.Add(this._mappingStoreDb.CreateInParameter(ParameterNameConstants.AgencyParameter, DbType.AnsiString, maintainableRef.AgencyId));
            }

            return parameters;
        }

        #endregion
    }
}
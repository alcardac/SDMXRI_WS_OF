// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dataflow command builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The dataflow command builder.
    /// </summary>
    internal class DataflowCommandBuilder : AuthArtefactCommandBuilder
    {
        #region Fields

        /// <summary>
        /// The filter.
        /// </summary>
        private readonly DataflowFilter _filter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The filter. (Optional defaults to <see cref="DataflowFilter.Production"/>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public DataflowCommandBuilder(Database mappingStoreDb,  DataflowFilter filter = DataflowFilter.Production)
            : base(mappingStoreDb)
        {
            this._filter = filter;
        }

        #endregion

        #region Methods

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
        protected override IList<DbParameter> CreateArtefactWhereClause(IMaintainableRefObject maintainableRef, StringBuilder sqlCommand, WhereState whereState, IList<IMaintainableRefObject> allowedDataflows)
        {
            IList<DbParameter> parameters = this.CreateArtefactWhereClause(maintainableRef, sqlCommand, whereState);
            if (parameters.Count > 0)
            {
                whereState = WhereState.And;
            }

            if (this._filter == DataflowFilter.Production)
            {
                SqlHelper.AddWhereClause(sqlCommand, whereState, DataflowConstant.ProductionWhereClause);
                whereState = WhereState.And;
            }

            return SecurityHelper.AddWhereClauses(maintainableRef, this.MappingStoreDB, sqlCommand, parameters, allowedDataflows, whereState);
        }

        #endregion
    }
}
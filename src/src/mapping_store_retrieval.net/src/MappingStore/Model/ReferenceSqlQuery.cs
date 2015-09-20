// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceSqlQuery.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The item SQL query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    /// <summary>
    ///     The reference SQL query.
    /// </summary>
    internal class ReferenceSqlQuery : SqlQueryBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceSqlQuery"/> class.
        /// </summary>
        /// <param name="queryInfo">
        ///     The query Info.
        /// </param>
        public ReferenceSqlQuery(SqlQueryInfo queryInfo)
            : base(queryInfo)
        {
        }

        #endregion
    }
}
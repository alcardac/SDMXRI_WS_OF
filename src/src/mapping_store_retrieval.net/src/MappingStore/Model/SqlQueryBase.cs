// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlQueryBase.cs" company="Eurostat">
//   Date Created : 2013-02-08
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SQL query base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    /// <summary>
    ///     The SQL query base.
    /// </summary>
    internal abstract class SqlQueryBase
    {
        #region Fields

        /// <summary>
        ///     The SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _queryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryBase"/> class.
        /// </summary>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        protected SqlQueryBase(SqlQueryInfo queryInfo)
        {
            this._queryInfo = queryInfo;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the SQL query info.
        /// </summary>
        public SqlQueryInfo QueryInfo
        {
            get
            {
                return this._queryInfo;
            }
        }

        #endregion
    }
}
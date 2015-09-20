// -----------------------------------------------------------------------
// <copyright file="SqlHelper.cs" company="Eurostat">
//   Date Created : 2013-03-01
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// The SQL helper class.
    /// </summary>
    internal static class SqlHelper
    {
        /// <summary>
        /// Add where clause to <paramref name="sqlCommand"/>.
        /// </summary>
        /// <param name="sqlCommand">
        /// The SQL command buffer.
        /// </param>
        /// <param name="whereState">
        /// The WHERE clause state in <paramref name="sqlCommand"/>
        /// </param>
        /// <param name="format">
        /// The WHERE clause format.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void AddWhereClause(StringBuilder sqlCommand, WhereState whereState, string format, params object[] parameters)
        {
            switch (whereState)
            {
                case WhereState.Nothing:
                    sqlCommand.Append(" WHERE ");
                    break;
                case WhereState.Where:
                    break;
                case WhereState.And:
                    sqlCommand.Append(" AND ");
                    break;
            }

            sqlCommand.AppendFormat(CultureInfo.InvariantCulture, format, parameters);
        }
    }
}
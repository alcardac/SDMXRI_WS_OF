// -----------------------------------------------------------------------
// <copyright file="SqlQueryInfo.cs" company="Eurostat">
//   Date Created : 2013-02-08
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// This class holds information about a SQL Query
    /// </summary>
    internal class SqlQueryInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryInfo"/> class. 
        /// </summary>
        public SqlQueryInfo()
        {
            this.WhereStatus = WhereState.And;
        }

        /// <summary>
        /// Gets or sets the SQL query format.
        /// </summary>
        public string QueryFormat { get; set; }

        /// <summary>
        /// Gets or sets the order by string if any. (Optional).
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it needs a WHERE clause.
        /// </summary>
        public WhereState WhereStatus { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="SqlQueryInfo"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="SqlQueryInfo"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.QueryFormat, this.OrderBy);
        }
    }
}
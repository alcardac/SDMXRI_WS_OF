// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountSqlBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Build SQL query for Count requests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System.Globalization;

    using Estat.Nsi.StructureRetriever.Model;

    /// <summary>
    /// Build SQL query for Count requests
    /// </summary>
    internal class CountSqlBuilder : SqlBuilderBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generate the SQL for executing on the DDB
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval information 
        /// </param>
        /// <returns>
        /// The generated sql. 
        /// </returns>
        public override string GenerateSql(StructureRetrievalInfo info)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SELECT COUNT(*) \n FROM ({0}) virtualDataset {1}",
                info.InnerSqlQuery,
                GenerateWhere(info));
        }

        #endregion
    }
}
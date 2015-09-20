// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialCodesSqlQuery.cs" company="Eurostat">
//   Date Created : 2013-04-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The partial codes sql query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The partial codes SQL query.
    /// </summary>
    internal class PartialCodesSqlQuery : ItemSqlQuery
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialCodesSqlQuery"/> class.
        /// </summary>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        /// <param name="parentSysId">
        /// The parent ItemScheme parent Id.
        /// </param>
        public PartialCodesSqlQuery(SqlQueryInfo queryInfo, long parentSysId)
            : base(queryInfo, parentSysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the concept id.
        /// </summary>
        public string ConceptId { get; set; }

        /// <summary>
        /// Gets or sets the dataflow reference.
        /// </summary>
        public IMaintainableRefObject DataflowReference { get; set; }

        #endregion
    }
}
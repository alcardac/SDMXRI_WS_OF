// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemCommandBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The item command builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    /// <summary>
    /// The item command builder.
    /// </summary>
    internal class ItemCommandBuilder : ICommandBuilder<ItemSqlQuery>
    {
        #region Fields

        /// <summary>
        ///     The mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCommandBuilder"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        public ItemCommandBuilder(Database mappingStoreDb)
        {
            this._mappingStoreDb = mappingStoreDb;
        }

        /// <summary>
        ///     Gets the mapping store DB.
        /// </summary>
        protected Database MappingStoreDb
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
        public virtual DbCommand Build(ItemSqlQuery buildFrom)
        {
            var inParameter = this.MappingStoreDb.CreateInParameter(ParameterNameConstants.IdParameter, DbType.Int64, buildFrom.ParentSysId);
            return this.MappingStoreDb.GetSqlStringCommandParam(buildFrom.QueryInfo.ToString(), inParameter);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DDbConnectionBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Dissemination database connection builder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System.Configuration;
    using System.Data.Common;

    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    /// <summary>
    /// Dissemination database connection builder
    /// </summary>
    internal class DDbConnectionBuilder : IBuilder<DbConnection, StructureRetrievalInfo>
    {
        #region Constants and Fields

        /// <summary>
        ///   The sigleton instance
        /// </summary>
        private static readonly DDbConnectionBuilder _instance = new DDbConnectionBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DDbConnectionBuilder"/> class from being created. 
        ///   Prevents a default instance of the <see cref="DDbConnectionBuilder"/> class from being created.
        /// </summary>
        private DDbConnectionBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance of this class
        /// </summary>
        public static DDbConnectionBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The method that builds a <see cref="DbConnection"/> from the specified <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The current Data retrieval state 
        /// </param>
        /// <returns>
        /// The <see cref="DbConnection"/> 
        /// </returns>
        public DbConnection Build(StructureRetrievalInfo info)
        {
            string dissdbConnectionString = info.MappingSet.DataSet.Connection.AdoConnectionString;
            string providerName = DatabaseType.GetProviderName(info.MappingSet.DataSet.Connection.DBType);
            var disseminationDb = new Database(new ConnectionStringSettings(info.MappingSet.DataSet.Connection.Name, dissdbConnectionString, providerName));
            DbConnection dbConnection = disseminationDb.CreateConnection();
            dbConnection.Open();
            return dbConnection;
        }

        #endregion
    }
}
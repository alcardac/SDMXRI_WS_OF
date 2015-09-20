// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabularSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Sql builder which includes all components and orders by dimensions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Builders
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;

    using log4net;
    using IstatExtension.Retriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using System.Configuration;
    using Org.Sdmxsource.Sdmx.Api.Constants;


    /// <summary>
    /// Sql builder which includes all components and orders by dimensions
    /// </summary>
    public class TabularSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly TabularSqlBuilder _instance = new TabularSqlBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(TabularSqlBuilder));

        /// <summary>
        /// The ordered component builder
        /// </summary>
        private static readonly SeriesOrderedDimensionBuilder _orderedComponentBuilder = new SeriesOrderedDimensionBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="TabularSqlBuilder" /> class from being created.
        /// </summary>
        private TabularSqlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static TabularSqlBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <param name="info">
        /// The current state of the data retrieval which containts the current query and mapping set 
        /// </param>
        public void GenerateSql(DataRetrievalInfo info)
        {
            Logger.Info(Resources.InfoBeginGenerateSql);

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                sql = GenerateSelect(false, info.ComponentMapping.Values);
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(info.MappingSet));

                if (info.ComplexQuery != null)
                {
                    sqlQuery.appendSql(GenerateComplexWhere(info));
                }
                else
                {
                    sqlQuery.appendSql(GenerateWhere(info));
                }

                var orderComponents = _orderedComponentBuilder.Build(info);
                sqlQuery.appendSql(GenerateOrderBy(info, orderComponents));
            }
            catch (DataRetrieverException dex)
            {
                throw new DataRetrieverException(dex, dex.SdmxErrorCode, dex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError),
                    Resources.ErrorUnableToGenerateSQL);
            }

            // log for easy debug
            Logger.Info(string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedSQLFormat1, sql));
            Logger.Info(Resources.InfoEndGenerateSql);

            info.SqlString = sqlQuery.getSql();
        }

        #endregion
    }
}
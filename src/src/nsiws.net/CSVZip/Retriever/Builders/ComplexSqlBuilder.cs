// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Sql builder which includes all components and orders by dimensions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using log4net;
using CSVZip.Retriever.Model;
using System.Globalization;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Estat.Nsi.DataRetriever.Properties;



namespace CSVZip.Retriever.Builders
{
    /// <summary>
    /// Sql builder which includes all components and orders by dimensions
    /// </summary>
    internal class ComplexSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly ComplexSqlBuilder _instance = new ComplexSqlBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ComplexSqlBuilder));

        /// <summary>
        /// The ordered component builder
        /// </summary>
        private static readonly SeriesOrderedDimensionBuilder _orderedComponentBuilder = new SeriesOrderedDimensionBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="ComplexSqlBuilder" /> class from being created.
        /// </summary>
        private ComplexSqlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static ComplexSqlBuilder Instance
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

            var seriesInfo = info as DataRetrievalInfoSeries;
            if (seriesInfo == null)
            {
                throw new ArgumentException("seriesInfo is not of DataRetrievalInfoSeries type");
            }

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                sql = GenerateSelect(false, seriesInfo.ComponentMapping.Values);
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(seriesInfo.MappingSet));

                AppendCachedWhere(seriesInfo, sqlQuery);

                //sqlQuery.appendSql(GenerateComplexWhere(info));

                var orderComponents = _orderedComponentBuilder.Build(seriesInfo);
                sqlQuery.appendSql(GenerateOrderBy(seriesInfo, orderComponents));
            }
            catch (DataRetrieverException)
            {
                throw;
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

        /// <summary>
        /// Appends the cached where to <paramref name="sql"/> from <see cref="DataRetrievalInfoSeries.SqlWhereCache"/> if it is not null or from <see cref="SqlBuilderBase.GenerateWhere"/>
        /// </summary>
        /// <param name="info">
        /// The current DataRetrieval state 
        /// </param>
        /// <param name="sql">
        /// The SQL String buffer to 
        /// </param>
        private static void AppendCachedWhere(DataRetrievalInfoSeries info, SqlQuery sqq)
        {
            if (string.IsNullOrEmpty(info.SqlWhereCache))
            {
                info.SqlWhereCache = GenerateComplexWhere(info);
            }

            sqq.appendSql(info.SqlWhereCache);
        }

        #endregion
    }
}

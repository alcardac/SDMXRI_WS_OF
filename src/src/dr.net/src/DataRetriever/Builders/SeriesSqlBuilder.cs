// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is responsible for building SQL Queries for Time Series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Builders
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Collections.Generic;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using System.Configuration;
    using Org.Sdmxsource.Sdmx.Api.Constants;


    /// <summary>
    /// This class is responsible for building SQL Queries for Time Series.
    /// </summary>
    internal class SeriesSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesSqlBuilder _instance = new SeriesSqlBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SeriesSqlBuilder));

        /// <summary>
        /// The ordered component builder
        /// </summary>
        private static readonly SeriesOrderedDimensionBuilder _orderedComponentBuilder;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesSqlBuilder" /> class from being created.
        /// </summary>
        private SeriesSqlBuilder()
        {
        }

        /// <summary>
        /// Initializes static members of the <see cref="SeriesSqlBuilder"/> class.
        /// </summary>
        static SeriesSqlBuilder()
        {
            _orderedComponentBuilder = new SeriesOrderedDimensionBuilder();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesSqlBuilder Instance
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

            MappingSetEntity mappingSet = info.MappingSet;

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                // Generate Query subparts
                var mappingEntities = ConvertToMapping(seriesInfo.AllComponentMappings);
                mappingEntities.Add(seriesInfo.TimeMapping);
                sql = GenerateSelect(false, mappingEntities);
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(mappingSet));

                AppendCachedWhere(seriesInfo, sqlQuery);

                sqlQuery.appendSql(GenerateOrderByLocalColumns(seriesInfo));
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

        #endregion

        #region Methods

        /// <summary>
        /// This method generates the ORDER BY part of the query
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// The string containing the ORDER BY part of the query 
        /// </returns>
        private static string GenerateOrderByLocalColumns(DataRetrievalInfo info)
        {
            var orderComponents = _orderedComponentBuilder.Build(info);

            var orderBy = GenerateOrderBy(info, orderComponents);

            return orderBy;
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
                info.SqlWhereCache = GenerateWhere(info);
            }

            sqq.appendSql(info.SqlWhereCache);
        }

        #endregion
    }
}
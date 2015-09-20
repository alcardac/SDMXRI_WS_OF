// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesDataSetSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is responsible for building SQL Queries for Time Series dataset attributes
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Builders
{
    using System;
    using System.Globalization;

    using IstatExtension.Retriever.Model;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Estat.Nsi.DataRetriever.Properties;

    /// <summary>
    /// This class is responsible for building SQL Queries for Time Series dataset attributes
    /// </summary>
    public sealed class SeriesDataSetSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesDataSetSqlBuilder _instance = new SeriesDataSetSqlBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SeriesDataSetSqlBuilder));

        private static IDataQueryBuilderManager dataQueryBuilderManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesDataSetSqlBuilder" /> class from being created.
        /// </summary>
        private SeriesDataSetSqlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesDataSetSqlBuilder Instance
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
            var seriesInfo = info as DataRetrievalInfoSeries;
            if (seriesInfo == null)
            {
                throw new ArgumentException("seriesInfo is not of DataRetrievalInfoSeries type");
            }

            GenerateDataSetSql(seriesInfo);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <param name="info">
        /// The current state of the data retrieval which containts the current query and mapping set 
        /// </param>
        private static void GenerateDataSetSql(DataRetrievalInfoSeries info)
        {
            if (info.DataSetAttributes.Count == 0)
            {
                return;
            }

           MappingSetEntity mappingSet = info.MappingSet;
            Logger.Info(Resources.InfoBeginGenerateSql);

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                // Generate Query subparts
                sql = GenerateSelect(true, ConvertToMapping(info.DataSetAttributes));
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(mappingSet));

                if (string.IsNullOrEmpty(info.SqlWhereCache))
                {
                    info.SqlWhereCache = GenerateWhere(info);
                }

                sqlQuery.appendSql(info.SqlWhereCache);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError),
                    Resources.ErrorUnableToGenerateSQL);
                //ErrorTypes.QUERY_PARSING_ERROR, Resources.ErrorUnableToGenerateSQL, ex);
            }

            // log for easy debug
            Logger.Info(string.Format(CultureInfo.InvariantCulture, Resources.InfoGeneratedSQLFormat1, sql));
            Logger.Info(Resources.InfoEndGenerateSql);

            info.DataSetSqlString = sqlQuery.getSql();
        }

        #endregion
    }
}
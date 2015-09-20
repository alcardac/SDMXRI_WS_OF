// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesGroupSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is responsible for building SQL Queries for Time Series groups.
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
    using System.Configuration;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Estat.Nsi.DataRetriever.Properties;

    /// <summary>
    /// This class is responsible for building SQL Queries for Time Series groups.
    /// </summary>
    public class SeriesGroupSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly SeriesGroupSqlBuilder _instance = new SeriesGroupSqlBuilder();

        private static ILog Logger = LogManager.GetLogger(typeof(SeriesGroupSqlBuilder));

        private static IDataQueryBuilderManager dataQueryBuilderManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="SeriesGroupSqlBuilder" /> class from being created.
        /// </summary>
        private SeriesGroupSqlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static SeriesGroupSqlBuilder Instance
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

            // generate sql queries for groups
            foreach (var groupEntity in seriesInfo.Groups)
            {
                var information = groupEntity.Value;
                information.SQL = GenerateGroupSql(information, seriesInfo);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <param name="groupBean">
        /// The group Bean. 
        /// </param>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <returns>
        /// The string containing the SQL query that needs to be executed on the dissemination database, in order to return the data required by the input query 
        /// </returns>
        private static string GenerateGroupSql(GroupInformation groupBean, DataRetrievalInfoSeries info)
        {
           MappingSetEntity mappingSet = info.MappingSet;
            Logger.Info(Resources.InfoBeginGenerateSql);

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                // Generate Query subparts
                sql = GenerateSelect(true, ConvertToMapping(groupBean.ComponentMappings));
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(mappingSet));

                if (string.IsNullOrEmpty(info.SqlWhereCache))
                {
                    info.SqlWhereCache = GenerateWhere(info);
                }

                sqlQuery.appendSql(info.SqlWhereCache);

                bool bFlat = false;
                var allDimensions = DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;
                IBaseDataQuery baseDataQuery = (IBaseDataQuery)info.ComplexQuery ?? info.Query;
                //the Flat option will be read only when we have flat aka AllDimensions
                if (baseDataQuery.DimensionAtObservation.Equals(allDimensions))
                    Boolean.TryParse(ConfigurationManager.AppSettings["QueryFlatFormat"], out bFlat);
                if (!bFlat)
                    sqlQuery.appendSql(GenerateOrderBy(info, groupBean.ThisGroup.Dimensions));
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

            return sqlQuery.getSql();
        }

        #endregion
    }
}
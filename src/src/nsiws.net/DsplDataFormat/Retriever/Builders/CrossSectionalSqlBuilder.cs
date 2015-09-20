// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalSqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   SQL Builder for CrossSectional output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DsplDataFormat.Retriever.Builders
{
    using System;
    using System.Globalization;

    using DsplDataFormat.Retriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using log4net;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using System.Collections.Generic;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using System.Configuration;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using DSplDataFormat.Retriever.Builders;

    /// <summary>
    /// SQL Builder for CrossSectional output
    /// </summary>
    internal class CrossSectionalSqlBuilder : SqlBuilderBase, ISqlBuilder
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly CrossSectionalSqlBuilder _instance = new CrossSectionalSqlBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(CrossSectionalSqlBuilder));

        /// <summary>
        /// The ordered component builder
        /// </summary>
        private static readonly SeriesOrderedDimensionBuilder _orderedComponentBuilder = new SeriesOrderedDimensionBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="CrossSectionalSqlBuilder" /> class from being created.
        /// </summary>
        private CrossSectionalSqlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static CrossSectionalSqlBuilder Instance
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

            MappingSetEntity mappingSet = info.MappingSet;
            Logger.Info(Resources.InfoBeginGenerateSql);

            SqlQuery sqlQuery = new SqlQuery();
            string sql = string.Empty;

            try
            {
                // Generate Query subparts
                sql = GenerateSelect(false, info.ComponentMapping.Values);
                sqlQuery.appendSql(sql);

                sqlQuery.appendSql(GenerateFrom(mappingSet));

                //the WHERE part
                sqlQuery.appendSql(GenerateWhere(info));

                sqlQuery.appendSql(GenerateXSOrderByLocalColumns(info));
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
        private static string GenerateXSOrderByLocalColumns(DataRetrievalInfo info)
        {
            DsdEntity dsd = info.MappingSet.Dataflow.Dsd;

            var orderComponents = _orderedComponentBuilder.Build(info);
            var crossOrderedComponents = new List<ComponentEntity>(orderComponents);
            crossOrderedComponents.Sort(OnCrossSectionalComparison);
            var orderBy = GenerateOrderBy(info, crossOrderedComponents);

            return orderBy;
        }

        /// <summary>
        /// Gets an array of the CrossSectional flags of <paramref name="component"/>
        /// </summary>
        /// <param name="component">
        /// The <see cref="ComponentEntity"/> 
        /// </param>
        /// <returns>
        /// an array of the CrossSectional flags of <paramref name="component"/> 
        /// </returns>
        private static bool[] GetCrossSectionalFlags(ComponentEntity component)
        {
            return new[]
                {
                    component.MeasureDimension, component.CrossSectionalLevelObs, component.CrossSectionalLevelSection, 
                    component.CrossSectionalLevelGroup || component.FrequencyDimension
                    || component.ComponentType == SdmxComponentType.TimeDimension, component.CrossSectionalLevelDataSet
                };
        }

        /// <summary>
        /// Compare two <paramref name="x"/> amd <paramref name="y"/> based on their Cross Sectional attachment level. The <see cref="ComponentEntity"/> attached to DataSet will be first, followed by Group, Section and finally observation.
        /// </summary>
        /// <param name="x">
        /// The first <see cref="ComponentEntity"/> to compare 
        /// </param>
        /// <param name="y">
        /// The second <see cref="ComponentEntity"/> to compare 
        /// </param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/> , 1 if <paramref name="x"/> is attached to a lower level than <paramref name="y"/> , -1 if <paramref name="x"/> is attached to a higher level than <paramref name="y"/> , else 0. 
        /// </returns>
        private static int OnCrossSectionalComparison(ComponentEntity x, ComponentEntity y)
        {
            var firstConditions = GetCrossSectionalFlags(x);
            var secondConditions = GetCrossSectionalFlags(y);

            for (var i = 0; i < firstConditions.Length; i++)
            {
                if (firstConditions[i] && secondConditions[i])
                {
                    return 0;
                }

                if (firstConditions[i] && !secondConditions[i])
                {
                    return 1;
                }

                if (!firstConditions[i] && secondConditions[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        #endregion
    }
}
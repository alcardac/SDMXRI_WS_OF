// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingConstants.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1.
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   mASTORE Setting (element tag names and attribute names) constants
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    /// <summary>
    /// MASTORE setting (element tag names and attribute names) constants
    /// </summary>
    internal static class SettingConstants
    {
        /// <summary>
        /// The attribute name of the flag for specifying if an SQL ORDER BY is allowed in a sub query.
        /// </summary>
        public const string SubQueryOrderBy = "subQueryOrderByAllowed";

        #region Constants and Fields

        /// <summary>
        /// The SQL Server Parameter Marker format
        /// </summary>
        public const string SqlServerParameterFormat = "@{0}";

        /// <summary>
        /// The Oracle Parameter Marker format
        /// </summary>
        public const string OracleParameterFormat = ":{0}";

        /// <summary>
        /// The MySQL Parameter Marker format
        /// </summary>
        public const string MySqlParameterFormat = "@{0}";

        /// <summary>
        /// The ODBC Parameter Marker format
        /// </summary>
        public const string OdbcParameterFormat = "?";

        /// <summary>
        /// The attribute name for the DB Date cast string
        /// </summary>
        public const string DateCast = "dateCast";

        /// <summary>
        /// The element tag name for the DB Mappings collection
        /// </summary>
        public const string DisseminationDatabaseSettings = "ddbSettings";

        /// <summary>
        /// The element tag name for the DB settings collection
        /// </summary>
        public const string GeneralDatabaseSettings = "DatabaseSettings";

        /// <summary>
        /// The element tag name for the Dataflow section
        /// </summary>
        public const string DataflowConfiguration = "Dataflow";

        /// <summary>
        /// The attribute name of the flag for ignoring the production flag for MappingSetRetriever.
        /// </summary>
        public const string IgnoreProductionForData = "ignoreProductionFlagForData";

        /// <summary>
        /// The attribute name of the flag for ignoring the production flag for dataflow retrieval (except MappingSetRetriever. <see cref="IgnoreProductionForData"/>.)
        /// </summary>
        public const string IgnoreProduction = "ignoreProductionFlagForStructure";

        /// <summary>
        /// The element tag name for the MYSQL mapping
        /// </summary>
        public const string MySqlElementName = "MySql";

        /// <summary>
        /// The attribute name for the Mapping Store DB_CONNECTION.DB_TYPE database name
        /// </summary>
        public const string NameAttributeName = "name";

        /// <summary>
        /// The cast used for date fields in Oracle and MySQL
        /// </summary>
        public const string OracleMySqlDateCast = "DATE";

        /// <summary>
        /// The attribute name for the DB provider/driver
        /// </summary>
        public const string ProviderAttributeName = "provider";

        /// <summary>
        /// The SQL Server SQL SUBSTRING command
        /// </summary>
        public const string SqlServerSubString = "SUBSTRING";

        /// <summary>
        /// The common SQL SUBSTRING command
        /// </summary>
        public const string StandardSubString = "SUBSTR";

        /// <summary>
        /// The attribute name for the DB substring command
        /// </summary>
        public const string SubStringAttributeName = "subStringCmd";

        /// <summary>
        /// The attribute name for the attribute indicating whether the substring requires length
        /// </summary>
        public const string SubStringNeedsLength = "subStringNeedsLength";

        /// <summary>
        /// The attribute name for the attribute indicating whether the substring requires length
        /// </summary>
        public const string ParameterMarkerFormat = "parameterMarkerFormat";

        /// <summary>
        /// The attribute name for the DB INT to VARCHAR cast string
        /// </summary>
        public const string CastToString = "castToStringFormat";

        #endregion
    }
}
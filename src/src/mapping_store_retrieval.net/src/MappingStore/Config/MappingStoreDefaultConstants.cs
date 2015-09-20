// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingStoreDefaultConstants.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The default Mapping store DB DB_TYPE to DB provider values
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    /// <summary>
    /// The default Mapping store DB DB_TYPE to DB provider values 
    /// </summary>
    public static class MappingStoreDefaultConstants
    {
        #region Constants and Fields

        /// <summary>
        /// The default name used for MySQL DDB at Mapping Store database
        /// </summary>
        public const string MySqlName = "MySQL";

        /// <summary>
        /// The default provider for MySQL
        /// </summary>
        public const string MySqlProvider = "MySql.Data.MySqlClient";

        /// <summary>
        /// The default name used for Oracle DDB at Mapping Store database
        /// </summary>
        public const string OracleName = "Oracle";

        /// <summary>
        /// The default provider for Oracle
        /// </summary>
        public const string OracleProvider = "System.Data.OracleClient";

        /// <summary>
        /// The alternative provider for Oracle
        /// </summary>
        public const string OracleProviderOdp = "Oracle.DataAccess.Client";

        /// <summary>
        /// The default name used for PCAxis DDB at Mapping Store database
        /// </summary>
        public const string PCAxisName = "PCAxis";

        /// <summary>
        /// The default provider for PCAxis
        /// </summary>
        public const string PCAxisProvider = "org.estat.PcAxis.PcAxisProvider";

        /// <summary>
        /// The default name used for SQL Server DDB at Mapping Store database
        /// </summary>
        public const string SqlServerName = "SqlServer";

        /// <summary>
        /// The default provider for SQL Server
        /// </summary>
        public const string SqlServerProvider = "System.Data.SqlClient";

        #endregion
    }
}
// -----------------------------------------------------------------------
// <copyright file="DataflowConstant.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The Dataflow table constant.
    /// </summary>
    internal static class DataflowConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the key family reference from a dataflow id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string KeyFamilyRefQueryFormat =
            "SELECT DSD.DSD_ID, ART.ID, ART.VERSION, ART.AGENCY FROM DSD, DATAFLOW DF, ARTEFACT_VIEW ART WHERE DF.DSD_ID = DSD.DSD_ID AND DSD.DSD_ID = ART.ART_ID AND DF.DF_ID = {0} ";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereClause = " T.PRODUCTION = 1";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereLatestClause = " AND T2.PRODUCTION = 1";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.Dataflow) { Table = "DATAFLOW", PrimaryKey = "DF_ID", ExtraFields = ", PRODUCTION" };

        /// <summary>
        /// Gets the table info.
        /// </summary>
        public static TableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }
    }
}
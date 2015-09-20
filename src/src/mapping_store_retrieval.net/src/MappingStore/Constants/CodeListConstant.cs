// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListConstant.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The code list and DSD_CODE tables constant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The code list and DSD_CODE tables constant.
    /// </summary>
    internal static class CodeListConstant
    {
        #region Constants

        /// <summary>
        ///     Gets the item order by.
        /// </summary>
        public const string ItemOrderBy = " ORDER BY T.LCD_ID";

        /// <summary>
        ///     The referencing from HCL. P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>
        /// </summary>
        public const string ReferencingFromHcl =
            " INNER JOIN HCL ON HCL.HCL_ID = P.ART_ID INNER JOIN HIERARCHY h ON h.HCL_ID = HCL.HCL_ID INNER JOIN HCL_CODE hc ON hc.H_ID = h.H_ID INNER JOIN DSD_CODE dc ON dc.LCD_ID = hc.LCD_ID INNER JOIN ARTEFACT A ON dc.CL_ID = A.ART_ID";

        /// <summary>
        ///     The referencing from DSD.P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>
        /// </summary>
        public const string ReferencingFromDsd =
            " INNER JOIN COMPONENT C ON C.DSD_ID = P.ART_ID INNER JOIN ARTEFACT A ON C.CL_ID = A.ART_ID";

        /// <summary>
        /// The SQL query to build the partial codelist from transcoded codes.
        /// </summary>
        public const string TranscodedCodes =
            " SELECT DSD_CODE.LCD_ID AS SYSID, ITEM.ID, LOCALISED_STRING.TEXT, LOCALISED_STRING.LANGUAGE, LOCALISED_STRING.TYPE, DSD_CODE.PARENT_CODE_ID AS PARENT   FROM DSD_CODE  INNER JOIN ITEM ON DSD_CODE.LCD_ID = ITEM.ITEM_ID   INNER JOIN LOCALISED_STRING ON ITEM.ITEM_ID = LOCALISED_STRING.ITEM_ID   WHERE DSD_CODE.CL_ID = {0}  AND DSD_CODE.LCD_ID in (       select TRANSCODING_RULE_DSD_CODE.CD_ID FROM TRANSCODING_RULE       INNER JOIN TRANSCODING_RULE_DSD_CODE ON TRANSCODING_RULE.TR_RULE_ID = TRANSCODING_RULE_DSD_CODE.TR_RULE_ID       INNER JOIN TRANSCODING ON TRANSCODING_RULE.TR_ID = TRANSCODING.TR_ID       INNER JOIN COMPONENT_MAPPING ON TRANSCODING.MAP_ID = COMPONENT_MAPPING.MAP_ID       INNER JOIN COM_COL_MAPPING_COMPONENT ON COMPONENT_MAPPING.MAP_ID = COM_COL_MAPPING_COMPONENT.MAP_ID       INNER JOIN MAPPING_SET ON COMPONENT_MAPPING.MAP_SET_ID = MAPPING_SET.MAP_SET_ID       INNER JOIN COMPONENT ON COM_COL_MAPPING_COMPONENT.COMP_ID = COMPONENT.COMP_ID       INNER JOIN DATAFLOW ON MAPPING_SET.MAP_SET_ID = DATAFLOW.MAP_SET_ID       INNER JOIN ARTEFACT A ON DATAFLOW.DF_ID = A.ART_ID where ( A.ID = {1} ) AND ( A.AGENCY = {2} ) AND ( dbo.isEqualVersion(A.VERSION1,A.VERSION2, A.VERSION3, {3}, {4}, {5} )=1 ) AND ( COMPONENT.ID = {6} ) {7} ) ";

        /// <summary>
        /// The <see cref="TranscodedCodes"/> order by.
        /// </summary>
        public const string TranscodedCodesOrderBy = " ORDER BY DSD_CODE.LCD_ID";

        /// <summary>
        /// The SQL query to build the partial codelist from LOCAL_CODES table.
        /// </summary>
        public const string LocalCodes =
            "SELECT I.ITEM_ID AS SYSID,I.ID, LOCALISED_STRING.TEXT, LOCALISED_STRING.LANGUAGE, LOCALISED_STRING.TYPE, T.PARENT_CODE_ID AS PARENT from  DSD_CODE T INNER JOIN ITEM I ON T.LCD_ID = I.ITEM_ID INNER JOIN  LOCALISED_STRING ON LOCALISED_STRING.ITEM_ID = I.ITEM_ID WHERE T.CL_ID = {0} and I.ID IN ( select LCI.ID  from  LOCAL_CODE LC INNER JOIN  ITEM LCI ON LC.LCD_ID = LCI.ITEM_ID INNER JOIN COM_COL_MAPPING_COLUMN DCM ON DCM.COL_ID = LC.COLUMN_ID INNER JOIN COMPONENT_MAPPING CM ON CM.MAP_ID = DCM.MAP_ID INNER JOIN DATAFLOW D ON D.MAP_SET_ID = CM.MAP_SET_ID INNER JOIN ARTEFACT A ON A.ART_ID = D.DF_ID INNER JOIN COM_COL_MAPPING_COMPONENT CCM ON CCM.MAP_ID =CM.MAP_ID INNER JOIN COMPONENT C ON CCM.COMP_ID = C.COMP_ID INNER JOIN ITEM CI ON CI.ITEM_ID = C.CON_ID WHERE A.ID = {1} and A.AGENCY = {2} and (dbo.isEqualVersion(A.VERSION1,A.VERSION2, A.VERSION3, {3}, {4}, {5})=1 ) and CI.ID = {6} {7}) ";

        /// <summary>
        /// The <see cref="LocalCodes"/> order by.
        /// </summary>
        public const string LocalCodesOrderBy = " ORDER BY T.LCD_ID";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The item table info. i.e. for table DSD_CODE
        /// </summary>
        private static readonly ItemTableInfo _itemTableInfo = new ItemTableInfo(SdmxStructureEnumType.Code) { Table = "DSD_CODE", PrimaryKey = "LCD_ID", ForeignKey = "CL_ID", ParentItem = "PARENT_CODE_ID" };

        /// <summary>
        ///     The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.CodeList) { Table = "CODELIST", PrimaryKey = "CL_ID", ExtraFields = ", IS_PARTIAL"};

        /// <summary>
        /// The <see cref="TranscodedCodes"/> <see cref="SqlQueryInfo"/>
        /// </summary>
        private static readonly SqlQueryInfo _transcodedSqlQueryInfo = new SqlQueryInfo { QueryFormat = TranscodedCodes, WhereStatus = WhereState.And, OrderBy = TranscodedCodesOrderBy };

        /// <summary>
        /// The <see cref="LocalCodes"/> <see cref="SqlQueryInfo"/>
        /// </summary>
        private static readonly SqlQueryInfo _localCodeSqlQueryInfo = new SqlQueryInfo { QueryFormat = LocalCodes, WhereStatus = WhereState.And, OrderBy = LocalCodesOrderBy };

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the item table info. i.e. for table DSD_CODE
        /// </summary>
        public static ItemTableInfo ItemTableInfo
        {
            get
            {
                return _itemTableInfo;
            }
        }

        /// <summary>
        ///     Gets the table info.
        /// </summary>
        public static TableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }

        /// <summary>
        /// Gets the <see cref="TranscodedCodes"/> <see cref="SqlQueryInfo"/>
        /// </summary>
        public static SqlQueryInfo TranscodedSqlQueryInfo
        {
            get
            {
                return _transcodedSqlQueryInfo;
            }
        }

        /// <summary>
        /// Gets the <see cref="LocalCodes"/> <see cref="SqlQueryInfo"/>
        /// </summary>
        public static SqlQueryInfo LocalCodeSqlQueryInfo
        {
            get
            {
                return _localCodeSqlQueryInfo;
            }
        }

        #endregion
    }
}
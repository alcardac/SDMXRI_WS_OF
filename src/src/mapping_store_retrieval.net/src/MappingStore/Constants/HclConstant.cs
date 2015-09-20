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
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The HCL, HIERARCHY, HCL_CODE and HLEVEL tables constants.
    /// </summary>
    internal static class HclConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the HIERARCY from a HCL id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string HierarchyQueryFormat =
                "SELECT T.H_ID as SYSID, A.ID, LN.TEXT, LN.LANGUAGE, LN.TYPE FROM HIERARCHY T, ARTEFACT A  LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = A.ART_ID WHERE T.H_ID = A.ART_ID AND T.HCL_ID ={0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the codelist references from the HCL id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string CodelistRefQueryFormat =
            "SELECT  distinct A.ID, A.AGENCY, A.VERSION FROM HCL_CODE T INNER JOIN DSD_CODE C ON T.LCD_ID = C.LCD_ID INNER JOIN ARTEFACT_VIEW A ON C.CL_ID = A.ART_ID INNER JOIN HIERARCHY H ON T.H_ID = H.H_ID WHERE  H.HCL_ID ={0} ";


        /// <summary>
        /// Gets the SQL Query format/template for retrieving the Hierarchical codelist level from a HIERARCHY id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string LevelQueryFormat =
              "SELECT T.LEVEL_ID as SYSID, A.ID, T.PARENT_LEVEL_ID AS PARENT, LN.TEXT, LN.LANGUAGE, LN.TYPE FROM HLEVEL T, ARTEFACT A  LEFT OUTER JOIN LOCALISED_STRING LN ON LN.ART_ID = A.ART_ID WHERE T.LEVEL_ID = A.ART_ID AND T.H_ID ={0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the code references from a HIERARCHY id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string CodeRefQueryFormat =
              "SELECT T.HCODE_ID as SYSID, A.ID as NodeAliasID, T.PARENT_HCODE_ID AS PARENT, I.ID as CODE_ID, AL.ID as LEVEL_REF, CL.ID as CLID, CL.VERSION as CLVERSION, CL.AGENCY as CLAGENCY, A.VALID_FROM, A.VALID_TO FROM HCL_CODE T  INNER JOIN ARTEFACT A ON T.HCODE_ID = A.ART_ID INNER JOIN DSD_CODE DC ON DC.LCD_ID = T.LCD_ID  INNER JOIN ITEM I ON I.ITEM_ID = DC.LCD_ID  INNER JOIN ARTEFACT_VIEW CL ON DC.CL_ID = CL.ART_ID  LEFT OUTER JOIN ARTEFACT AL ON AL.ART_ID = T.LEVEL_ID WHERE  T.H_ID ={0} ";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.HierarchicalCodelist) { Table = "HCL", PrimaryKey = "HCL_ID" };

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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorisationConstant.cs" company="Eurostat">
//   Date Created : 2013-02-14
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The categorisation constant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The categorisation constant.
    /// </summary>
    internal static class CategorisationConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the artefact reference from a categorisation id. Use with <see cref="string.Format(string,object)"/> and one parameter 
        /// 1. the <see cref="ProductionWhereClause"/> or <see cref="string.Empty"/> 
        /// </summary>
        public const string ArtefactRefQueryFormat =
            "SELECT C.CATN_ID, AV.ID, AV.VERSION, AV.AGENCY, T.STYPE FROM CATEGORISATION C INNER JOIN ARTEFACT_VIEW AV ON C.ART_ID = AV.ART_ID INNER JOIN (SELECT D.DF_ID as SID, 'Dataflow' as STYPE FROM DATAFLOW D INNER JOIN ARTEFACT A ON A.ART_ID = D.DF_ID {0} UNION ALL SELECT D.DSD_ID as SID, 'Dsd' as STYPE FROM DSD D UNION ALL SELECT D.CON_SCH_ID as SID, 'ConceptScheme' as STYPE FROM Concept_Scheme D UNION ALL SELECT D.CL_ID as SID, 'Codelist' as STYPE FROM Codelist D UNION ALL SELECT D.HCL_ID as SID, 'Hcl' as STYPE FROM HCL D) T  ON T.SID = C.ART_ID ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the category reference from a categorisation id.
        /// </summary>
        public const string CategoryRefQueryFormat =
            "SELECT C.CATN_ID, A.ID, A.VERSION, A.AGENCY, I.ID as CATID FROM CATEGORISATION C INNER JOIN CATEGORY CY ON C.CAT_ID = CY.CAT_ID INNER JOIN ARTEFACT_VIEW A ON CY.CAT_SCH_ID = A.ART_ID INNER JOIN ITEM I ON I.ITEM_ID = CY.CAT_ID ";

        /// <summary>
        /// The referenced by CATEGORISATION P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>.
        /// </summary>
        public const string ReferencedByCategorisation = " INNER JOIN CATEGORISATION T ON T.CATN_ID = P.ART_ID INNER JOIN ARTEFACT A ON T.ART_ID = A.ART_ID ";

        /// <summary>
        /// Gets the PRODUCTION clause
        /// </summary>
        public const string ProductionWhereClause = " D.PRODUCTION = 1 ";

        /// <summary>
        /// The categorisation version.
        /// </summary>
        public const string CategorisationVersion = "1.0";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.Categorisation) { Table = "CATEGORISATION", PrimaryKey = "CATN_ID", ExtraFields = ", DC_ORDER" };

        /// <summary>
        /// The _artefact reference.
        /// </summary>
        private static readonly SqlQueryInfo _artefactReference = new SqlQueryInfo { QueryFormat = ArtefactRefQueryFormat, WhereStatus = WhereState.Nothing };

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

        /// <summary>
        /// Gets the artefact reference.
        /// </summary>
        public static SqlQueryInfo ArtefactReference
        {
            get
            {
                return _artefactReference;
            }
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="CategorySchemeConstant.cs" company="Eurostat">
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
    /// The category scheme and category tables constant.
    /// </summary>
    internal static class CategorySchemeConstant
    {
        /// <summary>
        /// Gets the item order by (Category).
        /// </summary>
        public const string ItemOrderBy = " ORDER BY T.CORDER ASC, T.CAT_ID ASC ";

        /// <summary>
        /// Gets the order by for Category Scheme table
        /// </summary>
        public const string OrderBy = "  ORDER BY T.CS_ORDER ASC, T.CAT_SCH_ID ASC";

        /// <summary>
        /// The dataflow reference order by.
        /// </summary>
        public const string DataflowRefOrderBy = " ORDER BY DC.DC_ORDER ASC, T.DF_ID ASC";

        /// <summary>
        /// The referenced by CATEGORISATION P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>.
        /// </summary>
        public const string ReferencedByCategorisation = " INNER JOIN CATEGORISATION T ON T.CATN_ID = P.ART_ID INNER JOIN CATEGORY C ON T.CAT_ID = C.CAT_ID INNER JOIN ARTEFACT A ON C.CAT_SCH_ID = A.ART_ID ";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.CategoryScheme) { Table = "CATEGORY_SCHEME", PrimaryKey = "CAT_SCH_ID", ExtraFields = ", IS_PARTIAL, CS_ORDER" };

        /// <summary>
        /// The item table info. i.e. for table CATEGORY
        /// </summary>
        private static readonly ItemTableInfo _itemTableInfo = new ItemTableInfo(SdmxStructureEnumType.Category) { Table = "CATEGORY", PrimaryKey = "CAT_ID", ForeignKey = "CAT_SCH_ID", ParentItem = "PARENT_CAT_ID", ExtraFields = ", CORDER" };

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
        /// Gets the item table info. i.e. for table CATEGORY
        /// </summary>
        public static ItemTableInfo ItemTableInfo
        {
            get
            {
                return _itemTableInfo;
            }
        }
    }
}
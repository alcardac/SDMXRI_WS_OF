// -----------------------------------------------------------------------
// <copyright file="DsdConstant.cs" company="Eurostat">
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
    /// The DSD, COMPONENT, ATT_GROUP and DSD_GROUP constant.
    /// </summary>
    internal static class DsdConstant
    {
        /// <summary>
        /// Gets the SQL Query format/template for retrieving the Attribute attachment groups from a component id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string AttributeAttachmentGroupQueryFormat =
            "SELECT GR.ID FROM COMPONENT COMP  INNER JOIN ATT_GROUP AG ON COMP.COMP_ID = AG.COMP_ID INNER JOIN DSD_GROUP GR ON AG.GR_ID = GR.GR_ID WHERE COMP.COMP_ID = {0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the groups from the DSD id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string GroupQueryFormat =
            "SELECT G.GR_ID AS GR_ID, G.ID AS GROUP_ID, C.ID as DIMENSION_REF FROM DSD D, DSD_GROUP G, DIM_GROUP DG, COMPONENT C WHERE D.DSD_ID = G.DSD_ID AND G.GR_ID = DG.GR_ID AND DG.COMP_ID = C.COMP_ID AND D.DSD_ID = {0} ORDER BY G.GR_ID";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the attribute dimension references from the DSD id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string AttributeDimensionFormat =
            "SELECT A.COMP_ID, A.ID as AID, D.ID as DID FROM ATTR_DIMS AD INNER JOIN COMPONENT A ON A.COMP_ID = AD.ATTR_ID INNER JOIN COMPONENT D ON D.COMP_ID = AD.DIM_ID WHERE A.DSD_ID = D.DSD_ID AND A.DSD_ID = {0} ORDER BY A.COMP_ID";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving the DSD components from a DSD id. Use with <see cref="string.Format(string,object)"/> and one parameter the <see cref="ParameterNameConstants.IdParameter"/>
        /// </summary>
        public const string ComponentQueryFormat =
              "SELECT COMP.COMP_ID, COMP.TYPE, COMP.ID, RCS.ID as REP_CS_ID, RCS.VERSION as REP_CS_VERSION, RCS.AGENCY as REP_CS_AGENCY, I.ID as CONCEPTREF, CS.ID as CONCEPTSCHEME_ID, CS.VERSION as CONCEPT_VERSION, CS.AGENCY as CONCEPT_AGENCY, CL.ID as CODELIST_ID, CL.VERSION as CODELIST_VERSION, CL.AGENCY as CODELIST_AGENCY,  COMP.IS_FREQ_DIM, COMP.IS_MEASURE_DIM, COMP.ATT_ASS_LEVEL, COMP.ATT_STATUS, COMP.ATT_IS_TIME_FORMAT, COMP.XS_ATTLEVEL_DS, COMP.XS_ATTLEVEL_GROUP, COMP.XS_ATTLEVEL_SECTION, COMP.XS_ATTLEVEL_OBS, COMP.XS_MEASURE_CODE FROM DSD INNER JOIN COMPONENT COMP ON DSD.DSD_ID = COMP.DSD_ID INNER JOIN CONCEPT C ON COMP.CON_ID = C.CON_ID INNER JOIN ITEM I ON C.CON_ID = I.ITEM_ID INNER JOIN ARTEFACT_VIEW CS ON C.CON_SCH_ID = CS.ART_ID LEFT OUTER JOIN ARTEFACT_VIEW CL ON COMP.CL_ID = CL.ART_ID LEFT OUTER JOIN ARTEFACT_VIEW RCS ON COMP.CON_SCH_ID = RCS.ART_ID WHERE DSD.DSD_ID = {0} ";

        /// <summary>
        /// Gets the SQL Query format/template for retrieving all text formats used by components in a DSD.
        /// </summary>
        public const string TextFormatQueryFormat =
            "SELECT c.COMP_ID, e.ENUM_NAME, e.ENUM_VALUE, t.FACET_VALUE from COMPONENT c INNER JOIN TEXT_FORMAT t ON t.COMP_ID = c.COMP_ID INNER JOIN ENUMERATIONS e ON t.FACET_TYPE_ENUM = e.ENUM_ID WHERE c.DSD_ID = {0}";

        /// <summary>
        /// The component order by.
        /// </summary>
        public const string ComponentOrderBy = "ORDER BY COMP.COMP_ID ";

        /// <summary>
        /// The referenced by dataflow P table the "parent" and A is the referenced <see cref="ArtefactParentsSqlBuilder.SqlQueryFormat"/>.
        /// </summary>
        public const string ReferencedByDataflow = " INNER JOIN DATAFLOW T ON T.DF_ID = P.ART_ID INNER JOIN ARTEFACT A ON T.DSD_ID = A.ART_ID ";

        /// <summary>
        /// The _table info.
        /// </summary>
        private static readonly TableInfo _tableInfo = new TableInfo(SdmxStructureEnumType.Dsd) { Table = "DSD", PrimaryKey = "DSD_ID" };

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
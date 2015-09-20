// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxMedia.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest.Utils
{
    /// <summary>
    ///     MIME types
    /// </summary>
    public static class SdmxMedia
    {
        #region Static Fields

        /// <summary>
        /// The application xml.
        /// </summary>
        public const string ApplicationXml = "application/xml";

        /// <summary>
        /// The Text xml.
        /// </summary>
        public const string TextXml = "text/xml";

        /// <summary>
        /// The compact data.
        /// </summary>
        public const string CompactData = "application/vnd.sdmx.compactdata+xml";

        /// <summary>
        /// The cross sectional data.
        /// </summary>
        public const string CrossSectionalData = "application/vnd.sdmx.crosssectionaldata+xml";

        /// <summary>
        /// The CSV data.
        /// </summary>
        public const string CsvData = "text/csv";

        /// <summary>
        /// The EDI data.
        /// </summary>
        public const string EdiData = "application/vnd.sdmx.edidata";

        /// <summary>
        /// The EDI structure.
        /// </summary>
        public const string EdiStructure = "application/vnd.sdmx.edistructure";

        /// <summary>
        /// The generic data.
        /// </summary>
        public const string GenericData = "application/vnd.sdmx.genericdata+xml";

        /// <summary>
        /// The structure.
        /// </summary>
        public const string Structure = "application/vnd.sdmx.structure+xml";

        /// <summary>
        /// The structure specific data.
        /// </summary>
        public const string StructureSpecificData = "application/vnd.sdmx.structurespecificdata+xml";

        public const string RdfData = "application/rdf+xml";
        public const string DsplData = "application/dspl";
        public const string CsvZipData = "application/csv+zip";
        public const string JsonData = "application/json";
        public const string JsonZipData = "application/json+zip";
        public const string JsonStructure = "application/json";


        #endregion
    }
}
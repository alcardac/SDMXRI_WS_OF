// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterNameConstants.cs" company="Eurostat">
//   Date Created : 2011-08-22
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class contains common parameter names
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    /// <summary>
    /// This class contains common parameter names
    /// </summary>
    internal static class ParameterNameConstants
    {
        #region Constants and Fields

        /// <summary>
        /// Agency prepared statement named parameter
        /// </summary>
        public const string AgencyParameter = "Agency";

        /// <summary>
        /// Prefix for code prepared statement named parameter
        /// </summary>
        public const string CodeParameterName = "code";

        /// <summary>
        /// ID/SysID prepared statement named parameter
        /// </summary>
        public const string IdParameter = "Id";

        /// <summary>
        /// Dataflow ID prepared statement named parameter
        /// </summary>
        public const string DataflowIdParameter = "DfId";

        /// <summary>
        /// Concept ID prepared statement named parameter
        /// </summary>
        public const string ConceptIdParameter = "cid";

        /// <summary>
        /// Transcoding ID prepared statement named parameter
        /// </summary>
        public const string TranscodingId = "trid";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter = "Version";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter1 = "Version1";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter2 = "Version2";

        /// <summary>
        /// Version prepared statement named parameter
        /// </summary>
        public const string VersionParameter3 = "Version3";

        /// <summary>
        /// Production prepared statement named parameter
        /// </summary>
        public const string ProductionParameter = "Production";

        #endregion
    }
}
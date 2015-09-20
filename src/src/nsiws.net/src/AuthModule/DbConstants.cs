// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConstants.cs" company="Eurostat">
//   Date Created : 2011-08-13
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A list of constants used by the <see cref="DbAuthenticationProvider" /> and <see cref="DbAuthorizationProvider" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    /// <summary>
    /// A list of constants used by the <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/>
    /// </summary>
    internal static class DbConstants
    {
        #region Constants and Fields

        /// <summary>
        /// The dataflow agency id field. This string will replace the <see cref="DataflowAgencyIdMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowAgencyIdField = "dataflowAgencyIdField";

        /// <summary>
        /// The dataflow agency id macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowAgencyIdMacro = "${agencyId}";

        /// <summary>
        /// The dataflow id field. This string will replace the <see cref="DataflowIdMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowIdField = "dataflowIdField";

        /// <summary>
        /// The dataflow id macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowIdMacro = "${id}";

        /// <summary>
        /// The dataflow version field. This string will replace the <see cref="DataflowVersionMacro"/> from  <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowVersionField = "dataflowVersionField";

        /// <summary>
        /// The dataflow version macro variable used in the <see cref="DbAuthorizationProvider._selectQuery"/>
        /// </summary>
        public const string DataflowVersionMacro = "${version}";

        /// <summary>
        /// The user macro variable used in the user provided queries
        /// </summary>
        public const string UserMacro = "${user}";

        /// <summary>
        /// The prepared statement parameter name
        /// </summary>
        public const string UserParamName = "auserid";

        #endregion
    }
}
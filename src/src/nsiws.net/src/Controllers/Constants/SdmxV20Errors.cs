// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxV20Errors.cs" company="Eurostat">
//   Date Created : 2013-10-24
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sdmx v 20 errors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Constants
{
    /// <summary>
    /// The sdmx v 20 errors.
    /// </summary>
    public static class SdmxV20Errors
    {
        #region Constants

        /// <summary>
        ///     The error code for client related errors in soap fault messages
        /// </summary>
        public const string ErrorNumberClient = "2000";

        /// <summary>
        ///     The error code for server related errors in soap fault messages
        /// </summary>
        public const string ErrorNumberServer = "1000";

        #endregion
    }
}
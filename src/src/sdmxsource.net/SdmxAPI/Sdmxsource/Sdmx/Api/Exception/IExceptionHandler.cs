// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionHandler.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Exception
{
    /// <summary>
    /// ExceptionHandler is used to handle exceptions that are thrown in the system
    /// </summary>
    public interface IExceptionHandler
    {
        #region Public Methods and Operators

        /// <summary>
        /// Handle exceptions that are thrown in the system
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <exception cref="System.Exception">When a limit is hit.</exception>
        void HandleException(System.Exception ex);

        #endregion
    }
}

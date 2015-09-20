// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IErrorWriterManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Output
{
    #region Using directives

    using System;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model;

    #endregion

    /// <summary>
    /// Responsible for writing error messages in the format given
    /// </summary>
    public interface IErrorWriterManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes the exception to the output stream
        /// </summary>
        /// <param name="ex">
        /// The exception
        /// </param>
        /// <param name="outPutStream">
        /// The stream to write the error response to
        /// </param>
        /// <param name="outputFormat">
        /// The format to write the error message in
        /// </param>
        /// <returns>
        /// The HTTP status code based on the error type
        /// </returns>
        int WriteError(Exception ex, Stream outPutStream, IErrorFormat outputFormat);

        #endregion

    }
}

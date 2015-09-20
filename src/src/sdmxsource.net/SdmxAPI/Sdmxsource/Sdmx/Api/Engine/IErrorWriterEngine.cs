// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IErrorWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Engine
{

    #region Using directives

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Writes Throwable messsages to the output stream, in the format defined by the implementation
    /// </summary>
    public interface IErrorWriterEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes an error to the output stream
        /// </summary>
        /// <param name="ex">
        /// The error to write
        /// </param>
        /// <param name="outPutStream">
        /// The output stream to write to
        /// </param>
        /// <returns>
        /// The HTTP Status code
        /// </returns>
        int WriteError(Exception ex, Stream outPutStream);

        #endregion

    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
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

    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    #endregion

    /// <summary>
    ///     A WriterEngine is capable of writing messages
    /// </summary>
    public interface IWriterEngine : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        /// (Optional) Writes a header to the message, any missing mandatory attributes are defaulted.  If a header is required for a message, then this
        ///     call should be the first call on a WriterEngine
        ///     <p/>
        ///     If this method is not called, and a message requires a header, a default header will be generated.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        void WriteHeader(IHeader header);

        #endregion
    }
}
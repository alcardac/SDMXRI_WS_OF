// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IErrorWriterFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model;

    #endregion

    /// <summary>
    /// Plugin factory used to return error messages in the format specified
    /// </summary>
    public interface IErrorWriterFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an error writer in the format specified.  Returns null if the format is unknown by the implementation
        /// </summary>
        /// <param name="format">
        /// The format
        /// </param>
        /// <returns>
        /// Engine, or null if the format is unknown to this factory
        /// </returns>
        IErrorWriterEngine GetErrorWriterEngine(IErrorFormat format);

        #endregion

    }
}

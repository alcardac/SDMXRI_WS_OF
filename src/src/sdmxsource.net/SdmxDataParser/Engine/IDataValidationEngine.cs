// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataValidationEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    /// The DataValidationEngine is used to validate the data that it has a handle on
    /// </summary>
    public interface IDataValidationEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates the data and writes any thrown exceptions to the <paramref name="exceptionHandler"/> passed in
        /// </summary>
        /// <param name="exceptionHandler">
        /// The <see cref="IExceptionHandler"/>
        /// </param>
        void ValidateData(IExceptionHandler exceptionHandler);

        #endregion
    }
}

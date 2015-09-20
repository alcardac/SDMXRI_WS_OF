// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataValidationManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Validation
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    /// The DataValidationManager is responsible for validating data read from a DataReaderEngine
    /// </summary>
    public interface IDataValidationManager
    {
        #region Public Methods and Operators
        	
        /// <summary>
        /// Validates the data read from the DataReaderEngine 
        /// </summary>
        /// <param name="dre">
        /// The reader engine used to read the dataset.  The DataValidationManager will reset the DataReaderEngine 
        /// </param>
        /// <param name="exceptionHandler">
        /// The exception handler
        /// </param>
        /// <exception cref="SdmxSyntaxException">
        /// if the data is not syntactically valid
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// if the data is syntactically valid, however it contains invalid content
        /// </exception>
        void ValidateData(IDataReaderEngine dre, IExceptionHandler exceptionHandler);

        #endregion
    }
}

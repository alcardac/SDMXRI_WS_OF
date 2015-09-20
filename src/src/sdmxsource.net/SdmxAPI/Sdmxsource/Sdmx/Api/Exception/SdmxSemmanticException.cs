// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxSemmanticException.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Exception
{

    #region Using Directives

    using System;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    /// Access exception is a specialised form of SdmxException which 
    /// has a fixed Sdmx Exception type of SdmxErrorCodeEnumType.SemanticError
    /// </summary>
    [Serializable]
    public class SdmxSemmanticException : SdmxException
    {
        /// <summary>
        /// The SDMX error code
        /// </summary>
        private static readonly SdmxErrorCode _sdmxErrorCode = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError);

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// </summary>
        public SdmxSemmanticException()
            : base(_sdmxErrorCode.ErrorString, _sdmxErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// Creates Exception from an exception and an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        /// <param name="exception">
        /// The exception
        /// </param>
        public SdmxSemmanticException(string errorMessage, Exception exception)
            : base(exception, _sdmxErrorCode, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// Creates Exception from an exception, an exception code and some arguments
        /// </summary>
        /// <param name="exception">
        /// The exception
        /// </param>
        /// <param name="code">
        /// The exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxSemmanticException(Exception exception, ExceptionCode code, params object [] args)
            : base(exception, _sdmxErrorCode, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// Creates Exception from an exception code and some arguments
        /// </summary>
        /// <param name="code">
        /// The exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxSemmanticException(ExceptionCode code, params object[] args)
            : base(null, _sdmxErrorCode, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// Creates Exception from an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        public SdmxSemmanticException(string errorMessage)
            : base(errorMessage, _sdmxErrorCode)
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSemmanticException"/> class. 
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. 
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null. 
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). 
        /// </exception>
        protected SdmxSemmanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

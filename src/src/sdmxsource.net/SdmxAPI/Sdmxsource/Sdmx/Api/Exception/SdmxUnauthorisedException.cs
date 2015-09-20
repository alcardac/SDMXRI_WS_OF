// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxUnauthorisedException.cs" company="Eurostat">
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
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    /// Access exception is a specialized form of SdmxException which 
    /// has a fixed sDMX Exception type of SdmxErrorCodeEnumType.Unauthorised
    /// </summary>
    [Serializable]
    public class SdmxUnauthorisedException : SdmxException
    {
        /// <summary>
        /// The SDMX error code
        /// </summary>
        private static readonly SdmxErrorCode _sdmxErrorCode = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.Unauthorised);

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxUnauthorisedException"/> class. 
        /// Creates Exception from an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        public SdmxUnauthorisedException(string errorMessage)
            : base(errorMessage, _sdmxErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxUnauthorisedException"/> class. 
        /// </summary>
        /// <param name="code">
        /// The Exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxUnauthorisedException(ExceptionCode code, params object[] args)
            : base(_sdmxErrorCode, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxUnauthorisedException"/> class. 
        /// If the <paramref name="innerException"/> is a SdmxException - then the
        /// error code will be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public SdmxUnauthorisedException(string errorMessage, Exception innerException)
            : base(innerException, _sdmxErrorCode, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxUnauthorisedException"/> class. 
        /// </summary>
        public SdmxUnauthorisedException()
            : base(_sdmxErrorCode.ErrorString, _sdmxErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxUnauthorisedException"/> class. 
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
        protected SdmxUnauthorisedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the error type.
        /// </summary>
        public override string ErrorType
        {
            get
            {
                return "Security Exception";
            }
        }

        #endregion
    }
}

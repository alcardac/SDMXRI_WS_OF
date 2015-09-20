// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxNotImplementedException.cs" company="Eurostat">
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
    /// has a fixed SDMX not implemented Exception types
    /// </summary>
    [Serializable]
    public class SdmxNotImplementedException : SdmxException
    {
        /// <summary>
        /// The SDMX error code.
        /// </summary>
        private static readonly SdmxErrorCode _sdmxErrorCode = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.NotImplemented);

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// </summary>
        public SdmxNotImplementedException()
            : base(_sdmxErrorCode.ErrorString, _sdmxErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Creates Exception from an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        public SdmxNotImplementedException(string errorMessage)
            : base(errorMessage, _sdmxErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Creates Exception from an Error code and some arguments
        /// </summary>
        /// <param name="code">
        /// The Exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxNotImplementedException(ExceptionCode code, params object[] args)
            : this(null, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Creates Exception from some arguments
        /// </summary>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxNotImplementedException(params object[] args)
            : this(null, ExceptionCode.Unsupported, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Creates Exception from an Exception
        /// </summary>
        /// <param name="exception">
        /// The exception
        /// </param>
        public SdmxNotImplementedException(Exception exception)
            : this(exception, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Creates Exception from an Exception, an Exception code and some arguments
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
        public SdmxNotImplementedException(Exception exception, ExceptionCode code, params object[] args)
            : base(exception, _sdmxErrorCode, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// If the <paramref name="innerException"/> is a SdmxException - then the
        /// error code will be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public SdmxNotImplementedException(string errorMessage, Exception innerException)
            : base(innerException, _sdmxErrorCode, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxNotImplementedException"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
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
        protected SdmxNotImplementedException(SerializationInfo info, StreamingContext context)
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
                return "Not Implemented Exception";
            }
        }

        #endregion
    }
}

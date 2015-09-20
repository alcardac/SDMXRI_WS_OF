// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncompleteMappingSetException.cs" company="Eurostat">
//   Date Created : 2011-08-26
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Mapping Store related exceptions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval
{
    using System;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    /// Mapping Store related exceptions
    /// </summary>
    [Serializable]
    public class IncompleteMappingSetException : SdmxException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompleteMappingSetException"/> class.
        /// </summary>
        public IncompleteMappingSetException() : base(ExceptionCode.FailValidation)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompleteMappingSetException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public IncompleteMappingSetException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompleteMappingSetException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. 
        /// </param>
        /// <param name="message">
        ///     The error message that explains the reason for the exception. 
        /// </param>
        public IncompleteMappingSetException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompleteMappingSetException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. 
        ///                 </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. 
        ///                 </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null. 
        ///                 </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). 
        /// </exception>
        protected IncompleteMappingSetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
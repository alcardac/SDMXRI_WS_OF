// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthConfigurationException.cs" company="Eurostat">
//   Date Created : 2011-06-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An exception that is thrown when there is a configuration error
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception that is thrown when there is a configuration error
    /// </summary>
    [Serializable]
    public class AuthConfigurationException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfigurationException"/> class with the specified error message
        /// </summary>
        /// <param name="message">
        /// The error message
        /// </param>
        public AuthConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfigurationException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">
        /// The error message
        /// </param>
        /// <param name="innerException">
        /// The inner exception
        /// </param>
        public AuthConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfigurationException"/> class.
        /// </summary>
        public AuthConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfigurationException"/> class with serialized data.
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
        protected AuthConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRetrieverException.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is an exception thrown while retrieving structural metadata
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// This is an exception thrown while retrieving structural metadata
    /// </summary>
    [Serializable]
    public class StructureRetrieverException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the StructureRetrieverException class wiith a specified Error Type
        /// </summary>
        /// <param name="errorType">
        /// The error type of error 
        /// </param>
        public StructureRetrieverException(StructureRetrieverErrorTypes errorType)
        {
            this.ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the StructureRetrieverException class wiith a specified Error Type and error message
        /// </summary>
        /// <param name="errorType">
        /// The error type of error 
        /// </param>
        /// <param name="message">
        /// A message that describes the error. 
        /// </param>
        public StructureRetrieverException(StructureRetrieverErrorTypes errorType, string message)
            : base(message)
        {
            this.ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the StructureRetrieverException class wiith a specified Error Type, error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="errorType">
        /// The error type of error 
        /// </param>
        /// <param name="message">
        /// A message that describes the error. 
        /// </param>
        /// <param name="innerException">
        /// The inner exception reference. 
        /// </param>
        public StructureRetrieverException(
            StructureRetrieverErrorTypes errorType, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        ///   Initializes a new instance of the <see cref="StructureRetrieverException"/> class. Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public StructureRetrieverException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception. 
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. 
        /// </param>
        public StructureRetrieverException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public StructureRetrieverException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. 
        /// Initializes a new instance of the <see cref="StructureRetrieverException"/> class. Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. 
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The
        ///   <paramref name="info"/>
        ///   parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or
        ///   <see cref="P:System.Exception.HResult"/>
        ///   is zero (0).
        /// </exception>
        protected StructureRetrieverException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets type of error
        /// </summary>
        public StructureRetrieverErrorTypes ErrorType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. 
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The
        ///   <paramref name="info"/>
        ///   parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ErrorType", this.ErrorType);
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
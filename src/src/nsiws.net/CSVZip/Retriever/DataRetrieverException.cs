// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrieverException.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is an exception thrown while retrieving data
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// This is an exception thrown while retrieving data
    /// </summary>
    [Serializable]
    public class DataRetrieverException : SdmxException
    {
        #region Constants and Fields

        /// <summary>
        ///   An optional DataRetriever error enumeration
        /// </summary>
        private readonly string _errorType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrieverException"/> class. Initializes with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="nestedException">
        /// The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception. 
        /// </param>
        ///  /// <param name="errorCode">
        /// The error code 
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception. 
        /// </param>
        public DataRetrieverException(Exception nestedException, SdmxErrorCode errorCode, string message)
            : base(nestedException, errorCode, message)
        {
            this._errorType = errorCode.ErrorString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrieverException"/> class with a specified error message and error code
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception. 
        /// </param>
        /// <param name="errorCode">
        /// The error code
        /// </param>
        public DataRetrieverException(string errorMessage, SdmxErrorCode errorCode)
            : base(errorMessage, errorCode)
        {
            this._errorType = errorCode.ErrorString;
        }


        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the DataRetriever error enumeration (optional)
        /// </summary>
        public override string ErrorType
        {
            get
            {
                return this._errorType;
            }
        }

        #endregion

        #region Public Methods

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

            info.AddValue("ErrorType", this._errorType);
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseException.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Exception
{
    #region Using directives

    using System;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion


    /// <summary>
    /// The parse innerException.
    /// </summary>
    [Serializable]
    public class ParseException : SdmxException
    {
        #region Fields

        /// <summary>
        /// The action.
        /// </summary>
        private readonly DatasetActionEnumType _action;

        /// <summary>
        /// The artifact.
        /// </summary>
        private readonly ArtifactType _artifact;

        /// <summary>
        /// The is query.
        /// </summary>
        private readonly bool _isQuery;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class. 
        /// </summary>
        public ParseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="action">The action.</param>
        /// <param name="isQuery">if set to <c>true</c> [is query].</param>
        /// <param name="artifact">The artifact.</param>
        /// <param name="args">The arguments.</param>
        public ParseException(
            Exception innerException, DatasetActionEnumType action, bool isQuery, ArtifactType artifact, params object[] args)
            : base(innerException, null, null, args)
        {
            this._action = action;
            this._isQuery = isQuery;
            this._artifact = artifact;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class. 
        /// If the <paramref name="innerException"/> is a SdmxException - then the
        /// error code will be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public ParseException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class. 
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public ParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class. 
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
        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        public DatasetActionEnumType Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        /// Gets the artifact.
        /// </summary>
        public ArtifactType Artifact
        {
            get
            {
                return this._artifact;
            }
        }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        public override string ErrorType
        {
            get
            {
                return "Parse Exception";
            }
        }

        /// <summary>
        /// Gets a value indicating whether query.
        /// </summary>
        public bool Query
        {
            get
            {
                return this._isQuery;
            }
        }

        #endregion
    }
}
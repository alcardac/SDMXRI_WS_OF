// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxReferenceException.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Exception
{

    #region Using directives

    using System;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// A reference exception is thrown when a cross referenced structure can not be resolved
    /// </summary>
    [Serializable]
    public class SdmxReferenceException : SdmxSemmanticException
    {

        #region Fields

        /// <summary>
        /// The _referenced from.
        /// </summary>
        private readonly IIdentifiableObject _referencedFrom;

        /// <summary>
        /// The _reference to.
        /// </summary>
        private readonly IStructureReference _referenceTo;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
        /// </summary>
        public SdmxReferenceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
        /// Creates Exception from a reference to
        /// </summary>
        /// <param name="referenceTo">
        /// The reference to
        /// </param>
        public SdmxReferenceException(IStructureReference referenceTo)
            : base(GetErrorString(null, referenceTo))
        {
            this._referenceTo = referenceTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException" /> class.
        /// Creates Exception from a reference to
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="referenceTo">The reference to</param>
        public SdmxReferenceException(Exception innerException, IStructureReference referenceTo)
            : base( GetErrorString(null, referenceTo), innerException)
        {
            this._referenceTo = referenceTo;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
        /// Creates Exception from a reference from and a reference to
        /// </summary>
        /// <param name="referencedFrom">
        /// The reference from
        /// </param>
        /// <param name="referenceTo">
        /// The reference to
        /// </param>
        public SdmxReferenceException(IIdentifiableObject referencedFrom, IStructureReference referenceTo)
            : base(GetErrorString(referencedFrom, referenceTo))
        {
            this._referencedFrom = referencedFrom;
            this._referenceTo = referenceTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
        /// Creates Exception from an exception and an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        /// <param name="exception">
        /// The exception
        /// </param>
        public SdmxReferenceException(string errorMessage, Exception exception)
            : base(errorMessage, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
        /// Creates Exception from an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        public SdmxReferenceException(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxReferenceException"/> class. 
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
        protected SdmxReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this._referencedFrom = (IIdentifiableObject)info.GetValue("_referencedFrom", typeof(IIdentifiableObject));
            this._referenceTo = (IStructureReference)info.GetValue("_referenceTo", typeof(IStructureReference));
        }
       
        #endregion


        #region Public Properties


        /// <summary>
        /// Gets the referenced from
        /// </summary>
        public IIdentifiableObject ReferencedFrom
        {
            get
            {
                return this._referencedFrom;
            }
        }

        /// <summary>
        /// Gets the reference to
        /// </summary>
        public IStructureReference ReferenceTo
        {
            get
            {
                return this._referenceTo;
            }
        }

        #endregion

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_referenceTo", this._referenceTo);
            info.AddValue("_referencedFrom", this._referencedFrom);
        }

        #region Methods

        /// <summary>
        /// Get an error string based on IIdentifiableObject and IStructureReference parameters
        /// </summary>
        /// <param name="referencedFrom">
        /// The reference from
        /// </param>
        /// <param name="referenceTo">
        /// The reference to
        /// </param>
        /// <returns>
        /// The error string
        /// </returns>
        private static string GetErrorString(IIdentifiableObject referencedFrom, IStructureReference referenceTo)
        {
            if (referencedFrom == null)
            {
                return "Can not resolve reference to  '" + referenceTo + "'";
            }
            else
            {
                return "Can not resolve reference from '" + referencedFrom + "' to  '" + referenceTo + "'";
            }
        }

        #endregion

    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceException.cs" company="Eurostat">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// Creates a reference exception from a cross reference that could not be resolved in the system
    /// </summary>
    [Serializable]
    public class CrossReferenceException : SdmxSemmanticException
    {

        #region Fields

        /// <summary>
        /// The _cross reference
        /// </summary>
        private readonly ICrossReference _crossReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceException"/> class. 
        /// </summary>
        public CrossReferenceException()
            : base(ExceptionCode.ReferenceErrorUnresolvable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceException"/> class.
        /// </summary>
        /// <param name="crossReference">The cross reference</param>
        public CrossReferenceException(ICrossReference crossReference)
            : base(ExceptionCode.ReferenceErrorUnresolvable, GetReferenceFromIdentifier(crossReference), crossReference != null ? crossReference.TargetUrn : null)
        {
            if (crossReference == null)
            {
                throw new ArgumentNullException("crossReference");
            }

            this._crossReference = crossReference;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceException"/> class. 
        /// Creates Exception from an exception and an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        /// <param name="exception">
        /// The exception
        /// </param>
        public CrossReferenceException(string errorMessage, Exception exception)
            : base(exception, ExceptionCode.ReferenceErrorUnresolvable, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceException"/> class. 
        /// Creates Exception from an error Message
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        public CrossReferenceException(string errorMessage)
            : base(ExceptionCode.ReferenceErrorUnresolvable, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceException"/> class. 
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
        protected CrossReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this._crossReference = (ICrossReference)info.GetValue("_crossReference", typeof(ICrossReference));
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the cross reference that could not be resolved.
        /// </summary>
        /// <value>
        /// The cross reference.
        /// </value>
        public ICrossReference CrossReference
        {
            get
            {
                return this._crossReference;
            }
        }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        public override string ErrorType
        {
            get
            {
                return "Reference Exception";
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
            info.AddValue("_crossReference", this._crossReference);
        }

        /// <summary>
        /// Gets the reference from identifier.
        /// </summary>
        /// <param name="crossReference">The cross reference.</param>
        /// <returns>A string containing the URN or the <see cref="ISdmxObject"/> it referenced</returns>
        private static string GetReferenceFromIdentifier(ICrossReference crossReference)
        {
            if (crossReference == null)
            {
                throw new ArgumentNullException("crossReference");
            }

            var parentIdentifiable = crossReference.ReferencedFrom.GetParent<IIdentifiableObject>(true);
            if (parentIdentifiable != null)
            {
                return parentIdentifiable.Urn.ToString();
            }

            return crossReference.ReferencedFrom.ToString();
        }
    }
}

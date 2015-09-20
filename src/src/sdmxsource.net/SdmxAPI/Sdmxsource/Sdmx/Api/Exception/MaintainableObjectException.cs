// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableBeanException.cs" company="Eurostat">
//   Date Created : 2013-04-01
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
    using System.Globalization;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The maintainable Object exception.
    /// </summary>
    [Serializable]
    public class MaintainableObjectException : SdmxException
    {
        #region Fields

        /// <summary>
        ///     The _agency id.
        /// </summary>
        private readonly string _agencyId;

        /// <summary>
        ///     The _id.
        /// </summary>
        private readonly string _id;

        /// <summary>
        ///     The _structure type.
        /// </summary>
        private readonly SdmxStructureType _structureEnumType;

        /// <summary>
        ///     The _version.
        /// </summary>
        private readonly string _version;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class. 
        /// </summary>
        public MaintainableObjectException()
        {
            this._structureEnumType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure ENUM type .
        /// </param>
        /// <param name="agencyId">
        /// The agency id .
        /// </param>
        /// <param name="id">
        /// The id .
        /// </param>
        /// <param name="version">
        /// The version .
        /// </param>
        public MaintainableObjectException(Exception exception, SdmxStructureType structureEnumType, string agencyId, string id, string version)
            : base(string.Format(CultureInfo.InvariantCulture, "Error with maintainable artefact of type {0} and identifiers {1}:{2}({3})", structureEnumType != null ? structureEnumType.StructureType : null, GetAgencyId(agencyId), GetId(id), GetVersion(version)), exception)
        {
            if (structureEnumType == null)
            {
                throw new ArgumentNullException("structureEnumType");
            }

            this._agencyId = agencyId;
            this._id = id;
            this._version = version;
            this._structureEnumType = structureEnumType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="maintainableObject">
        /// The maintainable object.
        /// </param>
        public MaintainableObjectException(Exception exception, IMaintainableObject maintainableObject)
            : this(
                exception,
                maintainableObject != null ? maintainableObject.StructureType : null, 
                maintainableObject != null ? maintainableObject.AgencyId : null, 
                maintainableObject != null ? maintainableObject.Id : null, 
                maintainableObject != null ? maintainableObject.Version : null
                )
        {
            if (maintainableObject == null)
            {
                throw new ArgumentNullException("maintainableObject");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class. 
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public MaintainableObjectException(string message)
            : base(message)
        {
            this._structureEnumType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class. 
        /// If the <paramref name="innerException"/> is a SdmxException - then the
        /// error code will be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public MaintainableObjectException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            this._structureEnumType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectException"/> class. 
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
        protected MaintainableObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Gets the agency id.
        /// </summary>
        public string AgencyId
        {
            get
            {
                return this._agencyId;
            }
        }

        /// <summary>
        ///     Gets the error type.
        /// </summary>
        public override string ErrorType
        {
            get
            {
                return "Maintainable Object Exception";
            }
        }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        public string Id
        {
            get
            {
                return this._id;
            }
        }

        /// <summary>
        ///     Gets the structure  type.
        /// </summary>
        public SdmxStructureType StructureType
        {
            get
            {
                return this._structureEnumType;
            }
        }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        public string Version
        {
            get
            {
                return this._version;
            }
        }

        /// <summary>
        /// Get the error code
        /// </summary>
        public override SdmxErrorCode SdmxErrorCode
        {
            get
            {
                return SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_agencyId", this._agencyId);
            info.AddValue("_id", this._id);
            info.AddValue("_version", this._version);
            info.AddValue("_structureEnumType", this._structureEnumType);
        }

        /// <summary>
        /// The get agency id.
        /// </summary>
        /// <param name="agencyId0">
        /// The agency id 0.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        private static string GetAgencyId(string agencyId0)
        {
            if (string.IsNullOrEmpty(agencyId0))
            {
                return "agency missing";
            }

            return agencyId0;
        }

        /// <summary>
        /// The get id.
        /// </summary>
        /// <param name="id0">
        /// The id 0.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        private static string GetId(string id0)
        {
            if (string.IsNullOrEmpty(id0))
            {
                return "id missing";
            }

            return id0;
        }

        /// <summary>
        /// Returns the specified <paramref name="version0"/> if it is not null or empty; otherwise <see cref="MaintainableObject.DefaultVersion"/>.
        /// </summary>
        /// <param name="version0">
        /// The version.
        /// </param>
        /// <returns>
        /// The specified <paramref name="version0"/> if it is not null or empty; otherwise <see cref="MaintainableObject.DefaultVersion"/>.
        /// </returns>
        private static string GetVersion(string version0)
        {
            return string.IsNullOrEmpty(version0) ? MaintainableObject.DefaultVersion : version0;
        }

        #endregion
    }
}
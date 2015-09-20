// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The other object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    /// The other object core.
    /// </summary>
    /// <typeparam name="T">
    /// Generic type parameter - IMaintainableObject 
    /// </typeparam>
    /// <typeparam name="TK">
    /// Generic type parameter - IMaintainableMutableObject 
    /// </typeparam>
    [Serializable]
    public abstract class MaintainableObjectCore<T, TK> : NameableCore, IMaintainableObject
        where T : IMaintainableObject where TK : IMaintainableMutableObject
    {
        #region Fields

        /// <summary>
        ///   The agency id.
        /// </summary>
        private string _agencyId;

        /// <summary>
        ///   The end date.
        /// </summary>
        private ISdmxDate _endDate;

        /// <summary>
        ///   The is external reference.
        /// </summary>
        private TertiaryBool _isExternalReference;

        /// <summary>
        ///   The is final.
        /// </summary>
        private TertiaryBool _isFinal;

        /// <summary>
        ///   The start date.
        /// </summary>
        private ISdmxDate _startDate;

        /// <summary>
        ///   The service url.
        /// </summary>
        private Uri _serviceUrl;

        /// <summary>
        ///   The structure url.
        /// </summary>
        private Uri _structureUrl;

        /// <summary>
        ///   The version.
        /// </summary>
        private string _version;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectCore{T,TK}"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The itemMutableObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal MaintainableObjectCore(IMaintainableObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme)
        {
            this._isFinal = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._endDate = agencyScheme.EndDate;
            this._startDate = agencyScheme.StartDate;
            this._version = agencyScheme.Version;
            this._agencyId = agencyScheme.AgencyId;
            this._serviceUrl = agencyScheme.ServiceUrl;
            this._structureUrl = agencyScheme.StructureUrl;
            if (actualLocation == null)
            {
                throw new SdmxSemmanticException(
                    "Stub Objects require a Uri defining the actual service to obtain the full Sdmx artefact from");
            }

            if (isServiceUrl)
            {
                this._serviceUrl = actualLocation;
            }
            else
            {
                this._structureUrl = actualLocation;
            }

            if (agencyScheme.IsFinal != null)
            {
                this._isFinal = agencyScheme.IsFinal;
            }

            if (agencyScheme.IsExternalReference != null)
            {
                this._isExternalReference = agencyScheme.IsExternalReference;
            }

            this.ValidateMaintainableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public MaintainableObjectCore(SdmxStructureType structure, SdmxReader reader) {
        // super(structure, reader, null);
        // Map<string, string> attributes = reader.getAttribtues();
        // this.version = attributes.get("version");
        // this.agencyId = attributes.get("agencyID");
        // if(attributes.containsKey("validFrom")) {
        // this.startDate = new SdmxDateCore(attributes.get("validFrom"));
        // }
        // if(attributes.containsKey("validTo")) {
        // this.endDate = new SdmxDateCore(attributes.get("validTo"));
        // }
        // if(attributes.containsKey("serviceURL")) {
        // setServiceURL(attributes.get("serviceURL"));
        // }
        // if(attributes.containsKey("structureURL")) {
        // setStructureURL(attributes.get("structureURL"));
        // }
        // validateMaintainableAttributes();
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectCore{T,TK}"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject. 
        /// </param>
        protected internal MaintainableObjectCore(IMaintainableMutableObject itemMutableObject)
            : base(itemMutableObject, null)
        {
            this._isFinal = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._agencyId = itemMutableObject.AgencyId;

            // missing fix from 0.9.9
            if (itemMutableObject.StartDate != null)
            {
                this._startDate = new SdmxDateCore(itemMutableObject.StartDate, TimeFormatEnumType.DateTime);
            }

            if (itemMutableObject.EndDate != null)
            {
                this._endDate = new SdmxDateCore(itemMutableObject.EndDate, TimeFormatEnumType.DateTime);
            }

            if (itemMutableObject.FinalStructure != null)
            {
                this._isFinal = itemMutableObject.FinalStructure;
            }

            if (itemMutableObject.ExternalReference != null)
            {
                this._isExternalReference = itemMutableObject.ExternalReference;
            }

            if (itemMutableObject.Stub)
            {
                this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);
            }

            if (itemMutableObject.ServiceURL != null)
            {
                this.ServiceUrl = itemMutableObject.ServiceURL;
            }

            if (itemMutableObject.StructureURL != null)
            {
                this.StructureUrl = itemMutableObject.StructureURL;
            }

            this._version = itemMutableObject.Version;
            this.ValidateMaintainableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectCore{T,TK}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected internal MaintainableObjectCore(MaintainableType createdFrom, SdmxStructureType structureType)
            : base(createdFrom, structureType, null)
        {
            this._isFinal = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._agencyId = createdFrom.agencyID;
            if (createdFrom.validFrom != null)
            {
                this._startDate = new SdmxDateCore(createdFrom.validFrom.Value, TimeFormatEnumType.DateTime);
            }

            if (createdFrom.validTo != null)
            {
                this._endDate = new SdmxDateCore(createdFrom.validTo.Value, TimeFormatEnumType.DateTime);
            }

            this._isFinal = CreateTertiary(createdFrom.isFinal, createdFrom.isFinal);
            this._version = createdFrom.version;
            this._isExternalReference = CreateTertiary(createdFrom.isExternalReference, createdFrom.isExternalReference);
            this.ServiceUrl = createdFrom.serviceURL;
            this.StructureUrl = createdFrom.structureURL;
            this.ValidateMaintainableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectCore{T,TK}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="endDate0">
        /// The end date 0. 
        /// </param>
        /// <param name="startDate1">
        /// The start date 1. 
        /// </param>
        /// <param name="version2">
        /// The version 2. 
        /// </param>
        /// <param name="isFinal3">
        /// The is final 3. 
        /// </param>
        /// <param name="agencyId4">
        /// The agency id 4. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="description">
        /// The description. 
        /// </param>
        /// <param name="isExternalReference">
        /// The is external reference. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal MaintainableObjectCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            object endDate0, 
            object startDate1, 
            string version2, 
            TertiaryBool isFinal3, 
            string agencyId4, 
            string id, 
            Uri uri, 
            IList<TextType> name, 
            IList<TextType> description, 
            TertiaryBool isExternalReference, 
            AnnotationsType annotationsType)
            : base(createdFrom, structureType, id, uri, name, description, annotationsType, null)
        {
            this._isFinal = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._agencyId = agencyId4;
            if (startDate1 != null)
            {
                var norm = DateUtil.FormatDate(startDate1);
                this._startDate = new SdmxDateCore(norm, TimeFormatEnumType.DateTime);
            }

            if (endDate0 != null)
            {
                var norm = DateUtil.FormatDate(endDate0);
                this._endDate = new SdmxDateCore(norm, TimeFormatEnumType.DateTime);
            }

            if (isFinal3 != null)
            {
                this._isFinal = isFinal3;
            }

            this._version = version2;
            if (isExternalReference != null)
            {
                this._isExternalReference = isExternalReference;
            }

            if (this._isExternalReference.EnumType == TertiaryBoolEnumType.True)
            {
                this.StructureUrl = uri;
            }

            this.ValidateMaintainableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableObjectCore{T,TK}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="version0">
        /// The version 0. 
        /// </param>
        /// <param name="agencyId1">
        /// The agency id 1. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="isExternalReference2">
        /// The is external reference 2. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal MaintainableObjectCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string version0, 
            string agencyId1, 
            string id, 
            Uri uri, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> name, 
            TertiaryBool isExternalReference2, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType)
            : base(createdFrom, structureType, id, uri, name, null, annotationsType, null)
        {
            this._isFinal = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._agencyId = agencyId1;
            this._version = version0;
            this._isExternalReference = isExternalReference2;
            if (this._isExternalReference.EnumType == TertiaryBoolEnumType.True)
            {
                this.StructureUrl = uri;
            }

            this.ValidateMaintainableAttributes();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets or sets the agency id.
        /// </summary>
        public virtual string AgencyId
        {
            get
            {
                return this._agencyId;
            }

            set
            {
                this._agencyId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the agency id.
        /// </summary>
        public string BaseAgencyId
        {
            get
            {
                return this._agencyId;
            }

            set
            {
                this._agencyId = value;
            }
        }

        ///// <summary>
        /////   Gets the composites.
        ///// </summary>
        //public override ISet<ISdmxObject> Composites
        //{
        //    get
        //    {
        //        PropertyInfo[] arr1 = typeof(IMaintainableObject).GetProperties();
        //        PropertyInfo[] arr2 = typeof(ISdmxObject).GetProperties();
        //        var arrMerge = new PropertyInfo[arr1.Length + arr2.Length];
        //        int i = 0;
        //        foreach (PropertyInfo m in arr1)
        //        {
        //            arrMerge[i] = m;
        //            i++;
        //        }

        //        foreach (PropertyInfo m0 in arr2)
        //        {
        //            arrMerge[i] = m0;
        //            i++;
        //        }
                
        //        this.GenerateSdmxObjectComposites(arrMerge);
        //        return new HashSet<ISdmxObject>(this.CompositesInternal);
        //    }
        //}

        /// <summary>
        ///   Gets or sets the end date.
        /// </summary>
        public ISdmxDate EndDate
        {
            get
            {
                return this._endDate;
            }

            set
            {
                this._endDate = value;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public abstract TK MutableInstance { get; }

        /// <summary>
        ///   Gets the service url.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        public Uri ServiceUrl
        {
            get
            {
                return this._serviceUrl;
            }

            private set
            {
                if (value == null)
                {
                    this._serviceUrl = null;
                    return;
                }

                try
                {
                    this._serviceUrl = value;
                }
                catch (Exception th)
                {
                    throw new SdmxSemmanticException("Could not create attribute 'serviceURL' with value '" + value + "'", th);
                }
            }
        }

        /// <summary>
        ///   Gets or sets the start date.
        /// </summary>
        public ISdmxDate StartDate
        {
            get
            {
                return this._startDate;
            }

            set
            {
                this._startDate = value;
            }
        }

        /// <summary>
        ///   Gets the structure url.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        public Uri StructureUrl
        {
            get
            {
                return this._structureUrl;
            }

            private set
            {
                if (value == null)
                {
                    this._structureUrl = null;
                    return;
                }

                try
                {
                    this._structureUrl = value;
                }
                catch (Exception th)
                {
                    throw new SdmxSemmanticException("Could not create attribute 'structureURL' with value '" + value + "'", th);
                }
            }
        }

        /// <summary>
        ///   Gets the version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return this._version;
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        IMaintainableMutableObject IMaintainableObject.MutableInstance
        {
            get
            {
                return this.MutableInstance;
            }
        }

        /// <summary>
        ///   Gets or sets is final.
        /// </summary>
        public virtual TertiaryBool IsFinal
        {
            get
            {
                return this._isFinal;
            }

            set
            {
                this._isFinal = value;
            }
        }


        /// <summary>
        ///   Gets the is external reference.
        /// </summary>
        /// <returns> The <see cref="TertiaryBool" /> . </returns>
        public TertiaryBool IsExternalReference
        {
            get
            {
                return this._isExternalReference;
            }
        }

        #endregion

        /*public IMaintainableObject GetStub(Uri actualLocation, bool isServiceUrl);
        public IMaintainableMutableObject MutableInstance { get; }*/
        #region Public Methods and Operators

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        public virtual int CompareTo(IMaintainableObject other)
        {
            return VersionableUtil.IsHigherVersion(this._version, other.Version) ? +1 : -1;
        }

        /// <summary>
        /// from org.sdmxsource.sdmx.api.model.sdmxObjects.base.IMaintainableObject
        /// </summary>
        /// <param name="actualLocation">
        /// The actual Location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is Service Uri. 
        /// </param>
        /// <summary>
        /// from org.sdmxsource.sdmx.api.model.sdmxObjects.base.IMaintainableObject
        /// </summary>
        /// <returns>
        /// The <see cref="T"/> . 
        /// </returns>
        public abstract T GetStub(Uri actualLocation, bool isServiceUrl);

       #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> . 
        /// </returns>
        IMaintainableObject IMaintainableObject.GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return this.GetStub(actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform a deep equal comparison against <paramref name="maintainableObject"/>
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable object to compare against
        /// </param>
        /// <param name="includeFinalProperties">
        /// Set to true to compare final properties. 
        /// These are <see cref="StartDate"/>, <see cref="EndDate"/>, <see cref="IsExternalReference"/>, <see cref="ServiceUrl"/> 
        /// and <see cref="StructureUrl"/>. Otherwise those are ignored.
        /// </param>
        /// <returns>
        /// True if the <paramref name="maintainableObject"/> deep equals this instance; otherwise false
        /// </returns>
        protected internal bool DeepEqualsInternal(IMaintainableObject maintainableObject, bool includeFinalProperties)
        {
            if (maintainableObject == null)
            {
                return false;
            }

            if (includeFinalProperties)
            {
                if (!ObjectUtil.Equivalent(this._startDate, maintainableObject.StartDate))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._endDate, maintainableObject.EndDate))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._isExternalReference.IsTrue, maintainableObject.IsExternalReference.IsTrue))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._serviceUrl, maintainableObject.ServiceUrl))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._structureUrl, maintainableObject.StructureUrl))
                {
                    return false;
                }
            }

            if (!ObjectUtil.Equivalent(this._agencyId, maintainableObject.AgencyId))
            {
                return false;
            }

            if (!ObjectUtil.Equivalent(this._isFinal.IsTrue, maintainableObject.IsFinal.IsTrue))
            {
                return false;
            }

            return this.DeepEqualsNameable(maintainableObject, includeFinalProperties);
        }

        /// <summary>
        ///   The validate agency id.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal virtual void ValidateAgencyId()
        {
            if (string.IsNullOrWhiteSpace(this.AgencyId))
            {
                throw new SdmxSemmanticException(ExceptionCode.StructureMaintainableMissingAgency, this.StructureType);
            }
        }

        /// <summary>
        ///   The validate other attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void ValidateMaintainableAttributes()
        {
            this.ValidateAgencyId();
            if (string.IsNullOrWhiteSpace(this._version))
            {
                this._version = MaintainableObject.DefaultVersion;
            }

            if (!VersionableUtil.ValidVersion(this._version))
            {
                throw new SdmxSemmanticException(ExceptionCode.StructureInvalidVersion, this._version);
            }

            if (this.EndDate != null && this._startDate != null)
            {
                if (this._startDate.IsLater(this.EndDate))
                {
                    throw new SdmxSemmanticException(ExceptionCode.EndDateBeforeStartDate);
                }
            }

            if (this._isExternalReference == null)
            {
                this._isExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            }

            if (this._isExternalReference.IsTrue)
            {
                if (this._structureUrl == null && this._serviceUrl == null)
                {
                    throw new SdmxSemmanticException(ExceptionCode.ExternalStructureMissingUri);
                }
            }

            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                if (this.Id.Equals("ALL"))
                {
                    throw new SdmxSemmanticException("ALL is a reserved word and can not be used for an id");
                }
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The maintainable mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    /// The maintainable mutable core.
    /// </summary>
    /// <typeparam name="T">Generic type param of IMaintainableObject type
    /// </typeparam>
    [Serializable]
    public abstract class MaintainableMutableCore<T> : NameableMutableCore, IMaintainableMutableObject
        where T : IMaintainableObject
    {
        #region Fields

        /// <summary>
        ///   The _agency id.
        /// </summary>
        private string _agencyId;

        /// <summary>
        ///   The _end date.
        /// </summary>
        private DateTime? _endDate;

        /// <summary>
        ///   The _is external reference.
        /// </summary>
        private TertiaryBool _isExternalReference;

        /// <summary>
        ///   The _is final structure.
        /// </summary>
        private TertiaryBool _isFinalStructure;

        /// <summary>
        ///   The _is stub.
        /// </summary>
        private bool _isStub;

        /// <summary>
        ///   The _service url.
        /// </summary>
        private Uri _serviceUrl;

        /// <summary>
        ///   The _start date.
        /// </summary>
        private DateTime? _startDate;

        /// <summary>
        ///   The _structure url.
        /// </summary>
        private Uri _structureUrl;

        /// <summary>
        ///   The _version.
        /// </summary>
        private string _version;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableMutableCore{T}"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected MaintainableMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableMutableCore{T}"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        protected MaintainableMutableCore(IMaintainableObject objTarget)
            : base(objTarget)
        {

            this._agencyId = objTarget.AgencyId;
            if (objTarget.StartDate != null)
            {
                this._startDate = objTarget.StartDate.Date;
            }

            if (objTarget.EndDate != null)
            {
                this._endDate = objTarget.EndDate.Date;
            }

            if (objTarget.ServiceUrl != null)
            {
                this._serviceUrl = objTarget.ServiceUrl;
            }

            if (objTarget.StructureUrl != null)
            {
                this._structureUrl = objTarget.StructureUrl;
            }

            this._isFinalStructure = objTarget.IsFinal;
            this._version = objTarget.Version;
            this._isExternalReference = objTarget.IsExternalReference;
        }

        #endregion

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
        ///   Gets or sets the end date.
        /// </summary>
        public virtual DateTime? EndDate
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
        ///   Gets or sets the external reference.
        /// </summary>
        public virtual TertiaryBool ExternalReference
        {
            get
            {
                return this._isExternalReference;
            }

            set
            {
                this._isExternalReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets the final structure.
        /// </summary>
        public virtual TertiaryBool FinalStructure
        {
            get
            {
                return this._isFinalStructure;
            }

            set
            {
                this._isFinalStructure = value;
            }
        }

        /// <summary>
        ///   Gets ImmutableInstance
        /// </summary>
        public abstract T ImmutableInstance { get; }

        /// <summary>
        ///   Gets or sets the service url.
        /// </summary>
        public virtual Uri ServiceURL
        {
            get
            {
                return this._serviceUrl;
            }

            set
            {
                this._serviceUrl = value;
            }
        }

        /// <summary>
        ///   Gets or sets the start date.
        /// </summary>
        public virtual DateTime? StartDate
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
        ///   Gets or sets the structure url.
        /// </summary>
        public virtual Uri StructureURL
        {
            get
            {
                return this._structureUrl;
            }

            set
            {
                this._structureUrl = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether stub.
        /// </summary>
        public virtual bool Stub
        {
            get
            {
                return this._isStub;
            }

            set
            {
                this._isStub = value;
            }
        }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// ///
        /// <exception cref="ArgumentException">Throws ArgumentException.</exception>
        public virtual string Version
        {
            get
            {
                return this._version;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "1.0";
                }

                if (!VersionableUtil.ValidVersion(value))
                {
                    throw new ArgumentException("Version invalid : " + value);
                }

                this._version = VersionableUtil.FormatVersion(value);
            }
        }

        #endregion

        // TODO Test this method orders a list index 0 higher version then a list with index 1
        #region Explicit Interface Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        IMaintainableObject IMaintainableMutableObject.ImmutableInstance
        {
            get
            {
                return this.ImmutableInstance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable dataStructureObject. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        public int CompareTo(IMaintainableObject maintainableObject)
        {
            return VersionableUtil.IsHigherVersion(this._version, maintainableObject.Version) ? -1 : +1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build maintainable attributes.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        protected internal void BuildMaintainableAttributes(ISdmxReader reader)
        {
            this.BuildIdentifiableAttributes(reader);
            IDictionary<string, string> attributes = reader.Attributes;
            this._version = attributes["version"];
            this._agencyId = attributes["agencyID"];

            if (attributes.ContainsKey("validFrom"))
            {
                this._startDate = new SdmxDateCore(attributes["validFrom"]).Date;
            }

            if (attributes.ContainsKey("validTo"))
            {
                this._endDate = new SdmxDateCore(attributes["validTo"]).Date;
            }

            if (attributes.ContainsKey("serviceURL"))
            {
                this.ServiceURL = new Uri(attributes["serviceURL"]);
            }

            if (attributes.ContainsKey("structureURL"))
            {
                this.StructureURL = new Uri(attributes["structureURL"]);
            }
        }

        #endregion
    }
}
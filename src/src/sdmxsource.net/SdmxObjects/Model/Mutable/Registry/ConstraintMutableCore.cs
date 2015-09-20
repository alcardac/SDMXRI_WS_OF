// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The constraint mutable core.
    /// </summary>
    [Serializable]
    public abstract class ConstraintMutableCore<T> : MaintainableMutableCore<T>,
                                                  IConstraintMutableObject 
        where T : IConstraintObject
    {
        #region Fields

        /// <summary>
        ///   The _content constraint attachment.
        /// </summary>
        private IConstraintAttachmentMutableObject _contentConstraintAttachment;

        /// <summary>
        ///   The _excluded series keys.
        /// </summary>
        private IConstraintDataKeySetMutableObject _excludedSeriesKeys;

        /// <summary>
        ///   The _included series keys.
        /// </summary>
        private IConstraintDataKeySetMutableObject _includedSeriesKeys;

        private IConstraintDataKeySetMutableObject _includedMetadataKeys;
        private IConstraintDataKeySetMutableObject _excludedMetadataKeys;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public ConstraintMutableCore(IConstraintObject objTarget)
            : base(objTarget)
        {
            this._contentConstraintAttachment = null;
            if (objTarget.ConstraintAttachment != null)
            {
                this._contentConstraintAttachment = objTarget.ConstraintAttachment.CreateMutableInstance();
            }

            if (objTarget.IncludedSeriesKeys != null)
            {
                this._includedSeriesKeys = new ConstraintDataKeySetMutableCore(objTarget.IncludedSeriesKeys);
            }

            if (objTarget.ExcludedSeriesKeys != null)
            {
                this._excludedSeriesKeys = new ConstraintDataKeySetMutableCore(objTarget.ExcludedSeriesKeys);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public ConstraintMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
            this._contentConstraintAttachment = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the constraint attachment.
        /// </summary>
        public virtual IConstraintAttachmentMutableObject ConstraintAttachment
        {
            get
            {
                return this._contentConstraintAttachment;
            }

            set
            {
                this._contentConstraintAttachment = value;
            }
        }

        /// <summary>
        ///   Gets or sets the excluded series keys.
        /// </summary>
        public virtual IConstraintDataKeySetMutableObject ExcludedSeriesKeys
        {
            get
            {
                return this._excludedSeriesKeys;
            }

            set
            {
                this._excludedSeriesKeys = value;
            }
        }

        /// <summary>
        ///   Gets or sets the included series keys.
        /// </summary>
        public virtual IConstraintDataKeySetMutableObject IncludedSeriesKeys
        {
            get
            {
                return this._includedSeriesKeys;
            }

            set
            {
                this._includedSeriesKeys = value;
            }
        }

        public IConstraintDataKeySetMutableObject IncludedMetadataKeys
        {
            get
            {
                return this._includedMetadataKeys;
            }
            set
            {
                this._includedMetadataKeys = value;
            }
        }

        public IConstraintDataKeySetMutableObject ExcludedMetadataKeys
        {
            get
            {
                return this._excludedMetadataKeys;
            }
            set
            {
                this._excludedMetadataKeys = value;
            }
        }

        #endregion
    }
}
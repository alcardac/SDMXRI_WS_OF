// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentConstraintMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The content constraint mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;

    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///   The content constraint mutable core.
    /// </summary>
    [Serializable]
    public class ContentConstraintMutableCore : ConstraintMutableCore<IContentConstraintObject>, IContentConstraintMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _excluded cube region.
        /// </summary>
        private ICubeRegionMutableObject _excludedCubeRegion;

        /// <summary>
        ///   The _included cube region.
        /// </summary>
        private ICubeRegionMutableObject _includedCubeRegion;

        /// <summary>
        ///   The _is defining actual data present.
        /// </summary>
        private bool _isDefiningActualDataPresent; // Default Value

        /// <summary>
        ///   The _reference period.
        /// </summary>
        private IReferencePeriodMutableObject _referencePeriod;

        /// <summary>
        ///   The _release calendar.
        /// </summary>
        private IReleaseCalendarMutableObject _releaseCalendar;

        private IMetadataTargetRegionMutableObject _metadataTargetRegionBean;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContentConstraintMutableCore" /> class.
        /// </summary>
        public ContentConstraintMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint))
        {
            this._referencePeriod = null;
            this._releaseCalendar = null;
            this._isDefiningActualDataPresent = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentConstraintMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ContentConstraintMutableCore(IContentConstraintObject objTarget)
            : base(objTarget)
        {
            this._referencePeriod = null;
            this._releaseCalendar = null;
            this._isDefiningActualDataPresent = true;
            if (objTarget.IncludedCubeRegion != null)
            {
                this._includedCubeRegion = new CubeRegionMutableCore(objTarget.IncludedCubeRegion);
            }

            if (objTarget.ExcludedCubeRegion != null)
            {
                this._excludedCubeRegion = new CubeRegionMutableCore(objTarget.ExcludedCubeRegion);
            }

            if (objTarget.ReferencePeriod != null)
            {
                this._referencePeriod = objTarget.ReferencePeriod.CreateMutableObject();
            }

            if (objTarget.ReleaseCalendar != null)
            {
                this._releaseCalendar = objTarget.ReleaseCalendar.CreateMutableObject();
            }
            if (objTarget.MetadataTargetRegion != null) 
                this._metadataTargetRegionBean = new MetadataTargetRegionMutableObjectCore(objTarget.MetadataTargetRegion);

            this._isDefiningActualDataPresent = objTarget.IsDefiningActualDataPresent;
        }

        #endregion

        #region Public Properties

        public IMetadataTargetRegionMutableObject MetadataTargetRegion
        {
            get
            {
                return _metadataTargetRegionBean;
            }
            set
            {
                _metadataTargetRegionBean = value;
            }
        }

        /// <summary>
        ///   Gets or sets the excluded cube region.
        /// </summary>
        public virtual ICubeRegionMutableObject ExcludedCubeRegion
        {
            get
            {
                return this._excludedCubeRegion;
            }

            set
            {
                this._excludedCubeRegion = value;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IContentConstraintObject ImmutableInstance
        {
            get
            {
                return new ContentConstraintObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the included cube region.
        /// </summary>
        public virtual ICubeRegionMutableObject IncludedCubeRegion
        {
            get
            {
                return this._includedCubeRegion;
            }

            set
            {
                this._includedCubeRegion = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether is defining actual data present.
        /// </summary>
        public virtual bool IsDefiningActualDataPresent
        {
            get
            {
                return this._isDefiningActualDataPresent;
            }

            set
            {
                this._isDefiningActualDataPresent = value;
            }
        }

        /// <summary>
        ///   Gets or sets the reference period.
        /// </summary>
        public virtual IReferencePeriodMutableObject ReferencePeriod
        {
            get
            {
                return this._referencePeriod;
            }

            set
            {
                this._referencePeriod = value;
            }
        }

        /// <summary>
        ///   Gets or sets the release calendar.
        /// </summary>
        public virtual IReleaseCalendarMutableObject ReleaseCalendar
        {
            get
            {
                return this._releaseCalendar;
            }

            set
            {
                this._releaseCalendar = value;
            }
        }

        #endregion
    }

    
}
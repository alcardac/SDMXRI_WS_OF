// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeRangeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time range mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///   The time range mutable core.
    /// </summary>
    [Serializable]
    public class TimeRangeMutableCore : MutableCore, ITimeRangeMutableObject
    {
        #region Fields

        /// <summary>
        ///   The end date.
        /// </summary>
        private DateTime? endDate;

        /// <summary>
        ///   The is end inclusive.
        /// </summary>
        private bool isEndInclusive;

        /// <summary>
        ///   The is range.
        /// </summary>
        private bool _isIsRange;

        /// <summary>
        ///   The is start inclusive.
        /// </summary>
        private bool isStartInclusive;

        /// <summary>
        ///   The start date.
        /// </summary>
        private DateTime? startDate;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        public TimeRangeMutableCore() :
            base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeRange))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRangeMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public TimeRangeMutableCore(ITimeRange immutable)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeRange))
        {
            if (immutable.StartDate != null)
            {
                this.startDate = immutable.StartDate.Date;
            }

            if (immutable.EndDate != null)
            {
                this.endDate = immutable.EndDate.Date;
            }

            this._isIsRange = immutable.Range;
            this.isStartInclusive = immutable.StartInclusive;
            this.isEndInclusive = immutable.EndInclusive;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRangeMutableCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public TimeRangeMutableCore(TimeRangeValueType type)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeRange))
        {
            if (type.AfterPeriod != null)
            {
                this._isIsRange = false;

                // FUNC 2.1 ObservationalTimePeriodType - does this work?
                this.endDate = DateUtil.FormatDate(type.AfterPeriod.TypedValue, true);
                this.isEndInclusive = type.AfterPeriod.isInclusive;
            }

            if (type.BeforePeriod != null)
            {
                this._isIsRange = false;
                this.startDate = DateUtil.FormatDate(type.BeforePeriod.TypedValue, true);
                this.isStartInclusive = type.BeforePeriod.isInclusive;
            }

            if (type.StartPeriod != null)
            {
                this._isIsRange = true;
                this.startDate = DateUtil.FormatDate(type.StartPeriod.TypedValue, true);
                this.isStartInclusive = type.StartPeriod.isInclusive;
            }

            if (type.EndPeriod != null)
            {
                this._isIsRange = true;
                this.startDate = DateUtil.FormatDate(type.EndPeriod.TypedValue, true);
                this.isEndInclusive = type.EndPeriod.isInclusive;
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets or sets the end date.
        /// </summary>
        public virtual DateTime? EndDate
        {
            get
            {
                if (this.endDate != null)
                {
                    return new DateTime((this.endDate.Value.Ticks / 10000) * 10000);
                }

                return null;
            }

            set
            {
                this.endDate = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether is end inclusive.
        /// </summary>
        public virtual bool IsEndInclusive
        {
            get
            {
                return this.isEndInclusive;
            }

            set
            {
                this.isEndInclusive = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether is start inclusive.
        /// </summary>
        public virtual bool IsStartInclusive
        {
            get
            {
                return this.isStartInclusive;
            }

            set
            {
                this.isStartInclusive = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether range.
        /// </summary>
        public virtual bool IsRange
        {
            get
            {
                return this._isIsRange;
            }

            set
            {
                this._isIsRange = value;
            }
        }

        /// <summary>
        ///   Gets or sets the start date.
        /// </summary>
        public virtual DateTime? StartDate
        {
            get
            {
                if (this.startDate != null)
                {
                    return new DateTime((this.startDate.Value.Ticks / 10000) * 10000);
                }

                return null;
            }

            set
            {
                this.startDate = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create immutable instance.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="ITimeRange"/> . 
        /// </returns>
        public virtual ITimeRange CreateImmutableInstance(ISdmxStructure parent)
        {
            return new TimeRangeCore(this, parent);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.startDate == null && this.endDate == null)
            {
                throw new SdmxSemmanticException("Time period must define at least one date");
            }

            if (this._isIsRange)
            {
                if (this.startDate == null || this.endDate == null)
                {
                    throw new SdmxSemmanticException("Time period with a range requires both a start and end period");
                }

                if ((this.startDate.Value.Ticks / 10000) > (this.endDate.Value.Ticks / 10000))
                {
                    throw new SdmxSemmanticException("Time range can not specify start period after end period");
                }
            }
            else
            {
                if (this.startDate != null && this.endDate != null)
                {
                    throw new SdmxSemmanticException("Time period can not define both a before period and after period");
                }
            }
        }

        #endregion
    }
}
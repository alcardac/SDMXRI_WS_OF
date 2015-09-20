// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeRangeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time range core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The time range core.
    /// </summary>
    [Serializable]
    public class TimeRangeCore : SdmxStructureCore, ITimeRange
    {
        #region Fields

        /// <summary>
        ///   The _end date.
        /// </summary>
        private readonly ISdmxDate _endDate;

        /// <summary>
        ///   The _is end inclusive.
        /// </summary>
        private readonly bool _isEndInclusive;

        /// <summary>
        ///   The _is range.
        /// </summary>
        private readonly bool _isRange;

        /// <summary>
        ///   The _is start inclusive.
        /// </summary>
        private readonly bool _isStartInclusive;

        /// <summary>
        ///   The _start date.
        /// </summary>
        private readonly ISdmxDate _startDate;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRangeCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TimeRangeCore(ITimeRangeMutableObject mutable, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeRange), parent)
        {
            if (mutable.StartDate != null)
            {
                this._startDate = new SdmxDateCore(mutable.StartDate, TimeFormatEnumType.DateTime);
            }

            if (mutable.EndDate != null)
            {
                this._endDate = new SdmxDateCore(mutable.EndDate, TimeFormatEnumType.DateTime);
            }

            this._isRange = mutable.IsRange;
            this._isStartInclusive = mutable.IsStartInclusive;
            this._isEndInclusive = mutable.IsEndInclusive;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRangeCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TimeRangeCore(TimeRangeValueType type, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeRange), parent)
        {
            if (type.AfterPeriod != null)
            {
                this._isRange = false;

                // FUNC 2.1 ObservationalTimePeriodType - does this work?
                this._endDate = new SdmxDateCore(type.AfterPeriod.TypedValue.ToString());
                this._isEndInclusive = type.AfterPeriod.isInclusive;
            }

            if (type.BeforePeriod != null)
            {
                this._isRange = false;
                this._startDate = new SdmxDateCore(type.BeforePeriod.TypedValue.ToString());
                this._isStartInclusive = type.BeforePeriod.isInclusive;
            }

            if (type.StartPeriod != null)
            {
                this._isRange = true;
                this._startDate = new SdmxDateCore(type.StartPeriod.TypedValue.ToString());
                this._isStartInclusive = type.StartPeriod.isInclusive;
            }

            if (type.EndPeriod != null)
            {
                this._isRange = true;
                this._endDate = new SdmxDateCore(type.EndPeriod.TypedValue.ToString());
                this._isEndInclusive = type.EndPeriod.isInclusive;
            }

            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the end date.
        /// </summary>
        public virtual ISdmxDate EndDate
        {
            get
            {
                return this._endDate;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether end inclusive.
        /// </summary>
        public virtual bool EndInclusive
        {
            get
            {
                return this._isEndInclusive;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets a value indicating whether range.
        /// </summary>
        public virtual bool Range
        {
            get
            {
                return this._isRange;
            }
        }

        /// <summary>
        ///   Gets the start date.
        /// </summary>
        public virtual ISdmxDate StartDate
        {
            get
            {
                return this._startDate;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether start inclusive.
        /// </summary>
        public virtual bool StartInclusive
        {
            get
            {
                return this._isStartInclusive;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ITimeRange)sdmxObject;
                if (!ObjectUtil.Equivalent(this._startDate, that.StartDate))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._endDate, that.EndDate))
                {
                    return false;
                }

                if (this._isRange != that.Range)
                {
                    return false;
                }

                if (this._isStartInclusive != that.StartInclusive)
                {
                    return false;
                }

                if (this._isEndInclusive != that.EndInclusive)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this._startDate == null && this._endDate == null)
            {
                throw new SdmxSemmanticException("Time period must define at least one date");
            }

            if (this._isRange)
            {
                if (this._startDate == null || this._endDate == null)
                {
                    throw new SdmxSemmanticException("Time period with a range requires both a start and end period");
                }

                if (this._startDate.IsLater(this._endDate))
                {
                    throw new SdmxSemmanticException("Time range can not specify start period after end period");
                }
            }
            else
            {
                if (this._startDate != null && this._endDate != null)
                {
                    throw new SdmxSemmanticException("Time period can not define both a before period and after period");
                }
            }
        }

        #endregion
    }
}
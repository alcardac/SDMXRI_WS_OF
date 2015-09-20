// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferencePeriodMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reference period mutable core.
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
    ///   The reference period mutable core.
    /// </summary>
    [Serializable]
    public class ReferencePeriodMutableCore : MutableCore, IReferencePeriodMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _end time.
        /// </summary>
        private DateTime? _endTime;

        /// <summary>
        ///   The _start time.
        /// </summary>
        private DateTime? _startTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReferencePeriodMutableCore" /> class.
        /// </summary>
        public ReferencePeriodMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReferencePeriod))
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencePeriodMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public ReferencePeriodMutableCore(IReferencePeriod immutable)
            : base(immutable)
        {
            if (immutable.StartTime != null)
            {
                this._startTime = immutable.StartTime.Date;
            }

            if (immutable.EndTime != null)
            {
                this._endTime = immutable.EndTime.Date;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the end time.
        /// </summary>
        public virtual DateTime? EndTime
        {
            get
            {
                return this._endTime;
            }

            set
            {
                this._endTime = value;
            }
        }

        /// <summary>
        ///   Gets or sets the start time.
        /// </summary>
        public virtual DateTime? StartTime
        {
            get
            {
                return this._startTime;
            }

            set
            {
                this._startTime = value;
            }
        }

        #endregion
    }
}
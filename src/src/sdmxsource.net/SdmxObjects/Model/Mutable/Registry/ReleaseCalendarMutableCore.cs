// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseCalendarMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The release calendar mutable core.
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
    ///   The release calendar mutable core.
    /// </summary>
    [Serializable]
    public class ReleaseCalendarMutableCore : MutableCore, IReleaseCalendarMutableObject
    {
        #region Fields

        /// <summary>
        ///   The offset.
        /// </summary>
        private string offset;

        /// <summary>
        ///   The periodicity.
        /// </summary>
        private string periodicity;

        /// <summary>
        ///   The tolerance.
        /// </summary>
        private string tolerance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReleaseCalendarMutableCore" /> class.
        /// </summary>
        public ReleaseCalendarMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReleaseCalendar))
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseCalendarMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public ReleaseCalendarMutableCore(IReleaseCalendar immutable)
            : base(immutable)
        {
            this.periodicity = immutable.Periodicity;
            this.offset = immutable.Offset;
            this.tolerance = immutable.Tolerance;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the offset.
        /// </summary>
        public virtual string Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                this.offset = value;
            }
        }

        /// <summary>
        ///   Gets or sets the periodicity.
        /// </summary>
        public virtual string Periodicity
        {
            get
            {
                return this.periodicity;
            }

            set
            {
                this.periodicity = value;
            }
        }

        /// <summary>
        ///   Gets or sets the tolerance.
        /// </summary>
        public virtual string Tolerance
        {
            get
            {
                return this.tolerance;
            }

            set
            {
                this.tolerance = value;
            }
        }

        #endregion
    }
}
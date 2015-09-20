// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseCalendarBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The release calendar core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The release calendar core.
    /// </summary>
    [Serializable]
    public class ReleaseCalendarCore : SdmxStructureCore, IReleaseCalendar
    {
        #region Fields

        /// <summary>
        ///   The offset.
        /// </summary>
        private readonly string offset;

        /// <summary>
        ///   The periodicity.
        /// </summary>
        private readonly string periodicity;

        /// <summary>
        ///   The tolerance.
        /// </summary>
        private readonly string tolerance;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseCalendarCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ReleaseCalendarCore(IReleaseCalendarMutableObject mutable, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReleaseCalendar), parent)
        {
            if (mutable != null)
            {
                this.offset = mutable.Offset;
                this.periodicity = mutable.Periodicity;
                this.tolerance = mutable.Tolerance;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseCalendarCore"/> class.
        /// </summary>
        /// <param name="releaseCalendarType">
        /// The release calendar type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ReleaseCalendarCore(ReleaseCalendarType releaseCalendarType, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReleaseCalendar), parent)
        {
            this.offset = releaseCalendarType.Offset;
            this.periodicity = releaseCalendarType.Periodicity;
            this.tolerance = releaseCalendarType.Tolerance;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the offset.
        /// </summary>
        public virtual string Offset
        {
            get
            {
                return this.offset;
            }
        }

        /// <summary>
        ///   Gets the periodicity.
        /// </summary>
        public virtual string Periodicity
        {
            get
            {
                return this.periodicity;
            }
        }

        /// <summary>
        ///   Gets the tolerance.
        /// </summary>
        public virtual string Tolerance
        {
            get
            {
                return this.tolerance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create mutable object.
        /// </summary>
        /// <returns> The <see cref="IReleaseCalendarMutableObject" /> . </returns>
        public virtual IReleaseCalendarMutableObject CreateMutableObject()
        {
            return new ReleaseCalendarMutableCore(this);
        }

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
                var that = (IReleaseCalendar)sdmxObject;
                if (!ObjectUtil.Equivalent(this.offset, that.Offset))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.periodicity, that.Periodicity))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.tolerance, that.Tolerance))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
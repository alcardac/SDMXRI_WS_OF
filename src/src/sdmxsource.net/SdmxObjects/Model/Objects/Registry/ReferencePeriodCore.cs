// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferencePeriodBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reference period core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The reference period core.
    /// </summary>
    [Serializable]
    public class ReferencePeriodCore : SdmxStructureCore, IReferencePeriod
    {
        #region Fields

        /// <summary>
        ///   The end time.
        /// </summary>
        private readonly ISdmxDate endTime;

        /// <summary>
        ///   The start time.
        /// </summary>
        private readonly ISdmxDate startTime;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencePeriodCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReferencePeriodCore(IReferencePeriodMutableObject mutable, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReferencePeriod), parent)
        {
            // These items are mandatory and thus should exist
            if (mutable.StartTime != null)
            {
                this.startTime = new SdmxDateCore(mutable.StartTime, TimeFormatEnumType.DateTime);
            }

            if (mutable.EndTime != null)
            {
                this.endTime = new SdmxDateCore(mutable.EndTime, TimeFormatEnumType.DateTime);
            }

            if (this.startTime == null)
            {
                throw new SdmxSemmanticException("ReferencePeriodCore - start time can not be null");
            }

            if (this.endTime == null)
            {
                throw new SdmxSemmanticException("ReferencePeriodCore - end time can not be null");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencePeriodCore"/> class.
        /// </summary>
        /// <param name="refPeriodType">
        /// The ref period type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReferencePeriodCore(ReferencePeriodType refPeriodType, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReferencePeriod), parent)
        {
            this.startTime = new SdmxDateCore(refPeriodType.startTime, TimeFormatEnumType.DateTime);
            this.endTime = new SdmxDateCore(refPeriodType.endTime, TimeFormatEnumType.DateTime);
            if (this.startTime == null)
            {
                throw new SdmxSemmanticException("ReferencePeriodCore - start time can not be null");
            }

            if (this.endTime == null)
            {
                throw new SdmxSemmanticException("ReferencePeriodCore - start time can not be null");
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the end time.
        /// </summary>
        public virtual ISdmxDate EndTime
        {
            get
            {
                return this.endTime;
            }
        }

        /// <summary>
        ///   Gets the start time.
        /// </summary>
        public virtual ISdmxDate StartTime
        {
            get
            {
                return this.startTime;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create mutable object.
        /// </summary>
        /// <returns> The <see cref="IReferencePeriodMutableObject" /> . </returns>
        public virtual IReferencePeriodMutableObject CreateMutableObject()
        {
            return new ReferencePeriodMutableCore(this);
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
                var that = (IReferencePeriod)sdmxObject;
                if (!ObjectUtil.Equivalent(this.startTime, that.StartTime))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.endTime, that.EndTime))
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
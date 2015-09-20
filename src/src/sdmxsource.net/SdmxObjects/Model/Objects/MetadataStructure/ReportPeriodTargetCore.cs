// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportPeriodTargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The report period target core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;

    using ReportPeriodTarget = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.ReportPeriodTarget;

    /// <summary>
    ///   The report period target core.
    /// </summary>
    [Serializable]
    public class ReportPeriodTargetCore : IdentifiableCore, IReportPeriodTarget
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

        /// <summary>
        ///   The text type.
        /// </summary>
        private readonly TextType textType;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPeriodTargetCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportPeriodTargetCore(IIdentifiableObject parent, IReportPeriodTargetMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this.textType = TextType.GetFromEnum(TextEnumType.ObservationalTimePeriod);
            if (itemMutableObject.StartTime != null)
            {
                this.startTime = new SdmxDateCore(itemMutableObject.StartTime, TimeFormatEnumType.DateTime);
            }

            if (itemMutableObject.EndTime != null)
            {
                this.endTime = new SdmxDateCore(itemMutableObject.EndTime, TimeFormatEnumType.DateTime);
            }

            if (itemMutableObject.TextType != null)
            {
                this.textType = itemMutableObject.TextType;
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPeriodTargetCore"/> class.
        /// </summary>
        /// <param name="reportPeriodTargetType">
        /// The report period target type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal ReportPeriodTargetCore(ReportPeriodTargetType reportPeriodTargetType, IMetadataTarget parent)
            : base(
                reportPeriodTargetType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportPeriodTarget), parent)
        {
            this.textType = TextType.GetFromEnum(TextEnumType.ObservationalTimePeriod);
            if (reportPeriodTargetType.LocalRepresentation != null)
            {
                RepresentationType repType = reportPeriodTargetType.LocalRepresentation;
                if (repType.TextFormat != null)
                {
                    if (repType.TextFormat.startTime != null)
                    {
                        this.startTime = new SdmxDateCore(repType.TextFormat.startTime.ToString());
                    }

                    if (repType.TextFormat.endTime != null)
                    {
                        this.endTime = new SdmxDateCore(repType.TextFormat.endTime.ToString());
                    }

                    if (repType.TextFormat.textType != null)
                    {
                        this.textType = TextTypeUtil.GetTextType(repType.TextFormat.textType);
                    }
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
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
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return ReportPeriodTarget.FixedId;
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

        /// <summary>
        ///   Gets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this.textType;
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IReportPeriodTarget)sdmxObject;
                if (!ObjectUtil.Equivalent(this.startTime, that.StartTime))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.endTime, that.EndTime))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            this.Id = ReportPeriodTarget.FixedId;
            if (this.startTime != null && this.endTime != null)
            {
                if (this.startTime.IsLater(this.endTime))
                {
                    throw new SdmxSemmanticException("Report Period Target - start time can not be later then end time");
                }
            }
        }

        #endregion
    }
}
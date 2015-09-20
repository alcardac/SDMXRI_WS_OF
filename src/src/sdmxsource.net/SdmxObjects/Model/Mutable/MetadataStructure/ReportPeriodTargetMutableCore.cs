// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportPeriodTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The report period target mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The report period target mutable core.
    /// </summary>
    [Serializable]
    public class ReportPeriodTargetMutableCore : IdentifiableMutableCore, IReportPeriodTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The end time.
        /// </summary>
        private DateTime? endTime;

        /// <summary>
        ///   The start time.
        /// </summary>
        private DateTime? startTime;

        /// <summary>
        ///   The text type.
        /// </summary>
        private TextType textType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPeriodTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public ReportPeriodTargetMutableCore(IReportPeriodTarget objTarget)
            : base(objTarget)
        {
            this.textType = objTarget.TextType;
            if (objTarget.StartTime != null)
            {
                this.startTime = objTarget.StartTime.Date;
            }

            if (objTarget.EndTime != null)
            {
                this.startTime = objTarget.EndTime.Date;
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReportPeriodTargetMutableCore" /> class.
        /// </summary>
        public ReportPeriodTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportPeriodTarget))
        {
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
                return this.endTime;
            }

            set
            {
                this.endTime = value;
            }
        }

        /// <summary>
        ///   Gets or sets the start time.
        /// </summary>
        public virtual DateTime? StartTime
        {
            get
            {
                return this.startTime;
            }

            set
            {
                this.startTime = value;
            }
        }

        /// <summary>
        ///   Gets or sets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this.textType;
            }

            set
            {
                this.textType = value;
            }
        }

        #endregion
    }
}
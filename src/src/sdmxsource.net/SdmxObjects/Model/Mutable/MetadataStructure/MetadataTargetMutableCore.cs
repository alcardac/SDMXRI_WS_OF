// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata target mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     The metadata target mutable core.
    /// </summary>
    [Serializable]
    public class MetadataTargetMutableCore : IdentifiableMutableCore, IMetadataTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///     The _identifiable target.
        /// </summary>
        private readonly IList<IIdentifiableTargetMutableObject> _identifiableTarget;

        /// <summary>
        ///     The _constraint content target.
        /// </summary>
        private IConstraintContentTargetMutableObject _constraintContentTarget;

        /// <summary>
        ///     The _data set target.
        /// </summary>
        private IDataSetTargetMutableObject _dataSetTarget;

        /// <summary>
        ///     The _key descriptor values target.
        /// </summary>
        private IKeyDescriptorValuesTargetMutableObject _keyDescriptorValuesTarget;

        /// <summary>
        ///     The _report period target.
        /// </summary>
        private IReportPeriodTargetMutableObject _reportPeriodTarget;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target.
        /// </param>
        public MetadataTargetMutableCore(IMetadataTarget objTarget)
            : base(objTarget)
        {
            this._identifiableTarget = new List<IIdentifiableTargetMutableObject>();
            if (objTarget.KeyDescriptorValuesTarget != null)
            {
                this._keyDescriptorValuesTarget =
                    new KeyDescriptorValuesTargetMutableCore(objTarget.KeyDescriptorValuesTarget);
            }

            if (objTarget.DataSetTarget != null)
            {
                this._dataSetTarget = new DataSetTargetMutableCore(objTarget.DataSetTarget);
            }

            if (objTarget.ReportPeriodTarget != null)
            {
                this._reportPeriodTarget = new ReportPeriodTargetMutableCore(objTarget.ReportPeriodTarget);
            }

            if (objTarget.ConstraintContentTarget != null)
            {
                this._constraintContentTarget = new ConstraintContentTargetMutableCore(
                    objTarget.ConstraintContentTarget);
            }

            if (objTarget.IdentifiableTarget != null)
            {
                foreach (IIdentifiableTarget identifiableTarget in objTarget.IdentifiableTarget)
                {
                    this._identifiableTarget.Add(new IdentifiableTargetMutableCore(identifiableTarget));
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetadataTargetMutableCore" /> class.
        /// </summary>
        public MetadataTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget))
        {
            this._identifiableTarget = new List<IIdentifiableTargetMutableObject>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the constraint content target.
        /// </summary>
        public virtual IConstraintContentTargetMutableObject ConstraintContentTarget
        {
            get
            {
                return this._constraintContentTarget;
            }

            set
            {
                this._constraintContentTarget = value;
            }
        }

        /// <summary>
        ///     Gets or sets the data set target.
        /// </summary>
        public virtual IDataSetTargetMutableObject DataSetTarget
        {
            get
            {
                return this._dataSetTarget;
            }

            set
            {
                this._dataSetTarget = value;
            }
        }

        /// <summary>
        ///     Gets the identifiable target.
        /// </summary>
        public virtual IList<IIdentifiableTargetMutableObject> IdentifiableTarget
        {
            get
            {
                return this._identifiableTarget;
            }
        }

        /// <summary>
        ///     Gets or sets the key descriptor values target.
        /// </summary>
        public virtual IKeyDescriptorValuesTargetMutableObject KeyDescriptorValuesTarget
        {
            get
            {
                return this._keyDescriptorValuesTarget;
            }

            set
            {
                this._keyDescriptorValuesTarget = value;
            }
        }

        /// <summary>
        ///     Gets or sets the report period target.
        /// </summary>
        public virtual IReportPeriodTargetMutableObject ReportPeriodTarget
        {
            get
            {
                return this._reportPeriodTarget;
            }

            set
            {
                this._reportPeriodTarget = value;
            }
        }

        #endregion
    }
}
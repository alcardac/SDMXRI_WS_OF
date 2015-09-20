// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeasureListMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The measure list mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The measure list mutable core.
    /// </summary>
    [Serializable]
    public class MeasureListMutableCore : IdentifiableMutableCore, IMeasureListMutableObject
    {
        #region Fields

        /// <summary>
        ///   The iprimary measure mutable.
        /// </summary>
        private IPrimaryMeasureMutableObject primaryMeasureMutableObject;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="MeasureListMutableCore" /> class.
        /// </summary>
        public MeasureListMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureListMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public MeasureListMutableCore(IMeasureList objTarget)
            : base(objTarget)
        {
            if (objTarget.PrimaryMeasure != null)
            {
                this.primaryMeasureMutableObject = new PrimaryMeasureMutableCore(objTarget.PrimaryMeasure);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the primary measure.
        /// </summary>
        public virtual IPrimaryMeasureMutableObject PrimaryMeasure
        {
            get
            {
                return this.primaryMeasureMutableObject;
            }

            set
            {
                this.primaryMeasureMutableObject = value;
            }
        }

        #endregion
    }
}
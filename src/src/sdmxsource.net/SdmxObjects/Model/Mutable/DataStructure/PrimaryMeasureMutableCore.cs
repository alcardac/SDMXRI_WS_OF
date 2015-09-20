// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMeasureMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The primary measure mutable core.
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
    ///   The primary measure mutable core.
    /// </summary>
    [Serializable]
    public class PrimaryMeasureMutableCore : ComponentMutableCore, IPrimaryMeasureMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="PrimaryMeasureMutableCore" /> class.
        /// </summary>
        public PrimaryMeasureMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.PrimaryMeasure))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryMeasureMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public PrimaryMeasureMutableCore(IPrimaryMeasure objTarget)
            : base(objTarget)
        {
        }

        #endregion
    }
}
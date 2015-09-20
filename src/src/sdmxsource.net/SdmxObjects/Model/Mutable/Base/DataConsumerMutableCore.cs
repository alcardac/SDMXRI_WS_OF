// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConsumerMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data consumer mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The data consumer mutable core.
    /// </summary>
    [Serializable]
    public class DataConsumerMutableCore : OrganisationMutableObjectCore, IDataConsumerMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataConsumerMutableCore" /> class.
        /// </summary>
        public DataConsumerMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DataConsumerMutableCore(IDataConsumer objTarget)
            : base(objTarget)
        {
        }

        #endregion
    }
}
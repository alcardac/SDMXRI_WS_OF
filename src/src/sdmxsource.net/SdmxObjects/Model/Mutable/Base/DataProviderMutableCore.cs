// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data provider mutable core.
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
    ///   The data provider mutable core.
    /// </summary>
    [Serializable]
    public class DataProviderMutableCore : OrganisationMutableObjectCore, IDataProviderMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataProviderMutableCore" /> class.
        /// </summary>
        public DataProviderMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DataProviderMutableCore(IDataProvider objTarget)
            : base(objTarget)
        {
        }

        #endregion
    }
}
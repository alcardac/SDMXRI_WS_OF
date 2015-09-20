// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencyMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency mutable core.
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
    ///   The agency mutable core.
    /// </summary>
    [Serializable]
    public class AgencyMutableCore : OrganisationMutableObjectCore, IAgencyMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AgencyMutableCore" /> class.
        /// </summary>
        public AgencyMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public AgencyMutableCore(IAgency objTarget)
            : base(objTarget)
        {
        }

        #endregion
    }
}
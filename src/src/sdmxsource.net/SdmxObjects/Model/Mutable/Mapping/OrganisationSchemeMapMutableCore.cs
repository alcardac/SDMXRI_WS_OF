// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationSchemeMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation scheme map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The organisation scheme map mutable core.
    /// </summary>
    [Serializable]
    public class OrganisationSchemeMapMutableCore : ItemSchemeMapMutableCore, IOrganisationSchemeMapMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrganisationSchemeMapMutableCore" /> class.
        /// </summary>
        public OrganisationSchemeMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public OrganisationSchemeMapMutableCore(IOrganisationSchemeMapObject objTarget)
            : base(objTarget)
        {
        }

        #endregion
    }
}
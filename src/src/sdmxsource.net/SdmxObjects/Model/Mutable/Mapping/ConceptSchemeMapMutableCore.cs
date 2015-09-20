// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The concept scheme map mutable core.
    /// </summary>
    [Serializable]
    public class ConceptSchemeMapMutableCore : ItemSchemeMapMutableCore, IConceptSchemeMapMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConceptSchemeMapMutableCore" /> class.
        /// </summary>
        public ConceptSchemeMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="conceptSchemeMapObject">
        /// The icon. 
        /// </param>
        public ConceptSchemeMapMutableCore(IConceptSchemeMapObject conceptSchemeMapObject)
            : base(conceptSchemeMapObject)
        {
        }

        #endregion
    }
}
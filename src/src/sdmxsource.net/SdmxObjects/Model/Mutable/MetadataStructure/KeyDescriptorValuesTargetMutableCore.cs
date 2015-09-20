// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyDescriptorValuesTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The key descriptor values target mutable core.
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
    ///   The key descriptor values target mutable core.
    /// </summary>
    [Serializable]
    public class KeyDescriptorValuesTargetMutableCore : IdentifiableMutableCore, IKeyDescriptorValuesTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The text type.
        /// </summary>
        private TextType textType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDescriptorValuesTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public KeyDescriptorValuesTargetMutableCore(IKeyDescriptorValuesTarget objTarget)
            : base(objTarget)
        {
            this.textType = objTarget.TextType;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="KeyDescriptorValuesTargetMutableCore" /> class.
        /// </summary>
        public KeyDescriptorValuesTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptorValuesTarget))
        {
        }

        #endregion

        #region Public Properties

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
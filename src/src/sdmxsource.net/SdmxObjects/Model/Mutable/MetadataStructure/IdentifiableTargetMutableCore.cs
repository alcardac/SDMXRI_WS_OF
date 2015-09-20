// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The identifiable target mutable core.
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
    ///   The identifiable target mutable core.
    /// </summary>
    [Serializable]
    public class IdentifiableTargetMutableCore : ComponentMutableCore, IIdentifiableTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The referenced structure type.
        /// </summary>
        private SdmxStructureType referencedStructureType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public IdentifiableTargetMutableCore(IIdentifiableTarget objTarget)
            : base(objTarget)
        {
            this.referencedStructureType = objTarget.ReferencedStructureType;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="IdentifiableTargetMutableCore" /> class.
        /// </summary>
        public IdentifiableTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.IdentifiableObjectTarget))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the referenced structure type.
        /// </summary>
        public virtual SdmxStructureType ReferencedStructureType
        {
            get
            {
                return this.referencedStructureType;
            }

            set
            {
                this.referencedStructureType = value;
            }
        }

        #endregion
    }
}
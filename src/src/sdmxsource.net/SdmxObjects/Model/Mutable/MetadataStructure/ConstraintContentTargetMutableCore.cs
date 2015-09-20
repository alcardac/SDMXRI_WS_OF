// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintContentTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint content target mutable core.
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
    ///   The constraint content target mutable core.
    /// </summary>
    [Serializable]
    public class ConstraintContentTargetMutableCore : IdentifiableMutableCore, IConstraintContentTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The text type.
        /// </summary>
        private TextType textType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintContentTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ConstraintContentTargetMutableCore(IConstraintContentTarget objTarget)
            : base(objTarget)
        {
            this.textType = objTarget.TextType;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstraintContentTargetMutableCore" /> class.
        /// </summary>
        public ConstraintContentTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraintAttachment))
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
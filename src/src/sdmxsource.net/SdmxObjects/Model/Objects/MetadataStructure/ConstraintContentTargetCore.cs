// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintContentTargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint content target core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using ConstraintContentTarget = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.ConstraintContentTarget;

    /// <summary>
    ///   The constraint content target core.
    /// </summary>
    [Serializable]
    public class ConstraintContentTargetCore : IdentifiableCore, IConstraintContentTarget
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintContentTargetCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public ConstraintContentTargetCore(IMetadataTarget parent, IConstraintContentTargetMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintContentTargetCore"/> class.
        /// </summary>
        /// <param name="target">
        /// The target. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal ConstraintContentTargetCore(ConstraintContentTargetType target, IMetadataTarget parent)
            : base(target, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstraintContentTarget), parent)
        {
            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return ConstraintContentTarget.FixedId;
            }
        }

        /// <summary>
        ///   Gets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                // FIXED VALUE
                return TextType.GetFromEnum(TextEnumType.AttachmentConstraintReference);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IConstraintContentTarget)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
            this.Id = ConstraintContentTarget.FixedId;
        }

        #endregion
    }
}
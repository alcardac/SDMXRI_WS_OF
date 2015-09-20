// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyDescriptorValuesTargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The key descriptor values target core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;

    using KeyDescriptorValuesTarget = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.KeyDescriptorValuesTarget;

    /// <summary>
    ///   The key descriptor values target core.
    /// </summary>
    [Serializable]
    public class KeyDescriptorValuesTargetCore : IdentifiableCore, IKeyDescriptorValuesTarget
    {
        #region Fields

        /// <summary>
        ///   The text type.
        /// </summary>
        private readonly TextType textType;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDescriptorValuesTargetCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        public KeyDescriptorValuesTargetCore(IMetadataTarget parent, IKeyDescriptorValuesTargetMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this.textType = TextType.GetFromEnum(TextEnumType.KeyValues);
            try
            {
                this.textType = itemMutableObject.TextType;
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDescriptorValuesTargetCore"/> class.
        /// </summary>
        /// <param name="target">
        /// The target. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal KeyDescriptorValuesTargetCore(KeyDescriptorValuesTargetType target, IMetadataTarget parent)
            : base(target, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptorValuesTarget), parent)
        {
            this.textType = TextType.GetFromEnum(TextEnumType.KeyValues);
            if (target.LocalRepresentation != null)
            {
                RepresentationType repType = target.LocalRepresentation;
                if (repType.TextFormat != null)
                {
                    if (repType.TextFormat.textType != null)
                    {
                        this.textType = TextTypeUtil.GetTextType(repType.TextFormat.textType);
                    }
                }
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return KeyDescriptorValuesTarget.FixedId;
            }
        }

        /// <summary>
        ///   Gets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this.textType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IKeyDescriptorValuesTarget)sdmxObject, includeFinalProperties);
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
            this.Id = KeyDescriptorValuesTarget.FixedId;
        }

        #endregion
    }
}
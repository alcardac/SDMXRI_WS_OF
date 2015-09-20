// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableTargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The identifiable target core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;

    /// <summary>
    ///   The identifiable target core.
    /// </summary>
    [Serializable]
    public class IdentifiableTargetCore : ComponentCore, IIdentifiableTarget
    {
        #region Fields

        /// <summary>
        ///   The referenced structure type.
        /// </summary>
        private SdmxStructureType referencedStructureType;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableTargetCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public IdentifiableTargetCore(IIdentifiableTargetMutableObject itemMutableObject, IMetadataTarget parent)
            : base(itemMutableObject, parent, false)
        {
            this.referencedStructureType = itemMutableObject.ReferencedStructureType;
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableTargetCore"/> class.
        /// </summary>
        /// <param name="identifiableTargetType">
        /// The identifiable target type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal IdentifiableTargetCore(
            IdentifiableObjectTargetType identifiableTargetType, IMetadataTarget parent)
            : base(
                identifiableTargetType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.IdentifiableObjectTarget), 
                parent)
        {
            this.referencedStructureType = XmlobjectsEnumUtil.GetSdmxStructureType(identifiableTargetType.objectType);
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the referenced structure type.
        /// </summary>
        public virtual SdmxStructureType ReferencedStructureType
        {
            get
            {
                return this.referencedStructureType;
            }
        }
        
        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
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
                var that = (IIdentifiableTarget)sdmxObject;
                if (this.referencedStructureType != that.ReferencedStructureType)
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            // Sanity Check, make sure we can build both ways
            string objIdEnum = XmlobjectsEnumUtil.BuildV21(this.referencedStructureType);
            this.referencedStructureType = XmlobjectsEnumUtil.GetSdmxStructureTypeV21(objIdEnum);

            if (this.LocalRepresentation == null)
            {
                // Create a Local Representation if one doesn't exist. The schema insists on there being an Identifiable Reference
                ITextFormatMutableObject textFormatMutable = new TextFormatMutableCore();
                textFormatMutable.TextType = TextType.GetFromEnum(TextEnumType.IdentifiableReference);
                IRepresentationMutableObject representationMutable = new RepresentationMutableCore();
                representationMutable.TextFormat = textFormatMutable;
                this.LocalRepresentation = new RepresentationCore(representationMutable, this);
                return;
            }

            if (this.LocalRepresentation.Representation == null)
            {
                // Create a local representation if one doesn't exist. The schema insists on there being an Identifiable Reference 
                if (this.LocalRepresentation == null || this.LocalRepresentation.TextFormat == null
                    || this.LocalRepresentation.TextFormat.TextType.EnumType != TextEnumType.IdentifiableReference)
                {
                    ITextFormatMutableObject textFormatMutable = new TextFormatMutableCore();
                    textFormatMutable.TextType = TextType.GetFromEnum(TextEnumType.IdentifiableReference);
                    IRepresentationMutableObject representationMutable = new RepresentationMutableCore();
                    representationMutable.TextFormat = textFormatMutable;
                    this.LocalRepresentation = new RepresentationCore(representationMutable, this);
                }
            }
            else
            {
                if (this.LocalRepresentation.TextFormat != null)
                {
                    throw new SdmxSemmanticException(
                        "Can not have both TextFormat and Representation set on IdentifiableObjectTarget");
                }
            }

            if (this.referencedStructureType == null)
            {
                throw new SdmxSemmanticException("Identifiable target is missing Target CategorisationStructure Type (objectType)");
            }
        }

        #endregion
    }
}
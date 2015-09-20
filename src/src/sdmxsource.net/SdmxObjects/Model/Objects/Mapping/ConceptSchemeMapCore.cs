// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using ConceptMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ConceptMap;
    using ConceptMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptMapType;

    /// <summary>
    ///   The concept scheme map core.
    /// </summary>
    [Serializable]
    public class ConceptSchemeMapCore : ItemSchemeMapCore, IConceptSchemeMapObject
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeMapCore"/> class.
        /// </summary>
        /// <param name="conceptSchemeMapMutableObject">
        /// The icon. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeMapCore(IConceptSchemeMapMutableObject conceptSchemeMapMutableObject, IStructureSetObject parent)
            : base(conceptSchemeMapMutableObject, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap), parent)
        {
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
        /// Initializes a new instance of the <see cref="ConceptSchemeMapCore"/> class.
        /// </summary>
        /// <param name="conMapType">
        /// The con map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeMapCore(ConceptSchemeMapType conMapType, IStructureSetObject parent)
            : base(conMapType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap), parent)
        {
            this.SourceRef = RefUtil.CreateReference(this, conMapType.Source);
            this.TargetRef = RefUtil.CreateReference(this, conMapType.Target);

            if (conMapType.ItemAssociation != null)
            {
                foreach (ConceptMap conMap in conMapType.ItemAssociation)
                {
                    IItemMap item = new ItemMapCore(conMap.Source.GetTypedRef<LocalConceptRefType>().id, conMap.Target.GetTypedRef<LocalConceptRefType>().id, this);
                    this.AddInternalItem(item);
                }
            }

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
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeMapCore"/> class.
        /// </summary>
        /// <param name="conceptObject">
        /// The concept object. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptSchemeMapType conceptObject, 
            IStructureSetObject parent)
            : base(
                conceptObject, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap), 
                conceptObject.id, 
                null, 
                conceptObject.Name, 
                conceptObject.Description, 
                conceptObject.Annotations, 
                parent)
        {
            if (conceptObject.ConceptSchemeRef != null)
            {
                if (conceptObject.ConceptSchemeRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, conceptObject.ConceptSchemeRef.URN);
                }
                else
                {
                    this.SourceRef = new CrossReferenceImpl(
                        this, 
                        conceptObject.ConceptSchemeRef.AgencyID, 
                        conceptObject.ConceptSchemeRef.ConceptSchemeID, 
                        conceptObject.ConceptSchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));
                }
            }

            if (conceptObject.TargetConceptSchemeRef != null)
            {
                if (conceptObject.TargetConceptSchemeRef.URN != null)
                {
                    this.TargetRef = new CrossReferenceImpl(this, conceptObject.TargetConceptSchemeRef.URN);
                }
                else
                {
                    this.TargetRef = new CrossReferenceImpl(
                        this, 
                        conceptObject.TargetConceptSchemeRef.AgencyID, 
                        conceptObject.TargetConceptSchemeRef.ConceptSchemeID, 
                        conceptObject.TargetConceptSchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));
                }
            }

            // get list of code maps
            if (conceptObject.ConceptMap != null)
            {
                foreach (ConceptMapType conMap in conceptObject.ConceptMap)
                {
                    IItemMap item = new ItemMapCore(conMap.conceptAlias, conMap.ConceptID, conMap.TargetConceptID, this);
                    this.AddInternalItem(item);
                }
            }

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

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsNameable((IConceptSchemeMapObject)sdmxObject, includeFinalProperties);
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
        protected internal void Validate()
        {
            if (this.SourceRef == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "ConceptSchemeRef");
            }

            if (this.TargetRef == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "TargetConceptSchemeSchemeRef");
            }

            if (this.Items == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "conceptMap");
            }
        }

        #endregion
    }
}
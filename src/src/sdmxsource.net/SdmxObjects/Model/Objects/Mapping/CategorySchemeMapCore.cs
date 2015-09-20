// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using CategoryMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryMapType;

    /// <summary>
    ///   The category scheme map core.
    /// </summary>
    [Serializable]
    public class CategorySchemeMapCore : SchemeMapCore, ICategorySchemeMapObject
    {
        #region Fields

        /// <summary>
        ///   The category maps.
        /// </summary>
        private readonly IList<ICategoryMap> categoryMaps;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeMapCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeMapCore(ICategorySchemeMapMutableObject itemMutableObject, IStructureSetObject parent)
            : base(itemMutableObject, parent)
        {
            this.categoryMaps = new List<ICategoryMap>();
            if (itemMutableObject.CategoryMaps != null)
            {
                this.categoryMaps = new List<ICategoryMap>();

                foreach (ICategoryMapMutableObject catMap in itemMutableObject.CategoryMaps)
                {
                    ICategoryMap categoryMap = new CategoryMapCore(catMap, this);
                    this.categoryMaps.Add(categoryMap);
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
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeMapCore"/> class.
        /// </summary>
        /// <param name="catMapType">
        /// The cat map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeMapCore(CategorySchemeMapType catMapType, IStructureSetObject parent)
            : base(catMapType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategorySchemeMap), parent)
        {
            this.categoryMaps = new List<ICategoryMap>();

            this.SourceRef = RefUtil.CreateReference(this, catMapType.GetTypedSource<CategorySchemeReferenceType>());
            this.TargetRef = RefUtil.CreateReference(this, catMapType.GetTypedTarget<CategorySchemeReferenceType>());

            if (catMapType.ItemAssociation != null)
            {
                foreach (CategoryMap catMap in catMapType.ItemAssociation)
                {
                    ICategoryMap categoryMap = new CategoryMapCore(catMap.Content, this);
                    this.categoryMaps.Add(categoryMap);
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
        /// Initializes a new instance of the <see cref="CategorySchemeMapCore"/> class.
        /// </summary>
        /// <param name="catMapType">
        /// The cat map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategorySchemeMapType catMapType, IStructureSetObject parent)
            : base(
                catMapType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategorySchemeMap), 
                catMapType.id, 
                null, 
                catMapType.Name, 
                catMapType.Description, 
                catMapType.Annotations, 
                parent)
        {
            this.categoryMaps = new List<ICategoryMap>();

            if (catMapType.CategorySchemeRef != null)
            {
                if (catMapType.CategorySchemeRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, catMapType.CategorySchemeRef.URN);
                }
                else
                {
                    this.SourceRef = new CrossReferenceImpl(
                        this, 
                        catMapType.CategorySchemeRef.AgencyID, 
                        catMapType.CategorySchemeRef.CategorySchemeID, 
                        catMapType.CategorySchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
                }
            }

            if (catMapType.TargetCategorySchemeRef != null)
            {
                if (catMapType.TargetCategorySchemeRef.URN != null)
                {
                    this.TargetRef = new CrossReferenceImpl(this, catMapType.TargetCategorySchemeRef.URN);
                }
                else
                {
                    this.TargetRef = new CrossReferenceImpl(
                        this, 
                        catMapType.TargetCategorySchemeRef.AgencyID, 
                        catMapType.TargetCategorySchemeRef.CategorySchemeID, 
                        catMapType.TargetCategorySchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
                }
            }

            if (catMapType.CategoryMap != null)
            {
                foreach (CategoryMapType catMap in catMapType.CategoryMap)
                {
                    ICategoryMap categoryMap = new CategoryMapCore(catMap, this);
                    this.categoryMaps.Add(categoryMap);
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

        #region Public Properties

        /// <summary>
        ///   Gets the category maps.
        /// </summary>
        public virtual IList<ICategoryMap> CategoryMaps
        {
            get
            {
                return new List<ICategoryMap>(this.categoryMaps);
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
                var that = (ICategorySchemeMapObject)sdmxObject;
                if (!this.Equivalent(this.categoryMaps, that.CategoryMaps, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
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
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "CategorySchemeRef");
            }

            if (this.TargetRef == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "TargetCategorySchemeRef");
            }

            if (this.categoryMaps == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "CategoryMap");
            }
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
        	ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this.categoryMaps, composites);
            return composites;
        }

        #endregion
    }
}
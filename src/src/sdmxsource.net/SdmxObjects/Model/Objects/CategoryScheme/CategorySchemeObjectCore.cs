// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using CategoryType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryType;

    /// <summary>
    ///   The category scheme object core.
    /// </summary>
    [Serializable]
    public class CategorySchemeObjectCore :
        ItemSchemeObjectCore<ICategoryObject, ICategorySchemeObject, ICategorySchemeMutableObject, ICategoryMutableObject>, 
        ICategorySchemeObject
    {
        #region Static Fields

        /// <summary>
        ///   The _sdmx structure.
        /// </summary>
        private static readonly SdmxStructureType _sdmxStructure =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeObjectCore"/> class.
        /// </summary>
        /// <param name="categoryScheme">
        /// The category scheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeObjectCore(ICategorySchemeMutableObject categoryScheme)
            : base(categoryScheme)
        {
            try
            {
                if (categoryScheme.Items != null)
                {
                    foreach (ICategoryMutableObject currentcategory in categoryScheme.Items)
                    {
                        this.AddInternalItem(new CategoryCore(this, currentcategory));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeObjectCore"/> class.
        /// </summary>
        /// <param name="categoryScheme">
        /// The category scheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeObjectCore(CategorySchemeType categoryScheme)
            : base(categoryScheme, _sdmxStructure)
        {
            // base(categoryScheme, _sdmxStructure, categoryScheme.validTo, categoryScheme.validFrom, categoryScheme.version, Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base.SdmxStructureCore.CreateTertiary(categoryScheme.IsSetIsFinal(),categoryScheme.IsFinal), categoryScheme.agencyID, categoryScheme.Id, categoryScheme.uri.ToString(), categoryScheme.Name, categoryScheme.Description, Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base.SdmxStructureCore.CreateTertiary(categoryScheme.IsSetIsExternalReference(),categoryScheme.isExternalReference), categoryScheme.Annotations) {
            try
            {
                foreach (Category currentcategory in categoryScheme.Item)
                {
                    this.AddInternalItem(new CategoryCore(this, currentcategory.Content));
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeObjectCore"/> class.
        /// </summary>
        /// <param name="categoryScheme">
        /// The category scheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorySchemeObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategorySchemeType categoryScheme)
            : base(
                categoryScheme, 
                _sdmxStructure, 
                categoryScheme.validTo, 
                categoryScheme.validFrom, 
                categoryScheme.version, 
                CreateTertiary(categoryScheme.isFinal), 
                categoryScheme.agencyID, 
                categoryScheme.id, 
                categoryScheme.uri, 
                categoryScheme.Name, 
                categoryScheme.Description, 
                CreateTertiary(categoryScheme.isExternalReference), 
                categoryScheme.Annotations)
        {
            try
            {
                foreach (CategoryType currentcategory in categoryScheme.Category)
                {
                    this.AddInternalItem(new CategoryCore(this, currentcategory));
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private CategorySchemeObjectCore(ICategorySchemeObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override ICategorySchemeMutableObject MutableInstance
        {
            get
            {
                return new CategorySchemeMutableCore(this);
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

        #region Explicit Interface Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        IMaintainableMutableObject IMaintainableObject.MutableInstance
        {
            get
            {
                return this.MutableInstance;
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
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((ICategorySchemeObject)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get category.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <returns>
        /// The <see cref="ICategoryObject"/> . 
        /// </returns>
        public virtual ICategoryObject GetCategory(params string[] id)
        {
            return this.GetCategory(this.Items, id, 0);
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="ICategorySchemeObject"/> . 
        /// </returns>
        public override ICategorySchemeObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new CategorySchemeObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            base.ValidateId(false);
        }

        /// <summary>
        /// The get category.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="categories">
        /// The categories. 
        /// </param>
        /// <returns>
        /// The <see cref="ICategoryObject"/> . 
        /// </returns>
        private ICategoryObject GetCategory(string id, IEnumerable<ICategoryObject> categories)
        {
            foreach (ICategoryObject currentCategory in categories)
            {
                if (currentCategory.Id.Equals(id))
                {
                    return currentCategory;
                }
            }

            return null;
        }

        /// <summary>
        /// The get category.
        /// </summary>
        /// <param name="categories">
        /// The categories. 
        /// </param>
        /// <param name="ids">
        /// The ids. 
        /// </param>
        /// <param name="position">
        /// The position. 
        /// </param>
        /// <returns>
        /// The <see cref="ICategoryObject"/> . 
        /// </returns>
        private ICategoryObject GetCategory(IEnumerable<ICategoryObject> categories, string[] ids, int position)
        {
            string categoryId = ids[position];

            ICategoryObject cat = GetCategory(categoryId, categories);
            if (cat == null)
            {
                return null;
            }

            int newPos = ++position;
            if (ids.Length > newPos)
            {
                return GetCategory(cat.Items, ids, newPos);
            }

            return cat;
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            var urns = new HashSet<Uri>();
            if (this.Items != null)
            {
                foreach (ICategoryObject category in this.Items)
                {
                    if (urns.Contains(category.Urn))
                    {
                        throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, category.Urn);
                    }

                    urns.Add(category.Urn);
                }
            }
        }

        #endregion
    }
}
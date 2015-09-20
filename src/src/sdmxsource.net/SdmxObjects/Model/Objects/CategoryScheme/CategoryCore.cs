// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The category core.
    /// </summary>
    [Serializable]
    public class CategoryCore : ItemCore, ICategoryObject
    {
        #region Fields

        /// <summary>
        ///   The _categories.
        /// </summary>
        private readonly IList<ICategoryObject> _categories;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public CategoryCore(IIdentifiableObject parent, ICategoryMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this._categories = new List<ICategoryObject>();
            if (itemMutableObject.Items != null)
            {
                foreach (ICategoryMutableObject currentCat in itemMutableObject.Items)
                {
                    this._categories.Add(new CategoryCore(this, currentCat));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="category">
        /// The sdmxObject. 
        /// </param>
        public CategoryCore(IIdentifiableObject parent, CategoryType category)
            : base(category, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category), parent)
        {
            this._categories = new List<ICategoryObject>();
            if (category.Item != null)
            {
                foreach (Category currentCat in category.Item)
                {
                    this._categories.Add(new CategoryCore(this, currentCat.Content));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="category">
        /// The sdmxObject. 
        /// </param>
        public CategoryCore(
            IIdentifiableObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryType category)
            : base(
                category, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category), 
                category.id, 
                category.uri, 
                category.Name, 
                category.Description, 
                category.Annotations, 
                parent)
        {
            this._categories = new List<ICategoryObject>();
            if (category.Category != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryType currentCat in category.Category)
                {
                    this._categories.Add(new CategoryCore(this, currentCat));
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public virtual IList<ICategoryObject> Items
        {
            get
            {
                return new List<ICategoryObject>(this._categories);
            }
        }

        #endregion

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
            
            if(sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ICategoryObject)sdmxObject;
                if (!this.Equivalent(this._categories, that.Items, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////COMPOSITES                           //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////	

       /// <summary>
       /// The get composites internal.
       /// </summary>
       public new ISet<ISdmxObject> GetCompositesInternal()
       {
	      ISet<ISdmxObject> composites = base.GetCompositesInternal();
	      base.AddToCompositeSet(_categories, composites);
	      return composites;
	   }
 
        #endregion
    }
}
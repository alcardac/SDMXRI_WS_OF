// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;

    /// <summary>
    ///   The category scheme mutable core.
    /// </summary>
    [Serializable]
    public class CategorySchemeMutableCore :
        ItemSchemeMutableCore<ICategoryMutableObject, ICategoryObject, ICategorySchemeObject>, 
        ICategorySchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategorySchemeMutableCore" /> class.
        /// </summary>
        public CategorySchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeMutableCore"/> class.
        /// </summary>
        /// <param name="categorySchemeObject">
        /// The categorySchemeObject. 
        /// </param>
        public CategorySchemeMutableCore(ICategorySchemeObject categorySchemeObject)
            : base(categorySchemeObject)
        {
            // convert Category list to Mutable Category beans
            if (categorySchemeObject.Items != null)
            {
                foreach (ICategoryObject category in categorySchemeObject.Items)
                {
                    this.AddItem(new CategoryMutableCore(category));
                }
            }
        }

        #endregion

        /*public ICategorySchemeObject ImmutableInstance {
          get {
                return new CategorySchemeObjectCore(this);
            }
        }*/

        // public void setItems(List<ICategoryMutableObject> items) {
        // this.items = items;
        // }
        // // interface specifies ICategoryObject type so change to mutable inside add() 
        // public void addItem(ICategoryMutableObject item) {
        // this.items.add((ICategoryMutableObject)item);
        // }
        // public List<ICategoryMutableObject> getItems() {
        // return items;
        // }
        /* ^^^
        ICategorySchemeObject ICategorySchemeMutableObject.ImmutableInstance
        {
            get { return new CategorySchemeObjectCore(this);}
        }*/

        /* public override ICategorySchemeObject ImmutableInstance
        {
            get { return new CategorySchemeObjectCore(this); }
        }*/
        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override ICategorySchemeObject ImmutableInstance
        {
            get
            {
                return new CategorySchemeObjectCore(this);
            }
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<ICategoryMutableObject,ICategoryObject,ICategorySchemeObject>

        public override ICategoryMutableObject CreateItem(string id, string name)
        {
            ICategoryMutableObject cat = new CategoryMutableCore();
            cat.Id = id;
            cat.AddName("en", name);
            AddItem(cat);
            return cat;
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The category mutable core.
    /// </summary>
    [Serializable]
    public sealed class CategoryMutableCore : ItemMutableCore, ICategoryMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _categories.
        /// </summary>
        private readonly IList<ICategoryMutableObject> _categories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoryMutableCore" /> class.
        /// </summary>
        public CategoryMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category))
        {
            this._categories = new List<ICategoryMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CategoryMutableCore(ICategoryObject objTarget)
            : base(objTarget)
        {
            this._categories = new List<ICategoryMutableObject>();

            // make into mutable category beans
            if (objTarget.Items != null)
            {
                this._categories = new List<ICategoryMutableObject>();

                foreach (ICategoryObject category in objTarget.Items)
                {
                    this.AddItem(new CategoryMutableCore(category));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public IList<ICategoryMutableObject> Items
        {
            get
            {
                return this._categories;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add item.
        /// </summary>
        /// <param name="item">
        /// The item. 
        /// </param>
        public void AddItem(ICategoryMutableObject item)
        {
            this._categories.Add(item);
        }

        #endregion
    }
}
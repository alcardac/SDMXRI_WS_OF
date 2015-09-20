// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The category scheme map mutable core.
    /// </summary>
    [Serializable]
    public class CategorySchemeMapMutableCore : SchemeMapMutableCore, ICategorySchemeMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The category maps.
        /// </summary>
        private IList<ICategoryMapMutableObject> categoryMaps;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategorySchemeMapMutableCore" /> class.
        /// </summary>
        public CategorySchemeMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategorySchemeMap))
        {
            this.categoryMaps = new List<ICategoryMapMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CategorySchemeMapMutableCore(ICategorySchemeMapObject objTarget)
            : base(objTarget)
        {
            this.categoryMaps = new List<ICategoryMapMutableObject>();

            // change CategoryMap list objectList to mutable CategoryMap objectList
            foreach (ICategoryMap map in objTarget.CategoryMaps)
            {
                this.AddCategoryMap(new CategoryMapMutableCore(map));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the category maps.
        /// </summary>
        public virtual IList<ICategoryMapMutableObject> CategoryMaps
        {
            get
            {
                return new ReadOnlyCollection<ICategoryMapMutableObject>(this.categoryMaps);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add category map.
        /// </summary>
        /// <param name="categoryMap">
        /// The category map. 
        /// </param>
        public void AddCategoryMap(ICategoryMapMutableObject categoryMap)
        {
            this.categoryMaps.Add(categoryMap);
        }

        #endregion
    }
}
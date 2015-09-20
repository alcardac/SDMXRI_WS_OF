// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSchemeMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item scheme map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The item scheme map mutable core.
    /// </summary>
    [Serializable]
    public abstract class ItemSchemeMapMutableCore : SchemeMapMutableCore
    {
        #region Fields

        /// <summary>
        ///   The items.
        /// </summary>
        private IList<IItemMapMutableObject> items;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public ItemSchemeMapMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
            this.items = new List<IItemMapMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ItemSchemeMapMutableCore(IItemSchemeMapObject objTarget)
            : base(objTarget)
        {
            this.items = new List<IItemMapMutableObject>();
            if (objTarget.Items != null)
            {
                foreach (IItemMap item in objTarget.Items)
                {
                    this.items.Add(new ItemMapMutableCore(item));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public IList<IItemMapMutableObject> Items
        {
            get
            {
                return this.items;
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
        public void AddItem(IItemMapMutableObject item)
        {
            this.items.Add(item);
        }

        #endregion
    }
}
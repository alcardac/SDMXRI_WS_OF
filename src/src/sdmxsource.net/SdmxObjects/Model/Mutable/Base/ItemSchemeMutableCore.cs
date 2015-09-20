// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The item scheme mutable core.
    /// </summary>
    /// <typeparam name="TItemMutable">Generic type parameter type of: IItemMutableObject
    /// </typeparam>
    /// <typeparam name="TItem">Generic type parameter type of: IItemObject
    /// </typeparam>
    /// <typeparam name="TScheme">Generic type parameter type of: IItemSchemeObject
    /// </typeparam>
    [Serializable]
    public abstract class ItemSchemeMutableCore<TItemMutable, TItem, TScheme> : MaintainableMutableCore<TScheme>, 
                                                                                IItemSchemeMutableObject<TItemMutable>
        where TItemMutable : IItemMutableObject 
        where TItem : IItemObject 
        where TScheme : IItemSchemeObject<TItem>
    {
        #region Fields

        /// <summary>
        ///   The _is partial.
        /// </summary>
        private bool _isPartial;

        /// <summary>
        ///   The _items.
        /// </summary>
        private IList<TItemMutable> _items;

        #endregion

        /* @SuppressWarnings("rawtypes")*/
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMutableCore{TItemMutable,TItem,TScheme}"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        protected ItemSchemeMutableCore(IItemSchemeObject<TItem> objTarget)
            : base(objTarget)
        {
            this._items = new List<TItemMutable>();
            this._isPartial = objTarget.Partial;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMutableCore{TItemMutable,TItem,TScheme}"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected ItemSchemeMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
            this._items = new List<TItemMutable>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public virtual IList<TItemMutable> Items
        {
            get
            {
                return this._items;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether partial.
        /// </summary>
        public bool IsPartial
        {
            get
            {
                return this._isPartial;
            }

            set
            {
                this._isPartial = value;
            }
        }

        #endregion

        #region Public Methods and Operators

       	public virtual bool RemoveItem(string id)
        {
		     
            if(_items != null && id != null)
            {

                TItemMutable item = default(TItemMutable);
                foreach (TItemMutable currentItem in _items)
                {
				   if(currentItem.Id.Equals(id)) 
                   {
					  item = currentItem;
					  break;
				   }
			    }

                if (!EqualityComparer<TItemMutable>.Default.Equals(item, default(TItemMutable)))
                {
				   return _items.Remove(item);
			    }
		    }

		    return false;
	    }

        /// <summary>
        /// The set items.
        /// </summary>
        public virtual void SetItems(IList<TItemMutable> list) 
        {
 		     this._items = list;
 	    }


        /// <summary>
        /// The add item.
        /// </summary>
        /// <param name="item">
        /// The item. 
        /// </param>
        public void AddItem(TItemMutable item)
        {
            if (this._items == null)
            {
                this._items = new List<TItemMutable>();
            }

            this._items.Add(item);
        }

        public abstract TItemMutable CreateItem(string id, string name);


        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item scheme dataStructureObject core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    /// The item scheme dataStructureObject core.
    /// </summary>
    /// <typeparam name="TItem">
    /// Generic type parm - IItemObject 
    /// </typeparam>
    /// <typeparam name="TMaint">
    /// Generic type parm -IMaintainableObject 
    /// </typeparam>
    /// <typeparam name="TMaintMutable">
    /// Generic type parm - IMaintainableMutableObject 
    /// </typeparam>
    /// <typeparam name="TItemMutable">
    /// Generic type parm - IItemMutableObject 
    /// </typeparam>
    [Serializable]
    public abstract class ItemSchemeObjectCore<TItem, TMaint, TMaintMutable, TItemMutable> :
        MaintainableObjectCore<TMaint, TMaintMutable>, 
        IItemSchemeObject<TItem>
        where TItem : IItemObject
        where TMaint : IMaintainableObject
        where TMaintMutable : IMaintainableMutableObject
        where TItemMutable : IItemMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _is partial.
        /// </summary>
        private readonly bool _isPartial;

        /// <summary>
        ///   The items.
        /// </summary>
        private IList<TItem> items = new List<TItem>();

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeObjectCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The dataStructureObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        protected internal ItemSchemeObjectCore(IItemSchemeObject<TItem> agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this._isPartial = agencyScheme.Partial;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public ItemSchemeObjectCore(SdmxStructureType structure, SdmxReader reader) {
        // super(structure, reader);
        // string partial = reader.getAttributeValue("isPartial", false);
        // if(partial != null) {
        // isPartial = Boolean.parseBoolean(partial);
        // }
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        /* @SuppressWarnings("rawtypes")*/

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeObjectCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The dataStructureObject. 
        /// </param>
        protected internal ItemSchemeObjectCore(IItemSchemeMutableObject<TItemMutable> itemMutableObject)
            : base(itemMutableObject)
        {
            this._isPartial = itemMutableObject.IsPartial;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeObjectCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected internal ItemSchemeObjectCore(ItemSchemeType createdFrom, SdmxStructureType structureType)
            : base(createdFrom, structureType)
        {
            this._isPartial = createdFrom.isPartial;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeObjectCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="endDate">
        /// The end date. 
        /// </param>
        /// <param name="startDate">
        /// The start date. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="isFinal">
        /// The is final. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="description">
        /// The description. 
        /// </param>
        /// <param name="isExternalReference">
        /// The is external reference. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal ItemSchemeObjectCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            object endDate, 
            object startDate, 
            string version, 
            TertiaryBool isFinal, 
            string agencyId, 
            string id, 
            Uri uri, 
            IList<TextType> name, 
            IList<TextType> description, 
            TertiaryBool isExternalReference, 
            AnnotationsType annotationsType)
            : base(
                createdFrom, 
                structureType, 
                endDate, 
                startDate, 
                version, 
                isFinal, 
                agencyId, 
                id, 
                uri, 
                name, 
                description, 
                isExternalReference, 
                annotationsType)
        {
            this.items = new List<TItem>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeObjectCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="isExternalReference">
        /// The is external reference. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal ItemSchemeObjectCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string version, 
            string agencyId, 
            string id, 
            Uri uri, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> name, 
            TertiaryBool isExternalReference, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType)
            : base(createdFrom, structureType, version, agencyId, id, uri, name, isExternalReference, annotationsType)
        {
            this.items = new List<TItem>();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public IList<TItem> Items
        {
            get
            {
                if (this.items == null)
                {
                    return new List<TItem>();
                }

                return new List<TItem>(this.items);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether partial.
        /// </summary>
        public virtual bool Partial
        {
            get
            {
                return this._isPartial;
            }
        }

        #endregion

        // public  MutableInstance { get; }
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

        #region Methods

        /// <summary>
        /// Add item.to <see cref="Items"/>
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        protected void AddInternalItem(TItem item)
        {
            this.items.Add(item);
        }

        /// <summary>
        /// The deep equals internal.
        /// </summary>
        /// <param name="itemSchemeObject">
        /// The dataStructureObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal bool DeepEqualsInternal(IItemSchemeObject<TItem> itemSchemeObject, bool includeFinalProperties)
        {
            if(itemSchemeObject == null) return false;

            if (!this.Equivalent(itemSchemeObject.Items, this.Items, includeFinalProperties))
            {
                return false;
            }

            if (this._isPartial != itemSchemeObject.Partial)
            {
                return false;
            }

            return base.DeepEqualsInternal(itemSchemeObject, includeFinalProperties);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
	    ////////////COMPOSITES								 //////////////////////////////////////////////////
	    ///////////////////////////////////////////////////////////////////////////////////////////////////	
        
        /// <summary>
        /// Get composites internal.
        /// </summary>
        //// NOTE In Java it is public. It is probably a bug in Java. Do not make this method "public new" else it will break a lot of things. 
        //// It should be an "override" method.
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
		  ISet<ISdmxObject> composites = base.GetCompositesInternal();
		  base.AddToCompositeSet(items, composites);
		  return composites;
        }

        
	    public virtual IItemSchemeObject<TItem> FilterItems(ICollection<string> filterIds, bool isKeepSet) 
        {
            IItemSchemeMutableObject<IItemMutableObject> mutableScheme = (IItemSchemeMutableObject<IItemMutableObject>) MutableInstance;
		
	    	ICollection<string> removeSet = null;
		    if(isKeepSet) 
            {
			   ISet<string> itemsInScheme = new HashSet<string>();
			   foreach(TItem item in Items)
               {
				  itemsInScheme.Add(item.Id);
			   }
		   
			   itemsInScheme.ExceptWith(filterIds);

			   if(itemsInScheme.Count == 0)
               {
				  return this;
			   }
			     removeSet =  itemsInScheme;
		    }
            else 
            {
			     removeSet = filterIds;
	  	    }
		    foreach(string currentRemoveId in removeSet)
            {
		      	mutableScheme.RemoveItem(currentRemoveId);
		    }
		   
            return (IItemSchemeObject<TItem>) mutableScheme.ImmutableInstance;
	    }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSchemeMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item scheme map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The item scheme map core.
    /// </summary>
    [Serializable]
    public abstract class ItemSchemeMapCore : SchemeMapCore, IItemSchemeMapObject
    {
        #region Fields

        /// <summary>
        ///   The items.
        /// </summary>
        private readonly IList<IItemMap> _items; // Package Protected

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected ItemSchemeMapCore(IItemSchemeMapMutableObject itemMutableObject, SdmxStructureType structureType, IIdentifiableObject parent)
            : base(itemMutableObject, parent)
        {
            this._items = new List<IItemMap>();
            if (itemMutableObject.Items != null)
            {
                foreach (IItemMapMutableObject item in itemMutableObject.Items)
                {
                    this._items.Add(new ItemMapCore(item, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected ItemSchemeMapCore(NameableType createdFrom, SdmxStructureType structureType, IIdentifiableObject parent)
            : base(createdFrom, structureType, parent)
        {
            this._items = new List<IItemMap>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeMapCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
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
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected ItemSchemeMapCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string id, 
            Uri uri, 
            IList<TextType> name, 
            IList<TextType> description, 
            AnnotationsType annotationsType, 
            IIdentifiableObject parent)
            : base(createdFrom, structureType, id, uri, name, description, annotationsType, parent)
        {
            this._items = new List<IItemMap>();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public IList<IItemMap> Items
        {
            get
            {
                return new List<IItemMap>(this._items);
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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IItemSchemeMapObject)sdmxObject;
                if (!this.Equivalent(this._items, that.Items, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Add item.to <see cref="Items"/>
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        protected void AddInternalItem(IItemMap item)
        {
            this._items.Add(item);
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
	    	base.AddToCompositeSet(this._items, composites);
	    	return composites;
	    }
    }
}
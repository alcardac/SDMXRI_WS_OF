// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeListBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attributeObject list core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using Attribute = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.Attribute;
    using AttributeType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.AttributeType;

    /// <summary>
    ///   The attributeObject list core.
    /// </summary>
    [Serializable]
    public class AttributeListCore : IdentifiableCore, IAttributeList
    {
        #region Fields

        /// <summary>
        ///   The attributes.
        /// </summary>
        private readonly IList<IAttributeObject> attributes;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeListCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AttributeListCore(IAttributeListMutableObject itemMutableObject, IDataStructureObject parent)
            : base(itemMutableObject, parent)
        {
            this.attributes = new List<IAttributeObject>();
            if (itemMutableObject.Attributes != null)
            {
                foreach (IAttributeMutableObject currentAttribute in itemMutableObject.Attributes)
                {
                    this.attributes.Add(new AttributeObjectCore(currentAttribute, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeListCore"/> class.
        /// </summary>
        /// <param name="attributeList">
        /// The attributeObject list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AttributeListCore(AttributeListType attributeList, IMaintainableObject parent)
            : base(attributeList, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor), parent)
        {
            this.attributes = new List<IAttributeObject>();
            if (attributeList.Attribute != null)
            {
                foreach (Attribute currentAttribute in attributeList.Attribute)
                {
                    this.attributes.Add(new AttributeObjectCore(currentAttribute.Content, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.0 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeListCore"/> class.
        /// </summary>
        /// <param name="keyFamily">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AttributeListCore(KeyFamilyType keyFamily, IMaintainableObject parent)
            : base(
                AttributeListObject.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor), 
                parent)
        {
            this.attributes = new List<IAttributeObject>();
            if (keyFamily.Components != null && keyFamily.Components.Attribute != null)
            {
                foreach (AttributeType currentAttribute in keyFamily.Components.Attribute)
                {
                    this.attributes.Add(new AttributeObjectCore(currentAttribute, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1.0 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeListCore"/> class.
        /// </summary>
        /// <param name="keyFamily">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AttributeListCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.KeyFamilyType keyFamily, IMaintainableObject parent)
            : base(
                AttributeListObject.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor), 
                parent)
        {
            this.attributes = new List<IAttributeObject>();
            if (keyFamily.Components != null && keyFamily.Components.Attribute != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttributeType currentAttribute in
                    keyFamily.Components.Attribute)
                {
                    this.attributes.Add(new AttributeObjectCore(currentAttribute, this));
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the attribtues.
        /// </summary>
        public virtual IList<IAttributeObject> Attributes
        {
            get
            {
                return new List<IAttributeObject>(this.attributes);
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return AttributeListObject.FixedId;
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
                var that = (IAttributeList)sdmxObject;
                if (!this.Equivalent(this.attributes, that.Attributes, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES				 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       ///   The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal()
       {
        	ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this.attributes, composites);
            return composites;
       }

        #endregion
    }
}
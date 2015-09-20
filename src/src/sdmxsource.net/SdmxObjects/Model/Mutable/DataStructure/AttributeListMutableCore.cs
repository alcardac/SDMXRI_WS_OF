// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeListMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attribute list mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The attribute list mutable core.
    /// </summary>
    [Serializable]
    public class AttributeListMutableCore : IdentifiableMutableCore, IAttributeListMutableObject
    {
        #region Fields

        /// <summary>
        ///   The attributes.
        /// </summary>
        private readonly IList<IAttributeMutableObject> _attributes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AttributeListMutableCore" /> class.
        /// </summary>
        public AttributeListMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor))
        {
            this._attributes = new List<IAttributeMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeListMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public AttributeListMutableCore(IAttributeList objTarget)
            : base(objTarget)
        {
            this._attributes = new List<IAttributeMutableObject>();
            if (objTarget.Attributes != null)
            {
                foreach (IAttributeObject currentAttribute in objTarget.Attributes)
                {
                    this._attributes.Add(new AttributeMutableCore(currentAttribute));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attributes.
        /// </summary>
        public IList<IAttributeMutableObject> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specifiedd attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute. 
        /// </param>
        public void AddAttribute(IAttributeMutableObject attribute)
        {
            this._attributes.Add(attribute);
        }

        #endregion
    }
}
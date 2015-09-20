// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CubeRegionMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cube region mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Util;


    /// <summary>
    ///   The cube region mutable core.
    /// </summary>
    [Serializable]
    public class CubeRegionMutableCore : MutableCore, ICubeRegionMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _attribute values.
        /// </summary>
        private IList<IKeyValuesMutable> _attributeValues;

        /// <summary>
        ///   The _key values.
        /// </summary>
        private IList<IKeyValuesMutable> _keyValues;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CubeRegionMutableCore" /> class.
        /// </summary>
        public CubeRegionMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CubeRegion))
        {
            this._keyValues = new List<IKeyValuesMutable>();
            this._attributeValues = new List<IKeyValuesMutable>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeRegionMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public CubeRegionMutableCore(ICubeRegion immutable)
            : base(immutable)
        {
            this._keyValues = new List<IKeyValuesMutable>();
            this._attributeValues = new List<IKeyValuesMutable>();
            if (ObjectUtil.ValidCollection(immutable.KeyValues))
            {
                foreach (IKeyValues mutable in immutable.KeyValues)
                {
                    this._keyValues.Add(new KeyValuesMutableImpl(mutable));
                }
            }

            if (ObjectUtil.ValidCollection(immutable.AttributeValues))
            {
                foreach (IKeyValues keyValues in immutable.AttributeValues)
                {
                    this._attributeValues.Add(new KeyValuesMutableImpl(keyValues));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attribute values.
        /// </summary>
        public virtual IList<IKeyValuesMutable> AttributeValues
        {
            get
            {
                return this._attributeValues;
            }
        }

        /// <summary>
        ///   Gets the key values.
        /// </summary>
        public virtual IList<IKeyValuesMutable> KeyValues
        {
            get
            {
                return this._keyValues;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add attribute value.
        /// </summary>
        /// <param name="attvalue">
        /// The attvalue. 
        /// </param>
        public virtual void AddAttributeValue(IKeyValuesMutable attvalue)
        {
            this._attributeValues.Add(attvalue);
        }

        /// <summary>
        /// The add key value.
        /// </summary>
        /// <param name="keyvalue">
        /// The keyvalue. 
        /// </param>
        public virtual void AddKeyValue(IKeyValuesMutable inputValue)
        {
            	if (inputValue == null) {
			return;
		}
		
		IKeyValuesMutable foundKvm = null;
		foreach (IKeyValuesMutable kvm in _keyValues) {
			if (kvm.Id != null && kvm.Id.Equals(inputValue.Id)) {
				foundKvm = kvm;
				break;
			}
		}
		
		if (foundKvm == null) {
			// This id doesn't already exist so just add the KeyValuesMutable
			this._keyValues.Add(inputValue);
		} else {
			// A KeyValuesMutable with this id does already exist so merge the inputVlaue

		   

		    var q = (from x in inputValue.KeyValues select x).Distinct();
		    foreach (var itm in q)
		    {
		        foundKvm.AddValue(itm);
		    }
		    
		    /*foreach (String val in inputValue.KeyValues) {
				
                    foreach (String str in foundKvm.KeyValues) {
                        if (str.Equals(val))
                        {
                            break;
                        }
				
                    foundKvm.AddValue(val);
                }
            }*/
		}
        }

        /// <summary>
        /// The create mutable instance.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="ICubeRegion"/> . 
        /// </returns>
        public ICubeRegion CreateMutableInstance(IContentConstraintObject parent)
        {
            return new CubeRegionCore(this, parent);
        }

        #endregion
    }
}
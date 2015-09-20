// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CubeRegionBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cube region core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The cube region core.
    /// </summary>
    [Serializable]
    public class CubeRegionCore : SdmxStructureCore, ICubeRegion
    {
        #region Fields

        /// <summary>
        ///   The attribute values.
        /// </summary>
        private readonly IList<IKeyValues> attributeValues;

        /// <summary>
        ///   The key values.
        /// </summary>
        private readonly IList<IKeyValues> keyValues;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeRegionCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CubeRegionCore(ICubeRegionMutableObject mutable, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CubeRegion), parent)
        {
            this.keyValues = new List<IKeyValues>();
            this.attributeValues = new List<IKeyValues>();
            this.keyValues = new List<IKeyValues>();
            if (ObjectUtil.ValidCollection(mutable.KeyValues))
            {
                foreach (IKeyValuesMutable keyValuesMutable in mutable.KeyValues)
                {
                    if (ObjectUtil.ValidCollection(keyValuesMutable.KeyValues))
                    {
                        this.keyValues.Add(new KeyValuesCore(keyValuesMutable, this));    
                    }
                    
                }
            }

            this.attributeValues = new List<IKeyValues>();
            if (ObjectUtil.ValidCollection(mutable.AttributeValues))
            {
                foreach (IKeyValuesMutable keyValuesMutable in mutable.AttributeValues)
                {
                    if (ObjectUtil.ValidCollection(keyValuesMutable.KeyValues))
                    {
                        this.attributeValues.Add(new KeyValuesCore(keyValuesMutable, this));    
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeRegionCore"/> class.
        /// </summary>
        /// <param name="cubeRegionType">
        /// The cube region type. 
        /// </param>
        /// <param name="negate">
        /// The negate. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CubeRegionCore(CubeRegionType cubeRegionType, bool negate, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CubeRegion), parent)
        {
            this.keyValues = new List<IKeyValues>();
            this.attributeValues = new List<IKeyValues>();

            {
                var attributeValueSetTypes = cubeRegionType.GetTypedAttribute<AttributeValueSetType>();
                if (attributeValueSetTypes != null)
                {
                    foreach (AttributeValueSetType valueSetType in attributeValueSetTypes)
                    {
                        if (!valueSetType.include)
                        {
                            if (negate)
                            {
                                this.attributeValues.Add(new KeyValuesCore(valueSetType, this));
                            }
                        }
                        else if (!negate)
                        {
                            this.attributeValues.Add(new KeyValuesCore(valueSetType, this));
                        }
                    }
                }
            }

            var cubeRegionKeyTypes = cubeRegionType.GetTypedKeyValue<CubeRegionKeyType>();
            if (cubeRegionKeyTypes != null)
            {
                foreach (var valueSetType0 in cubeRegionKeyTypes)
                {
                    if (!valueSetType0.include)
                    {
                        if (negate)
                        {
                            this.keyValues.Add(new KeyValuesCore(valueSetType0, this));
                        }
                    }
                    else if (!negate)
                    {
                        this.keyValues.Add(new KeyValuesCore(valueSetType0, this));
                    }
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the attribute values.
        /// </summary>
        public virtual IList<IKeyValues> AttributeValues
        {
            get
            {
                return new List<IKeyValues>(this.attributeValues);
            }
        }

        /// <summary>
        ///   Gets the key values.
        /// </summary>
        public virtual IList<IKeyValues> KeyValues
        {
            get
            {
                return new List<IKeyValues>(this.keyValues);
            }
        }

        public ISet<string> GetValues(string componentId)
        {
           foreach(IKeyValues kvs in keyValues) {
			if(kvs.Id.Equals(componentId)) {
				return new HashSet<string>(kvs.Values);
			}
		}
		foreach(IKeyValues kvs in attributeValues) 
        {
			if(kvs.Id.Equals(componentId)) {
				return new HashSet<String>(kvs.Values);
			}
		}
		    return new HashSet<String>();
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
                var that = (ICubeRegion)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.keyValues, that.KeyValues))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.attributeValues, that.AttributeValues))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES		                     //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
    	    ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this.keyValues, composites);
            base.AddToCompositeSet(this.attributeValues, composites);
            return composites;
        }

        #endregion
    }
}
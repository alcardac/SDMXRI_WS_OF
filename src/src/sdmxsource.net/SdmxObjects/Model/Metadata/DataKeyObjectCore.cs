// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataKeyBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data key object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The data key object core.
    /// </summary>
    [Serializable]
    public class DataKeyObjectCore : SdmxObjectCore, IDataKey
    {
        #region Fields

        /// <summary>
        ///   The included.
        /// </summary>
        private readonly bool included;

        /// <summary>
        ///   The key value.
        /// </summary>
        private readonly IKeyValue keyValue;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataKeyObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="type">
        /// The type. 
        /// </param>
        public DataKeyObjectCore(IReferenceValue parent, ComponentValueSetType type)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues), parent)
        {
            this.included = type.include;
            this.keyValue = new KeyValueImpl(type.Value[0].TypedValue, type.id);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether included.
        /// </summary>
        public virtual bool Included
        {
            get
            {
                return this.included;
            }
        }

        /// <summary>
        ///   Gets the key value.
        /// </summary>
        public virtual IKeyValue KeyValue
        {
            get
            {
                return this.keyValue;
            }
        }

        #endregion

        ////////////DEEP EQUALS							 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
	    public  override  bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties) 
        {
            if (sdmxObject == null)
            {
			   return false;
		    }
            if (sdmxObject.StructureType == this.StructureType)
            {
               IDataKey that = (IDataKey)sdmxObject;
			   if(this.included != that.Included)
               {
				   return false;
			   }
			   if(!string.Equals(this.keyValue, that.KeyValue)) 
               {
				   return false;
			   }

			   return base.DeepEqualsInternal(that, includeFinalProperties);
		    }

	      	return false;
	    }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
	    ////////////COMPOSITES		                     //////////////////////////////////////////////////
	    ///////////////////////////////////////////////////////////////////////////////////////////////////
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            return new HashSet<ISdmxObject>();
        }
    }
}
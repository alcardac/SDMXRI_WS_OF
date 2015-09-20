// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintDataKeySetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint data key set core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The constraint data key set core.
    /// </summary>
    [Serializable]
    public class ConstraintDataKeySetCore : SdmxStructureCore, IConstraintDataKeySet
    {
        #region Fields

        /// <summary>
        ///   The contstrained keys.
        /// </summary>
        private readonly IList<IConstrainedDataKey> contstrainedKeys;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintDataKeySetCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ConstraintDataKeySetCore(IConstraintDataKeySetMutableObject mutable, IConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKeyset), parent)
        {
            this.contstrainedKeys = new List<IConstrainedDataKey>();

            foreach (IConstrainedDataKeyMutableObject each in mutable.ConstrainedDataKeys)
            {
                IConstrainedDataKey cdk = new ConstrainedDataKeyCore(each, this);
                if (ObjectUtil.ValidCollection(cdk.KeyValues))
                {
                    this.contstrainedKeys.Add(cdk);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintDataKeySetCore"/> class.
        /// </summary>
        /// <param name="dataKeySetType">
        /// The data key set type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ConstraintDataKeySetCore(DataKeySetType dataKeySetType, IConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKeyset), parent)
        {
            this.contstrainedKeys = new List<IConstrainedDataKey>();
            if (dataKeySetType.Key != null)
            {
                foreach (DistinctKeyType currentKey in dataKeySetType.Key)
                {
                    IConstrainedDataKey cdk = new ConstrainedDataKeyCore(currentKey, this);
                    if (ObjectUtil.ValidCollection(cdk.KeyValues))
                    {
                        this.contstrainedKeys.Add(cdk);
                    }
                }
            }
        }


        public ConstraintDataKeySetCore(MetadataKeySetType mdKeySetType, IConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKeyset), parent)
        {
            if (mdKeySetType.Key != null)
            {
                foreach (DistinctKeyType currentKey in mdKeySetType.Key)
                {
                    IConstrainedDataKey cdk = new ConstrainedDataKeyCore(currentKey, this);
                    if (ObjectUtil.ValidCollection(cdk.KeyValues))
                    {
                        contstrainedKeys.Add(cdk);
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
        ///   Gets the constrained data keys.
        /// </summary>
        public virtual IList<IConstrainedDataKey> ConstrainedDataKeys
        {
            get
            {
                return new List<IConstrainedDataKey>(this.contstrainedKeys);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The dataStructureObject. 
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
                var that = (IConstraintDataKeySet)sdmxObject;
                if (!this.Equivalent(this.contstrainedKeys, that.ConstrainedDataKeys, includeFinalProperties))
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
           base.AddToCompositeSet(this.contstrainedKeys, composites);
           return composites;
       }

       #endregion
    }
}
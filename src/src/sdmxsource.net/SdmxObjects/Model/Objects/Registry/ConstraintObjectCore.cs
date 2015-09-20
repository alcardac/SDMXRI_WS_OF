// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   The constraint object core.
    /// </summary>
    /// <typeparam name="T"> Generic type param of type IMaintainableObject </typeparam>
    /// <typeparam name="K"> Generic type param of type IMaintainableMutableObject </typeparam>
    [Serializable]
    public abstract class ConstraintObjectCore<T, K> : MaintainableObjectCore<T, K>, IConstraintObject
        where T : IMaintainableObject where K : IMaintainableMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _constraint attachment.
        /// </summary>
        private readonly IConstraintAttachment _constraintAttachment;

        /// <summary>
        ///   The _excluded series keys.
        /// </summary>
        private IConstraintDataKeySet _excludedSeriesKeys;

        /// <summary>
        ///   The _included series keys.
        /// </summary>
        private IConstraintDataKeySet _includedSeriesKeys;

        private readonly IConstraintDataKeySet _includedMetadataKeys;

        private readonly IConstraintDataKeySet _excludedMetadataKeys;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstraintObjectCore{T,K}" /> class.
        /// </summary>
        /// <param name="agencyScheme"> The agencySchemeMutable. </param>
        /// <param name="actualLocation"> The actual location. </param>
        /// <param name="isServiceUrl"> The is service url. </param>
        protected ConstraintObjectCore(IMaintainableObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstraintObjectCore{T,K}" /> class.
        /// </summary>
        /// <param name="itemMutableObject"> The agencySchemeMutable. </param>
        protected ConstraintObjectCore(IConstraintMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            if (this.MaintainableParent.IsExternalReference.IsTrue)
            {
                return;
            }

            if (itemMutableObject.IncludedSeriesKeys != null
                && itemMutableObject.IncludedSeriesKeys.ConstrainedDataKeys != null
                && itemMutableObject.IncludedSeriesKeys.ConstrainedDataKeys.Count > 0)
            {
                this._includedSeriesKeys = new ConstraintDataKeySetCore(itemMutableObject.IncludedSeriesKeys, this);
            }

            if (itemMutableObject.ExcludedSeriesKeys != null
                && itemMutableObject.ExcludedSeriesKeys.ConstrainedDataKeys != null
                && itemMutableObject.ExcludedSeriesKeys.ConstrainedDataKeys.Count > 0)
            {
                this._excludedSeriesKeys = new ConstraintDataKeySetCore(itemMutableObject.ExcludedSeriesKeys, this);
            }

            if (itemMutableObject.IncludedMetadataKeys != null
                && itemMutableObject.IncludedMetadataKeys.ConstrainedDataKeys != null
                && itemMutableObject.IncludedMetadataKeys.ConstrainedDataKeys.Count > 0)
            {
                this._includedMetadataKeys = new ConstraintDataKeySetCore(itemMutableObject.IncludedSeriesKeys, this);
            }

            if (itemMutableObject.ExcludedMetadataKeys != null
                && itemMutableObject.ExcludedMetadataKeys.ConstrainedDataKeys != null
                && itemMutableObject.ExcludedMetadataKeys.ConstrainedDataKeys.Count > 0)
            {
                this._excludedMetadataKeys = new ConstraintDataKeySetCore(itemMutableObject.ExcludedSeriesKeys, this);
            }

            if (itemMutableObject.ConstraintAttachment != null)
            {
                this._constraintAttachment = new ConstraintAttachmentCore(itemMutableObject.ConstraintAttachment, this);
            }

            Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintObjectCore{T,K}" /> class.
        /// </summary>
        /// <param name="createdFrom">The created from.</param>
        /// <param name="structureType">The structure type.</param>
        /// <param name="constraintAttachmentType">Type of the constraint attachment.</param>
        /// <exception cref="SdmxNotImplementedException">Throws Unsupported Exception.</exception>
        protected ConstraintObjectCore(ConstraintType createdFrom, SdmxStructureType structureType, ConstraintAttachmentType constraintAttachmentType)
            : base(createdFrom, structureType)
        {
            if (ObjectUtil.ValidCollection(createdFrom.DataKeySet))
            {
                var includedKeySet = new DataKeySetType();
                var excludedKeySet = new DataKeySetType();

                this.PopulateKeySets(createdFrom.DataKeySet, includedKeySet, excludedKeySet);

                if (includedKeySet.Key.Count > 0)
                {
                    this._includedSeriesKeys = new ConstraintDataKeySetCore(includedKeySet, this);
                }

                if (excludedKeySet.Key.Count > 0)
                {
                    this._excludedSeriesKeys = new ConstraintDataKeySetCore(excludedKeySet, this);
                }
            }

            if (ObjectUtil.ValidCollection(createdFrom.MetadataKeySet))
            {
                var includedMetadataKeySet = new DataKeySetType();
                var excludedMetadataKeySet = new DataKeySetType();

                this.PopulateMetadataKeySets(createdFrom.MetadataKeySet, includedMetadataKeySet, excludedMetadataKeySet);

                if (includedMetadataKeySet.Key.Count > 0)
                {
                    this._includedMetadataKeys = new ConstraintDataKeySetCore(includedMetadataKeySet, this);
                }

                if (excludedMetadataKeySet.Key.Count > 0)
                {
                    this._excludedMetadataKeys = new ConstraintDataKeySetCore(excludedMetadataKeySet, this);
                }
            }

            if (constraintAttachmentType != null)
            {
                this._constraintAttachment = new ConstraintAttachmentCore(constraintAttachmentType, this);
            }

            this.Validate();
        }

        private void PopulateMetadataKeySets(
            IList<MetadataKeySetType> allKeys, DataKeySetType includedKeySet, DataKeySetType excludedKeySet)
        {
            foreach (MetadataKeySetType currentDataKeySet in allKeys)
            {
                if (currentDataKeySet.isIncluded)
                {
                    foreach (DistinctKeyType currentKey in currentDataKeySet.Key)
                    {
                        if (!currentKey.include)
                        {
                            excludedKeySet.Key.Add(currentKey);
                        }
                        else
                        {
                            includedKeySet.Key.Add(currentKey);
                        }
                    }
                }
                else
                {
                    foreach (DistinctKeyType currentKey in currentDataKeySet.Key)
                    {
                        if (!currentKey.include)
                        {
                            excludedKeySet.Key.Add(currentKey);
                        }
                        else
                        {
                            includedKeySet.Key.Add(currentKey);
                        }
                    }
                }
            }
        }

        private void PopulateKeySets(
            IList<DataKeySetType> allKeys, DataKeySetType includedKeySet, DataKeySetType excludedKeySet)
        {
            foreach (DataKeySetType currentDataKeySet in allKeys)
            {
                if (currentDataKeySet.isIncluded)
                   //INCLUDED
                {
                    foreach (DistinctKeyType currentKey in currentDataKeySet.Key)
                    {
                        if (!currentKey.include)
                        {
                            //EXCLUDED (isInclude=false)
                            excludedKeySet.Key.Add(currentKey);
                        }
                        else
                        {
                            //INCLUDED
                            includedKeySet.Key.Add(currentKey);
                        }
                    }
                }
                else
                {
                    //EXCLUDED
                    foreach (DistinctKeyType currentKey in currentDataKeySet.Key)
                    {
                        if (!currentKey.include)
                        {
                            //INCLUDED (include=false on an already excluded list)
                            includedKeySet.Key.Add(currentKey);
                        }
                        else
                        {
                            //EXCLUDED
                            excludedKeySet.Key.Add(currentKey);
                        }
                    }
                }
            }
        }

        private void Validate()
        {
            if (_includedSeriesKeys != null && !ObjectUtil.ValidCollection(_includedSeriesKeys.ConstrainedDataKeys))
            {
                this._includedSeriesKeys = null;
            }
            if (_excludedSeriesKeys != null && !ObjectUtil.ValidCollection(_excludedSeriesKeys.ConstrainedDataKeys))
            {
                this._excludedSeriesKeys = null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the constraint attachment.
        /// </summary>
        public virtual IConstraintAttachment ConstraintAttachment
        {
            get
            {
                return this._constraintAttachment;
            }
        }

        /// <summary>
        ///   Gets the excluded series keys.
        /// </summary>
        public virtual IConstraintDataKeySet ExcludedSeriesKeys
        {
            get
            {
                return this._excludedSeriesKeys;
            }
        }

        /// <summary>
        ///   Gets the included series keys.
        /// </summary>
        public virtual IConstraintDataKeySet IncludedSeriesKeys
        {
            get
            {
                return this._includedSeriesKeys;
            }
        }

        public IConstraintDataKeySet IncludedMetadataKeys
        {
            get
            {
                return _includedMetadataKeys;
            }
        }

        public IConstraintDataKeySet ExcludedMetadataKeys
        {
            get
            {
                return _excludedMetadataKeys;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The deep equals internal.
        /// </summary>
        /// <param name="constraintObject"> The agencySchemeMutable. </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns> The <see cref="bool" /> . </returns>
        protected internal bool DeepEqualsInternal(IConstraintObject constraintObject, bool includeFinalProperties)
        {
            if (constraintObject == null)
            {
                return false;
            }

            if (constraintObject.StructureType == this.StructureType)
            {
                IConstraintObject that = constraintObject;
                if (!this.Equivalent(this._includedSeriesKeys, that.IncludedSeriesKeys, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._excludedSeriesKeys, that.ExcludedSeriesKeys, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._constraintAttachment, that.ConstraintAttachment, includeFinalProperties))
                {
                    return false;
                }

                return base.DeepEqualsInternal(that, includeFinalProperties);
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
		   base.AddToCompositeSet(this._includedSeriesKeys, composites);
		   base.AddToCompositeSet(this._excludedSeriesKeys, composites);
		   base.AddToCompositeSet(this._includedMetadataKeys, composites);
		   base.AddToCompositeSet(this._excludedMetadataKeys, composites);
		   base.AddToCompositeSet(this._constraintAttachment, composites);
		   return composites;
	   }

       #endregion
    }
}
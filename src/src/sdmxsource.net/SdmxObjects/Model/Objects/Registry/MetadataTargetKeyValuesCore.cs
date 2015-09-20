// -----------------------------------------------------------------------
// <copyright file="MetadataTargetKeyValuesCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class MetadataTargetKeyValuesCore : KeyValuesCore, IMetadataTargetKeyValues
    {
        private readonly IList<ICrossReference> _objectReferences;

        private readonly IList<IDataSetReference> _datasetReferences;

        public MetadataTargetKeyValuesCore(IMetadataTargetKeyValuesMutable mutable, IMetadataTargetRegion parent)
            : base(mutable, parent)
        {
            _objectReferences = new List<ICrossReference>();
            _datasetReferences = new List<IDataSetReference>();

            if (mutable.ObjectReferences != null)
            {
                foreach (IStructureReference sRef in mutable.ObjectReferences)
                {
                    this._objectReferences.Add(new CrossReferenceImpl(this, sRef));
                }
            }
            if (mutable.DatasetReferences != null)
            {
                foreach (IDataSetReferenceMutableObject currentRef in mutable.DatasetReferences)
                {
                    this._datasetReferences.Add(new DataSetReferenceCore(currentRef, this));
                }
            }
        }

        public MetadataTargetKeyValuesCore(ComponentValueSetType keyValueType, IMetadataTargetRegion parent)
            : base(keyValueType, parent)
        {
            _objectReferences = new List<ICrossReference>();
            _datasetReferences = new List<IDataSetReference>();

            if (keyValueType.DataSet != null)
            {
                foreach (SetReferenceType currentDatasetRef in keyValueType.DataSet)
                {
                    this._datasetReferences.Add(new DataSetReferenceCore(currentDatasetRef, this));
                }
            }
            if (keyValueType.Object != null)
            {
                foreach (ObjectReferenceType currentRef in keyValueType.Object)
                {
                    this._objectReferences.Add(RefUtil.CreateReference(this, currentRef));
                }
            }
        }

        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null)
            {
                return false;
            }
            var that = sdmxObject as IMetadataTargetKeyValues;
            if (that != null)
            {
                if (!ObjectUtil.EquivalentCollection(this._objectReferences, that.ObjectReferences))
                {
                    return false;
                }
                if (!base.Equivalent(this._datasetReferences, that.DatasetReferences, includeFinalProperties))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        #region Implementation of IMetadataTargetKeyValues

        public IList<ICrossReference> ObjectReferences
        {
            get
            {
                return new List<ICrossReference>(_objectReferences);
            }
        }

        public IList<IDataSetReference> DatasetReferences
        {
            get
            {
                return new List<IDataSetReference>(_datasetReferences);
            }
        }

        #endregion

        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////COMPOSITES				 //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
		    ISet<ISdmxObject> composites = base.GetCompositesInternal();
		    base.AddToCompositeSet(this._datasetReferences, composites);
		    return composites;
	    }

        #endregion
    }
}

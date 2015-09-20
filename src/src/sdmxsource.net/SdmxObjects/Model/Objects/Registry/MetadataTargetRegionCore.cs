// -----------------------------------------------------------------------
// <copyright file="MetadataTargetRegionCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class MetadataTargetRegionCore : SdmxStructureCore, IMetadataTargetRegion
    {
        private readonly bool _isInclude;

        private readonly string _report;

        private readonly string _metadataTarget;

        private readonly IList<IMetadataTargetKeyValues> _key;

        private readonly IList<IKeyValues> _attributes;

        public MetadataTargetRegionCore(IMetadataTargetRegionMutableObject mutable, IContentConstraintObject parent)
            : base(mutable, parent)
        {
            _key = new List<IMetadataTargetKeyValues>();
            _attributes = new List<IKeyValues>();

            this._report = mutable.Report;
            this._metadataTarget = mutable.MetadataTarget;
            if (mutable.Key != null)
            {
                foreach (IMetadataTargetKeyValuesMutable currentMetadataTarget in mutable.Key)
                {
                    this._key.Add(new MetadataTargetKeyValuesCore(currentMetadataTarget, this));
                }
            }
            if (mutable.Attributes != null)
            {
                foreach (IKeyValuesMutable currentKeyValue in mutable.Attributes)
                {
                    this._attributes.Add(new KeyValuesCore(currentKeyValue, this));
                }
            }
            try
            {
                Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        public MetadataTargetRegionCore(MetadataTargetRegionType type, IContentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTargetRegion), parent)
        {
            _key = new List<IMetadataTargetKeyValues>();
            _attributes = new List<IKeyValues>();

            this._isInclude = type.include;
            this._report = type.Report;
            this._metadataTarget = type.MetadataTarget;
            var metadataTargetRegionKeyTypes = type.GetTypedKeyValue<MetadataTargetRegionKeyType>();
            if (metadataTargetRegionKeyTypes != null)
            {
                foreach (MetadataTargetRegionKeyType cv in metadataTargetRegionKeyTypes)
                {
                    this._key.Add(new MetadataTargetKeyValuesCore(cv, this));
                }
            }
            var metadataAttributeValueSetTypes = type.GetTypedAttribute<MetadataAttributeValueSetType>();
            if (metadataAttributeValueSetTypes != null)
            {
                foreach (var cv in metadataAttributeValueSetTypes)
                {
                    this._attributes.Add(new KeyValuesCore(cv, this));
                }
            }
            try
            {
                Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        private void Validate()
        {
            if (!ObjectUtil.ValidString(_report))
            {
                throw new SdmxSemmanticException("Metadata Target Region missing mandatory 'report' identifier");
            }
            if (!ObjectUtil.ValidString(_metadataTarget))
            {
                throw new SdmxSemmanticException("Metadata Target Region missing mandatory 'metadata target' identifier");
            }
        }

        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null)
            {
                return false;
            }

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IMetadataTargetRegion)sdmxObject;

                if (!base.Equivalent(this._attributes, that.Attributes, includeFinalProperties))
                {
                    return false;
                }
                if (!base.Equivalent(this._key, that.Key, includeFinalProperties))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(this._metadataTarget, that.MetadataTarget))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(this._report, that.Report))
                {
                    return false;
                }
                if (this._isInclude != that.IsInclude)
                {
                    return false;
                }
            }
            return false;
        }

        #region Implementation of IMetadataTargetRegion

        public bool IsInclude
        {
            get
            {
                return this._isInclude;
            }
        }

        public string Report
        {
            get
            {
                return this._report;
            }
        }

        public string MetadataTarget
        {
            get
            {
                return this._metadataTarget;
            }
        }

        public IList<IMetadataTargetKeyValues> Key
        {
            get
            {
                return new List<IMetadataTargetKeyValues>(this._key);
            }
        }

        public IList<IKeyValues> Attributes
        {
            get
            {
                return new List<IKeyValues>(this._attributes);
            }
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
            base.AddToCompositeSet(this._key, composites);
            base.AddToCompositeSet(this._attributes, composites);
            return composites;
        }

        #endregion
    }
}

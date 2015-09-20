// -----------------------------------------------------------------------
// <copyright file="DataSetReferenceCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    #region Using directives

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class DataSetReferenceCore : SdmxStructureCore, IDataSetReference
    {
        private readonly string _datasetId;

        private readonly ICrossReference _dataProviderReference;

        public DataSetReferenceCore(IDataSetReferenceMutableObject mutableObject, IMetadataTargetKeyValues parent)
            : base(mutableObject, parent)
        {
            this._datasetId = mutableObject.DatasetId;
            if (mutableObject.DataProviderReference != null)
            {
                this._dataProviderReference = new CrossReferenceImpl(this, mutableObject.DataProviderReference);
                try
                {
                    Validate();
                }
                catch (SdmxSemmanticException e)
                {
                    throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
                }
            }
        }

        public DataSetReferenceCore(SetReferenceType sRefType, IMetadataTargetKeyValues parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DatasetReference), parent)
        {
            this._datasetId = sRefType.ID;
            this._dataProviderReference = RefUtil.CreateReference(this, sRefType.DataProvider);
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
            if (!ObjectUtil.ValidString(_datasetId))
            {
                throw new SdmxSemmanticException("Dataset Reference missing mandatory 'id' identifier");
            }
            if (_dataProviderReference == null)
            {
                throw new SdmxSemmanticException("Dataset Reference missing mandatory 'data provider reference'");
            }
        }

        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null)
            {
                return false;
            }
            var that = sdmxObject as IDataSetReference;
            if (that != null)
            {
                if (!ObjectUtil.Equivalent(this.DataProviderReference, that.DataProviderReference))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(this.DatasetId, that.DatasetId))
                {
                    return false;
                }
            }
            return false;
        }

        #region Implementation of IDataSetReference

        public string DatasetId
        {
            get
            {
                return _datasetId;
            }
        }

        public ICrossReference DataProviderReference
        {
            get
            {
                return _dataProviderReference;
            }
        }

        #endregion
    }
}

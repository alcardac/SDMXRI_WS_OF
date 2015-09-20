// -----------------------------------------------------------------------
// <copyright file="DataSetReferenceMutableObjectCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataSetReferenceMutableObjectCore : MutableCore, IDataSetReferenceMutableObject
    {
        private string datasetId;
        private IStructureReference dataProviderRef;

        public DataSetReferenceMutableObjectCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DatasetReference))
        {
        }

        public DataSetReferenceMutableObjectCore(IDataSetReference createdFrom)
            : base(createdFrom)
        {
            if (createdFrom.DataProviderReference != null)
            {
                this.dataProviderRef = new StructureReferenceImpl(createdFrom.DataProviderReference.TargetUrn);
            }
            this.datasetId = createdFrom.DatasetId;
        }

        #region Implementation of IDataSetReferenceMutableObject

        public string DatasetId
        {
            get
            {
                return datasetId;
            }
            set
            {
                datasetId = value;
            }
        }

        public IStructureReference DataProviderReference
        {
            get
            {
                return dataProviderRef;
            }
            set
            {
                dataProviderRef = value;
            }
        }

        #endregion
    }
}

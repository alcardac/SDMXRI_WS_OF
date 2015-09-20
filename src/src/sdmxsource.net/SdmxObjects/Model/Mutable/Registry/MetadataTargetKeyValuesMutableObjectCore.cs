// -----------------------------------------------------------------------
// <copyright file="MetadataTargetKeyValuesMutableObjectCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class MetadataTargetKeyValuesMutableObjectCore : KeyValuesMutableImpl, IMetadataTargetKeyValuesMutable
    {
        private readonly IList<IStructureReference> objectReferences = new List<IStructureReference>();

        private readonly IList<IDataSetReferenceMutableObject> datasetReferences = new List<IDataSetReferenceMutableObject>();

        public MetadataTargetKeyValuesMutableObjectCore()
        {
        }

        public MetadataTargetKeyValuesMutableObjectCore(IMetadataTargetKeyValues createdFrom)
            : base(createdFrom)
        {
            foreach (ICrossReference crossRef in createdFrom.ObjectReferences)
            {
                this.objectReferences.Add(new StructureReferenceImpl(crossRef.TargetUrn));
            }
            foreach (IDataSetReference dsRef in createdFrom.DatasetReferences)
            {
                this.datasetReferences.Add(new DataSetReferenceMutableObjectCore(dsRef));
            }
        }

        #region Implementation of IMetadataTargetKeyValuesMutable

        public IList<IStructureReference> ObjectReferences
        {
            get
            {
                return this.objectReferences;
            }
        }

        public void AddObjectReference(IStructureReference sRef)
        {
            this.objectReferences.Add(sRef);
        }

        public IList<IDataSetReferenceMutableObject> DatasetReferences
        {
            get
            {
                return datasetReferences;
            }
        }

        public void AddDatasetReference(IDataSetReferenceMutableObject reference)
        {
            this.datasetReferences.Add(reference);
        }

        #endregion
    }
}

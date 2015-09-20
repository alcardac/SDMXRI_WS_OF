// -----------------------------------------------------------------------
// <copyright file="MetadataflowCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MetadataflowCrossReferenceUpdaterEngine : IMetadataflowCrossReferenceUpdaterEngine
    {
        public IMetadataFlow UpdateReferences(IMetadataFlow maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, String newVersionNumber)
        {
            IMetadataFlowMutableObject mdf = maintianable.MutableInstance;
            mdf.Version = newVersionNumber;

            IStructureReference newTarget = updateReferences[mdf.MetadataStructureRef];

            if (newTarget != null)
            {
                mdf.MetadataStructureRef = newTarget;
            }

            return mdf.ImmutableInstance;
        }
    }
}

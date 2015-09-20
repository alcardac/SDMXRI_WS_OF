// -----------------------------------------------------------------------
// <copyright file="DataflowCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataflowCrossReferenceUpdaterEngine : IDataflowCrossReferenceUpdaterEngine
    {
        /// <summary>
        /// The update references.
        /// </summary>
        /// <param name="maintianable">
        /// The maintianable.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        /// <param name="newVersionNumber">
        /// The new version number.
        /// </param>
        /// <returns>
        /// The <see cref="IDataflowObject"/>.
        /// </returns>
        public IDataflowObject UpdateReferences(IDataflowObject maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, string newVersionNumber)
        {
            IDataflowMutableObject df = maintianable.MutableInstance;
            df.Version = newVersionNumber;

            IStructureReference newTarget = updateReferences[df.DataStructureRef];

            if (newTarget != null)
            {
                df.DataStructureRef = newTarget;
            }

            return df.ImmutableInstance;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="DataStructureCrossReferenceUpdaterEngine.cs" company="Microsoft">
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
    public class DataStructureCrossReferenceUpdaterEngine : ComponentCrossReferenceUpdater, IDataStructureCrossReferenceUpdaterEngine
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
        /// The <see cref="IDataStructureObject"/>.
        /// </returns>
        public IDataStructureObject UpdateReferences(IDataStructureObject maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, String newVersionNumber)
        {
            IDataStructureMutableObject dsd = maintianable.MutableInstance;
            dsd.Version = newVersionNumber;

            if (dsd.DimensionList != null && dsd.DimensionList.Dimensions != null)
            {
                UpdateComponentReferneces(dsd.DimensionList.Dimensions, updateReferences);
            }
            if (dsd.AttributeList != null && dsd.AttributeList.Attributes != null)
            {
                UpdateComponentReferneces(dsd.AttributeList.Attributes, updateReferences);
            }
            if (dsd.MeasureList != null && dsd.MeasureList.PrimaryMeasure != null)
            {
                UpdateComponentReferneces(dsd.MeasureList.PrimaryMeasure, updateReferences);
            }
            return dsd.ImmutableInstance;
        }
    }
}

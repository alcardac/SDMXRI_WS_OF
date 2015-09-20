// -----------------------------------------------------------------------
// <copyright file="ContentConstraintCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ContentConstraintCrossReferenceUpdaterEngine : IContentConstraintCrossReferenceUpdaterEngine
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
        /// The <see cref="IContentConstraintObject"/>.
        /// </returns>
        public IContentConstraintObject UpdateReferences(IContentConstraintObject maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, string newVersionNumber)
        {
            return null;
        }
    }
}

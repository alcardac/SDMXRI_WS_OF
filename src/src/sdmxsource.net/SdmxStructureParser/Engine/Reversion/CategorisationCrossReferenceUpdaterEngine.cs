// -----------------------------------------------------------------------
// <copyright file="CategorisationCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update
    /// </summary>
    public class CategorisationCrossReferenceUpdaterEngine : ICategorisationCrossReferenceUpdaterEngine
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
        /// The <see cref="ICategorisationObject"/>.
        /// </returns>
        public ICategorisationObject UpdateReferences(ICategorisationObject maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, string newVersionNumber)
        {
            ICategorisationMutableObject categorisationMutable = maintianable.MutableInstance;

            IStructureReference target = updateReferences[categorisationMutable.CategoryReference];
            if (target != null)
            {
                categorisationMutable.CategoryReference = target;
            }
            target = updateReferences[categorisationMutable.StructureReference];
            if (target != null)
            {
                categorisationMutable.StructureReference = target;
            }
            return categorisationMutable.ImmutableInstance;
        }
    }
}

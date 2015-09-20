// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceReversionEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICrossReferenceReversionEngine
    {
        /// <summary>
        /// Updates the references of the given maintainable and reversions the maintainable structure
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
        /// The <see cref="IMaintainableObject"/>.
        /// </returns>
        IMaintainableObject UdpateReferences(IMaintainableObject maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, string newVersionNumber);
    }
}

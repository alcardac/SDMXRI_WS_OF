// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceUpdaterEngine.cs" company="Microsoft">
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
    /// <typeparam name="T"> The Maintainable Object
    /// </typeparam>
    public interface ICrossReferenceUpdaterEngine<T> where T : IMaintainableObject
    {
        /// <summary>
        /// Updates the maintainable (T) by altering any references that exist in the key of the updateReferences Map, to the corresponding value in the Map, the version
        /// of the maintainable is also changed to the newVersionNumber.
        /// </summary>
        /// <param name="maintianable">
        /// The maintainable to alter
        /// </param>
        /// <param name="updateReferences">
        /// The references from/to map
        /// </param>
        /// <param name="newVersionNumber">
        /// The version number to give the new maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="T"/> newly created maintainable.
        /// </returns>
        T UpdateReferences(T maintianable, IDictionary<IStructureReference, IStructureReference> updateReferences, string newVersionNumber);
    }
}

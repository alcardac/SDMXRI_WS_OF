// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExternalReferenceManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Manages the retrieval of externally referenced structures.  These are determined by the isExternal attribute of a maintainable artifact being set
//   to true, and the URI attribute containing the URI of the full artifact.
//   <p />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    ///     Manages the retrieval of externally referenced structures.  These are determined by the isExternal attribute of a maintainable artifact being set
    ///     to true, and the URI attribute containing the URI of the full artifact.
    ///     <p />
    /// </summary>
    public interface IExternalReferenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Resolves external references, where the 'externalReference' attribute is set to 'true'.
        ///     The external reference locations are expected to be given by a URI, and the URI is expected to point to the
        ///     location of a valid SDMX document containing the referenced structure.
        ///     <p/>
        ///     External references can be of a different version to those that created the input StructureBeans.
        /// </summary>
        /// <param name="structures">
        /// containing structures which may have the external reference attribute set to `true`
        /// </param>
        /// <param name="isSubstitute">
        /// if set to true, this will substitute the external reference beans for the real beans
        /// </param>
        /// <param name="isLenient">
        /// The is Lenient.
        /// </param>
        /// <returns>
        /// a StructureBeans containing only the externally referenced beans
        /// </returns>
        ISdmxObjects ResolveExternalReferences(ISdmxObjects structures, bool isSubstitute, bool isLenient);

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The QueryParsingManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;

    /// <summary>
    ///     The QueryParsingManager interface.
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IQueryParsingManager" />.
    ///     <code source="..\ReUsingExamples\StructureQuery\ReUsingQueryParsingManager.cs" lang="cs" />
    /// </example> 
    public interface IQueryParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Processes the SDMX at the given URI and returns a workspace containing the information on what was being queried.
        ///     <p/>
        ///     The Query parsing manager processes queries that are in a RegistryInterface document, this includes queries for
        ///     Provisions, Registrations and Structures.  It also processes Queries that are in a QueryMessage document
        /// </summary>
        /// <param name="dataLocation">
        /// The SDMX URI
        /// </param>
        /// <returns>
        /// The <see cref="IQueryWorkspace"/>.
        /// </returns>
        IQueryWorkspace ParseQueries(IReadableDataLocation dataLocation);

        #endregion
    }
}
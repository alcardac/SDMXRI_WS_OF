// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The QueryWorkspace interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    // JAVADOC missing

    /// <summary>
    ///     The QueryWorkspace interface.
    /// </summary>
    public interface IQueryWorkspace
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of provision references
        /// </summary>
        IStructureReference ProvisionReferences { get; }

        /// <summary>
        /// Gets a ComplexStructureQuery 
        /// </summary>
        IComplexStructureQuery ComplexStructureQuery { get; }

        /// <summary>
        ///     Gets a list of registration references
        /// </summary>
        IStructureReference RegistrationReferences { get; }

        /// <summary>
        ///     Gets a value indicating whether the structure references should be resolved.
        ///     true if structure references should be resolved
        /// </summary>
        bool ResolveReferences { get; }

        /// <summary>
        ///     Gets a list of simple queries, these are queries by agency id, maintainable id version and id.
        ///     <p />
        ///     More complex queries such as query for DSD by dimension concept are not returned from this method call.
        /// </summary>
        IList<IStructureReference> SimpleStructureQueries { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets true if getProvisionReferences() returns a not null object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasProvisionQueries();

        /// <summary>
        ///     Gets true if getRegistrationReferences() returns a not null object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasRegistrationQueries();

        /// <summary>
        ///     Gets true if getSimpleStructureQueries() returns a list of 1 or more
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasStructureQueries();

        #endregion
    }
}
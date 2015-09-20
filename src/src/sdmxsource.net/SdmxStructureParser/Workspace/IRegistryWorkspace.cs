// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistryWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure workspace holds reference to a SubmitRegistryRequest document,
//   the contents of this document can be Structures, Registrations or Provisions
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using Org.Sdmxsource.Sdmx.Api.Model;

    /// <summary>
    ///     The structure workspace holds reference to a SubmitRegistryRequest document,
    ///     the contents of this document can be Structures, Registrations or Provisions
    /// </summary>
    public interface IRegistryWorkspace
    {
        #region Public Properties

        /// <summary>
        ///     Gets the provision workspace for this workspace
        /// </summary>
        IProvisionWorkspace ProvisionWorkspace { get; }

        /// <summary>
        ///     Gets the registration workspace for this workspace
        /// </summary>
        IRegistrationWorkspace RegistrationWorkspace { get; }

        /// <summary>
        ///     Gets the structure workspace for this workspace
        /// </summary>
        IStructureWorkspace StructureWorkspace { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if getProvisionWorkspace() returns a not null object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasProvisionWorkspace();

        /// <summary>
        ///     Returns true if getRegistrationWorkspace() returns a not null object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasRegitrationWorkspace();

        /// <summary>
        ///     Returns true if getStructureWorkspace() returns a not null object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool HasStructureWorkspace();

        #endregion
    }
}
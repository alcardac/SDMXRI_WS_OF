// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration workspace holds reference to provision agreements, and can also contain the cross referenced structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     The registration workspace holds reference to provision agreements, and can also contain the cross referenced structures.
    /// </summary>
    public interface IRegistrationWorkspace
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of registrations in the workspace
        /// </summary>
        IList<IRegistrationObject> Registrations { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets true if this workspace was built with all the cross references
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool ContainsCrossReferences();

        /// <summary>
        /// Gets the provision reference for the registration
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <returns>
        /// The <see cref="IProvisionAgreementObject"/>.
        /// </returns>
        IProvisionAgreementObject GetProvisionReference(IRegistrationObject registration);

        #endregion
    }
}
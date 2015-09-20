// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision workspace holds reference to provision agreements, and can also contain the cross referenced structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     The provision workspace holds reference to provision agreements, and can also contain the cross referenced structures.
    /// </summary>
    public interface IProvisionWorkspace
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of provision agreements in the workspace
        /// </summary>
        /// <value></value>
        IList<IProvisionAgreementObject> Provisions { get; }

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
        /// Gets the flow references for the provision
        /// </summary>
        /// <param name="provision">
        /// The provision.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/>.
        /// </returns>
        IMaintainableObject GetFlowReference(IProvisionAgreementObject provision);

        /// <summary>
        /// Gets the data provider references for the provision
        /// </summary>
        /// <param name="provision">
        /// The provision.
        /// </param>
        /// <returns>
        /// The <see cref="IDataProvider"/>.
        /// </returns>
        IDataProvider GetProviderReference(IProvisionAgreementObject provision);

        #endregion
    }
}
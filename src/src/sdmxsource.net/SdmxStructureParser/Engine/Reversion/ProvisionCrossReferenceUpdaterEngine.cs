// -----------------------------------------------------------------------
// <copyright file="ProvisionCrossReferenceUpdaterEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ProvisionCrossReferenceUpdaterEngine : IProvisionCrossReferenceUpdaterEngine
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
        /// The <see cref="IProvisionAgreementObject"/>.
        /// </returns>
        public IProvisionAgreementObject UpdateReferences(
            IProvisionAgreementObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IProvisionAgreementMutableObject provision = maintianable.MutableInstance;
            provision.Version = newVersionNumber;

            IStructureReference newTarget = updateReferences[provision.DataproviderRef];
            if (newTarget != null)
            {
                provision.DataproviderRef = newTarget;
            }
            newTarget = updateReferences[provision.StructureUsage];
            if (newTarget != null)
            {
                provision.StructureUsage = newTarget;
            }
            return provision.ImmutableInstance;
        }
    }
}

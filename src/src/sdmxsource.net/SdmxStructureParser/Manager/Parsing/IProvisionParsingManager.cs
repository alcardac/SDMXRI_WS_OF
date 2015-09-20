// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The ProvisionParsingManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The ProvisionParsingManager interface.
    /// </summary>
    public interface IProvisionParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Process a SMDX document to retrieve the Provisions, these can either be in
        ///     a QueryProvisionResponse message or inside a SubmitProvisionRequest message
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the SDMX document
        /// </param>
        /// <returns>
        /// The Provisions from <paramref name="dataLocation"/>
        /// </returns>
        IList<IProvisionAgreementObject> ParseXML(IReadableDataLocation dataLocation);

        #endregion
    }
}
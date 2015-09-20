// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The RegistrationParsingManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The RegistrationParsingManager interface.
    /// </summary>
    public interface IRegistrationParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Process a SMDX document to retrieve the Registrations, these can either be in
        ///     a QueryRegistrationResponse message or inside a SubmitRegistrationRequest message
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the XML file
        /// </param>
        /// <returns>
        /// the Registrations from <paramref name="dataLocation"/>
        /// </returns>
        IList<IRegistrationInformation> ParseRegXML(IReadableDataLocation dataLocation);

        #endregion
    }
}
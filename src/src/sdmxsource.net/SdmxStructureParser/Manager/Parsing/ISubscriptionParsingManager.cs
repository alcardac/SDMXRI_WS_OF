// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubscriptionParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The SubscriptionParsingManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The SubscriptionParsingManager interface.
    /// </summary>
    public interface ISubscriptionParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Process a SMDX document to retrieve the Subscriptions, these are expected to be in a SubmitSubscriptionRequest message
        /// </summary>
        /// <param name="dataLocation">
        /// The location of the SDMX document
        /// </param>
        /// <returns>
        /// The Subscriptions from <paramref name="dataLocation"/>
        /// </returns>
        IList<ISubscriptionObject> ParseSubscriptionXML(IReadableDataLocation dataLocation);

        #endregion
    }
}
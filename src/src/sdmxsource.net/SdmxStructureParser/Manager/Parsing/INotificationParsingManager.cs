// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotificationParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The notification parsing manager is used to parse subscription notification events
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The notification parsing manager is used to parse subscription notification events
    /// </summary>
    public interface INotificationParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parses the XML that is retrieved from the URI to create a notification event.
        ///     Makes sure the notification event is valid
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the SDMX XML file
        /// </param>
        /// <returns>
        /// The <see cref="INotificationEvent"/>.
        /// </returns>
        INotificationEvent CreateNotificationEvent(IReadableDataLocation dataLocation);

        #endregion
    }
}
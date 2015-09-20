// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The notification parsing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.XmlHelper;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Util.Log;

    /// <summary>
    ///     The notification parsing manager implementation.
    /// </summary>
    public class NotificationParsingManager : BaseParsingManager, INotificationParsingManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NotificationParsingManager));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationParsingManager" /> class.
        /// </summary>
        public NotificationParsingManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        internal NotificationParsingManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        #endregion

        // TODO Test that there is a notification event in this message!
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
        public virtual INotificationEvent CreateNotificationEvent(IReadableDataLocation dataLocation)
        {
            LoggingUtil.Debug(_log, "Parse Structure request, for xml at location: " + dataLocation);
            INotificationEvent notificationEvent;

            SdmxSchemaEnumType schemaVersion = this.GetSchemaVersion(dataLocation);
            LoggingUtil.Debug(_log, "Schema Version Determined to be : " + schemaVersion);

            ////XMLParser.ValidateXml(dataLocation, schemaVersion);
            LoggingUtil.Debug(_log, "XML VALID");
            using (Stream stream = dataLocation.InputStream)
            {
                using (XmlReader reader = XMLParser.CreateSdmxMlReader(stream, schemaVersion))
                {
                    switch (schemaVersion)
                    {
                        case SdmxSchemaEnumType.VersionTwo:
                            RegistryInterface rid = RegistryInterface.Load(reader);

                            if (rid.NotifyRegistryEvent == null)
                            {
                                throw new ArgumentException(
                                    "Can not parse message as NotifyRegistryEvent, as there are no NotifyRegistryEvent in message");
                            }

                            notificationEvent = new NotificationEventCore(rid.NotifyRegistryEvent);
                            break;
                        case SdmxSchemaEnumType.VersionTwoPointOne:
                            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface rid21 =
                                Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface.Load(reader);
                            NotifyRegistryEventType notifyRegistryEventType = rid21.Content.NotifyRegistryEvent;
                            if (notifyRegistryEventType == null)
                            {
                                throw new ArgumentException(
                                    "Can not parse message as NotifyRegistryEvent, as there are no NotifyRegistryEvent in message");
                            }

                            notificationEvent = new NotificationEventCore(notifyRegistryEventType);
                            break;
                        default:
                            throw new SdmxNotImplementedException(
                                ExceptionCode.Unsupported, "Parse NotificationEvent at version: " + schemaVersion);
                    }
                }
            }

            // TODO Validation
            return notificationEvent;
        }

        #endregion
    }
}
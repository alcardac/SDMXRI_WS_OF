// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The subscription parsing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.SubscriptionObjects;
    using Org.Sdmxsource.XmlHelper;

    using SubmitSubscriptionsRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitSubscriptionsRequestType;

    /// <summary>
    ///     The subscription parsing manager implementation.
    /// </summary>
    public class SubscriptionParsingManager : BaseParsingManager, ISubscriptionParsingManager
    {
        #region Fields

        /// <summary>
        ///     The subscription beans builder.
        /// </summary>
        private readonly ISubscriptionObjectsBuilder _subscriptionObjectsBuilder = new SubscriptionObjectsBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        public SubscriptionParsingManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubscriptionParsingManager" /> class.
        /// </summary>
        public SubscriptionParsingManager()
        {
        }

        #endregion

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
        public virtual IList<ISubscriptionObject> ParseSubscriptionXML(IReadableDataLocation dataLocation)
        {
            SdmxSchemaEnumType schemaVersion = this.GetSchemaVersion(dataLocation);

            //// XMLParser.ValidateXml(dataLocation, schemaVersion);
            using (Stream stream = dataLocation.InputStream)
            {
                using (XmlReader reader = XMLParser.CreateSdmxMlReader(stream, schemaVersion))
                {
                    IList<ISubscriptionObject> returnList = new List<ISubscriptionObject>();
                    switch (schemaVersion)
                    {
                        case SdmxSchemaEnumType.VersionTwoPointOne:
                            RegistryInterface rid = MessageFactory.Load<RegistryInterface, RegistryInterfaceType>(
                                reader);
                            SubmitSubscriptionsRequestType submitSubscriptionsRequestType =
                                rid.Content.SubmitSubscriptionsRequest;
                            if (submitSubscriptionsRequestType != null
                                && submitSubscriptionsRequestType.SubscriptionRequest != null)
                            {
                                SubmitSubscriptionsRequestType subscritpionRequestType = submitSubscriptionsRequestType;
                                returnList = this._subscriptionObjectsBuilder.Build(subscritpionRequestType);
                            }

                            break;

                        default:
                            throw new SdmxNotImplementedException(
                                ExceptionCode.Unsupported, "Subscription in version : " + schemaVersion.ToString());
                    }

                    return returnList;
                }
            }
        }

        #endregion
    }
}
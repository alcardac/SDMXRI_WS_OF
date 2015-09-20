// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit subscription builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.RegistryRequest
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using SubmitSubscriptionsRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitSubscriptionsRequestType;
    using SubscriptionType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubscriptionType;

    /// <summary>
    ///     The submit subscription builder.
    /// </summary>
    public class SubmitSubscriptionBuilder
    {
        #region Fields

        /// <summary>
        ///     The subscription xml bean builder.
        /// </summary>
        private readonly SubscriptionXmlBuilder _subscriptionXmlBuilder = new SubscriptionXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build registry interface document.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildRegistryInterfaceDocument(
            ICollection<ISubscriptionObject> buildFrom, DatasetActionEnumType action)
        {
            var rid = new RegistryInterface();
            RegistryInterfaceType rit = rid.Content;
            V21Helper.Header = rit;
            var submitSubscriptionsRequest = new SubmitSubscriptionsRequestType();
            rit.SubmitSubscriptionsRequest = submitSubscriptionsRequest;

            /* foreach */
            foreach (ISubscriptionObject currentSubscription in buildFrom)
            {
                SubscriptionType subscriptionType = this._subscriptionXmlBuilder.Build(currentSubscription);
                var subscriptionRequest = new SubscriptionRequestType();
                submitSubscriptionsRequest.SubscriptionRequest.Add(subscriptionRequest);
                subscriptionRequest.Subscription = subscriptionType;
                switch (action)
                {
                    case DatasetActionEnumType.Append:
                        subscriptionRequest.action = ActionTypeConstants.Append;
                        break;
                    case DatasetActionEnumType.Replace:
                        subscriptionRequest.action = ActionTypeConstants.Replace;
                        break;
                    case DatasetActionEnumType.Delete:
                        subscriptionRequest.action = ActionTypeConstants.Delete;
                        break;
                    case DatasetActionEnumType.Information:
                        subscriptionRequest.action = ActionTypeConstants.Information;
                        break;
                }
            }

            return rid;
        }

        #endregion
    }
}
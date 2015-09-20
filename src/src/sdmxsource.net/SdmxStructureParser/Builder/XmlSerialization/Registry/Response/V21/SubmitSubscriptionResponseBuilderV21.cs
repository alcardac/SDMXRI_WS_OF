// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit subscription response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    using SubmitSubscriptionsResponseType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitSubscriptionsResponseType;

    /// <summary>
    ///     The submit subscription response builder v 21.
    /// </summary>
    public class SubmitSubscriptionResponseBuilderV21 : AbstractResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the error response.
        /// </summary>
        /// <param name="notification">
        /// The notification.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(ISubscriptionObject notification, Exception exception)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            V21Helper.Header = regInterface;
            var returnType = new SubmitSubscriptionsResponseType();
            regInterface.SubmitSubscriptionsResponse = returnType;
            var subscriptionStatus = new SubscriptionStatusType();
            returnType.SubscriptionStatus.Add(subscriptionStatus);
            if (notification != null)
            {
                subscriptionStatus.SubscriptionURN = notification.Urn;
            }

            var statusMessageType = new StatusMessageType();
            subscriptionStatus.StatusMessage = statusMessageType;
            this.AddStatus(statusMessageType, exception);
            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="notifications">
        /// The notifications.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ICollection<ISubscriptionObject> notifications)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            V21Helper.Header = regInterface;

            /* foreach */
            foreach (ISubscriptionObject currentNotification in notifications)
            {
                var returnType = new SubmitSubscriptionsResponseType();
                regInterface.SubmitSubscriptionsResponse = returnType;
                var subscriptionStatus = new SubscriptionStatusType();
                returnType.SubscriptionStatus.Add(subscriptionStatus);
                var statusMessageType = new StatusMessageType();
                subscriptionStatus.StatusMessage = statusMessageType;

                this.AddStatus(statusMessageType, null);

                subscriptionStatus.SubscriptionURN = currentNotification.Urn;
            }

            return responseType;
        }

        #endregion
    }
}
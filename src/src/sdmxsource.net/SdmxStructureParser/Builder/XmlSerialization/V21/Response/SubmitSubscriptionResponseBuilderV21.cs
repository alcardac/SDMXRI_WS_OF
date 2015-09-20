// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Parses and SDMX Document to build the submit subscription response, can either be a RegistryInterfaceDocument or a
//   SubmitSubscriptionResponse Document
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.StatusMessageType;

    /// <summary>
    ///     Parses and SDMX Document to build the submit subscription response, can either be a RegistryInterfaceDocument or a
    ///     SubmitSubscriptionResponse Document
    /// </summary>
    public class SubmitSubscriptionResponseBuilderV21 : IBuilder<IList<ISubmitSubscriptionResponse>, RegistryInterface>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds and returns a list of <see cref="ISubmitSubscriptionResponse"/> from the specified
        ///     <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Interface.
        /// </param>
        /// <returns>
        /// returns a list of <see cref="ISubmitSubscriptionResponse"/> from the specified <paramref name="registryInterface"/>
        /// </returns>
        public virtual IList<ISubmitSubscriptionResponse> Build(RegistryInterface registryInterface)
        {
            IList<ISubmitSubscriptionResponse> returnList = new List<ISubmitSubscriptionResponse>();

            foreach (SubscriptionStatusType subscriptionStatusType in
                registryInterface.Content.SubmitSubscriptionsResponse.SubscriptionStatus)
            {
                if (subscriptionStatusType.StatusMessage != null && subscriptionStatusType.StatusMessage.status != null)
                {
                    IList<string> messages = new List<string>();
                    if (subscriptionStatusType.StatusMessage.MessageText != null)
                    {
                        // TODO Message Codes and Multilingual
                        foreach (StatusMessageType statusMessageType in subscriptionStatusType.StatusMessage.MessageText)
                        {
                            if (statusMessageType.Text != null)
                            {
                                foreach (TextType textType in statusMessageType.Text)
                                {
                                    messages.Add(textType.TypedValue);
                                }
                            }
                        }
                    }

                    IErrorList errors = null;
                    switch (subscriptionStatusType.StatusMessage.status)
                    {
                        case StatusTypeConstants.Failure:
                            errors = new ErrorListCore(messages, false);
                            break;
                        case StatusTypeConstants.Warning:
                            errors = new ErrorListCore(messages, true);
                            break;
                    }

                    Uri urn = subscriptionStatusType.SubscriptionURN;
                    IStructureReference structureReference = null;
                    if (urn != null)
                    {
                        structureReference = new StructureReferenceImpl(urn);
                    }

                    returnList.Add(
                        new SubmitSubscriptionResponseImpl(
                            structureReference, errors, subscriptionStatusType.SubscriberAssignedID));
                }
            }

            return returnList;
        }

        #endregion
    }
}
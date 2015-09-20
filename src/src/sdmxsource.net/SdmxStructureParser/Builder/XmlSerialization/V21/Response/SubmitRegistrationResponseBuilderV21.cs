// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Response
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse;

    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.StatusMessageType;

    /// <summary>
    ///     The submit registration response builder v 21.
    /// </summary>
    public class SubmitRegistrationResponseBuilderV21 : IBuilder<IList<ISubmitRegistrationResponse>, RegistryInterface>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds <see cref="ISubmitRegistrationResponse"/> list from <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registryInterface.
        /// </param>
        /// <returns>
        /// The list of <see cref="ISubmitRegistrationResponse"/>
        /// </returns>
        public virtual IList<ISubmitRegistrationResponse> Build(RegistryInterface registryInterface)
        {
            // TODO REFACTOR - THIS IS VERY SIMILAR TO SUBMIT SUBSCRIPTION RESPONSE
            IList<ISubmitRegistrationResponse> returnList = new List<ISubmitRegistrationResponse>();

            foreach (
                RegistrationStatusType registrationStatusType in
                    registryInterface.Content.SubmitRegistrationsResponse.RegistrationStatus)
            {
                if (registrationStatusType.StatusMessage != null && registrationStatusType.StatusMessage.status != null)
                {
                    IList<string> messages = new List<string>();
                    if (registrationStatusType.StatusMessage.MessageText != null)
                    {
                        // TODO Message Codes and Multilingual
                        foreach (StatusMessageType statusMessageType in registrationStatusType.StatusMessage.MessageText)
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
                    switch (registrationStatusType.StatusMessage.status)
                    {
                        case StatusTypeConstants.Failure:
                            errors = new ErrorListCore(messages, false);
                            break;
                        case StatusTypeConstants.Warning:
                            errors = new ErrorListCore(messages, true);
                            break;
                    }

                    IRegistrationObject registration = new RegistrationObjectCore(registrationStatusType.Registration);
                    returnList.Add(new SubmitRegistrationResponseImpl(registration, errors));
                }
            }

            return returnList;
        }

        #endregion
    }
}
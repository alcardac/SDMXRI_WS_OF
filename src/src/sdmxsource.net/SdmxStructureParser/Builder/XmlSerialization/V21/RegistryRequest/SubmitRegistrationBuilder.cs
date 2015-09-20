// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration builder.
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

    using SubmitRegistrationsRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitRegistrationsRequestType;

    /// <summary>
    ///     The submit registration builder.
    /// </summary>
    public class SubmitRegistrationBuilder
    {
        #region Fields

        /// <summary>
        ///     The registration xml bean builder.
        /// </summary>
        private readonly RegistrationXmlBuilder _registrationXmlBuilder = new RegistrationXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build the registry interface document.
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
            ICollection<IRegistrationObject> buildFrom, DatasetActionEnumType action)
        {
            var registryInterface = new RegistryInterface();
            RegistryInterfaceType interfaceType = registryInterface.Content;
            V21Helper.Header = interfaceType;
            var submitRegistrationRequest = new SubmitRegistrationsRequestType();
            interfaceType.SubmitRegistrationsRequest = submitRegistrationRequest;

            /* foreach */
            foreach (IRegistrationObject currentRegistration in buildFrom)
            {
                RegistrationType registrationType = this._registrationXmlBuilder.Build(currentRegistration);
                var registrationRequest = new RegistrationRequestType();
                submitRegistrationRequest.RegistrationRequest.Add(registrationRequest);
                registrationRequest.Registration = registrationType;
                switch (action)
                {
                    case DatasetActionEnumType.Append:
                        registrationRequest.action = ActionTypeConstants.Append;
                        break;
                    case DatasetActionEnumType.Replace:
                        registrationRequest.action = ActionTypeConstants.Replace;
                        break;
                    case DatasetActionEnumType.Delete:
                        registrationRequest.action = ActionTypeConstants.Delete;
                        break;
                    case DatasetActionEnumType.Information:
                        registrationRequest.action = ActionTypeConstants.Information;
                        break;
                }
            }

            return registryInterface;
        }

        #endregion
    }
}
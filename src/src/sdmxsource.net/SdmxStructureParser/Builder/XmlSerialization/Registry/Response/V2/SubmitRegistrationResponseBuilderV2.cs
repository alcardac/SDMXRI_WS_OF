// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     The submit registration response builder v 2.
    /// </summary>
    public class SubmitRegistrationResponseBuilderV2 : AbstractResponseBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly SubmitRegistrationResponseBuilderV2 _instance =
            new SubmitRegistrationResponseBuilderV2();

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="SubmitRegistrationResponseBuilderV2" /> class from being created.
        /// </summary>
        private SubmitRegistrationResponseBuilderV2()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static SubmitRegistrationResponseBuilderV2 Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception exception)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            V2Helper.Header = regInterface;
            var returnType = new SubmitRegistrationResponseType();
            regInterface.SubmitRegistrationResponse = returnType;
            var registrationStatusType = new RegistrationStatusType();
            returnType.RegistrationStatus.Add(registrationStatusType);
            registrationStatusType.StatusMessage = new StatusMessageType();

            this.AddStatus(registrationStatusType.StatusMessage, exception);
            return responseType;
        }

        /// <summary>
        /// The build response.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildResponse(IDictionary<IRegistrationObject, Exception> response)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new SubmitRegistrationResponseType();
            regInterface.SubmitRegistrationResponse = returnType;
            V2Helper.Header = regInterface;

            /* foreach */
            foreach (KeyValuePair<IRegistrationObject, Exception> registration in response)
            {
                this.ProcessResponse(returnType, registration.Key, registration.Value);
            }

            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The process response.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private void ProcessResponse(
            SubmitRegistrationResponseType returnType, IRegistrationObject registration, Exception exception)
        {
            var registrationStatusType = new RegistrationStatusType();
            returnType.RegistrationStatus.Add(registrationStatusType);
            registrationStatusType.StatusMessage = new StatusMessageType();
            this.AddStatus(registrationStatusType.StatusMessage, exception);
            if (registration.DataSource != null)
            {
                var datasourceType = new DatasourceType();
                registrationStatusType.Datasource = datasourceType;
                this.AddDatasource(registration.DataSource, datasourceType);
            }

            if (registration.ProvisionAgreementRef != null)
            {
                ICrossReference provRef = registration.ProvisionAgreementRef;
                var provRefType = new ProvisionAgreementRefType();
                registrationStatusType.ProvisionAgreementRef = provRefType;
                if (provRef.TargetUrn != null)
                {
                    provRefType.URN = provRef.TargetUrn;
                }
            }
        }

        #endregion
    }
}
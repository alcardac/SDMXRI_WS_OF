// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    using QueryableDataSourceType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryableDataSourceType;
    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.StatusMessageType;
    using SubmitRegistrationsResponseType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitRegistrationsResponseType;

    /// <summary>
    ///     The submit registration response builder v 21.
    /// </summary>
    public class SubmitRegistrationResponseBuilderV21 : AbstractResponseBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly SubmitRegistrationResponseBuilderV21 _instance =
            new SubmitRegistrationResponseBuilderV21();

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="SubmitRegistrationResponseBuilderV21" /> class from being created.
        /// </summary>
        private SubmitRegistrationResponseBuilderV21()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static SubmitRegistrationResponseBuilderV21 Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build error response.
        /// </summary>
        /// <param name="th">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception th)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new SubmitRegistrationsResponseType();
            regInterface.SubmitRegistrationsResponse = returnType;
            V21Helper.Header = regInterface;
            var registrationStatusType = new RegistrationStatusType();
            returnType.RegistrationStatus.Add(registrationStatusType);
            this.AddStatus(registrationStatusType.StatusMessage = new StatusMessageType(), th);
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
            var returnType = new SubmitRegistrationsResponseType();
            regInterface.SubmitRegistrationsResponse = returnType;
            V21Helper.Header = regInterface;

            foreach (KeyValuePair<IRegistrationObject, Exception> registration in response)
            {
                this.ProcessResponse(returnType, registration.Key, registration.Value);
            }

            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process the response.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="registrationBean">
        /// The registration bean.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private void ProcessResponse(
            SubmitRegistrationsResponseType returnType, IRegistrationObject registrationBean, Exception exception)
        {
            var registrationStatusType = new RegistrationStatusType();
            returnType.RegistrationStatus.Add(registrationStatusType);
            var statusMessageType = new StatusMessageType();
            registrationStatusType.StatusMessage = statusMessageType;

            this.AddStatus(statusMessageType, exception);
            var registrationType = new RegistrationType();
            registrationStatusType.Registration = registrationType;
            registrationType.id = registrationBean.Id;
            if (registrationBean.DataSource != null)
            {
                IDataSource datasourceBean = registrationBean.DataSource;
                var datasourceType = new DataSourceType();
                registrationType.Datasource = datasourceType;

                if (datasourceBean.SimpleDatasource)
                {
                    Uri simpleDatasourceType = datasourceBean.DataUrl;
                    datasourceType.SimpleDataSource.Add(simpleDatasourceType);
                }
                else
                {
                    var queryableDatasource = new QueryableDataSourceType();
                    datasourceType.QueryableDataSource.Add(queryableDatasource);
                    queryableDatasource.isRESTDatasource = datasourceBean.RESTDatasource;
                    queryableDatasource.isWebServiceDatasource = datasourceBean.WebServiceDatasource;
                    queryableDatasource.DataURL = datasourceBean.DataUrl;
                    if (datasourceBean.WsdlUrl != null)
                    {
                        queryableDatasource.WSDLURL = datasourceBean.WsdlUrl;
                    }

                    if (datasourceBean.WadlUrl != null)
                    {
                        queryableDatasource.WADLURL = datasourceBean.WadlUrl;
                    }
                }
            }

            if (registrationBean.ProvisionAgreementRef != null)
            {
                var refType = new ProvisionAgreementReferenceType();
                registrationType.ProvisionAgreement = refType;
                refType.URN.Add(registrationBean.ProvisionAgreementRef.TargetUrn);
            }
        }

        #endregion
    }
}
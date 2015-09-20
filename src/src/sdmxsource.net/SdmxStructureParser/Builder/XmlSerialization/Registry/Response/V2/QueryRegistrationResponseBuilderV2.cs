// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryRegistrationResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query registration response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The query registration response builder v 2.
    /// </summary>
    public class QueryRegistrationResponseBuilderV2 : AbstractResponseBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly QueryRegistrationResponseBuilderV2 _instance = new QueryRegistrationResponseBuilderV2();

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="QueryRegistrationResponseBuilderV2" /> class from being created.
        /// </summary>
        private QueryRegistrationResponseBuilderV2()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static QueryRegistrationResponseBuilderV2 Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds error response.
        /// </summary>
        /// <param name="ex">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception ex)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryRegistrationResponseType();
            regInterface.QueryRegistrationResponse = returnType;
            V2Helper.Header = regInterface;

            var queryResult = new QueryResultType();
            returnType.QueryResult.Add(queryResult);

            queryResult.timeSeriesMatch = false;
            var statusMessage = new StatusMessageType();
            queryResult.StatusMessage = statusMessage;

            this.AddStatus(statusMessage, ex);

            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="registrations">
        /// The registrations.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ICollection<IRegistrationObject> registrations)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryRegistrationResponseType();
            regInterface.QueryRegistrationResponse = returnType;
            V2Helper.Header = regInterface;

            if (!ObjectUtil.ValidCollection(registrations))
            {
                var queryResult = new QueryResultType();
                returnType.QueryResult.Add(queryResult);

                queryResult.timeSeriesMatch = false;
                var statusMessage = new StatusMessageType();
                queryResult.StatusMessage = statusMessage;

                statusMessage.status = StatusTypeConstants.Warning;
                var tt = new TextType();
                statusMessage.MessageText.Add(tt);

                tt.TypedValue = "No Registrations Match The Query Parameters";
            }
            else
            {
                /* foreach */
                foreach (IRegistrationObject currentRegistration in registrations)
                {
                    var queryResult0 = new QueryResultType();
                    returnType.QueryResult.Add(queryResult0);

                    var statusMessage1 = new StatusMessageType();
                    queryResult0.StatusMessage = statusMessage1;
                    this.AddStatus(statusMessage1, null);

                    queryResult0.timeSeriesMatch = false; // FUNC 1 - when is this true?  Also We need MetadataResult

                    var resultType = new ResultType();
                    queryResult0.DataResult = resultType;

                    if (currentRegistration.DataSource != null)
                    {
                        IDataSource datasourceBean = currentRegistration.DataSource;
                        var datasourceType = new DatasourceType();
                        resultType.Datasource = datasourceType;
                        if (datasourceBean.SimpleDatasource)
                        {
                            datasourceType.SimpleDatasource = datasourceBean.DataUrl;
                        }
                        else
                        {
                            var queryableDatasource = new QueryableDatasourceType();
                            datasourceType.QueryableDatasource = queryableDatasource;
                            queryableDatasource.isRESTDatasource = datasourceBean.RESTDatasource;
                            queryableDatasource.isWebServiceDatasource = datasourceBean.WebServiceDatasource;
                            queryableDatasource.DataUrl = datasourceBean.DataUrl;
                            if (datasourceBean.WsdlUrl != null)
                            {
                                queryableDatasource.WSDLUrl = datasourceBean.WsdlUrl;
                            }
                        }
                    }

                    if (currentRegistration.ProvisionAgreementRef != null)
                    {
                        WriteProvisionAgreementRef(currentRegistration.ProvisionAgreementRef, resultType);
                    }
                }
            }

            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write provision agreement ref.
        /// </summary>
        /// <param name="provRefBean">
        /// The Provision Agreement ref bean.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        private static void WriteProvisionAgreementRef(IStructureReference provRefBean, ResultType resultType)
        {
            var provRef = new ProvisionAgreementRefType();
            resultType.ProvisionAgreementRef = provRef;

            if (provRefBean.TargetUrn != null)
            {
                provRef.URN = provRefBean.TargetUrn;
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryRegistrationResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query registration response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21;
    using Org.Sdmxsource.Util;

    using QueryRegistrationResponseType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryRegistrationResponseType;
    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.StatusMessageType;

    /// <summary>
    ///     The query registration response builder v 21.
    /// </summary>
    public class QueryRegistrationResponseBuilderV21 : AbstractResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The registration xml bean builder.
        /// </summary>
        private readonly RegistrationXmlBuilder _registrationXmlBuilder = new RegistrationXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the error response.
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
            var returnType = new QueryRegistrationResponseType();
            regInterface.QueryRegistrationResponse = returnType;
            V21Helper.Header = regInterface;

            var statusMessage = new StatusMessageType();
            returnType.StatusMessage = statusMessage;
            this.AddStatus(statusMessage, exception);

            return responseType;
        }

        /// <summary>
        /// Builds the success response.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <paramref name="registrations"/> collection
        /// </typeparam>
        /// <param name="registrations">
        /// The registrations.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse<T>(ICollection<T> registrations)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryRegistrationResponseType();
            regInterface.QueryRegistrationResponse = returnType;
            V21Helper.Header = regInterface;
            var statusMessage = new StatusMessageType();
            returnType.StatusMessage = statusMessage;
            if (!ObjectUtil.ValidCollection(registrations))
            {
                // FUNC 2.1 - add warning to header?
                statusMessage.status = StatusTypeConstants.Warning;
                var statusMessageType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.StatusMessageType();
                statusMessage.MessageText.Add(statusMessageType);
                var textType = new TextType();
                statusMessageType.Text.Add(textType);
                textType.TypedValue = "No Registrations Match The Query Parameters";
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Success;

                /* foreach */
                foreach (IRegistrationObject currentRegistration in registrations)
                {
                    var queryResult = new QueryResultType();
                    returnType.QueryResult.Add(queryResult);
                    queryResult.timeSeriesMatch = false; // FUNC 1 - when is this true?  Also We need MetadataResult

                    var resultType = new ResultType();
                    queryResult.DataResult = resultType;
                    RegistrationType registrationType = this._registrationXmlBuilder.Build(currentRegistration);
                    resultType.Registration = registrationType;
                }
            }

            return responseType;
        }

        #endregion
    }
}
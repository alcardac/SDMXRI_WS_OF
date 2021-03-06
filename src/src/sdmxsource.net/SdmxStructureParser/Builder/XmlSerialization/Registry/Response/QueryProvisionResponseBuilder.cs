// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryProvisionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for querying provisions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for querying provisions.
    /// </summary>
    public class QueryProvisionResponseBuilder : XmlObjectBuilder, IQueryProvisionResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The error response builder 21.
        /// </summary>
        private readonly ErrorResponseBuilder _errorResponseBuilder21 = new ErrorResponseBuilder();

        /// <summary>
        ///     The v 2 builder.
        /// </summary>
        private readonly QueryProvisionResponseBuilderV2 _queryProvisionResponseBuilderV2;

        /// <summary>
        ///     The query structure response builder.
        /// </summary>
        private readonly IQueryStructureResponseBuilder _queryStructureResponseBuilder =
            new QueryStructureResponseBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryProvisionResponseBuilder" /> class.
        /// </summary>
        public QueryProvisionResponseBuilder()
        {
            this._queryProvisionResponseBuilderV2 = QueryProvisionResponseBuilderV2.Instance;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a QueryProvisioningResponse with a failure status along with an
        ///     error message generated from the <paramref name="exception"/>
        /// </summary>
        /// <param name="exception">
        /// The exception generated by the failure
        /// </param>
        /// <param name="schemaVersion">
        /// The schema Version.
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildErrorResponse(Exception exception, SdmxSchemaEnumType schemaVersion)
        {
            XTypedElement response = null;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    response = this._queryProvisionResponseBuilderV2.BuildErrorResponse(exception);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    response = this._errorResponseBuilder21.BuildErrorResponse(exception);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }

            base.WriteSchemaLocation(response, schemaVersion);
            return response;
        }

        /// <summary>
        /// Builds a QueryProvisioningResponse with a success status, along with
        ///     the provisions returned from the query, if there are no provisions returned
        ///     this will be noted in the response message.
        /// </summary>
        /// <param name="provisions">
        /// The source provisions
        /// </param>
        /// <param name="schemaVersion">
        /// The schema Version.
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildSuccessResponse(
            ICollection<IProvisionAgreementObject> provisions, SdmxSchemaEnumType schemaVersion)
        {
            XTypedElement response = null;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    response = this._queryProvisionResponseBuilderV2.BuildSuccessResponse(provisions);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    ISdmxObjects beans = new SdmxObjectsImpl();
                    beans.AddIdentifiables(provisions);
                    response = this._queryStructureResponseBuilder.BuildSuccessResponse(beans, schemaVersion, false);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }

            base.WriteSchemaLocation(response, schemaVersion);
            return response;
        }

        #endregion
    }
}
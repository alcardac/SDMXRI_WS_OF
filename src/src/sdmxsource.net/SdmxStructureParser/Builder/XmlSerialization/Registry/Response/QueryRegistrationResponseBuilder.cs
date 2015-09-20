// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryRegistrationResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for querying registrations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for querying registrations.
    /// </summary>
    public class QueryRegistrationResponseBuilder : XmlObjectBuilder, IQueryRegistrationResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The error response builder.
        /// </summary>
        private readonly ErrorResponseBuilder _errorResponseBuilder = new ErrorResponseBuilder();

        /// <summary>
        ///     The v 2 builder.
        /// </summary>
        private readonly QueryRegistrationResponseBuilderV2 _queryRegistrationResponseBuilderV2;

        /// <summary>
        ///     The v 21 builder.
        /// </summary>
        private readonly QueryRegistrationResponseBuilderV21 _v21Builder = new QueryRegistrationResponseBuilderV21();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryRegistrationResponseBuilder" /> class.
        /// </summary>
        public QueryRegistrationResponseBuilder()
        {
            this._queryRegistrationResponseBuilderV2 = QueryRegistrationResponseBuilderV2.Instance;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a QueryRegistrationResponse with a failure status along with an
        ///     error message generated from the <paramref name="exception"/>
        /// </summary>
        /// <param name="exception">
        /// The exception
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
                    response = this._queryRegistrationResponseBuilderV2.BuildErrorResponse(exception);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    response = this._v21Builder.BuildErrorResponse(exception);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
            base.WriteSchemaLocation(response, schemaVersion);
            return response;
        }

        /// <summary>
        /// Builds a QueryRegistrationResponse based on the registrations supplied.
        /// </summary>
        /// <param name="registrations">
        /// - the registration to be output in the response XML
        /// </param>
        /// <param name="schemaVersion">
        /// - the schema version to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildSuccessResponse(
            ICollection<IRegistrationObject> registrations, SdmxSchemaEnumType schemaVersion)
        {
            XTypedElement response = null;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    response = this._queryRegistrationResponseBuilderV2.BuildSuccessResponse(registrations);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    if (registrations.Count == 0)
                    {
                        response = this._errorResponseBuilder.BuildErrorResponse(SdmxErrorCodeEnumType.NoResultsFound);
                    }

                    response = this._v21Builder.BuildSuccessResponse(registrations);
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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for registration submissions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using Xml.Schema.Linq;

    using ErrorResponseBuilder = Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error.ErrorResponseBuilder;

    /// <summary>
    ///     Builds error and success responses for registration submissions.
    /// </summary>
    public class SubmitRegistrationResponseBuilder : ISubmitRegistrationResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The error response builder.
        /// </summary>
        private readonly IErrorResponseBuilder _errorResponseBuilder = new ErrorResponseBuilder();

        /// <summary>
        ///     The v 2 builder.
        /// </summary>
        private readonly SubmitRegistrationResponseBuilderV2 _submitRegistrationResponseBuilderV2;

        /// <summary>
        ///     The v 21 builder.
        /// </summary>
        private readonly SubmitRegistrationResponseBuilderV21 _submitRegistrationResponseBuilderV21 =
            SubmitRegistrationResponseBuilderV21.Instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubmitRegistrationResponseBuilder" /> class.
        /// </summary>
        public SubmitRegistrationResponseBuilder()
        {
            this._submitRegistrationResponseBuilderV2 = SubmitRegistrationResponseBuilderV2.Instance;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds a submit registration response,
        ///     then the error will be documented, and a status of failure will be put against it.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="schemaVersion">
        /// The SDMX Schema version
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildErrorResponse(Exception exception, SdmxSchemaEnumType schemaVersion)
        {
            // FUNC ERROR CODE?
            // TODO use a constant for 1000
            return this._errorResponseBuilder.BuildErrorResponse(exception, "1000");
        }

        /// <summary>
        /// Builds a submit registration response, if there is an exception against the Registration
        ///     then the error will be documented, and a status of failure will be put against it.
        /// </summary>
        /// <param name="responses">
        /// The responses
        /// </param>
        /// <param name="schemaVersion">
        /// The SDMX Schema version
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildResponse(
            IDictionary<IRegistrationObject, Exception> responses, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return this._submitRegistrationResponseBuilderV2.BuildResponse(responses);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._submitRegistrationResponseBuilderV21.BuildResponse(responses);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for structure submissions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for structure submissions.
    /// </summary>
    public class SubmitStructureResponseBuilder : ISubmitStructureResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The v 2 builder.
        /// </summary>
        private readonly SubmitStructureResponseBuilderV2 _submitStructureResponseBuilderV2 =
            new SubmitStructureResponseBuilderV2();

        /// <summary>
        ///     The v 21 builder.
        /// </summary>
        private readonly SubmitStructureResponseBuilderV21 _submitStructureResponseBuilderV21 =
            new SubmitStructureResponseBuilderV21();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an error response based on the submitted beans and exception
        /// </summary>
        /// <param name="exception">
        /// - the error
        /// </param>
        /// <param name="errorReference">
        /// The error Reference.
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildErrorResponse(
            Exception exception, IStructureReference errorReference, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return this._submitStructureResponseBuilderV2.BuildErrorResponse(exception, errorReference);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._submitStructureResponseBuilderV21.BuildErrorResponse(exception, errorReference);
                default:
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported, "SubmitStructureResponseBuilder.buildErrorResponse" + schemaVersion);
            }
        }

        /// <summary>
        /// Builds a success response based on the submitted beans
        /// </summary>
        /// <param name="beans">
        /// - the beans that were successfully submitted
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildSuccessResponse(ISdmxObjects beans, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return this._submitStructureResponseBuilderV2.BuildSuccessResponse(beans);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._submitStructureResponseBuilderV21.BuildSuccessResponse(beans);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

        #endregion
    }
}
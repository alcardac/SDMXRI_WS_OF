// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitStructureResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for submitting structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    using Xml.Schema.Linq;

    /// <summary>
    ///     A class supporting this interface can build error and success responses for submitting structures.
    /// </summary>
    public interface ISubmitStructureResponseBuilder
    {
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
        XTypedElement BuildErrorResponse(
            Exception exception, IStructureReference errorReference, SdmxSchemaEnumType schemaVersion);

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
        XTypedElement BuildSuccessResponse(ISdmxObjects beans, SdmxSchemaEnumType schemaVersion);

        #endregion
    }
}
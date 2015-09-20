// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryStructureResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for QueryStructureResponses.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    using Xml.Schema.Linq;

    /// <summary>
    ///     A class supporting this interface can build error and success responses for QueryStructureResponses.
    /// </summary>
    public interface IQueryStructureResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an error response based on the exception
        /// </summary>
        /// <param name="exception">
        /// - the error
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildErrorResponse(Exception exception, SdmxSchemaEnumType schemaVersion);

        /// <summary>
        /// Builds a success response along with the query results
        /// </summary>
        /// <param name="beans">
        /// - the beans that were successfully returned from the query
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <param name="returnAsStructureMessage">
        /// returns a structure message if true, otherwise returns as a query structure response
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildSuccessResponse(
            ISdmxObjects beans, SdmxSchemaEnumType schemaVersion, bool returnAsStructureMessage);

        #endregion
    }
}
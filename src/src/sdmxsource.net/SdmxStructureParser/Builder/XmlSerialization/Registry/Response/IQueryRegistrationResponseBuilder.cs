// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryRegistrationResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for QueryRegistrationResponses.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    using Xml.Schema.Linq;

    /// <summary>
    ///     A class supporting this interface can build error and success responses for QueryRegistrationResponses.
    /// </summary>
    public interface IQueryRegistrationResponseBuilder
    {
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
        XTypedElement BuildErrorResponse(Exception exception, SdmxSchemaEnumType schemaVersion);

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
        XTypedElement BuildSuccessResponse(
            ICollection<IRegistrationObject> registrations, SdmxSchemaEnumType schemaVersion);

        #endregion
    }
}
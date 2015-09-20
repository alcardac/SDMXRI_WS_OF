// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitRegistrationResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for submitting registrations.
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
    ///     A class supporting this interface can build error and success responses for submitting registrations.
    /// </summary>
    public interface ISubmitRegistrationResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a submit registration response,
        ///     then the error will be documented, and a status of failure will be put against it.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="schemaVersion">
        /// The SDMX Schema version
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildErrorResponse(Exception response, SdmxSchemaEnumType schemaVersion);

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
        XTypedElement BuildResponse(
            IDictionary<IRegistrationObject, Exception> responses, SdmxSchemaEnumType schemaVersion);

        #endregion
    }
}
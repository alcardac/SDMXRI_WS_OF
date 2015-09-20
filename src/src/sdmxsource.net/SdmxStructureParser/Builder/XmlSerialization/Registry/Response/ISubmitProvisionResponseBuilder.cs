// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitProvisionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for submission of provisions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    using Xml.Schema.Linq;

    /// <summary>
    ///     A class supporting this interface can build error and success responses for submission of provisions.
    /// </summary>
    public interface ISubmitProvisionResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build error response for submission of provisions..
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="structureReference">
        /// The structure Reference.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <returns>
        /// The error response for submission of provisions
        /// </returns>
        XTypedElement BuildErrorResponse(
            Exception exception, IStructureReference structureReference, SdmxSchemaEnumType schemaVersion);

        /// <summary>
        /// Returns an response based on the submitted provision, if there is a Exception against the provision
        ///     then the error will be documented, and a status of failure will be put against it.
        /// </summary>
        /// <param name="response">
        /// - a map of provision, and a error message (if there is one)
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildSuccessResponse(
            ICollection<IProvisionAgreementObject> response, SdmxSchemaEnumType schemaVersion);

        #endregion
    }
}
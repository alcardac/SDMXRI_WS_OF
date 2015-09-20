// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitSubscriptionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class supporting this interface can build error and success responses for submitting subscriptions.
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
    ///     A class supporting this interface can build error and success responses for submitting subscriptions.
    /// </summary>
    public interface ISubmitSubscriptionResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an error response based on the submitted beans and exception
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <param name="th">
        /// - the error
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildErrorResponse(
            ISubscriptionObject subscription, SdmxSchemaEnumType schemaVersion, Exception th);

        /// <summary>
        /// Builds a success response based on the submitted notifications
        /// </summary>
        /// <param name="notifications">
        /// - the notifications that were successfully submitted
        /// </param>
        /// <param name="schemaVersion">
        /// - the version of the schema to output the response in
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildSuccessResponse(
            ICollection<ISubscriptionObject> notifications, SdmxSchemaEnumType schemaVersion);

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for subscription submissions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for subscription submissions.
    /// </summary>
    public class SubmitSubscriptionResponseBuilder : ISubmitSubscriptionResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The v 21 builder.
        /// </summary>
        private readonly SubmitSubscriptionResponseBuilderV21 _v21Builder = new SubmitSubscriptionResponseBuilderV21();

        #endregion

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
        public virtual XTypedElement BuildErrorResponse(
            ISubscriptionObject subscription, SdmxSchemaEnumType schemaVersion, Exception th)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._v21Builder.BuildErrorResponse(subscription, th);
                default:
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported, "Submit Subscitpion response in version" + schemaVersion);
            }
        }

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
        public virtual XTypedElement BuildSuccessResponse(
            ICollection<ISubscriptionObject> notifications, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._v21Builder.BuildSuccessResponse(notifications);
                default:
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported, "Submit Subscitpion response in version" + schemaVersion);
            }
        }

        #endregion
    }
}
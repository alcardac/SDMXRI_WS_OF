// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitProvisionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for provision submissions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for provision submissions.
    /// </summary>
    public class SubmitProvisionResponseBuilder : ISubmitProvisionResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The submit Provision Response v2 builder.
        /// </summary>
        private readonly SubmitProvisionResponseBuilderV2 _submitProvisionResponseBuilderV2;

        /// <summary>
        ///     The submit Provision Response  v2.1 builder.
        /// </summary>
        private readonly SubmitStructureResponseBuilderV21 _v21Builder = new SubmitStructureResponseBuilderV21();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubmitProvisionResponseBuilder" /> class.
        /// </summary>
        public SubmitProvisionResponseBuilder()
        {
            this._submitProvisionResponseBuilderV2 = SubmitProvisionResponseBuilderV2.Instance;
        }

        #endregion

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
        public virtual XTypedElement BuildErrorResponse(
            Exception exception, IStructureReference structureReference, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return this._submitProvisionResponseBuilderV2.BuildErrorResponse(exception);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._v21Builder.BuildErrorResponse(exception, structureReference);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

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
        public virtual XTypedElement BuildSuccessResponse(
            ICollection<IProvisionAgreementObject> response, SdmxSchemaEnumType schemaVersion)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return this._submitProvisionResponseBuilderV2.BuildResponse(response);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    ISdmxObjects beans = new SdmxObjectsImpl();
                    beans.AddIdentifiables(response);
                    return this._v21Builder.BuildSuccessResponse(beans);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds error and success responses for querying structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21;

    using Xml.Schema.Linq;

    /// <summary>
    ///     Builds error and success responses for querying structures.
    /// </summary>
    public class QueryStructureResponseBuilder : XmlObjectBuilder, IQueryStructureResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The error response builder 21.
        /// </summary>
        private readonly ErrorResponseBuilder _errorResponseBuilder21 = new ErrorResponseBuilder();

        /// <summary>
        ///     The Query structure request v 2 builder.
        /// </summary>
        private readonly QueryStructureResponseBuilderV2 _queryStructureResponseBuilderV2 =
            new QueryStructureResponseBuilderV2();

        /// <summary>
        ///     The structure v 21 builder.
        /// </summary>
        private readonly StructureXmlBuilder _structV21Builder = new StructureXmlBuilder();

        /// <summary>
        ///     The structure 1 builder.
        /// </summary>
        private readonly XmlSerialization.V1.StructureXmlBuilder _structv1Builder =
            new XmlSerialization.V1.StructureXmlBuilder();

        /// <summary>
        ///     The structure v2 builder.
        /// </summary>
        private readonly XmlSerialization.V2.StructureXmlBuilder _structv2Builder =
            new XmlSerialization.V2.StructureXmlBuilder();

        #endregion

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
        public virtual XTypedElement BuildErrorResponse(Exception exception, SdmxSchemaEnumType schemaVersion)
        {
            XTypedElement response = null;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    response = this._errorResponseBuilder21.BuildErrorResponse(exception);
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    response = this._queryStructureResponseBuilderV2.BuildErrorResponse(exception);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
            base.WriteSchemaLocation(response, schemaVersion);
            return response;
        }

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
        public virtual XTypedElement BuildSuccessResponse(
            ISdmxObjects beans, SdmxSchemaEnumType schemaVersion, bool returnAsStructureMessage)
        {
            XTypedElement response = null;
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    if (beans.GetAllMaintainables().Count == 0)
                    {
                        response = this._errorResponseBuilder21.BuildErrorResponse(SdmxErrorCodeEnumType.NoResultsFound);
                    }

                    response = this._structV21Builder.Build(beans);
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    if (returnAsStructureMessage)
                    {
                        response = this._structv2Builder.Build(beans);
                    }

                    response = this._queryStructureResponseBuilderV2.BuildSuccessResponse(beans);
                    break;
                case SdmxSchemaEnumType.VersionOne:
                    response = this._structv1Builder.Build(beans);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }
            base.WriteSchemaLocation(response, schemaVersion);
            return response;
        }

        #endregion
    }
}
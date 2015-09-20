// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The error response builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;
    using System.Globalization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType;

    /// <summary>
    ///     The error response builder.
    /// </summary>
    public class ErrorResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="Error"/>.
        /// </returns>
        public Error BuildErrorResponse(Exception exception)
        {
            var errorDoc = new Error();
            ErrorType error = errorDoc.Content;

            // FUNC Standard error codes
            var errorMessage = new CodedStatusMessageType();
            error.ErrorMessage.Add(errorMessage);
            errorMessage.code = "1000";
            var text = new TextType();
            errorMessage.Text.Add(text);
            var uncheckedException = exception as SdmxException;
            text.TypedValue = uncheckedException != null ? uncheckedException.FullMessage : exception.Message;

            return errorDoc;
        }

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <returns>
        /// The <see cref="Error"/>.
        /// </returns>
        public Error BuildErrorResponse(SdmxErrorCodeEnumType errorCode)
        {
            return this.BuildErrorResponse(SdmxErrorCode.GetFromEnum(errorCode));
        }

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <returns>
        /// The <see cref="Error"/>.
        /// </returns>
        public Error BuildErrorResponse(SdmxErrorCode errorCode)
        {
            var errorDoc = new Error();
            ErrorType error = errorDoc.Content;

            // FUNC Standard error codes
            var errorMessage = new CodedStatusMessageType();
            error.ErrorMessage.Add(errorMessage);
            errorMessage.code = errorCode.ClientErrorCode.ToString(CultureInfo.InvariantCulture);
            var text = new TextType();
            errorMessage.Text.Add(text);
            text.TypedValue = errorCode.ErrorString;
            return errorDoc;
        }

        #endregion
    }
}
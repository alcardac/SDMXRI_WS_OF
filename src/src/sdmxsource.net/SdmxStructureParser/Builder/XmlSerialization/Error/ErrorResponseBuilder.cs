// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The error response builder implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error;


    using Xml.Schema.Linq;
    using Org.Sdmxsource.Sdmx.Util.Exception;

    /// <summary>
    ///     The error response builder implementation
    /// </summary>
    public class ErrorResponseBuilder : XmlObjectBuilder, IErrorResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="exceptionCode">
        /// The exception code.
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        public virtual XTypedElement BuildErrorResponse(Exception exception, string exceptionCode)
        {
            var errorDocument = new Error();

            var errorMessage = new CodedStatusMessageType();
            errorDocument.ErrorMessage.Add(errorMessage);

            errorMessage.code = exceptionCode;
            while (exception != null)
            {
                var text = new TextType();
                errorMessage.Text.Add(text);
                if (string.IsNullOrEmpty(exception.Message))
                {
                    if (exception.InnerException != null)
                    {
                        text.TypedValue = exception.InnerException.Message;
                    }
                }
                else
                {
                    text.TypedValue = exception.Message;
                }

                if (exception.GetType() == typeof(SchemaValidationException))
                {
                    ProcessSchemaValidationError(errorMessage, (SchemaValidationException)exception);
                }
                else
                {
                    ProcessThrowable(errorMessage, exception);
                }

                exception = exception.InnerException;
            }

            return errorDocument;
        }

        private void ProcessSchemaValidationError(CodedStatusMessageType errorMessage, SchemaValidationException e)
        {
            foreach (string error in e.GetValidationErrors())
            {
                TextType text = new TextType();
                errorMessage.Text.Add(text);
                text.TypedValue = error;
            }
        }

        private void ProcessThrowable(CodedStatusMessageType errorMessage, Exception th)
        {
            TextType text = new TextType();
            errorMessage.Text.Add(text);

            if (th.Message == null)
            {
                if (th.GetBaseException() != null)
                {
                    text.TypedValue = th.GetBaseException().Message;
                }
                else
                {
                    if (th.GetType() == typeof(NullReferenceException))
                    {
                        text.TypedValue = "Null Reference Exception";
                    }
                    else
                    {
                        text.TypedValue = "No Error Message Provided";
                    }
                }
            }
            else
            {
                text.TypedValue = th.Message;
            }
        }

        #endregion
    }
}
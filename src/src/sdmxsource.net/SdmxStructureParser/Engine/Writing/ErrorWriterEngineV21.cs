using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Engine;
using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error;
using System.IO;
using Xml.Schema.Linq;
using Org.Sdmxsource.Sdmx.Api.Exception;

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System.Globalization;

    public class ErrorWriterEngineV21 : IErrorWriterEngine
    {
        /// <summary>
        /// The error response builder.
        /// </summary>
        private readonly ErrorResponseBuilder _errorResponseBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWriterEngineV21"/> class.
        /// </summary>
        public ErrorWriterEngineV21()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWriterEngineV21"/> class.
        /// </summary>
        /// <param name="errorResponseBuilder">The error response builder.</param>
        public ErrorWriterEngineV21(ErrorResponseBuilder errorResponseBuilder)
        {
            this._errorResponseBuilder = errorResponseBuilder ?? new ErrorResponseBuilder();
        }

        /// <summary>
        /// Writes an error to the output stream
        /// </summary>
        /// <param name="ex">
        /// The error to write
        /// </param>
        /// <param name="outPutStream">
        /// The output stream to write to
        /// </param>
        /// <returns>
        /// The HTTP Status code
        /// </returns>
        public int WriteError(Exception ex, Stream outPutStream)
        {
            XTypedElement error;
            int statusCode;
            if (ex.GetType() == typeof(SdmxException))
            {
                SdmxException sdmxEx = (SdmxException)ex;
                statusCode = sdmxEx.HttpRestErrorCode;
                error = this._errorResponseBuilder.BuildErrorResponse(ex, sdmxEx.SdmxErrorCode.ClientErrorCode.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                error = this._errorResponseBuilder.BuildErrorResponse(ex, "500");
                statusCode = 500;
            }

            error.Untyped.Save(outPutStream);

            return statusCode;
        }
    }
}
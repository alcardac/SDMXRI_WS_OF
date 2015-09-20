// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxErrorExtension.cs" company="Eurostat">
//   Date Created : 2013-10-24
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sdmx error extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Extension
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    /// The sdmx error extension.
    /// </summary>
    public static class SdmxErrorExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// The is client error.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsClientError(this SdmxErrorCodeEnumType errorCode)
        {
            switch (errorCode)
            {
                case SdmxErrorCodeEnumType.NoResultsFound:
                case SdmxErrorCodeEnumType.Unauthorised:
                case SdmxErrorCodeEnumType.ResponseTooLarge:
                case SdmxErrorCodeEnumType.SyntaxError:
                case SdmxErrorCodeEnumType.SemanticError:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The is client error.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsClientError(this Exception exception)
        {
            var sdmxException = exception as SdmxException;
            if (sdmxException != null)
            {
                return sdmxException.SdmxErrorCode.EnumType.IsClientError();
            }

            var xmlSchemaException = exception as XmlSchemaValidationException;
            if (xmlSchemaException != null)
            {
                return true;
            }

            var xmlException = exception as XmlException;
            if (xmlException != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The to sdmx exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxException"/>.
        /// </returns>
        public static SdmxException ToSdmxException(this Exception exception)
        {
            var sdmxException = exception as SdmxException;
            if (sdmxException != null)
            {
                return sdmxException;
            }

            var xmlSchemaException = exception as XmlSchemaValidationException;
            if (xmlSchemaException != null)
            {
                return new SdmxSyntaxException(xmlSchemaException, ExceptionCode.FailValidation);
            }

            var xmlException = exception as XmlException;
            if (xmlException != null)
            {
                return new SdmxSyntaxException(xmlException, ExceptionCode.XmlParseException);
            }

            var notImplemented = exception as NotImplementedException;
            if (notImplemented != null)
            {
                return new SdmxNotImplementedException(notImplemented);
            }

            return new SdmxInternalServerException(exception.Message);
        }

        #endregion
    }
}
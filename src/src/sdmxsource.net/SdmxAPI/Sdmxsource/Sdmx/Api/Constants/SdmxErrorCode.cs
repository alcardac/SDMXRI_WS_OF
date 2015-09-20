// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxErrorCode.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Contains an enumerated list of the SDMX standard error codes, along with an english representation of the error
    /// </summary>
    public enum SdmxErrorCodeEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     The no results found.
        /// </summary>
        NoResultsFound,

        /// <summary>
        ///     The unauthorised.
        /// </summary>
        Unauthorised,

        /// <summary>
        ///     The response too large.
        /// </summary>
        ResponseTooLarge,

        /// <summary>
        ///     The syntax error.
        /// </summary>
        SyntaxError,

        /// <summary>
        ///     The semantic error.
        /// </summary>
        SemanticError,

        /// <summary>
        ///     The internal server error.
        /// </summary>
        InternalServerError,

        /// <summary>
        ///     The not implemented.
        /// </summary>
        NotImplemented,

        /// <summary>
        ///     The service unavailable.
        /// </summary>
        ServiceUnavailable,

        /// <summary>
        ///     The response size exceeds service limit.
        /// </summary>
        ResponseSizeExceedsServiceLimit
    }

    /// <summary>
    ///     The sdmx error code.
    /// </summary>
    public class SdmxErrorCode : BaseConstantType<SdmxErrorCodeEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<SdmxErrorCodeEnumType, SdmxErrorCode> Instances =
            new Dictionary<SdmxErrorCodeEnumType, SdmxErrorCode>
                {
                    {
                        SdmxErrorCodeEnumType.NoResultsFound, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.NoResultsFound, 
                        100, 
                        "No Results Found", 
                        404)
                    }, 
                    {
                        SdmxErrorCodeEnumType.Unauthorised, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.Unauthorised, 
                        110, 
                        "Unauthorized", 
                        401)
                    }, 
                    {
                        SdmxErrorCodeEnumType.ResponseTooLarge, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.ResponseTooLarge, 
                        130, 
                        "ResponseToo Large", 
                        413)
                    }, 
                    {
                        SdmxErrorCodeEnumType.SyntaxError, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.SyntaxError, 
                        140, 
                        "Syntax Error", 
                        400)
                    }, 
                    {
                        SdmxErrorCodeEnumType.SemanticError, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.SemanticError, 
                        150, 
                        "Semantic Error", 
                        400)
                    }, 
                    {
                        SdmxErrorCodeEnumType.InternalServerError, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.InternalServerError, 
                        500, 
                        "Internal Server Error", 
                        500)
                    }, 
                    {
                        SdmxErrorCodeEnumType.NotImplemented, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.NotImplemented, 
                        501, 
                        "Not Implemented", 
                        501)
                    }, 
                    {
                        SdmxErrorCodeEnumType.ServiceUnavailable, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType.ServiceUnavailable, 
                        503, 
                        "Service Unavailable", 
                        503)
                    }, 
                    {
                        SdmxErrorCodeEnumType
                        .ResponseSizeExceedsServiceLimit, 
                        new SdmxErrorCode(
                        SdmxErrorCodeEnumType
                        .ResponseSizeExceedsServiceLimit, 
                        510, 
                        "Response size exceeds service limit", 
                        413)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _client error code.
        /// </summary>
        private readonly int _clientErrorCode;

        /// <summary>
        ///     The _error string.
        /// </summary>
        private readonly string _errorString;

        /// <summary>
        ///     The _httpRestError code.
        /// </summary>
        private readonly int _httpRestErrorCode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxErrorCode"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="clientErrorCode">
        /// The client error code.
        /// </param>
        /// <param name="errorString">
        /// The error string.
        /// </param>
        /// <param name="httpRestErrorCode">
        /// The httpRestError code.
        /// </param>
        private SdmxErrorCode(SdmxErrorCodeEnumType enumType, int clientErrorCode, string errorString, int httpRestErrorCode)
            : base(enumType)
        {
            this._clientErrorCode = clientErrorCode;
            this._errorString = errorString;
            this._httpRestErrorCode = httpRestErrorCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<SdmxErrorCode> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the client error code.
        /// </summary>
        public int ClientErrorCode
        {
            get
            {
                return this._clientErrorCode;
            }
        }

        /// <summary>
        ///     Gets the error string.
        /// </summary>
        public string ErrorString
        {
            get
            {
                return this._errorString;
            }
        }

        /// <summary>
        /// Gets the httpRestError code.
        /// </summary>
        public int HttpRestErrorCode
        {
            get
            {
                return this._httpRestErrorCode;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="SdmxErrorCode"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="SdmxErrorCode"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static SdmxErrorCode GetFromEnum(SdmxErrorCodeEnumType enumType)
        {
            SdmxErrorCode output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Gets the instance of <see cref="SdmxErrorCode"/> mapped to <paramref name="code"/>
        /// </summary>
        /// <param name="code">
        /// The <c>ClientErrorCode</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="SdmxErrorCode"/> mapped to <paramref name="code"/>
        /// </returns>
        public static SdmxErrorCode ParseClientCode(int code) 
        {
            foreach (SdmxErrorCode currentCode in Values)
            {
			    if(currentCode.ClientErrorCode == code) {
				    return currentCode;
			    }
		    }
		    return null;
	    }

        #endregion
        	
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebFaultExceptionRestBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The web fault exception rest builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;

    using Estat.Sri.Ws.Controllers.Extension;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The web fault exception rest builder.
    /// </summary>
    public class WebFaultExceptionRestBuilder : IBuilder<WebFaultException<string>, SdmxException>, IBuilder<WebFaultException<string>, Exception>
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(WebFaultExceptionRestBuilder));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type  <see cref="WebFaultException{String}"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="WebFaultException{String}"/>
        /// </returns>
        public WebFaultException<string> Build(SdmxException buildFrom)
        {
            _log.ErrorFormat("SdmxError : {0}, code : {1}", buildFrom.SdmxErrorCode.ErrorString, buildFrom.SdmxErrorCode.ClientErrorCode);
            _log.Error(buildFrom.FullMessage, buildFrom);
            return new WebFaultException<string>(buildFrom.Message, (HttpStatusCode)buildFrom.HttpRestErrorCode);
        }

        /// <summary>
        /// Builds an object of type  <see cref="WebFaultException{String}"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="WebFaultException{String}"/>
        /// </returns>
        public WebFaultException<string> Build(Exception buildFrom)
        {
            var webExceptionGeneric = buildFrom as WebFaultException<string>;
            if (webExceptionGeneric != null)
            {
                return webExceptionGeneric;
            }

            var webException = buildFrom as WebFaultException;
            if (webException != null)
            {
                return new WebFaultException<string>(webException.Message, webException.StatusCode);
            }

            var sdmxException = buildFrom.ToSdmxException();
            if (sdmxException != null)
            {
                return this.Build(sdmxException);
            }

            return new WebFaultException<string>("Error processing request", HttpStatusCode.InternalServerError);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderUtils.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The header utils.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest.Utils
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.ServiceModel.Web;

    using log4net;

    /// <summary>
    /// The header utils.
    /// </summary>
    public static class HeaderUtils
    {
        #region Static Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HeaderUtils));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get query string as dict.
        /// </summary>
        /// <param name="queryParameters">
        /// The query parameters.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        /// <exception cref="WebFaultException">
        /// </exception>
        public static IDictionary<string, string> GetQueryStringAsDict(NameValueCollection queryParameters)
        {
            IDictionary<string, string> paramsDict = new Dictionary<string, string>();
            var enumQ = queryParameters.GetEnumerator();
            while (enumQ.MoveNext())
            {
                var queryName = enumQ.Current.ToString();
                var queryValue = queryParameters[queryName];
                if (paramsDict.ContainsKey(queryName))
                {
                    Logger.Error("Duplicate parameters values is semantically error");
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                paramsDict.Add(queryName, queryValue);
            }

            return paramsDict;
        }

        /// <summary>
        /// The get version from accept.
        /// </summary>
        /// <param name="acceptHeaderList">
        /// The accept header list.
        /// </param>
        /// <param name="mediatype">
        /// The mediatype.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetVersionFromAccept(IEnumerable<ContentType> acceptHeaderList, string mediatype)
        {
            string version = "2.1";

            var accept = acceptHeaderList.FirstOrDefault(h => h.MediaType.Contains(mediatype));
            if (accept != null)
            {
                if (accept.Parameters != null && accept.Parameters.ContainsKey("version"))
                {
                    version = accept.Parameters["version"];
                }
            }

            return version;
        }

        #endregion
    }
}
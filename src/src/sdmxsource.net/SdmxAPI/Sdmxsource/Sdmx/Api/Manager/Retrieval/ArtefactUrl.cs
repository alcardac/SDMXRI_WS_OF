// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactUrl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System;

    #endregion

    /// <summary>
    ///     The artefact Uri contains the url to resolve the artefact, and a boolean defining if it is a service Uri or a structure Uri
    /// </summary>
    public class ArtefactUrl
    {
        #region Fields

        /// <summary>
        ///     The _service url.
        /// </summary>
        private readonly bool _serviceUrl;

        /// <summary>
        ///     The _url.
        /// </summary>
        private readonly Uri _url;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactUrl"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="serviceUrl">
        /// The service url.
        /// </param>
        public ArtefactUrl(Uri url, bool serviceUrl)
        {
            this._url = url;
            this._serviceUrl = serviceUrl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether service url.
        /// </summary>
        public bool ServiceUrl
        {
            get
            {
                return this._serviceUrl;
            }
        }

        /// <summary>
        ///     Gets the url.
        /// </summary>
        public Uri Url
        {
            get
            {
                return this._url;
            }
        }

        #endregion
    }
}
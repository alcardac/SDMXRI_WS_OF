// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebServiceType.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines web service types, REST & SOAP
    /// </summary>
    public enum WebServiceType
    {
        /// <summary>
        /// Get
        /// </summary>
        RestGet,
        /// <summary>
        /// Post
        /// </summary>
        RestPost,
        /// <summary>
        /// Soap
        /// </summary>
        Soap
    }
}

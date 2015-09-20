// -----------------------------------------------------------------------
// <copyright file="UriExtensionMethods.cs" company="Eurostat">
//   Date Created : 2014-05-21
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Util.Url
{
    using System;

    /// <summary>
    /// The URI extension methods.
    /// </summary>
    public static class UriExtensionMethods
    {
        /// <summary>
        /// Convert to URI.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public static Uri ToUri(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? new Uri(value) : null;
        }
    }
}
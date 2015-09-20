// -----------------------------------------------------------------------
// <copyright file="SdmxContentMapper.cs" company="Eurostat">
//   Date Created : 2014-05-30
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System.ServiceModel.Channels;

    using Estat.Sri.Ws.Rest.Utils;

    public class SdmxContentMapper  : WebContentTypeMapper
    {
        /// <summary>
        /// When overridden in a derived class, returns the message format used for a specified content type.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.WebContentFormat"/> that specifies the format to which the message content type is mapped. 
        /// </returns>
        /// <param name="contentType">The content type that indicates the MIME type of data to be interpreted.</param>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            if (contentType.Contains("application/vnd.sdmx") || contentType.Contains("text/*") || contentType.Contains("application/*"))
            {
                return WebContentFormat.Raw;
            }
            if (contentType.Contains(SdmxMedia.ApplicationXml) || contentType.Contains(SdmxMedia.TextXml))
            {
                return WebContentFormat.Xml;
            }

            return WebContentFormat.Default;
        }
    }
}
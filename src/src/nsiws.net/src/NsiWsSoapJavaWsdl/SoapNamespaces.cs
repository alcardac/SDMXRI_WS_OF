// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoapNamespaces.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The soap namespaces.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;

    /// <summary>
    ///     The soap namespaces.
    /// </summary>
    public static class SoapNamespaces
    {
        #region Constants

        /// <summary>
        ///     The SDMX v2.0 with Eurostat extensions.
        /// </summary>
        public const string SdmxV20Estat = "http://ec.europa.eu/eurostat/sri/service/2.0/extended";

        /// <summary>
        ///     The SDMX v2.0 Java. (note the '/' on the end and those are NAMESPACES not URL.)
        /// </summary>
        public const string SdmxV20JavaStd = "http://ec.europa.eu/eurostat/sri/service/2.0/";

        /// <summary>
        ///     The SDMX v2.0 .NET. (note the missing '/' on the end and those are NAMESPACES not URL.)
        /// </summary>
        public const string SdmxV20NetStd = "http://ec.europa.eu/eurostat/sri/service/2.0";

        /// <summary>
        ///     The SDMX v2.1.
        /// </summary>
        public const string SdmxV21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/webservices";

        #endregion

        /// <summary>
        /// Gets the SOAP operation.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="SoapOperation"/>.
        /// </returns>
        /// <remarks>This method will the Message body contents.</remarks>
        public static SoapOperation GetSoapOperation(this Message message)
        {
            using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
            {
                do
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.NamespaceURI.Equals(SdmxV21) || reader.NamespaceURI.Equals(SdmxV20Estat) || reader.NamespaceURI.Equals(SdmxV20JavaStd))
                        {
                            SoapOperation soapOperation;
                            if (Enum.TryParse(reader.LocalName, out soapOperation))
                            {
                                return soapOperation;
                            }
                        }
                    }
                }
                while (reader.Read());
            }

            return SoapOperation.Null;
        }
    }
}
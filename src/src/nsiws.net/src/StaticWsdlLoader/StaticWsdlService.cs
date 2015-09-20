// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticWsdlService.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The static WSDL service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System;
    using System.IO;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Xml;

    using log4net;

    /// <summary>
    ///     The static WSDL service.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StaticWsdlService : IStaticWsdlService
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StaticWsdlService));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the WSDL.
        /// </summary>
        /// <param name="name">
        /// The service name.
        /// </param>
        /// <returns>
        /// The stream to the WSDL.
        /// </returns>
        public Stream GetWsdl(string name)
        {
            string prefix = null;
            string localName = null;
            string namespaceUri = null;
            try
            {
                var wsdlRegistry = WsdlRegistry.Instance;

                var wsdlInfo = wsdlRegistry.GetWsdlInfo(name);
                if (wsdlInfo == null)
                {
                    throw new WebFaultException(HttpStatusCode.InternalServerError);
                }

                var uriString = VirtualPathUtility.ToAbsolute("~/");
                var baseAddress = new Uri(HttpContext.Current.Request.Url, uriString);
                var stream = new MemoryStream();
                var originalPath = HostingEnvironment.MapPath(string.Format("~/{0}", wsdlInfo.OriginalPath));

                using (XmlReader reader = XmlReader.Create(originalPath, new XmlReaderSettings { IgnoreWhitespace = true, CloseInput = true, IgnoreComments = true }))
                using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                bool isEmptyElement = reader.IsEmptyElement;
                                prefix = reader.Prefix;
                                localName = reader.LocalName;
                                namespaceUri = reader.NamespaceURI;
                                writer.WriteStartElement(prefix, localName, namespaceUri);
                                if (!AddBaseAddress(reader, writer, "address", "http://schemas.xmlsoap.org/wsdl/soap/", "location", baseAddress)
                                    && !AddBaseAddress(reader, writer, "import", "http://www.w3.org/2001/XMLSchema", "schemaLocation", baseAddress))
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        writer.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
                                    }
                                }

                                if (isEmptyElement)
                                {
                                    writer.WriteEndElement();
                                }

                                break;
                            case XmlNodeType.None:
                                break;
                            case XmlNodeType.EndElement:
                                writer.WriteEndElement();
                                break;
                            case XmlNodeType.Text:
                                writer.WriteValue(reader.Value);
                                break;
                        }
                    }
                }

                stream.Position = 0;
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml; charset=utf-8";
                }

                return stream;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Element: {0}, Prefix : {1} , NS {2}", localName, prefix, namespaceUri);
                _log.Error("Error while converting the WSDL", e);
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the base address.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="targetNs">
        /// The target constant.
        /// </param>
        /// <param name="targetAttribute">
        /// The target attribute.
        /// </param>
        /// <param name="baseAddress">
        /// The base address.
        /// </param>
        /// <returns>
        /// <c>True if there was a match; otherwise false</c>
        /// </returns>
        private static bool AddBaseAddress(XmlReader reader, XmlWriter writer, string element, string targetNs, string targetAttribute, Uri baseAddress)
        {
            string localName = reader.LocalName;
            string ns = reader.NamespaceURI;
            bool ret = false;
            if (element.Equals(localName) && targetNs.Equals(ns))
            {
                while (reader.MoveToNextAttribute())
                {
                    string attribute = reader.LocalName;
                    string value = reader.Value;
                    if (targetAttribute.Equals(attribute))
                    {
                        var format = new Uri(baseAddress, value);
                        writer.WriteAttributeString(attribute, format.AbsoluteUri);
                    }
                    else
                    {
                        writer.WriteAttributeString(attribute, value);
                    }
                }

                ret = true;
            }

            return ret;
        }

        #endregion
    }
}
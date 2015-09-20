// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Xml
{
    using System;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///  Utility class providing helper methods to aid with XML.
    /// </summary>
    public static class XmlUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the root node of the XML file..
        /// </summary>
        /// <param name="sourceData">The source data.</param>
        /// <returns>The root node</returns>
        public static string GetRootNode(IReadableDataLocation sourceData)
        {
            try
            {
                var xmlReaderSettings = new XmlReaderSettings { IgnoreComments = true, IgnoreProcessingInstructions = true, IgnoreWhitespace = true };
                using (var stream = sourceData.InputStream)
                using (var parser = XmlReader.Create(stream, xmlReaderSettings))
                {
                    while (parser.Read())
                    {
                        switch (parser.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    return parser.LocalName;
                                }
                        }
                    }
                }

                return null;
            }
            catch (XmlException e)
            {
                throw new SdmxSyntaxException(e, ExceptionCode.XmlParseException);
            }
        }

        /// <summary>
        /// Formats the specified XML String so it has indentation
        /// </summary>
        /// <param name="unformattedXml">
        /// A string representation of some XML 
        /// </param>
        /// <returns>
        /// a formatted version of the input XML. 
        /// </returns>
        public static string FormatXml(string unformattedXml)
        {
            throw new NotImplementedException(
                "in .NET XmlWriter can format output see XmlWriterSettings.Indent and XmlWriter.Create(,XmlWriterSettings)");

            ////try {
            ////  XslCompiledTransform transformer = ILOG.J2CsMapping.XML.XmlTransformerFactory.NewInstance().NewTransformer();
            ////  transformer.SetOutputProperty(Javax.Xml.Transform.OutputKeys.INDENT, "yes");
            ////  XmlWriter result = XmlWriter.Create(new StringWriter(System.Globalization.NumberFormatInfo.InvariantInfo));

            ////  XmlDocumentBuilderFactory dbf = ILOG.J2CsMapping.XML.XmlDocumentBuilderFactory.NewInstance();
            ////  XmlDocumentBuilder db = dbf.NewDocumentBuilder();
            ////  XmlReader mask0 = XmlReader.Create(new StringReader(unformattedXml));
            ////  XmlDocument doc = db.Parse(mask0);

            ////  XmlDocument source = doc;
            ////  transformer.Transform(source, result);
            ////  return result.GetWriter().ToString();
            ////} catch (Exception th) {
            ////  throw new Exception(th.Message, th);
            ////}
        }

        /// <summary>
        /// Returns true if the specified ReadableDataLocation contains valid XML.
        /// </summary>
        /// <param name="sourceData">
        /// The ReadableDataLocation to test 
        /// </param>
        /// <returns>
        /// true if the specified ReadableDataLocation contains valid XML. 
        /// </returns>
        public static bool IsXML(IReadableDataLocation sourceData)
        {
            try
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;
                settings.ValidationType = ValidationType.None;
                using (XmlReader reader = XmlReader.Create(sourceData.InputStream))
                {
                    return reader.Read();
                }
            }
            catch (XmlException)
            {
                return false;
            }

            ////SAXReader reader = new SAXReader();
            ////reader.SetValidation(false);
            ////try {
            ////  reader.Read(sourceData.GetInputStream());
            ////} catch (Exception th) {
            ////  return false;
            ////}
            ////return true;
        }

        #endregion
    }
}
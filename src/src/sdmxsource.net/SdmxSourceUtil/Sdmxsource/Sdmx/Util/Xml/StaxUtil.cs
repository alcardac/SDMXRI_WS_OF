// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaxUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Xml
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// The stax util.
    /// </summary>
    public class StaxUtil
    {
        #region Constants

        /// <summary>
        /// The xml ns.
        /// </summary>
        private const string XmlNS = "http://www.w3.org/XML/1998/namespace";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Copies the current node, namespaces and all attributes.  Does not copy text if deepCopy is false.
        /// </summary>
        /// <param name="parser">The Xml reader
        /// </param>
        /// <param name="deepCopy">
        /// - copies all the children of the current node, copies all text 
        /// </param>
        /// <param name="includeCurrentNode">
        /// - if false will skip the current node and copy all children (only relevent if a deep copy - if false on a shallow copy, nothing will be copied) 
        /// </param>
        /// <param name="includeNamespaces">
        /// The include Namespaces.
        /// </param>
        /// <param name="writers">
        /// The writers.
        /// </param>
        /// <exception cref="XmlException">
        /// - if the parser is not at position XMLStreamConstants.START_ELEMENT
        /// </exception>
        public static void CopyNode(XmlReader parser, bool deepCopy, bool includeCurrentNode, bool includeNamespaces, params XmlWriter[] writers)
        {
            throw new NotImplementedException("Need to check if we need this.");

            ////if (writers == null || writers.Length == 0) {
            ////  throw new ArgumentException(
            ////          "StxUtil.copyNode expects at least one XMLStreamWriter to copy to");
            ////}
            ////if (includeCurrentNode) {
            ////  /* foreach */
            ////  foreach (XmlWriter writer in writers) {
            ////      writer.WriteStartElement(parser.LocalName, parser.NamespaceURI
            ////          );
            ////  }
            ////  if (includeNamespaces) {

            ////      for (int i = 0; i < parser.GetNamespaceCount(); i++) {
            ////          String nsPrefix = parser.GetNamespacePrefix(i);
            ////          String nsUri = parser.GetNamespaceURI(i);
            ////          /* foreach */
            ////          foreach (XMLStreamWriter writer_1 in writers) {
            ////              writer_1.WriteNamespace(nsPrefix, nsUri);
            ////          }
            ////      }
            ////  }
            ////  for (int i_2 = 0; i_2 < parser.GetAttributeCount(); i_2++) {
            ////      String attNs = parser.GetAttributeNamespace(i_2);
            ////      String attName = parser.GetAttributeLocalName(i_2);
            ////      String attVal = parser.GetAttributeValue(i_2);
            ////      if (attNs == null) {
            ////          /* foreach */
            ////          foreach (XMLStreamWriter writer_3 in writers) {
            ////              writer_3.WriteAttribute(attName, attVal);
            ////          }
            ////      } else {
            ////          if (attNs.Equals("http://www.w3.org/XML/1998/namespace")) {
            ////              /* foreach */
            ////              foreach (XMLStreamWriter writer_4 in writers) {
            ////                  writer_4.WriteAttribute("xml", attNs, attName, attVal);
            ////              }
            ////          } else {
            ////              /* foreach */
            ////              foreach (XMLStreamWriter writer_5 in writers) {
            ////                  writer_5.WriteAttribute(attNs, attName, attVal);
            ////              }
            ////          }
            ////      }
            ////  }
            ////}
            ////if (deepCopy) {
            ////  while (parser.HasNext()) {
            ////      int evt0 = parser.Next();
            ////      if (evt0 == Javax.Xml.Stream.XMLStreamConstants.CHARACTERS) {
            ////          /* foreach */
            ////          foreach (XMLStreamWriter writer_6 in writers) {
            ////              writer_6.WriteCharacters(parser.GetText());
            ////          }
            ////      } else if (evt0 == Javax.Xml.Stream.XMLStreamConstants.START_ELEMENT) {
            ////          CopyNode(parser, true, true, includeNamespaces, writers);
            ////      } else if (evt0 == Javax.Xml.Stream.XMLStreamConstants.END_ELEMENT) {
            ////          /* foreach */
            ////          foreach (XMLStreamWriter writer_7 in writers) {
            ////              writer_7.WriteEndElement();
            ////          }
            ////          return;
            ////      }
            ////  }
            ////}
        }

        /// <summary>
        /// Checks if the XML in the first input stream is the same as the XML in the second input stream, throws an exception if they differ
        /// </summary>
        /// <param name="xmlOne">
        /// The xml inputStream 
        /// </param>
        /// <param name="xmlTwo">
        /// The second xml inputStream 
        /// </param>
        /// <param name="ignoreAttrValue">
        /// if specified any attributes with this value will not be tested against the other stream 
        /// </param>
        /// <param name="ignoreNodes">
        /// an optional list of nodes which shouldn't be compared. 
        /// </param>
        /// <exception cref="System.Exception">Throws Exception
        /// </exception>
        public static void IsSameXml(Stream xmlOne, Stream xmlTwo, string ignoreAttrValue, params string[] ignoreNodes)
        {
            throw new NotImplementedException(
                "Need to check if we need this. MS XmlDiff might be an alternative.See http://msdn.microsoft.com/en-us/library/aa302294.aspx");

            ////  XMLInputFactory factory = Javax.Xml.Stream.XMLInputFactory.NewInstance();
            ////  XMLStreamReader parser1 = factory
            ////          .CreateXMLStreamReader(xmlOne, "UTF-8");

            ////  XMLStreamReader parser2 = factory
            ////          .CreateXMLStreamReader(xmlTwo, "UTF-8");
            ////  String nodeA = null;
            ////  String nodeB = null;

            ////  int element = 0;
            ////  int a = -1;
            ////  int b = -1;

            ////  outer: {
            ////      while (parser1.HasNext()) {
            ////          a = parser1.Next();
            ////          b = parser2.Next();
            ////          while (a == Javax.Xml.Stream.XMLStreamConstants.SPACE) {
            ////              a = parser1.Next();
            ////          }
            ////          while (b == Javax.Xml.Stream.XMLStreamConstants.SPACE) {
            ////              b = parser2.Next();
            ////          }
            ////          if (parser1.IsCharacters()) {
            ////              if (Org.Sdmxsource.Util.ObjectUtil.ValidString(parser1.GetText())) {
            ////                  if (!parser2.IsCharacters()) {
            ////                      throw new ArgumentException(
            ////                              "Text Differs on Node:" + nodeA);
            ////                  }
            ////                  if (!parser1.GetText().Equals(parser2.GetText())) {
            ////                      throw new ArgumentException(
            ////                              "Text Differs on Node:" + nodeA + "\nExpected:"
            ////                                      + parser1.GetText() + "\nActual:"
            ////                                      + parser2.GetText());
            ////                  }
            ////              } else {
            ////                  a = parser1.Next();
            ////                  if (parser2.IsCharacters()
            ////                          && !Org.Sdmxsource.Util.ObjectUtil.ValidString(parser2.GetText())) {
            ////                      b = parser2.Next();
            ////                  }
            ////              }
            ////          } else if (parser2.IsCharacters()) {

            ////              if (Org.Sdmxsource.Util.ObjectUtil.ValidString(parser2.GetText())) {
            ////                  throw new ArgumentException("Text Differs on Node:"
            ////                          + nodeA + " : " + parser2.GetText());
            ////              }
            ////              b = parser2.Next();
            ////          }

            ////          if (a == Javax.Xml.Stream.XMLStreamConstants.END_ELEMENT) {
            ////              if (b != a) {
            ////                  nodeA = parser1.GetLocalName();
            ////                  throw new ArgumentException(
            ////                          "Input A is ending XML element: " + nodeA
            ////                                  + " whilst input B is not");
            ////              }
            ////          } else if (a == Javax.Xml.Stream.XMLStreamConstants.START_ELEMENT) {
            ////              element++;
            ////              nodeA = parser1.GetLocalName();
            ////              if (b != Javax.Xml.Stream.XMLStreamConstants.START_ELEMENT) {
            ////                  while (parser2.HasNext()) {
            ////                      b = parser2.Next();
            ////                      if (b == Javax.Xml.Stream.XMLStreamConstants.START_ELEMENT) {
            ////                          nodeB = parser2.GetLocalName();
            ////                          break;
            ////                      }
            ////                  }
            ////              }
            ////              if (b != Javax.Xml.Stream.XMLStreamConstants.START_ELEMENT) {
            ////                  throw new ArgumentException(
            ////                          "Parser B end of document, Parser A on node: "
            ////                                  + nodeA);
            ////              }
            ////              nodeB = parser2.GetLocalName();

            ////              if (!nodeA.Equals(nodeB)) {
            ////                  throw new ArgumentException("XML NODES DIFFER: "
            ////                          + nodeA + "," + nodeB);
            ////              }
            ////              /* foreach */
            ////              foreach (String ignoreNode in ignoreNodes) {
            ////                  if (nodeA.Equals(ignoreNode)) {
            ////                      StaxUtil.SkipNode(parser1);
            ////                      StaxUtil.SkipNode(parser2);
            ////                      goto outer;
            ////                  }
            ////              }

            ////              if (parser1.GetNamespaceCount() != parser2.GetNamespaceCount()) {
            ////                  StringBuilder sb = new StringBuilder();
            ////                  sb.Append("Namespace count for parser 1 : "
            ////                          + parser1.GetNamespaceCount() + "\n");
            ////                  for (int i = 0; i < parser1.GetNamespaceCount(); i++) {
            ////                      sb.Append("Parser 1 Namespace #" + (i + 1) + ":  "
            ////                              + parser1.GetNamespaceURI(i) + "\n");
            ////                  }
            ////                  sb.Append("Namespace count for parser 2 : "
            ////                          + parser2.GetNamespaceCount() + "\n");
            ////                  for (int i_0 = 0; i_0 < parser2.GetNamespaceCount(); i_0++) {
            ////                      sb.Append("Parser 2 Namespace #" + (i_0 + 1) + ":  "
            ////                              + parser2.GetNamespaceURI(i_0) + "\n");
            ////                  }
            ////                  throw new ArgumentException(
            ////                          "Namespace count differs on node:" + nodeA + "\n"
            ////                                  + sb.ToString());
            ////              }

            ////              for (int i_1 = 0; i_1 < parser1.GetNamespaceCount(); i_1++) {
            ////                  bool found = false;
            ////                  String nameSpaceURI = parser1.GetNamespaceURI(i_1);
            ////                  for (int j = 0; j < parser2.GetNamespaceCount(); j++) {
            ////                      if (parser2.GetNamespaceURI(j).Equals(nameSpaceURI)) {
            ////                          found = true;
            ////                          break;
            ////                      }
            ////                  }
            ////                  if (!found) {
            ////                      throw new ArgumentException(
            ////                              "Namespace not found " + nameSpaceURI);
            ////                  }
            ////              }
            ////              if (parser1.GetAttributeCount() != parser2.GetAttributeCount()) {
            ////                  throw new ArgumentException(
            ////                          "Attribute count differs on node:" + nodeA);
            ////              }

            ////              for (int i_2 = 0; i_2 < parser1.GetAttributeCount(); i_2++) {
            ////                  bool found_3 = false;
            ////                  String attributeLocalName = parser1
            ////                          .GetAttributeLocalName(i_2);
            ////                  String attributeNameSpace = parser1
            ////                          .GetAttributeNamespace(i_2);
            ////                  String attributeValue = parser1.GetAttributeValue(i_2);
            ////                  int j_4;
            ////                  for (j_4 = 0; j_4 < parser2.GetAttributeCount(); j_4++) {
            ////                      if (parser2.GetAttributeLocalName(j_4).Equals(
            ////                              attributeLocalName)) {
            ////                          if (attributeNameSpace == null
            ////                                  && parser2.GetAttributeNamespace(j_4) == null) {
            ////                              //THIS IS OKAY
            ////                          } else {
            ////                              if (attributeNameSpace == null
            ////                                      && parser2.GetAttributeNamespace(j_4) != null) {
            ////                                  throw new ArgumentException(
            ////                                          "ATTRIBUTE NAMESPACE DIFFERS "
            ////                                                  + parser2
            ////                                                          .GetAttributeNamespace(i_2)
            ////                                                  + "," + attributeNameSpace);
            ////                              }
            ////                              if (attributeNameSpace != null
            ////                                      && parser2.GetAttributeNamespace(j_4) == null) {
            ////                                  throw new ArgumentException(
            ////                                          "ATTRIBUTE NAMESPACE DIFFERS "
            ////                                                  + parser2
            ////                                                          .GetAttributeNamespace(i_2)
            ////                                                  + "," + attributeNameSpace);
            ////                              }
            ////                              if (!parser2.GetAttributeNamespace(j_4).Equals(
            ////                                      attributeNameSpace)) {
            ////                                  throw new ArgumentException(
            ////                                          "ATTRIBUTE NAMESPACE DIFFERS "
            ////                                                  + parser2
            ////                                                          .GetAttributeNamespace(i_2)
            ////                                                  + "," + attributeNameSpace);
            ////                              }
            ////                          }

            ////                          found_3 = true;
            ////                          break;
            ////                      }

            ////                  }
            ////                  if (!found_3) {
            ////                      throw new ArgumentException(
            ////                              "ATTRIBUTE NOT FOUND ON NODE '" + nodeA
            ////                                      + "' : " + attributeLocalName);
            ////                  }

            ////                  // Ensure the attribute to be tested is not an ignore value then test it 
            ////                  if (!attributeValue.Equals(ignoreAttrValue)) {
            ////                      if (!parser2.GetAttributeValue(j_4)
            ////                              .Equals(attributeValue)) {
            ////                          String line_separator = System.Environment.GetEnvironmentVariable("line.separator");
            ////                          throw new ArgumentException(
            ////                                  "ATTRIBUTE VALUE DIFFERS on NODE :"
            ////                                          + element + line_separator
            ////                                          + attributeValue + line_separator
            ////                                          + parser2.GetAttributeValue(i_2));
            ////                      }
            ////                  }
            ////              }
            ////          }
            ////      }
            ////  }
            ////  gotoouter:
            ////  ;
        }

        /// <summary>
        /// The jump to node.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <param name="findNodeName">
        /// The find node name.
        /// </param>
        /// <param name="doNotProcessPastNodeName">
        /// The do not process past node name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool JumpToNode(XmlReader parser, string findNodeName, string doNotProcessPastNodeName)
        {
            while (parser.Read())
            {
                string nodeName;
                switch (parser.NodeType)
                {
                    case XmlNodeType.Element:
                        nodeName = parser.LocalName;
                        if (nodeName.Equals(findNodeName))
                        {
                            return true;
                        }

                        break;
                    case XmlNodeType.EndElement:
                        nodeName = parser.LocalName;
                        if (!string.IsNullOrEmpty(doNotProcessPastNodeName))
                        {
                            if (nodeName.Equals(doNotProcessPastNodeName))
                            {
                                return false;
                            }
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the current node, all attributes and children as a string.
        /// This just calls <see cref="XmlReader.ReadOuterXml"/>. Use <see cref="XmlReader.ReadOuterXml"/> instead.
        /// </summary>
        /// <param name="parser">The xml reader
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="XmlException">The XML was not well-formed, or an error occurred while parsing the XML.</exception>
        [Obsolete("This just calls XmlReader.ReadOuterXml(). Use XmlReader.ReadOuterXml() instead.")]
        public static string ParseString(XmlReader parser)
        {
            return parser.ReadOuterXml();
        }

        /// <summary>
        /// Moves the parser position forward to the point after this node and all children of this node
        /// This just calls <see cref="XmlReader.Skip"/>. Use <see cref="XmlReader.Skip"/> instead.
        /// </summary>
        /// <param name="parser">The Xml Reader
        /// </param>
        [Obsolete("This just calls XmlReader.Skip(). Use XmlReader.Skip() instead.")]
        public static void SkipNode(XmlReader parser)
        {
            parser.Skip();
        }

        /// <summary>
        /// Moves the parser position forward to the next instance of the end node given name
        /// </summary>
        /// <param name="parser">The Xml Reader
        /// </param>
        /// <param name="nodeName">
        /// The node Name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool SkipToEndNode(XmlReader parser, string nodeName)
        {
            while (parser.Read())
            {
                switch (parser.NodeType)
                {
                    case XmlNodeType.Element:
                        if (parser.LocalName.Equals(nodeName))
                        {
                            if (parser.IsEmptyElement)
                            {
                                return true;
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (parser.LocalName.Equals(nodeName))
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the parser position forward to the next instance of the node with the given name
        /// </summary>
        /// <param name="parser">The xml reader
        /// </param>
        /// <param name="nodeName">
        /// The node Name.
        /// </param>
        /// <returns>
        /// True if the <paramref name="nodeName"/> was found; otherwise false
        /// </returns>
        public static bool SkipToNode(XmlReader parser, string nodeName)
        {
            while (parser.Read())
            {
                switch (parser.NodeType)
                {
                    case XmlNodeType.Element:
                        if (parser.LocalName.Equals(nodeName))
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="senderId">
        /// The sender id.
        /// </param>
        public static void WriteHeader(XmlWriter writer, string id, string senderId)
        {
            writer.WriteStartElement("Header");

            // WRITE ID
            writer.WriteStartElement("ID");
            writer.WriteString(id);
            writer.WriteEndElement();

            // WRITE TEST
            writer.WriteStartElement("Test");
            writer.WriteString("false");
            writer.WriteEndElement();

            // WRITE PREPARED
            writer.WriteStartElement("Prepared");
            writer.WriteString(DateUtil.FormatDate(DateTime.Now));
            writer.WriteEndElement();

            // WRITE SENDER
            writer.WriteStartElement("Sender");
            writer.WriteAttributeString("id", senderId);
            writer.WriteEndElement();

            // END HEADER
            writer.WriteEndElement();
        }

        #endregion
    }
}
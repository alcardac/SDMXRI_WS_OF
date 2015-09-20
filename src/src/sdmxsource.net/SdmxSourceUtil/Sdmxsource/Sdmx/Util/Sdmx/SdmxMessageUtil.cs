// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxMessageUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Sdmx
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Xml;


    #endregion


    /// <summary>
    /// The <c>SDMX</c> message utility class
    /// </summary>
    public class SdmxMessageUtil
    {
        #region Public Methods and Operators

        public static void ParseSdmxErrorMessage(IReadableDataLocation dataLocation)
        {

            using (var stream = dataLocation.InputStream)
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    SdmxErrorCode errorCode = null;
                    string code = null;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                string nodeName = reader.LocalName;
                                if (nodeName.Equals("ErrorMessage"))
                                {
                                    code = reader.GetAttribute("code");
                                    errorCode = SdmxErrorCode.ParseClientCode(int.Parse(code));
                                }
                                else if (nodeName.Equals("Text"))
                                {
                                    if (errorCode == null)
                                    {
                                        errorCode = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError);
                                    }

                                    switch (errorCode.EnumType)
                                    {
                                        case SdmxErrorCodeEnumType.NoResultsFound:
                                            throw new SdmxNoResultsException(reader.Value);
                                        case SdmxErrorCodeEnumType.NotImplemented:
                                            throw new SdmxNotImplementedException(reader.Value);
                                        case SdmxErrorCodeEnumType.SemanticError:
                                            throw new SdmxSemmanticException(reader.Value);
                                        case SdmxErrorCodeEnumType.Unauthorised:
                                            throw new SdmxUnauthorisedException(reader.Value);
                                        case SdmxErrorCodeEnumType.ServiceUnavailable:
                                            throw new SdmxServiceUnavailableException(reader.Value);
                                        case SdmxErrorCodeEnumType.SyntaxError:
                                            throw new SdmxSyntaxException(reader.Value);
                                        case SdmxErrorCodeEnumType.ResponseSizeExceedsServiceLimit:
                                            throw new SdmxResponseSizeExceedsLimitException(reader.Value);
                                        case SdmxErrorCodeEnumType.ResponseTooLarge:
                                            throw new SdmxResponseTooLargeException(reader.Value);
                                        case SdmxErrorCodeEnumType.InternalServerError:
                                            throw new SdmxInternalServerException(reader.Value);
                                        default:
                                            throw new SdmxException(reader.Value, errorCode);
                                    }
                                }
                                break;
                        }
                    }
                }
            }

        }



        /// <summary>
        /// Returns the dataset action
        /// </summary>
        /// <param name="sourceData">
        /// The source Data. 
        /// </param>
        /// <returns>
        /// The <see cref="DatasetActionEnumType"/> . 
        /// </returns>
        public static DatasetActionEnumType GetDataSetAction(IReadableDataLocation sourceData)
        {
            MessageEnumType message = GetMessageType(sourceData);
            bool continueToSubmitStructureRequest = false;
            if (message == MessageEnumType.RegistryInterface)
            {
                RegistryMessageEnumType registryMessageType = GetRegistryMessageType(sourceData);
                if (registryMessageType == RegistryMessageEnumType.SubmitStructureRequest)
                {
                    continueToSubmitStructureRequest = true;
                }
            }

            string action = null;
            using (Stream stream = sourceData.InputStream)
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        string currentNode;
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                currentNode = reader.LocalName;
                                switch (currentNode)
                                {
                                    case "SubmitStructureRequest":
                                        string attribute = reader.GetAttribute("action");
                                        if (attribute != null)
                                        {
                                            return DatasetAction.GetAction(attribute).EnumType;
                                        }

                                        return DatasetActionEnumType.Append;
                                }

                                break;

                            case XmlNodeType.EndElement:
                                currentNode = reader.LocalName;
                                switch (currentNode)
                                {
                                    case "DataSetAction":

                                        switch (action)
                                        {
                                            case "Append":
                                            case "Update":
                                                return DatasetActionEnumType.Append;
                                            case "Replace":
                                                return DatasetActionEnumType.Replace;
                                            case "Delete":
                                                return DatasetActionEnumType.Delete;
                                            case "Information":
                                                return DatasetActionEnumType.Information;
                                        }

                                        break;
                                    case "Header":
                                        if (!continueToSubmitStructureRequest)
                                        {
                                            return DatasetActionEnumType.Append;
                                        }

                                        break;
                                }

                                break;
                            case XmlNodeType.Text:
                                action = reader.Value;
                                break;
                        }
                    }
                }
            }

            return DatasetActionEnumType.Append;
        }

        /// <summary>
        /// Returns MESSAGE_TYPE for the URI.  This is determined from the root node.
        /// </summary>
        /// <param name="sourceData">
        /// The source Data. 
        /// </param>
        /// <returns>
        /// The <see cref="MessageEnumType"/> of the <c>SDMX</c> message in 
        /// </returns>
        public static MessageEnumType GetMessageType(IReadableDataLocation sourceData)
        {
            if (IsEdi(sourceData))
            {
                return MessageEnumType.SdmxEdi;
            }

            using (Stream stream = sourceData.InputStream)
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            string rootNode = reader.LocalName;
                            return MessageType.ParseString(rootNode).EnumType;
                        }
                    }
                }
            }

            throw new ArgumentException("No root node found");
        }

        /// <summary>
        /// Determines the query message types for an input SDMX-ML query message
        /// </summary>
        /// <param name="sourceData">
        /// The source data. 
        /// </param>
        /// <returns>
        /// For v2.1 it will be always one query message due to schema constraints. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// "Can not determine query type
        /// </exception>
        public static IList<QueryMessageEnumType> GetQueryMessageTypes(IReadableDataLocation sourceData)
        {
            MessageEnumType messageType = GetMessageType(sourceData);
            if (messageType != MessageEnumType.Query)
            {
                throw new ArgumentException(
                    "Can not determine query type, as message is of type : " + messageType.ToString());
            }

            IList<QueryMessageEnumType> returnList = new List<QueryMessageEnumType>();

            using (Stream stream = sourceData.InputStream)
            {
                using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true, IgnoreProcessingInstructions = true }))
                {

                    reader.MoveToContent();

                    // Determine by the root node if it is a v2.1 DataQuery
                    var rootStartEvent = reader.NodeType;
                    if (rootStartEvent != XmlNodeType.Element)
                    {
                        throw new ArgumentException("Could not get root start element!");
                    }

                    string rootNode = reader.LocalName;

                    if (rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.GenericDataQuery).NodeName) || rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.StructureSpecificDataQuery).NodeName) ||
                            rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.GenericTimeseriesDataQuery).NodeName) || rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.StructureSpecificTimeseriesDataQuery).NodeName)
                            || rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.GenericMetadataQuery).NodeName) || rootNode.Equals(QueryMessageType.GetFromEnum(QueryMessageEnumType.StructureSpecificMetadataQuery).NodeName))
                    { // don't like but checking with parsingString exception is worse. May be boolean isQueryMessageType() in enum

                        returnList.Add(QueryMessageType.ParseString(rootNode).EnumType);
                        return returnList;
                    }

                    StaxUtil.SkipToNode(reader, "Query");

                    // continue determine type by  Where elements after Query 
                    bool getOut = false;
                    while (!getOut && reader.Read())
                    {
                        string nodeName;
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                nodeName = reader.LocalName;

                                if (nodeName.Equals("ReturnDetails"))
                                { //Skip details of 2.1 queries
                                    StaxUtil.SkipToEndNode(reader, "ReturnDetails");
                                }
                                else
                                {
                                    var queryMessageType = QueryMessageType.ParseString(nodeName);
                                    if (queryMessageType != null)
                                    {
                                        returnList.Add(queryMessageType.EnumType);
                                    }

                                    reader.Skip();
                                }
                                break;
                            case XmlNodeType.EndElement:
                                nodeName = reader.LocalName;
                                getOut = "Query".Equals(nodeName);
                                break;
                        }
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// Returns the registry message type - this will only work if the XML at the
        /// URI is a RegistryInterface message.
        /// The registry message type may be one of the following:
        /// <ul>
        ///   <li>SUBMIT STRUCTURE REQUEST</li>
        ///   <li>SUBMIT PROVISION REQUEST</li>
        ///   <li>SUBMIT REGISTRATION REQUEST</li>
        ///   <li>SUBMIT SUBSCRIPTION REQUEST</li>
        ///   <li>QUERY STRUCTURE REQUEST</li>
        ///   <li>QUERY PROVISION REQUEST</li>
        ///   <li>QUERY REGISTRATION REQUEST</li>
        ///   <li>SUBMIT STRUCTURE RESPONSE</li>
        ///   <li>SUBMIT PROVISION RESPONSE</li>
        ///   <li>SUBMIT REGISTRATION RESPONSE</li>
        ///   <li>QUERY STRUCTURE RESPONSE</li>
        ///   <li>QUERY PROVISION RESPONSE</li>
        ///   <li>QUERY REGISTRATION RESPONSE</li>
        ///   <li>SUBMIT SUBSCRIPTION RESPONSE</li>
        ///   <li>NOTIFY REGISTRY EVENT</li>
        /// </ul>
        /// </summary>
        /// <param name="sourceData">
        /// The source Data. 
        /// </param>
        /// <returns>
        /// The registry message type 
        /// </returns>
        public static RegistryMessageEnumType GetRegistryMessageType(IReadableDataLocation sourceData)
        {
            if (sourceData == null)
            {
                return RegistryMessageEnumType.Null;
            }

            using (Stream stream = sourceData.InputStream)
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    bool skippedHeader = StaxUtil.SkipToEndNode(reader, "Header");
                    if (!skippedHeader)
                    {
                        throw new ArgumentException("Header not found");
                    }

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                string rootNode = reader.LocalName;
                                return RegistryMessageType.GetMessageType(rootNode).EnumType;
                        }
                    }
                }
            }

            return RegistryMessageEnumType.Null;
        }

        /// <summary>
        /// Find SDMX message. Moves to the first SDMX element.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        public static void FindSdmx(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:

                        // should cover most cases
                        switch (reader.NamespaceURI)
                        {
                            case SdmxConstants.MessageNs10:
                            case SdmxConstants.MessageNs20:
                            case SdmxConstants.MessageNs21:
                                return;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Returns the first SDMX element
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        public static XElement FindSdmx(XDocument document)
        {
            var sdmxNamespaces = new[] { SdmxConstants.MessageNs10, SdmxConstants.MessageNs20, SdmxConstants.MessageNs21 };
            return document.Descendants().FirstOrDefault(d => sdmxNamespaces.Contains(d.Name.NamespaceName));
        }

        /// <summary>
        /// Returns the version of the schema that the document stored at this URI points to.
        /// <br/>
        /// The URI is inferred by the namespaces declared in the root element of the document.
        /// <br/>
        /// This will throw an error if there is no way of determining the schema version
        /// </summary>
        /// <param name="sourceData">
        /// The source data 
        /// </param>
        /// <returns>
        /// The <see cref="SdmxSchemaEnumType"/> . 
        /// </returns>
        public static SdmxSchemaEnumType GetSchemaVersion(IReadableDataLocation sourceData)
        {
            if (IsEdi(sourceData))
            {
                return SdmxSchemaEnumType.Edi;
            }

            if (IsEcv(sourceData))
            {
                return SdmxSchemaEnumType.Ecv;
            }

            using (Stream stream = sourceData.InputStream)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:

                                    // should cover most cases
                                    switch (reader.NamespaceURI)
                                    {
                                        case SdmxConstants.MessageNs10:
                                            return SdmxSchemaEnumType.VersionOne;
                                        case SdmxConstants.MessageNs20:
                                            return SdmxSchemaEnumType.VersionTwo;
                                        case SdmxConstants.MessageNs21:
                                            return SdmxSchemaEnumType.VersionTwoPointOne;
                                    }

                                    for (int i = 0; i < reader.AttributeCount; i++)
                                    {
                                        string ns = reader.GetAttribute(i);
                                        if (SdmxConstants.NamespacesV1.Contains(ns))
                                        {
                                            return SdmxSchemaEnumType.VersionOne;
                                        }

                                        if (SdmxConstants.NamespacesV2.Contains(ns))
                                        {
                                            return SdmxSchemaEnumType.VersionTwo;
                                        }

                                        if (SdmxConstants.NamespacesV21.Contains(ns))
                                        {
                                            return SdmxSchemaEnumType.VersionTwoPointOne;
                                        }
                                    }

                                    throw new SdmxSyntaxException("Can not get Scheme Version from SDMX message.  Unable to determine structure type from Namespaces- please ensure this is a valid SDMX document");
                            }
                        }
                    }

                    throw new SdmxSyntaxException(ExceptionCode.XmlParseException, "No root node found");
                }
                catch (XmlException e)
                {
                    throw new SdmxSyntaxException(ExceptionCode.XmlParseException, e);
                }
            }
        }

        /// <summary>
        /// The is ecv delete.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        /// <exception cref="Exception">Throws Exception
        /// </exception>
        public static bool IsEcvDelete(IReadableDataLocation dataLocation)
        {
            Stream stream = dataLocation.InputStream;
            try
            {
                TextReader br = new StreamReader(stream);
                var firstPortion = new char[100];
                br.Read(firstPortion, 0, 10);
                var str = new string(firstPortion);
                return str.ToUpper().StartsWith("ECV-");
            }
            catch (IOException e)
            {
                throw new ArgumentException("Error while trying to read dataLocation:" + dataLocation);
            }
            finally
            {
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (IOException e_0)
                    {
                        throw new Exception(e_0.Message, e_0);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The is ecv.
        /// </summary>
        /// <param name="sourceData">
        /// The source data. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        private static bool IsEcv(IReadableDataLocation sourceData)
        {
            using (Stream stream = sourceData.InputStream)
            {
                try
                {
                    TextReader br = new StreamReader(stream);
                    var firstPortion = new char[100];
                    br.Read(firstPortion, 0, 100);
                    var str = new string(firstPortion); // $$$ UTF encoding
                    return str.ToUpper().StartsWith("ECV");
                }
                catch (IOException e)
                {
                    throw new ArgumentException("Error while trying to read source:" + sourceData);
                }
            }
        }

        /// <summary>
        /// The is edi.
        /// </summary>
        /// <param name="sourceData">
        /// The source data. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        /// <exception cref="Exception">Throws Exception
        /// </exception>
        private static bool IsEdi(IReadableDataLocation sourceData)
        {
            Stream stream = sourceData.InputStream;
            try
            {
                TextReader br = new StreamReader(stream);
                var firstPortion = new char[100];
                br.Read(firstPortion, 0, 100);
                var str = new string(firstPortion);
                return str.ToUpper().StartsWith("UNA");
            }
            catch (IOException e)
            {
                throw new ArgumentException("Error while trying to read source:" + sourceData);
            }
            finally
            {
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (IOException e)
                    {
                        throw new Exception(e.Message, e);
                    }
                }
            }
        }

        #endregion
    }
}
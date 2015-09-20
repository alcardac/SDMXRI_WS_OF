// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataInformationManager.cs" company="Eurostat">
//   Date Created : 2014-07-15
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data information manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;
    using Estat.Sri.SdmxXmlConstants.Builder;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Sdmx.Util.Xml;
    using Org.Sdmxsource.Util.Xml;

    /// <summary>
    ///     The data information manager.
    /// </summary>
    public class DataInformationManager : IDataInformationManager
    {
        #region Fields

        /// <summary>
        ///     The _fixed concept engine
        /// </summary>
        private readonly IFixedConceptEngine _fixedConceptEngine;

        /// <summary>
        ///     The _reported date engine
        /// </summary>
        private readonly IReportedDateEngine _reportedDateEngine;

        /// <summary>
        /// The _XML reader builder
        /// </summary>
        private readonly IXmlReaderBuilder _xmlReaderBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInformationManager"/> class.
        /// </summary>
        public DataInformationManager() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInformationManager"/> class.
        /// </summary>
        /// <param name="fixedConceptEngine">
        /// The fixed concept engine.
        /// </param>
        /// <param name="reportedDateEngine">
        /// The reported date engine.
        /// </param>
        public DataInformationManager(IFixedConceptEngine fixedConceptEngine, IReportedDateEngine reportedDateEngine)
        {
            this._fixedConceptEngine = fixedConceptEngine ?? new FixedConceptEngine();
            this._reportedDateEngine = reportedDateEngine ?? new ReportedDateEngine();
            this._xmlReaderBuilder = new XmlReaderBuilder();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an ordered list of all the unique dates for each time format in the dataset.
        ///     <p/>
        ///     This list is ordered with the earliest date first.
        ///     <p/>
        ///     This method will call 
        /// <code>
        /// reset()
        /// </code>
        /// on the dataReaderEngine before and after the
        ///     information has been gathered
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine
        /// </param>
        /// <returns>
        /// The dictionary of time format to the list of dates that are contained for the time format
        /// </returns>
        public IDictionary<TimeFormat, IList<string>> GetAllReportedDates(IDataReaderEngine dataReaderEngine)
        {
            return this._reportedDateEngine.GetAllReportedDates(dataReaderEngine);
        }

        /// <summary>
        /// Returns DataInformation about the data, this processes the entire dataset to give an overview of what is in the
        ///     dataset.
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        /// <returns>
        /// The DataInformation
        /// </returns>
        public DataInformation GetDataInformation(IDataReaderEngine dre)
        {
            return new DataInformation(dre);
        }

        /// <summary>
        /// Returns the data type for the sourceData
        /// </summary>
        /// <param name="sourceData">
        /// The readable data location
        /// </param>
        /// <returns>
        /// The data type for the sourceData
        /// </returns>
        public IDataFormat GetDataType(IReadableDataLocation sourceData)
        {
            MessageEnumType messageEnumType = SdmxMessageUtil.GetMessageType(sourceData);
            if (messageEnumType == MessageEnumType.SdmxEdi)
            {
                return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.EdiTs));
            }

            if (messageEnumType == MessageEnumType.Error)
            {
                SdmxMessageUtil.ParseSdmxErrorMessage(sourceData);
            }

            // .NET implementation note. There is no XmlReader.GetNamespaceUri(int) in .NET 
            // Also the Java code seems to repeat code already in SdmxMessageUtil. 
            // So the .NET implementation is re-using SdmxMessageUtil to determine the data format and SDMX version.
            var sdmxVersion = SdmxMessageUtil.GetSchemaVersion(sourceData);
            switch (sdmxVersion)
            {
                case SdmxSchemaEnumType.VersionOne:
                    switch (messageEnumType)
                    {
                        case MessageEnumType.GenericData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Generic10));
                        case MessageEnumType.UtilityData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Utility10));
                        case MessageEnumType.CompactData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Compact10));
                        case MessageEnumType.CrossSectionalData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.CrossSectional10));
                        case MessageEnumType.MessageGroup:
                            BaseDataFormatEnumType dataFormat = this.GetMessageGroupDataFormat(sourceData);
                            switch (dataFormat)
                            {
                                case BaseDataFormatEnumType.Compact:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup10Compact));
                                case BaseDataFormatEnumType.Generic:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup10Generic));
                                case BaseDataFormatEnumType.Utility:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup10Utility));
                            }

                            throw new SdmxSyntaxException("Unknown Message Group Format");
                    }

                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    switch (messageEnumType)
                    {
                        case MessageEnumType.GenericData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Generic20));
                        case MessageEnumType.UtilityData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Utility20));
                        case MessageEnumType.CompactData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Compact20));
                        case MessageEnumType.CrossSectionalData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.CrossSectional20));
                        case MessageEnumType.MessageGroup:
                            BaseDataFormatEnumType dataFormat = this.GetMessageGroupDataFormat(sourceData);
                            switch (dataFormat)
                            {
                                case BaseDataFormatEnumType.Compact:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup20Compact));
                                case BaseDataFormatEnumType.Generic:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup20Generic));
                                case BaseDataFormatEnumType.Utility:
                                    return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.MessageGroup20Utility));
                            }

                            throw new SdmxSyntaxException("Unknown Message Group Format");
                    }

                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    switch (messageEnumType)
                    {
                        case MessageEnumType.GenericData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Generic21));
                        case MessageEnumType.CompactData:
                            return new SdmxDataFormatCore(DataType.GetFromEnum(DataEnumType.Compact21));
                    }

                    break;
                default:
                    throw new SdmxSyntaxException(ExceptionCode.XmlParseException, "Can not determine data type unknown namespaces defined");
            }

            string rootNode = XmlUtil.GetRootNode(sourceData);
            throw new SdmxSyntaxException("Unexpected root node '" + rootNode + "'");
        }

        /// <summary>
        /// Returns a list of dimension - value pairs where there is only a single value in the data for the dimension.  For
        ///     example if the entire
        ///     dataset had FREQ=A then one of the returned KeyValue pairs would be FREQ,A.  If FREQ=A and Q this would not be
        ///     returned.
        ///     <p/>
        ///     <b>Note : an initial call to DataReaderEngine.reset will be made</b>
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        /// <param name="includeObs">
        /// The include observation
        /// </param>
        /// <param name="includeAttributes">
        /// If true will also report the attributes that have only one value in the entire dataset
        /// </param>
        /// <returns>
        /// The list of dimension
        /// </returns>
        public IList<IKeyValue> GetFixedConcepts(IDataReaderEngine dre, bool includeObs, bool includeAttributes)
        {
            return this._fixedConceptEngine.GetFixedConcepts(dre, includeObs, includeAttributes);
        }

        /// <summary>
        /// Returns the target namespace of the dataset
        /// </summary>
        /// <param name="sourceData">
        /// The readable data location
        /// </param>
        /// <returns>
        /// The target namespace of the dataset
        /// </returns>
        public string GetTargetNamepace(IReadableDataLocation sourceData)
        {
            using (var stream = sourceData.InputStream)
            {
                return this.JumpToNode(stream, ElementNameTable.DataSet, null, true).NamespaceURI;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the message group data format.
        /// </summary>
        /// <param name="sourceData">
        /// The source data.
        /// </param>
        /// <returns>
        /// The <see cref="BaseDataFormatEnumType"/>.
        /// </returns>
        private BaseDataFormatEnumType GetMessageGroupDataFormat(IReadableDataLocation sourceData)
        {
            using (var stream = sourceData.InputStream)
            using (var parser = this._xmlReaderBuilder.Build(stream))
            {
                while (parser.Read())
                {
                    switch (parser.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                string rootNode = parser.LocalName;
                                if (ElementNameTable.DataSet.Is(rootNode))
                                {
                                    string namespaceUri = parser.NamespaceURI;
                                    if (SdmxConstants.GenericNs10.Equals(namespaceUri) || SdmxConstants.GenericNs20.Equals(namespaceUri))
                                    {
                                        return BaseDataFormatEnumType.Generic;
                                    }

                                    if (SdmxConstants.UtilityNs10.Equals(namespaceUri) || SdmxConstants.UtilityNs20.Equals(namespaceUri))
                                    {
                                        return BaseDataFormatEnumType.Utility;
                                    }

                                    if (SdmxConstants.CompactNs10.Equals(namespaceUri) || SdmxConstants.CompactNs20.Equals(namespaceUri) || namespaceUri.StartsWith("urn"))
                                    {
                                        return BaseDataFormatEnumType.Compact;
                                    }
                                }
                            }

                            break;
                    }
                }
            }

            return BaseDataFormatEnumType.Null;
        }

        /// <summary>
        /// Jumps to node.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="findNodeName">
        /// Name of the find node.
        /// </param>
        /// <param name="doNotProcessPastNodeName">
        /// Name of the do not process past node.
        /// </param>
        /// <param name="throwException">
        /// if set to <c>true</c> [throw exception].
        /// </param>
        /// <returns>
        /// The <see cref="XmlReader"/> at the specific position.
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// Could not find element:  + findNodeName
        /// </exception>
        private XmlReader JumpToNode(Stream stream, ElementNameTable findNodeName, string doNotProcessPastNodeName, bool throwException)
        {
            using (var parser = this._xmlReaderBuilder.Build(stream))
            {
                bool jumpToNode = StaxUtil.JumpToNode(parser, findNodeName.FastToString(), doNotProcessPastNodeName);
                if (!jumpToNode && throwException && doNotProcessPastNodeName != null)
                {
                    throw new SdmxSyntaxException("Could not find element: " + findNodeName);
                }

                return null;
            }
        }

        #endregion
    }
}
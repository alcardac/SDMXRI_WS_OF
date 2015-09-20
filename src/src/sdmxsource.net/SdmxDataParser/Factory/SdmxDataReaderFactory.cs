// -----------------------------------------------------------------------
// <copyright file="SdmxDataReaderFactory.cs" company="Eurostat">
//   Date Created : 2014-05-15
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Factory
{
    using System;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Engine.Reader;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.DataParser.Properties;
    using Org.Sdmxsource.Sdmx.EdiParser.Manager;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;

    /// <summary>
    /// The SDMX data reader factory.
    /// </summary>
    public class SdmxDataReaderFactory : IDataReaderFactory
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxDataReaderFactory));

        /// <summary>
        /// The data information manager
        /// </summary>
        private readonly IDataInformationManager _dataInformationManager;

        /// <summary>
        /// The EDI parse manager
        /// </summary>
        private readonly IEdiParseManager _ediParseManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxDataReaderFactory"/> class.
        /// </summary>
        /// <param name="dataInformationManager">The data information manager.</param>
        /// <param name="ediParseManager">The edi parse manager.</param>
        public SdmxDataReaderFactory(IDataInformationManager dataInformationManager, IEdiParseManager ediParseManager)
        {
            if (dataInformationManager == null)
            {
                throw new ArgumentNullException("dataInformationManager");
            }

            this._dataInformationManager = dataInformationManager;
            this._ediParseManager = ediParseManager;
        }

        /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
        /// </summary>
        /// <param name="sourceData">The source data, giving access to an InputStream of the data.</param>
        /// <param name="dsd">The Data Structure Definition, describes the data in terms of the dimensionality.</param>
        /// <param name="dataflow">The dataflow (optional). Provides further information about the data.</param>
        /// <returns>The <see cref="IDataReaderEngine"/>; otherwise null if the <paramref name="sourceData"/> cannot be read.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sourceData"/> is null -or- <paramref name="dsd"/> is null</exception>
        public IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, IDataStructureObject dsd, IDataflowObject dataflow)
        {
            _log.Debug("Get DataReader Engine");

            if (sourceData == null)
            {
                throw new ArgumentNullException("sourceData");
            }

            if (dsd == null)
            {
                throw new ArgumentNullException("dsd");
            }

            MessageEnumType messageType;
            try
            {
                messageType = SdmxMessageUtil.GetMessageType(sourceData);
            }
            catch (Exception e)
            {
               _log.Error("While trying to get the message type.", e);
                return null;
            }

            var dataFormat = this.GetDataFormat(sourceData, messageType);

            if (dataFormat != null && dataFormat.SdmxDataFormat != null)
            {
                switch (dataFormat.SdmxDataFormat.BaseDataFormat.EnumType)
                {
                    case BaseDataFormatEnumType.Compact:
                        return new CompactDataReaderEngine(sourceData, dataflow, dsd);
                    case BaseDataFormatEnumType.Generic:
                        return new GenericDataReaderEngine(sourceData, dataflow, dsd);
                    case BaseDataFormatEnumType.CrossSectional:
                        var crossDsd = dsd as ICrossSectionalDataStructureObject;
                        if (crossDsd == null)
                        {
                            throw new SdmxNotImplementedException("Not supported. Reading from CrossSectional Data for non cross-sectional dsd.");
                        }

                        return new CrossSectionalDataReaderEngine(sourceData, crossDsd, dataflow);
                    case BaseDataFormatEnumType.Edi:
                        if (this._ediParseManager != null)
                        {
                            return this._ediParseManager.ParseEdiMessage(sourceData).GetDataReader(dsd, dataflow);
                        }

                        break;
                    default:
                        _log.WarnFormat("SdmxDataReaderFactory encountered unsupported SDMX format: {0} ", dataFormat);
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
        /// </summary>
        /// <param name="sourceData">
        /// The source data, giving access to an InputStream of the data.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager. It is used to obtain the <see cref="IDataStructureObject"/> that describe the data.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/>; otherwise null if the <paramref name="sourceData"/> cannot be read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceData"/> is null -or- <paramref name="retrievalManager"/> is null
        /// </exception>
        public IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, ISdmxObjectRetrievalManager retrievalManager)
        {
            //// TODO check with MT why the two methods are so different.
            IDataFormat dataFormat = null;
            try
            {
                dataFormat = this._dataInformationManager.GetDataType(sourceData);
            }
            catch (Exception e)
            {
                _log.Error("While trying to get the data format", e);
            }

            if (dataFormat != null && dataFormat.SdmxDataFormat != null)
            {
                switch (dataFormat.SdmxDataFormat.BaseDataFormat.EnumType)
                {
                    case BaseDataFormatEnumType.Compact:
                        return new CompactDataReaderEngine(sourceData, retrievalManager, null, null);
                    case BaseDataFormatEnumType.Generic:
                        return new GenericDataReaderEngine(sourceData, retrievalManager, null, null);
                    case BaseDataFormatEnumType.CrossSectional:
                        return new CrossSectionalDataReaderEngine(sourceData, retrievalManager, null, null);
                    case BaseDataFormatEnumType.Edi:
                        var ediParseManager = this._ediParseManager;
                        if (ediParseManager != null)
                        {
                            return ediParseManager.ParseEdiMessage(sourceData).GetDataReader(retrievalManager);
                        }

                        break;
                    default:
                        _log.WarnFormat("SdmxDataReaderFactory encountered unsupported SDMX format: {0} ", dataFormat);
                        break;
                }
            }
 
            return null;
        }

        /// <summary>
        /// Gets the data format.
        /// </summary>
        /// <param name="sourceData">The source data.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>The <see cref="IDataFormat"/></returns>
        private IDataFormat GetDataFormat(IReadableDataLocation sourceData, MessageEnumType messageType)
        {
            switch (messageType)
            {
                case MessageEnumType.Error:
                    SdmxMessageUtil.ParseSdmxErrorMessage(sourceData);
                    break;
                case MessageEnumType.GenericData:
                case MessageEnumType.CompactData:
                case MessageEnumType.CrossSectionalData:
                case MessageEnumType.UtilityData:
                case MessageEnumType.SdmxEdi:
                    try
                    {
                        return this._dataInformationManager.GetDataType(sourceData);
                    }
                    catch (Exception e)
                    {
                        _log.Error("While trying to get the data format", e);
                    }

                    break;
                default:
                    throw new ArgumentException(Resources.ExceptionCannotProccessMessage + MessageType.GetFromEnum(messageType).NodeName, "sourceData");
            }

            return null;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxXmlStream.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// Implementation of <see cref="ISdmxXmlStream"/> for XML streaming and carrying SDMX-ML message information. Note <see cref="IReadableDataLocation.InputStream"/> and <see cref="IWriteableDataLocation.OutputStream"/> are not implemented.
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="SdmxXmlStream" />.
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManagerFast.cs" lang="cs" />
    /// </example> 
    public class SdmxXmlStream : BaseReadableDataLocation, IWriteableDataLocation, ISdmxXmlStream
    {
        #region Fields

        /// <summary>
        /// The _message type.
        /// </summary>
        private readonly MessageEnumType _messageType;

        /// <summary>
        /// The _reader.
        /// </summary>
        private readonly XmlReader _reader;

        /// <summary>
        /// The _registry type.
        /// </summary>
        private readonly RegistryMessageEnumType _registryType;

        /// <summary>
        /// The _schema enum type.
        /// </summary>
        private readonly SdmxSchemaEnumType _sdmxVersion;

        /// <summary>
        /// The _writer.
        /// </summary>
        private readonly XmlWriter _writer;

        /// <summary>
        /// The _query message type
        /// </summary>
        private IList<QueryMessageEnumType> _queryMessageTypes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <param name="sdmxVersion">
        /// The schema enum type.
        /// </param>
        /// <param name="registryType">
        /// The registry type.
        /// </param>
        public SdmxXmlStream(XmlReader reader, MessageEnumType messageType, SdmxSchemaEnumType sdmxVersion, RegistryMessageEnumType registryType)
        {
            if (!SdmxSchema.GetFromEnum(sdmxVersion).IsXmlFormat())
            {
                throw new ArgumentException("Input not an SDMX-ML file", "sdmxVersion");
            }

            this._reader = reader;
            this._messageType = messageType;
            this._sdmxVersion = sdmxVersion;
            this._registryType = registryType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <param name="sdmxVersion">
        /// The schema enum type.
        /// </param>
        /// <param name="registryType">
        /// The registry type.
        /// </param>
        public SdmxXmlStream(XmlNode node, MessageEnumType messageType, SdmxSchemaEnumType sdmxVersion, RegistryMessageEnumType registryType)
        {
            if (!SdmxSchema.GetFromEnum(sdmxVersion).IsXmlFormat())
            {
                throw new ArgumentException("Input not an SDMX-ML file", "sdmxVersion");
            }

            this._reader = new XmlNodeReader(node);
            this.AddDisposable(this._reader);
            this._messageType = messageType;
            this._sdmxVersion = sdmxVersion;
            this._registryType = registryType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public SdmxXmlStream(XmlWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="prettify">
        /// Prettify output.
        /// </param>
        public SdmxXmlStream(IWriteableDataLocation writer, bool prettify)
        {
            var xmlStream = writer as ISdmxXmlStream;
            if (xmlStream != null)
            {
                this._writer = xmlStream.Writer;
            }
            else
            {
                var settings = new XmlWriterSettings();
                if (prettify)
                {
                    settings.Indent = true;
                    settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                }

                this._writer = XmlWriter.Create(writer.OutputStream);
                this.AddDisposable(this._writer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="prettify">Prettify output</param>
        public SdmxXmlStream(Stream writer, bool prettify)
        {
            var settings = new XmlWriterSettings();
            if (prettify)
            {
                settings.Indent = true;
                settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            }

            this._writer = XmlWriter.Create(writer);
            this.AddDisposable(this._writer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxXmlStream"/> class.
        /// </summary>
        /// <param name="readableDataLocation">
        /// The readable data location.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Input not an SDMX-ML file
        /// </exception>
        public SdmxXmlStream(IReadableDataLocation readableDataLocation)
        {
            var xmlStream = readableDataLocation as ISdmxXmlStream;
            if (xmlStream != null)
            {
                this._reader = xmlStream.Reader;
                this._messageType = xmlStream.MessageType;
                this._sdmxVersion = xmlStream.SdmxVersion;
                this._registryType = xmlStream.RegistryType;
                this._queryMessageTypes = xmlStream.QueryMessageTypes;
            }
            else
            {
                this._sdmxVersion = SdmxMessageUtil.GetSchemaVersion(readableDataLocation);
                if (!SdmxSchema.GetFromEnum(this._sdmxVersion).IsXmlFormat())
                {
                    throw new ArgumentException("Input not an SDMX-ML file", "readableDataLocation");
                }

                this._messageType = SdmxMessageUtil.GetMessageType(readableDataLocation);
                this._registryType = this._messageType != MessageEnumType.RegistryInterface ? RegistryMessageEnumType.Null : SdmxMessageUtil.GetRegistryMessageType(readableDataLocation);
                if (this._messageType == MessageEnumType.Query)
                {
                    this._queryMessageTypes = SdmxMessageUtil.GetQueryMessageTypes(readableDataLocation);
                }
            
                this._reader = XMLParser.CreateSdmxMlReader(readableDataLocation.InputStream, this._sdmxVersion);
                this.AddDisposable(this._reader);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether has reader.
        /// </summary>
        public bool HasReader
        {
            get
            {
                return this._reader != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has writer.
        /// </summary>
        public bool HasWriter
        {
            get
            {
                return this._writer != null;
            }
        }

        /// <summary>
        /// Gets the input stream.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// This implementation does not support a re-Readable Stream. Use Reader instead.
        /// </exception>
        public override Stream InputStream
        {
            get
            {
                throw new NotSupportedException("This implementation does not support a re-Readable Stream. Use Reader instead.");
            }
        }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageEnumType MessageType
        {
            get
            {
                return this._messageType;
            }
        }

        /// <summary>
        ///     Gets the query message type.
        /// </summary>
        public IList<QueryMessageEnumType> QueryMessageTypes
        {
            get
            {
                return this._queryMessageTypes;
            }
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        public XmlReader Reader
        {
            get
            {
                return this._reader;
            }
        }

        /// <summary>
        /// Gets the registry type.
        /// </summary>
        public RegistryMessageEnumType RegistryType
        {
            get
            {
                return this._registryType;
            }
        }

        /// <summary>
        /// Gets the schema enum type.
        /// </summary>
        public SdmxSchemaEnumType SdmxVersion
        {
            get
            {
                return this._sdmxVersion;
            }
        }

        /// <summary>
        /// Gets the writer.
        /// </summary>
        public XmlWriter Writer
        {
            get
            {
                return this._writer;
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets the input stream.
        /// </summary>
        Stream IReadableDataLocation.InputStream
        {
            get
            {
                return this.InputStream;
            }
        }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// This implementation does not support a re-Readable Stream. Use Writer instead.
        /// </exception>
        Stream IWriteableDataLocation.OutputStream
        {
            get
            {
                throw new NotSupportedException("This implementation does not support a re-Readable Stream. Use Writer instead.");
            }
        }

        #endregion
    }
}
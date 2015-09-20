// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-03-21
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The abstract writing engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    using Xml.Schema.Linq;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using System.Collections.Generic;

    /// <summary>
    ///     The abstract writing engine.
    /// </summary>
    public abstract class AbstractWritingEngine : IStructureWriterEngine
    {
        #region Fields

        /// <summary>
        ///     The output stream.
        /// </summary>
        private readonly Stream _outputStream;

        /// <summary>
        ///     controls if output should be pretty (indented and no duplicate namespaces)
        /// </summary>
        private readonly bool _prettyfy;

        /// <summary>
        ///     the schema version
        /// </summary>
        private readonly SdmxSchemaEnumType _schemaVersion;

        /// <summary>
        ///     The xml writer.
        /// </summary>
        private readonly XmlWriter _writer;

        private SchemaLocationWriter _schemaLocationWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWritingEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        protected AbstractWritingEngine(XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this._writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWritingEngine"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <param name="prettyfy">
        /// controls if output should be pretty (indented and no duplicate namespaces)
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outputStream"/> is null
        /// </exception>
        protected AbstractWritingEngine(SdmxSchemaEnumType schemaVersion, Stream outputStream, bool prettyfy)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }

            this._outputStream = outputStream;
            this._schemaVersion = schemaVersion;
            this._prettyfy = prettyfy;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sets the schema location writer.
        /// </summary>
        /// <param name="schemaLocation">The schema location.</param>
        public void SetSchemaLocationWriter(SchemaLocationWriter schemaLocation)
        {
            this._schemaLocationWriter = schemaLocation;
        }

        /// <summary>
        /// The write structure.
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        public virtual void WriteStructure(IMaintainableObject bean)
        {
            ISdmxObjects beans = new SdmxObjectsImpl();
            beans.AddIdentifiable(bean);
            this.WriteStructures(beans);
        }

        /// <summary>
        /// Writes the beans to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="beans">
        /// The SDMX Objects to write
        /// </param>
        public virtual void WriteStructures(ISdmxObjects beans)
        {
            XTypedElement docV21 = this.Build(beans);
            if (this._writer != null)
            {
                Write(this._writer, docV21);
            }
            else
            {
                var settings = new XmlWriterSettings();
                if (this._schemaLocationWriter != null)
                {
                    List<string> schemaUri = new List<string>();
                    switch (_schemaVersion)
                    {
                        case SdmxSchemaEnumType.VersionOne:
                            schemaUri.Add(SdmxConstants.MessageNs10);
                            break;
                        case SdmxSchemaEnumType.VersionTwo:
                            schemaUri.Add(SdmxConstants.MessageNs20);
                            break;
                        case SdmxSchemaEnumType.VersionTwoPointOne:
                            schemaUri.Add(SdmxConstants.MessageNs21);
                            break;
                        default: throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Schema Version " + _schemaVersion);
                    }
                    this._schemaLocationWriter.WriteSchemaLocation(docV21, schemaUri.ToArray());
                }
                if (this._prettyfy)
                {
                    settings.Indent = true;
                    settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                }

                using (XmlWriter xmlWriter = XmlWriter.Create(this._outputStream, settings))
                {
                    Write(xmlWriter, docV21);
                }

                this._outputStream.Flush();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the XSD generated class objects from the specified <paramref name="beans"/>
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// the XSD generated class objects from the specified <paramref name="beans"/>
        /// </returns>
        protected internal abstract XTypedElement Build(ISdmxObjects beans);

        /// <summary>
        /// Write the <paramref name="message"/> using <paramref name="xmlWriter"/>
        /// </summary>
        /// <param name="xmlWriter">
        /// The xml writer.
        /// </param>
        /// <param name="message">
        /// The SDMX message
        /// </param>
        private static void Write(XmlWriter xmlWriter, XTypedElement message)
        {
            if (xmlWriter.WriteState == WriteState.Start)
            {
                message.Untyped.Save(xmlWriter);
            }
            else
            {
                //// this is needed for NSI WS & SOAP. We get a XmlWriter where the document has already started with SOAP envelope
                message.Untyped.WriteTo(xmlWriter);
            }

            xmlWriter.Flush();
        }

        #endregion
    }
}
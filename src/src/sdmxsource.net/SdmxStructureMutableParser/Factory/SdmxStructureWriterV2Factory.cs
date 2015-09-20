// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxStructureWriterV2Factory.cs" company="Eurostat">
//   Date Created : 2013-03-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <c>SDMX</c> Structure writer engine factory that uses <see cref="StructureWriterV2" /> and
//   <see cref="RegistryInterfaceWriterV2" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Factory
{
    using System;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Engine.V2;
    using Estat.Sri.SdmxStructureMutableParser.Properties;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing;

    /// <summary>
    ///     The <c>SDMX</c> Structure writer engine factory that uses <see cref="StructureWriterV2" /> and
    ///     <see cref="RegistryInterfaceWriterV2" /> for SDMX v2.0
    /// </summary>
    public class SdmxStructureWriterV2Factory : IStructureWriterFactory
    {
        #region Fields

        /// <summary>
        ///     The XML writer instance. May be null.
        /// </summary>
        private readonly XmlWriter _writer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureWriterV2Factory"/> class.
        /// </summary>
        /// <param name="writer">
        /// The XML writer instance to use with SDMX-ML <see cref="IStructureWriterEngine"/>.
        /// </param>
        public SdmxStructureWriterV2Factory(XmlWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SdmxStructureWriterV2Factory" /> class.
        /// </summary>
        public SdmxStructureWriterV2Factory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Obtains a StructureWritingEngine engine for the given output format
        /// </summary>
        /// <param name="structureFormat">
        /// An implementation of the StructureFormat to describe the output format for the structures (required)
        /// </param>
        /// <param name="streamWriter">
        /// The output stream to write to (can be null if it is not required)
        /// </param>
        /// <returns>
        /// Null if this factory is not capable of creating a data writer engine in the requested format
        /// </returns>
        public IStructureWriterEngine GetStructureWriterEngine(IStructureFormat structureFormat, Stream streamWriter)
        {
            if (streamWriter != null && !streamWriter.CanWrite)
            {
                throw new ArgumentException(Resources.ExcepteptionCannotWriteToStream, "streamWriter");
            }

            if (structureFormat.SdmxOutputFormat != null)
            {
                var outputFormat = structureFormat.SdmxOutputFormat;
                SdmxSchema schemaVersion = outputFormat.OutputVersion;
                if (!outputFormat.IsQueryResponse && !outputFormat.IsRegistryDocument)
                {
                    return this.GetObjectEngine(schemaVersion, streamWriter);
                }

                if (outputFormat.EnumType == StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument)
                {
                    if (this._writer != null)
                    {
                        return new RegistryInterfaceWriterV2(this._writer);
                    }

                    return new RegistryInterfaceWriterV2(streamWriter);
                }
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the <see cref="IStructureWriterEngine"/> engine.
        /// </summary>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureWriterEngine"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// The specified value at <paramref name="schemaVersion"/> is not supported
        /// </exception>
        private IStructureWriterEngine GetObjectEngine(BaseConstantType<SdmxSchemaEnumType> schemaVersion, Stream stream)
        {
            switch (schemaVersion.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                    return this._writer != null ? new StructureWriterEngineV1(this._writer) : new StructureWriterEngineV1(stream);
                case SdmxSchemaEnumType.VersionTwo:
                    return this._writer != null ? new StructureWriterV2(this._writer) : new StructureWriterV2(stream);
                case SdmxSchemaEnumType.Edi:
                    return new StructureWriterEngineEdi(stream);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._writer != null ? new StructureWriterEngineV21(this._writer) : new StructureWriterEngineV21(stream);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion + " - StructureWritingManagerImpl.writeStructure");
            }
        }

        #endregion
    }
}
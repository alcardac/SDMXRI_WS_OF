// -----------------------------------------------------------------------
// <copyright file="SdmxStructureWriterFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Factory
{
    using System.IO;
    using System.Xml;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SdmxStructureWriterFactory : IStructureWriterFactory
    {
        /// <summary>
        /// The XML writer instance. May be null.
        /// </summary>
        private readonly XmlWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureWriterFactory"/> class. 
        /// </summary>
        /// <param name="writer">
        /// The XML writer instance to use with SDMX-ML <see cref="IStructureWriterEngine"/>.
        /// </param>
        public SdmxStructureWriterFactory(XmlWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureWriterFactory"/> class. 
        /// </summary>
        public SdmxStructureWriterFactory()
        {
        }

        /// <summary>
        /// Obtains a StructureWritingEngine engine for the given output format
        /// </summary>
        /// <param name="structureFormat">An implementation of the StructureFormat to describe the output format for the structures (required)</param>
        /// <param name="streamWriter">The output stream to write to (can be null if it is not required)</param>
        /// <returns>Null if this factory is not capable of creating a data writer engine in the requested format</returns>
        public IStructureWriterEngine GetStructureWriterEngine(
            IStructureFormat structureFormat, Stream streamWriter)
        {
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
                        return new RegistryQueryResponseWriterEngineV2(this._writer);
                    }

                    return new RegistryQueryResponseWriterEngineV2(streamWriter);
                }
            }

            return null;
        }

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
        /// <exception cref="UnsupportedException">
        /// The specified value at <paramref name="schemaVersion"/> is not supported
        /// </exception>
        private IStructureWriterEngine GetObjectEngine(SdmxSchema schemaVersion, Stream stream)
        {
            switch (schemaVersion.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                    return this._writer != null ? new StructureWriterEngineV1(this._writer) : new StructureWriterEngineV1(stream);
                case SdmxSchemaEnumType.VersionTwo:
                    return this._writer != null ? new StructureWriterEngineV2(this._writer) : new StructureWriterEngineV2(stream);
                case SdmxSchemaEnumType.Edi:
                    return new StructureWriterEngineEdi(stream);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return this._writer != null ? new StructureWriterEngineV21(this._writer) : new StructureWriterEngineV21(stream);
                default:
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported, schemaVersion + " - StructureWritingManagerImpl.writeStructure");
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="RDFStructureWriterFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace RDFProvider.Structure.Factory
{
    using System.IO;
    using System.Xml;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using RDFProvider.Structure.Engine;
    using RDFProvider.Structure.Writing;
  

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RDFStructureWriterFactory : IStructureRDFWriterFactory
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
        public RDFStructureWriterFactory(XmlWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureWriterFactory"/> class. 
        /// </summary>
        public RDFStructureWriterFactory()
        {
        }

        public IStructureRDFWriterEngine GetStructureWriterEngine(Stream streamWriter)
        {
            return this.GetObjectEngine(streamWriter);                       
        }

        private IStructureRDFWriterEngine GetObjectEngine(Stream stream)
        {
            return this._writer != null ? new StructureWriterEngineRDF(this._writer) : new StructureWriterEngineRDF(stream);
        }
    }
}

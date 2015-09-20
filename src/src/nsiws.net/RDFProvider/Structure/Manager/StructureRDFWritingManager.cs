// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRDFWritingManager.cs" company="ISTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Structure.Manager
{
    using System.IO;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using RDFProvider.Structure.Factory;
    using RDFProvider.Structure.Manager.Output;
    using RDFProvider.Structure.Engine;
    
    

    /// <summary>
    ///   The structure writing manager implementation.
    /// </summary>
    /// <example>
    ///   A sample implementation in C# of <see cref="StructureWritingManager" />.
    ///   <code source="..\ReUsingExamples\Structure\ReUsingStructureWritingManager.cs" lang="cs" />
    /// </example>
    public class StructureRDFWritingManager : IStructureRDFWriterManager
    {
        #region Static Fields

        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureRDFWritingManager));

        private readonly IStructureRDFWriterFactory _structureRDFWriterFactory;

        public StructureRDFWritingManager()
            : this(null)
        {
        }

        public StructureRDFWritingManager(IStructureRDFWriterFactory structureWriterFactory)
        {
            this._structureRDFWriterFactory = structureWriterFactory ?? new RDFStructureWriterFactory();
        }

        #endregion

        public void RDFWriteStructures(ISdmxObjects sdmxObjects, Stream outputStream)
        {
            _log.Debug("Write Structures as RDF" );
            this.GetStructureWritingEngine(outputStream).RDFWriteStructures(sdmxObjects);
        }

        private IStructureRDFWriterEngine GetStructureWritingEngine(Stream outputStream)
        {
            IStructureRDFWriterEngine engine = this._structureRDFWriterFactory.GetStructureWriterEngine(outputStream);
            if (engine != null)
            {
                return engine;
            }

            throw new SdmxNotImplementedException("Could not write structures out in format: RDF");
        }

    }
}
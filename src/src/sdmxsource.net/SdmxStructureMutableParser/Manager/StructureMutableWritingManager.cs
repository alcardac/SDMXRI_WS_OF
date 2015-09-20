// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMutableWritingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing manager implementation.
//   It supports only a small subset of SDMX v2.0.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Manager
{
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Factory;

    using Org.Sdmxsource.Sdmx.Structureparser.Manager;

    /// <summary>
    ///     The structure writing manager implementation.
    ///     It supports only a small subset of SDMX v2.0.
    /// </summary>
    /// <remarks>
    ///     Only the following structures are supported:
    ///     - Category Schemes (<c>MetaDataflow Ref</c> are ignored)
    ///     - Codelists
    ///     - Concept schemes
    ///     - Dataflows
    ///     - Hierarchical Codelists
    ///     - KeyFamilies (DSD)
    /// </remarks>
    /// <example>
    ///     A sample implementation in C# of <see cref="StructureMutableWritingManager" />.
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureWritingManagerFast.cs" lang="cs" />
    /// </example> 
    public class StructureMutableWritingManager : StructureWriterManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMutableWritingManager"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public StructureMutableWritingManager(XmlWriter writer)
            : base(new SdmxStructureWriterV2Factory(writer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMutableWritingManager"/> class.
        /// </summary>
        public StructureMutableWritingManager()
            : base(new SdmxStructureWriterV2Factory())
        {
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureValidationManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure validation manager implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The structure validation manager implementation
    /// </summary>
    public class StructureValidationManager : BaseParsingManager, IStructureValidationManager
    {
        #region Static Fields

        /// <summary>
        ///     The output format.
        /// </summary>
        private static readonly StructureOutputFormat _outputFormat =
            StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument);

        #endregion

        #region Fields

        /// <summary>
        ///     The structure parsing manager.
        /// </summary>
        private readonly IStructureParsingManager _structureParsingManager;

        /// <summary>
        ///     The structure writing manager.
        /// </summary>
        private readonly IStructureWriterManager _structureWritingManager;

        /// <summary>
        /// The _readable data location factory
        /// </summary>
        private readonly IReadableDataLocationFactory _readableDataLocationFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureValidationManager"/> class. 
        /// </summary>
        /// <param name="structureParsingManager">
        /// The structure parsing manager.
        /// </param>
        /// <param name="structureWritingManager">
        /// The structure writing manager.
        /// </param>
        /// <param name="readableDataLocationFactory">
        /// The readable data location factory.
        /// </param>
        public StructureValidationManager(IStructureParsingManager structureParsingManager, IStructureWriterManager structureWritingManager, IReadableDataLocationFactory readableDataLocationFactory)
        {
            this._structureParsingManager = structureParsingManager ?? new StructureParsingManager();
            this._structureWritingManager = structureWritingManager ?? new StructureWriterManager();
            this._readableDataLocationFactory = readableDataLocationFactory ?? new ReadableDataLocationFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureValidationManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        public StructureValidationManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
            this._structureWritingManager = new StructureWriterManager();
            this._structureParsingManager = new StructureParsingManager(sdmxSchema);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Validates a structure is compliant to the 2.1 SDMX schema, by writing it out as SDMX and validating against the schema.
        ///     WARNING: This implementation writes temporary to memory.
        /// </summary>
        /// <param name="maintainableBean">
        /// The maintainable object
        /// </param>
        public virtual void ValidateStructure(IMaintainableObject maintainableBean)
        {
            using (var memoryStream = new MemoryStream())
            {
                this._structureWritingManager.WriteStructure(maintainableBean, null, new SdmxStructureFormat(_outputFormat), memoryStream);
                memoryStream.Position = 0;

                using (IReadableDataLocation dataLocation = this._readableDataLocationFactory.GetReadableDataLocation(memoryStream.ToArray()))
                {
                    this._structureParsingManager.ParseStructures(dataLocation);
                }
            }
        }

        #endregion
    }
}
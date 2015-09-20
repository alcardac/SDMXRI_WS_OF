// -----------------------------------------------------------------------
// <copyright file="StructureWriterManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Manager
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Factory;
    using Org.Sdmxsource.Util;

    public class StructureWriterManager : IStructureWriterManager
    {
        #region Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureWriterManager));

        /// <summary>
        ///     The structure writer factory.
        /// </summary>
        private readonly List<IStructureWriterFactory> _structureWriterFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterManager"/> class.
        /// </summary>
        public StructureWriterManager()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterManager"/> class.
        /// </summary>
        /// <param name="structureWriterFactory">
        /// The structure writer factory. If set to null the default factory will be used: <see cref="SdmxStructureWriterFactory"/>
        /// </param>
        public StructureWriterManager(params IStructureWriterFactory[] structureWriterFactory)
        {
            if (!ObjectUtil.ValidCollection<IStructureWriterFactory>(structureWriterFactory))
            {
                this._structureWriterFactory = new List<IStructureWriterFactory> { new SdmxStructureWriterFactory() };
            }
            else
            {
                this._structureWriterFactory = new List<IStructureWriterFactory>(structureWriterFactory);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the contents of the beans out to the output stream in the format specified.
        /// Will write the header information contained within the SdmxBeans container if there is a header present.  If the header
        /// is not present a default header will be created
        /// </summary>
        /// <param name="sdmxObjects">
        /// The SDMX objects to write to the output stream.
        /// </param>
        /// <param name="outputFormat">
        /// The output format of the message (required)
        /// </param>
        /// <param name="outputStream">
        /// the stream to write to, the stream is closed on completion, this can be null if not required (i.e the outputFormat may contain a writer)
        /// </param>
        public void WriteStructures(ISdmxObjects sdmxObjects, IStructureFormat outputFormat, Stream outputStream)
        {
            _log.Debug("Write Structures as " + outputFormat);
            GetStructureWritingEngine(outputFormat, outputStream).WriteStructures(sdmxObjects);
        }

        /// <summary>
        /// Writes the contents of the bean out to the output stream in the version specified.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObjects to write to the output stream
        /// </param>
        /// <param name="header">
        /// can be null, if null will create a default header
        /// </param>
        /// <param name="outputFormat">
        /// the output format of the message (required)
        /// </param>
        /// <param name="outputStream">
        /// the stream to write to, the stream is NOT closed on completion, this can be null if not required (i.e the outputFormat may contain a writer)
        /// </param>
        public void WriteStructure(IMaintainableObject sdmxObject, IHeader header, IStructureFormat outputFormat, Stream outputStream)
        {
            _log.Debug("Write Structure '" + sdmxObject + "' as " + outputFormat);
            GetStructureWritingEngine(outputFormat, outputStream).WriteStructure(sdmxObject);
        }

        /// <summary>
        /// Return structure writing engine.
        /// </summary>
        /// <param name="outputFormat">
        /// The output format.
        /// </param>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureWriterEngine"/>.
        /// </returns>
        private IStructureWriterEngine GetStructureWritingEngine(IStructureFormat outputFormat, Stream outputStream)
        {
            foreach (var structureWriterFactory in this._structureWriterFactory)
            {
                IStructureWriterEngine engine = structureWriterFactory.GetStructureWriterEngine(outputFormat, outputStream);
                if (engine != null)
                {
                    return engine;
                }
            }

            throw new SdmxNotImplementedException("Could not write structures out in format: " + outputFormat);
        }

        #endregion
    }
}

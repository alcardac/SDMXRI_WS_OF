// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure parsing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;

    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Factory;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Log;


    /// <summary>
    ///     The structure parsing manager implementation.
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="StructureParsingManager" />.
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManager.cs" lang="cs" />
    /// </example>
    public class StructureParsingManager : IStructureParsingManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureParsingManager));

        #endregion

        #region Fields

        /// <summary>
        ///     The external reference manager.
        /// </summary>
        private readonly IExternalReferenceManager _externalReferenceManager = new ExternalReferenceManager();

        /// <summary>
        ///     The structure parser factory
        /// </summary>
        private readonly IList<IStructureParserFactory> _structureParserFactories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureParsingManager"/> class.
        /// </summary>
        public StructureParsingManager() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        public StructureParsingManager(SdmxSchemaEnumType sdmxSchema)
        {
            this._structureParserFactories = new List<IStructureParserFactory> { new SdmxStructureParserFactory(MessageEnumType.Null, sdmxSchema, RegistryMessageEnumType.Null, null, null, null, null) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureParsingManager"/> class.
        /// </summary>
        /// <param name="structureParserFactory">
        /// The structure writer factory. If set to null the default factory will be used: <see cref="SdmxStructureParserFactory"/>
        /// </param>
        public StructureParsingManager(params IStructureParserFactory[] structureParserFactory)
        {
            if (!ObjectUtil.ValidCollection<IStructureParserFactory>(structureParserFactory))
            {
                this._structureParserFactories = new List<IStructureParserFactory> { new SdmxStructureParserFactory() };
            }
            else
            {
                this._structureParserFactories = new List<IStructureParserFactory>(structureParserFactory); 
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build workspace.
        /// </summary>
        /// <param name="sdmxObjects">
        /// The input SDMX objects.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureWorkspace"/>.
        /// </returns>
        public virtual IStructureWorkspace BuildWorkspace(
            ISdmxObjects sdmxObjects, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager)
        {
            if (settings.ResolveExternalReferences)
            {
                this._externalReferenceManager.ResolveExternalReferences(
                    sdmxObjects, settings.SubstituteExternal, settings.Lenient);
            }

            return new StructureWorkspace(
                sdmxObjects,
                retrievalManager,
                settings.ResolveCrossReferences,
                settings.ResolveAgencyReferences,
                settings.ResolutionDepth);
        }

        /// <summary>
        /// Parses a structure document OR a Registry document that contains structures.
        /// </summary>
        /// <param name="dataLocation">
        /// - the supplied structures
        /// </param>
        /// <returns>
        /// StructureWorkspace - from this structures can be retrieved in any format required
        /// </returns>
        /// <remarks>
        /// Validates the SDMX-ML against the correct schema, also validates the structure according to the SDMX standards,
        ///     using rules which can not be specified by schema.
        ///     Uses the default parsing settings, which is to not validate cross references, and therefore no <c>SdmxBeanRetrievalManager</c> is
        ///     required.
        /// </remarks>
        public virtual IStructureWorkspace ParseStructures(IReadableDataLocation dataLocation)
        {
            var settings = new ResolutionSettings(
                ResolveExternalSetting.DoNotResolve, ResolveCrossReferences.DoNotResolve);
            return this.ParseStructures(dataLocation, settings, null);
        }

        /// <summary>
        /// Parses a structure document OR a Registry document that contains structures.
        /// </summary>
        /// <param name="dataLocation">
        /// The structure location
        /// </param>
        /// <param name="settings">
        /// - addition settings to perform when parsing
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// StructureWorkspace - from this structures can be retrieved in any format required
        /// </returns>
        /// <remarks>
        /// Validates the SDMX-ML against the correct schema, also validates the structure according to the SDMX standards,
        ///     using rules which can not be specified by schema.  Uses the supplied settings to perform any extra operations.  If
        ///     resolve external references is set to true, then these structures will also be validated against the Schema and business logic.
        /// </remarks>
        public virtual IStructureWorkspace ParseStructures(
            IReadableDataLocation dataLocation, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager)
        {
            LoggingUtil.Debug(_log, "Parse Structure request, for xml at location: " + dataLocation);

            return this.BuildWorkspace(GetSdmxObjects(dataLocation), settings, retrievalManager);
        }

        /// <summary>
        /// Gets the sdmx objects from the source data location..
        /// </summary>
        /// <param name="sourceData">
        /// - the supplied structures
        /// </param>
        /// <returns>
        /// The Sdmx objects
        /// </returns>
        private ISdmxObjects GetSdmxObjects(IReadableDataLocation sourceData)
        {
            foreach (var structureParserFactory in this._structureParserFactories)
            {
                ISdmxObjects beans = structureParserFactory.GetSdmxObjects(sourceData);
                if (beans != null)
                {
                    return beans;
                }
            }

            throw new SdmxNotImplementedException("Can not parse structures.  Structure format is either not supported, or has an invalid syntax");
        }

        #endregion
    }
}
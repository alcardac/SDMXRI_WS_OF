// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMutableParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure mutable parsing manager.
//   It supports only a small subset of SDMX v2.0.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Manager
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using Estat.Sri.SdmxStructureMutableParser.Engine.V2;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.XmlHelper;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The structure mutable parsing manager.
    ///     It supports only a small subset of SDMX v2.0.
    /// </summary>
    /// <remarks>
    ///     Only the following structures are supported:
    ///     - Category Schemes (Metadataflow Ref are ignored)
    ///     - Codelists
    ///     - Concept schemes
    ///     - Dataflows
    ///     - Hierarchical Codelists
    ///     - KeyFamilies (DSD)
    /// </remarks>
    /// <example>
    ///     A sample implementation in C# of <see cref="StructureMutableParsingManager" />.
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManagerFast.cs" lang="cs" />
    /// </example> 
    public class StructureMutableParsingManager : IStructureParsingManager
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
        ///     The _registry reader.
        /// </summary>
        private readonly RegistryInterfaceReaderV2 _registryReader = new RegistryInterfaceReaderV2();

        /// <summary>
        ///     The _structure reader.
        /// </summary>
        private readonly StructureReaderV2 _structureReader = new StructureReaderV2();

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
        /// The <see cref="IStructureWorkspace"/> .
        /// </returns>
        public IStructureWorkspace BuildWorkspace(ISdmxObjects sdmxObjects, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager)
        {
            if (settings.ResolveExternalReferences)
            {
                this._externalReferenceManager.ResolveExternalReferences(sdmxObjects, settings.SubstituteExternal, settings.Lenient);
            }

            return new StructureWorkspace(sdmxObjects, retrievalManager, settings.ResolveCrossReferences, settings.ResolveAgencyReferences, settings.ResolutionDepth);
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
        public IStructureWorkspace ParseStructures(IReadableDataLocation dataLocation, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager)
        {
            var xmlStream = dataLocation as ISdmxXmlStream;
            if (xmlStream != null)
            {
                return this.ParseStructures(xmlStream, settings, retrievalManager);
            }

            _log.DebugFormat("Parse Structure request, for xml at location: {0} ", dataLocation);
            MessageEnumType structureType = SdmxMessageUtil.GetMessageType(dataLocation);
            XmlReaderSettings xmlSettings = XMLParser.GetSdmxXmlReaderSettings(SdmxSchemaEnumType.VersionTwo);
            xmlSettings.ValidationEventHandler += OnValidationEventHandler;
            xmlSettings.NameTable = NameTableCache.Instance.NameTable;

            using (Stream inputStream = dataLocation.InputStream)
            using (XmlReader reader = XmlReader.Create(inputStream, xmlSettings))
            {
                xmlStream = new SdmxXmlStream(reader, structureType, SdmxSchemaEnumType.VersionTwo, RegistryMessageEnumType.QueryStructureResponse);
                return this.ParseStructures(xmlStream, settings, retrievalManager);
            }
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
        public IStructureWorkspace ParseStructures(IReadableDataLocation dataLocation)
        {
            var settings = new ResolutionSettings(ResolveExternalSetting.DoNotResolve, ResolveCrossReferences.DoNotResolve);
            return this.ParseStructures(dataLocation, settings, null);
        }

        /// <summary>
        /// Parses a structure document OR a Registry document that contains structures.
        /// </summary>
        /// <param name="reader">
        /// The reader.
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
        public IStructureWorkspace ParseStructures(ISdmxXmlStream reader, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager)
        {
            if (!reader.HasReader)
            {
                throw new ArgumentException("ISdmxXmlStream doesnt have a Reader", "reader");
            }

            IMutableObjects objects = null;
            switch (reader.MessageType)
            {
                case MessageEnumType.RegistryInterface:
                    IRegistryInfo registryInfo = this._registryReader.Read(reader.Reader);
                    if (registryInfo.HasQueryStructureResponse && registryInfo.QueryStructureResponse.StatusMessage.Status != Status.Error)
                    {
                        objects = registryInfo.QueryStructureResponse.Structure;
                    }

                    break;
                case MessageEnumType.Structure:
                    objects = this._structureReader.Read(reader.Reader);
                    break;
            }

            if (objects == null)
            {
                objects = new MutableObjectsImpl();
            }

            ISdmxObjects immutableBeans = objects.ImmutableObjects;
            return this.BuildWorkspace(immutableBeans, settings, retrievalManager);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on validation event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected static void OnValidationEventHandler(object sender, ValidationEventArgs args)
        {
            _log.Warn(args.Message);
            if (args.Exception == null)
            {
                return;
            }

            _log.Warn(args.Exception);
            _log.WarnFormat("{3}:{0} Column: {1}. Error:\n {2}", args.Exception.SourceUri, args.Exception.LineNumber, args.Exception.LinePosition, args.Exception.Message);
            throw args.Exception;
        }

        #endregion
    }
}
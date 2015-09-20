// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryQueryResponseWriterEngineV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registry query response writing engine v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2;

    using Xml.Schema.Linq;

    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The registry query response writing engine v 2.
    /// </summary>
    public class RegistryQueryResponseWriterEngineV2 : AbstractWritingEngine
    {
        #region Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(RegistryQueryResponseWriterEngineV2));

        /// <summary>
        ///     The query structure response builder v 2.
        /// </summary>
        private readonly QueryStructureResponseBuilderV2 _queryStructureResponseBuilderV2 =
            new QueryStructureResponseBuilderV2();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryQueryResponseWriterEngineV2"/> class. 
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        public RegistryQueryResponseWriterEngineV2(XmlWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryQueryResponseWriterEngineV2"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <param name="prettyFy">
        /// controls if output should be pretty (indented and no duplicate namespaces)
        /// </param>
        public RegistryQueryResponseWriterEngineV2(Stream outputStream, bool prettyFy)
            : base(SdmxSchemaEnumType.VersionTwo, outputStream, prettyFy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryQueryResponseWriterEngineV2"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public RegistryQueryResponseWriterEngineV2(Stream outputStream)
            : base(SdmxSchemaEnumType.VersionTwo, outputStream, true)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the XSD generated class objects from the specified <paramref name="beans"/>
        /// </summary>
        /// <param name="beans">
        /// The sdmxObjects.
        /// </param>
        /// <returns>
        /// the XSD generated class objects from the specified <paramref name="beans"/>
        /// </returns>
        protected internal override XTypedElement Build(ISdmxObjects beans)
        {
            _log.Info("Write structures as a SDMX 2.0 Registry response message");
            return this._queryStructureResponseBuilderV2.BuildSuccessResponse(beans);
        }

        #endregion
    }
}
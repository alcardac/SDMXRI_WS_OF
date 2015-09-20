// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWritingEngineV1.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing engine for SDMX v1.0
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1;

    using Xml.Schema.Linq;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The structure writing engine for SDMX v1.0
    /// </summary>
    public class StructureWriterEngineV1 : AbstractWritingEngine
    {
        #region Fields

        /// <summary>
        ///     The structure xml sdmxObject builder sdmxObject.
        /// </summary>
        private readonly StructureXmlBuilder _structureXmlBuilder = new StructureXmlBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV1"/> class. 
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        public StructureWriterEngineV1(XmlWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV1"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <param name="prettyFy">
        /// controls if output should be pretty (indented and no duplicate namespaces)
        /// </param>
        public StructureWriterEngineV1(Stream outputStream, bool prettyFy)
            : base(SdmxSchemaEnumType.VersionOne, outputStream, prettyFy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV1"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public StructureWriterEngineV1(Stream outputStream)
            : base(SdmxSchemaEnumType.VersionOne, outputStream, true)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the XSD generated class objects from the specified <paramref name="beans"/>
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// the XSD generated class objects from the specified <paramref name="beans"/>
        /// </returns>
        protected internal override XTypedElement Build(ISdmxObjects beans)
        {
            return this._structureXmlBuilder.Build(beans);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWriterEngineV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing engine v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;

    using Xml.Schema.Linq;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The structure writing engine v 2.
    /// </summary>
    public class StructureWriterEngineV2 : AbstractWritingEngine
    {
        #region Fields

        /// <summary>
        ///     The structure xml bean builder bean.
        /// </summary>
        private readonly StructureXmlBuilder _structureXmlBuilderBean = new StructureXmlBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV2"/> class. 
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public StructureWriterEngineV2(XmlWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV2"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <param name="prettyFy">
        /// controls if output should be pretty (indented and no duplicate namespaces)
        /// </param>
        public StructureWriterEngineV2(Stream outputStream, bool prettyFy)
            : base(SdmxSchemaEnumType.VersionTwo, outputStream, prettyFy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV2"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public StructureWriterEngineV2(Stream outputStream)
            : base(SdmxSchemaEnumType.VersionTwo, outputStream, true)
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
            return this._structureXmlBuilderBean.Build(beans);
        }

        #endregion
    }
}
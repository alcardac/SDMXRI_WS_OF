// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class provides the method to read a SDMX Structure Message and output a <see cref="IMutableObjects" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Util.Exception;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     This class provides the method to read a SDMX Structure Message and output a <see cref="IMutableObjects" />
    /// </summary>
    /// <remarks>
    ///     Only the following structures are supported:
    ///     - Category Schemes
    ///     - Codelists
    ///     - Concept schemes
    ///     - Dataflows
    ///     - Hierarchical Codelists
    ///     - KeyFamilies (DSD)
    /// </remarks>
    public class StructureReaderV2 : StructureReaderBaseV20
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public StructureReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StructureReaderV2" /> class.
        /// </summary>
        public StructureReaderV2()
            : this(null)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses the reader opened against the stream containing the SDMX-ML Structure message
        ///     and returns a a IMutableObjects object. Internally, this method uses a XmlReader.
        ///     This is the central method of the class
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// textReader is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="reader">
        /// The xml reader opened against the stream for the SDMX-ML Query
        /// </param>
        /// <returns>
        /// The IMutableObjects object
        /// </returns>
        public IMutableObjects Read(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            reader.MoveToContent();
            object firstElement = reader.LocalName;
            if (!NameTableCache.IsElement(firstElement, ElementNameTable.Structure))
            {
                // TODO discuss throw exception because the first element is not Structure
                return null;
            }

            this.ReadHeader(reader);

            return this.Read(reader, new MutableObjectsImpl());
        }

        #endregion

        #region Methods

        /////// <summary>
        ///////     Initialize handlers based on parent type for Elements and element text
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add element text handlers
        ////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevelBase);
        ////}

        #endregion

        /// <summary>
        /// Handle top level elements.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            this.HandleTopLevelBase(parent, localName);
            return null;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class writes a SDMX Model IMutableObjects object to a stream
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Properties;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     This class writes a SDMX Model IMutableObjects object to a stream
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
    public class StructureWriterV2 : StructureWriterBaseV2, IStructureWriterEngine
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public StructureWriterV2(XmlWriter writer)
            : this(writer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public StructureWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterV2"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public StructureWriterV2(Stream stream)
            : this(XmlWriter.Create(stream))
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException(Resources.ExcepteptionCannotWriteToStream, "stream");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This is the main method of the class and is used to write the
        ///     <see cref="IMutableObjects"/>
        ///     to the <see cref="System.Xml.XmlTextWriter"/> given in the constructor
        /// </summary>
        /// <param name="structure">
        /// The <see cref="IMutableObjects"/> object we want to write
        /// </param>
        /// <param name="header">
        /// The SDMX message header.
        /// </param>
        public void WriteStructure(IMutableObjects structure, IHeader header)
        {
            if (structure == null)
            {
                throw new ArgumentNullException("structure");
            }

            bool startNow = this.SdmxMLWriter.WriteState == WriteState.Start;
            if (startNow)
            {
                this.WriteMessageTag(ElementNameTable.Structure);

                // print message header
                this.WriteMessageHeader(header);
            }

            /*
             * Sequence:
             *    <xs:element name="OrganisationSchemes" type="structure:OrganisationSchemesType" minOccurs="0"/>
             *  <xs:element name="Dataflows" type="structure:DataflowsType" minOccurs="0"/>
             *  <xs:element name="Metadataflows" type="structure:MetadataflowsType" minOccurs="0"/>
             *  <xs:element name="CategorySchemes" type="structure:CategorySchemesType" minOccurs="0"/>                    
             *  <xs:element name="CodeLists" type="structure:CodeListsType" minOccurs="0"/>
             *  <xs:element name="HierarchicalCodelists" type="structure:HierarchicalCodelistsType" minOccurs="0"/>
             *  <xs:element name="Concepts" type="structure:ConceptsType" minOccurs="0"/>
             *  <xs:element name="MetadataStructureDefinitions" type="structure:MetadataStructureDefinitionsType" minOccurs="0"/>
             *  <xs:element name="KeyFamilies" type="structure:KeyFamiliesType" minOccurs="0"/>
             *  <xs:element name="StructureSets" type="structure:StructureSetsType" minOccurs="0"/>
             *  <xs:element name="ReportingTaxonomies" type="structure:ReportingTaxonomiesType" minOccurs="0"/>                    
             *  <xs:element name="Processes" type="structure:ProcessesType" minOccurs="0"/>
             */

            // write dataflows
            if (structure.Dataflows.Count > 0)
            {
                var writer = new DataflowWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            // write category schemes
            if (structure.CategorySchemes.Count > 0)
            {
                var writer = new CategorySchemeWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            // write codelists
            if (structure.Codelists.Count > 0)
            {
                var writer = new CodeListWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            // write HCL
            if (structure.HierarchicalCodelists.Count > 0)
            {
                var writer = new HierarchicalCodeListWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            // write concept schemes
            if (structure.ConceptSchemes.Count > 0)
            {
                var writer = new ConceptSchemeWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            // write key families
            if (structure.DataStructures.Count > 0)
            {
                var writer = new DataStructureWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.SetTopElementsNS(this.RootNamespace);
                writer.Write(structure);
            }

            if (startNow)
            {
                // close document
                this.WriteEndElement();
                this.WriteEndDocument();
            }
        }

        /// <summary>
        /// Writes the <paramref name="maintainableObject"/> out to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainableObject.
        /// </param>
        public void WriteStructure(IMaintainableObject maintainableObject)
        {
            IMutableObjects mutableObjects = new MutableObjectsImpl();
            mutableObjects.AddIdentifiable(maintainableObject.MutableInstance);
            this.WriteStructure(mutableObjects, null);
        }

        /// <summary>
        /// Writes the sdmxObjects to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="sdmxObjects">
        /// SDMX objects
        /// </param>
        public void WriteStructures(ISdmxObjects sdmxObjects)
        {
            this.WriteStructure(sdmxObjects.MutableObjects, sdmxObjects.Header);
        }

        #endregion
    }
}
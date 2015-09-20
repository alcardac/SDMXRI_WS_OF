// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The concept scheme writer v 2.
    /// </summary>
    internal class ConceptSchemeWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public ConceptSchemeWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Write.the specified <paramref name="structure"/>
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        public void Write(IMutableObjects structure)
        {
            this.WriteConcepts(structure.ConceptSchemes);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write the element Concept using the given IConceptMutableObject object
        /// </summary>
        /// <param name="item">
        /// The IConceptMutableObject object to write
        /// </param>
        private void WriteConcept(IConceptMutableObject item)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Concept);
            this.WriteIdentifiableArtefactAttributes(item);

            ////TryWriteAttribute(AttributeNameTable.version, item.Version);
            ////TryWriteAttribute(AttributeNameTable.validFrom, (item).ValidFrom);
            ////TryWriteAttribute(AttributeNameTable.validTo, (item).ValidTo);
            this.TryWriteAttribute(AttributeNameTable.parent, item.ParentConcept);
            this.TryWriteAttribute(AttributeNameTable.parentAgency, item.ParentAgency);

            IStructureReference representation = null;
            ITextFormatMutableObject textFormat = null;
            if (item.CoreRepresentation != null)
            {
                representation = item.CoreRepresentation.Representation;
                textFormat = item.CoreRepresentation.TextFormat;
            }

            if (representation != null)
            {
                this.TryWriteAttribute(
                    AttributeNameTable.coreRepresentation, representation.MaintainableReference.MaintainableId);
                this.TryWriteAttribute(
                    AttributeNameTable.coreRepresentationAgency, representation.MaintainableReference.AgencyId);

                ////TryWriteAttribute(AttributeNameTable.coreRepresentationVersion, item.CoreRepresentation.Representation.MaintainableReference.Version);
            }

            this.WriteIdentifiableArtefactContent(item);

            if (textFormat != null)
            {
                this.WriteTextFormat(textFormat);
            }

            this.WriteAnnotations(ElementNameTable.Annotations, item.Annotations);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the concept schemes inside the <paramref name="conceptSchemes"/>
        /// </summary>
        /// <param name="conceptSchemes">
        /// The <see cref="IConceptSchemeMutableObject"/> collection
        /// </param>
        private void WriteConcepts(IEnumerable<IConceptSchemeMutableObject> conceptSchemes)
        {
            this.WriteStartElement(this.RootNamespace, ElementNameTable.Concepts);
            foreach (IConceptSchemeMutableObject itemScheme in conceptSchemes)
            {
                this.WriteMaintainableArtefact(ElementNameTable.ConceptScheme, itemScheme);
                foreach (IConceptMutableObject item in itemScheme.Items)
                {
                    this.WriteConcept(item);
                }

                this.WriteAnnotations(ElementNameTable.Annotations, itemScheme.Annotations);
                this.WriteEndElement();
            }

            this.WriteEndElement();
        }

        #endregion
    }
}
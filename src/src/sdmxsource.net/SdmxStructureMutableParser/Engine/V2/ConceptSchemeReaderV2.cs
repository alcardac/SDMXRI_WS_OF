// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The concept scheme reader v 2.
    /// </summary>
    internal class ConceptSchemeReaderV2 : StructureReaderBaseV20
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public ConceptSchemeReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Structure top level elements
        ///     This includes Concept Scheme
        /// </summary>
        /// <param name="parent">
        ///     The parent <see cref="IMutableObjects"/>
        /// </param>
        /// <param name="localName">
        ///     The name of the current xml element
        /// </param>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.ConceptScheme))
            {
                var cs = new ConceptSchemeMutableCore();
                ParseAttributes(cs, this.Attributes);
                parent.AddConceptScheme(cs);
                actions = this.AddNameableAction(cs, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the ConceptScheme element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IConceptSchemeMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private ElementActions HandleChildElements(IConceptSchemeMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Concept))
            {
                var concept = new ConceptMutableCore();
                ParseAttributes(concept, this.Attributes);

                string representation = Helper.TrySetFromAttribute(
                    this.Attributes, AttributeNameTable.coreRepresentation, string.Empty);
                if (!string.IsNullOrWhiteSpace(representation))
                {
                    concept.CoreRepresentation = new RepresentationMutableCore();
                    string coreRepresentationAgency = Helper.TrySetFromAttribute(
                        this.Attributes, AttributeNameTable.coreRepresentationAgency, string.Empty);
                    concept.CoreRepresentation.Representation = new StructureReferenceImpl(
                        coreRepresentationAgency, representation, null, SdmxStructureEnumType.CodeList);

                    /* TODO check if coreRepresentationVersion was used somewhere.
                     * var coreRepresentationVersion = Helper.TrySetFromAttribute(
                        this.Attributes, AttributeNameTable.coreRepresentationVersion, string.Empty); */
                }

                concept.ParentConcept = Helper.TrySetFromAttribute(
                    this.Attributes, AttributeNameTable.parent, concept.ParentConcept);
                concept.ParentAgency = Helper.TrySetFromAttribute(
                    this.Attributes, AttributeNameTable.parentAgency, concept.ParentAgency);
                parent.AddItem(concept);

                actions = this.AddNameableAction(concept, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the Concept element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IConceptMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private ElementActions HandleChildElements(IConceptMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.TextFormat))
            {
                parent.CoreRepresentation = new RepresentationMutableCore
                                                {
                                                    TextFormat =
                                                        HandleTextFormat(this.Attributes)
                                                };
                return ElementActions.Empty;
            }

            return null;
        }

        #endregion

        ///////// <summary>
        /////////     Initialize handlers based on parent type for Elements and element text
        ///////// </summary>
        //////private void InitializeTypeSwitch()
        //////{
        //////    // add element text handlers
        //////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        //////    this.AddHandleText<INameableMutableObject>(
        //////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        //////    // add element handlers
        //////    this.AddHandleElement<IConceptMutableObject>(this.HandleChildElements);
        //////    this.AddHandleElement<IConceptSchemeMutableObject>(this.HandleChildElements);
        //////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        //////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        //////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        //////}
    }
}
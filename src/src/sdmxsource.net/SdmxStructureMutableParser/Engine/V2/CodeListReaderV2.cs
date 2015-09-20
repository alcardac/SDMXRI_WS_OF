// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code list reader for SDMX v2.0
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    ///     The code list reader for SDMX v2.0
    /// </summary>
    internal class CodeListReaderV2 : StructureReaderBaseV20
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public CodeListReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Structure top level elements
        ///     This includes Codelist
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="IMutableObjects"/>
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CodeList))
            {
                ICodelistMutableObject codelist = new CodelistMutableCore();
                ParseAttributes(codelist, this.Attributes);
                parent.AddCodelist(codelist);
                actions = this.AddNameableAction(codelist, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the CodeList element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICodelistMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ICodelistMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Code))
            {
                var code = new CodeMutableCore();
                code.Id = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.value, code.Id);
                code.ParentCode = Helper.TrySetFromAttribute(
                    this.Attributes, AttributeNameTable.parentCode, code.ParentCode);
                code.Urn = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.urn, code.Urn);
                parent.AddItem(code);
                actions = this.BuildElementActions(code, this.HandleAnnotableChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handle the common text elements for <paramref name="code"/> based objects
        /// </summary>
        /// <param name="code">
        /// The parent <see cref="ICodeMutableObject"/>
        /// </param>
        /// <param name="localName">
        /// The local name of the XML tag
        /// </param>
        private void HandleTextChildElement(ICodeMutableObject code, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.Description))
            {
                //// NOTE in SDMX 2.0 Code has only description. The Common API expects it to have Name. So we put Code description to Name.
                code.Names.Add(new TextTypeWrapperMutableCore { Value = this.Text, Locale = this.Lang });
            }
        }

        #endregion

        ///////// <summary>
        /////////     Initialize handlers based on parent type for Elements and element text
        ///////// </summary>
        //////private void InitializeTypeSwitch()
        //////{
        //////    // add element text handlers
        //////    this.AddHandleText<ICodeMutableObject>(this.HandleTextChildElement);
        //////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        //////    this.AddHandleText<INameableMutableObject>(
        //////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        //////    // add element handlers
        //////    this.AddHandleElement<ICodelistMutableObject>(this.HandleChildElements);
        //////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        //////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        //////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        //////}
    }
}
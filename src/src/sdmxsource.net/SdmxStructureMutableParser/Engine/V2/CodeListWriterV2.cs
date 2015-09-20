// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code list writer v 2.
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
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;

    /// <summary>
    ///     The code list writer v 2.
    /// </summary>
    internal class CodeListWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public CodeListWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        public void Write(IMutableObjects structure)
        {
            this.WriteCodeLists(structure.Codelists);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write the element Code using the given code
        /// </summary>
        /// <param name="item">
        /// The <see cref="ICodeMutableObject"/> object to write
        /// </param>
        private void WriteCode(ICodeMutableObject item)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Code);
            this.WriteAttributeString(AttributeNameTable.value, item.Id);
            this.TryWriteAttribute(AttributeNameTable.urn, item.Urn);
            this.TryWriteAttribute(AttributeNameTable.parentCode, item.ParentCode);

            //// NOTE in SDMX 2.0 Code has only description. The Common API expects it to have Name. So we put Code description to Name.
            foreach (ITextTypeWrapperMutableObject textTypeBean in item.Names)
            {
                this.WriteTextType(this.DefaultNS, textTypeBean, ElementNameTable.Description);
            }

            this.WriteAnnotations(ElementNameTable.Annotations, item.Annotations);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the codelists inside the <paramref name="codeLists"/>
        /// </summary>
        /// <param name="codeLists">
        /// The <see cref="ICodelistMutableObject"/> collection
        /// </param>
        private void WriteCodeLists(IEnumerable<ICodelistMutableObject> codeLists)
        {
            this.WriteStartElement(this.RootNamespace, ElementNameTable.CodeLists);
            foreach (ICodelistMutableObject codelist in codeLists)
            {
                this.WriteMaintainableArtefact(ElementNameTable.CodeList, codelist);
                foreach (ICodeMutableObject code in codelist.Items)
                {
                    this.WriteCode(code);
                }

                this.WriteAnnotations(ElementNameTable.Annotations, codelist.Annotations);
                this.WriteEndElement();
            }

            this.WriteEndElement();
        }

        #endregion
    }
}
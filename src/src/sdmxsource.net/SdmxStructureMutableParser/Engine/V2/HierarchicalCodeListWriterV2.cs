// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodeListWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical code list writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;

    /// <summary>
    ///     The hierarchical code list writer v 2.
    /// </summary>
    internal class HierarchicalCodeListWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeListWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public HierarchicalCodeListWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
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
            this.WriteHierarchicalCodeLists(structure.HierarchicalCodelists);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Traverse the HCL CodeRef hierarchy tree and write each CodeRef starting from
        ///     the given ICodeRefMutableObject object.
        /// </summary>
        /// <param name="item">
        /// The root ICategoryMutableObject object
        /// </param>
        private void TraverseCodeRefTree(ICodeRefMutableObject item)
        {
            var parents = new Stack<ICodeRefMutableObject>();
            var open = new Dictionary<ICodeRefMutableObject, Queue<ICodeRefMutableObject>>();
            this.WriteCodeRef(item);

            parents.Push(item);
            open.Add(item, new Queue<ICodeRefMutableObject>(item.CodeRefs));
            while (parents.Count > 0)
            {
                ICodeRefMutableObject parent = parents.Peek();
                Queue<ICodeRefMutableObject> remainingCodeRef = open[parent];
                while (remainingCodeRef.Count > 0)
                {
                    ICodeRefMutableObject current = remainingCodeRef.Dequeue();
                    this.WriteCodeRef(current);
                    parents.Push(current);
                    remainingCodeRef = new Queue<ICodeRefMutableObject>(current.CodeRefs);
                    open.Add(current, remainingCodeRef);

                    // parent = parents.Peek();
                }

                parent = parents.Pop();
                this.WriteCodeRefTrailingElements(parent);
                this.WriteEndElement();
            }
        }

        /// <summary>
        /// The write code ref.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        private void WriteCodeRef(ICodeRefMutableObject codeRef)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.CodeRef);
            // this.TryToWriteElement(this.DefaultNS, ElementNameTable.URN, codeRef.Urn);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.CodelistAliasRef, codeRef.CodelistAliasRef);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.CodeID, codeRef.Id);
        }

        /// <summary>
        /// The write code ref trailing elements.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        private void WriteCodeRefTrailingElements(ICodeRefMutableObject codeRef)
        {
            var level = string.IsNullOrEmpty(codeRef.LevelReference) ? null : codeRef.LevelReference.Split('.').Last();
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.LevelRef, level);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.NodeAliasID, codeRef.CodeId);

            ////TryToWriteElement(this.DefaultNS, ElementNameTable.Version, codeRef.Version);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.ValidFrom, codeRef.ValidFrom);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.ValidTo, codeRef.ValidTo);
        }

        /// <summary>
        /// The write codelist ref.
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist ref.
        /// </param>
        private void WriteCodelistRef(ICodelistRefMutableObject codelistRef)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.CodelistRef);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.URN, codelistRef.CodelistReference.MaintainableUrn);
            this.TryToWriteElement(
                this.DefaultNS, ElementNameTable.AgencyID, codelistRef.CodelistReference.MaintainableReference.AgencyId);
            this.TryToWriteElement(
                this.DefaultNS, 
                ElementNameTable.CodelistID, 
                codelistRef.CodelistReference.MaintainableReference.MaintainableId);
            this.TryToWriteElement(
                this.DefaultNS, ElementNameTable.Version, codelistRef.CodelistReference.MaintainableReference.Version);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.Alias, codelistRef.Alias);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the codelists inside the <see cref="StructureWriterBaseV2._structure"/>
        /// </summary>
        /// <param name="hierarchicalCodelists">
        /// The collection of <c>Hierarchical Codelists</c>
        /// </param>
        private void WriteHierarchicalCodeLists(IEnumerable<IHierarchicalCodelistMutableObject> hierarchicalCodelists)
        {
            this.WriteStartElement(this.RootNamespace, ElementNameTable.HierarchicalCodelists);
            foreach (IHierarchicalCodelistMutableObject hcl in hierarchicalCodelists)
            {
                this.WriteMaintainableArtefact(ElementNameTable.HierarchicalCodelist, hcl);
                foreach (ICodelistRefMutableObject codelistRef in hcl.CodelistRef)
                {
                    this.WriteCodelistRef(codelistRef);
                }

                foreach (IHierarchyMutableObject hierachy in hcl.Hierarchies)
                {
                    this.WriteHierarchy(hierachy);
                }

                this.WriteAnnotations(ElementNameTable.Annotations, hcl.Annotations);
                this.WriteEndElement();
            }

            this.WriteEndElement();
        }

        /// <summary>
        /// Writes the hierarchy.
        /// </summary>
        /// <param name="hierarchy">
        /// The hierarchy.
        /// </param>
        private void WriteHierarchy(IHierarchyMutableObject hierarchy)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.Hierarchy);
            this.WriteIdentifiableArtefactAttributes(hierarchy);

            ////TryWriteAttribute(AttributeNameTable.version, hierarchy.Version);
            ////TryWriteAttribute(AttributeNameTable.validFrom, hierarchy.ValidFrom);
            ////TryWriteAttribute(AttributeNameTable.validTo, hierarchy.ValidTo);
            ////TryWriteAttribute(AttributeNameTable.isFinal, hierarchy.FinalStructure);
            this.WriteIdentifiableArtefactContent(hierarchy);
            foreach (ICodeRefMutableObject codeRef in hierarchy.HierarchicalCodeObjects)
            {
                this.TraverseCodeRefTree(codeRef);
            }

            ILevelMutableObject current = hierarchy.ChildLevel;
            int order = 1;
            while (current != null)
            {
                this.WriteLevel(current, order++);
                current = current.ChildLevel;
            }

            this.WriteAnnotations(ElementNameTable.Annotations, hierarchy.Annotations);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write level.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="order">
        /// The level order
        /// </param>
        private void WriteLevel(ILevelMutableObject level, int order)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.Level);
            this.WriteIdentifiableArtefactAttributes(level);
            this.WriteIdentifiableArtefactContent(level);
            this.WriteElement(this.DefaultNS, ElementNameTable.Order, order);
            if (level.CodingFormat != null)
            {
                this.WriteTextFormat(ElementNameTable.CodingType, level.CodingFormat);
            }

            this.WriteAnnotations(ElementNameTable.Annotations, level.Annotations);
            this.WriteEndElement();
        }

        #endregion
    }
}
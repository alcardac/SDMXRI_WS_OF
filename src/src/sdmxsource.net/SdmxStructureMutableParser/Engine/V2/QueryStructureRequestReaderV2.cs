// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure request reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The query structure request reader v 2.
    /// </summary>
    internal class QueryStructureRequestReaderV2 : RegistryInterfaceReaderBaseV2
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public QueryStructureRequestReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read contents from <paramref name="reader"/> to <paramref name="registry"/>.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="registry">
        /// The registry.
        /// </param>
        protected override void ReadContents(XmlReader reader, IRegistryInfo registry)
        {
            base.ReadContents(reader, localName => HandleChildElements(registry.QueryStructureRequest, localName));
        }

        /// <summary>
        /// Handle QueryStructureRequest Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IQueryStructureRequestInfo object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IQueryStructureRequestInfo parent, object localName)
        {
            SdmxStructureEnumType sdmxStructure;

            if (NameTableCache.IsElement(localName, ElementNameTable.DataflowRef))
            {
                IDataflowReferenceInfo dataflowReferenceInfo = new DataflowReferenceInfo();
                parent.References.Add(dataflowReferenceInfo);
                return this.AddReferenceAction(dataflowReferenceInfo, this.HandleChildElements, this.HandleTextChildElement);
            }

            if (NameTableCache.IsElement(localName, ElementNameTable.MetadataflowRef))
            {
                sdmxStructure = SdmxStructureEnumType.MetadataFlow;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CodelistRef))
            {
                sdmxStructure = SdmxStructureEnumType.CodeList;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.HierarchicalCodelistRef))
            {
                sdmxStructure = SdmxStructureEnumType.HierarchicalCodelist;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CategorySchemeRef))
            {
                sdmxStructure = SdmxStructureEnumType.CategoryScheme;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyRef))
            {
                sdmxStructure = SdmxStructureEnumType.Dsd;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.MetadataStructureRef))
            {
                sdmxStructure = SdmxStructureEnumType.Msd;
            }
            else
            {
                return null;
            }

            IReferenceInfo referenceInfo = new ReferenceInfo(sdmxStructure);
            parent.References.Add(referenceInfo);
            return this.AddReferenceAction(referenceInfo, DoNothingComplex, this.HandleTextChildElement);
        }

        #endregion

        /////// <summary>
        ///////     The initialize type switch.
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add text only element handlers
        ////    this.AddHandleText<IReferenceInfo>(this.HandleTextChildElement);
        ////    this.AddHandleText<IContentConstraintMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<IReleaseCalendarMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<IList<string>>(this.HandleTextChildElement);
        ////    this.AddHandleText<IKeyValuesMutable>(this.HandleTextChildElement);
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<IQueryStructureRequestInfo>(HandleChildElements);
        ////    this.AddHandleElement<IContentConstraintMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICubeRegionMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<IDataflowReferenceInfo>(this.HandleChildElements);
        ////    this.AddHandleElement<IKeyValuesMutable>(HandleChildElements);
        ////}
    }
}
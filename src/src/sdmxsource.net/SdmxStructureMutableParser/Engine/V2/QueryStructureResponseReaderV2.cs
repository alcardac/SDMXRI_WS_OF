// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureResponseReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure response reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     The query structure response reader v 2.
    /// </summary>
    internal class QueryStructureResponseReaderV2 : RegistryInterfaceReaderBaseV2
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureResponseReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public QueryStructureResponseReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read contents.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="rootObject">
        /// The root object.
        /// </param>
        protected override void ReadContents(XmlReader reader, IRegistryInfo rootObject)
        {
            base.ReadContents(reader, o => this.HandleChildElements(this.RegistryInterface.QueryStructureResponse, o));
        }

        /// <summary>
        /// Handle QueryStructureResponse Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent QueryStructureResponseBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IQueryStructureResponseInfo parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.StatusMessage))
            {
                IStatusMessageInfo status = this.CreateStatusMessage();
                parent.StatusMessage = status;
                actions = this.AddSimpleAction(status, this.HandleTextChildElement);
            }
            else
            {
                if (parent.Structure == null)
                {
                    var structure = new MutableObjectsImpl();
                    parent.Structure = structure;
                }

                this.HandleTopLevelBase(parent.Structure, localName);
            }

            return actions;
        }

        #endregion

        /////// <summary>
        ///////     The initialize type switch.
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add text only element handlers
        ////    this.AddHandleText<IStatusMessageInfo>(this.HandleTextChildElement);

        ////    // add element handlers
        ////    this.AddHandleElement<IQueryStructureResponseInfo>(this.HandleChildElements);
        ////}
    }
}
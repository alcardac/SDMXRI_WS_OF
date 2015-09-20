// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInterfaceReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A SDMX-ML Registry Interface message reader
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Util.Exception;

    /// <summary>
    ///     A SDMX-ML Registry Interface message reader
    /// </summary>
    public class RegistryInterfaceReaderV2 : RegistryInterfaceReaderBaseV2
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegistryInterfaceReaderV2" /> class.
        /// </summary>
        public RegistryInterfaceReaderV2()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public RegistryInterfaceReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
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
        /// <paramref name="reader"/> is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="reader">
        /// The text reader opened against the stream for the SDMX-ML Query
        /// </param>
        /// <returns>
        /// The IRegistryInfo object
        /// </returns>
        public IRegistryInfo Read(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            // initialise fields
            var registryInfo = new RegistryInfo();
            this.Read(registryInfo, reader);

            return registryInfo;
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
            bool foundMessage = false;
            while (!foundMessage && reader.Read())
            {
                switch (reader.NodeType)
                {
                        // Start Element
                    case XmlNodeType.Element:
                        object localName = reader.LocalName;
                        if (NameTableCache.IsElement(localName, ElementNameTable.RegistryInterface)
                            && reader.NamespaceURI.Equals(this.Namespaces.Message.NS))
                        {
                            foundMessage = true;
                            this.RegistryInterface.Header = this.ReadHeader(reader);
                        }

                        break;
                }
            }

            if (!foundMessage)
            {
                return;
            }

            base.ReadContents(reader, o => this.HandleTopLevel(this.RegistryInterface, o));
        }

        // ///<summary>Handle Structure Child elements</summary>
        // /// <param name="parent">The parent IMutableObjects object</param>
        // /// <param name="localName">The name of the current xml element</param>
        // /// <param name="reader">The xml reader to be passed to Structure Reader</param>
        // /// <returns>The created SDMX model entity object corresponding to the current xml element or null if there is none</returns>
        // private object HandleStructureChildElements(Model.Structure.IMutableObjects parent,object localName,Dictionary<string,string> attributes,XmlReader reader) {
        // object current = null;
        // ReadStructure(parent, reader);
        // return current;
        // }

        /// <summary>
        /// Handles the Registry Interface top level elements
        ///     This includes
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="IRegistryInfo"/>
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// Always null
        /// </returns>
        private ElementActions HandleTopLevel(IRegistryInfo parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.QueryStructureRequest))
            {
                parent.QueryStructureRequest = new QueryStructureRequestInfo();
                parent.QueryStructureRequest.ResolveReferences = Helper.TrySetFromAttribute(
                    this.Attributes, 
                    AttributeNameTable.resolveReferences, 
                    parent.QueryStructureRequest.ResolveReferences);
                parent.QueryStructureRequest.ReturnDetails = Helper.TrySetFromAttribute(
                    this.Attributes, AttributeNameTable.returnDetails, parent.QueryStructureRequest.ReturnDetails);
                var requestReaderV2 = new QueryStructureRequestReaderV2(this.Namespaces);
                requestReaderV2.Read(parent, this.Reader);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.QueryStructureResponse))
            {
                parent.QueryStructureResponse = new QueryStructureResponseInfo();
                var responseReaderV2 = new QueryStructureResponseReaderV2(this.Namespaces);
                responseReaderV2.Read(parent, this.Reader);
            }

            return null;
        }

        #endregion

        /////// <summary>
        ///////     The initialize type switch.
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add text only element handlers
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<IRegistryInfo>(this.HandleTopLevel);
        ////}
    }
}
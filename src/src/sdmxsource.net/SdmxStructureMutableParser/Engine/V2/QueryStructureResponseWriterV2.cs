// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureResponseWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure response writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    /// <summary>
    ///     The query structure response writer v 2.
    /// </summary>
    internal class QueryStructureResponseWriterV2 : RegistryInterfaceWriterBaseV2, IRegistryInterfaceWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureResponseWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public QueryStructureResponseWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Write the specified <paramref name="registry"/>
        /// </summary>
        /// <param name="registry">
        /// The <see cref="IRegistryInfo"/> object
        /// </param>
        public void Write(IRegistryInfo registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (registry.QueryStructureResponse != null)
            {
                this.WriteQueryStructureResponse(registry.QueryStructureResponse);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write QueryStructureResponse
        /// </summary>
        /// <param name="response">
        /// The QueryStructureResponseBean to write
        /// </param>
        private void WriteQueryStructureResponse(IQueryStructureResponseInfo response)
        {
            this.WriteStartElement(ElementNameTable.QueryStructureResponse);

            this.WriteStatusMessage(response.StatusMessage);

            var structureWriter = new StructureWriterV2(this.SdmxMLWriter);
            structureWriter.SetTopElementsNS(this.Namespaces.Registry);
            structureWriter.WriteStructure(response.Structure, null);

            this.WriteEndElement(); // </QueryStructureResponse>
        }

        #endregion
    }
}
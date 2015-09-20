// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure request reader v 2.
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
    ///     The query structure request reader v 2.
    /// </summary>
    internal class QueryStructureRequestWriterV2 : RegistryInterfaceWriterBaseV2, IRegistryInterfaceWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public QueryStructureRequestWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
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

            if (registry.QueryStructureRequest != null)
            {
                this.WriteQueryStructureRequest(registry.QueryStructureRequest);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write query structure request
        /// </summary>
        /// <param name="request">
        /// THe <see cref="IQueryStructureRequestInfo"/> to write
        /// </param>
        private void WriteQueryStructureRequest(IQueryStructureRequestInfo request)
        {
            this.WriteStartElement(ElementNameTable.QueryStructureRequest);
            this.WriteAttribute(AttributeNameTable.resolveReferences, request.ResolveReferences);
            if (!request.ReturnDetails)
            {
                // defaults to true. It is an extension to SDMX.
                this.TryWriteAttribute(AttributeNameTable.returnDetails, request.ReturnDetails);
            }

            foreach (IReferenceInfo refBean in request.References)
            {
                this.WriteRef(refBean);
            }

            this.WriteEndElement(); // </QueryStructureRequest>
        }

        #endregion
    }
}
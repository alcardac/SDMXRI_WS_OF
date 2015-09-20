// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxNamespaces.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The set of namespaces used to generate the xml SDMX messages
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Model
{
    /// <summary>
    ///     The set of namespaces used to generate the xml SDMX messages
    /// </summary>
    public class SdmxNamespaces
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the URL of the common namespace
        /// </summary>
        public NamespacePrefixPair Common { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the compact namespace
        /// </summary>
        public NamespacePrefixPair Compact { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the cross namespace
        /// </summary>
        public NamespacePrefixPair Cross { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the generic namespace
        /// </summary>
        public NamespacePrefixPair Generic { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the message namespace
        /// </summary>
        public NamespacePrefixPair Message { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the query namespace
        /// </summary>
        public NamespacePrefixPair Query { get; set; }

        /// <summary>
        ///     Gets or sets the  URL of the registry message
        /// </summary>
        public NamespacePrefixPair Registry { get; set; }

        /// <summary>
        ///     Gets or sets the schema location
        /// </summary>
        public string SchemaLocation { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the structure namespace
        /// </summary>
        public NamespacePrefixPair Structure { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the structure specific dataset namespace. Note this depends on the <c>DSD</c> and applies to all SDMX versions.
        /// </summary>
        public NamespacePrefixPair DataSetStructureSpecific { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the structure specific dataset namespace. Note this about <c>http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific</c>
        /// </summary>
        public NamespacePrefixPair StructureSpecific21 { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the ESTAT URN format should be used.
        /// </summary>
        public bool UseEstatUrn { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the utility namespace
        /// </summary>
        public NamespacePrefixPair Utility { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the XSI namespace
        /// </summary>
        public NamespacePrefixPair Xsi { get; set; }

        #endregion
    }
}
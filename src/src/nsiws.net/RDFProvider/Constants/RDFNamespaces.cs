// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RDFNamespaces.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The set of namespaces used to generate the xml RDF messages
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Constants
{
    /// <summary>
    ///     The set of namespaces used to generate the xml RDF messages
    /// </summary>
    public class RDFNamespaces
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the schema location
        /// </summary>
        public string SchemaLocation { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the XSI namespace
        /// </summary>
        public NamespacePrefixPair Xsi { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the rdf namespace
        /// </summary>
        public NamespacePrefixPair RDF { get; set; }

        public NamespacePrefixPair SdmxConcept { get; set; }

        public NamespacePrefixPair Property { get; set; }

        public NamespacePrefixPair QB { get; set; }

        public NamespacePrefixPair DocTerms { get; set; }

        public NamespacePrefixPair RDFs { get; set; }

        public NamespacePrefixPair Owl { get; set; }

        public NamespacePrefixPair Skos { get; set; }

        public NamespacePrefixPair XML { get; set; }

        public NamespacePrefixPair Xkos { get; set; }

        public NamespacePrefixPair Prov { get; set; }



        #endregion
    }
}
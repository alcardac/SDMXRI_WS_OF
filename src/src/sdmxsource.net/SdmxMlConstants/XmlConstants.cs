// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlConstants.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class contains XML Standard constants
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxXmlConstants
{
    /// <summary>
    ///     This class contains XML Standard constants
    /// </summary>
    public static class XmlConstants
    {
        #region Constants

        /// <summary>
        ///     The lang attribute
        /// </summary>
        /// <remarks>Designed for identifying the human language used in the scope of the element to which it's attached.</remarks>
        public const string LangAttribute = "lang";

        /// <summary>
        ///     XML Not A Number constant
        /// </summary>
        public const string NaN = "NaN";

        /// <summary>
        ///     The XML Schema schemaLocation
        /// </summary>
        public const string SchemaLocation = "schemaLocation";

        /// <summary>
        ///     The xml prefix
        /// </summary>
        /// <remarks>Namespace http://www.w3.org/XML/1998/namespace is bound by definition to the prefix xml:</remarks>
        public const string XmlPrefix = "xml";

        /// <summary>
        ///     The W3.org Schema XSD 1.0 namespace: http://www.w3.org/2001/XMLSchema-instance
        /// </summary>
        public const string XmlSchemaNS = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        ///     The W3.org Schema XSD 1.0 prefix: <c>xsi</c>
        /// </summary>
        public const string XmlSchemaPrefix = "xsi";

        /// <summary>
        ///     The attribute for setting XML namespaces
        /// </summary>
        public const string Xmlns = "xmlns";

        #endregion
    }
}
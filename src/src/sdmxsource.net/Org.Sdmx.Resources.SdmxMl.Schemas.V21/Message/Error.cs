namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System.Xml;

    using Xml.Schema.Linq;

    /// <summary>
    /// <para>
    /// ErrorType describes the structure of an error response.
    /// </para>
    /// </summary>
    public partial class Error
    {
        /// <summary>
        /// Load error message to a LINQ2XSD <see cref="Error"/> object
        /// </summary>
        /// <param name="xmlFile">The input file</param>
        /// <returns>a LINQ2XSD <see cref="Error"/> object</returns>
        public static Error Load(XmlReader xmlFile)
        {
            return XTypedServices.Load<Error, ErrorType>(xmlFile, LinqToXsdTypeManager.Instance);
        }
    }
}
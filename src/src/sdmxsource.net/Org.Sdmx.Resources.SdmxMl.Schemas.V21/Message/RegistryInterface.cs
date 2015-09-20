namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System.Xml;

    using Xml.Schema.Linq;

    public partial class RegistryInterface
    {
        /// <summary>
        /// Load error message to a LINQ2XSD <see cref="RegistryInterface"/> object
        /// </summary>
        /// <param name="xmlFile">The input file</param>
        /// <returns>a LINQ2XSD <see cref="RegistryInterface"/> object</returns>
        public static RegistryInterface Load(XmlReader xmlFile)
        {
            return XTypedServices.Load<RegistryInterface, RegistryInterfaceType>(xmlFile, LinqToXsdTypeManager.Instance);
        }
    }
}
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V20.message
{

    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.compact;

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
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V20.message
{
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.compact;

    using Xml.Schema.Linq;

    public partial class Structure
    {
        /// <summary>
        /// Load error message to a LINQ2XSD <see cref="Structure"/> object
        /// </summary>
        /// <param name="xmlFile">The input file</param>
        /// <returns>a LINQ2XSD <see cref="Structure"/> object</returns>
        public static Structure Load(XmlReader xmlFile)
        {
            return XTypedServices.Load<Structure, StructureType>(xmlFile, LinqToXsdTypeManager.Instance);
        } 
    }
}
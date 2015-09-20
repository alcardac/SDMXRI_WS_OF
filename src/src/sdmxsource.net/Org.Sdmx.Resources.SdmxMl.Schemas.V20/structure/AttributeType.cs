// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeType.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure
{
    using System.Xml.Linq;
    using System.Xml.Schema;

    using Xml.Schema.Linq;

    /// <summary>
    /// The Attribute type.
    /// </summary>
    public partial class AttributeType
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the concept scheme version (ESTAT extension)
        ///     <para>
        ///         Occurrence: optional
        ///     </para>
        /// </summary>
        public string ConceptSchemeVersionEstat
        {
            get
            {
                XAttribute x = this.Attribute(XName.Get("conceptSchemeVersion", string.Empty));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }

            set
            {
                this.SetAttribute(XName.Get("conceptSchemeVersion", string.Empty), value, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
        }

        #endregion
    }
}
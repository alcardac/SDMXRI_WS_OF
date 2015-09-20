// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Structure.cs" company="EUROSTAT">
//   EUPL
// </copyright>
// <summary>
//   The structure.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;

    using Xml.Schema.Linq;

    /// <summary>
    /// The structure.
    /// </summary>
    public partial class Structure
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the footer.
        /// </summary>
        public Footer.Footer Footer
        {
            get
            {
                return this.ContentField.Footer;
            }

            set
            {
                this.ContentField.Footer = value;
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public StructureHeaderType Header
        {
            get
            {
                return this.ContentField.Header;
            }

            set
            {
                this.ContentField.Header = value;
            }
        }

        /// <summary>
        /// Gets or sets the structures.
        /// </summary>
        public StructuresType Structures
        {
            get
            {
                return this.ContentField.Structures;
            }

            set
            {
                this.ContentField.Structures = value;
            }
        }

        #endregion

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
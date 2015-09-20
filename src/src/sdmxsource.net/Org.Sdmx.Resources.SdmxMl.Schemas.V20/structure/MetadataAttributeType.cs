// -----------------------------------------------------------------------
// <copyright file="MetadataAttributeType.cs" company="EUROSTAT">
//   Date Created : 2014-12-16
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure
{
    using System.Xml.Linq;
    using System.Xml.Schema;

    using Xml.Schema.Linq;

    public partial class MetadataAttributeType
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the concept scheme version (ESTAT extension)
        ///     <para>
        ///         Occurrence: optional
        ///     </para>
        /// </summary>
        public string RepresentationSchemeVersionEstat
        {
            get
            {
                XAttribute x = this.Attribute(XName.Get("representationSchemeVersion", string.Empty));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }

            set
            {
                this.SetAttribute(XName.Get("representationSchemeVersion", string.Empty), value, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
        }

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
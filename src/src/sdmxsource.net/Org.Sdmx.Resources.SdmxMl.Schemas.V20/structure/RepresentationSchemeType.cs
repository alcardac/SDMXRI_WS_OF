// -----------------------------------------------------------------------
// <copyright file="RepresentationSchemeType.cs" company="EUROSTAT">
//   Date Created : 2014-12-15
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

    /// <summary>
    /// The representation scheme type.
    /// </summary>
    public partial class RepresentationSchemeType
    {
        /// <summary>
        /// Gets or sets the representation scheme version. This is an ESTAT extension.
        /// </summary>
        /// <value>
        /// The representation scheme version. This is an ESTAT extension.
        /// </value>
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
    }
}
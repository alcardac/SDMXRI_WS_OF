// -----------------------------------------------------------------------
// <copyright file="ConceptType.cs" company="EUROSTAT">
//   Date Created : 2014-12-19
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

    public partial class ConceptType
    {
        /// <summary>
        ///     Gets or sets the core representation version (ESTAT extension)
        ///     <para>
        ///         Occurrence: optional
        ///     </para>
        /// </summary>
        public string CoreRepresentationVersionEstat
        {
            get
            {
                XAttribute x = this.Attribute(XName.Get("coreRepresentationVersion", string.Empty));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }

            set
            {
                this.SetAttribute(XName.Get("coreRepresentationVersion", string.Empty), value, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
        } 
    }
}
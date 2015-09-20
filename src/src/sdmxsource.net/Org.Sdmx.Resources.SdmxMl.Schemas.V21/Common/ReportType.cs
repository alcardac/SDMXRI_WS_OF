// -----------------------------------------------------------------------
// <copyright file="ReportType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Linq;
    using Xml.Schema.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class MetadataTargetRegionType
    {
        public string Report
        {
            get
            {
                XElement x = this.GetElement(XName.Get("report", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common"));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("report", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common"), value, "report", global::Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.IDType.TypeDefinition);
            }
        }

        public string MetadataTarget
        {
            get
            {
                XElement x = this.GetElement(XName.Get("metadataTarget", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common"));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("metadataTarget", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common"), value, "metadataTarget", global::Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.IDType.TypeDefinition);
            }
        }
    }
}

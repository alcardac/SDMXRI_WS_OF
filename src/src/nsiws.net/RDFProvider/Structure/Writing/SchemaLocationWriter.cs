using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Xml.Schema.Linq;

//using Org.Sdmxsource.Util.Xml;
using System.Xml;
using System.Xml.Linq;

namespace RDFProvider.Structure.Writing
{
    public class SchemaLocationWriter
    {
        private static readonly Dictionary<SdmxSchema, string> schemaLocationMap = new Dictionary<SdmxSchema, string>();

        /**
         * Writes the schema location(s) to the XML Document
         * @param doc the XML document to write the schema location(s) to
         * @param schemaVersion the version of the schema to write in
         * @param namespaceUri the URI of the namespace to write
         */
        public void WriteSchemaLocation(XTypedElement doc, string[] namespaceUri)
        {
            if (namespaceUri == null)
                return;

            SdmxSchema schemaVersion;
            StringBuilder schemaLocation = new StringBuilder();

            string concat = "";
            foreach (string currentNamespaceUri in namespaceUri)
            {
                schemaVersion = SdmxConstants.GetSchemaVersion(currentNamespaceUri);
                //Base location of schema for version e.g. http://www.sss.sss/schema/
                string schemaBaseLocation = GetSchemaLocation(schemaVersion);
                string schemaName = SdmxConstants.GetSchemaName(currentNamespaceUri);
                schemaLocation.Append(concat + currentNamespaceUri + " " + concat + schemaBaseLocation + schemaName);
                concat = "\r\n";// System.getProperty("line.separator");
            }

            doc.Untyped.SetAttributeValue(XName.Get("http://www.w3.org/2001/XMLSchema-instance", "schemaLocation"), schemaLocation.ToString());
        }

        public string GetSchemaLocation(SdmxSchema schemaVersion)
        {
            return schemaLocationMap[schemaVersion];
        }
    }
}
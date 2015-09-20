using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xml.Schema.Linq;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Exception;
using Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing;

/**
 * Adds schema location to response, if there is a schema location
 *
 */
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization
{
    public abstract class XmlObjectBuilder
    {
        #region Fields

        private readonly SchemaLocationWriter schemaLocationWriter = new SchemaLocationWriter();

        #endregion

	
	    #region  Public Methods and Operators

	    protected void WriteSchemaLocation(XTypedElement doc, SdmxSchemaEnumType schemaVersion) 
        {
		    if (schemaLocationWriter != null)
            {
                List<string> schemaUri = new List<string>();
			    switch(schemaVersion) 
                {
			        case SdmxSchemaEnumType.VersionOne :  
                        schemaUri.Add(SdmxConstants.MessageNs10);
			            break;
			        case SdmxSchemaEnumType.VersionTwo:  
                        schemaUri.Add(SdmxConstants.MessageNs20);
			            break;
			        case SdmxSchemaEnumType.VersionTwoPointOne :  
                        schemaUri.Add(SdmxConstants.MessageNs21);
			            break;
			        default : 
                        throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Schema Version " + schemaVersion);
			    }
			    schemaLocationWriter.WriteSchemaLocation(doc, schemaUri.ToArray());
		    }
	    }
    }
    #endregion
}
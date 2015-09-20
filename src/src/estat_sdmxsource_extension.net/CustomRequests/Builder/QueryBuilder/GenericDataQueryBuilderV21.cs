// -----------------------------------------------------------------------
// <copyright file="GenericDataQueryBuilderV21.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using DataQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataQueryType;
    using GenericDataQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.GenericDataQueryType;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GenericDataQueryBuilderV21 : IComplexDataQueryBuilder
    {
        public XDocument BuildComplexDataQuery(IComplexDataQuery query)
        {
                
            var queryMessageType = new GenericDataQueryType();
            
            queryMessageType.Header = new BasicHeaderType();
            V21Helper.SetHeader(queryMessageType.Header, null);
            var queryType = new DataQueryType();
            queryMessageType.BaseDataQueryType = queryType;

            var coreBuilder = new ComplexDataQueryCoreBuilderV21();
            coreBuilder.FillDataQueryType(queryType, query);
            
            var queryMessageDocument = new GenericDataQuery(queryMessageType);
            var xDoc = new XDocument(queryMessageDocument.Untyped);

            return xDoc;
        }
    }
}

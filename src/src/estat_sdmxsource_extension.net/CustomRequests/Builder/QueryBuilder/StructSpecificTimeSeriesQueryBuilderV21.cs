// -----------------------------------------------------------------------
// <copyright file="StructSpecificTimeSeriesQueryBuilderV21.cs" company="Microsoft">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StructSpecificTimeSeriesQueryBuilderV21 : IComplexDataQueryBuilder
    {
        public XDocument BuildComplexDataQuery(IComplexDataQuery query)
        {
            
            var timeSeriesQueryDataType = new StructureSpecificTimeSeriesDataQueryType();

            timeSeriesQueryDataType.Header = new BasicHeaderType();

            V21Helper.SetHeader(timeSeriesQueryDataType.Header, null);

            var queryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataQueryType();
           

            var coreBuilder = new ComplexDataQueryCoreBuilderV21();
            coreBuilder.FillDataQueryType(queryType, query);
           
            timeSeriesQueryDataType.BaseDataQueryType = queryType;
            
            var queryMessageDocument = new StructureSpecificTimeSeriesDataQuery(timeSeriesQueryDataType);

            return new XDocument(queryMessageDocument.Untyped);
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="GenericTimeSeriesDataQueryBuilderV21.cs" company="Microsoft">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using DataQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataQueryType;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GenericTimeSeriesDataQueryBuilderV21 : IComplexDataQueryBuilder
    {
        public XDocument BuildComplexDataQuery(IComplexDataQuery query)
        {
            var queryMessageDocument = new GenericTimeSeriesDataQuery(new GenericTimeSeriesDataQueryType());

            queryMessageDocument.Content.Header = new BasicHeaderType();
            V21Helper.SetHeader(queryMessageDocument.Content.Header, null);

            var queryType = new DataQueryType();
            var coreBuilder = new ComplexDataQueryCoreBuilderV21();
            coreBuilder.FillDataQueryType(queryType, query);
            queryMessageDocument.Content.BaseDataQueryType = queryType;
            
            var xDocument = new XDocument(queryMessageDocument.Untyped);

            return xDocument;

        }
    }
}

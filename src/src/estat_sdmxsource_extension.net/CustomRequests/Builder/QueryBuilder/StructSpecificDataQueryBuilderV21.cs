// -----------------------------------------------------------------------
// <copyright file="StructSpecificDataQueryBuilderV21.cs" company="Microsoft">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using DataQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.DataQueryType;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StructSpecificDataQueryBuilderV21 : IComplexDataQueryBuilder
    {

        public XDocument BuildComplexDataQuery(IComplexDataQuery query)
        {
            var queryMessageType = new DataQueryType();

            queryMessageType.Header = new BasicHeaderType();
            V21Helper.SetHeader(queryMessageType.Header, null);

            var queryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataQueryType();

            var coreBuilder = new ComplexDataQueryCoreBuilderV21();
            coreBuilder.FillDataQueryType(queryType, query);
            queryMessageType.BaseDataQueryType = queryType;
            var queryMessageDocument = new StructureSpecificDataQuery(queryMessageType);

            return new XDocument(queryMessageDocument.Untyped);
        }
    }
}

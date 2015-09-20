// -----------------------------------------------------------------------
// <copyright file="ComplexDataQueryBuilderManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.CustomRequests.Factory;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ComplexDataQueryBuilderManager : IComplexDataQueryBuilderManager
    {
        private readonly IComplexDataQueryFactory _factory;
        /// <summary>
        /// 
        /// </summary>
        public ComplexDataQueryBuilderManager(IComplexDataQueryFactory factory)
        {
            _factory = factory;
        }

        public XDocument BuildComplexDataQuery(IComplexDataQuery complexDataQuery, IDataQueryFormat<XDocument> dataQueryFormat)
        {
            IComplexDataQueryBuilder builder = _factory.GetComplexDataQueryBuilder(dataQueryFormat);
            if (builder != null)
            {
                return builder.BuildComplexDataQuery(complexDataQuery);
            }
            throw new SdmxUnauthorisedException("Unsupported ComplexDataQueryFormat: " + dataQueryFormat);
        }
    }
}

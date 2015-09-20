// -----------------------------------------------------------------------
// <copyright file="IComplexDataQueryFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A ComplexDataQueryFactory is responsible fore creating a DataQueryBuilder
    /// that build a query that can be used for services external to the SdmxSource framework
    /// </summary>
    public interface IComplexDataQueryFactory
    {
        /// <summary>
        /// Returns a ComplexDataQueryBuilder only if this factory understands the DataQueryFormat.
        /// If the format is unknown, null will be returned
        /// </summary>
        /// <param name="format"></param>
        /// <returns>IComplexDataQueryBuilder is this factory knows how to build this query format, or null if it doesn't</returns>
        IComplexDataQueryBuilder GetComplexDataQueryBuilder(IDataQueryFormat<XDocument> format);
         
    }
}

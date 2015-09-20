// -----------------------------------------------------------------------
// <copyright file="IComplexDataQueryBuilder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    /// Responsible for building a Complex Data Query
    /// that can be used to query services external to the SdmxSource framework 
    /// </summary>
    public interface IComplexDataQueryBuilder
    {
        /// <summary>
        /// Builds a ComplexDataQuery that matches the passed in format
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        XDocument BuildComplexDataQuery(IComplexDataQuery query);
    }
}

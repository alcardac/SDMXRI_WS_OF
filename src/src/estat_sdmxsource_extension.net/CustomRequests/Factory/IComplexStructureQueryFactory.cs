// -----------------------------------------------------------------------
// <copyright file="IComplexStructureQueryFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Estat.Sri.CustomRequests.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A ComplexStructureQueryFactory is responsible fore creating a ComplexStructureQueryBuilder
    /// that build a query that can be used for services external to the SdmxSource framework
    /// </summary>
    public interface IComplexStructureQueryFactory<T>
    {
        /// <summary>
        /// Returns a ComplexStructureQueryBuilder only if this factory understands the StructureQueryFormat.
        /// If the format is unknown, null will be returned
        /// </summary>
        /// <param name="format">format</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>ComplexStructureQueryBuilder is this factory knows how to build this query format, or null if it doesn't</returns>
        IComplexStructureQueryBuilder<T> GetComplexStructureQueryBuilder(IStructureQueryFormat<T> format);
    }
}

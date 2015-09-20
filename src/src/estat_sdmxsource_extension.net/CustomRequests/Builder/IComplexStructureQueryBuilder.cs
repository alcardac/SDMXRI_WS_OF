// -----------------------------------------------------------------------
// <copyright file="IComplexStructureQueryBuilder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// Responsible for building a Complex Structure Query
    /// that can be used to query services external to the SdmxSource framework
    /// </summary>
    public interface IComplexStructureQueryBuilder<T>
    {
        /// <summary>
        /// Builds a ComplexStructureQuery that matches the passed in format
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        T BuildComplexStructureQuery(IComplexStructureQuery query);

    }
}

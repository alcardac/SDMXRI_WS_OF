// -----------------------------------------------------------------------
// <copyright file="IComplexStructureQueryBuilderManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// Builds a StructureQuery in the required format 
    /// </summary>
    public interface IComplexStructureQueryBuilderManager<T>
    {
        /// <summary>
        /// Builds a complex structure query in the requested format
        /// </summary>
        /// <param name="complexStructureQuery">complexStructureQuery the query to build a representation of</param>
        /// <param name="structureQueryFormat">the required format</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Representation of query in the desired format.</returns>
        T BuildComplexStructureQuery(IComplexStructureQuery complexStructureQuery, IStructureQueryFormat<T> structureQueryFormat);
    }
}

// -----------------------------------------------------------------------
// <copyright file="IComplexDataQueryBuilderManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Manager
{
    using System.Xml.Linq;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// Builds a DataQuery in the required format
    /// </summary>
    public interface IComplexDataQueryBuilderManager
    {
        /// <summary>
        /// Builds a complex data query in the requested format
        /// </summary>
        /// <param name="complexDataQuery">The query to build a representation of</param>
        /// <param name="dataQueryFormat">The required format</param>
        /// <returns>Query in the desired format</returns>
        XDocument BuildComplexDataQuery(IComplexDataQuery complexDataQuery, IDataQueryFormat<XDocument> dataQueryFormat);
    }
}

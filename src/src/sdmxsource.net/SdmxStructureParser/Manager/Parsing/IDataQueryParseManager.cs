// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryParseManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The DataQueryParseManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    ///     The DataQueryParseManager interface.
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IDataQueryParseManager" />
    ///     <code source="..\ReUsingExamples\DataQuery\ReUsingDataQueryParsingManager.cs" lang="cs" />
    /// </example>
    public interface IDataQueryParseManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a <see cref="IDataQuery"/> list from a message that contains one or more data queries
        /// </summary>
        /// <param name="dataLocation">
        /// The data location
        /// </param>
        /// <param name="beanRetrievalManager">
        /// optional, if given will retrieve the key family bean the query is for
        /// </param>
        /// <returns>
        /// a <see cref="IDataQuery"/> list
        /// </returns>
        IList<IDataQuery> BuildDataQuery(
            IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager beanRetrievalManager);

        /// <summary>
        /// Parse the specified <paramref name="query"/>.
        /// </summary>
        /// <param name="query">
        /// The REST data query.
        /// </param>
        /// <param name="beanRetrievalManager">
        /// The <c>SDMX</c> object retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IDataQuery"/> from <paramref name="query"/>.
        /// </returns>
        IDataQuery ParseRestQuery(string query, ISdmxObjectRetrievalManager beanRetrievalManager);

        /**
	     * Builds Complex Data Queries for 2.1 data query messages
	     * @param dataLocation
	     * @param beanRetrievalManager, if given will retrieve the data structure bean the query is for
	     * @return
	     */
        IList<IComplexDataQuery> BuildComplexDataQuery(IReadableDataLocation dataLocation, ISdmxObjectRetrievalManager beanRetrievalManager);


        #endregion
    }
}
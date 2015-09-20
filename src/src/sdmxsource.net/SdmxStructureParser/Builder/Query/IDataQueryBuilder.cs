// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The DataQueryBuilder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System.Collections.Generic;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.query;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using DataWhereType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.query.DataWhereType;

    /// <summary>
    ///     The DataQueryBuilder interface.
    /// </summary>
    public interface IDataQueryBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a data query from a <c>dataWhereType</c>
        /// </summary>
        /// <param name="queryType">
        /// - the IXML to build the domain object (DataQuery) from
        /// </param>
        /// <param name="structureRetrievalManager">
        /// optional, if supplied the retrieval manager to use to retrieve the Dataflow and Provider defined by the
        ///     <paramref name="queryType"/>
        /// </param>
        /// <returns>
        /// a data query from a <c>dataWhereType</c>
        /// </returns>
        IList<IDataQuery> BuildDataQuery(QueryType queryType, ISdmxObjectRetrievalManager structureRetrievalManager);

        /// <summary>
        /// Builds a data query from a <c>dataWhereType</c>
        /// </summary>
        /// <param name="queryType">
        /// - the IXML to build the domain object (DataQuery) from
        /// </param>
        /// <param name="structureRetrievalManager">
        /// optional, if supplied the retrieval manager to use to retrieve the Dataflow and Provider defined by the
        ///     <paramref name="queryType"/>
        /// </param>
        /// <returns>
        /// a data query from a <c>dataWhereType</c>
        /// </returns>
        IList<IDataQuery> BuildDataQuery(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.query.QueryType queryType,
            ISdmxObjectRetrievalManager structureRetrievalManager);

        /**
         * Builds a data query from a dataQueryType
         * @param dataQueryType - the XMLBean to build the domain object (DataQuery) from
         * @param structureRetrievalManager optional, if supplied the retrieval manager to use to retrieve the DSD, which is stored on the query,
         * if not supplied then the DSD will not be stored on the query
         * @return
         */
        IList<IComplexDataQuery> BuildComplexDataQuery(DataQueryType dataQueryType, ISdmxObjectRetrievalManager beanRetrievalManager);


        /// <summary>
        /// Builds a data query from a <c>dataWhereType</c>
        /// </summary>
        /// <param name="dataWhereType">
        /// - the IXML to build the domain object (DataQuery) from
        /// </param>
        /// <param name="structureRetrievalManager">
        /// optional, if supplied the retrieval manager to use to retrieve the Dataflow and Provider defined by the
        ///     <paramref name="dataWhereType"/>
        /// </param>
        /// <returns>
        /// a data query from a <c>dataWhereType</c>
        /// </returns>
        IDataQuery BuildDataQuery(DataWhereType dataWhereType, ISdmxObjectRetrievalManager structureRetrievalManager);

        /// <summary>
        /// Builds  a data query from a <c>dataWhereType</c>
        /// </summary>
        /// <param name="dataWhereType">
        /// - the IXML to build the domain object (DataQuery) from
        /// </param>
        /// <param name="structureRetrievalManager">
        /// optional, if supplied the retrieval manager to use to retrieve the Dataflow and Provider defined by the
        ///     <paramref name="dataWhereType"/>
        /// </param>
        /// <returns>
        /// a data query from a <c>dataWhereType</c>
        /// </returns>
        IDataQuery BuildDataQuery(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.query.DataWhereType dataWhereType,
            ISdmxObjectRetrievalManager structureRetrievalManager);

        #endregion
    }
}
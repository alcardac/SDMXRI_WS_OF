// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query builder implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.query;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    ///     The data query builder implementation
    /// </summary>
    public class DataQueryBuilder : IDataQueryBuilder
    {
        #region Fields

        /// <summary>
        ///     The data query builder v 2.
        /// </summary>
        private readonly DataQueryBuilderV2 _dataQueryBuilderV2 = new DataQueryBuilderV2();

        private readonly DataQueryBuilderV21 _dataQueryBuilderV21 = new DataQueryBuilderV21();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds  a data query from a <c>dataWhereType</c>. Not implemented
        /// </summary>
        /// <param name="dataWhereType">
        /// - the IXML to build the domain object (DataQuery) from
        /// </param>
        /// <param name="structureRetrievalManager">
        /// optional, if supplied the retrieval manager to use to retrieve the Dataflow and Provider defined by the
        ///     <paramref name="dataWhereType"/>
        /// </param>
        /// <exception cref="NotImplementedException">
        /// DataQueryBuilder.buildDataQuery (Version 1.0 SDMX)
        /// </exception>
        /// <returns>
        /// a data query from a <c>dataWhereType</c>
        /// </returns>
        public virtual IDataQuery BuildDataQuery(
            DataWhereType dataWhereType, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            throw new SdmxNotImplementedException("DataQueryBuilder.buildDataQuery (Version 1.0 SDMX)");

            //// FUNC Support 1.0 data query
            ////throw new UnsupportedException(
            ////    ExceptionCode.Unsupported, "DataQueryBuilder.buildDataQuery (Version 1.0 SDMX)");
        }

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
        public virtual IList<IDataQuery> BuildDataQuery(
            QueryType queryType, ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            throw new SdmxNotImplementedException("DataQueryBuilder.buildDataQuery (Version 1.0 SDMX)");

            ////throw new UnsupportedException(
            ////    ExceptionCode.Unsupported, "DataQueryBuilder.buildDataQuery (Version 1.0 SDMX)");
        }

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
        public virtual IDataQuery BuildDataQuery(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.query.DataWhereType dataWhereType,
            ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            return this._dataQueryBuilderV2.BuildDataQuery(dataWhereType, structureRetrievalManager);
        }

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
        public virtual IList<IDataQuery> BuildDataQuery(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.query.QueryType queryType,
            ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            return this._dataQueryBuilderV2.BuildDataQuery(queryType, structureRetrievalManager);
        }

        public virtual IList<IComplexDataQuery> BuildComplexDataQuery(
            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataQueryType dataQueryType,
            ISdmxObjectRetrievalManager structureRetrievalManager)
        {
            return this._dataQueryBuilderV21.BuildComplexDataQuery(dataQueryType, structureRetrievalManager);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryBuilderManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query builder manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Manager
{
    #region Using Directives

    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public class DataQueryBuilderManager : IDataQueryBuilderManager
    {
        #region Fields

        private readonly IDataQueryFactory[] _dataDataQueryFactory;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Set the data query factory
        /// </summary>
        /// <param name="dataQueryFactory">
        /// The data query factory
        /// </param>
        public DataQueryBuilderManager(params IDataQueryFactory[] dataQueryFactory)
        {
            this._dataDataQueryFactory = ObjectUtil.ValidArray(dataQueryFactory) ? dataQueryFactory : new IDataQueryFactory[] { new DataQueryFactory(), };
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Builds a data query in the requested format
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <param name="dataQuery">
        /// The query to build a representation of
        /// </param>
        /// <param name="dataQueryFormat">
        /// The required format
        /// </param>
        /// <returns>
        /// Representation of query in the desired format.
        /// </returns>
        public T BuildDataQuery<T>(IDataQuery dataQuery, IDataQueryFormat<T> dataQueryFormat)
        {
            foreach (var dataQueryFactory in this._dataDataQueryFactory)
            {
                var builder = dataQueryFactory.GetDataQueryBuilder(dataQueryFormat);
                if (builder != null)
                {
                    return builder.BuildDataQuery(dataQuery);
                }
            }

            return default(T);
        }

        #endregion

    }
}

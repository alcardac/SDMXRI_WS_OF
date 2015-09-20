// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureQueryBuilderManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure query builder manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Manager
{
    #region Using Directives

    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    /// Checks each StructureQueryFactory registered to the Spring beans framework asking each one in turn to
    /// obtain a query builder.  The StructureQueryFactory to respond with a not null value, will be returned.
    /// </summary>
    public class StructureQueryBuilderManager : IStructureQueryBuilderManager
    {
        #region Fields

        /// <summary>
        /// The _structure query factories
        /// </summary>
        private readonly IStructureQueryFactory[] _structureQueryFactories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureQueryBuilderManager"/> class.
        /// </summary>
        /// <param name="structureQueryFactories">The structure query factory.</param>
        public StructureQueryBuilderManager(params IStructureQueryFactory[] structureQueryFactories)
        {
            this._structureQueryFactories = ObjectUtil.ValidArray(structureQueryFactories) ? structureQueryFactories : new IStructureQueryFactory[] { new RestStructureQueryFactory() };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds a structure query in the requested format
        /// </summary>
        /// <param name="structureQuery">
        /// The query to build a representation of
        /// </param>
        /// <param name="structureQueryFormat">
        /// The required format
        /// </param>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <returns>
        /// Representation of query in the desired format.
        /// </returns>
        public T BuildStructureQuery<T>(IRestStructureQuery structureQuery, IStructureQueryFormat<T> structureQueryFormat)
        {
            foreach (var structureQueryFactory in this._structureQueryFactories)
            {
                var builder = structureQueryFactory.GetStructureQueryBuilder(structureQueryFormat);
                if (builder != null)
                {
                    return builder.BuildStructureQuery(structureQuery);
                }
            }
            
            return default(T);
        }

        #endregion
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestStructureQueryFactory.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Reststructure query factory implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory
{

    #region Using Directives

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Builder;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Model;

    #endregion

    /// <summary>
    /// If the required format is RestQueryFormat, then will returns a builder that can build REST
    /// </summary>
    public class RestStructureQueryFactory : IStructureQueryFactory
    {
        #region Fields

        private readonly StructureQueryBuilderRest _structureQueryBuilderRest = new StructureQueryBuilderRest();

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Returns a StructureQueryBuilder only if this factory understands the StructureQueryFormat.  If the format is unknown, null will be returned
        /// </summary>
        /// <typeparam name="T">generic type parameter</typeparam>
        /// <param name="format">Format</param>
        /// <returns>StructureQueryBuilder is this factory knows how to build this query format, or null if it doesn't</returns>
        public IStructureQueryBuilder<T> GetStructureQueryBuilder<T>(IStructureQueryFormat<T> format)
        {
		    if(format is RestQueryFormat) {
			    return (IStructureQueryBuilder<T>)_structureQueryBuilderRest;
		    }
		    return null;
        }

        #endregion
    }
}

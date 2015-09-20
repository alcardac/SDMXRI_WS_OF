// -----------------------------------------------------------------------
// <copyright file="ComplexStructureQueryBuilderManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Manager
{
    using Estat.Sri.CustomRequests.Factory;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ComplexStructureQueryBuilderManager<T> : IComplexStructureQueryBuilderManager<T>
    {
        private readonly IComplexStructureQueryFactory<T> _factory;

       /// <summary>
       /// Checks each ComplexStructureQueryFactory registered to the Spring beans framework asking each one in turn to
       /// obtain a query builder.  The StructureQueryFactory to respond with a not null value, will be returned 
       /// </summary>
       /// <param name="factory"></param>
       public ComplexStructureQueryBuilderManager(IComplexStructureQueryFactory<T> factory)
       {
           this._factory = factory;
       }

        public T BuildComplexStructureQuery(IComplexStructureQuery query, IStructureQueryFormat<T> structureQueryFormat)
        {
            var builder =  this._factory.GetComplexStructureQueryBuilder(structureQueryFormat);
			if(builder != null) {
                return builder.BuildComplexStructureQuery(query);
			}
		
		throw new SdmxUnauthorisedException("Unsupported ComplexStructureQueryFormat: " + structureQueryFormat);
        }
    }
}

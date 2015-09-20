// -----------------------------------------------------------------------
// <copyright file="ComplexStructureQueryFactoryV21.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.CustomRequests.Builder.QueryBuilder;
    using Estat.Sri.CustomRequests.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ComplexStructureQueryFactoryV21<T> :IComplexStructureQueryFactory<XDocument>
    {
        public IComplexStructureQueryBuilder<XDocument> GetComplexStructureQueryBuilder(IStructureQueryFormat<XDocument> format)
        {
            if(format is ComplexQueryFormatV21)
            {
                return new ComplexStructureQueryBuilderV21();
		    }        
          
		return null;
        }

      
    }
}

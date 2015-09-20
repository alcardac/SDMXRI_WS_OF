// -----------------------------------------------------------------------
// <copyright file="ComplexDataQueryFactoryV21.cs" company="Microsoft">
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
	public class ComplexDataQueryFactoryV21 : IComplexDataQueryFactory
	{

		public IComplexDataQueryBuilder GetComplexDataQueryBuilder(IDataQueryFormat<XDocument> format)
		{
            //HAHAA, why not using an enum?
			if (format is GenericTimeSeriesDataFormatV21) {
			return new GenericTimeSeriesDataQueryBuilderV21();
		}
			if (format is GenericDataDocumentFormatV21) {
				return  new GenericDataQueryBuilderV21();
			}
			if (format is StructSpecificDataFormatV21) {
				return  new StructSpecificDataQueryBuilderV21();
			}
			if (format is StructSpecificTimeSeriesDataFormatV21) {
                return new StructSpecificTimeSeriesQueryBuilderV21();
			}

			return null;
		}
	}
}

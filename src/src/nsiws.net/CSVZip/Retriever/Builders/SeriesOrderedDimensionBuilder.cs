// -----------------------------------------------------------------------
// <copyright file="SeriesOrderedDimensionBuilder.cs" company="Eurostat">
//   Date Created : 2013-11-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace CSVZip.Retriever.Builders
{
    using System.Collections.Generic;
    using System.Configuration;

    using CSVZip.Retriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// The series ordered dimension builder.
    /// </summary>
    internal class SeriesOrderedDimensionBuilder
    {
        /// <summary>
        /// Builds an ordered list of Components from the specified <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The DataRetriever state information.
        /// </param>
        /// <returns>
        /// The list of components.
        /// </returns>
        public IList<ComponentEntity> Build(DataRetrievalInfo info)
        {
            DsdEntity dsd = info.MappingSet.Dataflow.Dsd;
            IBaseDataQuery baseDataQuery = (IBaseDataQuery)info.ComplexQuery ?? info.Query;

            // build a list of the components that that must affect the order,
            // in the correct order (dimensions, then time)
            var orderComponents = new List<ComponentEntity>();
            var dimensionAtObservation = baseDataQuery.DimensionAtObservation;

            var allDimensions = DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value;
            if (dimensionAtObservation.Equals(allDimensions))
            {
                HandleFlat(orderComponents, dsd);
            }
            else
            {
                HandleOrdered(dsd, dimensionAtObservation, orderComponents);
            }

            return orderComponents;
        }

        /// <summary>
        /// Handles the ordered.
        /// </summary>
        /// <param name="dsd">The DSD.</param>
        /// <param name="dimensionAtObservation">The dimension attribute observation.</param>
        /// <param name="orderComponents">The order components.</param>
        private static void HandleOrdered(DsdEntity dsd, string dimensionAtObservation, List<ComponentEntity> orderComponents)
        {
            ComponentEntity dimensionAtObsEntity = dsd.TimeDimension;
            foreach (ComponentEntity dim in dsd.Dimensions)
            {
                if (!dim.Id.Equals(dimensionAtObservation))
                {
                    orderComponents.Add(dim);
                }
                else
                {
                    dimensionAtObsEntity = dim;
                }
            }

            if (!Equals(dimensionAtObsEntity, dsd.TimeDimension))
            {
               orderComponents.Add(dsd.TimeDimension);
            }

            orderComponents.Add(dimensionAtObsEntity);
        }

        /// <summary>
        /// Handles the flat.
        /// </summary>
        /// <param name="orderComponents">The order components.</param>
        /// <param name="dsd">The DSD.</param>
        private static void HandleFlat(List<ComponentEntity> orderComponents, DsdEntity dsd)
        {
            bool bFlat;
            if (!bool.TryParse(ConfigurationManager.AppSettings["QueryFlatFormat"], out bFlat) || !bFlat)
            {
                orderComponents.AddRange(dsd.Dimensions);
                if (dsd.TimeDimension == null)
                {
                    // Comment out because I don't understand why we remove the last dimension and why we add the last attribute to the order by.
                    // This code will fail if there are no attributes in a DSD 
                    ////orderComponents.RemoveAt(orderComponents.Count - 1);
                    ////orderComponents.Add(dsd.Attributes[dsd.Attributes.Count - 1]);
                }
                else
                {
                    orderComponents.Add(dsd.TimeDimension);
                }
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQueryFluentBuilder.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDataQueryFluentBuilder
    {
        /// <summary>
        /// Initialize the object to create a new DataQuery instance
        /// </summary>
        /// <param name="dataStructure"></param>
        /// <param name="dataflow"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder Initialize(IDataStructureObject dataStructure, IDataflowObject dataflow);

        /// <summary>
        /// Add Last Upated parameter
        /// </summary>
        /// <param name="lastUpdated"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithLastUpdated(ISdmxDate lastUpdated);

        /// <summary>
        /// Add Data Query Detail
        /// </summary>
        /// <param name="dataQueryDetail"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDataQueryDetail(DataQueryDetail dataQueryDetail);

        /// <summary>
        /// Add Data Providers
        /// </summary>
        /// <param name="dataProviders"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDataProviders(ISet<IDataProvider> dataProviders);

        /// <summary>
        /// Add Date From
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDateFrom(DateTime dateFrom);

        /// <summary>
        /// Add Date To
        /// </summary>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDateTo(DateTime dateTo);

        /// <summary>
        /// Add Max Observations
        /// </summary>
        /// <param name="maxObs"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithMaxObservations(int maxObs);

        /// <summary>
        /// Add Ordrer Asc
        /// </summary>
        /// <param name="orderAsc"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithOrderAsc(bool orderAsc);

        /// <summary>
        /// Add First N Obs
        /// </summary>
        /// <param name="firstNObs"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithFirstNObs(int firstNObs);

        /// <summary>
        /// Add Last N Obs
        /// </summary>
        /// <param name="lastNObs"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithLastNObs(int lastNObs);

        /// <summary>
        /// Add Dimension Observation At
        /// </summary>
        /// <param name="dimensionAtObservation"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDimensionAtObservation(string dimensionAtObservation);

        /// <summary>
        /// Add Data Query Selections
        /// </summary>
        /// <param name="dataQuerySelections"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDataQuerySelections(ISet<IDataQuerySelection> dataQuerySelections);

        /// <summary>
        /// Add Data Query Selection Group
        /// </summary>
        /// <param name="dataQuerySelectionGroups"></param>
        /// <returns></returns>
        IDataQueryFluentBuilder WithDataQuerySelectionGroup(ICollection<IDataQuerySelectionGroup> dataQuerySelectionGroups);

        /// <summary>
        /// Materialize the object construction
        /// </summary>
        /// <returns>Returns a new instance of DataQuery</returns>
        IDataQuery Build();
    }
}

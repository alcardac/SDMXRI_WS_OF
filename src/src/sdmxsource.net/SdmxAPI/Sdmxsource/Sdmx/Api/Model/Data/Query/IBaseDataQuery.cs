// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseDataQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data.Query
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    /// Base interface for both standard and complex data queries
    /// </summary>
    public interface IBaseDataQuery : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Returns the Dataflow that the query is returning data for.
        /// </summary>
        IDataflowObject Dataflow { get; }

        /// <summary>
        /// Returns the data provider(s) that the query is for, an empty list represents ALL
        /// </summary>
        ISet<IDataProvider> DataProvider { get; }

        /// <summary>
        /// Returns the detail of the query.  The detail specifies whether to return just the series keys, just the data or everything.  
        /// <p/>
        /// Defaults to FULL (everything)
        /// </summary>
        DataQueryDetail DataQueryDetail { get; }

        /// <summary>
        /// Returns the Key Family (Data Structure Definition) that this query is returning data for.
        /// </summary>
        IDataStructureObject DataStructure { get; }

        /// <summary>
        /// Returns the first 'n' observations, for each series key,  to return as a result of a data query.
        /// </summary>
        int? FirstNObservations { get; }

        /// <summary>
        /// Returns the last 'n' observations, for each series key, to return as a result of a data query.
        /// </summary>
        int? LastNObservations { get; }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Gets the id of the dimension to be attached at the observation level,
        /// if not specified the returned data will be time series.
        /// </summary>
        /// <value>
        /// The dimension
        /// </value>
        string DimensionAtObservation { get; }

        /// <summary>
        /// Returns true if this query has one or more selection groups on it, false means the query is a query for all.
        /// </summary>
        /// <returns>
        /// The boolean
        /// </returns>
        bool HasSelections();
 
        #endregion
    }
}

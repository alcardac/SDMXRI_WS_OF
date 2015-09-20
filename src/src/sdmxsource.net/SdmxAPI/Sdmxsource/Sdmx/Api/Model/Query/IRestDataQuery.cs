// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestDataQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Query
{
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// This interface holds a SDMX REST Data Query.  
    /// </summary>
    public interface IRestDataQuery
    {
        /// <summary>
        /// Gets a String representation of this query, in SDMX REST format starting from Data/.
        /// <example>
        /// Example Data/ACY,FLOW,1.0/M.Q+P....L/ALL?detail=seriesKeysOnly
        /// </example>
        /// </summary>
        string RestQuery { get; }

        /// <summary>
        /// Gets the dataflow reference
        /// </summary>
        IStructureReference FlowRef { get; }

        /// <summary>
        /// Gets the data provider reference, or null if ALL
        /// </summary>
        IStructureReference ProviderRef { get; }

        /// <summary>
        /// Gets the start date to  the data from, or null if undefined
        /// </summary>
        ISdmxDate StartPeriod { get; }

        /// <summary>
        /// Gets the end date to  the data from, or null if undefined
        /// </summary>
        ISdmxDate EndPeriod { get; }

        /// <summary>
        /// Gets the updated after date to  the data from, or null if undefined
        /// </summary>
        ISdmxDate UpdatedAfter { get; }

        /// <summary>
        /// Gets the first 'n' observations, for each series key, to return as a result of a data query.  
        /// </summary>
        int? LastNObsertations { get; }

        /// <summary>
        /// Gets the last 'n' observations, for each series key,  to return as a result of a data query
        /// </summary>
        int? FirstNObservations { get; }

        /// <summary>
        /// Gets the level of detail for the returned data, or null if undefined
        /// </summary>
        DataQueryDetail QueryDetail { get; }

        /// <summary>
        /// Gets the dimension to , or null if undefined
        /// </summary>
        string DimensionAtObservation { get; }

        /// <summary>
        /// Gets the list of dimension code id filters, in the same order as the dimensions are defined by the DataStructure
        /// </summary>
        IList<ISet<string>> QueryList { get; }
    }
}

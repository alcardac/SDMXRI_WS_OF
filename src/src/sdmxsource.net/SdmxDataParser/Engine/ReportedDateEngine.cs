// -----------------------------------------------------------------------
// <copyright file="ReportedDateEngine.cs" company="Eurostat">
//   Date Created : 2014-07-16
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;

    /// <summary>
    /// The reported date engine.
    /// </summary>
    public class ReportedDateEngine : IReportedDateEngine
    {
        /// <summary>
        /// Gets all reported dates.
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine</param>
        /// <returns>
        /// The reported dates
        /// </returns>
        public IDictionary<TimeFormat, IList<string>> GetAllReportedDates(IDataReaderEngine dataReaderEngine)
        {
            dataReaderEngine.Reset();
            var processedDates = new HashSet<string>(StringComparer.Ordinal);

            var timeFormatToSortedMap = new Dictionary<TimeFormatEnumType, IDictionary<DateTime, string>>();
            try
            {
                while (dataReaderEngine.MoveNextKeyable())
                {
                    var obs = dataReaderEngine.CurrentObservation;
                    var obsTime = obs.ObsTime;

                    // Check we have not already processed this date
                    if (processedDates.Contains(obsTime))
                    {
                        var obsTimeFormat = obs.ObsTimeFormat;
                        IDictionary<DateTime, string> sortedMap;

                        // Get the correct sorted map (or create it if it does not yet exist)
                        if (!timeFormatToSortedMap.TryGetValue(obsTimeFormat, out sortedMap))
                        {
                            sortedMap = new SortedDictionary<DateTime, string>();
                            timeFormatToSortedMap.Add(obsTimeFormat, sortedMap);
                        }

                        Debug.Assert(obs.ObsAsTimeDate.HasValue, "Observation time was null. Ported from Java where null is valid key for HashMap.");
                        sortedMap.Add(obs.ObsAsTimeDate.Value, obsTime);
                        processedDates.Add(obsTime);
                    }
                }

                // Create the response map. TODO split method.
                var responseMap = new Dictionary<TimeFormat, IList<string>>();
                foreach (var keyValuePair in timeFormatToSortedMap)
                {
                    var sortedDateList = new List<string>();
                    responseMap.Add(TimeFormat.GetFromEnum(keyValuePair.Key), sortedDateList);
                    var sortedMap = keyValuePair.Value;
                    sortedDateList.AddRange(sortedMap.Values);
                }

                return responseMap;
            }
            finally
            {
                dataReaderEngine.Reset();
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetPosition.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    /// <summary>
    ///     Contains a list of possible position in a dataset
    /// </summary>
    public enum DatasetPosition
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Position is at the dataset level
        /// </summary>
        Dataset, 

        /// <summary>
        ///     Position is at the dataset attribute level
        /// </summary>
        DatasetAttribute, 

        /// <summary>
        ///     Position is at the series level
        /// </summary>
        Series, 

        /// <summary>
        ///     Position is at the series key level
        /// </summary>
        SeriesKey, 

        /// <summary>
        ///     Position is at the series key attribute level
        /// </summary>
        SeriesKeyAttribute, 

        /// <summary>
        ///     Position is at the group level
        /// </summary>
        Group, 

        /// <summary>
        ///     Position is at the group key level
        /// </summary>
        GroupKey, 

        /// <summary>
        ///     Position is at the group key attribute level
        /// </summary>
        GroupKeyAttribute, 

        // GROUP_SERIES_KEY,
        // GROUP_SERIES_KEY_ATTRIBUTE,

        /// <summary>
        ///     Position is at the observation level
        /// </summary>
        Observation, 

        /// <summary>
        ///     Position is at the observation attribute level
        /// </summary>
        ObservationAttribute, 

        /// <summary>
        ///     Position is at the series level, when the series also contains the observation information (SDMX 2.1 only)
        /// </summary>
        ObservationAsSeries
    }
}
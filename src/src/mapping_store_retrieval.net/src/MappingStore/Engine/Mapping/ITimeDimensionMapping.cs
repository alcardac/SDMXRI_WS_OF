// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeDimension.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A common interface for Time Dimension Transcoding classes
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// A common interface for Time Dimension Transcoding classes
    /// </summary>
    public interface ITimeDimensionMapping : IMapping
    {
        #region Public Methods

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query Time
        /// </summary>
        /// <param name="dateFrom">The start time</param>
        /// <param name="dateTo">The end time</param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo);

        #endregion
    }
}
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
    using System.Data;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// A common interface for Time Dimension Transcoding classes
    /// </summary>
    public interface ITimeDimension
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the DSD Component.
        /// </summary>
        ComponentEntity Component { get; set; }

        /// <summary>
        /// Gets or sets the mapping
        /// </summary>
        MappingEntity Mapping { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query Time
        /// </summary>
        /// <param name="dateFrom">The start time</param>
        /// <param name="dateTo">The end time</param>
        /// <param name="frequencyValue">
        /// The frequency value
        /// </param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo, string frequencyValue);

        /// <summary>
        /// Map component.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="frequencyValue">
        /// The frequency value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string MapComponent(IDataReader reader, string frequencyValue);

        #endregion
    }
}
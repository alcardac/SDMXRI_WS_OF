// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionMultiFrequency.cs" company="Eurostat">
//   Date Created : 2013-07-10
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The time dimension multi frequency transcoding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// The time dimension multi frequency transcoding.
    /// </summary>
    internal class TimeDimensionMultiFrequency : ITimeDimension
    {
        #region Fields

        /// <summary>
        /// The _time dimension mappings.
        /// </summary>
        private readonly IDictionary<string, ITimeDimensionMapping> _timeDimensionMappings;

        /// <summary>
        /// The _frequency component mapping
        /// </summary>
        private readonly IComponentMapping _frequencyComponentMapping;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimensionMultiFrequency" /> class.
        /// </summary>
        /// <param name="timeDimensionMappings">The time dimension mappings.</param>
        /// <param name="frequencyComponentMapping">The frequency component mapping.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="timeDimensionMappings"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="timeDimensionMappings" /> is null</exception>
        public TimeDimensionMultiFrequency(IDictionary<string, ITimeDimensionMapping> timeDimensionMappings, IComponentMapping frequencyComponentMapping)
        {
            if (timeDimensionMappings == null)
            {
                throw new ArgumentNullException("timeDimensionMappings");
            }

            this._timeDimensionMappings = timeDimensionMappings;
            this._frequencyComponentMapping = frequencyComponentMapping;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the DSD Component.
        /// </summary>
        public ComponentEntity Component { get; set; }

        /// <summary>
        ///     Gets or sets the mapping
        /// </summary>
        public MappingEntity Mapping { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query Time
        /// </summary>
        /// <param name="dateFrom">
        /// The start time
        /// </param>
        /// <param name="dateTo">
        /// The end time
        /// </param>
        /// <param name="frequencyValue">
        /// The frequency value 
        /// </param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        public string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo, string frequencyValue)
        {
            var whereSql = new StringBuilder();
            if (frequencyValue != null)
            {
                ITimeDimensionMapping engine;
                if (this._timeDimensionMappings.TryGetValue(frequencyValue, out engine))
                {
                    var whereClause = this.GenerateWhereClause(dateFrom, dateTo, frequencyValue, engine);
                    whereSql.Append(whereClause);
                }
            }
            else if (this._timeDimensionMappings.Count > 0)
            {
                string op = string.Empty;
                foreach (var timeDimensionMapping in this._timeDimensionMappings)
                {
                    var generateWhere = this.GenerateWhereClause(dateFrom, dateTo, timeDimensionMapping.Key, timeDimensionMapping.Value);
                    if (!string.IsNullOrWhiteSpace(generateWhere))
                    {
                        whereSql.AppendFormat(CultureInfo.InvariantCulture, "{0} ( {1} )", op, generateWhere);
                        op = " OR ";
                    }
                }
            }

            return whereSql.Length > 0 ? string.Format("({0})", whereSql) : string.Empty;
        }

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
        /// The transcoded value
        /// </returns>
        public string MapComponent(IDataReader reader, string frequencyValue)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (frequencyValue == null)
            {
                throw new ArgumentNullException("frequencyValue");
            }

            ITimeDimensionMapping engine;
            if (this._timeDimensionMappings.TryGetValue(frequencyValue, out engine))
            {
                return engine.MapComponent(reader);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Generates the where clause.
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="frequencyValue">The frequency value.</param>
        /// <param name="engine">The engine.</param>
        /// <returns>The where clause </returns>
        private string GenerateWhereClause(ISdmxDate dateFrom, ISdmxDate dateTo, string frequencyValue, ITimeDimensionMapping engine)
        {
            string frequencyWhereClause = this._frequencyComponentMapping.GenerateComponentWhere(frequencyValue);
            if (!string.IsNullOrWhiteSpace(frequencyWhereClause))
            {
                string timePeriodsWhereClauses = engine.GenerateWhere(dateFrom, dateTo);
                if (!string.IsNullOrWhiteSpace(timePeriodsWhereClauses))
                {
                    var whereClause = string.Format(CultureInfo.InvariantCulture, "(( {0} ) and ( {1} ))", frequencyWhereClause, timePeriodsWhereClauses);
                    return whereClause;
                }
            }

            return string.Empty;
        }
    }
}
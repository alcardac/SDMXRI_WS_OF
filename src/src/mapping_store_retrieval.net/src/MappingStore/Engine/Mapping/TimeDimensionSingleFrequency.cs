// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionSingleFrequency.cs" company="Eurostat">
//   Date Created : 2013-07-10
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The time dimension single frequency.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Data;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    /// The time dimension single frequency.
    /// </summary>
    public class TimeDimensionSingleFrequency : ITimeDimension
    {
        #region Fields

        /// <summary>
        /// The _time dimension mapping.
        /// </summary>
        private readonly ITimeDimensionMapping _timeDimensionMapping;

        /// <summary>
        /// The _component.
        /// </summary>
        private ComponentEntity _component;

        /// <summary>
        /// The _mapping.
        /// </summary>
        private MappingEntity _mapping;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimensionSingleFrequency"/> class.
        /// </summary>
        /// <param name="timeDimensionMapping">
        /// The time dimension mapping.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="timeDimensionMapping"/> is null
        /// </exception>
        public TimeDimensionSingleFrequency(ITimeDimensionMapping timeDimensionMapping)
        {
            if (timeDimensionMapping == null)
            {
                throw new ArgumentNullException("timeDimensionMapping");
            }

            this._timeDimensionMapping = timeDimensionMapping;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the DSD Component.
        /// </summary>
        public ComponentEntity Component
        {
            get
            {
                return this._component;
            }

            set
            {
                this._component = value;
            }
        }

        /// <summary>
        ///     Gets or sets the mapping
        /// </summary>
        public MappingEntity Mapping
        {
            get
            {
                return this._mapping;
            }

            set
            {
                this._mapping = value;
            }
        }

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
            if (!string.IsNullOrEmpty(frequencyValue))
            {
                TimeFormat format = TimeFormat.GetTimeFormatFromCodeId(frequencyValue);
                if (dateFrom != null)
                {
                    dateFrom = new SdmxDateCore(dateFrom.Date, format.EnumType);
                }

                if (dateTo != null)
                {
                    dateTo = new SdmxDateCore(dateTo.Date, format.EnumType);
                }
            }

            return this._timeDimensionMapping.GenerateWhere(dateFrom, dateTo);
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
        /// The <see cref="string"/>.
        /// </returns>
        public string MapComponent(IDataReader reader, string frequencyValue)
        {
            return this._timeDimensionMapping.MapComponent(reader);
        }

        #endregion
    }
}
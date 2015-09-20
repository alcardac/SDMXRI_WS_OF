// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeriodicityFactory.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The periodicity factory.
    /// </summary>
    public static class PeriodicityFactory
    {
        /// <summary>
        /// The time format to periodicities dictionary
        /// </summary>
        private static readonly IDictionary<TimeFormatEnumType, IPeriodicity> _periodicities = new Dictionary<TimeFormatEnumType, IPeriodicity>();

        /// <summary>
        /// Initializes static members of the <see cref="PeriodicityFactory"/> class.
        /// </summary>
        static PeriodicityFactory()
        {
            _periodicities.Add(TimeFormatEnumType.Year, new Annual());
            _periodicities.Add(TimeFormatEnumType.Month, new Monthly());
            _periodicities.Add(TimeFormatEnumType.QuarterOfYear, new Quarterly());
            _periodicities.Add(TimeFormatEnumType.HalfOfYear, new Semester());
            _periodicities.Add(TimeFormatEnumType.Date, new Daily());
            _periodicities.Add(TimeFormatEnumType.DateTime, new Hourly());
            _periodicities.Add(TimeFormatEnumType.Hour, new Hourly());
            _periodicities.Add(TimeFormatEnumType.ThirdOfYear, new TriAnnual());
        }

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="timeFormat">
        /// The time format.
        /// </param>
        /// <returns>
        /// The <see cref="IPeriodicity"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws ArgumentOutOfRangeException
        /// </exception>
        public static IPeriodicity Create(TimeFormatEnumType timeFormat)
        {
            IPeriodicity value;
            if (_periodicities.TryGetValue(timeFormat, out value))
            {
                return value;
            }

            throw new ArgumentOutOfRangeException("timeFormat", timeFormat, "Not supported periodicity.");
        }

        #endregion
    }
}
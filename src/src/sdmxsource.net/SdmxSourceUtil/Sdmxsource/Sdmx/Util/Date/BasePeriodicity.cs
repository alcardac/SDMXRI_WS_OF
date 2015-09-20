// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePeriodicity.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The base periodicity class.
    /// </summary>
    public abstract class BasePeriodicity
    {
        #region Static Fields

        /// <summary>
        /// The default calendar instance
        /// </summary>
        private static readonly Calendar _calendar = CultureInfo.InvariantCulture.Calendar;

        #endregion

        #region Fields

        /// <summary>
        /// The time format
        /// </summary>
        private readonly TimeFormat _timeFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePeriodicity"/> class.
        /// </summary>
        /// <param name="timeFormat">
        /// The time format. 
        /// </param>
        protected BasePeriodicity(TimeFormatEnumType timeFormat)
        {
            this._timeFormat = TimeFormat.GetFromEnum(timeFormat);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the time format
        /// </summary>
        public TimeFormat TimeFormat
        {
            get
            {
                return this._timeFormat;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default calendar instance
        /// </summary>
        protected internal static Calendar Calendar
        {
            get
            {
                return _calendar;
            }
        }

        #endregion
    }
}
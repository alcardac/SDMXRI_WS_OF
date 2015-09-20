// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesPeriod.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Date
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// This class contains information related to a GESMES/TS periodicity
    /// </summary>
    public class GesmesPeriod
    {
        /// <summary>
        /// The periodicity
        /// </summary>
        private readonly IPeriodicity _periodicity;

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesPeriod"/> class.
        /// </summary>
        /// <param name="periodicity">The periodicity.</param>
        public GesmesPeriod(IPeriodicity periodicity)
        {
            this._periodicity = periodicity;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the single period time format code
        /// </summary>
        public EdiTimeFormat DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the period number format
        /// </summary>
        public string PeriodFormat { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of a period
        /// </summary>
        public int PeriodMax { get; set; }

        /// <summary>
        /// Gets or sets the extra modifiers that should be XOR'ed with <see cref="DateFormat" /> for the <see
        ///  cref="RangeTimeFormat" />
        /// </summary>
        public EdiTimeFormat RangeFormat { get; set; }

        /// <summary>
        /// Gets the SDMX time format.
        /// </summary>
        /// <value>
        /// The SDMX time format.
        /// </value>
        public TimeFormat SdmxTimeFormat
        {
            get
            {
                return this._periodicity.TimeFormat;
            }
        }

        /// <summary>
        /// Gets the range period time format code
        /// </summary>
        public int RangeTimeFormat
        {
            get
            {
                return (int)this.RangeFormat;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the difference between <paramref name="first"/> and <paramref name="second"/>
        /// </summary>
        /// <param name="first">
        /// The first value 
        /// </param>
        /// <param name="second">
        /// The second value 
        /// </param>
        /// <returns>
        /// the difference between <paramref name="first"/> and <paramref name="second"/> 
        /// </returns>
        public int Diff(int first, int second)
        {
            if (string.IsNullOrEmpty(this.PeriodFormat))
            {
                return first - second;
            }

            int firstYear;
            int firstPeriod;
            int secondYear;
            int secondPeriod;
            if (first > 99999)
            {
                firstYear = first / 100;
                firstPeriod = first % 100;
            }
            else
            {
                firstYear = first / 10;
                firstPeriod = first % 10;
            }

            if (second > 99999)
            {
                secondYear = second / 100;
                secondPeriod = second % 100;
            }
            else
            {
                secondYear = second / 10;
                secondPeriod = second % 10;
            }

            return ((firstYear - secondYear) * this.PeriodMax) + (firstPeriod - secondPeriod);
        }

        #endregion
    }
}
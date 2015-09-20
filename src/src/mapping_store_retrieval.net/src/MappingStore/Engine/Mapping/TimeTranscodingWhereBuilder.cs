// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeTranscodingWhereBuilder.cs" company="Eurostat">
//   Date Created : 2013-08-02
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The time transcoding where builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// The time transcoding where builder.
    /// </summary>
    internal class TimeTranscodingWhereBuilder
    {
        #region Static Fields

        /// <summary>
        /// The _format provider.
        /// </summary>
        private static readonly IFormatProvider _formatProvider = CultureInfo.InvariantCulture;

        #endregion

        #region Fields

        /// <summary>
        /// The _expression entity.
        /// </summary>
        private readonly TimeExpressionEntity _expressionEntity;

        /// <summary>
        /// The _periodicity.
        /// </summary>
        private readonly IPeriodicity _periodicity;

        /// <summary>
        /// The _where format.
        /// </summary>
        private readonly string _whereFormat;

        /// <summary>
        /// The _year only end.
        /// </summary>
        private readonly string _yearOnlyEnd;

        /// <summary>
        /// The _year only start.
        /// </summary>
        private readonly string _yearOnlyStart;

        /// <summary>
        /// The _year only where format.
        /// </summary>
        private readonly string _yearOnlyWhereFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTranscodingWhereBuilder"/> class. 
        /// </summary>
        /// <param name="periodicity">
        /// The periodicity.
        /// </param>
        /// <param name="expressionEntity">
        /// The expression Entity.
        /// </param>
        /// <param name="whereFormat">
        /// The where Format.
        /// </param>
        /// <param name="yearOnlyEnd">
        /// The year Only End.
        /// </param>
        /// <param name="yearOnlyStart">
        /// The year Only Start.
        /// </param>
        /// <param name="yearOnlyWhereFormat">
        /// The year Only Where Format.
        /// </param>
        public TimeTranscodingWhereBuilder(IPeriodicity periodicity, TimeExpressionEntity expressionEntity, string whereFormat, string yearOnlyEnd, string yearOnlyStart, string yearOnlyWhereFormat)
        {
            this._periodicity = periodicity;
            this._expressionEntity = expressionEntity;
            this._whereFormat = whereFormat;
            this._yearOnlyEnd = yearOnlyEnd;
            this._yearOnlyStart = yearOnlyStart;
            this._yearOnlyWhereFormat = yearOnlyWhereFormat;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Extracts a SdmxQueryTimeVO<see cref="SdmxQueryTimeVO"/> object from a TimeBean<see cref="ISdmxDate"/>
        /// </summary>
        /// <param name="dateFrom">
        /// Start time <see cref="ISdmxDate"/> A SDMX Query Time element
        /// </param>
        /// <param name="dateTo">
        /// End time <see cref="ISdmxDate"/> A SDMX Query Time element
        /// </param>
        /// <returns>
        /// SdmxQueryTimeVO<see cref="SdmxQueryTimeVO"/>
        /// </returns>
        public SdmxQueryTimeVO ExtractTimeBean(ISdmxDate dateFrom, ISdmxDate dateTo)
        {
            var startDate = dateFrom.ToQueryPeriod(this._periodicity) ?? new SdmxQueryPeriod { HasPeriod = false, Year = 0, Period = 1 };
            var now = DateTime.Now;
            var endDate = dateTo.ToQueryPeriod(this._periodicity) ?? new SdmxQueryPeriod { HasPeriod = false, Year = now.Year, Period = ((DateTime.Now.Month - 1) / this._periodicity.MonthsPerPeriod) + 1 };

            if (!startDate.HasPeriod && startDate.Period == 0)
            {
                startDate.Period = 1;
            }

            if (!endDate.HasPeriod && endDate.Period == 0)
            {
                endDate.Period = this._periodicity.PeriodCount;
            }

            return new SdmxQueryTimeVO { EndPeriod = endDate.Period, EndYear = endDate.Year, HasEndPeriod = endDate.HasPeriod, HasStartPeriod = startDate.HasPeriod, StartPeriod = startDate.Period, StartYear = startDate.Year };
        }

        /// <summary>
        /// The where build.
        /// </summary>
        /// <param name="fromDate">
        /// The from date.
        /// </param>
        /// <param name="toDate">
        /// The to date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string WhereBuild(ISdmxDate fromDate, ISdmxDate toDate)
        {
            if (fromDate == null && toDate == null)
            {
                return string.Empty;
            }

            // NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;
            var ret = new StringBuilder("(");

            SdmxQueryTimeVO time = this.ExtractTimeBean(fromDate, toDate);

            // start time includes a period and the freq is not annual
            if (time.HasStartPeriod && this._expressionEntity.Freq != TimeFormatEnumType.Year)
            {
                int firstPeriodLength = this._periodicity.PeriodCount;
                if (time.StartYear == time.EndYear)
                {
                    firstPeriodLength = time.EndPeriod;
                }

                string yearStr = time.StartYear.ToString(_formatProvider);
                int lastClause = this.BuildPeriodWhere(ret, yearStr, time.StartPeriod, firstPeriodLength, this._periodicity, this._expressionEntity);
                if (time.StartYear + 1 < time.EndYear)
                {
                    ret.Append(" (");
                    yearStr = (time.StartYear + 1).ToString(_formatProvider);
                    ret.AppendFormat(CultureInfo.InvariantCulture, this._yearOnlyStart, yearStr);
                    yearStr = (time.EndYear - 1).ToString(_formatProvider);
                    ret.Append(" and ");
                    ret.AppendFormat(CultureInfo.InvariantCulture, this._yearOnlyEnd, yearStr);
                    ret.Append(")");

                    lastClause = ret.Length;
                    ret.Append(" or ");
                }

                if (time.StartYear != time.EndYear)
                {
                    if (time.HasEndPeriod)
                    {
                        yearStr = time.EndYear.ToString(_formatProvider);
                        lastClause = this.BuildPeriodWhere(ret, yearStr, 1, time.EndPeriod, this._periodicity, this._expressionEntity);
                    }
                    else
                    {
                        ret.Append(string.Format(CultureInfo.InvariantCulture, this._yearOnlyWhereFormat, time.EndYear.ToString(_formatProvider)));
                        lastClause = ret.Length;
                        ret.Append(" or ");
                    }
                }

                ret.Length = lastClause;
            }
            else
            {
                ret.AppendFormat(CultureInfo.InvariantCulture, this._yearOnlyStart, time.StartYear.ToString(_formatProvider));
                if (toDate != null)
                {
                    ret.Append(" and ");
                    ret.AppendFormat(CultureInfo.InvariantCulture, this._yearOnlyEnd, time.EndYear.ToString(_formatProvider));
                }
            }

            ret.Append(")");
            return ret.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build where clauses in the form of : (year and (periodA or periodB or ... ) and write them
        /// to a specified StringBuilder
        /// </summary>
        /// <param name="ret">The StringBuilder to write the output</param>
        /// <param name="yearStr">The year string</param>
        /// <param name="firstPeriod">The first period</param>
        /// <param name="lastPeriod">The last period</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="timeExpressionEntity">The time expression entity.</param>
        /// <returns>
        /// The position of the last clause inside the StringBuilder
        /// </returns>
        private int BuildPeriodWhere(StringBuilder ret, string yearStr, int firstPeriod, int lastPeriod, IPeriodicity periodicity, TimeExpressionEntity timeExpressionEntity)
        {
            var queryCode = new CodeCollection();

            ret.Append("( ");
            ret.AppendFormat(this._yearOnlyWhereFormat, yearStr);
            int lastClause = ret.Length;
            ret.Append(" and ");
            ret.Append("( ");
            for (int x = firstPeriod; x <= lastPeriod; x++)
            {
                string periodStr = x.ToString(periodicity.Format, _formatProvider);
                queryCode.Clear();
                queryCode.Add(periodStr);
                string periodCode = timeExpressionEntity.TranscodingRules.GetLocalCodes(queryCode)[0][0];
                ret.AppendFormat(CultureInfo.InvariantCulture, this._whereFormat, periodCode);
                lastClause = ret.Length;
                ret.Append(" or ");
            }

            ret.Length = lastClause;
            ret.Append(") ");
            ret.Append(") ");
            lastClause = ret.Length;
            ret.Append(" or ");
            return lastClause;
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimension1Column.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This Time Dimension Transcoding class is used for 1-1 mappings between
//   a Time Dimension and one dissemination column (not Date type)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    /// <summary>
    /// This Time Dimension Transcoding class is used for 1-1 mappings between
    /// a Time Dimension and one dissemination column (not Date type)
    /// </summary>
    internal class TimeDimension1Column : TimeDimensionMapping, ITimeDimensionMapping
    {
        #region Constants and Fields

        /// <summary>
        /// Holds the current local codes for period
        /// </summary>
        private readonly CodeCollection _periodLocalCode;

        /// <summary>
        /// The where builder.
        /// </summary>
        private readonly TimeTranscodingWhereBuilder _whereBuilder;

        /// <summary>
        /// The field ordinals
        /// </summary>
        private readonly TimeTranscodingFieldOrdinal _fieldOrdinals;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimension1Column"/> class. 
        /// </summary>
        /// <param name="mapping">
        /// The time dimension mapping
        /// </param>
        /// <param name="expression">
        /// The TRANSCODING.EXPRESSION contents
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor from  DB_CONNECTION.DB_TYPE at Mapping Store database. It is used to determine the substring command to use
        /// </param>
        /// <exception cref="TranscodingException">
        /// Occurs when transcoding cannot performed due to incorrect mapping store data
        /// </exception>
        public TimeDimension1Column(MappingEntity mapping, TimeExpressionEntity expression, string databaseType)
            : base(mapping, expression, databaseType)
        {
            this._periodLocalCode = new CodeCollection();
            string yearPeriodColumn = GetColumnName(mapping, expression.YearColumnSysId);
            string yearOnlyStart = this.CreateSubStringClause(yearPeriodColumn, expression.YearStart + 1, expression.YearLength, ">=");
            string yearOnlyEnd = this.CreateSubStringClause(yearPeriodColumn, expression.YearStart + 1, expression.YearLength, "<=");

            string whereFormat = this.CreateSubStringClause(yearPeriodColumn, expression.PeriodStart + 1, expression.PeriodLength, "=");

            string yearOnlyWhereFormat = this.CreateSubStringClause(yearPeriodColumn, expression.YearStart + 1, expression.YearLength, "=");

            this._whereBuilder = new TimeTranscodingWhereBuilder(this.Periodicity, this.Expression, whereFormat, yearOnlyEnd, yearOnlyStart, yearOnlyWhereFormat);
            this._fieldOrdinals = new TimeTranscodingFieldOrdinal(mapping, this.Expression);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Query where condition from the SDMX Query TimeBean <see cref="ISdmxDate"/>
        /// </summary>
        /// <param name="dateFrom">The start time period</param>
        /// <param name="dateTo">The end time period</param>
        /// <returns>
        /// The string containing SQL Query where condition
        /// </returns>
        public string GenerateWhere(ISdmxDate dateFrom, ISdmxDate dateTo)
        {
            return this._whereBuilder.WhereBuild(dateFrom, dateTo);
        }

        /// <summary>
        /// Transcodes the time period returned by the local database to SDMX Time period
        /// </summary>
        /// <param name="reader">
        /// The data reader reading the Dissemination database
        /// </param>
        /// <returns>
        /// The transcoded time period, as in SDMX Time period type
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            string ret;
            this._fieldOrdinals.BuildOrdinal(reader);

            string yearperiod = DataReaderHelper.GetString(reader, this._fieldOrdinals.YearOrdinal);
            if (this.Expression.YearStart + this.Expression.YearLength > yearperiod.Length)
            {
                return null;
            }

            string year = yearperiod.Substring(this.Expression.YearStart, this.Expression.YearLength);
            var rowFreq = this.Expression.Freq;
            if (rowFreq != TimeFormatEnumType.Year)
            {
                if (this.Expression.PeriodStart >= yearperiod.Length)
                {
                    return null;
                }

                string period;
                if (this.Expression.PeriodLength > 0)
                {
                    int rowPeriodLen = this.Expression.PeriodLength;
                    if (this.Expression.PeriodLength + this.Expression.PeriodStart > yearperiod.Length)
                    {
                        rowPeriodLen = yearperiod.Length - this.Expression.PeriodStart;
                    }

                    period = yearperiod.Substring(this.Expression.PeriodStart, rowPeriodLen);
                }
                else
                {
                    period = yearperiod.Substring(this.Expression.PeriodStart);
                }

                this._periodLocalCode.Clear();
                this._periodLocalCode.Add(period);
                CodeCollection periodDsdCode =
                    this.Expression.TranscodingRules.GetDsdCodes(this._periodLocalCode);
                if (periodDsdCode == null)
                {
                    return null; // MAT-495

                    // periodDsdCode = periodLocalCode;
                }

                ret = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", year, periodDsdCode[0]);
            }
            else
            {
                ret = year.ToString(FormatProvider);
            }

            return ret;
        }

        #endregion
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionDateType.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This Time Dimension Transcoding class is used for 1-1 mappings between
//   a Time Dimension and a dissemination column of Date type as generated from DATASET.QUERY
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// This Time Dimension Transcoding class is used for 1-1 mappings between
    /// a Time Dimension and a dissemination column of Date type as generated from DATASET.QUERY
    /// </summary>
    internal class TimeDimensionDateType : TimeDimensionMapping, ITimeDimensionMapping
    {
        #region Constants and Fields

        /// <summary>
        /// Holds the format (for System.String.Format) for EndTime
        /// </summary>
        private readonly string _endWhereFormat;

        /// <summary>
        /// The _result parsing map.
        /// </summary>
        private readonly IDictionary<Type, ParseResult<object>> _resultParsingMap =
            new Dictionary<Type, ParseResult<object>>(4);

        /// <summary>
        /// Holds the format (for System.String.Format) for StartTime
        /// </summary>
        private readonly string _startWhereFormat;

        /// <summary>
        /// The field ordinals
        /// </summary>
        private readonly TimeTranscodingFieldOrdinal _fieldOrdinals;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimensionDateType"/> class. 
        /// Builds a TimeDimensionDateType
        /// </summary>
        /// <param name="mapping">
        /// The mapping of TimeDimension component
        /// </param>
        /// <param name="expression">
        /// The expression that is used in this mapping
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database type
        /// </param>
        public TimeDimensionDateType(MappingEntity mapping, TimeExpressionEntity expression, string databaseType)
            : base(mapping, expression, databaseType)
        {
            string provider = DatabaseType.GetProviderName(databaseType);
            string cast = DatabaseType.DatabaseSettings[provider].DateCast;

            /*
            if (databaseType.Equals(DatabaseType.Mappings.Oracle.Name, StringComparison.OrdinalIgnoreCase)
                || databaseType.Equals(DatabaseType.Mappings.MySql.Name, StringComparison.OrdinalIgnoreCase))
            {
                cast = "DATE";
            }
            */
            var dateColumnSysId = expression.DateColumnSysId;
            string columnName = GetColumnName(mapping, dateColumnSysId);
            this._startWhereFormat = string.Format(
                CultureInfo.InvariantCulture, "({0}>= {2}'{1}')", columnName, "{0}", cast);
            this._endWhereFormat = string.Format(
                CultureInfo.InvariantCulture, "({0}<= {2}'{1}')", columnName, "{0}", cast);

            this._resultParsingMap.Add(typeof(DateTime), x => (DateTime)x);
            this._resultParsingMap.Add(typeof(long), x => new DateTime((long)x));
            this._resultParsingMap.Add(
                typeof(string), 
                x =>
                    {
                        DateTime time;
                        return DateTime.TryParse((string)x, CultureInfo.InvariantCulture, DateTimeStyles.None, out time)
                                   ? DateTime.MinValue
                                   : time;
                    });

            this._fieldOrdinals = new TimeTranscodingFieldOrdinal(mapping, this.Expression);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for methods that parse the result and convert it to <see cref="DateTime"/>
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <typeparam name="T">
        /// The type to try to convert to <see cref="DateTime"/>
        /// </typeparam>
        /// <returns>
        /// A <see cref="DateTime"/> object. If it conversion was not successful it returns <see cref="DateTime.MinValue"/>
        /// </returns>
        private delegate DateTime ParseResult<in T>(T input);

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
            if (dateFrom == null && dateTo == null)
            {
                return string.Empty;
            }

            var sqlArray = new List<string>();
            IFormatProvider fmt = CultureInfo.InvariantCulture;
            
            // DateTime startTime = SdmxPeriodToDateTime(timeBean.StartTime,true);
            if (dateFrom != null)
            {
                var dateTime = dateFrom.Date;
                DateTime startTime = dateTime.HasValue ? dateTime.Value : DateUtil.FormatDate(dateFrom.DateInSdmxFormat, true);
                sqlArray.Add(
                    string.Format(
                        CultureInfo.InvariantCulture, this._startWhereFormat, startTime.ToString("yyyy-MM-dd", fmt)));
            }

            if (dateTo != null)
            {
                // DateTime endTime = SdmxPeriodToDateTime(timeBean.EndTime, false);
                var dateTime = dateTo.Date;
                DateTime endTime = dateTime.HasValue ? dateTime.Value : DateUtil.FormatDate(dateTo.DateInSdmxFormat, true);
                sqlArray.Add(
                    string.Format(
                        CultureInfo.InvariantCulture, this._endWhereFormat, endTime.ToString("yyyy-MM-dd", fmt)));
            }

            return string.Format(CultureInfo.InvariantCulture, "({0})", string.Join(" and ", sqlArray.ToArray()));
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
            string ret = null;
            this._fieldOrdinals.BuildOrdinal(reader);

            object result = reader.GetValue(this._fieldOrdinals.DateOrdinal);
            ParseResult<object> parseResult;
            if (!Convert.IsDBNull(result) && result != null
                && this._resultParsingMap.TryGetValue(result.GetType(), out parseResult))
            {
                DateTime time = parseResult(result);
                if (time != DateTime.MinValue)
                {
                    ret = this.Periodicity.ToString(time);
                }
            }

            return ret;
        }

        #endregion
    }
}
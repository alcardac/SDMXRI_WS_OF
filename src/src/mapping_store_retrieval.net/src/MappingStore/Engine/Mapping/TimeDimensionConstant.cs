// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionConstant.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   DThis Time Dimension Transcoding class is used for Constant mapping
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    /// DThis Time Dimension Transcoding class is used for Constant mapping
    /// </summary>
    public class TimeDimensionConstant : TimeDimensionMapping, ITimeDimensionMapping
    {
        #region Constants and Fields

        /// <summary>
        /// The format template for checking if two year/period are equal
        /// </summary>
        private readonly string _equalsWhere;

        /// <summary>
        /// The format template for the 'from' where clause.
        /// </summary>
        private readonly string _fromWhere;

        /// <summary>
        ///  The format template for the 'to' where clause.
        /// </summary>
        private readonly string _toWhere;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimensionConstant"/> class. 
        /// Initialize an new instance of the TimeDimensionMapping based class
        /// </summary>
        /// <param name="mapping">
        /// The time dimension mapping
        /// </param>
        /// <param name="expression">
        /// It is not used in this implementation 
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor from  DB_CONNECTION.DB_TYPE at Mapping Store database. It is used to determine the substring command to use
        /// </param>
        /// <exception cref="TranscodingException">
        /// It is not used in this implementation
        /// </exception>
        public TimeDimensionConstant(MappingEntity mapping, TimeExpressionEntity expression, string databaseType)
            : base(mapping, expression, databaseType)
        {
            this._fromWhere = string.Format(
                CultureInfo.InvariantCulture, " ( '{0}' >= '{1}' )", mapping.Constant, "{0}");
            this._toWhere = string.Format(CultureInfo.InvariantCulture, " ( '{0}' <= '{1}' )", mapping.Constant, "{0}");
            this._equalsWhere = string.Format(
                CultureInfo.InvariantCulture, " ( '{0}' = '{1}' )", mapping.Constant, "{0}");
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
            if (dateFrom == null && dateTo == null)
            {
                return string.Empty;
            }

            ISdmxDate constantDate = new SdmxDateCore(Mapping.Constant);

            var ret = new StringBuilder("(");
            bool timeWhereStarted = false;
            string normDateFrom = dateFrom != null ? new SdmxDateCore(dateFrom.Date, constantDate.TimeFormatOfDate.EnumType).DateInSdmxFormat : null;
            string normDateTo = dateTo != null ? new SdmxDateCore(dateTo.Date, constantDate.TimeFormatOfDate.EnumType).DateInSdmxFormat : null;
            bool areEqual = Equals(normDateFrom, normDateTo);

            if (normDateFrom != null)
            {
                var startTime = normDateFrom.Replace("'", "''");
                if (areEqual)
                {
                    ret.AppendFormat(this._equalsWhere, startTime);
                }
                else
                {
                    ret.AppendFormat(this._fromWhere, startTime);
                    timeWhereStarted = true;
                }
            }

            if (normDateTo != null && !areEqual)
            {
                if (timeWhereStarted)
                {
                    ret.Append(" and ");
                }

                var endTime = normDateTo.Replace("'", "''");
                ret.AppendFormat(this._toWhere, endTime);
            }

            ret.Append(") ");
            return ret.ToString();
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
            return this.Mapping.Constant;
        }

        #endregion
    }
}
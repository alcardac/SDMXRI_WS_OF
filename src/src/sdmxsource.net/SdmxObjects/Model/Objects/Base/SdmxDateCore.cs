// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxDateCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx sdmxDate core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///   The sdmx sdmxDate core.
    /// </summary>
    [Serializable]
    public class SdmxDateCore : ISdmxDate
    {
        #region Fields

        /// <summary>
        ///   The _date.
        /// </summary>
        private readonly DateTime _date;

        /// <summary>
        ///   The _date in sdmx.
        /// </summary>
        private readonly string _dateInSdmx;

        /// <summary>
        ///   The _time format.
        /// </summary>
        private readonly TimeFormat _timeFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxDateCore"/> class.
        /// </summary>
        /// <param name="date">
        /// The sdmxDate. 
        /// </param>
        /// <param name="timeFormat">
        /// The time format. 
        /// </param>
        /// ///
        /// <exception cref="ArgumentNullException">
        /// Throws ArgumentNullException.
        /// </exception>
        public SdmxDateCore(DateTime? date, TimeFormatEnumType timeFormat)
        {
            if (date == null)
            {
                throw new ArgumentNullException("date");
            }

            this._date = date.Value;
            this._timeFormat = TimeFormat.GetFromEnum(timeFormat);
            this._dateInSdmx = DateUtil.FormatDate(date.Value, timeFormat);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxDateCore"/> class.
        /// </summary>
        /// <param name="dateInSdmx">
        /// The sdmxDate in sdmx. 
        /// </param>
        public SdmxDateCore(string dateInSdmx)
        {
            this._date = DateUtil.FormatDate(dateInSdmx, true);
            this._timeFormat = DateUtil.GetTimeFormatOfDate(dateInSdmx);
            this._dateInSdmx = dateInSdmx;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxDateCore"/> class.
        /// </summary>
        /// <param name="dateInSdmx">
        /// The sdmxDate in sdmx. 
        /// </param>
        public SdmxDateCore(object dateInSdmx)
        {
            if (dateInSdmx == null)
            {
                throw new ArgumentNullException("dateInSdmx");
            }

            this._date = DateUtil.FormatDate(dateInSdmx, true);

            var dateStr = dateInSdmx as string;
            if (dateStr != null)
            {
                this._timeFormat = DateUtil.GetTimeFormatOfDate(dateStr);
                this._dateInSdmx = dateStr;
            }
            else if (dateInSdmx is DateTime)
            {
                this._timeFormat = TimeFormat.GetFromEnum(TimeFormatEnumType.Hour);
                this._dateInSdmx = DateUtil.FormatDate((DateTime)dateInSdmx, TimeFormatEnumType.Hour);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the sdmxDate.
        /// </summary>
        public virtual DateTime? Date
        {
            get
            {
                return this._date;
            }
        }

        /// <summary>
        ///   Gets the sdmxDate in sdmx format.
        /// </summary>
        public virtual string DateInSdmxFormat
        {
            get
            {
                return this._dateInSdmx;
            }
        }

        /// <summary>
        ///   Gets the time format of sdmxDate.
        /// </summary>
        public virtual TimeFormat TimeFormatOfDate
        {
            get
            {
                return this._timeFormat;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The agencySchemeMutable. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as ISdmxDate;
            if (that != null)
            {
                return that.DateInSdmxFormat.Equals(this.DateInSdmxFormat);
            }

            return false;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            return this._dateInSdmx.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The is later.
        /// </summary>
        /// <param name="sdmxDate">
        /// The sdmxDate. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool IsLater(ISdmxDate sdmxDate)
        {
            DateTime? dateTime = this.Date;
            return sdmxDate.Date != null
                   && (dateTime != null && (dateTime.Value.Ticks / 10000) > (sdmxDate.Date.Value.Ticks / 10000));
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///   Contains all the SDMX Text Types
    /// </summary>
    public enum TextEnumType
    {
        /// <summary>
        ///   Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///   The alpha.
        /// </summary>
        Alpha, 

        /// <summary>
        ///   The alpha numeric.
        /// </summary>
        Alphanumeric, 

        /// <summary>
        ///   The attachment constraint reference.
        /// </summary>
        AttachmentConstraintReference, 

        /// <summary>
        ///   The basic time period.
        /// </summary>
        BasicTimePeriod, 

        /// <summary>
        ///   The string.
        /// </summary>
        String, // ALPHA

        /// <summary>
        ///   The big integer.
        /// </summary>
        BigInteger, // NUM

        /// <summary>
        ///   The integer.
        /// </summary>
        Integer, // NUM

        /// <summary>
        ///   The long.
        /// </summary>
        Long, // NUM

        /// <summary>
        ///   The short.
        /// </summary>
        Short, // NUM

        /// <summary>
        ///   The decimal.
        /// </summary>
        Decimal, // NUM

        /// <summary>
        ///   The float.
        /// </summary>
        Float, // NUM

        /// <summary>
        ///   The double.
        /// </summary>
        Double, // NUM

        /// <summary>
        ///   The boolean.
        /// </summary>
        Boolean, 

        /// <summary>
        ///   The date time.
        /// </summary>
        DateTime, 

        /// <summary>
        ///   The date.
        /// </summary>
        Date, 

        /// <summary>
        ///   The time.
        /// </summary>
        Time, 

        /// <summary>
        ///   The year.
        /// </summary>
        Year, 

        /// <summary>
        ///   The month.
        /// </summary>
        Month, 

        /// <summary>
        ///   The numeric.
        /// </summary>
        Numeric, 

        /// <summary>
        ///   The day.
        /// </summary>
        Day, 

        /// <summary>
        ///   The month day.
        /// </summary>
        MonthDay, 

        /// <summary>
        ///   The year month.
        /// </summary>
        YearMonth, 

        /// <summary>
        ///   The duration.
        /// </summary>
        Duration, 

        /// <summary>
        ///   The uri.
        /// </summary>
        Uri, 

        /// <summary>
        ///   The timespan.
        /// </summary>
        Timespan, 

        /// <summary>
        ///   The count.
        /// </summary>
        Count, 

        /// <summary>
        ///   The data set reference.
        /// </summary>
        DataSetReference, 

        /// <summary>
        ///   The inclusive value range.
        /// </summary>
        InclusiveValueRange, 

        /// <summary>
        ///   The exclusive value range.
        /// </summary>
        ExclusiveValueRange, 

        /// <summary>
        ///   The incremental.
        /// </summary>
        Incremental, 

        /// <summary>
        ///   The observational time period.
        /// </summary>
        ObservationalTimePeriod, 

        /// <summary>
        ///   The key values.
        /// </summary>
        KeyValues, 

        /// <summary>
        ///   The time period.
        /// </summary>
        TimePeriod, 

        /// <summary>
        ///   The gregorian day.
        /// </summary>
        GregorianDay, 

        /// <summary>
        ///   The gregorian time period.
        /// </summary>
        GregorianTimePeriod, 

        /// <summary>
        ///   The gregorian year.
        /// </summary>
        GregorianYear, 

        /// <summary>
        ///   The gregorian year month.
        /// </summary>
        GregorianYearMonth, 

        /// <summary>
        ///   The reporting day.
        /// </summary>
        ReportingDay, 

        /// <summary>
        ///   The reporting month.
        /// </summary>
        ReportingMonth, 

        /// <summary>
        ///   The reporting quarter.
        /// </summary>
        ReportingQuarter, 

        /// <summary>
        ///   The reporting semester.
        /// </summary>
        ReportingSemester, 

        /// <summary>
        ///   The reporting time period.
        /// </summary>
        ReportingTimePeriod, 

        /// <summary>
        ///   The reporting trimester.
        /// </summary>
        ReportingTrimester, 

        /// <summary>
        ///   The reporting week.
        /// </summary>
        ReportingWeek, 

        /// <summary>
        ///   The reporting year.
        /// </summary>
        ReportingYear, 

        /// <summary>
        ///   The standard time period.
        /// </summary>
        StandardTimePeriod, 

        /// <summary>
        ///   The times range.
        /// </summary>
        TimesRange, 

        /// <summary>
        ///   The identifiable reference.
        /// </summary>
        IdentifiableReference, 

        /// <summary>
        ///   The xhtml.
        /// </summary>
        Xhtml
    }

    /// <summary>
    ///   The text type.
    /// </summary>
    public class TextType : BaseConstantType<TextEnumType>
    {
        #region Static Fields

        /// <summary>
        ///   The _instances.
        /// </summary>
        private static readonly Dictionary<TextEnumType, TextType> Instances = new Dictionary<TextEnumType, TextType>
            {
                { TextEnumType.Alpha, new TextType(TextEnumType.Alpha, false) }, 
                { TextEnumType.Alphanumeric, new TextType(TextEnumType.Alphanumeric, false) }, 
                {
                    TextEnumType.AttachmentConstraintReference, 
                    new TextType(TextEnumType.AttachmentConstraintReference, false)
                }, 
                { TextEnumType.BasicTimePeriod, new TextType(TextEnumType.BasicTimePeriod, true) }, 
                { TextEnumType.String, new TextType(TextEnumType.String, false) }, 
                {
                    // ALPHA
                    TextEnumType.BigInteger, new TextType(TextEnumType.BigInteger, false)
                }, 
                {
                    // NUM
                    TextEnumType.Integer, new TextType(TextEnumType.Integer, false)
                }, 
                {
                    // NUM
                    TextEnumType.Long, new TextType(TextEnumType.Long, false)
                }, 
                {
                    // NUM
                    TextEnumType.Short, new TextType(TextEnumType.Short, false)
                }, 
                {
                    // NUM
                    TextEnumType.Decimal, new TextType(TextEnumType.Decimal, false)
                }, 
                {
                    // NUM
                    TextEnumType.Float, new TextType(TextEnumType.Float, false)
                }, 
                {
                    // NUM
                    TextEnumType.Double, new TextType(TextEnumType.Double, false)
                }, 
                {
                    // NUM
                    TextEnumType.Boolean, new TextType(TextEnumType.Boolean, false)
                }, 
                { TextEnumType.DateTime, new TextType(TextEnumType.DateTime, true) }, 
                { TextEnumType.Date, new TextType(TextEnumType.Date, false) }, 
                { TextEnumType.Time, new TextType(TextEnumType.Time, false) }, 
                { TextEnumType.Year, new TextType(TextEnumType.Year, false) }, 
                { TextEnumType.Month, new TextType(TextEnumType.Month, false) }, 
                { TextEnumType.Numeric, new TextType(TextEnumType.Numeric, false) }, 
                { TextEnumType.Day, new TextType(TextEnumType.Day, false) }, 
                { TextEnumType.MonthDay, new TextType(TextEnumType.MonthDay, false) }, 
                { TextEnumType.YearMonth, new TextType(TextEnumType.YearMonth, false) }, 
                { TextEnumType.Duration, new TextType(TextEnumType.Duration, false) }, 
                { TextEnumType.Uri, new TextType(TextEnumType.Uri, false) }, 
                { TextEnumType.Timespan, new TextType(TextEnumType.Timespan, false) }, 
                { TextEnumType.Count, new TextType(TextEnumType.Count, false) }, 
                { TextEnumType.DataSetReference, new TextType(TextEnumType.DataSetReference, false) }, 
                { TextEnumType.InclusiveValueRange, new TextType(TextEnumType.InclusiveValueRange, false) }, 
                { TextEnumType.ExclusiveValueRange, new TextType(TextEnumType.ExclusiveValueRange, false) }, 
                { TextEnumType.Incremental, new TextType(TextEnumType.Incremental, false) }, 
                { TextEnumType.ObservationalTimePeriod, new TextType(TextEnumType.ObservationalTimePeriod, true) }, 
                { TextEnumType.KeyValues, new TextType(TextEnumType.KeyValues, false) }, 
                { TextEnumType.TimePeriod, new TextType(TextEnumType.TimePeriod, false) }, 
                { TextEnumType.GregorianDay, new TextType(TextEnumType.GregorianDay, true) }, 
                { TextEnumType.GregorianTimePeriod, new TextType(TextEnumType.GregorianTimePeriod, true) }, 
                { TextEnumType.GregorianYear, new TextType(TextEnumType.GregorianYear, true) }, 
                { TextEnumType.GregorianYearMonth, new TextType(TextEnumType.GregorianYearMonth, true) }, 
                { TextEnumType.ReportingDay, new TextType(TextEnumType.ReportingDay, true) }, 
                { TextEnumType.ReportingMonth, new TextType(TextEnumType.ReportingMonth, true) }, 
                { TextEnumType.ReportingQuarter, new TextType(TextEnumType.ReportingQuarter, true) }, 
                { TextEnumType.ReportingSemester, new TextType(TextEnumType.ReportingSemester, true) }, 
                { TextEnumType.ReportingTimePeriod, new TextType(TextEnumType.ReportingTimePeriod, true) }, 
                { TextEnumType.ReportingTrimester, new TextType(TextEnumType.ReportingTrimester, true) }, 
                { TextEnumType.ReportingWeek, new TextType(TextEnumType.ReportingWeek, true) }, 
                { TextEnumType.ReportingYear, new TextType(TextEnumType.ReportingYear, true) }, 
                { TextEnumType.StandardTimePeriod, new TextType(TextEnumType.StandardTimePeriod, true) }, 
                { TextEnumType.TimesRange, new TextType(TextEnumType.TimesRange, true) }, 
                { TextEnumType.IdentifiableReference, new TextType(TextEnumType.IdentifiableReference, false) }, 
                { TextEnumType.Xhtml, new TextType(TextEnumType.Xhtml, false) }
            };

        #endregion

        #region Fields

        /// <summary>
        ///   The _is valid time dimension text type.
        /// </summary>
        private readonly bool _isValidTimeDimensionTextType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type. 
        /// </param>
        /// <param name="isValidTimeDimensionTextType">
        /// The is valid time dimension text type. 
        /// </param>
        private TextType(TextEnumType enumType, bool isValidTimeDimensionTextType)
            : base(enumType)
        {
            this._isValidTimeDimensionTextType = isValidTimeDimensionTextType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the values.
        /// </summary>
        public static IEnumerable<TextType> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether is valid time dimension text type.
        /// </summary>
        public bool IsValidTimeDimensionTextType
        {
            get
            {
                return this._isValidTimeDimensionTextType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="TextType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type 
        /// </param>
        /// <returns>
        /// the instance of <see cref="TextType"/> mapped to <paramref name="enumType"/> 
        /// </returns>
        public static TextType GetFromEnum(TextEnumType enumType)
        {
            TextType output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data type builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    using TextType = Org.Sdmxsource.Sdmx.Api.Constants.TextType;

    /// <summary>
    ///     The data type builder.
    /// </summary>
    public class DataTypeBuilder : IBuilder<string, TextType>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="Enum"/>.
        /// </returns>
        public virtual string Build(TextType buildFrom)
        {
            switch (buildFrom.EnumType)
            {
                case TextEnumType.Alpha:
                    return DataTypeConstants.Alpha;
                case TextEnumType.Alphanumeric:
                    return DataTypeConstants.AlphaNumeric;
                case TextEnumType.AttachmentConstraintReference:
                    return DataTypeConstants.AttachmentConstraintReference;
                case TextEnumType.String:
                    return DataTypeConstants.String;
                case TextEnumType.BigInteger:
                    return DataTypeConstants.BigInteger;
                case TextEnumType.Integer:
                    return DataTypeConstants.Integer;
                case TextEnumType.Long:
                    return DataTypeConstants.Long;
                case TextEnumType.Short:
                    return DataTypeConstants.Short;
                case TextEnumType.Decimal:
                    return DataTypeConstants.Decimal;
                case TextEnumType.Float:
                    return DataTypeConstants.Float;
                case TextEnumType.Double:
                    return DataTypeConstants.Double;
                case TextEnumType.Boolean:
                    return DataTypeConstants.Boolean;
                case TextEnumType.DateTime:
                    return DataTypeConstants.DateTime;
                case TextEnumType.Time:
                    return DataTypeConstants.Time;
                case TextEnumType.Year:
                    return DataTypeConstants.GregorianYear;
                case TextEnumType.Month:
                    return DataTypeConstants.Month;
                case TextEnumType.Day:
                    return DataTypeConstants.Day;
                case TextEnumType.MonthDay:
                    return DataTypeConstants.MonthDay;
                case TextEnumType.Numeric:
                    return DataTypeConstants.Numeric;
                case TextEnumType.YearMonth:
                    return DataTypeConstants.GregorianYearMonth;
                case TextEnumType.Duration:
                    return DataTypeConstants.Duration;
                case TextEnumType.Uri:
                    return DataTypeConstants.URI;
                case TextEnumType.Timespan:
                    return DataTypeConstants.GregorianTimePeriod;
                case TextEnumType.Count:
                    return DataTypeConstants.Count;
                case TextEnumType.InclusiveValueRange:
                    return DataTypeConstants.InclusiveValueRange;
                case TextEnumType.ExclusiveValueRange:
                    return DataTypeConstants.ExclusiveValueRange;
                case TextEnumType.Incremental:
                    return DataTypeConstants.Incremental;
                case TextEnumType.ObservationalTimePeriod:
                    return DataTypeConstants.ObservationalTimePeriod;
                case TextEnumType.IdentifiableReference:
                    return DataTypeConstants.IdentifiableReference;
                case TextEnumType.Date:
                    return DataTypeConstants.DateTime;
                case TextEnumType.KeyValues:
                    return DataTypeConstants.KeyValues;
                case TextEnumType.BasicTimePeriod:
                    return DataTypeConstants.BasicTimePeriod;
                case TextEnumType.DataSetReference:
                    return DataTypeConstants.DataSetReference;
                case TextEnumType.GregorianDay:
                    return DataTypeConstants.GregorianDay;
                case TextEnumType.GregorianTimePeriod:
                    return DataTypeConstants.GregorianTimePeriod;
                case TextEnumType.GregorianYear:
                    return DataTypeConstants.GregorianYear;
                case TextEnumType.GregorianYearMonth:
                    return DataTypeConstants.GregorianYearMonth;
                case TextEnumType.ReportingDay:
                    return DataTypeConstants.ReportingDay;
                case TextEnumType.ReportingMonth:
                    return DataTypeConstants.ReportingMonth;
                case TextEnumType.ReportingQuarter:
                    return DataTypeConstants.ReportingQuarter;
                case TextEnumType.ReportingSemester:
                    return DataTypeConstants.ReportingSemester;
                case TextEnumType.ReportingTimePeriod:
                    return DataTypeConstants.ReportingTimePeriod;
                case TextEnumType.ReportingTrimester:
                    return DataTypeConstants.ReportingTrimester;
                case TextEnumType.ReportingWeek:
                    return DataTypeConstants.ReportingWeek;
                case TextEnumType.ReportingYear:
                    return DataTypeConstants.ReportingYear;
                case TextEnumType.StandardTimePeriod:
                    return DataTypeConstants.StandardTimePeriod;
                case TextEnumType.TimePeriod:
                    return DataTypeConstants.TimeRange;
                case TextEnumType.TimesRange:
                    return DataTypeConstants.TimeRange;
                case TextEnumType.Xhtml:
                    return DataTypeConstants.XHTML;
            }

            return null;
        }

        #endregion
    }
}
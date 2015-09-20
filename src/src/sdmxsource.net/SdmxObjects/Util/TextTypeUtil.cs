// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTypeUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text type util.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using System.Collections.Generic;
    using System.Threading;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmxsource.Sdmx.Api.Constants.TextType;

    /// <summary>
    ///   The text type util.
    /// </summary>
    public class TextTypeUtil
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TextTypeUtil"/> class from being created. 
        /// </summary>
        private TextTypeUtil()
        {
        }

        #region Public Methods and Operators

        /// <summary>
        /// The get default locale.
        /// </summary>
        /// <param name="textTypes">
        /// The text types. 
        /// </param>
        /// <returns>
        /// The <see cref="ITextTypeWrapper"/> . 
        /// </returns>
        public static ITextTypeWrapper GetDefaultLocale(IList<ITextTypeWrapper> textTypes)
        {
            // FUNC This only returns the first locale
            if (ObjectUtil.ValidCollection(textTypes))
            {
                string twoLetterIsoLanguageName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                ITextTypeWrapper firstLocale = null;
                foreach (ITextTypeWrapper tt in textTypes)
                {
                    if (firstLocale == null || "en".Equals(tt.Locale))
                    {
                        firstLocale = tt;
                    }

                    if (twoLetterIsoLanguageName.Equals(tt.Locale))
                    {
                        return tt;
                    }
                }

                return firstLocale;
            }

            return null;
        }

        /// <summary>
        /// The get text type.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <returns>
        /// The <see cref="Api.Constants.TextType"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static TextType GetTextType(string type)
        {
            if (type == null)
            {
                return default(TextType);
            }

            switch (type)
            {
                case DataTypeConstants.Alpha:
                    return TextType.GetFromEnum(TextEnumType.Alpha);
                case DataTypeConstants.AlphaNumeric:
                    return TextType.GetFromEnum(TextEnumType.Alphanumeric);
                case DataTypeConstants.BasicTimePeriod:
                    return TextType.GetFromEnum(TextEnumType.BasicTimePeriod);
                case DataTypeConstants.BigInteger:
                    return TextType.GetFromEnum(TextEnumType.BigInteger);
                case DataTypeConstants.Boolean:
                    return TextType.GetFromEnum(TextEnumType.Boolean);
                case DataTypeConstants.Count:
                    return TextType.GetFromEnum(TextEnumType.Count);
                case DataTypeConstants.DataSetReference:
                    return TextType.GetFromEnum(TextEnumType.DataSetReference);
                case DataTypeConstants.DateTime:
                    return TextType.GetFromEnum(TextEnumType.DateTime);
                case DataTypeConstants.Day:
                    return TextType.GetFromEnum(TextEnumType.Day);
                case DataTypeConstants.Decimal:
                    return TextType.GetFromEnum(TextEnumType.Decimal);
                case DataTypeConstants.Double:
                    return TextType.GetFromEnum(TextEnumType.Double);
                case DataTypeConstants.Duration:
                    return TextType.GetFromEnum(TextEnumType.Duration);
                case DataTypeConstants.ExclusiveValueRange:
                    return TextType.GetFromEnum(TextEnumType.ExclusiveValueRange);
                case DataTypeConstants.Float:
                    return TextType.GetFromEnum(TextEnumType.Float);
                case DataTypeConstants.GregorianDay:
                    return TextType.GetFromEnum(TextEnumType.GregorianDay);
                case DataTypeConstants.GregorianTimePeriod:
                    return TextType.GetFromEnum(TextEnumType.GregorianTimePeriod);
                case DataTypeConstants.GregorianYear:
                    return TextType.GetFromEnum(TextEnumType.GregorianYear);
                case DataTypeConstants.GregorianYearMonth:
                    return TextType.GetFromEnum(TextEnumType.GregorianYearMonth);
                case DataTypeConstants.IdentifiableReference:
                    return TextType.GetFromEnum(TextEnumType.IdentifiableReference);
                case DataTypeConstants.InclusiveValueRange:
                    return TextType.GetFromEnum(TextEnumType.InclusiveValueRange);
                case DataTypeConstants.Incremental:
                    return TextType.GetFromEnum(TextEnumType.Incremental);
                case DataTypeConstants.Integer:
                    return TextType.GetFromEnum(TextEnumType.Integer);
                case DataTypeConstants.KeyValues:
                    return TextType.GetFromEnum(TextEnumType.KeyValues);
                case DataTypeConstants.Long:
                    return TextType.GetFromEnum(TextEnumType.Long);
                case DataTypeConstants.Month:
                    return TextType.GetFromEnum(TextEnumType.Month);
                case DataTypeConstants.MonthDay:
                    return TextType.GetFromEnum(TextEnumType.MonthDay);
                case DataTypeConstants.Numeric:
                    return TextType.GetFromEnum(TextEnumType.Numeric);
                case DataTypeConstants.ObservationalTimePeriod:
                    return TextType.GetFromEnum(TextEnumType.ObservationalTimePeriod);
                case DataTypeConstants.ReportingDay:
                    return TextType.GetFromEnum(TextEnumType.ReportingDay);
                case DataTypeConstants.ReportingMonth:
                    return TextType.GetFromEnum(TextEnumType.ReportingMonth);
                case DataTypeConstants.ReportingQuarter:
                    return TextType.GetFromEnum(TextEnumType.ReportingQuarter);
                case DataTypeConstants.ReportingSemester:
                    return TextType.GetFromEnum(TextEnumType.ReportingSemester);
                case DataTypeConstants.ReportingTimePeriod:
                    return TextType.GetFromEnum(TextEnumType.ReportingTimePeriod);
                case DataTypeConstants.ReportingTrimester:
                    return TextType.GetFromEnum(TextEnumType.ReportingTrimester);
                case DataTypeConstants.ReportingWeek:
                    return TextType.GetFromEnum(TextEnumType.ReportingWeek);
                case DataTypeConstants.ReportingYear:
                    return TextType.GetFromEnum(TextEnumType.ReportingYear);
                case DataTypeConstants.Short:
                    return TextType.GetFromEnum(TextEnumType.Short);
                case DataTypeConstants.StandardTimePeriod:
                    return TextType.GetFromEnum(TextEnumType.StandardTimePeriod);
                case DataTypeConstants.String:
                    return TextType.GetFromEnum(TextEnumType.String);
                case DataTypeConstants.Time:
                    return TextType.GetFromEnum(TextEnumType.Time);
                case DataTypeConstants.TimeRange:
                    return TextType.GetFromEnum(TextEnumType.TimesRange);
                case DataTypeConstants.URI:
                    return TextType.GetFromEnum(TextEnumType.Uri);
                case DataTypeConstants.XHTML:
                    return TextType.GetFromEnum(TextEnumType.Xhtml);

                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, type);
            }
        }

        /// <summary>
        /// The unwrap text type.
        /// </summary>
        /// <param name="text">
        /// The text. 
        /// </param>
        /// <returns>
        /// The <see cref="TextType"/> array. 
        /// </returns>
        public static Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType[] UnwrapTextType(
            IList<ITextTypeWrapper> text)
        {
            var textType = new Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType[text.Count];
            for (int i = 0; i < text.Count; i++)
            {
                ITextTypeWrapper currentWrapper = text[i];
                var type = new Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType(); // $$$ null ???
                type.lang = currentWrapper.Locale;
                type.TypedValue = currentWrapper.Value;
                textType[i] = type;
            }

            return textType;
        }

        /// <summary>
        /// The wrap text type v 1.
        /// </summary>
        /// <param name="textTypeList">
        /// The text type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{ITextTypeWrapper}"/> . 
        /// </returns>
        public static IList<ITextTypeWrapper> WrapTextTypeV1(
            IEnumerable<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> textTypeList, ISdmxObject parent)
        {
            IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();

            if (textTypeList != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType currentTextType in textTypeList)
                {
                    if (!string.IsNullOrWhiteSpace(currentTextType.TypedValue))
                    {
                        returnList.Add(new TextTypeWrapperImpl(currentTextType, parent));
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// The wrap text type v 2.
        /// </summary>
        /// <param name="textTypeList">
        /// The text type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{ITextTypeWrapper}"/> . 
        /// </returns>
        public static IList<ITextTypeWrapper> WrapTextTypeV2(
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType> textTypeList, ISdmxObject parent)
        {
            IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();

            if (textTypeList != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType currentTextType in textTypeList)
                {
                    if (!string.IsNullOrWhiteSpace(currentTextType.TypedValue))
                    {
                        returnList.Add(new TextTypeWrapperImpl(currentTextType, parent));
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// The wrap text type v 21.
        /// </summary>
        /// <param name="textTypeList">
        /// The text type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <typeparam name="T">Generic type param of type TextType
        /// </typeparam>
        /// <returns>
        /// The <see cref="IList{ITextTypeWrapper}"/> . 
        /// </returns>
        public static IList<ITextTypeWrapper> WrapTextTypeV21<T>(IEnumerable<T> textTypeList, ISdmxObject parent)
            where T : Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType
        {
            IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();

            if (textTypeList != null)
            {
                foreach (T currentTextType in textTypeList)
                {
                    if (!string.IsNullOrWhiteSpace(currentTextType.TypedValue))
                    {
                        returnList.Add(new TextTypeWrapperImpl(currentTextType, parent));
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// The wrap text type v 21.
        /// </summary>
        /// <param name="textTypeList">
        /// The text type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{ITextTypeWrapper}"/> . 
        /// </returns>
        public static IList<ITextTypeWrapper> WrapTextTypeV21(IEnumerable<Name> textTypeList, ISdmxObject parent)
        {
            IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();

            if (textTypeList != null)
            {
                foreach (Name currentTextType in textTypeList)
                {
                    if (!string.IsNullOrWhiteSpace(currentTextType.TypedValue))
                    {
                        returnList.Add(new TextTypeWrapperImpl(currentTextType, parent));
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// The wrap text type v 21.
        /// </summary>
        /// <param name="textTypeList">
        /// The text type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{ITextTypeWrapper}"/> . 
        /// </returns>
        public static IList<ITextTypeWrapper> WrapTextTypeV21(IEnumerable<Description> textTypeList, ISdmxObject parent)
        {
            IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();

            if (textTypeList != null)
            {
                foreach (Description currentTextType in textTypeList)
                {
                    if (!string.IsNullOrWhiteSpace(currentTextType.TypedValue))
                    {
                        returnList.Add(new TextTypeWrapperImpl(currentTextType, parent));
                    }
                }
            }

            return returnList;
        }

        #endregion
    }
}
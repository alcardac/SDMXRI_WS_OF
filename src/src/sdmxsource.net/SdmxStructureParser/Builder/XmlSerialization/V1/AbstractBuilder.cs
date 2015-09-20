// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds Version 1.o SDMX from ISdmx
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Log;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType;

    /// <summary>
    ///     Builds Version 1.o SDMX from ISdmx
    /// </summary>
    public class AbstractBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static ILog log;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the log.
        /// </summary>
        public static ILog Log
        {
            get
            {
                return log;
            }

            set
            {
                log = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the annotations type.
        /// </summary>
        /// <param name="annotable">
        /// The object that can be annotated..
        /// </param>
        /// <returns>
        /// The <see cref="AnnotationsType"/> .
        /// </returns>
        internal AnnotationsType GetAnnotationsType(IAnnotableObject annotable)
        {
            if (!ObjectUtil.ValidCollection(annotable.Annotations))
            {
                return null;
            }

            var returnType = new AnnotationsType();

            /* foreach */
            foreach (IAnnotation currentAnnotationBean in annotable.Annotations)
            {
                var annotation = new AnnotationType();
                returnType.Annotation.Add(annotation);
                annotation.AnnotationText = this.GetTextType(currentAnnotationBean.Text);
                annotation.AnnotationTitle = currentAnnotationBean.Title;
                annotation.AnnotationType1 = currentAnnotationBean.Type;
                if (currentAnnotationBean.Uri != null)
                {
                    annotation.AnnotationURL = currentAnnotationBean.Uri;
                }
            }

            return returnType;
        }

        /// <summary>
        /// Gets the text type from the specified <paramref name="textTypeWrappers"/>
        /// </summary>
        /// <param name="textTypeWrappers">
        /// The text type wrappers list.
        /// </param>
        /// <returns>
        /// the text type array from the specified <paramref name="textTypeWrappers"/>
        /// </returns>
        internal TextType[] GetTextType(IList<ITextTypeWrapper> textTypeWrappers)
        {
            if (!ObjectUtil.ValidCollection(textTypeWrappers))
            {
                return null;
            }

            var textTypes = new TextType[textTypeWrappers.Count];
            for (int i = 0; i < textTypeWrappers.Count; i++)
            {
                TextType tt = this.GetTextType(textTypeWrappers[i]);
                textTypes[i] = tt;
            }

            return textTypes;
        }

        /// <summary>
        /// Gets the text type from the specified <paramref name="textTypeWrapper"/>
        /// </summary>
        /// <param name="textTypeWrapper">
        /// The text type wrapper.
        /// </param>
        /// <returns>
        /// the text type from the specified <paramref name="textTypeWrapper"/>
        /// </returns>
        internal TextType GetTextType(ITextTypeWrapper textTypeWrapper)
        {
            var textType = new TextType { lang = textTypeWrapper.Locale, TypedValue = textTypeWrapper.Value };
            return textType;
        }

        /// <summary>
        /// Check if <paramref name="annotable"/> has annotations.
        /// </summary>
        /// <param name="annotable">
        /// The object that can be annotated.
        /// </param>
        /// <returns>
        /// True if <paramref name="annotable"/> has annotations.
        /// </returns>
        internal bool HasAnnotations(IAnnotableObject annotable)
        {
            if (ObjectUtil.ValidCollection(annotable.Annotations))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The populate text format type.
        /// </summary>
        /// <param name="textFormatType">
        /// The text format type.
        /// </param>
        /// <param name="textFormat">
        /// The text format.
        /// </param>
        internal void PopulateTextFormatType(TextFormatType textFormatType, ITextFormat textFormat)
        {
            if (textFormat.MaxLength != null)
            {
                textFormatType.length = textFormat.MaxLength;
            }

            if (textFormat.Decimals != null)
            {
                textFormatType.decimals = textFormat.Decimals;
            }

            if (textFormat.TextType != null)
            {
                switch (textFormat.TextType.EnumType)
                {
                    case TextEnumType.BigInteger:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.Boolean:
                        LoggingUtil.Warn(
                            log, "can not map text type BOOLEAN to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Count:
                        LoggingUtil.Warn(
                            log, "can not map text type COUNT to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Date:
                        LoggingUtil.Warn(
                            log, "can not map text type DATE to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.DateTime:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type DATE TIME to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Day:
                        LoggingUtil.Warn(
                            log, "can not map text type DAY to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Decimal:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.Double:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.Duration:
                        LoggingUtil.Warn(
                            log, "can not map text type DURATION to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.ExclusiveValueRange:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type EXCLUSIVE VALUE RANGE to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Float:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.InclusiveValueRange:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type INCLUSIVE VALUE RANGE to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Incremental:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type INCREMENTAL to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Long:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.Month:
                        LoggingUtil.Warn(
                            log, "can not map text type MONTH to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.MonthDay:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type MONTH DAY to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.ObservationalTimePeriod:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type OBSERVATIONAL TIME PERIOD to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Short:
                        textFormatType.TextType = TextTypeConstants.Num;
                        break;
                    case TextEnumType.String:
                        textFormatType.TextType = TextTypeConstants.Alpha;
                        break;
                    case TextEnumType.Time:
                        LoggingUtil.Warn(
                            log, "can not map text type TIME to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Timespan:
                        LoggingUtil.Warn(
                            log, "can not map text type TIMESPAN to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Uri:
                        LoggingUtil.Warn(
                            log, "can not map text type URI to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.Year:
                        LoggingUtil.Warn(
                            log, "can not map text type YEAR to SDMX version 1.0 schema instance, property ignored ");
                        break;
                    case TextEnumType.YearMonth:
                        LoggingUtil.Warn(
                            log, 
                            "can not map text type YEAR MONTH to SDMX version 1.0 schema instance, property ignored ");
                        break;
                }
            }
        }

        #endregion
    }
}
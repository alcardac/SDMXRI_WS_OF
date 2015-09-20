// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    using DataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.DataflowRefType;
    using DataProviderRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.DataProviderRefType;
    using MetadataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.MetadataflowRefType;
    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///     The abstract builder.
    /// </summary>
    public class AbstractBuilder
    {
        #region Constants

        /// <summary>
        ///     The default language.
        /// </summary>
        public const string DefaultLang = "en";

        /// <summary>
        ///     The default text value.
        /// </summary>
        public const string DefaultTextValue = "Undefined";

        #endregion

        #region Methods

        /// <summary>
        /// Gets annotations type.
        /// </summary>
        /// <param name="annotable">
        /// The annotable.
        /// </param>
        /// <returns>
        /// The <see cref="AnnotationsType"/> .
        /// </returns>
        internal AnnotationsType GetAnnotationsType(IAnnotableObject annotable)
        {
            IList<IAnnotation> currentAnnotationBeans = annotable.Annotations;
            if (!ObjectUtil.ValidCollection(currentAnnotationBeans))
            {
                return null;
            }

            var returnType = new AnnotationsType();

            /* foreach */
            foreach (IAnnotation currentAnnotationBean in currentAnnotationBeans)
            {
                var annotation = new AnnotationType();
                returnType.Annotation.Add(annotation);
                IList<ITextTypeWrapper> text = currentAnnotationBean.Text;
                if (ObjectUtil.ValidCollection(text))
                {
                    annotation.AnnotationText = this.GetTextType(text);
                }

                string value = currentAnnotationBean.Title;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    annotation.AnnotationTitle = currentAnnotationBean.Title;
                }

                string value1 = currentAnnotationBean.Type;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    annotation.AnnotationType1 = currentAnnotationBean.Type;
                }

                if (currentAnnotationBean.Uri != null)
                {
                    annotation.AnnotationURL = currentAnnotationBean.Uri;
                }
            }

            return returnType;
        }

        /// <summary>
        /// Gets text type.
        /// </summary>
        /// <param name="textTypeWrappers">
        /// The text type wrappers.
        /// </param>
        /// <returns>
        /// The array of <see cref="TextType"/> .
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
        /// Gets the text type from <paramref name="textTypeWrapper"/>
        /// </summary>
        /// <param name="textTypeWrapper">
        /// The text Type wrapper.
        /// </param>
        /// <returns>
        /// The <see cref="TextType"/> from <paramref name="textTypeWrapper"/> .
        /// </returns>
        internal TextType GetTextType(ITextTypeWrapper textTypeWrapper)
        {
            var tt = new TextType { lang = textTypeWrapper.Locale, TypedValue = textTypeWrapper.Value };
            return tt;
        }

        /// <summary>
        /// Gets text type.
        /// </summary>
        /// <param name="englishString">
        /// The english string.
        /// </param>
        /// <returns>
        /// The <see cref="TextType"/> .
        /// </returns>
        internal TextType GetTextType(string englishString)
        {
            var tt = new TextType { lang = DefaultLang, TypedValue = englishString };
            return tt;
        }

        /// <summary>
        /// Returns true if <paramref name="annotable"/> has annotations.
        /// </summary>
        /// <param name="annotable">
        /// The annotable.
        /// </param>
        /// <returns>
        /// True if <paramref name="annotable"/> has annotations; otherwise false
        /// </returns>
        internal bool HasAnnotations(IAnnotableObject annotable)
        {
            IList<IAnnotation> annotations = annotable.Annotations;
            if (ObjectUtil.ValidCollection(annotations))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Populate the <paramref name="dataflowRef"/>  from <paramref name="crossReference"/>
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <param name="dataflowRef">
        /// The reference to populate
        /// </param>
        internal void PopulateDataflowRef(ICrossReference crossReference, DataflowRefType dataflowRef)
        {
            IMaintainableRefObject maintRef = crossReference.MaintainableReference;
            dataflowRef.URN = crossReference.TargetUrn;

            string str1 = maintRef.AgencyId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                dataflowRef.AgencyID = maintRef.AgencyId;
            }

            string str2 = maintRef.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                dataflowRef.DataflowID = maintRef.MaintainableId;
            }

            string str3 = maintRef.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                dataflowRef.Version = maintRef.Version;
            }
        }

        /// <summary>
        /// Populate the <paramref name="dataProviderRef"/>  from <paramref name="crossReference"/>
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <param name="dataProviderRef">
        /// The data Provider reference.
        /// </param>
        internal void PopulateDataproviderRef(ICrossReference crossReference, DataProviderRefType dataProviderRef)
        {
            IMaintainableRefObject maintRef = crossReference.MaintainableReference;
            {
                dataProviderRef.URN = crossReference.TargetUrn;
            }

            string str1 = maintRef.AgencyId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                dataProviderRef.OrganisationSchemeAgencyID = maintRef.AgencyId;
            }

            string str2 = maintRef.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                dataProviderRef.OrganisationSchemeID = maintRef.MaintainableId;
            }

            string str3 = maintRef.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                dataProviderRef.Version = maintRef.Version;
            }

            string str4 = crossReference.ChildReference.Id;
            if (!string.IsNullOrWhiteSpace(str4))
            {
                dataProviderRef.DataProviderID = crossReference.ChildReference.Id;
            }
        }

        /// <summary>
        /// Populate the <paramref name="metadataflowRef"/>  from <paramref name="crossReference"/>
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <param name="metadataflowRef">
        /// The metadataflow reference.
        /// </param>
        internal void PopulateMetadataflowRef(ICrossReference crossReference, MetadataflowRefType metadataflowRef)
        {
            IMaintainableRefObject maintRef = crossReference.MaintainableReference;
            {
                metadataflowRef.URN = crossReference.TargetUrn;
            }

            string str1 = maintRef.AgencyId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                metadataflowRef.AgencyID = maintRef.AgencyId;
            }

            string str2 = maintRef.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                metadataflowRef.MetadataflowID = maintRef.MaintainableId;
            }

            string str3 = maintRef.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                metadataflowRef.Version = maintRef.Version;
            }
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
            if (textFormat.TextType != null)
            {
                switch (textFormat.TextType.EnumType)
                {
                    case TextEnumType.BigInteger:
                        textFormatType.textType = TextTypeTypeConstants.BigInteger;
                        break;
                    case TextEnumType.Boolean:
                        textFormatType.textType = TextTypeTypeConstants.Boolean;
                        break;
                    case TextEnumType.Count:
                        textFormatType.textType = TextTypeTypeConstants.Count;
                        break;
                    case TextEnumType.Date:
                        textFormatType.textType = TextTypeTypeConstants.Date;
                        break;
                    case TextEnumType.DateTime:
                        textFormatType.textType = TextTypeTypeConstants.DateTime;
                        break;
                    case TextEnumType.Day:
                        textFormatType.textType = TextTypeTypeConstants.Day;
                        break;
                    case TextEnumType.Decimal:
                        textFormatType.textType = TextTypeTypeConstants.Decimal;
                        break;
                    case TextEnumType.Double:
                        textFormatType.textType = TextTypeTypeConstants.Double;
                        break;
                    case TextEnumType.Duration:
                        textFormatType.textType = TextTypeTypeConstants.Duration;
                        break;
                    case TextEnumType.ExclusiveValueRange:
                        textFormatType.textType = TextTypeTypeConstants.ExclusiveValueRange;
                        break;
                    case TextEnumType.Float:
                        textFormatType.textType = TextTypeTypeConstants.Float;
                        break;
                    case TextEnumType.InclusiveValueRange:
                        textFormatType.textType = TextTypeTypeConstants.InclusiveValueRange;
                        break;
                    case TextEnumType.Incremental:
                        textFormatType.textType = TextTypeTypeConstants.Incremental;
                        break;
                    case TextEnumType.Integer:
                        textFormatType.textType = TextTypeTypeConstants.Integer;
                        break;
                    case TextEnumType.Long:
                        textFormatType.textType = TextTypeTypeConstants.Long;
                        break;
                    case TextEnumType.Month:
                        textFormatType.textType = TextTypeTypeConstants.Month;
                        break;
                    case TextEnumType.MonthDay:
                        textFormatType.textType = TextTypeTypeConstants.MonthDay;
                        break;
                    case TextEnumType.ObservationalTimePeriod:
                        textFormatType.textType = TextTypeTypeConstants.ObservationalTimePeriod;
                        break;
                    case TextEnumType.Short:
                        textFormatType.textType = TextTypeTypeConstants.Short;
                        break;
                    case TextEnumType.String:
                        textFormatType.textType = TextTypeTypeConstants.String;
                        break;
                    case TextEnumType.Time:
                        textFormatType.textType = TextTypeTypeConstants.Time;
                        break;
                    case TextEnumType.Timespan:
                        textFormatType.textType = TextTypeTypeConstants.Timespan;
                        break;
                    case TextEnumType.Uri:
                        textFormatType.textType = TextTypeTypeConstants.URI;
                        break;
                    case TextEnumType.Year:
                        textFormatType.textType = TextTypeTypeConstants.Year;
                        break;
                    case TextEnumType.YearMonth:
                        textFormatType.textType = TextTypeTypeConstants.YearMonth;
                        break;
                }
            }

            string str0 = textFormat.Pattern;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                textFormatType.pattern = textFormat.Pattern;
            }

            TimeSpan result;
            string str1 = textFormat.TimeInterval;
            if (!string.IsNullOrWhiteSpace(str1) && TimeSpan.TryParse(textFormat.TimeInterval, out result))
            {
                textFormatType.timeInterval = result;
            }

            if (textFormat.Decimals != null)
            {
                textFormatType.decimals = textFormat.Decimals;
            }

            if (textFormat.EndValue != null)
            {
                textFormatType.endValue = Convert.ToDouble(textFormat.EndValue);
            }

            if (textFormat.Interval != null)
            {
                textFormatType.interval = Convert.ToDouble(textFormat.Interval);
            }

            if (textFormat.MaxLength != null)
            {
                textFormatType.maxLength = textFormat.MaxLength;
            }

            if (textFormat.MinLength != null)
            {
                textFormatType.minLength = textFormat.MinLength;
            }

            if (textFormat.StartValue != null)
            {
                textFormatType.startValue = Convert.ToDouble(textFormat.StartValue);
            }

            if (textFormat.Sequence.IsSet())
            {
                textFormatType.isSequence = textFormat.Sequence.IsTrue;
            }
        }

        /// <summary>
        /// Sets the default text.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        protected void SetDefaultText(TextType value)
        {
            value.lang = DefaultLang;
            value.TypedValue = DefaultTextValue;
        }

        #endregion
    }
}
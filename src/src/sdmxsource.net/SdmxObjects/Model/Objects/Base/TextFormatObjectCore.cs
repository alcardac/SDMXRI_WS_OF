// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFormatBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text format object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Text.RegularExpressions;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;
    using System.Collections.Generic;

    using TextFormatType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.TextFormatType;

    /// <summary>
    ///   The text format dataStructureObject core.
    /// </summary>
    [Serializable]
    public class TextFormatObjectCore : SdmxObjectCore, ITextFormat
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(TextFormatObjectCore));

        /// <summary>
        ///   The _text format type.
        /// </summary>
        private static readonly SdmxStructureType _textFormatType =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextFormat);

        /// <summary>
        ///   The _time interval regex.
        /// </summary>
        private static readonly Regex _timeIntervalRegex =
            new Regex("^P(([0-9]+Y)?([0-9]+M)?([0-9]+D)?)(T([0-9]+H)?([0-9]+M)?([0-9]+S)?)?$", RegexOptions.Compiled);

        #endregion

        #region Fields

        /// <summary>
        ///   The _decimals.
        /// </summary>
        private readonly long? _decimals;

        /// <summary>
        ///   The _end time.
        /// </summary>
        private readonly ISdmxDate _endTime;

        /// <summary>
        ///   The _end value.
        /// </summary>
        private readonly decimal? _endValue;

        /// <summary>
        ///   The _interval.
        /// </summary>
        private readonly decimal? _interval;

        /// <summary>
        ///   The _is multi lingual.
        /// </summary>
        private readonly TertiaryBool multilingual;

        /// <summary>
        ///   The _is sequence.
        /// </summary>
        private readonly TertiaryBool _isSequence;

        /// <summary>
        ///   The _max length.
        /// </summary>
        private readonly long? _maxLength;

        /// <summary>
        ///   The _max value.
        /// </summary>
        private readonly decimal? _maxValue;

        /// <summary>
        ///   The _min value.
        /// </summary>
        private readonly decimal? _minValue;

        /// <summary>
        ///   The _pattern.
        /// </summary>
        private readonly string _pattern;

        /// <summary>
        ///   The _start time.
        /// </summary>
        private readonly ISdmxDate _startTime;

        /// <summary>
        ///   The _start value.
        /// </summary>
        private readonly decimal? _startValue;

        /// <summary>
        ///   The _text type.
        /// </summary>
        private readonly TextType _textType;

        /// <summary>
        ///   The _time interval.
        /// </summary>
        private readonly string _timeInterval;

        /// <summary>
        ///   The _min length.
        /// </summary>
        private long? _minLength;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatObjectCore"/> class.
        /// </summary>
        /// <param name="textFormatMutable">
        /// The text format mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextFormatObjectCore(ITextFormatMutableObject textFormatMutable, ISdmxObject parent)
            : base(textFormatMutable, parent)
        {
            this._isSequence = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this.multilingual = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._textType = textFormatMutable.TextType;
            if (textFormatMutable.Sequence != null)
            {
                this._isSequence = textFormatMutable.Sequence;
            }

            this._maxLength = textFormatMutable.MaxLength;
            this._minLength = textFormatMutable.MinLength;
            this._startValue = textFormatMutable.StartValue;
            this._endValue = textFormatMutable.EndValue;
            this._maxValue = textFormatMutable.MaxValue;
            this._minValue = textFormatMutable.MinValue;
            this._interval = textFormatMutable.Interval;
            this._timeInterval = textFormatMutable.TimeInterval;
            this._decimals = textFormatMutable.Decimals;
            this._pattern = textFormatMutable.Pattern;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatObjectCore"/> class.
        /// </summary>
        /// <param name="textFormatType">
        /// The text format type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextFormatObjectCore(TextFormatType textFormatType, ISdmxObject parent)
            : base(_textFormatType, parent)
        {
            this._isSequence = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this.multilingual = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            if (textFormatType.textType != null)
            {
                this._textType = TextTypeUtil.GetTextType(textFormatType.textType);
            }

            if (textFormatType.isMultiLingual)
            {
                this.multilingual = TertiaryBool.ParseBoolean(textFormatType.isMultiLingual);
            }

            if (textFormatType.isSequence != null && textFormatType.isSequence.Value)
            {
                this._isSequence = TertiaryBool.ParseBoolean(textFormatType.isSequence.Value);
            }

            if (textFormatType.maxLength.HasValue)
            {
                this._maxLength = (long?)textFormatType.maxLength;
            }

            if (textFormatType.minLength != null)
            {
                this._minLength = (long?)textFormatType.minLength;
            }

            if (textFormatType.startValue.HasValue)
            {
                this._startValue = textFormatType.startValue.Value;
            }

            if (textFormatType.endValue.HasValue)
            {
                this._endValue = textFormatType.endValue.Value;
            }

            if (textFormatType.maxValue.HasValue)
            {
                this._maxValue = textFormatType.maxValue.Value;
            }

            if (textFormatType.minValue.HasValue)
            {
                this._minValue = (long)textFormatType.minValue.Value;
            }

            if (textFormatType.interval.HasValue)
            {
                this._interval = textFormatType.interval.Value;
            }

            if (textFormatType.timeInterval != null)
            {
                this._timeInterval = textFormatType.timeInterval.ToString();
            }

            if (textFormatType.decimals.HasValue)
            {
                this._decimals = (long)textFormatType.decimals.Value;
            }

            if (!string.IsNullOrEmpty(textFormatType.pattern))
            {
                this._pattern = textFormatType.pattern;
            }

            if (textFormatType.endTime != null)
            {
                this._endTime = new SdmxDateCore(textFormatType.endTime.ToString());
            }

            if (textFormatType.startTime != null)
            {
                this._startTime = new SdmxDateCore(textFormatType.startTime.ToString());
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatObjectCore"/> class.
        /// </summary>
        /// <param name="textFormat">
        /// The text format. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextFormatObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TextFormatType textFormat, ISdmxObject parent)
            : base(_textFormatType, parent)
        {
            this._isSequence = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this.multilingual = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            if (textFormat.textType != null)
            {
                string textType = textFormat.textType;
                switch (textType)
                {
                        // TODO java 0.9.9 bug. No Integer case
                        // http://www.metadatatechnology.com/mantis/view.php?id=1425
                    case TextTypeTypeConstants.BigInteger:
                        this._textType = TextType.GetFromEnum(TextEnumType.BigInteger);
                        break;
                    case TextTypeTypeConstants.Boolean:
                        this._textType = TextType.GetFromEnum(TextEnumType.Boolean);
                        break;
                    case TextTypeTypeConstants.Count:
                        this._textType = TextType.GetFromEnum(TextEnumType.Count);
                        break;
                    case TextTypeTypeConstants.Date:
                        this._textType = TextType.GetFromEnum(TextEnumType.Date);
                        break;
                    case TextTypeTypeConstants.DateTime:
                        this._textType = TextType.GetFromEnum(TextEnumType.DateTime);
                        break;
                    case TextTypeTypeConstants.Day:
                        this._textType = TextType.GetFromEnum(TextEnumType.Day);
                        break;
                    case TextTypeTypeConstants.Decimal:
                        this._textType = TextType.GetFromEnum(TextEnumType.Decimal);
                        break;
                    case TextTypeTypeConstants.Double:
                        this._textType = TextType.GetFromEnum(TextEnumType.Double);
                        break;
                    case TextTypeTypeConstants.Duration:
                        this._textType = TextType.GetFromEnum(TextEnumType.Duration);
                        break;
                    case TextTypeTypeConstants.ExclusiveValueRange:
                        this._textType = TextType.GetFromEnum(TextEnumType.ExclusiveValueRange);
                        break;
                    case TextTypeTypeConstants.Float:
                        this._textType = TextType.GetFromEnum(TextEnumType.Float);
                        break;
                    case TextTypeTypeConstants.InclusiveValueRange:
                        this._textType = TextType.GetFromEnum(TextEnumType.InclusiveValueRange);
                        break;
                    case TextTypeTypeConstants.Incremental:
                        this._textType = TextType.GetFromEnum(TextEnumType.Incremental);
                        break;
                    case TextTypeTypeConstants.Integer:
                        this._textType = TextType.GetFromEnum(TextEnumType.Integer);
                        break;
                    case TextTypeTypeConstants.Long:
                        this._textType = TextType.GetFromEnum(TextEnumType.Long);
                        break;
                    case TextTypeTypeConstants.Month:
                        this._textType = TextType.GetFromEnum(TextEnumType.Month);
                        break;
                    case TextTypeTypeConstants.MonthDay:
                        this._textType = TextType.GetFromEnum(TextEnumType.MonthDay);
                        break;
                    case TextTypeTypeConstants.ObservationalTimePeriod:
                        this._textType = TextType.GetFromEnum(TextEnumType.ObservationalTimePeriod);
                        break;
                    case TextTypeTypeConstants.Short:
                        this._textType = TextType.GetFromEnum(TextEnumType.Short);
                        break;
                    case TextTypeTypeConstants.String:
                        this._textType = TextType.GetFromEnum(TextEnumType.String);
                        break;
                    case TextTypeTypeConstants.Time:
                        this._textType = TextType.GetFromEnum(TextEnumType.Time);
                        break;
                    case TextTypeTypeConstants.Timespan:
                        this._textType = TextType.GetFromEnum(TextEnumType.Timespan);
                        break;
                    case TextTypeTypeConstants.URI:
                        this._textType = TextType.GetFromEnum(TextEnumType.Uri);
                        break;
                    case TextTypeTypeConstants.Year:
                        this._textType = TextType.GetFromEnum(TextEnumType.Year);
                        break;
                    case TextTypeTypeConstants.YearMonth:
                        this._textType = TextType.GetFromEnum(TextEnumType.YearMonth);
                        break;
                }

                if (textFormat.isSequence.HasValue)
                {
                    this._isSequence = TertiaryBool.ParseBoolean(textFormat.isSequence);
                }

                if (textFormat.maxLength.HasValue)
                {
                    this._maxLength = (long?)textFormat.maxLength;
                }

                if (textFormat.minLength.HasValue)
                {
                    this._minLength = (long?)textFormat.minLength;
                }

                if (textFormat.startValue.HasValue)
                {
                    this._startValue = (decimal)textFormat.startValue.Value;
                }

                if (textFormat.endValue.HasValue)
                {
                    this._endValue = new decimal(textFormat.endValue.Value);
                }

                if (textFormat.interval.HasValue)
                {
                    this._interval = new decimal(textFormat.interval.Value);
                }

                if (textFormat.timeInterval != null)
                {
                    this._timeInterval = textFormat.timeInterval.ToString();
                }

                if (textFormat.decimals.HasValue)
                {
                    this._decimals = (long)textFormat.decimals.Value;
                }

                if (!string.IsNullOrEmpty(textFormat.pattern))
                {
                    this._pattern = textFormat.pattern;
                }
            }

            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the decimals.
        /// </summary>
        public virtual long? Decimals
        {
            get
            {
                return this._decimals;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the end time.
        /// </summary>
        public virtual ISdmxDate EndTime
        {
            get
            {
                return this._endTime;
            }
        }

        /// <summary>
        ///   Gets the end value.
        /// </summary>
        public decimal? EndValue
        {
            get
            {
                return this._endValue;
            }
        }

        /// <summary>
        ///   Gets the interval.
        /// </summary>
        public decimal? Interval
        {
            get
            {
                return this._interval;
            }
        }

        /// <summary>
        ///   Gets the is multi lingual.
        /// </summary>
        public TertiaryBool Multilingual
        {
            get
            {
                return this.multilingual;
            }
        }

        /// <summary>
        ///   Gets the max length.
        /// </summary>
        public long? MaxLength
        {
            get
            {
                return this._maxLength;
            }
        }

        /// <summary>
        ///   Gets the max value.
        /// </summary>
        public decimal? MaxValue
        {
            get
            {
                return this._maxValue;
            }
        }

        /// <summary>
        ///   Gets the min length.
        /// </summary>
        public long? MinLength
        {
            get
            {
                return this._minLength;
            }
        }

        /// <summary>
        ///   Gets the min value.
        /// </summary>
        public decimal? MinValue
        {
            get
            {
                return this._minValue;
            }
        }

        /// <summary>
        ///   Gets the pattern.
        /// </summary>
        public string Pattern
        {
            get
            {
                return this._pattern;
            }
        }

        /// <summary>
        ///   Gets the start time.
        /// </summary>
        public virtual ISdmxDate StartTime
        {
            get
            {
                return this._startTime;
            }
        }

        /// <summary>
        ///   Gets the start value.
        /// </summary>
        public decimal? StartValue
        {
            get
            {
                return this._startValue;
            }
        }

        /// <summary>
        ///   Gets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this._textType;
            }
        }

        /// <summary>
        ///   Gets the time interval.
        /// </summary>
        public string TimeInterval
        {
            get
            {
                return this._timeInterval;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The dataStructureObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if(sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ITextFormat)sdmxObject;
                if (!ObjectUtil.Equivalent(this._textType, that.TextType))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._isSequence, that.Sequence))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.multilingual, that.Multilingual))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._minLength, that.MinLength))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._maxLength, that.MaxLength))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._minValue, that.MinValue))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._maxValue, that.MaxValue))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._startValue, that.StartValue))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._endValue, that.EndValue))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._interval, that.Interval))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._timeInterval, that.TimeInterval))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._decimals, that.Decimals))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._pattern, that.Pattern))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._startTime, that.StartTime))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._endTime, that.EndTime))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///   The is sequence.
        /// </summary>
        /// <value> The &lt; see cref= &quot; TertiaryBool &quot; / &gt; . </value>
        public virtual TertiaryBool Sequence
        {
            get
            {
                return this._isSequence;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this._minLength != null)
            {
                if (Convert.ToInt32(this._minLength) == 0)
                {
                    LOG.Warn("Text format of 0 converted to 1");
                    this._minLength = 1;
                }
                else if (Convert.ToInt32(this._minLength) < 0)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, min length must be a positive integer - got "
                        + Convert.ToInt32(this._minLength));
                }
            }

            if (this._maxLength != null)
            {
                if (Convert.ToInt32(this._maxLength) <= 0)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, max length must be a positive integer - got "
                        + Convert.ToInt32(this._maxLength));
                }
            }

            if (this._minLength != null && this._maxLength != null)
            {
                if (this._minLength.Value.CompareTo(this._maxLength) > 0)
                {
                    throw new SdmxSemmanticException("Invalid Text Format, min length can not be greater then max length");
                }
            }

            if (this._minValue != null && this._maxValue != null)
            {
                if (this._minValue.Value.CompareTo(this._maxValue) > 0)
                {
                    throw new SdmxSemmanticException("Invalid Text Format, min value can not be greater then max value");
                }
            }

            if (this._decimals != null)
            {
                if (Convert.ToInt32(this._decimals) <= 0)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, decimals must be a positive integer - got "
                        + Convert.ToInt32(this._decimals));
                }
            }

            if (this._startTime != null && this._endTime != null)
            {
                if (this._startTime.IsLater(this._endTime))
                {
                    throw new SdmxSemmanticException("Invalid Text Format, start time can not be after end time");
                }
            }

            if (this._isSequence.IsTrue)
            {
                if (this._timeInterval == null && this._interval == null)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, time interval or interval must be set if isSequence is set to true");
                }

                if (!string.IsNullOrWhiteSpace(this._timeInterval) && this._startTime == null)
                {
                    throw new SdmxSemmanticException("Invalid Text Format, start time must be set if time interval is set");
                }

                if (this._interval != null && this._startValue == null)
                {
                    throw new SdmxSemmanticException("Invalid Text Format, start value must be set if interval is set");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this._timeInterval))
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, time interval can only be set if isSequence is set to true");
                }

                if (this._startTime != null)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, start time can only be set if isSequence is set to true");
                }

                if (this._interval != null)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, interval can only be set if isSequence is set to true");
                }

                if (this._startValue != null)
                {
                    throw new SdmxSemmanticException(
                        "Invalid Text Format, start value can only be set if isSequence is set to true");
                }
            }

            if (!string.IsNullOrWhiteSpace(this._timeInterval))
            {
                // Validate that the time interval matches the allowed xs:duration format PnYnMnDTnHnMnS - Use the RegEx, and make sure the string
                // is greater then length 1, as the regex does not ensure that there is any content after the P
                // The Regex ensures if there is anything after the P, it is of the valid type, example P5Y and P91DT12M are both valid formats
                // Pattern timeIntervalPattern = ILOG.J2CsMapping.Text.Pattern.Compile("P(([0-9]+Y)?([0-9]+M)?([0-9]+D)?)(T([0-9]+H)?([0-9]+M)?([0-9]+S)?)?");
                if (this._timeInterval.Length == 1 || !_timeIntervalRegex.IsMatch(this._timeInterval))
                {
                    throw new SdmxSemmanticException(
                        "Invalid time interval, pattern must be PnYnMnDTnHnMnS, where n=positive integer, and each section is optional after each n (example P5Y)");
                }
            }
        }

        /// <summary>
        ///   The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            return new HashSet<ISdmxObject>();
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFormatMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text format mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The text format mutable core.
    /// </summary>
    [Serializable]
    public class TextFormatMutableCore : MutableCore, ITextFormatMutableObject
    {
        #region Fields

        /// <summary>
        ///   The decimals.
        /// </summary>
        private long? decimals;

        /// <summary>
        ///   The end value.
        /// </summary>
        private decimal? endValue;

        /// <summary>
        ///   The interval.
        /// </summary>
        private decimal? interval;

        /// <summary>
        ///   The is multi lingual.
        /// </summary>
        private TertiaryBool isMultilingual = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);

        /// <summary>
        ///   The is sequence.
        /// </summary>
        private TertiaryBool isSequence = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);

        /// <summary>
        ///   The max length.
        /// </summary>
        private long? maxLength;

        /// <summary>
        ///   The max value.
        /// </summary>
        private decimal? maxValue;

        /// <summary>
        ///   The min length.
        /// </summary>
        private long? minLength;

        /// <summary>
        ///   The min value.
        /// </summary>
        private decimal? minValue;

        /// <summary>
        ///   The pattern.
        /// </summary>
        private string pattern;

        /// <summary>
        ///   The start value.
        /// </summary>
        private decimal? startValue;

        /// <summary>
        ///   The text type.
        /// </summary>
        private TextType textType;

        /// <summary>
        ///   The time interval.
        /// </summary>
        private string timeInterval;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TextFormatMutableCore" /> class.
        /// </summary>
        public TextFormatMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextFormat))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatMutableCore"/> class.
        /// </summary>
        /// <param name="textFormat">
        /// The itxt. 
        /// </param>
        public TextFormatMutableCore(ITextFormat textFormat)
            : base(textFormat)
        {
            this.textType = textFormat.TextType;
            this.isSequence = textFormat.Sequence;
            this.maxLength = textFormat.MaxLength;
            this.minLength = textFormat.MinLength;
            this.startValue = textFormat.StartValue;
            this.endValue = textFormat.EndValue;
            this.interval = textFormat.Interval;
            this.maxValue = textFormat.MaxValue;
            this.minValue = textFormat.MinValue;
            this.isMultilingual = textFormat.Multilingual;
            if (textFormat.TimeInterval != null)
            {
                this.timeInterval = textFormat.TimeInterval;
            }

            this.decimals = textFormat.Decimals;
            this.pattern = textFormat.Pattern;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the decimals.
        /// </summary>
        public virtual long? Decimals
        {
            get
            {
                return this.decimals;
            }

            set
            {
                this.decimals = value;
            }
        }

        /// <summary>
        ///   Gets or sets the end value.
        /// </summary>
        public virtual decimal? EndValue
        {
            get
            {
                return this.endValue;
            }

            set
            {
                this.endValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the interval.
        /// </summary>
        public virtual decimal? Interval
        {
            get
            {
                return this.interval;
            }

            set
            {
                this.interval = value;
            }
        }

        /// <summary>
        ///   Gets or sets the max length.
        /// </summary>
        public virtual long? MaxLength
        {
            get
            {
                return this.maxLength;
            }

            set
            {
                this.maxLength = value;
            }
        }

        /// <summary>
        ///   Gets or sets the max value.
        /// </summary>
        public virtual decimal? MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                this.maxValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the min length.
        /// </summary>
        public virtual long? MinLength
        {
            get
            {
                return this.minLength;
            }

            set
            {
                this.minLength = value;
            }
        }

        /// <summary>
        ///   Gets or sets the min value.
        /// </summary>
        public virtual decimal? MinValue
        {
            get
            {
                return this.minValue;
            }

            set
            {
                this.minValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the multi lingual.
        /// </summary>
        public virtual TertiaryBool Multilingual
        {
            get
            {
                return this.isMultilingual;
            }

            set
            {
                this.isMultilingual = value;
            }
        }

        /// <summary>
        ///   Gets or sets the pattern.
        /// </summary>
        public virtual string Pattern
        {
            get
            {
                return this.pattern;
            }

            set
            {
                this.pattern = value;
            }
        }

        /// <summary>
        ///   Gets or sets the sequence.
        /// </summary>
        public virtual TertiaryBool Sequence
        {
            get
            {
                return this.isSequence;
            }

            set
            {
                this.isSequence = value;
            }
        }

        /// <summary>
        ///   Gets or sets the start value.
        /// </summary>
        public virtual decimal? StartValue
        {
            get
            {
                return this.startValue;
            }

            set
            {
                this.startValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this.textType;
            }

            set
            {
                this.textType = value;
            }
        }

        /// <summary>
        ///   Gets or sets the time interval.
        /// </summary>
        public virtual string TimeInterval
        {
            get
            {
                return this.timeInterval;
            }

            set
            {
                this.timeInterval = value;
            }
        }

        #endregion
    }
}
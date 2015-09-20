// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTypeWrapperMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text type wrapper mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The text type wrapper mutable core.
    /// </summary>
    [Serializable]
    public class TextTypeWrapperMutableCore : MutableCore, ITextTypeWrapperMutableObject
    {
        #region Fields

        /// <summary>
        ///   The locale.
        /// </summary>
        private string locale;

        /// <summary>
        ///   The valueren.
        /// </summary>
        private string valueren;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TextTypeWrapperMutableCore" /> class.
        /// </summary>
        public TextTypeWrapperMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType))
        {
        }

        public TextTypeWrapperMutableCore(string locale, string value) : 
            base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType))
        {
            this.locale = locale;
            this.valueren = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperMutableCore"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        public TextTypeWrapperMutableCore(ITextTypeWrapper textType)
            : base(textType)
        {
            this.locale = textType.Locale;
            this.valueren = textType.Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the locale.
        /// </summary>
        public virtual string Locale
        {
            get
            {
                return this.locale;
            }

            set
            {
                this.locale = value;
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return this.valueren;
            }

            set
            {
                this.valueren = value;
            }
        }

        #endregion
    }
}
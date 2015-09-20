// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTypeWrapperImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text type wrapper impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Globalization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using System.Collections.Generic;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType;

    /// <summary>
    ///   The text type wrapper impl.
    /// </summary>
    [Serializable]
    public class TextTypeWrapperImpl : SdmxObjectCore, ITextTypeWrapper
    {
        #region Fields

        /// <summary>
        ///   The is html text.
        /// </summary>
        private readonly bool isHtmlText;

        /// <summary>
        ///   The locale.
        /// </summary>
        private string locale;

        /// <summary>
        ///   The valueren.
        /// </summary>
        private string valueren;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM VALUES OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="locale0">
        /// The locale 0. 
        /// </param>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(string locale0, string valueren, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = locale0;
            this.Value = valueren;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(ITextTypeWrapperMutableObject textType, ISdmxObject parent)
            : base(textType, parent)
        {
            this.Locale = textType.Locale;
            this.Value = textType.Value;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(TextType textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = textType.lang;
            this.Value = textType.TypedValue;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(Name textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = textType.lang;
            this.Value = textType.TypedValue;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(Description textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = textType.lang;
            this.Value = textType.TypedValue;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(XHTMLType textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            // XPathNavigator cursor = textType.;
            this.Locale = textType.lang;
            this.Value = textType.Untyped.Value; // ??? cursor?
            this.isHtmlText = true;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = textType.lang;
            this.Value = textType.TypedValue;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTypeWrapperImpl"/> class.
        /// </summary>
        /// <param name="textType">
        /// The text type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TextTypeWrapperImpl(Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType textType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TextType), parent)
        {
            this.Locale = textType.lang;
            this.Value = textType.TypedValue;
            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether html.
        /// </summary>
        public virtual bool Html
        {
            get
            {
                return this.isHtmlText;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets or sets the locale.
        /// </summary>
        public string Locale
        {
            get
            {
                return this.locale;
            }

            set
            {
                if (!ObjectUtil.ValidString(locale))
                {
                    locale = "en";
                }
                //Bug fix, in XML Locale contains a '-' to be valid, in Java '_' is used
                locale = locale.Replace("_", "-");
                this.locale = value;
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.valueren;
            }

            set
            {
                if (value != null)
                {
                    this.valueren = value.Trim();
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null)
            {
                return false;
            }
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ITextTypeWrapper)sdmxObject;
                if (!ObjectUtil.Equivalent(this.locale, that.Locale))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.valueren, that.Value))
                {
                    return false;
                }

                if (this.isHtmlText != that.Html)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.locale))
            {
                // Default to english
                this.locale = CultureInfo.CreateSpecificCulture("en").TwoLetterISOLanguageName;
            }

            if (!ParseLocale(locale))
            {
                throw new SdmxSemmanticException("Illegal Locale: " + locale);
            }

            if (string.IsNullOrWhiteSpace(this.valueren))
            {
                throw new SdmxSemmanticException("Text Type can not have an empty string value");
            }
        }

        /// <summary>
        ///   The parse locale.
        /// </summary>
        private bool ParseLocale(string locale)
        {
            try
            {
                CultureInfo.GetCultureInfo(locale);
                return true;

            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }

      	///////////////////////////////////////////////////////////////////////////////////////////////////
	    ////////////COMPOSITES                           //////////////////////////////////////////////////
	    ///////////////////////////////////////////////////////////////////////////////////////////////////	

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
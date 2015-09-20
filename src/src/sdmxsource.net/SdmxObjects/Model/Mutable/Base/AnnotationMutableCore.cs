// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The annotation mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The annotation mutable core.
    /// </summary>
    [Serializable]
    public class AnnotationMutableCore : MutableCore, IAnnotationMutableObject
    {
        #region Fields

        /// <summary>
        ///   The textValue.
        /// </summary>
        private IList<ITextTypeWrapperMutableObject> _text = new List<ITextTypeWrapperMutableObject>();

        /// <summary>
        ///   The id.
        /// </summary>
        private string _id;

        /// <summary>
        ///   The title.
        /// </summary>
        private string _title;

        /// <summary>
        ///   The type.
        /// </summary>
        private string _type;

        /// <summary>
        ///   The url.
        /// </summary>
        private Uri uri;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotationMutableCore" /> class.
        /// </summary>
        public AnnotationMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Annotation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationMutableCore"/> class.
        /// </summary>
        /// <param name="annotation">
        /// The annotation. 
        /// </param>
        public AnnotationMutableCore(IAnnotation annotation)
            : base(annotation)
        {
            this._id = annotation.Id;
            this._title = annotation.Title;
            this._type = annotation.Type;
            if (annotation.Uri != null)
            {
                this.uri = annotation.Uri;
            }

            if (annotation.Text != null)
            {
                foreach (ITextTypeWrapper currentTextType in annotation.Text)
                {
                    this._text.Add(new TextTypeWrapperMutableCore(currentTextType));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationMutableCore"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        public AnnotationMutableCore(ISdmxReader reader)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Annotation))
        {
            this._id = reader.GetAttributeValue("id", false);

            reader.MoveNextElement();
            while (this.ProcessReader(reader))
            {
                reader.MoveNextElement();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        ///   Gets the textValue.
        /// </summary>
        public virtual IList<ITextTypeWrapperMutableObject> Text
        {
            get
            {
                return this._text;
            }
        }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this._title;
            }

            set
            {
                this._title = value;
            }
        }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        public virtual string Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
            }
        }

        /// <summary>
        ///   Gets or sets the url.
        /// </summary>
        public virtual Uri Uri
        {
            get
            {
                return this.uri;
            }

            set
            {
                this.uri = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add textValue.
        /// </summary>
        /// <param name="textType">
        /// The textValue 0. 
        /// </param>
        public virtual void AddText(ITextTypeWrapperMutableObject textType)
        {
            if (this._text == null)
                this._text = new List<ITextTypeWrapperMutableObject>();

            this._text.Add(textType);
        }

        /// <summary>
        /// The add textValue.
        /// </summary>
        /// <param name="locale">
        /// The locale. 
        /// </param>
        /// <param name="textValue">
        /// The textValue 0. 
        /// </param>
        public virtual void AddText(string locale, string textValue)
        {
            ITextTypeWrapperMutableObject tt = new TextTypeWrapperMutableCore();
            tt.Locale = locale;
            tt.Value = textValue;
            this._text.Add(tt);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The process reader.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal bool ProcessReader(ISdmxReader reader)
        {
            if (reader.CurrentElement.Equals("AnnotationType"))
            {
                this._type = reader.CurrentElementValue;
                return true;
            }

            if (reader.CurrentElement.Equals("AnnotationTitle"))
            {
                this._title = reader.CurrentElementValue;
                return true;
            }

            if (reader.CurrentElement.Equals("AnnotationText"))
            {
                this.AddText(reader.GetAttributeValue("lang", false), reader.CurrentElementValue);
                return true;
            }

            if (reader.CurrentElement.Equals("AnnotationURL"))
            {
                this.uri = new Uri(reader.CurrentElementValue);
                return true;
            }

            return false;
        }

        #endregion
    }
}
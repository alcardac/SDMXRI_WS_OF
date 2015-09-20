// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The annotation object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The annotation object core.
    /// </summary>
    [Serializable]
    public class AnnotationObjectCore : SdmxObjectCore, IAnnotation
    {
        #region Static Fields

        /// <summary>
        ///   The _annotation type.
        /// </summary>
        private static readonly SdmxStructureType _annotationType =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Annotation);

        #endregion

        #region Fields

        /// <summary>
        ///   The _id.
        /// </summary>
        private readonly string _id;

        /// <summary>
        ///   The _text.
        /// </summary>
        private readonly IList<ITextTypeWrapper> _text;

        /// <summary>
        ///   The _title.
        /// </summary>
        private readonly string _title;

        /// <summary>
        ///   The _type.
        /// </summary>
        private readonly string _type;

        /// <summary>
        ///   The uri.
        /// </summary>
        private Uri uri;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationObjectCore"/> class.
        /// </summary>
        /// <param name="annotationMutable">
        /// The annotation mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxException">
        /// Throws Validate exception.
        /// </exception>
        public AnnotationObjectCore(IAnnotationMutableObject annotationMutable, ISdmxObject parent)
            : base(annotationMutable, parent)
        {
            this._text = new List<ITextTypeWrapper>();
            this._id = annotationMutable.Id;
            this._title = annotationMutable.Title;
            this._type = annotationMutable.Type;
            if (annotationMutable.Text != null)
            {
                foreach (ITextTypeWrapperMutableObject mutable in annotationMutable.Text)
                {
                    if (!string.IsNullOrWhiteSpace(mutable.Value))
                    {
                        this._text.Add(new TextTypeWrapperImpl(mutable, this));
                    }
                }
            }

            this.Uri = annotationMutable.Uri;
            try
            {
                this.Validate();
            }
            catch (SdmxException ex)
            {
                throw new SdmxException("Annotation is not valid", ex);
            }
            catch (Exception th)
            {
                throw new SdmxException("Annotation is not valid", th);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public AnnotationObjectCore(SdmxReader reader, ISdmxObject parent) {
        // super(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ANNOTATION), parent);
        // this.id = reader.getAttributeValue("id", false);
        // while(processNextElement(reader)) {
        // }
        // return;
        // }

        // private boolean processNextElement(SdmxReader reader) {
        // string nextEl = reader.peek();
        // if(nextEl.equals("AnnotationTitle")) {
        // reader.moveNextElement();
        // this.title = reader.getCurrentElementValue();
        // return true;
        // }
        // if(nextEl.equals("AnnotationType")) {
        // reader.moveNextElement();
        // this.type = reader.getCurrentElementValue();
        // return true;
        // } 
        // if(nextEl.equals("AnnotationURL")) {
        // reader.moveNextElement();
        // setURL(reader.getCurrentElementValue());
        // return true;
        // } 
        // if(nextEl.equals("AnnotationText")) {
        // reader.moveNextElement();
        // this.text.add(new TextTypeWrapperImpl(reader.getAttributeValue("lang", false), reader.getCurrentElementValue(), this));
        // return true;
        // } 
        // return false;
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationObjectCore"/> class.
        /// </summary>
        /// <param name="annotation">
        /// The annotation. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxException">
        /// Throws Validate exception.
        /// </exception>
        public AnnotationObjectCore(AnnotationType annotation, ISdmxObject parent)
            : base(_annotationType, parent)
        {
            this._text = new List<ITextTypeWrapper>();
            this._title = annotation.AnnotationTitle;
            this._type = annotation.AnnotationType1;
            this._id = annotation.id;
            this._text = TextTypeUtil.WrapTextTypeV21(annotation.AnnotationText, this);
            this.Uri = annotation.AnnotationURL;
            try
            {
                this.Validate();
            }
            catch (SdmxException ex)
            {
                throw new SdmxException("Annotation is not valid", ex);
            }
            catch (Exception th)
            {
                throw new SdmxException("Annotation is not valid", th);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationObjectCore"/> class.
        /// </summary>
        /// <param name="annotation">
        /// The annotation. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxException">
        /// Throws Validate exception.
        /// </exception>
        public AnnotationObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.AnnotationType annotation, ISdmxObject parent)
            : base(_annotationType, parent)
        {
            this._text = new List<ITextTypeWrapper>();
            this._title = annotation.AnnotationTitle;
            this._type = annotation.AnnotationType1; // $$$ 1 ?
            this._text = TextTypeUtil.WrapTextTypeV2(annotation.AnnotationText, this);
            this.Uri = annotation.AnnotationURL;
            try
            {
                this.Validate();
            }
            catch (SdmxException ex)
            {
                throw new SdmxException("Annotation is not valid", ex);
            }
            catch (Exception th)
            {
                throw new SdmxException("Annotation is not valid", th);
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationObjectCore"/> class.
        /// </summary>
        /// <param name="annotation">
        /// The annotation. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxException">
        /// Throws Validate exception.
        /// </exception>
        public AnnotationObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationType annotation, ISdmxObject parent)
            : base(_annotationType, parent)
        {
            this._text = new List<ITextTypeWrapper>();
            this._title = annotation.AnnotationTitle;
            this._type = annotation.AnnotationType1;
            this._text = TextTypeUtil.WrapTextTypeV1(annotation.AnnotationText, this);
            this.Uri = annotation.AnnotationURL;
            try
            {
                this.Validate();
            }
            catch (SdmxException ex)
            {
                throw new SdmxException("Annotation is not valid", ex);
            }
            catch (Exception th)
            {
                throw new SdmxException("Could not create Annotation as it did not validate", th);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this._id;
            }
        }

        /// <summary>
        ///   Gets the text.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Text
        {
            get
            {
                return new List<ITextTypeWrapper>(this._text);
            }
        }

        /// <summary>
        ///   Gets the title.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this._title;
            }
        }

        /// <summary>
        ///   Gets the type.
        /// </summary>
        public virtual string Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        ///   Gets the url.
        /// </summary>
        /// <exception cref="SdmxException">Throws Validate exception.</exception>
        public Uri Uri
        {
            get
            {
                return this.uri;
            }

            private set
            {
                if (value == null)
                {
                    this.uri = null;
                    return;
                }

                try
                {
                    this.uri = value;
                }
                catch (SdmxException ex)
                {
                    throw new SdmxException("Could not create attribute 'annotationURL' with value '" + value + "'", ex);
                }
                catch (Exception th)
                {
                    throw new SdmxException("Could not create attribute 'annotationURL' with value '" + value + "'", th);
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (!includeFinalProperties)
            {
                //If we don't care about the final properties, then don't check this
                return true;
            }
            return this.Equals(sdmxObject);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The annotationObject. 
        /// </param>
        protected bool Equals(AnnotationObjectCore other)
        {
          if (other is AnnotationObjectCore)
          {
             AnnotationObjectCore annotation = (AnnotationObjectCore)other;
             if (!string.Equals(this._id, annotation.Id))
             {
                 return false;
             }
             if (!string.Equals(this._title, annotation.Title))
             {
                 return false;
             }

             if (!string.Equals(this._type, annotation.Type))
             {
                 return false;
             }
             if (!string.Equals(this.uri, annotation.Uri))
             {
                 return false;
             }
             if (!base.Equivalent(this._text, annotation.Text, true))
             {
                 return false;
             }
             return true;
         }
         return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AnnotationObjectCore) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_id != null ? _id.GetHashCode() : 0);
                foreach (var tt in this._text) 
                {
                    hashCode = (hashCode * 397) ^ (tt.Locale != null ? tt.Locale.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (tt.Value != null ? tt.Value.GetHashCode() : 0);
                }
                hashCode = (hashCode*397) ^ (_title != null ? _title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_type != null ? _type.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (uri != null ? uri.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
            ValidationUtil.ValidateTextType(this._text, null);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES				 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////	

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
            base.AddToCompositeSet(this._text, composites);
            return composites;
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Title:" + Title);
            sb.Append("Type:" + Type);
            sb.Append("URI:" + Uri);

            return sb.ToString();
        }
    }
}
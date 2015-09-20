// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotableMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The annotable mutable core.
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
    ///   The annotable mutable core.
    /// </summary>
    [Serializable]
    public abstract class AnnotableMutableCore : MutableCore, IAnnotableMutableObject
    {
        #region Fields

        /// <summary>
        ///   The annotations.
        /// </summary>
        private IList<IAnnotationMutableObject> _annotations;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotableMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected AnnotableMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
            this._annotations = new List<IAnnotationMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotableMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        protected AnnotableMutableCore(IAnnotableObject objTarget)
            : base(objTarget)
        {
            this._annotations = new List<IAnnotationMutableObject>();
            if (objTarget.Annotations != null)
            {
                foreach (IAnnotation annotation in objTarget.Annotations)
                {
                    this._annotations.Add(new AnnotationMutableCore(annotation));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the annotations.
        /// </summary>
        public virtual IList<IAnnotationMutableObject> Annotations
        {
            get
            {
                return this._annotations;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation. 
        /// </param>
        public virtual void AddAnnotation(IAnnotationMutableObject annotation)
        {
            if (annotation == null) _annotations = new List<IAnnotationMutableObject>();
                this._annotations.Add(annotation);
        }

        public IAnnotationMutableObject AddAnnotation(string title, string type, string url)
        {
            IAnnotationMutableObject mutable = new AnnotationMutableCore();
            mutable.Title = title;
            mutable.Type = type;
            mutable.Uri = new Uri(url);
            AddAnnotation(mutable);
            return mutable;
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
        protected internal virtual bool ProcessReader(ISdmxReader reader)
        {
            if (reader.CurrentElement.Equals("Annotations"))
            {
                reader.MoveNextElement();
                while (reader.CurrentElement.Equals("Annotation"))
                {
                    this.AddAnnotation(new AnnotationMutableCore(reader));
                }
            }

            return false;
        }

        #endregion
    }
}
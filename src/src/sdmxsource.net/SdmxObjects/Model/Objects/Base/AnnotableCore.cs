// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The annotable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    using AnnotationsType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.AnnotationsType;

    #endregion

    /// <summary>
    ///   The annotable core.
    /// </summary>
    [Serializable]
    public abstract class AnnotableCore : SdmxStructureCore, IAnnotableObject
    {
        #region Fields

        /// <summary>
        ///   The annotations.
        /// </summary>
        private readonly IList<IAnnotation> annotations;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotableCore" /> class.
        /// </summary>
        /// <param name="agencyScheme"> The agencyScheme. </param>
        protected internal AnnotableCore(IAnnotableObject agencyScheme)
            : base(agencyScheme)
        {
            this.annotations = new List<IAnnotation>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotableCore" /> class.
        /// </summary>
        /// <param name="mutableObject"> The mutable object. </param>
        /// <param name="parent"> The parent. </param>
        protected internal AnnotableCore(IAnnotableMutableObject mutableObject, ISdmxStructure parent)
            : base(mutableObject, parent)
        {
            this.annotations = new List<IAnnotation>();
            if (mutableObject != null && mutableObject.Annotations != null)
            {
                foreach (IAnnotationMutableObject currentAnnotation in mutableObject.Annotations)
                {
                    this.annotations.Add(new AnnotationObjectCore(currentAnnotation, this));
                }
            }
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        // ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public AnnotableCore(SdmxStructureType structure, SdmxReader reader, ISdmxStructure parent) {
        // super(structure, parent);
        // string currentElement = reader.getCurrentElement();
        // if(reader.peek().equals("Annotations")) {
        // reader.moveNextElement();
        // while(reader.peek().equals("Annotation")) {
        // reader.moveNextElement();
        // annotations.add(new AnnotationObjectCore(reader, this));
        // }
        // reader.moveBackToElement(currentElement);
        // }
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotableCore" /> class.
        /// </summary>
        /// <param name="createdFrom"> The created from. </param>
        /// <param name="structureType"> The structure type. </param>
        /// <param name="parent"> The parent. </param>
        protected internal AnnotableCore(
            AnnotableType createdFrom, SdmxStructureType structureType, ISdmxStructure parent)
            : base(structureType, parent)
        {
            this.annotations = new List<IAnnotation>();
            if (createdFrom != null)
            {
                Annotations annotations1 = createdFrom.Annotations;
                if (annotations1 != null && annotations1.Annotation != null)
                {
                    IList<AnnotationType> annotationType = annotations1.Annotation;
                    if (annotationType != null)
                    {
                        foreach (AnnotationType currentAnnotation in annotationType)
                        {
                            this.annotations.Add(new AnnotationObjectCore(currentAnnotation, this));
                        }
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotableCore" /> class.
        /// </summary>
        /// <param name="createdFrom"> The created from. </param>
        /// <param name="annotationType"> The annotation type. </param>
        /// <param name="structureType"> The structure type. </param>
        /// <param name="parent"> The parent. </param>
        protected internal AnnotableCore(
            IXmlSerializable createdFrom,
            AnnotationsType annotationType,
            SdmxStructureType structureType,
            ISdmxStructure parent)
            : base(structureType, parent)
        {
            this.annotations = new List<IAnnotation>();
            if (annotationType != null && annotationType.Annotation != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.AnnotationType currentAnnotation in
                    annotationType.Annotation)
                {
                    this.annotations.Add(new AnnotationObjectCore(currentAnnotation, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        ///   Initializes a new instance of the <see cref="AnnotableCore" /> class.
        /// </summary>
        /// <param name="createdFrom"> The created from. </param>
        /// <param name="annotationType"> The annotation type. </param>
        /// <param name="structureType"> The structure type. </param>
        /// <param name="parent"> The parent. </param>
        protected internal AnnotableCore(
            IXmlSerializable createdFrom,
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationType,
            SdmxStructureType structureType,
            ISdmxStructure parent)
            : base(structureType, parent)
        {
            this.annotations = new List<IAnnotation>();
            if (annotationType != null && annotationType.Annotation != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationType currentAnnotation in
                    annotationType.Annotation)
                {
                    this.annotations.Add(new AnnotationObjectCore(currentAnnotation, this));
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Properties

        /// <summary>
        ///   Gets the annotations.
        /// </summary>
        public virtual IList<IAnnotation> Annotations
        {
            get
            {
                return new List<IAnnotation>(this.annotations);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The has annotation type.
        /// </summary>
        /// <param name="annoationType"> The annoation type. </param>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasAnnotationType(string annoationType)
        {
            return GetAnnotationsByType(annoationType).Count > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public ISet<IAnnotation> GetAnnotationsByType(String type)
        {
            ISet<IAnnotation> returnSet = new HashSet<IAnnotation>();
            foreach (IAnnotation currentAnnotation in annotations)
            {
                if (currentAnnotation.Type != null && currentAnnotation.Type.Equals(type))
                {
                    returnSet.Add(currentAnnotation);
                }
            }
            return returnSet;
        }

        public ISet<IAnnotation> GetAnnotationsByTitle(String title)
        {
            ISet<IAnnotation> returnSet = new HashSet<IAnnotation>();
            foreach (IAnnotation currentAnnotation in annotations)
            {
                if (currentAnnotation.Title != null && currentAnnotation.Title.Equals(title))
                {
                    returnSet.Add(currentAnnotation);
                }
            }
            return returnSet;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The deep equals internal.
        /// </summary>
        /// <param name="annotableObject"> The agencyScheme. </param>
        /// <returns> The <see cref="bool" /> . </returns>
        protected internal bool DeepEqualsInternalAnnotable(
            IAnnotableObject annotableObject, bool includeFinalProperties)
        {
            if (includeFinalProperties)
            {
                IList<IAnnotation> thatAnnotations = annotableObject.Annotations;
                if (!this.Equivalent(thatAnnotations, this.annotations, includeFinalProperties))
                {
                    return false;
                }
            }
            return true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES								 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////	
        
        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(annotations, composites);
            return composites;
        }

        #endregion
    }
}
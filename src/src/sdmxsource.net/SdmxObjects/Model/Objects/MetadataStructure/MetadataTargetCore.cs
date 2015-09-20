// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataTargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata target core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The metadata target core.
    /// </summary>
    [Serializable]
    public class MetadataTargetCore : IdentifiableCore, IMetadataTarget
    {
        #region Fields

        /// <summary>
        ///   The iconstraint content target.
        /// </summary>
        private readonly IConstraintContentTarget constraintContentTarget;

        /// <summary>
        ///   The idata set target.
        /// </summary>
        private readonly IDataSetTarget dataSetTarget;

        /// <summary>
        ///   The identifiable target.
        /// </summary>
        private readonly IList<IIdentifiableTarget> _identifiableTarget = new List<IIdentifiableTarget>();

        /// <summary>
        ///   The ikey descriptor values target.
        /// </summary>
        private readonly IKeyDescriptorValuesTarget keyDescriptorValuesTarget;

        /// <summary>
        ///   The ireport period target.
        /// </summary>
        private readonly IReportPeriodTarget reportPeriodTarget;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTargetCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.Throws Validate exception.
        /// </exception>
        public MetadataTargetCore(IIdentifiableObject parent, IMetadataTargetMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            if (itemMutableObject.ConstraintContentTarget != null)
            {
                this.constraintContentTarget = new ConstraintContentTargetCore(this, itemMutableObject.ConstraintContentTarget);
            }

            if (itemMutableObject.KeyDescriptorValuesTarget != null)
            {
                this.keyDescriptorValuesTarget = new KeyDescriptorValuesTargetCore(this, itemMutableObject.KeyDescriptorValuesTarget);
            }

            if (itemMutableObject.DataSetTarget != null)
            {
                this.dataSetTarget = new DataSetTargetCore(itemMutableObject.DataSetTarget, this);
            }

            if (itemMutableObject.ReportPeriodTarget != null)
            {
                this.reportPeriodTarget = new ReportPeriodTargetCore(this, itemMutableObject.ReportPeriodTarget);
            }

            if (itemMutableObject.IdentifiableTarget != null)
            {
                foreach (IIdentifiableTargetMutableObject identifiableTargetMutableObject in itemMutableObject.IdentifiableTarget)
                {
                    this._identifiableTarget.Add(new IdentifiableTargetCore(identifiableTargetMutableObject, this));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTargetCore"/> class.
        /// </summary>
        /// <param name="metadataTarget">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal MetadataTargetCore(MetadataTargetType metadataTarget, IMetadataStructureDefinitionObject parent)
            : base(metadataTarget, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget), parent)
        {
            if (ObjectUtil.ValidCollection(metadataTarget.KeyDescriptorValuesTarget))
            {
                if (metadataTarget.KeyDescriptorValuesTarget.Count > 1)
                {
                    throw new SdmxSemmanticException(
                        "Metadata Target can not have more then one KeyDescriptorValuesTarget");
                }

                this.keyDescriptorValuesTarget =
                    new KeyDescriptorValuesTargetCore(metadataTarget.KeyDescriptorValuesTarget[0].Content, this);
            }

            if (ObjectUtil.ValidCollection(metadataTarget.DataSetTarget))
            {
                if (metadataTarget.DataSetTarget.Count > 1)
                {
                    throw new SdmxSemmanticException("Metadata Target can not have more then one DataSetTarget");
                }

                this.dataSetTarget = new DataSetTargetCore(metadataTarget.DataSetTarget[0].Content, this);
            }

            if (ObjectUtil.ValidCollection(metadataTarget.ReportPeriodTarget))
            {
                if (metadataTarget.ReportPeriodTarget.Count > 1)
                {
                    throw new SdmxSemmanticException("Metadata Target can not have more then one ReportPeriodTarget");
                }

                this.reportPeriodTarget = new ReportPeriodTargetCore(metadataTarget.ReportPeriodTarget[0].Content, this);
            }

            if (ObjectUtil.ValidCollection(metadataTarget.ConstraintContentTarget))
            {
                if (metadataTarget.DataSetTarget.Count > 1)
                {
                    throw new SdmxSemmanticException("Metadata Target can not have more then one ConstraintContentTarget");
                }

                this.constraintContentTarget = new ConstraintContentTargetCore(
                    metadataTarget.ConstraintContentTarget[0].Content, this);
            }

            if (metadataTarget.IdentifiableObjectTarget != null)
            {
                foreach (IdentifiableObjectTarget currentTarget in metadataTarget.IdentifiableObjectTarget)
                {
                    this._identifiableTarget.Add(new IdentifiableTargetCore(currentTarget.Content, this));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTargetCore"/> class.
        /// </summary>
        /// <param name="fullTarget">The SDMX v2.0 full target identifier.</param>
        /// <param name="parent">The parent.</param>
        protected internal MetadataTargetCore(FullTargetIdentifierType fullTarget, IMetadataStructureDefinitionObject parent)
            : base(fullTarget, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget), fullTarget.id, fullTarget.uri, fullTarget.Annotations, parent)
        {
            if (fullTarget.IdentifierComponent != null)
            {
                foreach (var currentIdentifier in fullTarget.IdentifierComponent)
                {
                    var identifiableTarget = new IdentifiableTargetMutableCore() { Id = currentIdentifier.id, Uri = currentIdentifier.uri };
                    if (currentIdentifier.Annotations != null && currentIdentifier.Annotations.Annotation != null)
                    {
                        foreach (var annotationType in currentIdentifier.Annotations.Annotation)
                        {
                            var annotation = new AnnotationMutableCore { Title = annotationType.AnnotationTitle, Type = annotationType.AnnotationType1, Uri = annotationType.AnnotationURL };
                            foreach (var textType in annotationType.AnnotationText)
                            {
                                annotation.AddText(textType.lang, textType.TypedValue);
                            }

                            identifiableTarget.AddAnnotation(annotation);
                        }
                    }

                    if (currentIdentifier.RepresentationScheme != null)
                    {
                        var id = currentIdentifier.RepresentationScheme.representationScheme;
                        var agency = currentIdentifier.RepresentationScheme.representationSchemeAgency;
                        var schemaType = XmlobjectsEnumUtil.GetSdmxStructureTypeFromRepresentationSchemeTypeV20(currentIdentifier.RepresentationScheme.representationSchemeType1);

                        // Only on .NET
                        var version = currentIdentifier.RepresentationScheme.RepresentationSchemeVersionEstat ?? MaintainableObject.DefaultVersion;

                        var representation = new RepresentationMutableCore();
                        var representationRef = new StructureReferenceImpl(new MaintainableRefObjectImpl(agency, id, version), schemaType);
                        representation.Representation = representationRef;
                        identifiableTarget.Representation = representation;
                    }

                    if (currentIdentifier.TargetObjectClass != null)
                    {
                        identifiableTarget.ReferencedStructureType = XmlobjectsEnumUtil.GetSdmxStructureType(currentIdentifier.TargetObjectClass);
                    }

                    this._identifiableTarget.Add(new IdentifiableTargetCore(identifiableTarget, this));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTargetCore"/> class.
        /// </summary>
        /// <param name="partialTarget">The partial target.</param>
        /// <param name="fullTarget">The full target.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="SdmxSemmanticException">Partial Target references undefined IdentifierComponentRef ' + identifierRef + '</exception>
        protected internal MetadataTargetCore(PartialTargetIdentifierType partialTarget, IMetadataTarget fullTarget, MetadataStructureDefinitionObjectCore parent)
            : base(partialTarget, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget), partialTarget.id, partialTarget.uri, partialTarget.Annotations, parent)
        {
            if (partialTarget.IdentifierComponentRef != null)
            {
                foreach (var identifierRef in partialTarget.IdentifierComponentRef)
                {
                    if (!string.IsNullOrWhiteSpace(identifierRef))
                    {
                        var identifiableTarget = fullTarget.IdentifiableTarget.FirstOrDefault(target => string.Equals(target.Id, identifierRef));
                        if (identifiableTarget == null)
                        {
                            throw new SdmxSemmanticException("Partial Target references undefined IdentifierComponentRef '" + identifierRef + "'");
                        }

                        this._identifiableTarget.Add(new IdentifiableTargetCore(new IdentifiableTargetMutableCore(identifiableTarget), this));
                    }
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the constraint content target.
        /// </summary>
        public virtual IConstraintContentTarget ConstraintContentTarget
        {
            get
            {
                return this.constraintContentTarget;
            }
        }

        /// <summary>
        ///   Gets the data set target.
        /// </summary>
        public virtual IDataSetTarget DataSetTarget
        {
            get
            {
                return this.dataSetTarget;
            }
        }

        /// <summary>
        ///   Gets the identifiable target.
        /// </summary>
        public virtual IList<IIdentifiableTarget> IdentifiableTarget
        {
            get
            {
                return new List<IIdentifiableTarget>(this._identifiableTarget);
            }
        }

        /// <summary>
        ///   Gets the key descriptor values target.
        /// </summary>
        public virtual IKeyDescriptorValuesTarget KeyDescriptorValuesTarget
        {
            get
            {
                return this.keyDescriptorValuesTarget;
            }
        }

        /// <summary>
        ///   Gets the report period target.
        /// </summary>
        public virtual IReportPeriodTarget ReportPeriodTarget
        {
            get
            {
                return this.reportPeriodTarget;
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IMetadataTarget)sdmxObject;
                if (!this.Equivalent(this.constraintContentTarget, that.ConstraintContentTarget, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.dataSetTarget, that.DataSetTarget, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.keyDescriptorValuesTarget, that.KeyDescriptorValuesTarget, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.reportPeriodTarget, that.ReportPeriodTarget, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._identifiableTarget, that.IdentifiableTarget, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.keyDescriptorValuesTarget == null && this.dataSetTarget == null && this.reportPeriodTarget == null
                && this.constraintContentTarget == null && !ObjectUtil.ValidCollection(this._identifiableTarget))
            {
                throw new SdmxSemmanticException(
                    "Metadata Target must provide at least one target (key descriptor values, dataset, constraint content, report period, or identifiable object)");
            }

            foreach (IIdentifiableTarget currentTarget in this._identifiableTarget)
            {
                string currentTargetId = currentTarget.Id;
                if (this.constraintContentTarget != null)
                {
                    if (currentTargetId.Equals(this.constraintContentTarget.Id))
                    {
                        throw new SdmxSemmanticException(
                            "IdentifiableTarget Id can not be the same as the ConstraintContentTarget Id: "
                            + this.constraintContentTarget.Id);
                    }
                }

                if (this.dataSetTarget != null)
                {
                    if (currentTargetId.Equals(this.dataSetTarget.Id))
                    {
                        throw new SdmxSemmanticException(
                            "IdentifiableTarget Id can not be the same as the DataSetTarget Id: "
                            + this.dataSetTarget.Id);
                    }
                }

                if (this.keyDescriptorValuesTarget != null)
                {
                    if (currentTargetId.Equals(this.keyDescriptorValuesTarget.Id))
                    {
                        throw new SdmxSemmanticException(
                            "IdentifiableTarget Id can not be the same as the KeyDescriptorValuesTarget Id: "
                            + this.keyDescriptorValuesTarget.Id);
                    }
                }

                if (this.reportPeriodTarget != null)
                {
                    if (currentTargetId.Equals(this.reportPeriodTarget.Id))
                    {
                        throw new SdmxSemmanticException(
                            "IdentifiableTarget Id can not be the same as the ReportPeriodTarget Id: "
                            + this.reportPeriodTarget.Id);
                    }
                }
            }
        }

      ///////////////////////////////////////////////////////////////////////////////////////////////////
      ////////////COMPOSITES		                     //////////////////////////////////////////////////
     ///////////////////////////////////////////////////////////////////////////////////////////////////
     
      /// <summary>
      ///   Get composites internal.
      /// </summary>
      protected override ISet<ISdmxObject> GetCompositesInternal()
      {
    	  ISet<ISdmxObject> composites = base.GetCompositesInternal();
          base.AddToCompositeSet(this.constraintContentTarget, composites);
          base.AddToCompositeSet(this.dataSetTarget, composites);
          base.AddToCompositeSet(this.keyDescriptorValuesTarget, composites);
          base.AddToCompositeSet(this.reportPeriodTarget, composites);
          base.AddToCompositeSet(this._identifiableTarget, composites);
          return composites;
      }

        #endregion
    }
}
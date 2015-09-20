// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using System.Collections.Generic;

    using ConceptType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ConceptType;

    /// <summary>
    ///   The concept core.
    /// </summary>
    [Serializable]
    public class ConceptCore : ItemCore, IConceptObject
    {
        #region Fields

        /// <summary>
        ///   The core representation.
        /// </summary>
        private readonly IRepresentation coreRepresentation;

        /// <summary>
        ///   The iso concept reference.
        /// </summary>
        private readonly ICrossReference isoConceptReference;

        /// <summary>
        ///   The parent agency.
        /// </summary>
        private readonly string parentAgency;

        /// <summary>
        ///   The parent concept.
        /// </summary>
        private readonly string parentConcept;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public ConceptCore(IConceptSchemeObject parent, IConceptMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            if(itemMutableObject.CoreRepresentation != null && (itemMutableObject.CoreRepresentation.TextFormat != null || itemMutableObject.CoreRepresentation.Representation != null))
            {
                this.coreRepresentation = new RepresentationCore(itemMutableObject.CoreRepresentation, this);
            }

            if (itemMutableObject.IsoConceptReference != null)
            {
                this.isoConceptReference = new CrossReferenceImpl(this, itemMutableObject.IsoConceptReference);
            }

            this.parentConcept = itemMutableObject.ParentConcept;
            this.parentAgency = itemMutableObject.ParentAgency;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="concept">
        /// The sdmxObject. 
        /// </param>
        public ConceptCore(IConceptSchemeObject parent, ConceptType concept)
            : base(concept, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept), parent)
        {
            if (concept.CoreRepresentation != null)
            {
                this.coreRepresentation = new RepresentationCore(concept.CoreRepresentation, this);
            }

            if (concept.ISOConceptReference != null)
            {
                this.isoConceptReference = new CrossReferenceImpl(
                    this, 
                    concept.ISOConceptReference.ConceptAgency, 
                    concept.ISOConceptReference.ConceptSchemeID, 
                    null, 
                    SdmxStructureEnumType.Concept, 
                    concept.ISOConceptReference.ConceptID);
            }

            var localItemReferenceType = concept.GetTypedParent<LocalConceptReferenceType>();
            if (localItemReferenceType != null)
            {
                if (ObjectUtil.ValidCollection(localItemReferenceType.URN))
                {
                    IStructureReference structureReference = new StructureReferenceImpl(localItemReferenceType.URN[0]);
                    this.parentConcept = structureReference.ChildReference.Id;
                    this.parentAgency = structureReference.MaintainableReference.AgencyId;
                }
                else
                {
                    this.parentConcept = localItemReferenceType.GetTypedRef<LocalConceptRefType>().id;
                    this.parentAgency = this.MaintainableParent.AgencyId;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="concept">
        /// The sdmxObject. 
        /// </param>
        public ConceptCore(
            IConceptSchemeObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptType concept)
            : base(
                concept, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept), 
                concept.id, 
                concept.uri, 
                concept.Name, 
                concept.Description, 
                concept.Annotations, 
                parent)
        {
            if (!string.IsNullOrWhiteSpace(concept.coreRepresentation))
            {
                string representationAgency = concept.coreRepresentationAgency;
                if (representationAgency == null)
                {
                    representationAgency = parent.AgencyId;
                }

                string representationVersion = concept.CoreRepresentationVersionEstat ?? MaintainableObject.DefaultVersion;

                this.coreRepresentation = new RepresentationCore(
                    concept.TextFormat, 
                    representationAgency, 
                    concept.coreRepresentation, 
                    representationVersion, 
                    this);
            }

            else if (concept.TextFormat != null)
            {
                this.coreRepresentation = new RepresentationCore(concept.TextFormat, null, null, null, this);
            }

            this.parentConcept = concept.parent;
            this.parentAgency = concept.parentAgency;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1.0 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="concept">
        /// The sdmxObject. 
        /// </param>
        public ConceptCore(
            IConceptSchemeObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.ConceptType concept)
            : base(
                concept, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept), 
                concept.id, 
                concept.uri, 
                concept.Name, 
                null, 
                concept.Annotations, 
                parent)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the core representation.
        /// </summary>
        public virtual IRepresentation CoreRepresentation
        {
            get
            {
                return this.coreRepresentation;
            }
        }

        /// <summary>
        ///   Gets the iso concept reference.
        /// </summary>
        public virtual ICrossReference IsoConceptReference
        {
            get
            {
                return this.isoConceptReference;
            }
        }

        /// <summary>
        ///   Gets the parent agency.
        /// </summary>
        public virtual string ParentAgency
        {
            get
            {
                return this.parentAgency;
            }
        }

        /// <summary>
        ///   Gets the parent concept.
        /// </summary>
        public virtual string ParentConcept
        {
            get
            {
                return this.parentConcept;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether stand alone concept.
        /// </summary>
        public virtual bool StandAloneConcept
        {
            get
            {
                return false;
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
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IConceptObject)sdmxObject;
                if (!ObjectUtil.Equivalent(this.parentConcept, that.ParentConcept))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.parentAgency, that.ParentAgency))
                {
                    return false;
                }

                if (!this.Equivalent(this.isoConceptReference, that.IsoConceptReference))
                {
                    return false;
                }

                if (!this.Equivalent(this.coreRepresentation, that.CoreRepresentation, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Not allowed to start with an integer
            base.ValidateId(false);
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       ///   The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal()
       {
    	  ISet<ISdmxObject> composites = base.GetCompositesInternal();
          base.AddToCompositeSet(this.coreRepresentation, composites);
          return composites;
       }

        #endregion
    }
}
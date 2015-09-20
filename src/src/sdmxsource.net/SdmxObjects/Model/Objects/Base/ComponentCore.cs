// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
	using System;
	using System.Xml.Serialization;

	using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
	using Org.Sdmxsource.Sdmx.Util.Objects;
	using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
	using Org.Sdmxsource.Util;
    using System.Collections.Generic;

	using TextFormatType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TextFormatType;

	/// <summary>
	///   The component core.
	/// </summary>
	[Serializable]
	public abstract class ComponentCore : IdentifiableCore, IComponent
	{
		#region Fields

		/// <summary>
		///   The _concept ref.
		/// </summary>
		private readonly ICrossReference _conceptRef;
		
		/// <summary>
		///   The local representation.
		/// </summary>
		private IRepresentation _localRepresentation;

		#endregion

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////
		#region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentCore" /> class.
        /// </summary>
        /// <param name="itemMutableObject">The agencyScheme.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="validateComponentAttributes">if set to <c>true</c> [validate component attributes].</param>
        /// <exception cref="SdmxException">Throws Validate exception.</exception>
		protected internal ComponentCore(IComponentMutableObject itemMutableObject, IIdentifiableObject parent, bool validateComponentAttributes = true)
			: base(itemMutableObject, parent)
		{
			try
			{
				if (itemMutableObject.Representation != null)
				{
					this.LocalRepresentation = new RepresentationCore(itemMutableObject.Representation, this);
				}

				if (itemMutableObject.ConceptRef != null)
				{
					this._conceptRef = new CrossReferenceImpl(this, itemMutableObject.ConceptRef);
				}
			}
			catch (Exception th)
			{
                throw new SdmxException("IsError creating component: " + this, th);
			}

            if (validateComponentAttributes)
            {
                this.ValidateComponentAttributes();
            }
        }

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentCore"/> class.
		/// </summary>
		/// <param name="createdFrom">
		/// The created from. 
		/// </param>
		/// <param name="representation"> The local representation of the component type</param>
		/// <param name="structureType">
		/// The structure type. 
		/// </param>
		/// <param name="parent">
		/// The parent. 
		/// </param>
		protected internal ComponentCore(
			ComponentType createdFrom, RepresentationType representation, SdmxStructureType structureType, IIdentifiableObject parent)
			: base(createdFrom, structureType, parent)
		{
			if (representation != null)
			{
				this.LocalRepresentation = new RepresentationCore(representation, this);
			}

			if (createdFrom.ConceptIdentity != null)
			{
				this._conceptRef = RefUtil.CreateReference(this, createdFrom.ConceptIdentity);
			}

			// FUNC 2.1 put in Concept Identifity = conceptRef
			this.ValidateComponentAttributes();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentCore"/> class.
		/// </summary>
		/// <param name="createdFrom">
		/// The created from. 
		/// </param>
		/// <param name="structureType">
		/// The structure type. 
		/// </param>
		/// <param name="parent">
		/// The parent. 
		/// </param>
		protected internal ComponentCore(
			ComponentType createdFrom, SdmxStructureType structureType, IIdentifiableObject parent)
			: base(createdFrom, structureType, parent)
		{
			var simpleDataStructureRepresentationType = createdFrom.GetTypedLocalRepresentation<SimpleDataStructureRepresentationType>();
			if (simpleDataStructureRepresentationType != null)
			{
				this.LocalRepresentation = new RepresentationCore(simpleDataStructureRepresentationType, this);
			}

			if (createdFrom.ConceptIdentity != null)
			{
				this._conceptRef = RefUtil.CreateReference(this, createdFrom.ConceptIdentity);
			}

			// FUNC 2.1 put in Concept Identifity = conceptRef
			this.ValidateComponentAttributes();
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentCore"/> class.
		/// </summary>
		/// <param name="createdFrom">
		/// The created from. 
		/// </param>
		/// <param name="structureType">
		/// The structure type. 
		/// </param>
		/// <param name="annotationType">
		/// The annotation type. 
		/// </param>
		/// <param name="textFormat">
		/// The text format. 
		/// </param>
		/// <param name="codelistAgency">
		/// The codelist agency. 
		/// </param>
		/// <param name="codelistId">
		/// The codelist id. 
		/// </param>
		/// <param name="codelistVersion">
		/// The codelist version. 
		/// </param>
		/// <param name="conceptSchemeAgency">
		/// The concept scheme agency. 
		/// </param>
		/// <param name="conceptSchemeId">
		/// The concept scheme id. 
		/// </param>
		/// <param name="conceptSchemeVersion">
		/// The concept scheme version. 
		/// </param>
		/// <param name="conceptAgency">
		/// The concept agency. 
		/// </param>
		/// <param name="conceptId">
		/// The concept id. 
		/// </param>
		/// <param name="parent">
		/// The parent. 
		/// </param>
		protected internal ComponentCore(
			IXmlSerializable createdFrom, 
			SdmxStructureType structureType, 
			AnnotationsType annotationType, 
			TextFormatType textFormat, 
			string codelistAgency, 
			string codelistId, 
			string codelistVersion, 
			string conceptSchemeAgency, 
			string conceptSchemeId, 
			string conceptSchemeVersion, 
			string conceptAgency, 
			string conceptId, 
			IIdentifiableObject parent)
			: base(createdFrom, structureType, conceptId, null, annotationType, parent)
		{
			if (string.IsNullOrWhiteSpace(conceptAgency))
			{
				conceptAgency = this.MaintainableParent.AgencyId;
			}

			if (textFormat != null || ObjectUtil.ValidOneString(codelistAgency, codelistId, codelistVersion))
			{
				if (ObjectUtil.ValidOneString(codelistAgency, codelistId, codelistVersion))
				{
					if (string.IsNullOrWhiteSpace(codelistAgency))
					{
						codelistAgency = this.MaintainableParent.AgencyId;
					}
				}

				this.LocalRepresentation = new RepresentationCore(
					textFormat, codelistAgency, codelistId, codelistVersion, this);
			}

			this._conceptRef = ConceptRefUtil.BuildConceptRef(
				this, conceptSchemeAgency, conceptSchemeId, conceptSchemeVersion, conceptAgency, conceptId);
			this.ValidateComponentAttributes();
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentCore"/> class.
		/// </summary>
		/// <param name="createdFrom">
		/// The created from. 
		/// </param>
		/// <param name="structureType">
		/// The structure type. 
		/// </param>
		/// <param name="annotationType">
		/// The annotation type. 
		/// </param>
		/// <param name="codelistId">
		/// The codelist id. 
		/// </param>
		/// <param name="conceptId">
		/// The concept id. 
		/// </param>
		/// <param name="parent">
		/// The parent. 
		/// </param>
		protected internal ComponentCore(
			IXmlSerializable createdFrom, 
			SdmxStructureType structureType, 
			Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationType, 
			string codelistId, 
			string conceptId, 
			ISdmxStructure parent)
			: base(createdFrom, structureType, conceptId, null, annotationType, parent)
		{
			if (!string.IsNullOrWhiteSpace(codelistId))
			{
				this.LocalRepresentation = new RepresentationCore(codelistId, this);
			}

			this._conceptRef = new CrossReferenceImpl(
				this, 
				this.MaintainableParent.AgencyId, 
				ConceptSchemeObject.DefaultSchemeVersion, 
				ConceptSchemeObject.DefaultSchemeVersion, 
				SdmxStructureEnumType.Concept, 
				conceptId);
			this.ValidateComponentAttributes();
		}

		#endregion

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////DEEP EQUALS                             //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////
		#region Public Properties

		/// <summary>
		///   Gets the concept ref.
		/// </summary>
		public virtual ICrossReference ConceptRef
		{
			get
			{
				return this._conceptRef;
			}
		}

		/// <summary>
		///   Gets the id.
		/// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
		public override string Id
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(base.Id))
				{
					return base.Id;
				}

				if (this._conceptRef != null)
				{
					return this._conceptRef.ChildReference.Id;
				}

				throw new SdmxSemmanticException("Id not set for component");
			}
		}

		/// <summary>
		///   Gets the representation.
		/// </summary>
		public virtual IRepresentation Representation
		{
			get
			{
				return this.LocalRepresentation;
			}
		}

		/// <summary>
		///   Gets or sets the local representation.
		/// </summary>
		public IRepresentation LocalRepresentation
		{
			get
			{
				return this._localRepresentation;
			}

			set
			{
				this._localRepresentation = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///   The has coded representation.
		/// </summary>
		/// <returns> The <see cref="bool" /> . </returns>
		public virtual bool HasCodedRepresentation()
		{
			if (this.LocalRepresentation != null)
			{
				return this.LocalRepresentation.Representation != null;
			}

			return false;
		}

		#endregion

		#region Methods

		/// <summary>
		/// The deep equals internal.
		/// </summary>
		/// <param name="component">
		/// The agencyScheme. 
		/// </param>
		/// <returns>
		/// The <see cref="bool"/> . 
		/// </returns>
		protected internal bool DeepEqualsInternal(IComponent component, bool includeFinalProperties)
		{
			if (component == null)
			{
				return false;
			}
			if (!this.Equivalent(this._conceptRef, component.ConceptRef))
			{
				return false;
			}

			if (!this.Equivalent(this.LocalRepresentation, component.Representation, includeFinalProperties))
			{
				return false;
			}

			return base.DeepEqualsInternal(component, includeFinalProperties);
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////VALIDATION                             //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		///   The validate component attributes.
		/// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
		protected internal void ValidateComponentAttributes()
		{
            ValidateComponetReference();
			base.ValidateId(false);
		}

        protected void ValidateComponetReference()
        {
            if (this._conceptRef == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredAttribute, this.StructureType.GetType(), "conceptRef");
            }
            if (this._conceptRef.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept))
            {
                throw new SdmxSemmanticException("Component reference is invalid, expected " + SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept).GetType() + " reference, got " + this._conceptRef.TargetReference.GetType() + " reference");
            }
        }

		/// <summary>
		/// The validate id.
		/// </summary>
		/// <param name="startWithIntAllowed">
		/// The start with int allowed. 
		/// </param>
		protected internal override void ValidateId(bool startWithIntAllowed)
		{
			// Do nothing yet, not yet fully built
		}

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES                           //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
    
        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
    	  ISet<ISdmxObject> composites = base.GetCompositesInternal();
    	  base.AddToCompositeSet(LocalRepresentation, composites);
    	  return composites;
        }
		#endregion
	}
}
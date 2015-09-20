// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The representation core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using System.Collections.Generic;

    using TextFormatType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TextFormatType;

    /// <summary>
    ///   The representation core.
    /// </summary>
    [Serializable]
    public class RepresentationCore : SdmxStructureCore, IRepresentation
    {
        #region Static Fields

        /// <summary>
        ///   The _local representation.
        /// </summary>
        private static readonly SdmxStructureType _localRepresentation =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.LocalRepresentation);

        #endregion

        #region Fields

        /// <summary>
        ///   The representation ref.
        /// </summary>
        private readonly ICrossReference representation;

        /// <summary>
        ///   The text format.
        /// </summary>
        private readonly ITextFormat textFormat;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationCore"/> class.
        /// </summary>
        /// <param name="representationMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RepresentationCore(IRepresentationMutableObject representationMutableObject, IIdentifiableObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.LocalRepresentation), parent)
        {
            if (representationMutableObject.TextFormat != null)
            {
                this.textFormat = new TextFormatObjectCore(representationMutableObject.TextFormat, this);
            }

            if (representationMutableObject.Representation != null)
            {
                this.representation = new CrossReferenceImpl(this, representationMutableObject.Representation);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationCore"/> class.
        /// </summary>
        /// <param name="conceptRepresentation">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RepresentationCore(ConceptRepresentation conceptRepresentation, IIdentifiableObject parent)
            : base(_localRepresentation, parent)
        {
            if (conceptRepresentation.TextFormat != null)
            {
                this.textFormat = new TextFormatObjectCore(conceptRepresentation.TextFormat, this);
            }

            if (conceptRepresentation.Enumeration != null)
            {
                this.representation = RefUtil.CreateReference(this, conceptRepresentation.Enumeration);
                if (conceptRepresentation.EnumerationFormat != null)
                {
                    this.textFormat = new TextFormatObjectCore(conceptRepresentation.EnumerationFormat, this);
                }
            }

            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationCore"/> class.
        /// </summary>
        /// <param name="representationType">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RepresentationCore(RepresentationType representationType, IIdentifiableObject parent)
            : base(_localRepresentation, parent)
        {
            if (representationType.TextFormat != null)
            {
                this.textFormat = new TextFormatObjectCore(representationType.TextFormat, this);
            }

            ItemSchemeReferenceBaseType reference;
            if (representationType.CodelistEnumeration != null)
            {
                reference = representationType.CodelistEnumeration;
            }
            else
            {
                reference = representationType.ConceptSchemeEnumeration;
            }

            if (reference != null)
            {
                this.representation = RefUtil.CreateReference(this, reference);
                if (representationType.EnumerationFormat != null)
                {
                    this.textFormat = new TextFormatObjectCore(representationType.EnumerationFormat, this);
                }
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationCore"/> class.
        /// </summary>
        /// <param name="textFormat0">
        /// The text format 0. 
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
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RepresentationCore(
            TextFormatType textFormat0, 
            string codelistAgency, 
            string codelistId, 
            string codelistVersion, 
            IIdentifiableObject parent)
            : base(_localRepresentation, parent)
        {
            if (textFormat0 != null)
            {
                this.textFormat = new TextFormatObjectCore(textFormat0, this);
            }

            if (ObjectUtil.ValidOneString(codelistAgency, codelistId, codelistVersion))
            {
                if (string.IsNullOrWhiteSpace(codelistAgency))
                {
                    codelistAgency = this.MaintainableParent.AgencyId;
                }

                SdmxStructureType structureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                if (parent.MaintainableParent is ICrossSectionalDataStructureObject)
                {
                    if (parent is IDimension)
                    {
                        if (((IDimension)parent).MeasureDimension)
                        {
                            structureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                        }
                    }
                }

                this.representation = new CrossReferenceImpl(
                    this, codelistAgency, codelistId, codelistVersion, structureType);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationCore"/> class.
        /// </summary>
        /// <param name="codelistId">
        /// The codelist id. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal RepresentationCore(string codelistId, IIdentifiableObject parent)
            : base(_localRepresentation, parent)
        {
            if (!string.IsNullOrWhiteSpace(codelistId))
            {
                this.representation = new CrossReferenceImpl(
                    this, 
                    parent.MaintainableParent.AgencyId, 
                    codelistId, 
                    null, 
                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            }

            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the representation ref.
        /// </summary>
        public virtual ICrossReference Representation
        {
            get
            {
                return this.representation;
            }
        }

        /// <summary>
        ///   Gets the text format.
        /// </summary>
        public virtual ITextFormat TextFormat
        {
            get
            {
                return this.textFormat;
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
            if(sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IRepresentation)sdmxObject;

                if (!this.Equivalent(this.representation, that.Representation))
                {
                    return false;
                }

                if (!this.Equivalent(this.textFormat, that.TextFormat, includeFinalProperties))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                           //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.representation == null && this.textFormat == null)
            {
                throw new SdmxSemmanticException("Representation must have a codelist reference or text format");
            }
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   The get composites internal.
        /// </summary>
        protected  override ISet<ISdmxObject> GetCompositesInternal() 
        {
	    	ISet<ISdmxObject> composites = base.GetCompositesInternal();
		    base.AddToCompositeSet(textFormat, composites);
		    return composites;
	    }

        #endregion
    }
}
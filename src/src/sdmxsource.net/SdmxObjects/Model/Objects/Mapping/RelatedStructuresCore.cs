// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelatedStructuresCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The related object structures core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using CategorySchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategorySchemeRefType;
    using ConceptSchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptSchemeRefType;
    using HierarchicalCodelistRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.HierarchicalCodelistRefType;
    using MetadataStructureRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataStructureRefType;
    using OrganisationSchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationSchemeRefType;

    /// <summary>
    ///   The related object structures core.
    /// </summary>
    [Serializable]
    public class RelatedStructuresCore : SdmxStructureCore, IRelatedStructures
    {
        #region Fields

        /// <summary>
        ///   The category scheme ref.
        /// </summary>
        private readonly IList<ICrossReference> _categorySchemeRef;

        /// <summary>
        ///   The concept scheme ref.
        /// </summary>
        private readonly IList<ICrossReference> _conceptSchemeRef;

        /// <summary>
        ///   The hier codelist ref.
        /// </summary>
        private readonly IList<ICrossReference> _hierCodelistRef;

        /// <summary>
        ///   The key family ref.
        /// </summary>
        private readonly IList<ICrossReference> _keyFamilyRef;

        /// <summary>
        ///   The metadata structure ref.
        /// </summary>
        private readonly IList<ICrossReference> _metadataStructureRef;

        /// <summary>
        ///   The org scheme ref.
        /// </summary>
        private readonly IList<ICrossReference> _orgSchemeRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedStructuresCore"/> class.
        /// </summary>
        /// <param name="relatedStructuresMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RelatedStructuresCore(IRelatedStructuresMutableObject relatedStructuresMutableObject, ISdmxStructure parent)
            : base(relatedStructuresMutableObject, parent)
        {
            this._keyFamilyRef = new List<ICrossReference>();
            this._metadataStructureRef = new List<ICrossReference>();
            this._conceptSchemeRef = new List<ICrossReference>();
            this._categorySchemeRef = new List<ICrossReference>();
            this._orgSchemeRef = new List<ICrossReference>();
            this._hierCodelistRef = new List<ICrossReference>();
            this._keyFamilyRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.DataStructureRef);
            this._metadataStructureRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.MetadataStructureRef);
            this._conceptSchemeRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.ConceptSchemeRef);
            this._categorySchemeRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.CategorySchemeRef);
            this._orgSchemeRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.OrgSchemeRef);
            this._hierCodelistRef = this.CreateCrossReferenceList(relatedStructuresMutableObject.HierCodelistRef);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedStructuresCore"/> class.
        /// </summary>
        /// <param name="relStrucTypeList">
        /// The rel struc type list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public RelatedStructuresCore(IList<StructureOrUsageReferenceType> relStrucTypeList, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RelatedStructures), parent)
        {
            this._keyFamilyRef = new List<ICrossReference>();
            this._metadataStructureRef = new List<ICrossReference>();
            this._conceptSchemeRef = new List<ICrossReference>();
            this._categorySchemeRef = new List<ICrossReference>();
            this._orgSchemeRef = new List<ICrossReference>();
            this._hierCodelistRef = new List<ICrossReference>();

            foreach (StructureOrUsageReferenceType relStrucType in relStrucTypeList)
            {
                ICrossReference structureReference = RefUtil.CreateReference(this, relStrucType);
                switch (structureReference.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.Dsd:
                        this._keyFamilyRef.Add(structureReference);
                        break;
                    case SdmxStructureEnumType.Msd:
                        this._metadataStructureRef.Add(structureReference);
                        break;
                    case SdmxStructureEnumType.ConceptScheme:
                        this._conceptSchemeRef.Add(structureReference);
                        break;
                    case SdmxStructureEnumType.CategoryScheme:
                        this._categorySchemeRef.Add(structureReference);
                        break;
                    case SdmxStructureEnumType.OrganisationUnitScheme:
                        this._orgSchemeRef.Add(structureReference);
                        break;
                    case SdmxStructureEnumType.HierarchicalCodelist:
                        this._hierCodelistRef.Add(structureReference);
                        break;
                    default:
                        throw new SdmxSemmanticException(
                            "IRelatedStructures can not reference : " + structureReference.TargetReference.GetType());
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedStructuresCore"/> class.
        /// </summary>
        /// <param name="relStrucType">
        /// The rel struc type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RelatedStructuresCore(RelatedStructuresType relStrucType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RelatedStructures), parent)
        {
            this._keyFamilyRef = new List<ICrossReference>();
            this._metadataStructureRef = new List<ICrossReference>();
            this._conceptSchemeRef = new List<ICrossReference>();
            this._categorySchemeRef = new List<ICrossReference>();
            this._orgSchemeRef = new List<ICrossReference>();
            this._hierCodelistRef = new List<ICrossReference>();

            // get list of key family ref
            if (relStrucType.KeyFamilyRef != null)
            {
                foreach (KeyFamilyRefType keyFam in relStrucType.KeyFamilyRef)
                {
                    if (keyFam.URN != null)
                    {
                        this._keyFamilyRef.Add(new CrossReferenceImpl(this, keyFam.URN));
                    }
                    else
                    {
                        this._keyFamilyRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                keyFam.KeyFamilyAgencyID, 
                                keyFam.KeyFamilyID, 
                                keyFam.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd)));
                    }
                }
            }

            // get list of metadata structure ref
            if (relStrucType.MetadataStructureRef != null)
            {
                foreach (MetadataStructureRefType metStruc in relStrucType.MetadataStructureRef)
                {
                    if (metStruc.URN != null)
                    {
                        this._metadataStructureRef.Add(new CrossReferenceImpl(this, metStruc.URN));
                    }
                    else
                    {
                        this._metadataStructureRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                metStruc.MetadataStructureAgencyID, 
                                metStruc.MetadataStructureID, 
                                metStruc.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd)));
                    }
                }
            }

            // get list of concept scheme ref
            if (relStrucType.ConceptSchemeRef != null)
            {
                foreach (ConceptSchemeRefType conStruc in relStrucType.ConceptSchemeRef)
                {
                    if (conStruc.URN != null)
                    {
                        this._conceptSchemeRef.Add(new CrossReferenceImpl(this, conStruc.URN));
                    }
                    else
                    {
                        this._conceptSchemeRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                conStruc.AgencyID, 
                                conStruc.ConceptSchemeID, 
                                conStruc.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme)));
                    }
                }
            }

            // get list of category scheme ref
            if (relStrucType.CategorySchemeRef != null)
            {
                foreach (CategorySchemeRefType catStruc in relStrucType.CategorySchemeRef)
                {
                    if (catStruc.URN != null)
                    {
                        this._categorySchemeRef.Add(new CrossReferenceImpl(this, catStruc.URN));
                    }
                    else
                    {
                        this._categorySchemeRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                catStruc.AgencyID, 
                                catStruc.CategorySchemeID, 
                                catStruc.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme)));
                    }
                }
            }

            // get list of organisation scheme ref
            if (relStrucType.OrganisationSchemeRef != null)
            {
                foreach (OrganisationSchemeRefType orgScheme in relStrucType.OrganisationSchemeRef)
                {
                    if (orgScheme.URN != null)
                    {
                        this._orgSchemeRef.Add(new CrossReferenceImpl(this, orgScheme.URN));
                    }
                    else
                    {
                        this._orgSchemeRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                orgScheme.AgencyID, 
                                orgScheme.OrganisationSchemeID, 
                                orgScheme.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme)));
                    }
                }
            }

            // get list of hierarchical codelist ref
            if (relStrucType.HierarchicalCodelistRef != null)
            {
                foreach (HierarchicalCodelistRefType hierCode in relStrucType.HierarchicalCodelistRef)
                {
                    if (hierCode.URN != null)
                    {
                        this._hierCodelistRef.Add(new CrossReferenceImpl(this, hierCode.URN));
                    }
                    else
                    {
                        this._hierCodelistRef.Add(
                            new CrossReferenceImpl(
                                this, 
                                hierCode.AgencyID, 
                                hierCode.HierarchicalCodelistID, 
                                hierCode.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)));
                    }
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the category scheme ref.
        /// </summary>
        public virtual IList<ICrossReference> CategorySchemeRef
        {
            get
            {
                return new List<ICrossReference>(this._categorySchemeRef);
            }
        }

        /// <summary>
        ///   Gets the concept scheme ref.
        /// </summary>
        public virtual IList<ICrossReference> ConceptSchemeRef
        {
            get
            {
                return new List<ICrossReference>(this._conceptSchemeRef);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the data structure ref.
        /// </summary>
        public virtual IList<ICrossReference> DataStructureRef
        {
            get
            {
                return new List<ICrossReference>(this._keyFamilyRef);
            }
        }

        /// <summary>
        ///   Gets the hier codelist ref.
        /// </summary>
        public virtual IList<ICrossReference> HierCodelistRef
        {
            get
            {
                return new List<ICrossReference>(this._hierCodelistRef);
            }
        }

        /// <summary>
        ///   Gets the metadata structure ref.
        /// </summary>
        public virtual IList<ICrossReference> MetadataStructureRef
        {
            get
            {
                return new List<ICrossReference>(this._metadataStructureRef);
            }
        }

        /// <summary>
        ///   Gets the org scheme ref.
        /// </summary>
        public virtual IList<ICrossReference> OrgSchemeRef
        {
            get
            {
                return new List<ICrossReference>(this._orgSchemeRef);
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
                var that = (IRelatedStructures)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this._keyFamilyRef, that.DataStructureRef))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this._metadataStructureRef, that.MetadataStructureRef))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this._conceptSchemeRef, that.ConceptSchemeRef))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this._categorySchemeRef, that.CategorySchemeRef))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this._orgSchemeRef, that.OrgSchemeRef))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this._hierCodelistRef, that.HierCodelistRef))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create cross reference list.
        /// </summary>
        /// <param name="structureReferences">
        /// The structure references. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{ICrossReference}"/> . 
        /// </returns>
        private IList<ICrossReference> CreateCrossReferenceList(IEnumerable<IStructureReference> structureReferences)
        {
            IList<ICrossReference> retrurnList = new List<ICrossReference>();
            if (structureReferences != null)
            {
                foreach (IStructureReference currentStructureReference in structureReferences)
                {
                    retrurnList.Add(new CrossReferenceImpl(this, currentStructureReference));
                }
            }

            return retrurnList;
        }

        #endregion
    }
}
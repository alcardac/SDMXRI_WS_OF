// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure set object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The structure set object core.
    /// </summary>
    [Serializable]
    public class StructureSetObjectCore : MaintainableObjectCore<IStructureSetObject, IStructureSetMutableObject>, 
                                          IStructureSetObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(StructureSetObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The category scheme map list.
        /// </summary>
        private readonly IList<ICategorySchemeMapObject> categorySchemeMapList;

        /// <summary>
        ///   The codelist map list.
        /// </summary>
        private readonly IList<ICodelistMapObject> codelistMapList;

        /// <summary>
        ///   The concept scheme map list.
        /// </summary>
        private readonly IList<IConceptSchemeMapObject> conceptSchemeMapList;

        /// <summary>
        ///   The organisation scheme map list.
        /// </summary>
        private readonly IList<IOrganisationSchemeMapObject> organisationSchemeMapList;

        /// <summary>
        ///   The related structures.
        /// </summary>
        private readonly IRelatedStructures relatedStructures;

        /// <summary>
        ///   The structure map list.
        /// </summary>
        private readonly IList<IStructureMapObject> structureMapList;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetObjectCore"/> class.
        /// </summary>
        /// <param name="structureSet">
        /// The structure set. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureSetObjectCore(IStructureSetMutableObject structureSet)
            : base(structureSet)
        {
            this.structureMapList = new List<IStructureMapObject>();
            this.codelistMapList = new List<ICodelistMapObject>();
            this.categorySchemeMapList = new List<ICategorySchemeMapObject>();
            this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();
            this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();
            LOG.Debug("Building IStructureSetObject from Mutable Object");
            try
            {
                if (structureSet.RelatedStructures != null)
                {
                    this.relatedStructures = new RelatedStructuresCore(structureSet.RelatedStructures, this);
                }

                if (structureSet.StructureMapList != null)
                {
                    foreach (IStructureMapMutableObject each in structureSet.StructureMapList)
                    {
                        this.structureMapList.Add(new StructureMapCore(each, this));
                    }
                }

                if (structureSet.CodelistMapList != null)
                {
                    foreach (ICodelistMapMutableObject each0 in structureSet.CodelistMapList)
                    {
                        this.codelistMapList.Add(new CodelistMapCore(each0, this));
                    }
                }

                if (structureSet.CategorySchemeMapList != null)
                {
                    foreach (ICategorySchemeMapMutableObject each1 in structureSet.CategorySchemeMapList)
                    {
                        this.categorySchemeMapList.Add(new CategorySchemeMapCore(each1, this));
                    }
                }

                if (structureSet.ConceptSchemeMapList != null)
                {
                    foreach (IConceptSchemeMapMutableObject each2 in structureSet.ConceptSchemeMapList)
                    {
                        this.conceptSchemeMapList.Add(new ConceptSchemeMapCore(each2, this));
                    }
                }

                if (structureSet.OrganisationSchemeMapList != null)
                {
                    foreach (IOrganisationSchemeMapMutableObject each3 in structureSet.OrganisationSchemeMapList)
                    {
                        this.organisationSchemeMapList.Add(new OrganisationSchemeMapCore(each3, this));
                    }
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IStructureSetObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetObjectCore"/> class.
        /// </summary>
        /// <param name="structureSet">
        /// The structure set. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureSetObjectCore(StructureSetType structureSet)
            : base(structureSet, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet))
        {
            this.structureMapList = new List<IStructureMapObject>();
            this.codelistMapList = new List<ICodelistMapObject>();
            this.categorySchemeMapList = new List<ICategorySchemeMapObject>();
            this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();
            this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();
            LOG.Debug("Building IStructureSetObject from 2.1 SDMX");
            try
            {
                if (structureSet.RelatedStructure != null)
                {
                    this.relatedStructures = new RelatedStructuresCore(structureSet.RelatedStructure, this);
                }

                if (structureSet.StructureMap != null)
                {
                    this.structureMapList = new List<IStructureMapObject>();

                    foreach (StructureMapType each in structureSet.StructureMap)
                    {
                        this.structureMapList.Add(new StructureMapCore(each, this));
                    }
                }

                if (structureSet.CodelistMap != null)
                {
                    this.codelistMapList = new List<ICodelistMapObject>();

                    foreach (CodelistMapType each0 in structureSet.CodelistMap)
                    {
                        this.codelistMapList.Add(new CodelistMapCore(each0, this));
                    }
                }

                if (structureSet.CategorySchemeMap != null)
                {
                    this.categorySchemeMapList = new List<ICategorySchemeMapObject>();

                    foreach (CategorySchemeMapType each1 in structureSet.CategorySchemeMap)
                    {
                        this.categorySchemeMapList.Add(new CategorySchemeMapCore(each1, this));
                    }
                }

                if (structureSet.ConceptSchemeMap != null)
                {
                    this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();

                    foreach (ConceptSchemeMapType each2 in structureSet.ConceptSchemeMap)
                    {
                        this.conceptSchemeMapList.Add(new ConceptSchemeMapCore(each2, this));
                    }
                }

                if (structureSet.OrganisationSchemeMap != null)
                {
                    this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();

                    foreach (OrganisationSchemeMapType each3 in structureSet.OrganisationSchemeMap)
                    {
                        this.organisationSchemeMapList.Add(new OrganisationSchemeMapCore(each3, this));
                    }
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IStructureSetObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetObjectCore"/> class.
        /// </summary>
        /// <param name="structureSet">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureSetObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.StructureSetType structureSet)
            : base(
                structureSet, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet), 
                structureSet.validTo, 
                structureSet.validFrom, 
                structureSet.version, 
                CreateTertiary(structureSet.isFinal), 
                structureSet.agencyID, 
                structureSet.id, 
                structureSet.uri, 
                structureSet.Name, 
                structureSet.Description, 
                CreateTertiary(structureSet.isExternalReference), 
                structureSet.Annotations)
        {
            this.structureMapList = new List<IStructureMapObject>();
            this.codelistMapList = new List<ICodelistMapObject>();
            this.categorySchemeMapList = new List<ICategorySchemeMapObject>();
            this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();
            this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();
            LOG.Debug("Building IStructureSetObject from 2.0 SDMX");
            try
            {
                if (structureSet.RelatedStructures != null)
                {
                    this.relatedStructures = new RelatedStructuresCore(structureSet.RelatedStructures, this);
                }

                if (structureSet.StructureMap != null)
                {
                    this.structureMapList = new List<IStructureMapObject>();
                    this.structureMapList.Add(new StructureMapCore(structureSet.StructureMap, this));
                }

                if (structureSet.CodelistMap != null)
                {
                    this.codelistMapList = new List<ICodelistMapObject>();
                    this.codelistMapList.Add(new CodelistMapCore(structureSet.CodelistMap, this));
                }

                if (structureSet.CategorySchemeMap != null)
                {
                    this.categorySchemeMapList = new List<ICategorySchemeMapObject>();
                    this.categorySchemeMapList.Add(new CategorySchemeMapCore(structureSet.CategorySchemeMap, this));
                }

                if (structureSet.ConceptSchemeMap != null)
                {
                    this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();
                    this.conceptSchemeMapList.Add(new ConceptSchemeMapCore(structureSet.ConceptSchemeMap, this));
                }

                if (structureSet.OrganisationSchemeMap != null)
                {
                    this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();
                    this.organisationSchemeMapList.Add(new OrganisationSchemeMapCore(structureSet.OrganisationSchemeMap, this));
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IStructureSetObject Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private StructureSetObjectCore(IStructureSetObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this.structureMapList = new List<IStructureMapObject>();
            this.codelistMapList = new List<ICodelistMapObject>();
            this.categorySchemeMapList = new List<ICategorySchemeMapObject>();
            this.conceptSchemeMapList = new List<IConceptSchemeMapObject>();
            this.organisationSchemeMapList = new List<IOrganisationSchemeMapObject>();
            LOG.Debug("Stub IStructureSetObject Built");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the category scheme map list.
        /// </summary>
        public virtual IList<ICategorySchemeMapObject> CategorySchemeMapList
        {
            get
            {
                var list = new List<ICategorySchemeMapObject>();

                foreach (ICategorySchemeMapObject each in this.categorySchemeMapList)
                {
                    list.Add(each);
                }

                return list;
            }
        }

        /// <summary>
        ///   Gets the codelist map list.
        /// </summary>
        public virtual IList<ICodelistMapObject> CodelistMapList
        {
            get
            {
                var list = new List<ICodelistMapObject>();

                foreach (ICodelistMapObject each in this.codelistMapList)
                {
                    list.Add(each);
                }

                return list;
            }
        }

        /// <summary>
        ///   Gets the concept scheme map list.
        /// </summary>
        public virtual IList<IConceptSchemeMapObject> ConceptSchemeMapList
        {
            get
            {
                var list = new List<IConceptSchemeMapObject>();

                foreach (IConceptSchemeMapObject each in this.conceptSchemeMapList)
                {
                    list.Add(each);
                }

                return list;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IStructureSetMutableObject MutableInstance
        {
            get
            {
                return new StructureSetMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the organisation scheme map list.
        /// </summary>
        public virtual IList<IOrganisationSchemeMapObject> OrganisationSchemeMapList
        {
            get
            {
                return new List<IOrganisationSchemeMapObject>(this.organisationSchemeMapList);
            }
        }

        /// <summary>
        ///   Gets the related structures.
        /// </summary>
        public virtual IRelatedStructures RelatedStructures
        {
            get
            {
                return this.relatedStructures;
            }
        }

        /// <summary>
        ///   Gets the structure map list.
        /// </summary>
        public virtual IList<IStructureMapObject> StructureMapList
        {
            get
            {
                var list = new List<IStructureMapObject>();

                foreach (IStructureMapObject each in this.structureMapList)
                {
                    list.Add(each);
                }

                return list;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                var that = (IStructureSetObject)sdmxObject;
                if (!this.Equivalent(this.relatedStructures, that.RelatedStructures, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.structureMapList, that.StructureMapList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.codelistMapList, that.CodelistMapList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.categorySchemeMapList, that.CategorySchemeMapList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.conceptSchemeMapList, that.ConceptSchemeMapList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.organisationSchemeMapList, that.OrganisationSchemeMapList, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IStructureSetObject"/> . 
        /// </returns>
        public override IStructureSetObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new StructureSetObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
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
           base.AddToCompositeSet(relatedStructures, composites);
           base.AddToCompositeSet(structureMapList, composites);
           base.AddToCompositeSet(codelistMapList, composites);
           base.AddToCompositeSet(categorySchemeMapList, composites);
           base.AddToCompositeSet(conceptSchemeMapList, composites);
           base.AddToCompositeSet(organisationSchemeMapList, composites);
           return composites;
        }

        #endregion
    }
}
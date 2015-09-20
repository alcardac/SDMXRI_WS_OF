// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure set mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping;

    /// <summary>
    ///   The structure set mutable core.
    /// </summary>
    [Serializable]
    public class StructureSetMutableCore : MaintainableMutableCore<IStructureSetObject>, IStructureSetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The category scheme map list.
        /// </summary>
        private IList<ICategorySchemeMapMutableObject> categorySchemeMapList;

        /// <summary>
        ///   The codelist map list.
        /// </summary>
        private IList<ICodelistMapMutableObject> codelistMapList;

        /// <summary>
        ///   The concept scheme map list.
        /// </summary>
        private IList<IConceptSchemeMapMutableObject> conceptSchemeMapList;

        /// <summary>
        ///   The organisation scheme map list.
        /// </summary>
        private IList<IOrganisationSchemeMapMutableObject> organisationSchemeMapList;

        /// <summary>
        ///   The related structures.
        /// </summary>
        private IRelatedStructuresMutableObject relatedStructures;

        /// <summary>
        ///   The structure map list.
        /// </summary>
        private IList<IStructureMapMutableObject> structureMapList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="StructureSetMutableCore" /> class. 
        ///   Constructor.
        /// </summary>
        public StructureSetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSetMutableCore"/> class. 
        ///   Copy constructor
        /// </summary>
        /// <param name="objTarget">
        /// immutable variant to copy from 
        /// </param>
        public StructureSetMutableCore(IStructureSetObject objTarget)
            : base(objTarget)
        {
            if (objTarget.RelatedStructures != null)
            {
                this.relatedStructures = new RelatedStructuresMutableCore(objTarget.RelatedStructures);
            }

            foreach (IStructureMapObject each in objTarget.StructureMapList)
            {
                this.AddStructureMap(new StructureMapMutableCore(each));
            }

            foreach (ICodelistMapObject each0 in objTarget.CodelistMapList)
            {
                this.AddCodelistMap(new CodelistMapMutableCore(each0));
            }

            foreach (ICategorySchemeMapObject each1 in objTarget.CategorySchemeMapList)
            {
                this.AddCategorySchemeMap(new CategorySchemeMapMutableCore(each1));
            }

            foreach (IConceptSchemeMapObject each2 in objTarget.ConceptSchemeMapList)
            {
                this.AddConceptSchemeMap(new ConceptSchemeMapMutableCore(each2));
            }

            foreach (IOrganisationSchemeMapObject each3 in objTarget.OrganisationSchemeMapList)
            {
                this.AddOrganisationSchemeMap(new OrganisationSchemeMapMutableCore(each3));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the category scheme map list.
        /// </summary>
        public IList<ICategorySchemeMapMutableObject> CategorySchemeMapList
        {
            get
            {
                if (this.categorySchemeMapList == null)
                {
                    this.categorySchemeMapList = new List<ICategorySchemeMapMutableObject>();
                }

                return this.categorySchemeMapList;
            }
        }

        /// <summary>
        ///   Gets the codelist map list.
        /// </summary>
        public IList<ICodelistMapMutableObject> CodelistMapList
        {
            get
            {
                return this.codelistMapList ?? (this.codelistMapList = new List<ICodelistMapMutableObject>());
            }
        }

        /// <summary>
        ///   Gets the concept scheme map list.
        /// </summary>
        public IList<IConceptSchemeMapMutableObject> ConceptSchemeMapList
        {
            get
            {
                if (this.conceptSchemeMapList == null)
                {
                    this.conceptSchemeMapList = new List<IConceptSchemeMapMutableObject>();
                }

                return this.conceptSchemeMapList;
            }
        }

        /// <summary>
        ///   Create immutable variant
        /// </summary>
        public override IStructureSetObject ImmutableInstance
        {
            get
            {
                return new StructureSetObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets the organisation scheme map list.
        /// </summary>
        public IList<IOrganisationSchemeMapMutableObject> OrganisationSchemeMapList
        {
            get
            {
                return this.organisationSchemeMapList
                       ?? (this.organisationSchemeMapList = new List<IOrganisationSchemeMapMutableObject>());
            }
        }

        /// <summary>
        ///   Gets or sets the related structures.
        /// </summary>
        public IRelatedStructuresMutableObject RelatedStructures
        {
            get
            {
                return this.relatedStructures;
            }

            set
            {
                this.relatedStructures = value;
            }
        }

        /// <summary>
        ///   Gets the structure map list.
        /// </summary>
        public IList<IStructureMapMutableObject> StructureMapList
        {
            get
            {
                if (this.structureMapList == null)
                {
                    this.structureMapList = new List<IStructureMapMutableObject>();
                }

                return this.structureMapList;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////ADDERS                                  //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The add category scheme map.
        /// </summary>
        /// <param name="categorySchemeMap">
        /// The category scheme map. 
        /// </param>
        public void AddCategorySchemeMap(ICategorySchemeMapMutableObject categorySchemeMap)
        {
            this.CategorySchemeMapList.Add(categorySchemeMap);
        }

        /// <summary>
        /// The add codelist map.
        /// </summary>
        /// <param name="codelistMap">
        /// The codelist map. 
        /// </param>
        public void AddCodelistMap(ICodelistMapMutableObject codelistMap)
        {
            this.CodelistMapList.Add(codelistMap);
        }

        /// <summary>
        /// The add concept scheme map.
        /// </summary>
        /// <param name="conceptSchemeMap">
        /// The concept scheme map. 
        /// </param>
        public void AddConceptSchemeMap(IConceptSchemeMapMutableObject conceptSchemeMap)
        {
            this.ConceptSchemeMapList.Add(conceptSchemeMap);
        }

        /// <summary>
        /// The add organisation scheme map.
        /// </summary>
        /// <param name="organisationSchemeMap">
        /// The organisation scheme map. 
        /// </param>
        public void AddOrganisationSchemeMap(IOrganisationSchemeMapMutableObject organisationSchemeMap)
        {
            this.OrganisationSchemeMapList.Add(organisationSchemeMap);
        }

        /// <summary>
        /// The add structure map.
        /// </summary>
        /// <param name="structureMap">
        /// The structure map. 
        /// </param>
        public void AddStructureMap(IStructureMapMutableObject structureMap)
        {
            this.StructureMapList.Add(structureMap);
        }

        #endregion
    }
}
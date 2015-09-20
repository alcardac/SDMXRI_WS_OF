// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The concept mutable core.
    /// </summary>
    [Serializable]
    public class ConceptMutableCore : ItemMutableCore, IConceptMutableObject
    {
        #region Fields

        /// <summary>
        ///   The core representation.
        /// </summary>
        private IRepresentationMutableObject _coreRepresentation;

        /// <summary>
        ///   The iso concept reference.
        /// </summary>
        private IStructureReference _isoConceptReference;

        /// <summary>
        ///   The parent.
        /// </summary>
        private string _parent;

        /// <summary>
        ///   The parent agency.
        /// </summary>
        private string _parentAgency;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConceptMutableCore" /> class.
        /// </summary>
        public ConceptMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public ConceptMutableCore(IConceptObject objTarget)
            : base(objTarget)
        {
            if (objTarget.CoreRepresentation != null)
            {
                this._coreRepresentation = new RepresentationMutableCore(objTarget.CoreRepresentation);
            }

            if (objTarget.IsoConceptReference != null)
            {
                this._isoConceptReference = objTarget.IsoConceptReference.CreateMutableInstance();
            }

            this._parent = objTarget.ParentConcept;
            this._parentAgency = objTarget.ParentAgency;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Creates a new instance with an id and an english name
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
         public static IConceptMutableObject GetInstance(string id, string name)
         {
		    
            IConceptMutableObject newInstance = new ConceptMutableCore();
		    newInstance.Id = id;
		    newInstance.AddName("en", name);
		    return newInstance;
	     }
	

        /// <summary>
        ///   Gets or sets the core representation.
        /// </summary>
        public virtual IRepresentationMutableObject CoreRepresentation
        {
            get
            {
                return this._coreRepresentation;
            }

            set
            {
                this._coreRepresentation = value;
            }
        }

        /// <summary>
        ///   Gets or sets the iso concept reference.
        /// </summary>
        public virtual IStructureReference IsoConceptReference
        {
            get
            {
                return this._isoConceptReference;
            }

            set
            {
                this._isoConceptReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parent agency.
        /// </summary>
        public virtual string ParentAgency
        {
            get
            {
                return this._parentAgency;
            }

            set
            {
                this._parentAgency = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parent concept.
        /// </summary>
        public virtual string ParentConcept
        {
            get
            {
                return this._parent;
            }

            set
            {
                this._parent = value;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether stand alone concept.
        /// </summary>
        public virtual bool StandaloneConcept
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodelistBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical codelist object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using HierarchicalCodelistType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.HierarchicalCodelistType;
    using HierarchyType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.HierarchyType;

    /// <summary>
    ///   The hierarchical codelist object core.
    /// </summary>
    [Serializable]
    public class HierarchicalCodelistObjectCore :
        MaintainableObjectCore<IHierarchicalCodelistObject, IHierarchicalCodelistMutableObject>, 
        IHierarchicalCodelistObject
    {
        #region Fields

        /// <summary>
        ///   The codelist ref.
        /// </summary>
        private readonly IList<ICodelistRef> _codelistRef;

        /// <summary>
        ///   The hierarchies.
        /// </summary>
        private readonly IList<IHierarchy> _hierarchies;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodelistObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchicalCodelistObjectCore(IHierarchicalCodelistMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            this._hierarchies = new List<IHierarchy>();
            this._codelistRef = new List<ICodelistRef>();
            try
            {
                if (itemMutableObject.CodelistRef != null)
                {
                    foreach (ICodelistRefMutableObject currentRef in itemMutableObject.CodelistRef)
                    {
                        this._codelistRef.Add(new CodelistRefCore(currentRef, this));
                    }
                }

                if (itemMutableObject.Hierarchies != null)
                {
                    foreach (IHierarchyMutableObject currentHierarchy in itemMutableObject.Hierarchies)
                    {
                        this._hierarchies.Add(new HierarchyCore(this, currentHierarchy));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodelistObjectCore"/> class.
        /// </summary>
        /// <param name="hierarchicalCodelist">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchicalCodelistObjectCore(HierarchicalCodelistType hierarchicalCodelist)
            : base(hierarchicalCodelist, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist))
        {
            this._hierarchies = new List<IHierarchy>();
            this._codelistRef = new List<ICodelistRef>();

            try
            {
                if (hierarchicalCodelist.IncludedCodelist != null)
                {
                    foreach (IncludedCodelistReferenceType currentRef in hierarchicalCodelist.IncludedCodelist)
                    {
                        var codelistRefType = currentRef.GetTypedRef<Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.CodelistRefType>();
                        if (codelistRefType == null)
                        {
                            this._codelistRef.Add(new CodelistRefCore(currentRef.URN.FirstOrDefault(), currentRef.alias, this));
                        }
                        else
                        {
                            this._codelistRef.Add(
                                new CodelistRefCore(
                                    codelistRefType.agencyID,
                                    codelistRefType.id,
                                    codelistRefType.version, 
                                    currentRef.alias, 
                                    this));
                        }
                    }
                }

                if (hierarchicalCodelist.Hierarchy != null)
                {
                    foreach (HierarchyType currentValueHierarchy in hierarchicalCodelist.Hierarchy)
                    {
                        this._hierarchies.Add(new HierarchyCore(this, currentValueHierarchy));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodelistObjectCore"/> class.
        /// </summary>
        /// <param name="hierarchicalCodelist">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        public HierarchicalCodelistObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.HierarchicalCodelistType hierarchicalCodelist)
            : base(
                hierarchicalCodelist, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist), 
                hierarchicalCodelist.validTo, 
                hierarchicalCodelist.validFrom, 
                hierarchicalCodelist.version, 
                CreateTertiary(hierarchicalCodelist.isFinal), 
                hierarchicalCodelist.agencyID, 
                hierarchicalCodelist.id, 
                hierarchicalCodelist.uri, 
                hierarchicalCodelist.Name, 
                hierarchicalCodelist.Description, 
                CreateTertiary(hierarchicalCodelist.isExternalReference), 
                hierarchicalCodelist.Annotations)
        {
            this._hierarchies = new List<IHierarchy>();
            this._codelistRef = new List<ICodelistRef>();
            try
            {
                if (hierarchicalCodelist.CodelistRef != null)
                {
                    foreach (CodelistRefType currentRef in hierarchicalCodelist.CodelistRef)
                    {
                        if (currentRef.URN != null)
                        {
                            try
                            {
                                this._codelistRef.Add(
                                    new CodelistRefCore(currentRef.URN, currentRef.Alias, this));
                            }
                            catch (Exception th)
                            {
                                throw new SdmxSemmanticException("IsError while trying to parse CodelistRef for " + SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist), th);
                            }
                        }
                        else
                        {
                            try
                            {
                                this._codelistRef.Add(
                                    new CodelistRefCore(
                                        currentRef.AgencyID, 
                                        currentRef.CodelistID, 
                                        currentRef.Version, 
                                        currentRef.Alias, 
                                        this));
                            }
                            catch (Exception th0)
                            {
                                throw new SdmxSemmanticException("IsError while trying to parse CodelistRef for " + SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist), th0);
                            }
                        }
                    }
                }

                if (hierarchicalCodelist.Hierarchy != null)
                {
                    foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.HierarchyType currentHierarchy in
                        hierarchicalCodelist.Hierarchy)
                    {
                        this._hierarchies.Add(new HierarchyCore(this, currentHierarchy));
                    }
                }
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e2)
            {
                throw new SdmxSemmanticException(e2, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th2)
            {
                throw new SdmxException(th2, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodelistObjectCore"/> class.
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
        private HierarchicalCodelistObjectCore(IHierarchicalCodelistObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this._hierarchies = new List<IHierarchy>();
            this._codelistRef = new List<ICodelistRef>();
        }

        #endregion

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
        ///   Gets the codelist ref.
        /// </summary>
        public virtual IList<ICodelistRef> CodelistRef
        {
            get
            {
                return new List<ICodelistRef>(this._codelistRef);
            }
        }

        /// <summary>
        ///   Gets the hierarchies.
        /// </summary>
        public virtual IList<IHierarchy> Hierarchies
        {
            get
            {
                return new List<IHierarchy>(this._hierarchies);
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IHierarchicalCodelistMutableObject MutableInstance
        {
            get
            {
                return new HierarchicalCodelistMutableCore(this);
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
                var that = (IHierarchicalCodelistObject)sdmxObject;
                if (!this.Equivalent(this._hierarchies, that.Hierarchies, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._codelistRef, that.CodelistRef, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

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
        /// The <see cref="IHierarchicalCodelistObject"/> . 
        /// </returns>
        public override IHierarchicalCodelistObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new HierarchicalCodelistObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            // Ensure codelist refs have unique aliases
            if (this._codelistRef != null)
            {
                ISet<string> codelisAlias = new HashSet<string>();

                foreach (ICodelistRef currentRef in this._codelistRef)
                {
                    if (!string.IsNullOrWhiteSpace(currentRef.Alias))
                    {
                        if (codelisAlias.Contains(currentRef.Alias))
                        {
                            throw new SdmxSemmanticException(ExceptionCode.DuplicateAlias, currentRef.Alias);
                        }

                        codelisAlias.Add(currentRef.Alias);
                    }
                }
            }

            // Ensure hierarchies and their levels have unique URNs
            if (this._hierarchies != null)
            {
                var hierarchyUrns = new HashSet<Uri>();

                foreach (IHierarchy currentHierarhy in this._hierarchies)
                {
                    try
                    {
                        if (hierarchyUrns.Contains(currentHierarhy.Urn))
                        {
                            throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, currentHierarhy);
                        }

                        hierarchyUrns.Add(currentHierarhy.Urn);
                    }
                    catch (SdmxSemmanticException ex)
                    {
                        throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
                    }
                    catch (Exception th)
                    {
                        throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
                    }
                }
            }
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
        base.AddToCompositeSet(this._hierarchies, composites);
        base.AddToCompositeSet(this._codelistRef, composites);
        return composites;
     }

        #endregion
    }
}
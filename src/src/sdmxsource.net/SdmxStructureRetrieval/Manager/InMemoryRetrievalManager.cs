// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-01-29
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The local retrieval manager provides interfaces to retrieve structures off an in memory storage of the ISdmxObjects.
//   <p />
//   This class is able to updated its cache as if it were a local storage with the interface methods provided by the StructurePersistenceManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Persist;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.CrossReference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     The local retrieval manager provides interfaces to retrieve structures off an in memory storage of the ISdmxObjects.
    ///     <p />
    ///     This class is able to updated its cache as if it were a local storage with the interface methods provided by the StructurePersistenceManager.
    /// </summary>
    public class InMemoryRetrievalManager : BaseSdmxObjectRetrievalManager, IStructurePersistenceManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(InMemoryRetrievalManager));

        #endregion

        #region Fields

        /// <summary>
        ///     The cross ref control.
        /// </summary>
        private readonly ICrossReferencedRetrievalManager _crossRefControl;

        /// <summary>
        ///     The beans.
        /// </summary>
        private readonly ISdmxObjects _sdmxObjects;

        private readonly ResultExtractor<IAgencyScheme> _agencySchemeExtractor = new ResultExtractor<IAgencyScheme>();
        private readonly ResultExtractor<IAttachmentConstraintObject> _attachmentConstraintExtractor = new ResultExtractor<IAttachmentConstraintObject>(new MaintainableUtil<IAttachmentConstraintObject>());
        private readonly ResultExtractor<ICategorisationObject> _categorisationExtractor = new ResultExtractor<ICategorisationObject>();
        private readonly ResultExtractor<ICategorySchemeObject> _categorySchemeExtractor = new ResultExtractor<ICategorySchemeObject>(new MaintainableUtil<ICategorySchemeObject>());
        private readonly ResultExtractor<ICodelistObject> _codelistExtractor = new ResultExtractor<ICodelistObject>(new MaintainableUtil<ICodelistObject>());
        private readonly ResultExtractor<IConceptSchemeObject> _conceptSchemeExtractor = new ResultExtractor<IConceptSchemeObject>(new MaintainableUtil<IConceptSchemeObject>());
        private readonly ResultExtractor<IContentConstraintObject> _contentConstraintExtractor = new ResultExtractor<IContentConstraintObject>(new MaintainableUtil<IContentConstraintObject>());
        private readonly ResultExtractor<IDataConsumerScheme> _dataConsumerSchemeExtractor = new ResultExtractor<IDataConsumerScheme>();
        private readonly ResultExtractor<IDataProviderScheme> _dataProviderSchemeExtractor = new ResultExtractor<IDataProviderScheme>();
        private readonly ResultExtractor<IDataStructureObject> _dataStructureExtractor = new ResultExtractor<IDataStructureObject>(new MaintainableUtil<IDataStructureObject>());
        private readonly ResultExtractor<IDataflowObject> _dataflowExtractor = new ResultExtractor<IDataflowObject>(new MaintainableUtil<IDataflowObject>());
        private readonly ResultExtractor<IHierarchicalCodelistObject> _hierarchicalCodelistExtractor = new ResultExtractor<IHierarchicalCodelistObject>(new MaintainableUtil<IHierarchicalCodelistObject>());
        private readonly ResultExtractor<IMetadataStructureDefinitionObject> _metadataStructureDefinitionExtractor = new ResultExtractor<IMetadataStructureDefinitionObject>(new MaintainableUtil<IMetadataStructureDefinitionObject>());
        private readonly ResultExtractor<IMetadataFlow> _metadataFlowExtractor = new ResultExtractor<IMetadataFlow>(new MaintainableUtil<IMetadataFlow>());
        private readonly ResultExtractor<IOrganisationUnitSchemeObject> _organisationUnitSchemeExtractor = new ResultExtractor<IOrganisationUnitSchemeObject>(new MaintainableUtil<IOrganisationUnitSchemeObject>());
        private readonly ResultExtractor<IProcessObject> _processBeanExtractor = new ResultExtractor<IProcessObject>(new MaintainableUtil<IProcessObject>());
        private readonly ResultExtractor<IProvisionAgreementObject> _provisionAgreementExtractor = new ResultExtractor<IProvisionAgreementObject>(new MaintainableUtil<IProvisionAgreementObject>());
        private readonly ResultExtractor<IReportingTaxonomyObject> _reportingTaxonomyExtractor = new ResultExtractor<IReportingTaxonomyObject>(new MaintainableUtil<IReportingTaxonomyObject>());
        private readonly ResultExtractor<IStructureSetObject> _structureSetExtractor = new ResultExtractor<IStructureSetObject>(new MaintainableUtil<IStructureSetObject>());
        private readonly ResultExtractor<ISubscriptionObject> _subscriptionExtractor = new ResultExtractor<ISubscriptionObject>(new MaintainableUtil<ISubscriptionObject>());

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRetrievalManager"/> class.
        ///     Default constructor
        /// </summary>
        public InMemoryRetrievalManager()
        {
            this._sdmxObjects = new SdmxObjectsImpl();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRetrievalManager"/> class.
        ///     Create an in memory retrieval manager using a URI as a seed, the URI may reference a file (local or external on the web) or be a SDMX REST query
        /// </summary>
        /// <param name="seed">
        /// The <see cref="IReadableDataLocation"/> which points to structural metadata.
        /// </param>
        /// <param name="structureParsingManager">
        /// The structure Parsing Manager.
        /// </param>
        /// <param name="crossReferenceRetrievalManager">
        /// The cross Reference Retrieval Manager.
        /// </param>
        public InMemoryRetrievalManager(IReadableDataLocation seed, IStructureParsingManager structureParsingManager, ICrossReferencedRetrievalManager crossReferenceRetrievalManager)
        {
            this._sdmxObjects = new SdmxObjectsImpl();
            this._crossRefControl = crossReferenceRetrievalManager;
            if (seed != null)
            {
                this._sdmxObjects = structureParsingManager.ParseStructures(seed).GetStructureObjects(false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRetrievalManager"/> class.
        /// </summary>
        /// <param name="objects">
        /// The beans 0.
        /// </param>
        /// <param name="crossReferenceRetrievalManager">
        /// The cross Reference Retrieval Manager.
        /// </param>
        public InMemoryRetrievalManager(ISdmxObjects objects, ICrossReferencedRetrievalManager crossReferenceRetrievalManager)
        {
            this._sdmxObjects = objects ?? new SdmxObjectsImpl();

            this._crossRefControl = crossReferenceRetrievalManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRetrievalManager"/> class.
        /// </summary>
        /// <param name="objects">
        /// The beans 0.
        /// </param>
        public InMemoryRetrievalManager(ISdmxObjects objects)
            : this(objects, (IStructureParsingManager)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRetrievalManager"/> class.
        /// </summary>
        /// <param name="objects">
        /// The beans 0.
        /// </param>
        /// <param name="structureParsingManager">
        /// The structure Parsing Manager.
        /// </param>
        public InMemoryRetrievalManager(ISdmxObjects objects, IStructureParsingManager structureParsingManager)
            : this(objects, (ICrossReferencedRetrievalManager)null)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Deletes the maintainable structures in the supplied objects
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        public void DeleteStructure(IMaintainableObject maintainable)
        {
            _log.Info("deleteStructure:" + maintainable.Urn);
            this._sdmxObjects.RemoveMaintainable(maintainable);
        }

        /// <summary>
        /// The delete structures.
        /// </summary>
        /// <param name="beans0">
        /// The beans 0.
        /// </param>
        public virtual void DeleteStructures(ISdmxObjects beans0)
        {
            _log.Info("deleteStructure:" + beans0.ToString());

            foreach (IMaintainableObject currentMaint in beans0.GetAllMaintainables())
            {
                this._sdmxObjects.RemoveMaintainable(currentMaint);
            }
        }


        /// <summary>
        /// If set then will be used to create stubs
        /// </summary>
        /// <param name="serviceRetrievalManager">
        /// The service retrieval manager
        /// </param>
        public void SetServiceRetrievalManager(IServiceRetrievalManager serviceRetrievalManager)
        {
            _serviceRetrievalManager = serviceRetrievalManager;
        }

        /// <summary>
        /// The get agency scheme bean.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IAgencyScheme"/>.
        /// </returns>
        public virtual IAgencyScheme GetAgencySchemeObject(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IAgencyScheme>.ResolveReference(this._sdmxObjects.AgenciesSchemes, xref);
            return (IAgencyScheme)maint;
        }

        /// <summary>
        /// Returns a single agency scheme , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IAgencyScheme"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IAgencyScheme GetAgencySchemeObject(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IAgencyScheme>.ResolveReference(this._sdmxObjects.AgenciesSchemes, xref));
                return (IAgencyScheme)maint;
            }
            else
                return GetAgencySchemeObject(xref);
        }

        /// <summary>
        /// The get agency scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAgencyScheme}"/>.
        /// </returns>
        public virtual ISet<IAgencyScheme> GetAgencySchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetAgenciesSchemes(xref);
        }

        /// <summary>
        /// Returns AgencySchemeObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all AgencySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAgencyScheme> GetAgencySchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            return this._agencySchemeExtractor.FilterResults(this._sdmxObjects.GetAgenciesSchemes(xref), false, returnStub);
        }

        /// <summary>
        /// The get attachment constraint.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IAttachmentConstraintObject"/>.
        /// </returns>
        public virtual IAttachmentConstraintObject GetAttachmentConstraint(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IAttachmentConstraintObject>.ResolveReference(this._sdmxObjects.AttachmentConstraints, xref);
            return (IAttachmentConstraintObject)maint;
        }

        /// <summary>
        /// Returns a single attachment constraint, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IAttachmentConstraintObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IAttachmentConstraintObject GetAttachmentConstraint(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IAttachmentConstraintObject>.ResolveReference(this._sdmxObjects.AttachmentConstraints, xref));
                return (IAttachmentConstraintObject)maint;
            }
            else
                return GetAttachmentConstraint(xref);

        }

        /// <summary>
        /// The get attachment constraints.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAttachmentConstraintObject}"/>.
        /// </returns>
        public virtual ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetAttachmentConstraints(xref);
        }

        /// <summary>
        /// Returns AttachmentConstraintBeans that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._attachmentConstraintExtractor.FilterResults(this._sdmxObjects.GetAttachmentConstraints(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get categorisation.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ICategorisationObject"/>.
        /// </returns>
        public virtual ICategorisationObject GetCategorisation(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<ICategorisationObject>.ResolveReference(this._sdmxObjects.Categorisations, xref);
            return (ICategorisationObject)maint;
        }

        /// <summary>
        /// Returns a single Categorisation, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="ICategorisationObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual ICategorisationObject GetCategorisation(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<ICategorisationObject>.ResolveReference(this._sdmxObjects.Categorisations, xref));
                return (ICategorisationObject)maint;
            }
            else
                return GetCategorisation(xref);
        }

        /// <summary>
        /// The get categorisation beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<ICategorisationObject> GetCategorisationObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetCategorisations(xref);
        }

        /// <summary>
        /// Returns CategorisationObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorisationObject> GetCategorisationObjects(IMaintainableRefObject xref, bool returnStub)
        {
            return this._categorisationExtractor.FilterResults(this._sdmxObjects.GetCategorisations(xref), false, returnStub);
        }

        /// <summary>
        /// The get category scheme.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ICategorySchemeObject"/>.
        /// </returns>
        public virtual ICategorySchemeObject GetCategoryScheme(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<ICategorySchemeObject>.ResolveReference(this._sdmxObjects.CategorySchemes, xref);
            return (ICategorySchemeObject)maint;
        }

        /// <summary>
        /// Returns a single CategoryScheme , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="ICategorySchemeObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual ICategorySchemeObject GetCategoryScheme(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<ICategorySchemeObject>.ResolveReference(this._sdmxObjects.CategorySchemes, xref));
                return (ICategorySchemeObject)maint;
            }
            else
                return GetCategoryScheme(xref);
        }

        /// <summary>
        /// The get category scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<ICategorySchemeObject> GetCategorySchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetCategorySchemes(xref);
        }

        /// <summary>
        /// Returns CategorySchemeObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorySchemeObject> GetCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._categorySchemeExtractor.FilterResults(this._sdmxObjects.GetCategorySchemes(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get codelist.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObject"/>.
        /// </returns>
        public virtual ICodelistObject GetCodelist(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<ICodelistObject>.ResolveReference(this._sdmxObjects.Codelists, xref);
            return (ICodelistObject)maint;
        }

        /// <summary>
        /// Returns a single CodeList , this expects the reference object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual ICodelistObject GetCodelist(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<ICodelistObject>.ResolveReference(this._sdmxObjects.Codelists, xref));
                return (ICodelistObject)maint;
            }
            else
                return GetCodelist(xref);
        }

        /// <summary>
        /// The get codelist beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<ICodelistObject> GetCodelistObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetCodelists(xref);
        }

        /// <summary>
        /// Returns CodelistObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICodelistObject> GetCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._codelistExtractor.FilterResults(this._sdmxObjects.GetCodelists(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get concept scheme.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IConceptSchemeObject"/>.
        /// </returns>
        public virtual IConceptSchemeObject GetConceptScheme(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IConceptSchemeObject>.ResolveReference(this._sdmxObjects.ConceptSchemes, xref);
            return (IConceptSchemeObject)maint;
        }

        /// <summary>
        /// Returns a single ConceptScheme , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IConceptSchemeObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IConceptSchemeObject GetConceptScheme(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IConceptSchemeObject>.ResolveReference(this._sdmxObjects.ConceptSchemes, xref));
                return (IConceptSchemeObject)maint;
            }
            else
                return GetConceptScheme(xref);
        }

        /// <summary>
        /// The get concept scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IConceptSchemeObject> GetConceptSchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetConceptSchemes(xref);
        }

        /// <summary>
        /// Returns ConceptSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IConceptSchemeObject> GetConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._conceptSchemeExtractor.FilterResults(this._sdmxObjects.GetConceptSchemes(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get content constraint.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/>.
        /// </returns>
        public virtual IContentConstraintObject GetContentConstraint(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IContentConstraintObject>.ResolveReference(this._sdmxObjects.ContentConstraintObjects, xref);
            return (IContentConstraintObject)maint;
        }

        /// <summary>
        /// Returns a single content constraint, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IContentConstraintObject GetContentConstraint(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IContentConstraintObject>.ResolveReference(this._sdmxObjects.ContentConstraintObjects, xref));
                return (IContentConstraintObject)maint;
            }
            else
                return GetContentConstraint(xref);
        }

        /// <summary>
        /// The get content constraints.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IContentConstraintObject> GetContentConstraints(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetContentConstraintObjects(xref);
        }

        /// <summary>
        /// Returns ContentConstraintObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IContentConstraintObject> GetContentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._contentConstraintExtractor.FilterResults(this._sdmxObjects.GetContentConstraintObjects(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get data consumer scheme bean.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IDataConsumerScheme"/>.
        /// </returns>
        public virtual IDataConsumerScheme GetDataConsumerSchemeObject(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IDataConsumerScheme>.ResolveReference(this._sdmxObjects.DataConsumerSchemes, xref);
            return (IDataConsumerScheme)maint;
        }

        /// <summary>
        /// Returns a single data consumer scheme , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IDataConsumerScheme"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IDataConsumerScheme GetDataConsumerSchemeObject(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IDataConsumerScheme>.ResolveReference(this._sdmxObjects.DataConsumerSchemes, xref));
                return (IDataConsumerScheme)maint;
            }
            else
                return GetDataConsumerSchemeObject(xref);
        }

        /// <summary>
        /// The get data consumer scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IDataConsumerScheme> GetDataConsumerSchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetDataConsumerSchemes(xref);
        }

        /// <summary>
        /// Returns DataConsumerSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all DataConsumerSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataConsumerScheme> GetDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            return this._dataConsumerSchemeExtractor.FilterResults(this._sdmxObjects.GetDataConsumerSchemes(xref), false, returnStub);
        }

        /// <summary>
        /// The get data provider scheme bean.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IDataProviderScheme"/>.
        /// </returns>
        public virtual IDataProviderScheme GetDataProviderSchemeObject(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IDataProviderScheme>.ResolveReference(this._sdmxObjects.DataProviderSchemes, xref);
            return (IDataProviderScheme)maint;
        }

        /// <summary>
        /// Returns a single data provider scheme , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IDataProviderScheme"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IDataProviderScheme GetDataProviderSchemeObject(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IDataProviderScheme>.ResolveReference(this._sdmxObjects.DataProviderSchemes, xref));
                return (IDataProviderScheme)maint;
            }
            else
                return GetDataProviderSchemeObject(xref);
        }

        /// <summary>
        /// The get data provider scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IDataProviderScheme> GetDataProviderSchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetDataProviderSchemes(xref);
        }

        /// <summary>
        /// Returns DataProviderSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all DataProviderSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataProviderScheme> GetDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            return this._dataProviderSchemeExtractor.FilterResults(this._sdmxObjects.GetDataProviderSchemes(xref), false, returnStub);
        }

        /// <summary>
        /// The get data structure.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureObject"/>.
        /// </returns>
        public virtual IDataStructureObject GetDataStructure(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IDataStructureObject>.ResolveReference(this._sdmxObjects.DataStructures, xref);
            return (IDataStructureObject)maint;
        }

        /// <summary>
        /// Returns a single DataStructure.
        /// This expects the ref object to contain all the attributes required to uniquely identify the object.
        /// If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IDataStructureObject GetDataStructure(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IDataStructureObject>.ResolveReference(this._sdmxObjects.DataStructures, xref));
                return (IDataStructureObject)maint;
            }
            else
                return GetDataStructure(xref);
        }

        /// <summary>
        /// The get data structure beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IDataStructureObject> GetDataStructureObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetDataStructures(xref);
        }

        /// <summary>
        /// Returns DataStructureObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all DataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataStructureObject> GetDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._dataStructureExtractor.FilterResults(this._sdmxObjects.GetDataStructures(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get dataflow.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IDataflowObject"/>.
        /// </returns>
        public virtual IDataflowObject GetDataflow(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IDataflowObject>.ResolveReference(this._sdmxObjects.Dataflows, xref);
            return (IDataflowObject)maint;
        }

        /// <summary>
        /// Returns a single Dataflow , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IDataflowObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IDataflowObject GetDataflow(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IDataflowObject>.ResolveReference(this._sdmxObjects.Dataflows, xref));
                return (IDataflowObject)maint;
            }
            else
                return GetDataflow(xref);
        }

        /// <summary>
        /// The get dataflow beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IDataflowObject> GetDataflowObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetDataflows(xref);
        }

        public override ISet<IDataflowObject> GetDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._dataflowExtractor.FilterResults(this._sdmxObjects.GetDataflows(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get hierarchic code list.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistObject"/>.
        /// </returns>
        public virtual IHierarchicalCodelistObject GetHierarchicCodeList(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IHierarchicalCodelistObject>.ResolveReference(this._sdmxObjects.HierarchicalCodelists, xref);
            return (IHierarchicalCodelistObject)maint;
        }

        /// <summary>
        /// Returns a single HierarchicCodeList , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IHierarchicalCodelistObject GetHierarchicCodeList(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IHierarchicalCodelistObject>.ResolveReference(this._sdmxObjects.HierarchicalCodelists, xref));
                return (IHierarchicalCodelistObject)maint;
            }
            else
                return GetHierarchicCodeList(xref);
        }

        /// <summary>
        /// The get hierarchic code list beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IHierarchicalCodelistObject> GetHierarchicCodeListObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetHierarchicalCodelists(xref);
        }

        /// <summary>
        /// Returns HierarchicalCodelistObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IHierarchicalCodelistObject> GetHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._hierarchicalCodelistExtractor.FilterResults(this._sdmxObjects.GetHierarchicalCodelists(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a maintainable defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="sRref">
        /// The query.
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual IMaintainableObject GetMaintainable(IStructureReference sRref, bool returnStub)
        {
            IMaintainableRefObject xref = sRref.MaintainableReference;
            switch (sRref.MaintainableStructureEnumType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return this.GetAgencySchemeObject(xref);
                case SdmxStructureEnumType.AttachmentConstraint:
                    return this.GetAttachmentConstraint(xref);
                case SdmxStructureEnumType.ContentConstraint:
                    return this.GetContentConstraint(xref);
                case SdmxStructureEnumType.DataConsumerScheme:
                    return this.GetDataConsumerSchemeObject(xref);
                case SdmxStructureEnumType.DataProviderScheme:
                    return this.GetDataProviderSchemeObject(xref);
                case SdmxStructureEnumType.Categorisation:
                    return this.GetCategorisation(xref);
                case SdmxStructureEnumType.CategoryScheme:
                    return this.GetCategoryScheme(xref);
                case SdmxStructureEnumType.CodeList:
                    return this.GetCodelist(xref);
                case SdmxStructureEnumType.ConceptScheme:
                    return this.GetConceptScheme(xref);
                case SdmxStructureEnumType.Dataflow:
                    return this.GetDataflow(xref);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return this.GetHierarchicCodeList(xref);
                case SdmxStructureEnumType.Dsd:
                    return this.GetDataStructure(xref);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.GetMetadataflow(xref);
                case SdmxStructureEnumType.Msd:
                    return this.GetMetadataStructure(xref);
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return this.GetOrganisationUnitScheme(xref);
                case SdmxStructureEnumType.Process:
                    return this.GetProcessObject(xref);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return this.GetReportingTaxonomy(xref);
                case SdmxStructureEnumType.StructureSet:
                    return this.GetStructureSet(xref);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return this.GetProvisionAgreementObject(xref);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, sRref.TargetReference);
            }
        }


        /// <summary>
        /// The get maintainable with references.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IMaintainableObject> GetMaintainableWithReferences(IStructureReference query)
        {
            ISet<IMaintainableObject> maintainables = this.GetMaintainables(query);
            ISdmxObjects beans0 = new SdmxObjectsImpl();
            beans0.AddIdentifiables(maintainables);
            this.ResolveReferences(beans0);
            return beans0.GetAllMaintainables();
        }

        /// <summary>
        /// The get metadata structure.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IMetadataStructureDefinitionObject"/>.
        /// </returns>
        public virtual IMetadataStructureDefinitionObject GetMetadataStructure(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IMetadataStructureDefinitionObject>.ResolveReference(this._sdmxObjects.MetadataStructures, xref);
            return (IMetadataStructureDefinitionObject)maint;
        }

        /// <summary>
        /// Returns a single MetadataStructure , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IMetadataStructureDefinitionObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IMetadataStructureDefinitionObject GetMetadataStructure(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IMetadataStructureDefinitionObject>.ResolveReference(this._sdmxObjects.MetadataStructures, xref));
                return (IMetadataStructureDefinitionObject)maint;
            }
            else
                return GetMetadataStructure(xref);
        }

        /// <summary>
        /// The get metadata structure beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IMetadataStructureDefinitionObject> GetMetadataStructureObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetMetadataStructures(xref);
        }

        /// <summary>
        /// Returns MetadataStructureObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataStructureDefinitionObject> GetMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._metadataStructureDefinitionExtractor.FilterResults(this._sdmxObjects.GetMetadataStructures(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get metadataflow.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IMetadataFlow"/>.
        /// </returns>
        public virtual IMetadataFlow GetMetadataflow(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IMetadataFlow>.ResolveReference(this._sdmxObjects.Metadataflows, xref);
            return (IMetadataFlow)maint;
        }

        /// <summary>
        /// Returns a single Metadataflow , this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IMetadataFlow"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IMetadataFlow GetMetadataflow(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IMetadataFlow>.ResolveReference(this._sdmxObjects.Metadataflows, xref));
                return (IMetadataFlow)maint;
            }
            else
                return GetMetadataflow(xref);
        }

        /// <summary>
        /// The get metadataflow beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IMetadataFlow> GetMetadataflowObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetMetadataflows(xref);
        }

        /// <summary>
        /// Returns MetadataFlowObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataFlow> GetMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._metadataFlowExtractor.FilterResults(this._sdmxObjects.GetMetadataflows(xref), returnLatest, returnStub);
        }

        /// <summary>
        ///     Returns a copy of the underlying beans for this retrieval Manager
        /// </summary>
        /// <returns>
        ///     The <see cref="ISdmxObjects" />.
        /// </returns>
        public ISdmxObjects GetObjects()
        {
            return new SdmxObjectsImpl(this._sdmxObjects);
        }

        /// <summary>
        /// The get organisation unit scheme.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IOrganisationUnitSchemeObject"/>.
        /// </returns>
        public virtual IOrganisationUnitSchemeObject GetOrganisationUnitScheme(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IOrganisationUnitSchemeObject>.ResolveReference(this._sdmxObjects.OrganisationUnitSchemes, xref);
            return (IOrganisationUnitSchemeObject)maint;
        }

        /// <summary>
        /// Returns a single organisation unit scheme, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IOrganisationUnitSchemeObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IOrganisationUnitSchemeObject GetOrganisationUnitScheme(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IOrganisationUnitSchemeObject>.ResolveReference(this._sdmxObjects.OrganisationUnitSchemes, xref));
                return (IOrganisationUnitSchemeObject)maint;
            }
            else
                return GetOrganisationUnitScheme(xref);
        }

        /// <summary>
        /// The get organisation unit scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemeObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetOrganisationUnitSchemes(xref);
        }

        /// <summary>
        /// Returns OrganisationUnitSchemeObjects that match the parameters in the ref object.  If the ref object is null or 
        /// has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._organisationUnitSchemeExtractor.FilterResults(this._sdmxObjects.GetOrganisationUnitSchemes(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get process bean.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IProcessObject"/>.
        /// </returns>
        public virtual IProcessObject GetProcessObject(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IProcessObject>.ResolveReference(this._sdmxObjects.Processes, xref);
            return (IProcessObject)maint;
        }

        /// <summary>
        /// Returns a process bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IProcessObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IProcessObject GetProcessObject(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IProcessObject>.ResolveReference(this._sdmxObjects.Processes, xref));
                return (IProcessObject)maint;
            }
            else
                return GetProcessObject(xref);
        }

        /// <summary>
        /// The get process beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IProcessObject> GetProcessObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetProcesses(xref);
        }

        /// <summary>
        /// Returns ProcessObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all ProcessObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProcessObject> GetProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._processBeanExtractor.FilterResults(this._sdmxObjects.GetProcesses(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get provision agreement bean.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IProvisionAgreementObject"/>.
        /// </returns>
        public virtual IProvisionAgreementObject GetProvisionAgreementObject(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IProvisionAgreementObject>.ResolveReference(this._sdmxObjects.ProvisionAgreements, xref);
            return (IProvisionAgreementObject)maint;
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IProvisionAgreementObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IProvisionAgreementObject GetProvisionAgreementObject(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IProvisionAgreementObject>.ResolveReference(this._sdmxObjects.ProvisionAgreements, xref));
                return (IProvisionAgreementObject)maint;
            }
            else
                return GetProvisionAgreementObject(xref);
        }

        /// <summary>
        /// The get provision agreement beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IProvisionAgreementObject> GetProvisionAgreementObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetProvisionAgreements(xref);
        }

        // <summary>
        /// The get subscription agreement objects.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}<ISubscriptionObject>"/>.
        /// </returns>
        public override ISet<ISubscriptionObject> GetSubscriptionObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return _subscriptionExtractor.FilterResults(_sdmxObjects.GetSubscriptions(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// Returns ProvisionAgreementObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all ProvisionAgreementObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProvisionAgreementObject> GetProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._provisionAgreementExtractor.FilterResults(this._sdmxObjects.GetProvisionAgreements(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get reporting taxonomy.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IReportingTaxonomyObject"/>.
        /// </returns>
        public virtual IReportingTaxonomyObject GetReportingTaxonomy(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IReportingTaxonomyObject>.ResolveReference(this._sdmxObjects.ReportingTaxonomys, xref);
            return (IReportingTaxonomyObject)maint;
        }

        /// <summary>
        /// Returns a reporting taxonomy bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        ///  If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IReportingTaxonomyObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IReportingTaxonomyObject GetReportingTaxonomy(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IReportingTaxonomyObject>.ResolveReference(this._sdmxObjects.ReportingTaxonomys, xref));
                return (IReportingTaxonomyObject)maint;
            }
            else
                return GetReportingTaxonomy(xref);
        }

        /// <summary>
        /// The get reporting taxonomy beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IReportingTaxonomyObject> GetReportingTaxonomyObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetReportingTaxonomys(xref);
        }

        /// <summary>
        /// Returns ReportingTaxonomyObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IReportingTaxonomyObject> GetReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._reportingTaxonomyExtractor.FilterResults(this._sdmxObjects.GetReportingTaxonomys(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// The get structure set.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="IStructureSetObject"/>.
        /// </returns>
        public virtual IStructureSetObject GetStructureSet(IMaintainableRefObject xref)
        {
            IMaintainableObject maint = MaintainableUtil<IStructureSetObject>.ResolveReference(this._sdmxObjects.StructureSets, xref);
            return (IStructureSetObject)maint;
        }

        /// <summary>
        /// Returns a structure set bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, this is expected to uniquely identify one object
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IStructureSetObject"/> .
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented.
        /// </exception>
        public virtual IStructureSetObject GetStructureSet(IMaintainableRefObject xref, bool returnStub)
        {
            if (returnStub)
            {
                IMaintainableObject maint = _serviceRetrievalManager.CreateStub(MaintainableUtil<IStructureSetObject>.ResolveReference(this._sdmxObjects.StructureSets, xref));
                return (IStructureSetObject)maint;
            }
            else
                return GetStructureSet(xref);
        }

        /// <summary>
        /// The get structure set beans.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference..
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        public virtual ISet<IStructureSetObject> GetStructureSetObjects(IMaintainableRefObject xref)
        {
            return this._sdmxObjects.GetStructureSets(xref);
        }

        /// <summary>
        /// Returns StructureSetObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        /// then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IStructureSetObject> GetStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._structureSetExtractor.FilterResults(this._sdmxObjects.GetStructureSets(xref), returnLatest, returnStub);
        }

        /// <summary>
        /// Saves the maintainable
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        public void SaveStructure(IMaintainableObject maintainable)
        {
            _log.Info("saveStructure:" + maintainable.Urn);
            this._sdmxObjects.AddIdentifiable(maintainable);
        }

        /// <summary>
        /// The save structures.
        /// </summary>
        /// <param name="beans0">
        /// The beans 0.
        /// </param>
        public virtual void SaveStructures(ISdmxObjects beans0)
        {
            _log.Info("saveStructure:" + beans0.ToString());
            this._sdmxObjects.Merge(beans0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get maintainables.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableObject}"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// <paramref name="query"/> is not for a maintainable.
        /// </exception>
        private ISet<IMaintainableObject> GetMaintainables(IStructureReference query)
        {
            if (!query.TargetReference.IsMaintainable)
            {
                throw new SdmxNotImplementedException(ExceptionCode.Unsupported, query.TargetReference + " is not maintainable");
            }

            IMaintainableRefObject xref = query.MaintainableReference;
            switch (query.TargetReference.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return new HashSet<IMaintainableObject>(this.GetAgencySchemeObjects(xref));
                case SdmxStructureEnumType.DataConsumerScheme:
                    return new HashSet<IMaintainableObject>(this.GetDataConsumerSchemeObjects(xref));
                case SdmxStructureEnumType.AttachmentConstraint:
                    return new HashSet<IMaintainableObject>(this.GetAttachmentConstraints(xref));
                case SdmxStructureEnumType.ContentConstraint:
                    return new HashSet<IMaintainableObject>(this.GetContentConstraints(xref));
                case SdmxStructureEnumType.DataProviderScheme:
                    return new HashSet<IMaintainableObject>(this.GetDataProviderSchemeObjects(xref));
                case SdmxStructureEnumType.Categorisation:
                    return new HashSet<IMaintainableObject>(this.GetCategorisationObjects(xref));
                case SdmxStructureEnumType.CategoryScheme:
                    return new HashSet<IMaintainableObject>(this.GetCategorySchemeObjects(xref));
                case SdmxStructureEnumType.CodeList:
                    return new HashSet<IMaintainableObject>(this.GetCodelistObjects(xref));
                case SdmxStructureEnumType.ConceptScheme:
                    return new HashSet<IMaintainableObject>(this.GetConceptSchemeObjects(xref));
                case SdmxStructureEnumType.Dataflow:
                    return new HashSet<IMaintainableObject>(this.GetDataflowObjects(xref));
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return new HashSet<IMaintainableObject>(this.GetHierarchicCodeListObjects(xref));
                case SdmxStructureEnumType.Dsd:
                    return new HashSet<IMaintainableObject>(this.GetDataStructureObjects(xref));
                case SdmxStructureEnumType.MetadataFlow:
                    return new HashSet<IMaintainableObject>(this.GetMetadataflowObjects(xref));
                case SdmxStructureEnumType.Msd:
                    return new HashSet<IMaintainableObject>(this.GetMetadataStructureObjects(xref));
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return new HashSet<IMaintainableObject>(this.GetOrganisationUnitSchemeObjects(xref));
                case SdmxStructureEnumType.Process:
                    return new HashSet<IMaintainableObject>(this.GetProcessObjects(xref));
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return new HashSet<IMaintainableObject>(this.GetReportingTaxonomyObjects(xref));
                case SdmxStructureEnumType.StructureSet:
                    return new HashSet<IMaintainableObject>(this.GetStructureSetObjects(xref));
                case SdmxStructureEnumType.ProvisionAgreement:
                    return new HashSet<IMaintainableObject>(this.GetProvisionAgreementObjects(xref));
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, query.TargetReference);
            }
        }

        /// <summary>
        /// The resolve references.
        /// </summary>
        /// <param name="beans0">
        /// The beans 0.
        /// </param>
        private void ResolveReferences(ISdmxObjects beans0)
        {
            if (this._crossRefControl != null)
            {
                _log.Debug("resolve references for beans use crossRefControl");

                int numberBeansCurrent = -1;

                ISdmxObjects allBeans = beans0;
                ISdmxObjects referencesToResolve = beans0;
                ISdmxObjects newReferencesToResolve = new SdmxObjectsImpl();

                int numberBeansLast;
                do
                {
                    numberBeansLast = numberBeansCurrent;

                    ISet<string> agencyReferences = new HashSet<string>(StringComparer.Ordinal);

                    foreach (IMaintainableObject maint in referencesToResolve.GetAllMaintainables())
                    {
                        agencyReferences.Add(maint.AgencyId);
                        newReferencesToResolve.AddIdentifiables(this._crossRefControl.GetCrossReferencedStructures(maint, false));

                        foreach (IIdentifiableObject ident in maint.IdentifiableComposites)
                        {
                            newReferencesToResolve.AddIdentifiables(this._crossRefControl.GetCrossReferencedStructures(ident, false));
                        }
                    }

                    foreach (string currentRef in agencyReferences)
                    {
                        foreach (IAgency acy in this._sdmxObjects.Agencies)
                        {
                            if (acy.FullId.Equals(currentRef))
                            {
                                newReferencesToResolve.AddIdentifiable(acy);
                                break;
                            }
                        }
                    }

                    allBeans.Merge(newReferencesToResolve);
                    referencesToResolve = newReferencesToResolve;
                    numberBeansCurrent = allBeans.GetAllMaintainables().Count;
                }
                while (numberBeansCurrent != numberBeansLast);
            }
            else
            {
                _log.Debug("resolve references for beans create new cross reference resolver engine");
                ICrossReferenceResolverEngine resolver = new CrossReferenceResolverEngineCore();
                IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> crossReferenceMap = resolver.ResolveReferences(beans0, false, 0, this);

                foreach (var keyValuePair in crossReferenceMap)
                {
                    beans0.AddIdentifiable(keyValuePair.Key);

                    foreach (IIdentifiableObject valueren in keyValuePair.Value)
                    {
                        beans0.AddIdentifiable(valueren);
                    }
                }
            }

            _log.Debug("resolve references complete");
        }

        #endregion


        #region Internal Classes

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">
        /// The maintainable object
        /// </typeparam>
        /* @SuppressWarnings("deprecation")*/
        private class ResultExtractor<T> where T : IMaintainableObject
        {
            #region Fields

            private readonly MaintainableUtil<T> _maintainableUtil;

            #endregion


            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ResultExtractor{T}"/> class.
            ///     Default constructor
            /// </summary>
            public ResultExtractor()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ResultExtractor{T}"/> class.
            /// </summary>
            /// <param name="maintainableUtil">
            /// The maintainable util
            /// </param>
            public ResultExtractor(MaintainableUtil<T> maintainableUtil)
            {
                this._maintainableUtil = maintainableUtil;
            }

            #endregion

            /// <summary>
            /// TODO
            /// </summary>
            /// <param name="results">
            /// The results set
            /// </param>
            /// <param name="returnLatest">
            /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
            /// then it will be ignored
            /// </param>
            /// <param name="returnStub">
            /// If true then a stub object will be returned
            /// </param>
            /// <returns>
            /// The set of maintainable objects
            /// </returns>
            public ISet<T> FilterResults(ISet<T> results, bool returnLatest, bool returnStub)
            {

                if (returnLatest && this._maintainableUtil != null)
                {
                    results = this._maintainableUtil.FilterCollectionGetLatestOfType(results);
                }

                if (returnStub && _serviceRetrievalManager != null)
                {
                    ISet<T> newSet = new HashSet<T>();
                    foreach (T result in results)
                    {
                        newSet.Add((T)_serviceRetrievalManager.CreateStub(result));
                    }

                    results = newSet;
                }

                return results;
            }
        }

        #endregion
    }
}
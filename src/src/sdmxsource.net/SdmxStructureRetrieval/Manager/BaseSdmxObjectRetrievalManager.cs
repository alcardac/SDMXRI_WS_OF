// -----------------------------------------------------------------------
// <copyright file="BaseSdmxObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2014-04-17
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.CrossReference;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
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
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.Util.Extension;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    /// <summary>
    /// The base SDMX object retrieval manager.
    /// </summary>
    public abstract class BaseSdmxObjectRetrievalManager : IdentifiableRetrievalManagerCore, ISdmxObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BaseSdmxObjectRetrievalManager));

        /// <summary>
        /// The service retrieval manager
        /// </summary>
        protected static IServiceRetrievalManager _serviceRetrievalManager;

        /// <summary>
        /// The external service retrieval manager
        /// </summary>
        private static IExternalReferenceRetrievalManager _externalReferenceRetrievalManager;

        /// <summary>
        /// The header retrieval manager
        /// </summary>
        private static HeaderRetrievalManager _headerRetrievalManager;

        /// <summary>
        /// The cross referenced retrieval manager
        /// </summary>
        private readonly ICrossReferencedRetrievalManager _crossReferenceRetrievalManager;

        /// <summary>
        /// The cross referencing retrieval manager
        /// </summary>
        protected ICrossReferencingRetrievalManager _crossReferencingRetrievalManager;

        /// <summary>
        /// The  retrieval manager
        /// </summary>
        private ISdmxObjectRetrievalManager _proxy;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSdmxObjectRetrievalManager"/> class.
        ///     Default constructor
        /// </summary>
        protected BaseSdmxObjectRetrievalManager()
        {
            this.RetrievalManager = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSdmxObjectRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The <see cref="ISdmxObjectRetrievalManager"/> retrieval manager.
        /// </param>
        protected BaseSdmxObjectRetrievalManager(ISdmxObjectRetrievalManager retrievalManager)
            : base(null, retrievalManager)
        {
        }

        #endregion

        /// <summary>
        /// Gets a maintainable that is of the given type, determined by T, and matches the reference parameters in the IMaintainableRefObject.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="maintainableReference">
        /// The reference object that must match on the returned structure. If version information is missing, then latest version is assumed
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual T GetMaintainableObject<T>(IMaintainableRefObject maintainableReference) where T : IMaintainableObject
        {
            return GetMaintainableObject<T>(maintainableReference, false, false);
        }

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        /// An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <typeparam name="T">The type of the maintainable. It is constraint  </typeparam>
        /// <param name="maintainableInterface">The maintainable interface.</param>
        /// <param name="maintainableReference">Contains the identifiers of the structures to returns, can include wild-carded values (null indicates a wild-card).</param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject" /> .
        /// </returns>
        /// <remarks>This method exists only for compatibility reasons with Java implementation of this interface which uses raw types and unchecked generics.</remarks>
        public virtual ISet<T> GetMaintainableObjects<T>(Type maintainableInterface, IMaintainableRefObject maintainableReference) where T : ISdmxObject
        {
            SdmxStructureType type = SdmxStructureType.ParseClass(maintainableInterface);
            return new HashSet<T>(GetMaintainablesOfType<IMaintainableObject>(type, maintainableReference, false, false).Cast<T>());
        }

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        /// An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <typeparam name="T">The type of the maintainable. It is constraint</typeparam>
        /// <param name="maintainableInterface">The maintainable interface.</param>
        /// <param name="maintainableReference">Contains the identifiers of the structures to returns, can include wild-carded values (null indicates a wild-card).</param>
        /// <param name="returnStub">if set to <c>true</c> [return stub].</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject" /> .
        /// </returns>
        /// <remarks>
        /// This method exists only for compatibility reasons with Java implementation of this interface which uses raw types and unchecked generics.
        /// </remarks>
        public virtual ISet<T> GetMaintainableObjects<T>(Type maintainableInterface, IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject
        {
            SdmxStructureType type = SdmxStructureType.ParseClass(maintainableInterface);
            return this.GetMaintainablesOfType<T>(type, maintainableReference, returnStub, returnLatest);
        }

        /// <summary>
        /// Gets a maintainable defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="structureReference">
        /// The reference object defining the search parameters, this is expected to uniquely identify one MaintainableObject
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual IMaintainableObject GetMaintainableObject(IStructureReference structureReference)
        {
            return GetMaintainableObject(structureReference, false, false);
        }


        /// <summary>
        /// Gets the SDMX objects.
        /// </summary>
        /// <param name="structureReference">The <see cref="IStructureReference"/> which must not be null.</param>
        /// <param name="resolveCrossReferences">either 'do not resolve', 'resolve all' or 'resolve all excluding agencies'. If not set to 'do not resolve' then all the structures that are referenced by the resulting structures are also returned (and also their children).  This will be equivalent to descendants for a <c>RESTful</c> query..</param>
        /// <returns>Returns a <see cref="ISdmxObjects"/> container containing all the Maintainable Objects that match the query parameters as defined by the <paramref name="structureReference"/>.</returns>
        public virtual ISdmxObjects GetSdmxObjects(IStructureReference structureReference, ResolveCrossReferences resolveCrossReferences)
        {
            ISet<IMaintainableObject> maintainables = this.GetMaintainablesOfType<IMaintainableObject>(SdmxStructureType.ParseClass(structureReference.MaintainableStructureEnumType.MaintainableInterface),
                                                                structureReference.MaintainableReference, false, false);
            ISdmxObjects beans = new SdmxObjectsImpl();
            beans.AddIdentifiables(maintainables);

            switch (resolveCrossReferences)
            {
                case ResolveCrossReferences.DoNotResolve:
                    break;
                case ResolveCrossReferences.ResolveAll:
                    this.ResolveReferences(beans, true);
                    break;
                case ResolveCrossReferences.ResolveExcludeAgencies:
                    this.ResolveReferences(beans, false);
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Unknown condition encountered for resolveCrossReferences. Value: " + resolveCrossReferences);
            }

            return beans;
        }

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T
        ///     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual ISet<T> GetMaintainableObjects<T>() where T : IMaintainableObject
        {
            return GetMaintainableObjects<T>(null);
        }

        /// <summary>
        /// Gets a maintainable defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="structureReference">
        /// The reference object defining the search parameters, this is expected to uniquely identify one MaintainableObject
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual IMaintainableObject GetMaintainableObject(IStructureReference structureReference, bool returnStub, bool returnLatest)
        {
            if (structureReference == null)
                throw new ArgumentException("GetMaintainableObject was passed a null StructureReferenceBean this is not allowed");

            return ExtractFromSet(GetMaintainableObjects<IMaintainableObject>(structureReference.MaintainableStructureEnumType.MaintainableInterface, structureReference.MaintainableReference, returnStub, returnLatest));
        }

        /// <summary>
        /// Gets a maintainable that is of the given type, determined by T, and matches the reference parameters in the IMaintainableRefObject.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="maintainableReference">
        /// The reference object that must match on the returned structure. If version information is missing, then latest version is assumed
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// /// <param name="returnLatest">
        /// If true then the latest version is returned, regardless of whether version information is supplied
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual T GetMaintainableObject<T>(IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject
        {
            if (!returnLatest)
                returnLatest = !ObjectUtil.ValidObject(maintainableReference.Version);

            return ExtractFromSet(GetMaintainableObjects<T>(maintainableReference, returnStub, returnLatest));
        }


        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        //     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <param name="maintainableReference">
        /// Contains the identifiers of the structures to returns, can include widcarded values (null indicates a wildcard). 
        /// </param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual ISet<T> GetMaintainableObjects<T>(IMaintainableRefObject maintainableReference) where T : IMaintainableObject
        {
            return GetMaintainableObjects<T>(maintainableReference, false, false);
        }


        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        //     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <param name="maintainableReference">
        /// Contains the identifiers of the structures to returns, can include widcarded values (null indicates a wildcard). 
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// /// <param name="returnLatest">
        /// If true then the latest version is returned, regardless of whether version information is supplied
        /// </param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        public virtual ISet<T> GetMaintainableObjects<T>(IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject
        {
            ISet<T> returnSet;

            if (returnLatest)
                maintainableReference = new MaintainableRefObjectImpl(maintainableReference.AgencyId, maintainableReference.MaintainableId, null);

            SdmxStructureType type = SdmxStructureType.ParseClass(typeof(T));
            IStructureReference sRef = new StructureReferenceImpl(maintainableReference, type);
            switch (sRef.TargetReference.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    returnSet = new HashSet<T>(this.GetAgencySchemeObjects(maintainableReference, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    returnSet = new HashSet<T>(this.GetDataConsumerSchemeObjects(maintainableReference, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.AttachmentConstraint:
                    returnSet = new HashSet<T>(this.GetAttachmentConstraints(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.ContentConstraint:
                    returnSet = new HashSet<T>(this.GetContentConstraints(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    returnSet = new HashSet<T>(this.GetDataProviderSchemeObjects(maintainableReference, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.Categorisation:
                    returnSet = new HashSet<T>(this.GetCategorisationObjects(maintainableReference, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    returnSet = new HashSet<T>(this.GetCategorySchemeObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.CodeList:
                    returnSet = new HashSet<T>(this.GetCodelistObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    returnSet = new HashSet<T>(this.GetConceptSchemeObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.Dataflow:
                    returnSet = new HashSet<T>(this.GetDataflowObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    returnSet = new HashSet<T>(this.GetHierarchicCodeListObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.Dsd:
                    returnSet = new HashSet<T>(this.GetDataStructureObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    returnSet = new HashSet<T>(this.GetMetadataflowObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.Msd:
                    returnSet = new HashSet<T>(this.GetMetadataStructureObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    returnSet = new HashSet<T>(this.GetOrganisationUnitSchemeObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.Process:
                    returnSet = new HashSet<T>(this.GetProcessObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    returnSet = new HashSet<T>(this.GetReportingTaxonomyObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.StructureSet:
                    returnSet = new HashSet<T>(this.GetStructureSetObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                case SdmxStructureEnumType.ProvisionAgreement:
                    returnSet = new HashSet<T>(this.GetProvisionAgreementObjects(maintainableReference, returnLatest, returnStub).Cast<T>());
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, sRef.TargetReference);
            }

            if (returnStub && _serviceRetrievalManager != null)
            {
                ISet<T> stubSet = new HashSet<T>();
                foreach (T returnItm in returnSet)
                {
                    if (returnItm.IsExternalReference.IsTrue)
                        stubSet.Add(returnItm);
                    else
                        stubSet.Add((T)_serviceRetrievalManager.CreateStub(returnItm));
                }
                returnSet = stubSet;
            }
            return returnSet;
        }

        /// <summary>
        /// Gets a set of all MaintainableObjects of type xType that match the reference parameters in the IMaintainableRefObject argument.
        //     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <param name="xType">
        /// Contains sdmx structure enum type
        /// </param>
        /// <param name="maintainableReference">
        /// Contains the identifiers of the structures to returns, can include widcarded values (null indicates a wildcard). 
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// /// <param name="returnLatest">
        /// If true then the latest version is returned, regardless of whether version information is supplied
        /// </param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        private ISet<T> GetMaintainablesOfType<T>(SdmxStructureEnumType xType, IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject
        {
            ISet<T> returnSet;

            if (returnLatest)
                maintainableReference = new MaintainableRefObjectImpl(maintainableReference.AgencyId, maintainableReference.MaintainableId, null);

            switch (xType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IAgencyScheme>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IDataConsumerScheme>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.AttachmentConstraint:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IAttachmentConstraintObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.ContentConstraint:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IContentConstraintObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IDataProviderScheme>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.Categorisation:
                    returnSet = new HashSet<T>(GetMaintainableObjects<ICategorisationObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<ICategorySchemeObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.CodeList:
                    returnSet = new HashSet<T>(GetMaintainableObjects<ICodelistObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IConceptSchemeObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.Dataflow:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IDataflowObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IHierarchicalCodelistObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.Dsd:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IDataStructureObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IMetadataFlow>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.Msd:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IMetadataStructureDefinitionObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IOrganisationUnitSchemeObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.Process:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IProcessObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IReportingTaxonomyObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.StructureSet:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IStructureSetObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                case SdmxStructureEnumType.ProvisionAgreement:
                    returnSet = new HashSet<T>(GetMaintainableObjects<IProvisionAgreementObject>(maintainableReference, returnStub, returnLatest).Cast<T>());
                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, xType);
            }

            if (returnStub && _serviceRetrievalManager != null)
            {
                ISet<T> stubSet = new HashSet<T>();
                foreach (T returnItm in returnSet)
                {
                    if (returnItm.IsExternalReference.IsTrue)
                        stubSet.Add(returnItm);
                    else
                        stubSet.Add((T)_serviceRetrievalManager.CreateStub(returnItm));
                }
                returnSet = stubSet;
            }
            return returnSet;
        }

        /// <summary>
        /// Gets all the maintainable objects in this container, returns an empty set if no maintainable objects exist in this container
        /// </summary>
        /// <returns>
        /// set of all maintainable objects, minus any which match the optional exclude parameters
        /// </returns>
        /// <param name="exclude">
        /// do not return the maintainable objects which match the optional exclude parameters
        /// </param>
        protected virtual ISet<IMaintainableObject> GetAllMaintainables(IMaintainableRefObject xref, bool returnLatest, bool returnStubs)
        {
            ISet<IMaintainableObject> q = new HashSet<IMaintainableObject>();
            foreach (SdmxStructureType currentMaintainable in SdmxStructureType.MaintainableStructureTypes)
            {
                if (currentMaintainable != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration) &&
                    currentMaintainable != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription))
                {
                    foreach (IMaintainableObject mr in GetMaintainablesOfType<IMaintainableObject>(currentMaintainable.EnumType, xref, returnStubs, returnLatest))
                        q.Add(mr);
                }
            }
            return q;
        }

        /// <summary>
        /// Get all the maintainable that match the <paramref name="restquery"/>
        /// </summary>
        /// <param name="restquery">The REST structure query.</param>
        /// <returns>the maintainable that match the <paramref name="restquery"/></returns>
        public virtual ISdmxObjects GetMaintainables(IRestStructureQuery restquery)
        {
            _log.Info("Query for maintainables: " + restquery);

            bool isAllStubs = restquery.StructureQueryMetadata.StructureQueryDetail == StructureQueryDetailEnumType.AllStubs;
            bool isRefStubs = isAllStubs || restquery.StructureQueryMetadata.StructureQueryDetail == StructureQueryDetailEnumType.ReferencedStubs;
            bool isLatest = restquery.StructureQueryMetadata.IsReturnLatest;

            bool retrieveStubs = false;
            SdmxStructureType type = restquery.StructureReference.MaintainableStructureEnumType;

            //if the type is NULL we'll ask for ANY SdmxStructureType
            if (type == null)
                type = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any);

            IMaintainableRefObject xref = restquery.StructureReference.ChangeStarsToNull().MaintainableReference;

            ISet<IMaintainableObject> queryResultMaintainables;

            if (type.EnumType == SdmxStructureEnumType.Any)
            {
                queryResultMaintainables = GetAllMaintainables(xref, isLatest, retrieveStubs);

            }
            else if (type.EnumType == SdmxStructureEnumType.OrganisationScheme)
            {
                ISdmxObjects beans = new SdmxObjectsImpl();
                beans.AddIdentifiables(GetMaintainableObjects<IAgencyScheme>(xref, retrieveStubs, isLatest));
                beans.AddIdentifiables(GetMaintainableObjects<IDataProviderScheme>(xref, retrieveStubs, isLatest));
                beans.AddIdentifiables(GetMaintainableObjects<IOrganisationUnitSchemeObject>(xref, retrieveStubs, isLatest));
                beans.AddIdentifiables(GetMaintainableObjects<IDataConsumerScheme>(xref, retrieveStubs, isLatest));
                queryResultMaintainables = beans.GetAllMaintainables();
            }
            else
            {
                queryResultMaintainables = GetMaintainablesOfType<IMaintainableObject>(SdmxStructureType.ParseClass(type.MaintainableInterface), xref, retrieveStubs, isLatest);
            }

            ISet<IMaintainableObject> resolvedStubs = new HashSet<IMaintainableObject>();
            if (_externalReferenceRetrievalManager != null && !isAllStubs)
            {
                foreach (IMaintainableObject currentMaint in queryResultMaintainables)
                {
                    if (currentMaint.IsExternalReference.IsTrue)
                        resolvedStubs.Add(_externalReferenceRetrievalManager.ResolveFullStructure(currentMaint));
                }
            }

            foreach (IMaintainableObject qrm in queryResultMaintainables)
                resolvedStubs.Add(qrm);
            queryResultMaintainables = resolvedStubs;
            _log.Info("Returned " + queryResultMaintainables.Count + " results");

            IHeader header = null;
            if (_headerRetrievalManager != null)
                header = _headerRetrievalManager.Header;

            ISdmxObjects referencedBeans = new SdmxObjectsImpl(header);
            ISdmxObjects referenceMerge = new SdmxObjectsImpl();

            switch (restquery.StructureQueryMetadata.StructureReferenceDetail.EnumType)
            {
                case StructureReferenceDetailEnumType.None:
                    _log.Info("Reference detail NONE");
                    break;
                case StructureReferenceDetailEnumType.Parents:
                    _log.Info("Reference detail PARENTS");
                    ResolveParents(queryResultMaintainables, referencedBeans, isRefStubs);
                    break;
                case StructureReferenceDetailEnumType.ParentsSiblings:
                    _log.Info("Reference detail PARENTS_SIBLINGS");
                    ResolveParents(queryResultMaintainables, referencedBeans, isRefStubs);
                    ResolveChildren(referencedBeans.GetAllMaintainables(), referenceMerge, isRefStubs);
                    referencedBeans.Merge(referenceMerge);
                    break;
                case StructureReferenceDetailEnumType.Children:
                    _log.Info("Reference detail CHILDREN");
                    ResolveChildren(queryResultMaintainables, referencedBeans, isRefStubs);
                    break;
                case StructureReferenceDetailEnumType.Descendants:
                    _log.Info("Reference detail DESCENDANTS");
                    ResolveDescendants(queryResultMaintainables, referencedBeans, isRefStubs);
                    break;
                case StructureReferenceDetailEnumType.All:
                    _log.Info("Reference detail ALL");
                    ResolveParents(queryResultMaintainables, referencedBeans, isRefStubs);
                    ResolveDescendants(queryResultMaintainables, referenceMerge, isRefStubs);
                    referencedBeans.Merge(referenceMerge);
                    break;
                case StructureReferenceDetailEnumType.Specific:
                    _log.Info("Reference detail Children");
                    ResolveSpecific(queryResultMaintainables, referencedBeans, restquery.StructureQueryMetadata.SpecificStructureReference, isRefStubs);
                    break;
            }

            referencedBeans.AddIdentifiables(queryResultMaintainables);

            // Determine if we are returning stubs or not
            if (isAllStubs && (referencedBeans != null))
            {
                // It is not necessary to specify a serviceRetrievalManager so it may be null at this stage
                if (_serviceRetrievalManager == null)
                    throw new SdmxNotImplementedException("Cannot return stubs since no ServiceRetrievalManager has been supplied!");

                ISet<IMaintainableObject> stubSet = new HashSet<IMaintainableObject>();

                foreach (IMaintainableObject currentMaint in referencedBeans.GetAllMaintainables(null))
                    stubSet.Add(_serviceRetrievalManager.CreateStub(currentMaint));

                referencedBeans = new SdmxObjectsImpl(header);
                referencedBeans.AddIdentifiables(stubSet);
            }


            _log.Info("Result Size : " + referencedBeans.GetAllMaintainables().Count);
            return referencedBeans;
        }

        /// <summary>
        /// The resolve specific references.
        /// </summary>
        /// <param name="resolveFor">
        /// The set of references.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced objects.
        /// </param>
        /// <param name="specificType">
        /// The specific type.
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        private void ResolveSpecific<T>(ISet<T> resolveFor, ISdmxObjects referencedBeans, SdmxStructureType specificType, bool returnStub) where T : IMaintainableObject
        {
            _log.Info("Resolving Child Structures");
            foreach (IMaintainableObject currentMaintainable in resolveFor)
            {
                _log.Debug("Resolving Children of " + currentMaintainable.Urn);
                referencedBeans.AddIdentifiables(_crossReferenceRetrievalManager.GetCrossReferencedStructures(currentMaintainable, returnStub, specificType));
                if (this._crossReferencingRetrievalManager != null)
                    referencedBeans.AddIdentifiables(_crossReferencingRetrievalManager.GetCrossReferencingStructures(currentMaintainable, returnStub, specificType));
            }
            _log.Info(referencedBeans.GetAllMaintainables().Count + " children found");
        }

        /// <summary>
        /// The resolve descendants references.
        /// </summary>
        /// <param name="resolveFor">
        /// The set of references.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced objects.
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        private void ResolveDescendants<T>(ISet<T> resolveFor, ISdmxObjects referencedBeans, bool returnStub) where T : IMaintainableObject
        {
            int numBeans = -2;
            while (numBeans != resolveFor.Count)
            {
                numBeans = referencedBeans.GetAllMaintainables().Count;
                ResolveChildren<T>(resolveFor, referencedBeans, returnStub);
                resolveFor = new HashSet<T>(referencedBeans.GetAllMaintainables().Cast<T>());
            }
            _log.Info(referencedBeans.GetAllMaintainables().Count + " descendants found");
        }

        /// <summary>
        /// The resolve children references.
        /// </summary>
        /// <param name="resolveFor">
        /// The set of references.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced objects.
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        private void ResolveChildren<T>(IEnumerable<T> resolveFor, ISdmxObjects referencedBeans, bool returnStub) where T : IMaintainableObject
        {
            _log.Info("Resolving Child Structures");
            foreach (T currentMaintainable in resolveFor)
            {
                _log.Debug("Resolving Children of " + currentMaintainable.Urn);
                referencedBeans.AddIdentifiables(_crossReferenceRetrievalManager.GetCrossReferencedStructures(currentMaintainable, returnStub));
            }
            _log.Info(referencedBeans.GetAllMaintainables().Count + " children found");
        }


        /// <summary>
        /// The resolve parents references.
        /// </summary>
        /// <param name="resolveFor">
        /// The set of references.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced objects.
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        private void ResolveParents<T>(IEnumerable<T> resolveFor, ISdmxObjects referencedBeans, bool returnStub) where T : IMaintainableObject
        {
            _log.Info("Resolving Parents Structures");
            foreach (T currentMaintainable in resolveFor)
            {
                _log.Debug("Resolving Parents of " + currentMaintainable.Urn);
                if (_crossReferencingRetrievalManager == null)
                    throw new SdmxNotImplementedException("Resolve parents not supported");
                referencedBeans.AddIdentifiables(_crossReferencingRetrievalManager.GetCrossReferencingStructures(currentMaintainable, returnStub));
            }
            _log.Info(referencedBeans.GetAllMaintainables().Count + " parents found");
        }

        /// <summary>
        /// The resolve references.
        /// </summary>
        /// <param name="beans0">
        /// The beans 0.
        /// </param>
        /// <param name="resolveAgencies">
        /// If true the agencies will be included
        /// </param>
        private void ResolveReferences(ISdmxObjects beans0, bool resolveAgencies)
        {
            _log.Debug("resolve references for beans create new cross reference resolver engine");
            ICrossReferenceResolverEngine resolver = new CrossReferenceResolverEngineCore();
            IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> crossReferenceMap = resolver.ResolveReferences(beans0, resolveAgencies, 0, this);

            foreach (var keyValuePair in crossReferenceMap)
            {
                beans0.AddIdentifiable(keyValuePair.Key);

                foreach (IIdentifiableObject valueren in keyValuePair.Value)
                {
                    beans0.AddIdentifiable(valueren);
                }
            }

            _log.Debug("resolve references complete");
        }

        /// <summary>
        /// If the set is of size 1, then returns the element in the set.
        ///     Returns null if the set is null or of size 0
        /// </summary>
        /// <typeparam name="T">
        /// The maintainable type
        /// </typeparam>
        /// <param name="set">
        /// set of elements
        /// </param>
        /// <returns>
        /// The first element of the set.
        ///     Throws SdmxException if the set contains more then 1 element
        /// </returns>
        private static T ExtractFromSet<T>(ICollection<T> set) where T : IMaintainableObject
        {
            if (!ObjectUtil.ValidCollection(set))
            {
                return default(T);
            }

            if (set.Count == 1)
            {
                return set.First();
            }

            throw new SdmxException("Did not expect more then 1 structure from query, got " + set.Count + " structures.");
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
        public virtual ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IAttachmentConstraintObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetAttachmentConstraints");
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
        public virtual ISet<ICategorisationObject> GetCategorisationObjects(IMaintainableRefObject xref, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<ICategorisationObject>(xref, returnStub, false);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetCategorisationObjects");
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
        public virtual ISet<ICodelistObject> GetCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<ICodelistObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetCodelistObjects");
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
        public virtual ISet<IConceptSchemeObject> GetConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IConceptSchemeObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetConceptSchemeObjects");
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
        public virtual ISet<IContentConstraintObject> GetContentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IContentConstraintObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetContentConstraints");
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
        public virtual ISet<ICategorySchemeObject> GetCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<ICategorySchemeObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetCategorySchemeObjects");
        }

        /// <summary>
        /// Returns DataflowObjects that match the parameters in the ref object.  If the ref object is null or
        /// has no attributes set, then this will be interpreted as a search for all DataflowObjects
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
        public virtual ISet<IDataflowObject> GetDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IDataflowObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetDataflowObjects");
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
        public virtual ISet<IHierarchicalCodelistObject> GetHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IHierarchicalCodelistObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetHierarchicCodeListObjects");
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
        public virtual ISet<IMetadataFlow> GetMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IMetadataFlow>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetMetadataflowObjects");
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
        public virtual ISet<IDataStructureObject> GetDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IDataStructureObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetDataStructureObjects");
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
        public virtual ISet<IMetadataStructureDefinitionObject> GetMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IMetadataStructureDefinitionObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetMetadataStructureObjects");
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
        public virtual ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IOrganisationUnitSchemeObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetOrganisationUnitSchemeObjects");
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
        public virtual ISet<IDataProviderScheme> GetDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IDataProviderScheme>(xref, returnStub, false);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetDataProviderSchemeObjects");
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
        public virtual ISet<IDataConsumerScheme> GetDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IDataConsumerScheme>(xref, returnStub, false);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetDataConsumerSchemeObjects");
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
        public virtual ISet<IAgencyScheme> GetAgencySchemeObjects(IMaintainableRefObject xref, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IAgencyScheme>(xref, returnStub, false);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetAgencySchemeObjects");
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
        public virtual ISet<IProcessObject> GetProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IProcessObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetProcessObjects");
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
        public virtual ISet<IProvisionAgreementObject> GetProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IProvisionAgreementObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetProvisionAgreementObjects");
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
        public virtual ISet<IReportingTaxonomyObject> GetReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IReportingTaxonomyObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetReportingTaxonomyObjects");
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
        public virtual ISet<IStructureSetObject> GetStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<IStructureSetObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetStructureSetObjects");
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
        public virtual ISet<ISubscriptionObject> GetSubscriptionObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (_proxy != null)
                return _proxy.GetMaintainableObjects<ISubscriptionObject>(xref, returnStub, returnLatest);
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "GetSubscriptionObjects");
        }
    }
}
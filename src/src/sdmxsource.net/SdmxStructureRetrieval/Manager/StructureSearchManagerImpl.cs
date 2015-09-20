// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSearchManagerImpl.cs" company="Eurostat">
//   Date Created : 2012-10-12
//   //   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The structure search manager impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureretrieval.Manager
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Categoryscheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine.Impl;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The structure search manager
    /// </summary>
    public class StructureSearchManagerImpl : IStructureSearchManager
    {
        #region Fields

        /// <summary>
        /// The cross reference retrieval manager.
        /// </summary>
        private readonly ICrossReferenceRetrievalManager _crossReferenceRetrievalManager;

        /// <summary>
        /// The <c>SDMX</c> retrieval manager.
        /// </summary>
        private readonly ISdmxRetrievalManager _sdmxBeanRetrievalManager;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(StructureSearchManagerImpl));

        /// <summary>
        /// The header retrieval manager.
        /// </summary>
        private IHeaderRetrievalManager headerRetrievalManager;

        /// <summary>
        /// The registration retrieval manager.
        /// </summary>
        private IRegistrationRetrievalManager _registrationRetrievalManager;

        /// <summary>
        /// The service retrieval manager.
        /// </summary>
        private IServiceRetrievalManager _serviceRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSearchManagerImpl"/> class.
        /// </summary>
        /// <param name="sdmxBeanRetrievalManager">
        /// The <c>SDMX</c> object retrieval manager.
        /// </param>
        /// <param name="crossReferenceRetrievalManager">
        /// The cross reference retrieval manager.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sdmxBeanRetrievalManager"/> is null
        /// -or-
        /// <paramref name="crossReferenceRetrievalManager"/> is null
        /// </exception>
        public StructureSearchManagerImpl(
            ISdmxRetrievalManager sdmxBeanRetrievalManager, 
            ICrossReferenceRetrievalManager crossReferenceRetrievalManager)
        {
            if (sdmxBeanRetrievalManager == null)
            {
                throw new ArgumentNullException("sdmxBeanRetrievalManager");
            }

            if (crossReferenceRetrievalManager == null)
            {
                throw new ArgumentNullException("crossReferenceRetrievalManager");
            }

            this._sdmxBeanRetrievalManager = sdmxBeanRetrievalManager;
            this._crossReferenceRetrievalManager = crossReferenceRetrievalManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets cross referencing <c>categorisations</c>.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        public virtual IList<ICategorisationObject> GetCrossReferencingCategorisations(IMaintainableObject maintainable)
        {
            IList<ICategorisationObject> returnList = new List<ICategorisationObject>();

            /* foreach */
            foreach (IMaintainableObject currentCategorisation in
                this._crossReferenceRetrievalManager.GetCrossReferencingStructures(
                    maintainable, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation)))
            {
                if (currentCategorisation.Stub)
                {
                    returnList.Add(
                        this._sdmxBeanRetrievalManager.GetCategorisation(
                            currentCategorisation.AsReference().MaintainableReference));
                }
                else
                {
                    returnList.Add((ICategorisationObject)currentCategorisation);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Gets the maintainable <see cref="ISDMXObject"/>
        /// </summary>
        /// <param name="complexQuery">
        /// The complex query.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects GetMaintainables(IStructureQuery complexQuery)
        {
            this._log.Debug("Executing query for maintainables");

            ISet<IMaintainableObject> queryResultMaintainables =
                this.RetrieveStructures(
                    complexQuery.StructureReference.MaintainableReference, 
                    complexQuery.StructureReference.MaintainableStructureEnumType, 
                    false).GetAllMaintinables();
            if (this._log.IsDebugEnabled)
            {
                this._log.Debug("Returned " + queryResultMaintainables.Count + " results");
            }

            if (complexQuery.ReturnLatest)
            {
                // TODO This functionality should really live on the interface
                IDictionary<string, IMaintainableObject> resultMap = new Dictionary<string, IMaintainableObject>();
                bool filteredResponse = false;

                /* foreach */
                foreach (IMaintainableObject currentMaint in queryResultMaintainables)
                {
                    string key = currentMaint.StructureType.GetType() + string.Empty + currentMaint.AgencyId
                                 + string.Empty + currentMaint.Id;
                    IMaintainableObject storedAgainstKey;
                    if (resultMap.TryGetValue(key, out storedAgainstKey))
                    {
                        filteredResponse = true;
                        if (VersionableUtil.IsHigherVersion(currentMaint.Version, storedAgainstKey.Version))
                        {
                            resultMap.Add(key, currentMaint);
                        }
                    }
                    else
                    {
                        resultMap.Add(key, currentMaint);
                    }
                }

                if (filteredResponse)
                {
                    // We have removed some maintainables of duplicate id and agency (lower version)
                    queryResultMaintainables = new HashSet<IMaintainableObject>(resultMap.Values);
                }
            }

            ISdmxObjects referencedBeans = new SdmxObjectsImpl();
            ISdmxObjects referenceMerge = new SdmxObjectsImpl();
            switch (complexQuery.StructureReferenceDetail.EnumType)
            {
                case StructureReferenceDetailEnumType.None:
                    this._log.Debug("Reference detail NONE");
                    break;
                case StructureReferenceDetailEnumType.Parents:
                    this._log.Debug("Reference detail PARENTS");
                    this.ResolveParents(queryResultMaintainables, referencedBeans);

                    break;
                case StructureReferenceDetailEnumType.ParentsSiblings:
                    this._log.Debug("Reference detail ParentsSiblings");
                    this.ResolveParents(queryResultMaintainables, referencedBeans);
                    this.ResolveChildren(referencedBeans.GetAllMaintinables(), referenceMerge);
                    referencedBeans.Merge(referenceMerge);
                    break;
                case StructureReferenceDetailEnumType.Children:
                    this._log.Debug("Reference detail CHILDREN");
                    this.ResolveChildren(queryResultMaintainables, referencedBeans);
                    break;
                case StructureReferenceDetailEnumType.Descendants:
                    this._log.Debug("Reference detail DESCENDANTS");
                    this.ResolveDescendants(queryResultMaintainables, referencedBeans);
                    break;
                case StructureReferenceDetailEnumType.All:
                    this.ResolveParents(queryResultMaintainables, referencedBeans);

                    // 1. Put all the parents that were resolved in a set
                    ISet<IMaintainableObject> fullSet = referencedBeans.GetAllMaintinables();

                    // 2. Put all the query result beans in the same set 
                    queryResultMaintainables.AddAll(fullSet);

                    this.ResolveDescendants(fullSet, referenceMerge);
                    referencedBeans.Merge(referenceMerge);
                    break;
                case StructureReferenceDetailEnumType.Specific:
                    this.ResolveSpecific(
                        queryResultMaintainables, referencedBeans, complexQuery.SpecificStructureEnumReference);
                    break;
            }

            // Strip out the registrations and subscriptions
            ISet<IMaintainableObject> maintainableReferences =
                referencedBeans.GetAllMaintinables(
                    SdmxStructureEnumType.Registration, SdmxStructureEnumType.Subscription);
            IHeader header = null;
            if (this.headerRetrievalManager != null)
            {
                header = this.headerRetrievalManager.Header;
            }

            referencedBeans = new SdmxObjectsImpl(header);

            switch (complexQuery.StructureQueryDetail)
            {
                case StructureQueryDetail.AllStubs:
                    this._log.Debug("Query detail AllStubs");
                    {
                        /* foreach */
                        foreach (IMaintainableObject currentBean in maintainableReferences)
                        {
                            IMaintainableObject stubBean = this._serviceRetrievalManager.CreateStub(currentBean);
                            referencedBeans.AddIdentifiable(stubBean);
                        }
                    }

                    {
                        /* foreach */
                        foreach (IMaintainableObject currentBean0 in queryResultMaintainables)
                        {
                            IMaintainableObject stubBean1 = this._serviceRetrievalManager.CreateStub(currentBean0);
                            referencedBeans.AddIdentifiable(stubBean1);
                        }
                    }

                    break;
                case StructureQueryDetail.Full:
                    this._log.Debug("Query detail FULL");
                    referencedBeans.AddIdentifiables(queryResultMaintainables);
                    referencedBeans.AddIdentifiables(maintainableReferences);
                    break;
                case StructureQueryDetail.ReferencedStubs:
                    this._log.Debug("Query detail ReferencedStubs");
                    {
                        /* foreach */
                        foreach (IMaintainableObject currentReference in maintainableReferences)
                        {
                            IMaintainableObject stubBean2 = this._serviceRetrievalManager.CreateStub(currentReference);
                            referencedBeans.AddIdentifiable(stubBean2);
                        }
                    }

                    referencedBeans.AddIdentifiables(queryResultMaintainables);
                    break;
            }

            if (this._log.IsDebugEnabled)
            {
                this._log.Debug("Result Size : " + referencedBeans.GetAllMaintinables().Count);
            }

            return referencedBeans;
        }

        /// <summary>
        /// The retrieve structures.
        /// </summary>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="resolveReferences">
        /// The resolve references.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects RetrieveStructures(IList<IStructureReference> queries, bool resolveReferences)
        {
            var returnBeansArr = new ISdmxObjects[queries.Count];
            for (int i = 0; i < queries.Count; i++)
            {
                ISdmxObjects returnedBeans = this.RetrieveStructures(
                    queries[i].MaintainableReference, queries[i].MaintainableStructureEnumType, false);

                // FUNC filter list if the query was for an identifiable
                returnBeansArr[i] = new SdmxObjectsImpl(returnedBeans);
            }

            ISdmxObjects returnBeans = new SdmxObjectsImpl(returnBeansArr);
            if (resolveReferences)
            {
                this.ResolveReferences(returnBeans);
            }

            return returnBeans;
        }

        /// <summary>
        /// Retrieve structures.
        /// </summary>
        /// <param name="xref">
        /// The cross-reference.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="resolveReferences">
        /// The resolve references.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        /// <exception cref="UnsupportedException">
        /// <paramref name="structureType"/> is not maintainable
        /// </exception>
        public ISdmxObjects RetrieveStructures(IMaintainableRefObject xref, SdmxStructureType structureType, bool resolveReferences)
        {
            if (structureType.EnumType != SdmxStructureEnumType.Any
                && structureType.EnumType != SdmxStructureEnumType.OrganisationScheme
                && !structureType.IsMaintainable)
            {
                throw new UnsupportedException(ExceptionCode.Unsupported, structureType + " is not maintainable");
            }

            ISdmxObjects returnBeans = new SdmxObjectsImpl();
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Any:
                    {
                        /* foreach */
                        foreach (SdmxStructureType currentMaintainable in SdmxStructureType.MaintainableStructureTypes)
                        {
                            Console.Out.WriteLine(currentMaintainable);
                            if (currentMaintainable != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration)
                                &&
                                currentMaintainable != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription))
                            {
                                returnBeans.Merge(this.RetrieveStructures(xref, currentMaintainable, resolveReferences));
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.OrganisationScheme:
                    returnBeans.Merge(
                        this.RetrieveStructures(
                            xref, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme), resolveReferences));
                    returnBeans.Merge(
                        this.RetrieveStructures(
                            xref, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme), 
                            resolveReferences));
                    returnBeans.Merge(
                        this.RetrieveStructures(
                            xref, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme), 
                            resolveReferences));
                    returnBeans.Merge(
                        this.RetrieveStructures(
                            xref, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme), 
                            resolveReferences));
                    break;
                case SdmxStructureEnumType.AgencyScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetAgencySchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetDataProviderSchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetDataConsumerSchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.Categorisation:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetCategorisationBeans(xref));
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetCategorySchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.CodeList:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetCodelistBeans(xref));
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetConceptSchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.Dataflow:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetDataflowBeans(xref));
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetHierarchicCodeListBeans(xref));
                    break;
                case SdmxStructureEnumType.Dsd:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetDataStructureBeans(xref));
                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetMetadataflowBeans(xref));
                    break;
                case SdmxStructureEnumType.Msd:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetMetadataStructureBeans(xref));
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetOrganisationUnitSchemeBeans(xref));
                    break;
                case SdmxStructureEnumType.Process:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetProcessBeans(xref));
                    break;
                case SdmxStructureEnumType.StructureSet:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetStructureSetBeans(xref));
                    break;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetReportingTaxonomyBeans(xref));
                    break;
                case SdmxStructureEnumType.AttachmentConstraint:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetAttachmentConstraints(xref));
                    break;
                case SdmxStructureEnumType.ContentConstraint:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetContentConstraints(xref));
                    break;
                case SdmxStructureEnumType.ProvisionAgreement:
                    AddToSdmxObjects(returnBeans, this._sdmxBeanRetrievalManager.GetProvisionAgreementBeans(xref));
                    break;
                case SdmxStructureEnumType.Registration:
                    AddToSdmxObjects(returnBeans, this._registrationRetrievalManager.GetRegistrations(xref));
                    break;
                default:
                    throw new UnsupportedException(
                        ExceptionCode.Unsupported, "Unsupported query for : " + structureType);
            }

            if (resolveReferences)
            {
                this.ResolveReferences(returnBeans);
            }

            return returnBeans;
        }

        /// <summary>
        /// The set registration retrieval manager.
        /// </summary>
        /// <param name="registrationRetrievalManager0">
        /// The registration retrieval manager 0.
        /// </param>
        public void SetRegistrationRetrievalManager(IRegistrationRetrievalManager registrationRetrievalManager0)
        {
            this._registrationRetrievalManager = registrationRetrievalManager0;
        }

        /// <summary>
        /// The set service retrieval manager.
        /// </summary>
        /// <param name="serviceRetrievalManager0">
        /// The service retrieval manager 0.
        /// </param>
        public void SetServiceRetrievalManager(IServiceRetrievalManager serviceRetrievalManager0)
        {
            this._serviceRetrievalManager = serviceRetrievalManager0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The resolve references.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        internal void ResolveReferences(ISdmxObjects beans)
        {
            ICrossReferenceResolverEngine resolver = new CrossReferenceResolverEngineCore();
            IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> crossReferenceMap =
                resolver.ResolveReferences(beans, false, 0, this._sdmxBeanRetrievalManager);

            /* foreach */
            foreach (KeyValuePair<IIdentifiableObject, ISet<IIdentifiableObject>> keyValuePair in crossReferenceMap)
            {
                beans.AddIdentifiable(keyValuePair.Key);

                /* foreach */
                foreach (IIdentifiableObject valueren in keyValuePair.Value)
                {
                    beans.AddIdentifiable(valueren);
                }
            }
        }

        /// <summary>
        /// Adds the specified <c>SDMX</c> objects <paramref name="currentMaintainables"/> of type <typeparamref name="T"/> to <paramref name="returnBeans"/>
        /// </summary>
        /// <param name="returnBeans">
        /// The return beans.
        /// </param>
        /// <param name="currentMaintainables">
        /// The current maintainable set.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="currentMaintainables"/>
        /// </typeparam>
        private static void AddToSdmxObjects<T>(ISdmxObjects returnBeans, IEnumerable<T> currentMaintainables)
            where T : IMaintainableObject
        {
            foreach (T currentBean in currentMaintainables)
            {
                returnBeans.AddIdentifiable(currentBean);
            }
        }

        /// <summary>
        /// The resolve children.
        /// </summary>
        /// <param name="resolveFor">
        /// The resolve for.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced beans.
        /// </param>
        private void ResolveChildren(IEnumerable<IMaintainableObject> resolveFor, ISdmxObjects referencedBeans)
        {
            this._log.Debug("Resolving Child Structures");

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in resolveFor)
            {
                this._log.Debug("Resolving Children of " + currentMaintainable.Urn);
                referencedBeans.AddIdentifiables(
                    this._crossReferenceRetrievalManager.GetCrossReferencedStructures(currentMaintainable));
            }

            if (this._log.IsDebugEnabled)
            {
                this._log.Debug(referencedBeans.GetAllMaintinables().Count + " children found");
            }
        }

        /// <summary>
        /// The resolve descendants.
        /// </summary>
        /// <param name="resolveFor">
        /// The resolve for.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced beans.
        /// </param>
        private void ResolveDescendants(ISet<IMaintainableObject> resolveFor, ISdmxObjects referencedBeans)
        {
            int numBeans = -2;
            while (numBeans != resolveFor.Count)
            {
                numBeans = referencedBeans.GetAllMaintinables().Count;
                this.ResolveChildren(resolveFor, referencedBeans);
                resolveFor = referencedBeans.GetAllMaintinables();
            }

            if (this._log.IsDebugEnabled)
            {
                this._log.Debug(referencedBeans.GetAllMaintinables().Count + " descendants found");
            }
        }

        /// <summary>
        /// The resolve parents.
        /// </summary>
        /// <param name="resolveFor">
        /// The resolve for.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced beans.
        /// </param>
        private void ResolveParents(IEnumerable<IMaintainableObject> resolveFor, ISdmxObjects referencedBeans)
        {
            this._log.Debug("Resolving Parents Structures");

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in resolveFor)
            {
                this._log.Debug("Resolving Parents of " + currentMaintainable.Urn);
                referencedBeans.AddIdentifiables(
                    this._crossReferenceRetrievalManager.GetCrossReferencingStructures(currentMaintainable));
            }

            if (this._log.IsDebugEnabled)
            {
                this._log.Debug(referencedBeans.GetAllMaintinables().Count + " parents found");
            }
        }

        /// <summary>
        /// The resolve specific.
        /// </summary>
        /// <param name="resolveFor">
        /// The resolve for.
        /// </param>
        /// <param name="referencedBeans">
        /// The referenced beans.
        /// </param>
        /// <param name="specificType">
        /// The specific type.
        /// </param>
        private void ResolveSpecific(
            IEnumerable<IMaintainableObject> resolveFor, ISdmxObjects referencedBeans, SdmxStructureType specificType)
        {
            this._log.Debug("Resolving Child Structures");

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in resolveFor)
            {
                this._log.Debug("Resolving Children of " + currentMaintainable.Urn);
                referencedBeans.AddIdentifiables(
                    this._crossReferenceRetrievalManager.GetCrossReferencedStructures(currentMaintainable, specificType));
                referencedBeans.AddIdentifiables(
                    this._crossReferenceRetrievalManager.GetCrossReferencingStructures(currentMaintainable, specificType));
            }

            if (this._log.IsDebugEnabled)
            {
                this._log.Debug(referencedBeans.GetAllMaintinables().Count + " children found");
            }
        }

        #endregion
    }
}
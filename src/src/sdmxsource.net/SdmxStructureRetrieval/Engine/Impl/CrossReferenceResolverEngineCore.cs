// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceResolverEngineCore.cs" company="Eurostat">
//   Date Created : 2012-10-12
//   //   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The cross reference resolver engine impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Engine.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    // JAVADOC missing

    /// <summary>
    /// The <see cref="ICrossReferenceResolverEngine"/> implementation
    /// </summary>
    public class CrossReferenceResolverEngineCore : ICrossReferenceResolverEngine
    {
        #region Fields

        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(CrossReferenceResolverEngineCore));

        /// <summary>
        /// The agencies.
        /// </summary>
        private IDictionary<string, IAgency> _agencies;

        /// <summary>
        /// The cross references.
        /// </summary>
        private IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> _crossReferences;

        /// <summary>
        /// The set of maintainable artifact
        /// </summary>
        private ISet<IMaintainableObject> _maintianables;

        /// <summary>
        /// The resolve agencies.
        /// </summary>
        private bool _resolveAgencies;

        /// <summary>
        /// The structure retrieval manager.
        /// </summary>
        private ISdmxRetrievalManager _structureRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceResolverEngineCore"/> class.
        /// </summary>
        public CrossReferenceResolverEngineCore()
        {
            this._crossReferences = new Dictionary<IIdentifiableObject, ISet<IIdentifiableObject>>();
            this._maintianables = new HashSet<IMaintainableObject>();
            this._agencies = new Dictionary<string, IAgency>(StringComparer.Ordinal);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceResolverEngineCore"/> class.
        /// </summary>
        /// <param name="structureRetrievalManager">
        /// The structure retrieval manager.
        /// </param>
        public CrossReferenceResolverEngineCore(ISdmxRetrievalManager structureRetrievalManager) : this()
        {
            this._structureRetrievalManager = structureRetrievalManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// For the included <paramref name="sdmxObjects"/>, returns a map of agency URN to maintainable Bean that references the agency
        /// </summary>
        /// <param name="sdmxObjects">
        /// The included <c>SDMX</c> objects
        /// </param>
        /// <param name="retrievalManager">
        /// The <see cref="ISdmxRetrievalManager"/>
        /// </param>
        /// <returns>
        /// The included <paramref name="sdmxObjects"/>, returns a map of agency URN to maintainable Bean that references the agency
        /// </returns>
        public virtual IDictionary<string, ISet<IMaintainableObject>> GetMissingAgencies(
            ISdmxObjects sdmxObjects, ISdmxRetrievalManager retrievalManager)
        {
            ISet<string> agencyIds = new HashSet<string>();

            /* foreach */
            var agencies = sdmxObjects.Agencies;
            foreach (IAgency acy in agencies)
            {
                agencyIds.Add(acy.FullId);
            }

            IDictionary<string, ISet<IMaintainableObject>> returnMap =
                new Dictionary<string, ISet<IMaintainableObject>>();

            /* foreach */
            var maintainableObjects = sdmxObjects.GetAllMaintinables();
            foreach (IMaintainableObject currentMaint in maintainableObjects)
            {
                string referencedAgencyId = currentMaint.AgencyId;
                if (!agencyIds.Contains(referencedAgencyId))
                {
                    if (retrievalManager != null)
                    {
                        try
                        {
                            IAgency acy0 = retrievalManager.GetAgency(referencedAgencyId);
                            if (acy0 != null)
                            {
                                agencyIds.Add(acy0.FullId);
                                continue;
                            }
                        }
                        catch (Exception th)
                        {
                            Console.Error.WriteLine(th.StackTrace);
                        }
                    }

                    ISet<IMaintainableObject> maintainables;
                    if (!returnMap.TryGetValue(referencedAgencyId, out maintainables))
                    {
                        maintainables = new HashSet<IMaintainableObject>();
                        returnMap.Add(referencedAgencyId, maintainables);
                    }

                    maintainables.Add(currentMaint);
                }
            }

            return returnMap;
        }

        /// <summary>
        /// Gets a Dictionary of <see cref="IIdentifiableObject"/> alongside any cross references they declare that could not be found in the set of <paramref name="beans"/> provided, and the <paramref name="retrievalManager"/> (if given).
        ///  <p/>
        ///  <b>NOTE :</b>An empty Map is returned if all cross references are present.
        /// </summary>
        /// <param name="beans">
        /// - the objects to return the Map of missing references for 
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing  until it has found every directly and indirectly referenced 
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans 
        /// </param>
        /// <returns>
        /// Map of IIdentifiableObject with a Set of CrossReferences that could not be resolved for the IIdentifiableObject - an empty Map is returned if all cross references are present 
        /// </returns>
        public virtual IDictionary<IIdentifiableObject, ISet<ICrossReference>> GetMissingCrossReferences(
            ISdmxObjects beans, int numberLevelsDeep, ISdmxRetrievalManager retrievalManager)
        {
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> returnMap =
                new Dictionary<IIdentifiableObject, ISet<ICrossReference>>();
            this.ResolveReferences(beans, false, numberLevelsDeep, retrievalManager, returnMap);
            return returnMap;
        }

        /// <summary>
        /// Resolves a reference from <paramref name="crossReference"/>
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference instance
        /// </param>
        /// <param name="structRetrievalManager">
        /// The structure Retrieval Manager.
        /// </param>
        /// <returns>
        /// a reference from <paramref name="crossReference"/>
        /// </returns>
        public virtual IIdentifiableObject ResolveCrossReference(
            ICrossReference crossReference, ISdmxRetrievalManager structRetrievalManager)
        {
            if (crossReference.TargetReference == SdmxStructureEnumType.Agency)
            {
                return this.ResoveAgency(crossReference.ChildReference.Id);
            }

            var maintainableParent =
                crossReference.ReferencedFrom.GetParent<IMaintainableObject>(typeof(IMaintainableObject), true);
            IStructureReference maintainableReferenceObject;

            if (crossReference.HasChildReference())
            {
                maintainableReferenceObject = new StructureReferenceImpl(
                    crossReference.MaintainableReference, crossReference.MaintainableStructureEnumType);
            }
            else
            {
                maintainableReferenceObject = crossReference;
            }

            IMaintainableObject resolvedMaintainable = this.ResolveMaintainableFromLocalMaps(
                maintainableReferenceObject, maintainableParent);
            if (resolvedMaintainable == null && structRetrievalManager != null)
            {
                // TODO avoid try catch
                try
                {
                    resolvedMaintainable = structRetrievalManager.GetMaintainable(maintainableReferenceObject);
                }
                catch (Exception)
                {
                    throw new ReferenceException(crossReference);
                }
            }

            if (resolvedMaintainable == null)
            {
                throw new ReferenceException(crossReference);
            }

            // Add the maintainable to the local map, so we don't have to go back to the DAO if there is another reference to the same maintainable
            this._maintianables.Add(resolvedMaintainable);
            if (crossReference.HasChildReference())
            {
                string targetUrn = crossReference.TargetUrn;

                /* foreach */
                foreach (IIdentifiableObject currentComposite in resolvedMaintainable.IdentifiableComposites)
                {
                    if (currentComposite.Urn.Equals(targetUrn))
                    {
                        return currentComposite;
                    }
                }
            }
            else
            {
                return resolvedMaintainable;
            }

            throw new ReferenceException(crossReference);
        }

        /// <summary>
        /// Returns a set of structures that are directly referenced from this provision
        /// </summary>
        /// <param name="provision">
        /// - the provision to resolve the references for 
        /// </param>
        /// <param name="structRetrievalManager">
        /// - must not be null as this will be used to resolve the references 
        /// </param>
        /// <returns>
        ///  a set of structures that are directly referenced from this provision
        /// </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IProvisionAgreementObject provision, ISdmxRetrievalManager structRetrievalManager)
        {
            if (structRetrievalManager == null)
            {
                throw new ArgumentException("StructureRetrievalManager can not be null");
            }

            this._structureRetrievalManager = structRetrievalManager;

            ISet<IIdentifiableObject> returnSet = new HashSet<IIdentifiableObject>();
            if (provision.StructureUseage != null)
            {
                IMaintainableObject structureUseage = structRetrievalManager.GetMaintainable(provision.StructureUseage);

                if (structureUseage == null)
                {
                    throw new ReferenceException(
                        ExceptionCode.ReferenceErrorUnresolvable, 
                        provision.StructureUseage.TargetReference.GetType(), 
                        provision.StructureUseage);
                }

                returnSet.Add(structureUseage);
            }

            if (provision.DataproviderRef != null)
            {
                IDataProviderScheme orgScheme =
                    structRetrievalManager.GetDataProviderSchemeBean(provision.DataproviderRef.MaintainableReference);
                if (orgScheme == null)
                {
                    throw new ReferenceException(
                        ExceptionCode.ReferenceErrorUnresolvable, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme).ToString(), 
                        provision.DataproviderRef.MaintainableReference);
                }

                var dataProviders = orgScheme.Items;
                if (!ObjectUtil.ValidCollection(dataProviders))
                {
                    throw new ReferenceException(
                        ExceptionCode.ReferenceErrorUnresolvable, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider).ToString(), 
                        provision.DataproviderRef);
                }

                bool found = false;

                /* foreach */
                foreach (IDataProvider dataProvider in dataProviders)
                {
                    if (dataProvider.Id.Equals(provision.DataproviderRef.ChildReference.Id))
                    {
                        found = true;
                        returnSet.Add(orgScheme);
                        break;
                    }
                }

                if (!found)
                {
                    throw new ReferenceException(
                        ExceptionCode.ReferenceErrorUnresolvable, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider).ToString(), 
                        provision.DataproviderRef);
                }
            }

            return returnSet;
        }

        /// <summary>
        /// Returns a set of IdentifiableBeans that are directly referenced from this registration
        /// </summary>
        /// <param name="registation">
        /// - the registration to resolve the references for 
        /// </param>
        /// <param name="structRetrievalManager">
        /// - Used to resolve the structure references. can be null if level = 1 and registration is linked to a provision (as only the provision manager is needed) 
        /// </param>
        /// <param name="provRetrievalManager">
        /// - Used to resolve the provision references. Can be null if registration is not linked to a provision 
        /// </param>
        /// <returns>
        /// a set of IdentifiableBeans that are directly referenced from this registration
        /// </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IRegistrationObject registation, 
            ISdmxRetrievalManager structRetrievalManager, 
            IProvisionRetrievalManager provRetrievalManager)
        {
            ISet<IIdentifiableObject> returnSet = new HashSet<IIdentifiableObject>();

            if (registation.ProvisionAgreementRef != null)
            {
                if (provRetrievalManager == null)
                {
                    throw new ArgumentException("ProvisionRetrievalManager can not be null");
                }

                IProvisionAgreementObject provision = provRetrievalManager.GetProvision(registation);
                if (provision == null)
                {
                    throw new ReferenceException(
                        ExceptionCode.ReferenceErrorUnresolvable, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement).ToString(), 
                        registation.ProvisionAgreementRef);
                }

                returnSet.Add(provision);
            }

            return returnSet;
        }

        /// <summary>
        /// Resolves all references and returns a Map containing all the input beans and the objects that are cross referenced,
        ///  the Map's key set contains the Identifiable that is the referencing object and the Map's value collection contains the referenced artifacts.
        /// </summary>
        /// <param name="beans">
        /// - the <see cref="ISdmxObjects"/> container, containing all the beans to check references for 
        /// </param>
        /// <param name="resolveAgencies">
        /// - if true the resolver engine will also attempt to resolve referenced agencies 
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this. 
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans 
        /// </param>
        /// <returns>
        /// Map of referencing versus  references 
        /// </returns>
        /// <exception cref="ReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        public virtual IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> ResolveReferences(
            ISdmxObjects beans, bool resolveAgencies, int numberLevelsDeep, ISdmxRetrievalManager retrievalManager)
        {
            return this.ResolveReferences(beans, resolveAgencies, numberLevelsDeep, retrievalManager, null);
        }

        /// <summary>
        /// Returns a set of IdentifiableBeans that the IMaintainableObject cross references
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <param name="resolveAgencies">
        /// - if true will also resolve the agencies 
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this. 
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans 
        /// </param>
        /// <exception cref="ReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        /// <returns>
        ///  a set of IdentifiableBeans that the IMaintainableObject cross references
        /// </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IMaintainableObject bean, 
            bool resolveAgencies, 
            int numberLevelsDeep, 
            ISdmxRetrievalManager retrievalManager)
        {
            this.ResetMaps();
            ISdmxObjects beans = new SdmxObjectsImpl();
            beans.AddIdentifiable(bean);
            IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> references = this.ResolveReferences(
                beans, resolveAgencies, numberLevelsDeep, retrievalManager);

            ISet<IIdentifiableObject> returnSet = new HashSet<IIdentifiableObject>();

            /* foreach */
            foreach (KeyValuePair<IIdentifiableObject, ISet<IIdentifiableObject>> key in references)
            {
                    returnSet.AddAll(key.Value);
            }

            return returnSet;
        }

        /// <summary>
        /// The set structure retrieval manager.
        /// </summary>
        /// <param name="structureRetrievalManager0">
        /// The structure retrieval manager 0.
        /// </param>
        public void SetStructureRetrievalManager(ISdmxRetrievalManager structureRetrievalManager0)
        {
            this._structureRetrievalManager = structureRetrievalManager0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The count values.
        /// </summary>
        /// <param name="map">
        /// The map.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int CountValues(IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> map)
        {
            /* foreach */

            return map.Values.Sum(refences => refences.Count);
        }

        /// <summary>
        /// Handles a missing reference either by throwing an exception, if the populateMissingMap, or by populating the map, if both the map is not null and the reference exception has reference to the
        ///  cross referenced artefact.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <param name="populateMissingMap">
        /// The populate Missing Map.
        /// </param>
        private static void HandleMissingReference(
            ReferenceException e, IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMissingMap)
        {
            if (populateMissingMap != null && e.CrossReference != null)
            {
                ICrossReference crossReference = e.CrossReference;
                ISet<ICrossReference> missingRefences;
                var identifiableObject = (IIdentifiableObject)crossReference.ReferencedFrom;
                if (!populateMissingMap.TryGetValue(identifiableObject, out missingRefences) || missingRefences == null)
                {
                    missingRefences = new HashSet<ICrossReference>();
                    if (crossReference.ReferencedFrom.StructureType.IsIdentifiable)
                    {
                        populateMissingMap.Add(identifiableObject, missingRefences);
                    }
                    else
                    {
                        populateMissingMap.Add(
                            crossReference.ReferencedFrom.GetParent<IIdentifiableObject>(
                                typeof(IIdentifiableObject), true), 
                            missingRefences);
                    }
                }

                missingRefences.Add(crossReference);
            }
            else
            {
                throw e;
            }
        }

        /// <summary>
        /// The reset maps.
        /// </summary>
        private void ResetMaps()
        {
            this._crossReferences = new Dictionary<IIdentifiableObject, ISet<IIdentifiableObject>>();
            this._maintianables = new HashSet<IMaintainableObject>();
            this._agencies = new Dictionary<string, IAgency>();
        }

        /// <summary>
        /// The resolve maintainable from local maps.
        /// </summary>
        /// <param name="queryObject">
        /// The query object.
        /// </param>
        /// <param name="maintainableParent">
        /// The maintainable parent.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/>.
        /// </returns>
        /// <exception cref="ReferenceException">
        /// </exception>
        private IMaintainableObject ResolveMaintainableFromLocalMaps(
            IStructureReference queryObject, IMaintainableObject maintainableParent)
        {
            IMaintainableRefObject xref = queryObject.MaintainableReference;
            string agencyId = xref.AgencyId;
            string version = xref.Version;

            if (!xref.HasAgencyId())
            {
                agencyId = maintainableParent.AgencyId;
            }

            if (!ObjectUtil.ValidString(xref.MaintainableId))
            {
                throw new ReferenceException(
                    ExceptionCode.ReferenceErrorMissingParameters, queryObject.MaintainableStructureEnumType, xref);
            }

            if (!ObjectUtil.ValidString(xref.Version))
            {
                version = MaintainableBeanConstants.DefaultVersion;
            }

            /* foreach */
            foreach (IMaintainableObject currentMaint in this._maintianables)
            {
                if (currentMaint.StructureType == queryObject.MaintainableStructureEnumType)
                {
                    if (ObjectUtil.ValidString(currentMaint.AgencyId))
                    {
                        if (currentMaint.AgencyId.Equals(agencyId))
                        {
                            if (currentMaint.Id.Equals(xref.MaintainableId))
                            {
                                if (currentMaint.Version.Equals(version))
                                {
                                    return currentMaint;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The resolve references.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="resolveAgencies0">
        /// The resolve agencies 0.
        /// </param>
        /// <param name="numberLevelsDeep">
        /// The number levels deep.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <param name="populateMap">
        /// The populate map.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary{TKey,TValue}"/>.
        /// </returns>
        private IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> ResolveReferences(
            ISdmxObjects beans, 
            bool resolveAgencies0, 
            int numberLevelsDeep, 
            ISdmxRetrievalManager retrievalManager, 
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMap)
        {
            this.ResetMaps();
            this._resolveAgencies = resolveAgencies0;
            IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> returnMap;
            int numberBeansLast;
            int numberReferencesLast;

            int numberBeansCurrent = -1;
            int numberReferencesCurrent = -1;

            ISdmxObjects allBeans = beans;
            int currentLevel = 1;
            do
            {
                numberBeansLast = numberBeansCurrent;
                numberReferencesLast = numberReferencesCurrent;
                returnMap = this.ResolveReferencesInternal(allBeans, retrievalManager, populateMap);
                numberBeansCurrent = returnMap.Count;
                numberReferencesCurrent = CountValues(returnMap);

                allBeans = new SdmxObjectsImpl(beans);

                /* foreach */
                foreach (ISet<IIdentifiableObject> currentBeanSet in returnMap.Values)
                {
                    /* foreach */
                    foreach (IIdentifiableObject currentBean in currentBeanSet)
                    {
                        allBeans.AddIdentifiable(currentBean);
                    }
                }

                if (currentLevel == numberLevelsDeep)
                {
                    break;
                }
            }
            while (numberBeansCurrent != numberBeansLast || numberReferencesCurrent != numberReferencesLast);

            return returnMap;
        }

        /// <summary>
        /// The resolve references internal.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <param name="populateMissingMap">
        /// The populate missing map.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        /// <exception cref="ReferenceException">
        /// </exception>
        private IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> ResolveReferencesInternal(
            ISdmxObjects beans, 
            ISdmxRetrievalManager retrievalManager, 
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMissingMap)
        {
            this._structureRetrievalManager = retrievalManager;

            /* foreach */
            foreach (IAgency currentAgency in beans.Agencies)
            {
                this._agencies.Add(currentAgency.FullId, currentAgency);
            }

            // Add all the top level beans to the maintainables list
            beans.GetAllMaintinables().AddAll(this._maintianables);

            // LOOP THROUGH ALL THE BEANS AND RESOLVE ALL THE REFERENCES
            if (this._resolveAgencies)
            {
                /* foreach */
                foreach (IMaintainableObject currentBean in beans.GetAllMaintinables())
                {
                    try
                    {
                        this.ResoveAgency(currentBean);
                    }
                    catch (ReferenceException e)
                    {
                        throw new ReferenceException(
                            e, 
                            ExceptionCode.ReferenceError, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency), 
                            currentBean.StructureType, 
                            currentBean.ToString());
                    }
                }
            }

            ISet<IMaintainableObject> loopSet = new HashSet<IMaintainableObject>();
            this._maintianables.AddAll(loopSet);

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in loopSet)
            {
                this._log.Debug("Resolving References For : " + currentMaintainable.Urn);
                ISet<ICrossReference> crossReferences0 = currentMaintainable.CrossReferences;
                this._log.Debug("Number of References : " + crossReferences0.Count);
                int i = 0;

                /* foreach */
                foreach (ICrossReference crossReference in crossReferences0)
                {
                    i++;
                    if (this._log.IsDebugEnabled)
                    {
                        this._log.Debug(
                            "Resolving Reference " + i + ": " + crossReference + " - referenced from -"
                            + crossReference.ReferencedFrom.StructureType);
                    }

                    try
                    {
                        this.StoreRef(
                            crossReference.ReferencedFrom, this.ResolveCrossReference(crossReference, retrievalManager));
                    }
                    catch (ReferenceException e1)
                    {
                        HandleMissingReference(e1, populateMissingMap);
                    }
                }
            }

            return this._crossReferences;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////AGENCY REFERENCES                                            ///////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The resove agency.
        /// </summary>
        /// <param name="maint">
        /// The maint.
        /// </param>
        private void ResoveAgency(IMaintainableObject maint)
        {
            if (!this._resolveAgencies)
            {
                return;
            }

            if (maint.AgencyId.Equals(AgencySchemeBean_Constants.DEFAULT_SCHEME))
            {
                return;
            }

            IAgency agency = this.ResoveAgency(maint.AgencyId);
            this._agencies.Add(agency.Id, agency);
            this.StoreRef(maint, agency);
        }

        /// <summary>
        /// The resolve agency.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// The <see cref="IAgency"/>.
        /// </returns>
        /// <exception cref="ReferenceException">
        /// <see cref="ExceptionCode.ReferenceErrorUnresolvable"/>
        /// </exception>
        private IAgency ResoveAgency(string agencyId)
        {
            IAgency agency;
            if (this._agencies.TryGetValue(agencyId, out agency))
            {
                return agency;
            }

            if (this._structureRetrievalManager != null)
            {
                agency = this._structureRetrievalManager.GetAgency(agencyId);
            }

            if (agency == null)
            {
                throw new ReferenceException(
                    ExceptionCode.ReferenceErrorUnresolvable, 
                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency), 
                    agencyId);
            }

            return agency;
        }

        /// <summary>
        /// The store ref.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from.
        /// </param>
        /// <param name="reference">
        /// The reference.
        /// </param>
        private void StoreRef(ISDMXObject referencedFrom, IIdentifiableObject reference)
        {
            IIdentifiableObject refFromIdentifiable;
            if (referencedFrom.StructureType.IsIdentifiable)
            {
                refFromIdentifiable = (IIdentifiableObject)referencedFrom;
            }
            else
            {
                refFromIdentifiable = referencedFrom.GetParent<IIdentifiableObject>(typeof(IIdentifiableObject), true);
            }

            ISet<IIdentifiableObject> refList;
            if (!this._crossReferences.TryGetValue(refFromIdentifiable, out refList))
            {
                refList = new HashSet<IIdentifiableObject>();
                this._crossReferences.Add(refFromIdentifiable, refList);
            }

            refList.Add(reference);
        }

        #endregion

        // Spring Controlled
    }
}
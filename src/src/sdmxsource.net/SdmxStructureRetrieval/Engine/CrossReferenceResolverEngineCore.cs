// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceResolverEngineCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The <see cref="ICrossReferenceResolverEngine" /> implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;
    using Org.Sdmxsource.Util.Extensions;

    // JAVADOC missing

    /// <summary>
    ///   The <see cref="ICrossReferenceResolverEngine" /> implementation
    /// </summary>
    public class CrossReferenceResolverEngineCore : ICrossReferenceResolverEngine
    {
        #region Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(CrossReferenceResolverEngineCore));

        /// <summary>
        ///   The agencies.
        /// </summary>
        private IDictionary<string, IAgency> _agencies;

        /// <summary>
        ///   The cross references.
        /// </summary>
        private IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> _crossReferences;

        /// <summary>
        /// The map that contains all identifiable objet, with URN used a key.
        /// </summary>
        private readonly IDictionary<Uri, IIdentifiableObject> _allIdentifiables;

        /// <summary>
        ///   The resolve agencies.
        /// </summary>
        private bool _resolveAgencies;

        /// <summary>
        ///   The maintainable retrieval engine.
        /// </summary>
        private readonly IMaintainableCrossReferenceRetrieverEngine _maintainableCrossReferenceRetrieverEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossReferenceResolverEngineCore" /> class.
        /// </summary>
        public CrossReferenceResolverEngineCore() : this(null, null)
        {
           
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossReferenceResolverEngineCore" /> class.
        /// </summary>
        /// <param name="sdmxObjects"> The sdmx objects. </param>
        public CrossReferenceResolverEngineCore(ISdmxObjects sdmxObjects)
            : this()
        {
            this.AddObjectsToMap(sdmxObjects);
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossReferenceResolverEngineCore" /> class.
        /// </summary>
        /// <param name="structureRetrievalManager"> The structure retrieval manager. </param>
        public CrossReferenceResolverEngineCore(IIdentifiableRetrievalManager structureRetrievalManager, IMaintainableCrossReferenceRetrieverEngine maintainableCrossReferenceRetrieverEngine)
        {
            this._maintainableCrossReferenceRetrieverEngine = maintainableCrossReferenceRetrieverEngine;
            this._crossReferences = new DictionaryOfSets<IIdentifiableObject, IIdentifiableObject>();
            this._agencies = new Dictionary<string, IAgency>(StringComparer.Ordinal);
            this._allIdentifiables = new Dictionary<Uri, IIdentifiableObject>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   For the included <paramref name="sdmxObjects" />, returns a map of agency URN to maintainable Bean that references the agency
        /// </summary>
        /// <param name="sdmxObjects"> The included <c>SDMX</c> objects </param>
        /// <param name="retrievalManager"> The <see cref="ISdmxObjectRetrievalManager" /> </param>
        /// <returns> The included <paramref name="sdmxObjects" /> , returns a map of agency URN to maintainable Bean that references the agency </returns>
        public virtual IDictionary<string, ISet<IMaintainableObject>> GetMissingAgencies(ISdmxObjects sdmxObjects,
                                    IIdentifiableRetrievalManager identifiableRetrievalManager)
        {
            ISet<string> agencyIds = new HashSet<string>();

            /* foreach */
            ISet<IAgency> agencies = sdmxObjects.Agencies;
            foreach (IAgency acy in agencies)
            {
                agencyIds.Add(acy.FullId);
            }

            IDictionary<string, ISet<IMaintainableObject>> returnMap =
                new Dictionary<string, ISet<IMaintainableObject>>();

            /* foreach */
            ISet<IMaintainableObject> maintainableObjects = sdmxObjects.GetAllMaintainables();
            foreach (IMaintainableObject currentMaint in maintainableObjects)
            {
                string referencedAgencyId = currentMaint.AgencyId;
                if (!agencyIds.Contains(referencedAgencyId))
                {
                    if (identifiableRetrievalManager != null)
                    {
                        try
                        {
                            IAgency acy0 = ResolveAgency(referencedAgencyId, identifiableRetrievalManager);
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
        ///   Gets a Dictionary of <see cref="IIdentifiableObject" /> alongside any cross references they declare that could not be found in the set of
        ///   <paramref name="beans" />
        ///   provided, and the <paramref name="retrievalManager" /> (if given).
        ///   <p />
        ///   <b>NOTE :</b>An empty Map is returned if all cross references are present.
        /// </summary>
        /// <param name="beans"> - the objects to return the Map of missing references for </param>
        /// <param name="numberLevelsDeep"> references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced </param>
        /// <param name="retrievalManager"> - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied sdmxObjects </param>
        /// <returns> Map of IIdentifiableObject with a Set of CrossReferences that could not be resolved for the IIdentifiableObject - an empty Map is returned if all cross references are present </returns>
        public virtual IDictionary<IIdentifiableObject, ISet<ICrossReference>> GetMissingCrossReferences(
            ISdmxObjects beans, int numberLevelsDeep, IIdentifiableRetrievalManager retrievalManager)
        {
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> returnMap =
                new Dictionary<IIdentifiableObject, ISet<ICrossReference>>();
            this.ResolveReferences(beans, false, numberLevelsDeep, retrievalManager, returnMap);
            return returnMap;
        }

        /// <summary>
        ///   Resolves a reference from <paramref name="crossReference" />
        /// </summary>
        /// <param name="crossReference"> The cross reference instance </param>
        /// <param name="structRetrievalManager"> The structure Retrieval Manager. </param>
        /// <returns> a reference from <paramref name="crossReference" /> </returns>
        public virtual IIdentifiableObject ResolveCrossReference(
            ICrossReference crossReference, IIdentifiableRetrievalManager structRetrievalManager)
        {
            if (crossReference.TargetReference.EnumType == SdmxStructureEnumType.Agency)
            {
                return this.ResolveAgency(crossReference.ChildReference.Id, structRetrievalManager);
            }

            IIdentifiableObject resolvedIdentifiable = ResolveMaintainableFromLocalMaps(crossReference);
            if (resolvedIdentifiable != null)
            {
                return resolvedIdentifiable;
            }

            IIdentifiableObject identifiableBean = null;

            if (structRetrievalManager != null)
            {
                _log.Info("IdentifiableBean '" + crossReference + "' not found locally, check IdentifiableRetrievalManager");
                identifiableBean = structRetrievalManager.GetIdentifiableObject(crossReference);
            }

            if (identifiableBean == null)
            {
                throw new CrossReferenceException(crossReference);
            }

            AddMaintainableToMap(identifiableBean.MaintainableParent);
            return identifiableBean;
        }

        /// <summary>
        ///   Returns a set of structures that are directly referenced from this provision
        /// </summary>
        /// <param name="provision"> - the provision to resolve the references for </param>
        /// <param name="structRetrievalManager"> - must not be null as this will be used to resolve the references </param>
        /// <returns> a set of structures that are directly referenced from this provision </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IProvisionAgreementObject provision, IIdentifiableRetrievalManager structRetrievalManager)
        {
            if (structRetrievalManager == null)
            {
                throw new ArgumentNullException("structRetrievalManager", "StructureRetrievalManager can not be null");
            }

            ISet<IIdentifiableObject> returnSet = new HashSet<IIdentifiableObject>();
            if (provision.StructureUseage != null)
            {
                IIdentifiableObject structureUseage =
                    structRetrievalManager.GetIdentifiableObject(provision.StructureUseage);

                if (structureUseage == null)
                {
                    throw new CrossReferenceException(
                        provision.StructureUseage);
                }

                returnSet.Add(structureUseage);
            }

            if (provision.DataproviderRef != null)
            {
                IIdentifiableObject dataProvider = structRetrievalManager.GetIdentifiableObject(provision.DataproviderRef);
                if (dataProvider == null)
                    throw new CrossReferenceException(provision.DataproviderRef);

                returnSet.Add(dataProvider);
            }

            return returnSet;
        }

        /// <summary>
        ///   Returns a set of IdentifiableBeans that are directly referenced from this registration
        /// </summary>
        /// <param name="registation"> - the registration to resolve the references for </param>
        /// <param name="structRetrievalManager"> - Used to resolve the structure references. can be null if level = 1 and registration is linked to a provision (as only the provision manager is needed) </param>
        /// <param name="provRetrievalManager"> - Used to resolve the provision references. Can be null if registration is not linked to a provision </param>
        /// <returns> a set of IdentifiableBeans that are directly referenced from this registration </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IRegistrationObject registation,
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
                    throw new CrossReferenceException(
                        registation.ProvisionAgreementRef);
                }

                returnSet.Add(provision);
            }

            return returnSet;
        }

        /// <summary>
        ///   Resolves all references and returns a Map containing all the input sdmxObjects and the objects that are cross referenced,
        ///   the Map's key set contains the Identifiable that is the referencing object and the Map's value collection contains the referenced artifacts.
        /// </summary>
        /// <param name="beans"> - the <see cref="ISdmxObjects" /> container, containing all the sdmxObjects to check references for </param>
        /// <param name="resolveAgencies"> - if true the resolver engine will also attempt to resolve referenced agencies </param>
        /// <param name="numberLevelsDeep"> references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this. </param>
        /// <param name="retrievalManager"> - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied sdmxObjects </param>
        /// <returns> Map of referencing versus references </returns>
        /// <exception cref="CrossReferenceException">- if any of the references could not be resolved</exception>
        public virtual IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> ResolveReferences(
            ISdmxObjects beans, bool resolveAgencies, int numberLevelsDeep, IIdentifiableRetrievalManager retrievalManager)
        {
            return this.ResolveReferences(beans, resolveAgencies, numberLevelsDeep, retrievalManager, null);
        }

        /// <summary>
        ///   Returns a set of IdentifiableBeans that the IMaintainableObject cross references
        /// </summary>
        /// <param name="bean"> The bean. </param>
        /// <param name="resolveAgencies"> - if true will also resolve the agencies </param>
        /// <param name="numberLevelsDeep"> references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this. </param>
        /// <param name="retrievalManager"> - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied sdmxObjects </param>
        /// <exception cref="CrossReferenceException">- if any of the references could not be resolved</exception>
        /// <returns> a set of IdentifiableBeans that the IMaintainableObject cross references </returns>
        public virtual ISet<IIdentifiableObject> ResolveReferences(
            IMaintainableObject bean,
            bool resolveAgencies,
            int numberLevelsDeep,
            IIdentifiableRetrievalManager retrievalManager)
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

        #endregion

        #region Methods

        /// <summary>
        ///   The count values.
        /// </summary>
        /// <param name="map"> The map. </param>
        /// <returns> The <see cref="int" /> . </returns>
        private static int CountValues(IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> map)
        {
            return map.Values.Sum(refences => refences.Count);
        }

        /// <summary>
        ///   Handles a missing reference either by throwing an exception, if the populateMissingMap, or by populating the map, if both the map is not null and the reference exception has reference to the
        ///   cross referenced artefact.
        /// </summary>
        /// <param name="e"> The e. </param>
        /// <param name="populateMissingMap"> The populate Missing Map. </param>
        private static void HandleMissingReference(
            CrossReferenceException e, IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMissingMap)
        {
            if (populateMissingMap != null && e.CrossReference != null)
            {
                ICrossReference crossReference = e.CrossReference;

                // NOTE Made changes from Java port to make it work.
                var identifiableObject = crossReference.ReferencedFrom as IIdentifiableObject;
                if (identifiableObject != null)
                {
                    ISet<ICrossReference> missingRefences;
                    if (!populateMissingMap.TryGetValue(identifiableObject, out missingRefences) || missingRefences == null)
                    {
                        missingRefences = new HashSet<ICrossReference>();
                        populateMissingMap.Add(
                            crossReference.ReferencedFrom.StructureType.IsIdentifiable ? identifiableObject : crossReference.ReferencedFrom.GetParent<IIdentifiableObject>(true),
                            missingRefences);
                    }

                    missingRefences.Add(crossReference);
                }

            }
        }

        /// <summary>
        ///   The reset maps.
        /// </summary>
        private void ResetMaps()
        {
            this._crossReferences = new DictionaryOfSets<IIdentifiableObject, IIdentifiableObject>();
            //this._maintianables = new HashSet<IMaintainableObject>();
            this._agencies = new Dictionary<string, IAgency>();
        }

        /// <summary>
        ///   The resolve maintainable from local maps.
        /// </summary>
        /// <param name="queryObject"> The query object. </param>
        /// <returns> The <see cref="IMaintainableObject" /> . </returns>
        /// <exception cref="CrossReferenceException">Missing parameters - Invalid
        ///   <see cref="IMaintainableRefObject.MaintainableId" />
        ///   at
        ///   <paramref name="queryObject" />
        /// </exception>
        private IIdentifiableObject ResolveMaintainableFromLocalMaps(
            ICrossReference queryObject)
        {
            IIdentifiableObject identifiable;
            if (this._allIdentifiables.TryGetValue(queryObject.TargetUrn, out identifiable))
            {
                return identifiable;
            }

            return null;
        }

        /// <summary>
        ///   The resolve references.
        /// </summary>
        /// <param name="beans"> The sdmxObjects. </param>
        /// <param name="resolveAgencies"> The resolve agencies 0. </param>
        /// <param name="numberLevelsDeep"> The number levels deep. </param>
        /// <param name="retrievalManager"> The retrieval manager. </param>
        /// <param name="populateMap"> The populate map. </param>
        /// <returns> The <see cref="IDictionary{TKey,TValue}" /> . </returns>
        private IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> ResolveReferences(
            ISdmxObjects beans,
            bool resolveAgencies,
            int numberLevelsDeep,
            IIdentifiableRetrievalManager retrievalManager,
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMap)
        {
            _log.Info("Resolve References, bean retrieval manager: " + retrievalManager);

            this.ResetMaps();
            this._resolveAgencies = resolveAgencies;
            IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> returnMap;
            int numberBeansLast = 0;
            int numberReferencesLast = 0;

            int numberBeansCurrent = -1;
            int numberReferencesCurrent = -1;

            ISdmxObjects allBeans = beans;

            int currentLevel = 1;
            do
            {
                _log.Debug("numberBeansLast= " + numberBeansLast);
                _log.Debug("numberReferencesLast= " + numberReferencesLast);

                numberBeansLast = numberBeansCurrent;
                numberReferencesLast = numberReferencesCurrent;
                returnMap = this.ResolveReferencesInternal(allBeans, retrievalManager, populateMap);
                numberBeansCurrent = returnMap.Count;
                numberReferencesCurrent = CountValues(returnMap);

                allBeans = new SdmxObjectsImpl(beans);

                foreach (ISet<IIdentifiableObject> currentBeanSet in returnMap.Values)
                {
                    foreach (IIdentifiableObject currentBean in currentBeanSet)
                    {
                        allBeans.AddIdentifiable(currentBean);
                    }
                }

                _log.Debug("numberBeansLast= " + numberBeansLast);
                _log.Debug("numberReferencesLast= " + numberReferencesLast);
                _log.Debug("numberBeansCurrent= " + numberBeansCurrent);
                _log.Debug("numberReferencesCurrent= " + numberReferencesCurrent);
                _log.Debug("currentLevel= " + currentLevel);
                _log.Debug("numberLevelsDeep= " + numberLevelsDeep);

                if (currentLevel == numberLevelsDeep)
                {
                    break;
                }

                currentLevel++;
            }
            while (numberBeansCurrent != numberBeansLast || numberReferencesCurrent != numberReferencesLast);

            return returnMap;
        }

        /// <summary>
        ///   The resolve references internal.
        /// </summary>
        /// <param name="sdmxObjects"> The sdmxObjects. </param>
        /// <param name="retrievalManager"> The retrieval manager. </param>
        /// <param name="populateMissingMap"> The populate missing map. </param>
        /// <returns> The <see cref="IIdentifiableObject" /> dictionary. </returns>
        /// <exception cref="CrossReferenceException">Reference error</exception>
        private IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> ResolveReferencesInternal(
            ISdmxObjects sdmxObjects,
            IIdentifiableRetrievalManager retrievalManager,
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> populateMissingMap)
        {
            _log.Info("Resolve References, bean retrieval manager: " + retrievalManager);

            /* foreach */
            foreach (IAgency currentAgency in sdmxObjects.Agencies)
            {
                this._agencies.Add(currentAgency.FullId, currentAgency);
            }

            // Add all the top level sdmxObjects to the maintainables list
            this.AddObjectsToMap(sdmxObjects);

            // LOOP THROUGH ALL THE BEANS AND RESOLVE ALL THE REFERENCES
            if (this._resolveAgencies)
            {
                /* foreach */
                foreach (IMaintainableObject currentBean in sdmxObjects.GetAllMaintainables())
                {
                    try
                    {
                        this.ResolveAgency(currentBean, retrievalManager);
                    }
                    catch (CrossReferenceException e)
                    {
                        throw new SdmxReferenceException(e,
                            AgencyRef(currentBean.AgencyId));
                    }
                }
            }

            ISet<IMaintainableObject> loopSet = new HashSet<IMaintainableObject>();
            loopSet.AddAll(sdmxObjects.GetAllMaintainables());
            ISdmxObjectRetrievalManager retMan = new InMemoryRetrievalManager(sdmxObjects);

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in loopSet)
            {
                this._log.Debug("Resolving References For : " + currentMaintainable.Urn);
                ISet<ICrossReference> crossReferences0;
                if (_maintainableCrossReferenceRetrieverEngine != null)
                {
                    crossReferences0 = _maintainableCrossReferenceRetrieverEngine.GetCrossReferences(retMan, currentMaintainable);
                }
                else
                {
                    crossReferences0 = currentMaintainable.CrossReferences;
                }
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
                    catch (CrossReferenceException e)
                    {
                        if (populateMissingMap == null)
                        {
                            throw;
                        }
                        
                        HandleMissingReference(e, populateMissingMap);
                        //throw new ReferenceException(e, "Reference from structure '" + currentMaintainable.Urn + "' can not be resolved");
                    }
                }
            }

            return this._crossReferences;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////AGENCY REFERENCES                                            ///////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   The resolve agency.
        /// </summary>
        /// <param name="maintainable"> The maintainable. </param>
        /// <param name="sdmxObjectRetrievalManager"> </param>
        private void ResolveAgency(IMaintainableObject maintainable, IIdentifiableRetrievalManager sdmxObjectRetrievalManager)
        {
            if (!this._resolveAgencies)
            {
                return;
            }

            if (maintainable.AgencyId.Equals(AgencyScheme.DefaultScheme))
            {
                return;
            }

            IAgency agency = this.ResolveAgency(maintainable.AgencyId, sdmxObjectRetrievalManager);
            this._agencies.Add(agency.Id, agency);
            this.StoreRef(maintainable, agency);
        }

        /// <summary>
        ///   The resolve agency.
        /// </summary>
        /// <param name="agencyId"> The agency id. </param>
        /// <returns> The <see cref="IAgency" /> . </returns>
        /// <exception cref="SdmxReferenceException">
        ///   <see cref="ExceptionCode.ReferenceErrorUnresolvable" />
        /// </exception>
        private IAgency ResolveAgency(string agencyId, IIdentifiableRetrievalManager identifiableRetrievalManager)
        {
            IAgency agency;
            if (this._agencies.TryGetValue(agencyId, out agency))
            {
                return agency;
            }

            if (identifiableRetrievalManager != null)
            {
                string[] split = agencyId.Split(new[] { "\\." }, StringSplitOptions.RemoveEmptyEntries);
                string parentAgencyId = AgencyScheme.DefaultScheme;
                string targetAgencyId = agencyId;
                if (split.Length > 1)
                {
                    targetAgencyId = split[split.Length - 1];
                    split[split.Length - 1] = null;
                    string concat = "";
                    parentAgencyId = "";
                    foreach (string currentSplit in split)
                    {
                        if (currentSplit != null)
                            parentAgencyId += concat + currentSplit;

                        concat = ".";
                    }
                }
                IStructureReference agencyRef = new StructureReferenceImpl(parentAgencyId, AgencyScheme.FixedId, AgencyScheme.FixedVersion, SdmxStructureEnumType.Agency, targetAgencyId);
                agency = identifiableRetrievalManager.GetIdentifiableObject<IAgency>(agencyRef);
            }

            if (agency == null)
            {
                throw new SdmxReferenceException(AgencyRef(agencyId));
            }

            return agency;
        }

        /// <summary>
        ///   The store ref.
        /// </summary>
        /// <param name="referencedFrom"> The referenced from. </param>
        /// <param name="reference"> The reference. </param>
        private void StoreRef(ISdmxObject referencedFrom, IIdentifiableObject reference)
        {
            IIdentifiableObject refFromIdentifiable;
            if (referencedFrom.StructureType.IsIdentifiable)
            {
                refFromIdentifiable = (IIdentifiableObject)referencedFrom;
            }
            else
            {
                refFromIdentifiable = referencedFrom.GetParent<IIdentifiableObject>(true);
            }

            ISet<IIdentifiableObject> refList;
            if (!this._crossReferences.TryGetValue(refFromIdentifiable, out refList))
            {
                refList = new HashSet<IIdentifiableObject>();
                this._crossReferences.Add(refFromIdentifiable, refList);
            }

            refList.Add(reference);
        }

        /// <summary>
        /// Add the specified <paramref name="sdmxObjects"/> to map.
        /// </summary>
        /// <param name="sdmxObjects">
        /// The SDMX objects.
        /// </param>
        private void AddObjectsToMap(ISdmxObjects sdmxObjects)
        {
            foreach (IMaintainableObject maint in sdmxObjects.GetAllMaintainables())
            {
                this.AddMaintainableToMap(maint);
            }
        }

        /// <summary>
        /// Add <paramref name="maintainable"/> to map.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        private void AddMaintainableToMap(IMaintainableObject maintainable)
        {
            if (!this._allIdentifiables.ContainsKey(maintainable.Urn))
            {
                this._allIdentifiables.Add(maintainable.Urn, maintainable);
                foreach (IIdentifiableObject identifiableBean in maintainable.IdentifiableComposites)
                {
                    if (identifiableBean.StructureType.EnumType == SdmxStructureEnumType.Agency)
                    {
                        var acy = (IAgency)identifiableBean;
                        this._agencies.Add(acy.FullId, acy);
                    }

                    this._allIdentifiables.Add(identifiableBean.Urn, identifiableBean);
                }
            }
        }

        /// <summary>
        /// The agency ref.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id
        /// </param>
        /// <returns>
        /// The IStructureReference
        /// </returns>
        private IStructureReference AgencyRef(string agencyId)
        {
            string parentAgency = Api.Constants.InterfaceConstant.Agency.DefaultAgency;
            if (agencyId.Contains("."))
            {
                parentAgency = agencyId.Substring(0, agencyId.IndexOf('.'));
                agencyId = agencyId.Substring(agencyId.IndexOf('.') + 1);
            }

            return new StructureReferenceImpl(parentAgency, AgencyScheme.DefaultScheme, MaintainableObject.DefaultVersion, SdmxStructureEnumType.Agency, agencyId);
        }

        #endregion
    }
}
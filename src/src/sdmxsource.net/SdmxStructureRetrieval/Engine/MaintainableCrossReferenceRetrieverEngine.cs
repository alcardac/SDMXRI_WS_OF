// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableCrossReferenceRetrieverEngine.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The <see cref="IMaintainableCrossReferenceRetrieverEngine" /> implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    public class MaintainableCrossReferenceRetrieverEngine : IMaintainableCrossReferenceRetrieverEngine
    {
        #region Fields

        /// <summary>
        ///   The sdmx objects retrieval manager.
        /// </summary>
        private ISdmxObjectRetrievalManager _beanRetrievalManager;

        #endregion

        #region Public properties

        /// <summary>
        ///Gets and sets the sdmx objects retrieval manager.
        /// </summary>
        public ISdmxObjectRetrievalManager BeanRetrievalManager
        {
            get { return _beanRetrievalManager; }
            set { _beanRetrievalManager = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the set of cross referenced structures from this maintainable.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="maintainable">
        /// The maintainable object
        /// </param>
        /// <returns>
        /// The cross references set.
        /// </returns>
        public ISet<ICrossReference> GetCrossReferences(ISdmxObjectRetrievalManager retrievalManager, IMaintainableObject maintainable)
        {
            switch (maintainable.StructureType.EnumType)
            {
                case SdmxStructureEnumType.ContentConstraint:
                    return GetCrossReferences(retrievalManager, (IContentConstraintObject)maintainable);
                default:
                    return maintainable.CrossReferences;
            }
        }

        /// <summary>
        /// Returns the set of cross referenced structures.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="constraintBean">
        /// The constraint object
        /// </param>
        /// <returns>
        /// The cross references set.
        /// </returns>
        private ISet<ICrossReference> GetCrossReferences(ISdmxObjectRetrievalManager retrievalManager, IContentConstraintObject constraintBean)
        {
            ISet<ICrossReference> returnReferences = constraintBean.CrossReferences;

            IDictionary<string, IComponent> componentMap = new Dictionary<String, IComponent>();

            foreach (ICrossReference crossRef in constraintBean.ConstraintAttachment.StructureReference)
            {
                switch (crossRef.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.ProvisionAgreement:
                    case SdmxStructureEnumType.Dataflow:
                    case SdmxStructureEnumType.Dsd:
                        AddComponenents(retrievalManager, crossRef, componentMap);
                        break;
                    case SdmxStructureEnumType.DataProvider:
                        //Important:
                        //It is important to get this from the BeanRetrievalManager and not the ProvisionRetrievalManager
                        //As the ProvisionRetrievalManager relies on CrossReferecnce retrieval, which may not have been built yet (as
                        //This class is responsible for aiding the building of cross references
                        foreach (IProvisionAgreementObject currentProv in _beanRetrievalManager.GetMaintainableObjects<IProvisionAgreementObject>())
                        {
                            if (currentProv.DataproviderRef.TargetUrn.Equals(crossRef.TargetUrn))
                            {
                                AddComponenents(retrievalManager, currentProv.StructureUseage, componentMap);
                            }
                        }
                        foreach (IProvisionAgreementObject currentProv in retrievalManager.GetMaintainableObjects<IProvisionAgreementObject>())
                        {
                            if (currentProv.DataproviderRef.TargetUrn.Equals(crossRef.TargetUrn))
                            {
                                AddComponenents(retrievalManager, currentProv.StructureUseage, componentMap);
                            }
                        }
                        break;
                }
            }

            AddCrossReferencesForCubeRegion(constraintBean, returnReferences, componentMap, constraintBean.IncludedCubeRegion);
            AddCrossReferencesForCubeRegion(constraintBean, returnReferences, componentMap, constraintBean.ExcludedCubeRegion);
            AddCrossReferencesForConstraintKey(constraintBean, returnReferences, componentMap, constraintBean.IncludedSeriesKeys);
            AddCrossReferencesForConstraintKey(constraintBean, returnReferences, componentMap, constraintBean.ExcludedSeriesKeys);

            return returnReferences;
        }

        /// <summary>
        /// Adds components to the component map.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="crossRef">
        /// The cross reference object
        /// </param>
        /// <param name="componentMap">
        /// The components map
        /// </param>
        private void AddComponenents(ISdmxObjectRetrievalManager retrievalManager, ICrossReference crossRef, IDictionary<String, IComponent> componentMap)
        {
            switch (crossRef.TargetReference.EnumType)
            {
                case SdmxStructureEnumType.ProvisionAgreement:
                    IProvisionAgreementObject prov = retrievalManager.GetMaintainableObject<IProvisionAgreementObject>(crossRef);
                    if (prov == null)
                    {
                        prov = _beanRetrievalManager.GetMaintainableObject<IProvisionAgreementObject>(crossRef);
                        if (prov == null)
                            throw new CrossReferenceException(crossRef);
                    }
                    crossRef = prov.StructureUseage;  //Drop through to the next switch statement
                    break;
                case SdmxStructureEnumType.Dataflow:
                    IDataflowObject flow = retrievalManager.GetMaintainableObject<IDataflowObject>(crossRef);
                    if (flow == null)
                    {
                        flow = _beanRetrievalManager.GetMaintainableObject<IDataflowObject>(crossRef);
                        if (flow == null)
                            throw new CrossReferenceException(crossRef);
                    }
                    crossRef = flow.DataStructureRef;  //Drop through to the next switch statement
                    break;
                case SdmxStructureEnumType.Dsd:
                    IDataStructureObject dsd = retrievalManager.GetMaintainableObject<IDataStructureObject>(crossRef);
                    if (dsd == null)
                    {
                        dsd = _beanRetrievalManager.GetMaintainableObject<IDataStructureObject>(crossRef);
                        if (dsd == null)
                            throw new CrossReferenceException(crossRef);
                    }
                    foreach (IComponent currentComponent in dsd.Components)
                        componentMap.Add(currentComponent.Id, currentComponent);
                    break;
            }
        }

        /// <summary>
        /// Adds cross references for a constraint key to components map.
        /// </summary>
        /// <param name="constraintBean">
        /// The constraint object
        /// </param>
        /// <param name="returnReferences">
        /// The returned references
        /// </param>
        /// <param name="componentMap">
        /// The components map
        /// </param>
        /// <param name="constraintDataKey">
        /// The constraint data key
        /// </param>
        private void AddCrossReferencesForConstraintKey(IContentConstraintObject constraintBean,
                                                        ISet<ICrossReference> returnReferences,
                                                        IDictionary<String, IComponent> componentMap,
                                                        IConstraintDataKeySet constraintDataKey)
        {
            if (constraintDataKey == null)
                return;

            IDictionary<string, ISet<string>> componentCodeMap = new Dictionary<string, ISet<string>>();
            foreach (IConstrainedDataKey cdk in constraintDataKey.ConstrainedDataKeys)
            {
                foreach (IKeyValue kv in cdk.KeyValues)
                {
                    ISet<string> componentCodes = componentCodeMap[kv.Concept];
                    if (componentCodes == null)
                    {
                        componentCodes = new HashSet<String>();
                        componentCodeMap.Add(kv.Concept, componentCodes);
                    }
                    componentCodes.Add(kv.Code);
                }
            }
            foreach (string currentComponentId in componentCodeMap.Keys)
            {
                ICrossReference codelistRef = GetCodelistReferenceForComponent(constraintBean, currentComponentId, componentMap);
                if (codelistRef == null)
                    return;
                AddCodeReferences(constraintBean, returnReferences, codelistRef, componentCodeMap[currentComponentId]);
            }
        }

        /// <summary>
        /// Adds cross references for a cube region to components map.
        /// </summary>
        /// <param name="constraintBean">
        /// The constraint object
        /// </param>
        /// <param name="returnReferences">
        /// The returned references
        /// </param>
        /// <param name="componentMap">
        /// The components map
        /// </param>
        /// <param name="cubeRegionBean">
        /// The cube region object
        /// </param>
        private void AddCrossReferencesForCubeRegion(IContentConstraintObject constraintBean,
                                                     ISet<ICrossReference> returnReferences,
                                                     IDictionary<String, IComponent> componentMap,
                                                     ICubeRegion cubeRegionBean)
        {
            if (cubeRegionBean == null)
                return;

            AddCrossReferencesForCubeRegion(constraintBean, returnReferences, componentMap, cubeRegionBean.KeyValues);
            AddCrossReferencesForCubeRegion(constraintBean, returnReferences, componentMap, cubeRegionBean.AttributeValues);
        }

        /// <summary>
        /// Adds cross references for a cube region to components map.
        /// </summary>
        /// <param name="constraintBean">
        /// The constraint object
        /// </param>
        /// <param name="returnReferences">
        /// The returned references
        /// </param>
        /// <param name="componentMap">
        /// The components map
        /// </param>
        /// <param name="keyValues">
        /// The key values
        /// </param>
        private void AddCrossReferencesForCubeRegion(IContentConstraintObject constraintBean,
                                                    ISet<ICrossReference> returnReferences,
                                                    IDictionary<string, IComponent> componentMap,
                                                    IList<IKeyValues> keyValues)
        {
            foreach (IKeyValues kvs in keyValues)
            {
                ICrossReference codelistRef = GetCodelistReferenceForComponent(constraintBean, kvs.Id, componentMap);
                if (codelistRef == null)
                    return;

                AddCodeReferences(constraintBean, returnReferences, codelistRef, kvs.Values);
            }
        }

        /// <summary>
        /// Gets the codes list reference for a component.
        /// </summary>
        /// <param name="constraintBean">
        /// The constraint object.
        /// </param>
        /// <param name="componentId">
        /// The component id.
        /// </param>
        /// <param name="componentMap">
        /// The components map.
        /// </param>
        /// <returns>
        /// The cross reference object.
        /// </returns>
        private ICrossReference GetCodelistReferenceForComponent(IContentConstraintObject constraintBean, string componentId, IDictionary<string, IComponent> componentMap)
        {
            IComponent component = componentMap[componentId];
            if (component == null)
                throw new SdmxSemmanticException("Constraint '" + constraintBean + "' references component which can not be resolved: '" + componentId + "'");

            if (component.Representation == null || component.Representation.Representation == null)
            {
                //TODO THink about whether we allow to constraint uncoded components
                //throw new SdmxSemmanticException("Can not constrain component with id '"componentId"' as it is not a coded "  component.getStructureType().getType());
                return null;
            }
            return component.Representation.Representation;
        }

        /// <summary>
        /// Adds code references.
        /// </summary>
        /// <param name="constraintBean">
        /// The constraint object.
        /// </param>
        /// <param name="returnReferences">
        /// The returned references.
        /// </param>
        /// <param name="codelistRef">
        /// The code list reference object.
        /// </param>
        /// <param name="codeIds">
        /// The code ids.
        /// </param>
        private void AddCodeReferences(IContentConstraintObject constraintBean, ISet<ICrossReference> returnReferences, ICrossReference codelistRef, ICollection<string> codeIds)
        {
            string agencyId = codelistRef.AgencyId;
            string codelistId = codelistRef.MaintainableId;
            string version = codelistRef.Version;
            foreach (string codeId in codeIds)
            {
                ICrossReference codeRef = new CrossReferenceImpl(constraintBean, agencyId, codelistId, version, SdmxStructureEnumType.Code, codeId);
                returnReferences.Add(codeRef);
            }
        }

        #endregion
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthCachedAdvancedStructureRetriever.cs" company="Eurostat">
//   Date Created : 2013-09-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The authorization aware advanced structure retriever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///     The authorization aware advanced structure retriever.
    /// </summary>
    public class AuthCachedAdvancedStructureRetriever : IAuthAdvancedSdmxMutableObjectRetrievalManager
    {
        #region Static Fields

        /// <summary>
        ///     The builder that builds a <see cref="IStructureReference" /> from a <see cref="IMaintainableMutableObject" />
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthCachedRetrievalManager));

        #endregion

        #region Fields

        /// <summary>
        ///     The _request to artefacts.
        /// </summary>
        private readonly DictionaryOfSets<IStructureReference, IMaintainableMutableObject>[] _requestToArtefacts;

        /// <summary>
        ///     The _request to artefacts.
        /// </summary>
        private readonly Dictionary<IStructureReference, IMaintainableMutableObject>[] _requestToArtefactsLatest;

        /// <summary>
        ///     The _retrieval manager.
        /// </summary>
        private readonly IAuthAdvancedSdmxMutableObjectRetrievalManager _retrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCachedAdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager. It can be null in which case the <see cref="AdvancedStructureRetriever"/> will be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retrievalManager"/> is null.
        /// </exception>
        public AuthCachedAdvancedStructureRetriever(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            this._retrievalManager = retrievalManager;

            int maxCacheIndex = GetMaxCacheIndex();
            this._requestToArtefacts = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>[maxCacheIndex];
            for (int m = 0; m < maxCacheIndex; m++)
            {
                this._requestToArtefacts[m] = new DictionaryOfSets<IStructureReference, IMaintainableMutableObject>();
            }

            this._requestToArtefactsLatest = new Dictionary<IStructureReference, IMaintainableMutableObject>[maxCacheIndex];
            for (int m = 0; m < maxCacheIndex; m++)
            {
                this._requestToArtefactsLatest[m] = new Dictionary<IStructureReference, IMaintainableMutableObject>();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IAgencySchemeMutableObject"/> .
        /// </returns>
        public IAgencySchemeMutableObject GetMutableAgencyScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableAgencyScheme);
        }

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableAgencySchemeObjects);
        }

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject"/> .
        /// </returns>
        public ICategorisationMutableObject GetMutableCategorisation(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetLatest(complexRef, returnDetail, allowedDataflows, this._retrievalManager.GetMutableCategorisation);
        }

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.GetArtefacts(complexRef, returnDetail, allowedDataflows, this._retrievalManager.GetMutableCategorisationObjects);
        }

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorySchemeMutableObject"/> .
        /// </returns>
        public ICategorySchemeMutableObject GetMutableCategoryScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableCategoryScheme);
        }

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableCategorySchemeObjects);
        }

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.ICodelistMutableObject"/> .
        /// </returns>
        public ICodelistMutableObject GetMutableCodelist(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableCodelist);
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICodelistMutableObject> GetMutableCodelistObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableCodelistObjects);
        }

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme.IConceptSchemeMutableObject"/> .
        /// </returns>
        public IConceptSchemeMutableObject GetMutableConceptScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableConceptScheme);
        }

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableConceptSchemeObjects);
        }

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        public IContentConstraintMutableObject GetMutableContentConstraint(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableContentConstraint);
        }

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="complexRef">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableContentConstraintObjects);
        }

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataConsumerSchemeMutableObject"/> .
        /// </returns>
        public IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableDataConsumerScheme);
        }

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableDataConsumerSchemeObjects);
        }

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataProviderSchemeMutableObject"/> .
        /// </returns>
        public IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableDataProviderScheme);
        }

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableDataProviderSchemeObjects);
        }

        /// <summary>
        /// Gets a single DataStructure.
        ///     This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        ///     If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataStructureMutableObject"/> .
        /// </returns>
        public IDataStructureMutableObject GetMutableDataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableDataStructure);
        }

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableDataStructureObjects);
        }

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject"/> .
        /// </returns>
        public IDataflowMutableObject GetMutableDataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflow)
        {
            return this.GetLatest(complexRef, returnDetail, allowedDataflow, this._retrievalManager.GetMutableDataflow);
        }

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataflowMutableObject> GetMutableDataflowObjects(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflow)
        {
            return this.GetArtefacts(complexRef, returnDetail, allowedDataflow, this._retrievalManager.GetMutableDataflowObjects);
        }

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.IHierarchicalCodelistMutableObject"/> .
        /// </returns>
        public IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableHierarchicCodeList);
        }

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableHierarchicCodeListObjects);
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the
        ///     StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IMaintainableMutableObject"/> .
        /// </returns>
        public IMaintainableMutableObject GetMutableMaintainable(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflows)
        {
            switch (complexRef.ReferencedStructureType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return this.GetMutableAgencyScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.DataConsumerScheme:
                    return this.GetMutableDataConsumerScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.DataProviderScheme:
                    return this.GetMutableDataProviderScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Categorisation:
                    return this.GetMutableCategorisation(complexRef, returnDetail, allowedDataflows);
                case SdmxStructureEnumType.CategoryScheme:
                    return this.GetMutableCategoryScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.CodeList:
                    return this.GetMutableCodelist(complexRef, returnDetail);
                case SdmxStructureEnumType.ConceptScheme:
                    return this.GetMutableConceptScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Dataflow:
                    return this.GetMutableDataflow(complexRef, returnDetail, allowedDataflows);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return this.GetMutableHierarchicCodeList(complexRef, returnDetail);
                case SdmxStructureEnumType.Dsd:
                    return this.GetMutableDataStructure(complexRef, returnDetail);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.GetMutableMetadataflow(complexRef, returnDetail);
                case SdmxStructureEnumType.Msd:
                    return this.GetMutableMetadataStructure(complexRef, returnDetail);
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return this.GetMutableOrganisationUnitScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Process:
                    return this.GetMutableProcessObject(complexRef, returnDetail);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return this.GetMutableReportingTaxonomy(complexRef, returnDetail);
                case SdmxStructureEnumType.StructureSet:
                    return this.GetMutableStructureSet(complexRef, returnDetail);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return this.GetMutableProvisionAgreement(complexRef, returnDetail);
                case SdmxStructureEnumType.ContentConstraint:
                    return this.GetMutableContentConstraint(complexRef, returnDetail);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, complexRef.ReferencedStructureType);
            }
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the
        ///     StructureQueryObject parameter.
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflow.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Collections.Generic.ISet`1"/> .
        /// </returns>
        public ISet<IMaintainableMutableObject> GetMutableMaintainables(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            switch (complexRef.ReferencedStructureType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableAgencySchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.DataConsumerScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataConsumerSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.DataProviderScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataProviderSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Categorisation:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorisationObjects(complexRef, returnDetail, allowedDataflows));
                case SdmxStructureEnumType.CategoryScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorySchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.CodeList:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCodelistObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ConceptScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableConceptSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Dataflow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataflowObjects(complexRef, returnDetail, allowedDataflows));
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableHierarchicCodeListObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Dsd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataStructureObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.MetadataFlow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataflowObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Msd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataStructureObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableOrganisationUnitSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Process:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProcessObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableReportingTaxonomyObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.StructureSet:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableStructureSetObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ProvisionAgreement:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProvisionAgreementBeans(complexRef, returnDetail));
                case SdmxStructureEnumType.ContentConstraint:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableContentConstraintObjects(complexRef, returnDetail));
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, complexRef.ReferencedStructureType.ToString());
            }
        }

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The
        ///     <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject"/>
        ///     .
        /// </returns>
        public IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableMetadataStructure);
        }

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableMetadataStructureObjects);
        }

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject"/> .
        /// </returns>
        public IMetadataFlowMutableObject GetMutableMetadataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableMetadataflow);
        }

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableMetadataflowObjects);
        }

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IOrganisationUnitSchemeMutableObject"/> .
        /// </returns>
        public IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableOrganisationUnitScheme);
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null
        ///     or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableOrganisationUnitSchemeObjects);
        }

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process.IProcessMutableObject"/> .
        /// </returns>
        public IProcessMutableObject GetMutableProcessObject(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableProcessObject);
        }

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IProcessMutableObject> GetMutableProcessObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableProcessObjects);
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        ///     all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public IProvisionAgreementMutableObject GetMutableProvisionAgreement(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableProvisionAgreement);
        }

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="complexRef">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementBeans(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableProvisionAgreementBeans);
        }

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.IReportingTaxonomyMutableObject"/> .
        /// </returns>
        public IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableReportingTaxonomy);
        }

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableReportingTaxonomyObjects);
        }

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping.IStructureSetMutableObject"/> .
        /// </returns>
        public IStructureSetMutableObject GetMutableStructureSet(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetLatest(complexRef, returnDetail, this._retrievalManager.GetMutableStructureSet);
        }

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this.GetArtefacts(complexRef, returnDetail, this._retrievalManager.GetMutableStructureSetObjects);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the cache index
        /// </summary>
        /// <param name="queryDetail">
        /// The query detail.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> latest version cache.
        /// </param>
        /// <returns>
        /// The cache index depending on <paramref name="queryDetail"/> and <paramref name="returnLatest"/>
        /// </returns>
        private static int GetCacheIndex(BaseConstantType<ComplexStructureQueryDetailEnumType> queryDetail, TertiaryBool returnLatest)
        {
            var detailIndex = (int)queryDetail.EnumType << 1;
            var latestIndex = returnLatest != null && returnLatest.IsTrue ? 1 : 0;
            var index = detailIndex + latestIndex;
            return index;
        }

        /// <summary>
        /// Gets the maximum index of the cache.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int GetMaxCacheIndex()
        {
            return (ComplexStructureQueryDetail.Values.Count() * 2) + 1 /* return latest */ + 1;
        }

        /// <summary>
        /// Returns the artefacts of <typeparamref name="T"/> that match <paramref name="complexRef"/>
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <param name="getterAuth">
        /// The getter method to retrieve the artefacts if <see cref="_requestToArtefacts"/> doesn't not contain them with
        ///     authentication.
        /// </param>
        /// <typeparam name="T">
        /// The type of the returned artefacts.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        private ISet<T> GetArtefacts<T>(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflow, 
            Func<IComplexStructureReferenceObject, ComplexStructureQueryDetail, IList<IMaintainableRefObject>, ISet<T>> getterAuth) where T : class, IMaintainableMutableObject
        {
            return this.GetArtefacts(complexRef, returnDetail, (o, latest) => getterAuth(o, latest, allowedDataflow));
        }

        /// <summary>
        /// Returns the artefacts of <typeparamref name="T"/> that match <paramref name="complexRef"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the returned artefacts.
        /// </typeparam>
        /// <param name="complexRef">
        /// The complex preference.
        /// </param>
        /// <param name="returnDetail">
        /// The return detail.
        /// </param>
        /// <param name="getter">
        /// The getter method to retrieve the artefacts if <see cref="_requestToArtefacts"/> doesn't not
        ///     contain them.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        private ISet<T> GetArtefacts<T>(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            Func<IComplexStructureReferenceObject, ComplexStructureQueryDetail, ISet<T>> getter) where T : class, IMaintainableMutableObject
        {
            ISet<IMaintainableMutableObject> mutableObjects;
            ISet<T> retrievedObjects;
            IStructureReference structureReference = new StructureReferenceImpl(complexRef.GetMaintainableRefObject(), complexRef.ReferencedStructureType);
            var cache = this.GetCache(returnDetail, complexRef.VersionReference.IsReturnLatest);
            if (!cache.TryGetValue(structureReference, out mutableObjects))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Cache miss: {0}", structureReference);
                retrievedObjects = getter(complexRef, returnDetail);
                cache.Add(structureReference, new HashSet<IMaintainableMutableObject>(retrievedObjects));
                foreach (T retrievedObject in retrievedObjects)
                {
                    var reference = _fromMutable.Build(retrievedObject);
                    cache.AddToSet(reference, retrievedObject);
                }
            }
            else
            {
                retrievedObjects = new HashSet<T>(mutableObjects.Cast<T>());
            }

            return retrievedObjects;
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <param name="queryDetail">
        /// if set to <c>true</c> select the query detail cache.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> latest version cache.
        /// </param>
        /// <returns>
        /// The cache depending on <paramref name="queryDetail"/> and <paramref name="returnLatest"/>
        /// </returns>
        private DictionaryOfSets<IStructureReference, IMaintainableMutableObject> GetCache(BaseConstantType<ComplexStructureQueryDetailEnumType> queryDetail, TertiaryBool returnLatest)
        {
            var index = GetCacheIndex(queryDetail, returnLatest);
            return this._requestToArtefacts[index];
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <param name="queryDetail">
        /// if set to <c>true</c> select the query detail cache.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> latest version cache.
        /// </param>
        /// <returns>
        /// The cache depending on <paramref name="queryDetail"/> and <paramref name="returnLatest"/>
        /// </returns>
        private Dictionary<IStructureReference, IMaintainableMutableObject> GetCacheLatest(BaseConstantType<ComplexStructureQueryDetailEnumType> queryDetail, TertiaryBool returnLatest)
        {
            var index = GetCacheIndex(queryDetail, returnLatest);
            return this._requestToArtefactsLatest[index];
        }

        /// <summary>
        /// Return the latest artefact of type <typeparamref name="T"/> that matches the <paramref name="complexRef"/>.
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="getter">
        /// The getter method to retrieve the artefact if <see cref="_requestToArtefactsLatest"/> doesn't not contain it.
        /// </param>
        /// <typeparam name="T">
        /// The type of the requested artefact
        /// </typeparam>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/> of type <typeparamref name="T"/>; otherwise null
        /// </returns>
        private T GetLatest<T>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, Func<IComplexStructureReferenceObject, ComplexStructureQueryDetail, T> getter)
            where T : class, IMaintainableMutableObject
        {
            IMaintainableMutableObject mutableObject;
            IStructureReference structureReference = new StructureReferenceImpl(complexRef.GetMaintainableRefObject(), complexRef.ReferencedStructureType);
            var cache = this.GetCacheLatest(returnDetail, complexRef.VersionReference.IsReturnLatest);
            if (!cache.TryGetValue(structureReference, out mutableObject))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Cache miss: {0}", structureReference);
                T retrievedObject = getter(complexRef, returnDetail);
                if (retrievedObject != null)
                {
                    cache.Add(structureReference, retrievedObject);
                    var reference = _fromMutable.Build(retrievedObject);

                    var allVersionCache = this.GetCache(returnDetail, TertiaryBool.ParseBoolean(false));
                    allVersionCache.AddToSet(reference, retrievedObject);

                    var latestCache = this.GetCache(returnDetail, TertiaryBool.ParseBoolean(true));
                    latestCache.AddToSet(reference, retrievedObject);

                    return retrievedObject;
                }

                return null;
            }

            return mutableObject as T;
        }

        /// <summary>
        /// Return the latest artefact of type <typeparamref name="T"/> that matches the <paramref name="complexRef"/>.
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        /// <param name="getterAuth">
        /// The getter method to retrieve the artefact if <see cref="_requestToArtefactsLatest"/> doesn't not contain it with
        ///     used of the
        ///     <paramref name="allowedDataflow"/>
        ///     .
        /// </param>
        /// <typeparam name="T">
        /// The type of the requested artefact
        /// </typeparam>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/> of type <typeparamref name="T"/>; otherwise null
        /// </returns>
        private T GetLatest<T>(
            IComplexStructureReferenceObject complexRef, 
            ComplexStructureQueryDetail returnDetail, 
            IList<IMaintainableRefObject> allowedDataflow, 
            Func<IComplexStructureReferenceObject, ComplexStructureQueryDetail, IList<IMaintainableRefObject>, T> getterAuth) where T : class, IMaintainableMutableObject
        {
            return this.GetLatest(complexRef, returnDetail, (o, b) => getterAuth(o, b, allowedDataflow));
        }

        #endregion
    }
}
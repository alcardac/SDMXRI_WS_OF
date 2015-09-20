// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthAdvancedStructureRetriever.cs" company="Eurostat">
//   Date Created : 2013-06-14
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The auth advanced structure retriever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
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

    /// <summary>
    /// The authorization aware advanced structure retriever.
    /// </summary>
    public class AuthAdvancedStructureRetriever : IAuthAdvancedSdmxMutableObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        /// The _retrieval manager.
        /// </summary>
        private readonly IAdvancedSdmxMutableObjectRetrievalManager _retrievalManager;

        /// <summary>
        ///     The retrieval engine container.
        /// </summary>
        private readonly RetrievalEngineContainer _retrievalEngineContainer;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager. It can be null in which case the <see cref="AdvancedStructureRetriever"/> will be used.
        /// </param>
        /// <param name="mappingStoreDatabase">
        /// The mapping Store Database.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDatabase"/> is null.
        /// </exception>
        public AuthAdvancedStructureRetriever(IAdvancedSdmxMutableObjectRetrievalManager retrievalManager, Database mappingStoreDatabase) 
        {
            if (mappingStoreDatabase == null)
            {
                throw new ArgumentNullException("mappingStoreDatabase");
            }

            this._retrievalManager = retrievalManager ?? new AdvancedStructureRetriever(mappingStoreDatabase);
            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDatabase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedStructureRetriever"/> class. 
        /// </summary>
        /// <param name="mappingStoreSettings">
        /// The mapping Store Settings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreSettings"/> is null.
        /// </exception>
        public AuthAdvancedStructureRetriever(ConnectionStringSettings mappingStoreSettings) : this(null, mappingStoreSettings != null ? new Database(mappingStoreSettings) : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="mappingStoreDatabase">
        /// The mapping store database.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDatabase"/> is null.
        /// </exception>
        public AuthAdvancedStructureRetriever(Database mappingStoreDatabase) : this(null, mappingStoreDatabase)
        {
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
            return this._retrievalManager.GetMutableAgencyScheme(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableAgencySchemeObjects(complexRef, returnDetail);
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
            IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflows)
        {
            var xref = complexRef.GetMaintainableRefObject();
            if (xref.HasVersion())
            {
                return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints(), allowedDataflows).FirstOrDefault();
            }

            //// complexRef.GetVersionConstraints() Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType, allowedDataflows);
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
            IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflows)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints(), allowedDataflows);
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
            return this._retrievalManager.GetMutableCategoryScheme(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableCategorySchemeObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableCodelist(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableCodelistObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableConceptScheme(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableConceptSchemeObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableContentConstraint(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableContentConstraintObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataConsumerScheme(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataConsumerSchemeObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataProviderScheme(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataProviderSchemeObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataStructure(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableDataStructureObjects(complexRef, returnDetail);
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
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints(), allowedDataflow).FirstOrDefault()
                       : this._retrievalEngineContainer.DataflowRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType, allowedDataflow);
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
            IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflow)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints(), allowedDataflow);
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
            return this._retrievalManager.GetMutableHierarchicCodeList(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableHierarchicCodeListObjects(complexRef, returnDetail);
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
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
            IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList<IMaintainableRefObject> allowedDataflows)
        {
            throw new NotImplementedException();
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
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject"/> .
        /// </returns>
        public IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            return this._retrievalManager.GetMutableMetadataStructure(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableMetadataStructureObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableMetadataflow(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableMetadataflowObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableOrganisationUnitScheme(complexRef, returnDetail);
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
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
            return this._retrievalManager.GetMutableOrganisationUnitSchemeObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableProcessObject(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableProcessObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableProvisionAgreement(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableProvisionAgreementBeans(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableReportingTaxonomy(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableReportingTaxonomyObjects(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableStructureSet(complexRef, returnDetail);
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
            return this._retrievalManager.GetMutableStructureSetObjects(complexRef, returnDetail);
        }

        #endregion
    }
}
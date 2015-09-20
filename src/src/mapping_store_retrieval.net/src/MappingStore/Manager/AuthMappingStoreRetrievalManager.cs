// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthMappingStoreRetrievalManager.cs" company="Eurostat">
//   Date Created : 2014-05-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mapping store retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;

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

    /// <summary>
    ///     The mapping store retrieval manager.
    /// </summary>
    public class AuthMappingStoreRetrievalManager : AuthRetrievalManagerBase
    {
        #region Fields

        /// <summary>
        ///     The retrieval engine container.
        /// </summary>
        private readonly RetrievalEngineContainer _retrievalEngineContainer;

        /// <summary>
        ///     The retrieval manager.
        /// </summary>
        private readonly ISdmxMutableObjectRetrievalManager _retrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMappingStoreRetrievalManager"/> class.
        /// </summary>
        /// <param name="mappingStoreSettings">
        /// The mapping store DB.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        public AuthMappingStoreRetrievalManager(ConnectionStringSettings mappingStoreSettings, ISdmxMutableObjectRetrievalManager retrievalManager)
        {
            var mappingStoreDB = new Database(mappingStoreSettings);
            this._retrievalManager = retrievalManager ?? new MappingStoreRetrievalManager(mappingStoreDB);
            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDB);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMappingStoreRetrievalManager"/> class.
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping store DB.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        public AuthMappingStoreRetrievalManager(Database mappingStoreDB, ISdmxMutableObjectRetrievalManager retrievalManager)
        {
            this._retrievalManager = retrievalManager ?? new MappingStoreRetrievalManager(mappingStoreDB);
            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDB);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IAgencySchemeMutableObject"/> .
        /// </returns>
        public override IAgencySchemeMutableObject GetMutableAgencyScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableAgencyScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableAgencySchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed
        ///     dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject"/> .
        /// </returns>
        public override ICategorisationMutableObject GetMutableCategorisation(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (xref.HasVersion())
            {
                return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All, allowedDataflows).FirstOrDefault();
            }

            //// TODO Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail(), allowedDataflows);
        }

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(
            IMaintainableRefObject xref, 
            bool returnLatest, 
            bool returnStub, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            //// TODO Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints(), allowedDataflows);
        }

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorySchemeMutableObject"/> .
        /// </returns>
        public override ICategorySchemeMutableObject GetMutableCategoryScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableCategoryScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableCategorySchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.ICodelistMutableObject"/> .
        /// </returns>
        public override ICodelistMutableObject GetMutableCodelist(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableCodelist(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableCodelistObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme.IConceptSchemeMutableObject"/> .
        /// </returns>
        public override IConceptSchemeMutableObject GetMutableConceptScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableConceptScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableConceptSchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        public override IContentConstraintMutableObject GetMutableContentConstraint(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableContentConstraint(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableContentConstraintObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataConsumerSchemeMutableObject"/> .
        /// </returns>
        public override IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataConsumerScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataConsumerSchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataProviderSchemeMutableObject"/> .
        /// </returns>
        public override IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataProviderScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataProviderSchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single DataStructure.
        ///     This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        ///     If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataStructureMutableObject"/> .
        /// </returns>
        public override IDataStructureMutableObject GetMutableDataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataStructure(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableDataStructureObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed
        ///     dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject"/> .
        /// </returns>
        public override IDataflowMutableObject GetMutableDataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All, allowedDataflows).FirstOrDefault()
                       : this._retrievalEngineContainer.DataflowRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail(), allowedDataflows);
        }

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataflowMutableObject> GetMutableDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints(), allowedDataflows);
        }

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.IHierarchicalCodelistMutableObject"/> .
        /// </returns>
        public override IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableHierarchicCodeList(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableHierarchicCodeListObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The
        ///     <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject"/>
        ///     .
        /// </returns>
        public override IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableMetadataStructure(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableMetadataStructureObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject"/> .
        /// </returns>
        public override IMetadataFlowMutableObject GetMutableMetadataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableMetadataflow(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableMetadataflowObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IOrganisationUnitSchemeMutableObject"/> .
        /// </returns>
        public override IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableOrganisationUnitScheme(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null
        ///     or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableOrganisationUnitSchemeObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process.IProcessMutableObject"/> .
        /// </returns>
        public override IProcessMutableObject GetMutableProcessObject(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableProcessObject(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProcessMutableObject> GetMutableProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableProcessObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        ///     all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override IProvisionAgreementMutableObject GetMutableProvisionAgreement(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableProvisionAgreement(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableProvisionAgreementObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// if set to <c>true</c> [return latest].
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.IReportingTaxonomyMutableObject"/> .
        /// </returns>
        public override IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableReportingTaxonomy(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableReportingTaxonomyObjects(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping.IStructureSetMutableObject"/> .
        /// </returns>
        public override IStructureSetMutableObject GetMutableStructureSet(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableStructureSet(xref, returnLatest, returnStub);
        }

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// Set to <c>true</c> to return only the latest version.
        /// </param>
        /// <param name="returnStub">
        /// Set to <c>true</c> to return only stubs.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalManager.GetMutableStructureSetObjects(xref, returnLatest, returnStub);
        }

        #endregion
    }
}
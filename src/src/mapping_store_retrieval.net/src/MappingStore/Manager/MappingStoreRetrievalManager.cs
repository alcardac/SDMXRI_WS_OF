// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingStoreRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mapping store retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;

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
    public class MappingStoreRetrievalManager : RetrievalManagerBase
    {
        #region Fields

        /// <summary>
        ///     The retrieval engine container.
        /// </summary>
        private readonly RetrievalEngineContainer _retrievalEngineContainer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreRetrievalManager"/> class.
        /// </summary>
        /// <param name="mappingStoreSettings">
        /// The mapping store settings.
        /// </param>
        public MappingStoreRetrievalManager(ConnectionStringSettings mappingStoreSettings) : this(mappingStoreSettings != null ? new Database(mappingStoreSettings) : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreRetrievalManager"/> class.
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping Store DB.
        /// </param>
        public MappingStoreRetrievalManager(Database mappingStoreDB)
        {
            if (mappingStoreDB == null)
            {
                throw new ArgumentNullException("mappingStoreDB");
            }

            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDB);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IAgencySchemeMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IAgencySchemeMutableObject GetMutableAgencyScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject" /> .
        /// </returns>
        public override ICategorisationMutableObject GetMutableCategorisation(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            if (xref.HasVersion())
            {
                return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault();
            }

            //// TODO Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            //// TODO Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorySchemeMutableObject" /> .
        /// </returns>
        public override ICategorySchemeMutableObject GetMutableCategoryScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.CategorySchemeRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.CategorySchemeRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.CategorySchemeRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.ICodelistMutableObject" /> .
        /// </returns>
        public override ICodelistMutableObject GetMutableCodelist(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.CodeListRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.CodeListRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.CodeListRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme.IConceptSchemeMutableObject" /> .
        /// </returns>
        public override IConceptSchemeMutableObject GetMutableConceptScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IContentConstraintMutableObject GetMutableContentConstraint(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataConsumerSchemeMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataProviderSchemeMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single DataStructure.
        /// This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        /// If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataStructureMutableObject" /> .
        /// </returns>
        public override IDataStructureMutableObject GetMutableDataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.DSDRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.DSDRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.DSDRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject" /> .
        /// </returns>
        public override IDataflowMutableObject GetMutableDataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.DataflowRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataflowMutableObject> GetMutableDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.IHierarchicalCodelistMutableObject" /> .
        /// </returns>
        public override IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return xref.HasVersion() && !returnLatest
                       ? this._retrievalEngineContainer.HclRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), VersionQueryType.All).FirstOrDefault()
                       : this._retrievalEngineContainer.HclRetrievalEngine.RetrieveLatest(xref, returnStub.GetComplexQueryDetail());
        }

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            return this._retrievalEngineContainer.HclRetrievalEngine.Retrieve(xref, returnStub.GetComplexQueryDetail(), returnLatest.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IMetadataFlowMutableObject GetMutableMetadataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IOrganisationUnitSchemeMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process.IProcessMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IProcessMutableObject GetMutableProcessObject(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProcessMutableObject> GetMutableProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IProvisionAgreementMutableObject GetMutableProvisionAgreement(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="xref">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public override ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.IReportingTaxonomyMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping.IStructureSetMutableObject" /> .
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented</exception>
        public override IStructureSetMutableObject GetMutableStructureSet(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="xref">
        /// The maintainable reference.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingStoreSdmxObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mapping store SDMX object retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
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
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;

    /// <summary>
    ///     The mapping store SDMX object retrieval manager.
    /// </summary>
    public class MappingStoreSdmxObjectRetrievalManager : BaseSdmxObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The _mutable object retrieval manager.
        /// </summary>
        private readonly ISdmxMutableObjectRetrievalManager _mutableObjectRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreSdmxObjectRetrievalManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public MappingStoreSdmxObjectRetrievalManager(ConnectionStringSettings connectionStringSettings)
        {
            this._mutableObjectRetrievalManager = new MappingStoreRetrievalManager(connectionStringSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreSdmxObjectRetrievalManager"/> class.
        /// </summary>
        /// <param name="mutableObjectRetrievalManager">
        /// The mutable object retrieval manager.
        /// </param>
        public MappingStoreSdmxObjectRetrievalManager(ISdmxMutableObjectRetrievalManager mutableObjectRetrievalManager)
        {
            this._mutableObjectRetrievalManager = mutableObjectRetrievalManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns AgencySchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all AgencySchemeObjects
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
        public override ISet<IAgencyScheme> GetAgencySchemeObjects(IMaintainableRefObject xref,  bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns AttachmentConstraintBeans that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns CategorisationObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
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
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableCategorisationObjects(xref, false, returnStub);
            return new HashSet<ICategorisationObject>(mutableObjects.Select(o => o.ImmutableInstance)); 
        }

        /// <summary>
        /// Returns CategorySchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICategorySchemeObject> GetCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableCategorySchemeObjects(xref, returnLatest, returnStub);
            return new HashSet<ICategorySchemeObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns CodelistObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<ICodelistObject> GetCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableCodelistObjects(xref, returnLatest, returnStub);
            return new HashSet<ICodelistObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns ConceptSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IConceptSchemeObject> GetConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableConceptSchemeObjects(xref, returnLatest, returnStub);
            return new HashSet<IConceptSchemeObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns ContentConstraintObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IContentConstraintObject> GetContentConstraints(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableContentConstraintObjects(xref, returnLatest, returnStub);
            return new HashSet<IContentConstraintObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns DataConsumerSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataConsumerSchemeObjects
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns DataProviderSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataProviderSchemeObjects
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns DataStructureObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataStructureObject> GetDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableDataStructureObjects(xref, returnLatest, returnStub);
            return new HashSet<IDataStructureObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns DataflowObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IDataflowObject> GetDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableDataflowObjects(xref, returnLatest, returnStub);
            return new HashSet<IDataflowObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns HierarchicalCodelistObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IHierarchicalCodelistObject> GetHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableHierarchicCodeListObjects(xref, returnLatest, returnStub);
            return new HashSet<IHierarchicalCodelistObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns MetadataStructureObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataStructureDefinitionObject> GetMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableMetadataStructureObjects(xref, returnLatest, returnStub);
            return new HashSet<IMetadataStructureDefinitionObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns MetadataFlowObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IMetadataFlow> GetMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableMetadataflowObjects(xref, returnLatest, returnStub);
            return new HashSet<IMetadataFlow>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns OrganisationUnitSchemeObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableOrganisationUnitSchemeObjects(xref, returnLatest, returnStub);
            return new HashSet<IOrganisationUnitSchemeObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns ProcessObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProcessObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProcessObject> GetProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableProcessObjects(xref, returnLatest, returnStub);
            return new HashSet<IProcessObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns ProvisionAgreementObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreementObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IProvisionAgreementObject> GetProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableProvisionAgreementObjects(xref, returnLatest, returnStub);
            return new HashSet<IProvisionAgreementObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns ReportingTaxonomyObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IReportingTaxonomyObject> GetReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableReportingTaxonomyObjects(xref, returnLatest, returnStub);
            return new HashSet<IReportingTaxonomyObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        /// <summary>
        /// Returns StructureSetObjects that match the parameters in the ref object.  If the ref object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="xref">
        /// The reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnLatest">
        /// If true then the latest versions of the structures that match the query will be returned.  If version information is supplied
        ///     then it will be ignored
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// Set of sdmxObjects that match the search criteria
        /// </returns>
        public override ISet<IStructureSetObject> GetStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub)
        {
            var mutableObjects = this._mutableObjectRetrievalManager.GetMutableStructureSetObjects(xref, returnLatest, returnStub);
            return new HashSet<IStructureSetObject>(mutableObjects.Select(o => o.ImmutableInstance));
        }

        #endregion
    }
}
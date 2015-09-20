// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutableObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable
{
    #region Using directives

    using System.Collections.Generic;

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

    #endregion

    /// <summary>
    ///     Manages the retrieval of MaintainableMutableObject structures
    /// </summary>
    public interface ISdmxMutableObjectRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IAgencySchemeMutableObject" /> .
        /// </returns>
        IAgencySchemeMutableObject GetMutableAgencyScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="ICategorisationMutableObject" /> .
        /// </returns>
        ICategorisationMutableObject GetMutableCategorisation(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="ICategorySchemeMutableObject" /> .
        /// </returns>
        ICategorySchemeMutableObject GetMutableCategoryScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="ICodelistMutableObject" /> .
        /// </returns>
        ICodelistMutableObject GetMutableCodelist(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IConceptSchemeMutableObject" /> .
        /// </returns>
        IConceptSchemeMutableObject GetMutableConceptScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        IContentConstraintMutableObject GetMutableContentConstraint(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IDataConsumerSchemeMutableObject" /> .
        /// </returns>
        IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IDataProviderSchemeMutableObject" /> .
        /// </returns>
        IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single DataStructure.
        /// This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        /// If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IDataStructureMutableObject" /> .
        /// </returns>
        IDataStructureMutableObject GetMutableDataStructure(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IDataflowMutableObject" /> .
        /// </returns>
        IDataflowMutableObject GetMutableDataflow(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IDataflowMutableObject> GetMutableDataflowObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistMutableObject" /> .
        /// </returns>
        IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        /// <p />
        /// Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject" /> .
        /// </returns>
        IMaintainableMutableObject GetMutableMaintainable(IStructureReference query, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> .
        /// </returns>
        ISet<IMaintainableMutableObject> GetMutableMaintainables(IStructureReference query, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IMetadataStructureDefinitionMutableObject" /> .
        /// </returns>
        IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IMetadataFlowMutableObject" /> .
        /// </returns>
        IMetadataFlowMutableObject GetMutableMetadataflow(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IOrganisationUnitSchemeMutableObject" /> .
        /// </returns>
        IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IProcessMutableObject" /> .
        /// </returns>
        IProcessMutableObject GetMutableProcessObject(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IProcessMutableObject> GetMutableProcessObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        /// all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        IProvisionAgreementMutableObject GetMutableProvisionAgreement(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IReportingTaxonomyMutableObject" /> .
        /// </returns>
        IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="maintainableReference">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <returns>
        /// The <see cref="IStructureSetMutableObject" /> .
        /// </returns>
        IStructureSetMutableObject GetMutableStructureSet(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="maintainableReference">
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
        ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IMaintainableRefObject maintainableReference, bool returnLatest, bool returnStub);

        #endregion
    }
}
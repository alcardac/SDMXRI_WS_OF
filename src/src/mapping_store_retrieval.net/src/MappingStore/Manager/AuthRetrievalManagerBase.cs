// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetrievalManagerBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The retrieval manager base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Manager;

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

    /// <summary>
    ///     The retrieval manager base class.
    /// </summary>
    public abstract class AuthRetrievalManagerBase : IAuthSdmxMutableObjectRetrievalManager
    {
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
        public abstract IAgencySchemeMutableObject GetMutableAgencyScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        /// a URN or all the attributes required to uniquely identify the object.  If version information
        /// is missing then the latest version is assumed.
        /// </summary>
        /// <param name="xref">The maintainable reference.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <param name="allowedDataflows">The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.</param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject" /> .
        /// </returns>
        public abstract ICategorisationMutableObject GetMutableCategorisation(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows);

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
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public abstract ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(
            IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows);

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
        public abstract ICategorySchemeMutableObject GetMutableCategoryScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ICodelistMutableObject GetMutableCodelist(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IConceptSchemeMutableObject GetMutableConceptScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IContentConstraintMutableObject GetMutableContentConstraint(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IDataStructureMutableObject GetMutableDataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
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
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject"/> .
        /// </returns>
        public abstract IDataflowMutableObject GetMutableDataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows);

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
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public abstract ISet<IDataflowMutableObject> GetMutableDataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows);

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
        public abstract IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        /// <p />
        /// Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="returnLatest">if set to <c>true</c> [return latest].</param>
        /// <param name="returnStub">The return Stub.</param>
        /// <param name="allowedDataflows">The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.</param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject" /> .
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">Unsupported structure.</exception>
        public IMaintainableMutableObject GetMutableMaintainable(IStructureReference query, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            IMaintainableRefObject xref = query.MaintainableReference;
            switch (query.MaintainableStructureEnumType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return this.GetMutableAgencyScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.DataConsumerScheme:
                    return this.GetMutableDataConsumerScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.DataProviderScheme:
                    return this.GetMutableDataProviderScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.Categorisation:
                    return this.GetMutableCategorisation(xref, returnLatest, returnStub, allowedDataflows);
                case SdmxStructureEnumType.CategoryScheme:
                    return this.GetMutableCategoryScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.CodeList:
                    return this.GetMutableCodelist(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.ConceptScheme:
                    return this.GetMutableConceptScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.Dataflow:
                    return this.GetMutableDataflow(xref, returnLatest, returnStub, allowedDataflows);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return this.GetMutableHierarchicCodeList(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.Dsd:
                    return this.GetMutableDataStructure(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.GetMutableMetadataflow(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.Msd:
                    return this.GetMutableMetadataStructure(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return this.GetMutableOrganisationUnitScheme(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.Process:
                    return this.GetMutableProcessObject(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return this.GetMutableReportingTaxonomy(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.StructureSet:
                    return this.GetMutableStructureSet(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return this.GetMutableProvisionAgreement(xref, returnLatest, returnStub);
                case SdmxStructureEnumType.ContentConstraint:
                    return this.GetMutableContentConstraint(xref, returnLatest, returnStub);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, query.TargetReference);
            }
        }

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
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> .
        /// </returns>
        public ISet<IMaintainableMutableObject> GetMutableMaintainables(IStructureReference query, bool returnLatest, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            IMaintainableRefObject xref = query.MaintainableReference;
            switch (query.MaintainableStructureEnumType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableAgencySchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.DataConsumerScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataConsumerSchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.DataProviderScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataProviderSchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.Categorisation:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorisationObjects(xref, returnLatest, returnStub, allowedDataflows));
                case SdmxStructureEnumType.CategoryScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorySchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.CodeList:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCodelistObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.ConceptScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableConceptSchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.Dataflow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataflowObjects(xref, returnLatest, returnStub, allowedDataflows));
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableHierarchicCodeListObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.Dsd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataStructureObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.MetadataFlow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataflowObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.Msd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataStructureObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableOrganisationUnitSchemeObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.Process:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProcessObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableReportingTaxonomyObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.StructureSet:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableStructureSetObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.ProvisionAgreement:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProvisionAgreementObjects(xref, returnLatest, returnStub));
                case SdmxStructureEnumType.ContentConstraint:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableContentConstraintObjects(xref, returnLatest, returnStub));
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, query.TargetReference);
            }
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
        public abstract IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
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
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject"/> .
        /// </returns>
        public abstract IMetadataFlowMutableObject GetMutableMetadataflow(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IProcessMutableObject GetMutableProcessObject(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IProcessMutableObject> GetMutableProcessObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IProvisionAgreementMutableObject GetMutableProvisionAgreement(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract IStructureSetMutableObject GetMutableStructureSet(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

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
        public abstract ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IMaintainableRefObject xref, bool returnLatest, bool returnStub);

        #endregion
    }
}
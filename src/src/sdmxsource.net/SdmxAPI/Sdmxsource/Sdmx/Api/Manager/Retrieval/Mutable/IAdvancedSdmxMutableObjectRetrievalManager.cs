// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdvancedSdmxMutableObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///     Manages the retrieval of MaintainableMutableObject structures
    /// </summary>
    public interface IAdvancedSdmxMutableObjectRetrievalManager
    {
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
        /// The <see cref="IAgencySchemeMutableObject"/> .
        /// </returns>
        IAgencySchemeMutableObject GetMutableAgencyScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// <returns>
        /// The <see cref="ICategorisationMutableObject"/> .
        /// </returns>
        ICategorisationMutableObject GetMutableCategorisation(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="ICategorySchemeMutableObject"/> .
        /// </returns>
        ICategorySchemeMutableObject GetMutableCategoryScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="ICodelistMutableObject"/> .
        /// </returns>
        ICodelistMutableObject GetMutableCodelist(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<ICodelistMutableObject> GetMutableCodelistObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IConceptSchemeMutableObject"/> .
        /// </returns>
        IConceptSchemeMutableObject GetMutableConceptScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        IContentConstraintMutableObject GetMutableContentConstraint(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IDataConsumerSchemeMutableObject"/> .
        /// </returns>
        IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IDataProviderSchemeMutableObject"/> .
        /// </returns>
        IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IDataStructureMutableObject"/> .
        /// </returns>
        IDataStructureMutableObject GetMutableDataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// <returns>
        /// The <see cref="IDataflowMutableObject"/> .
        /// </returns>
        IDataflowMutableObject GetMutableDataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        ISet<IDataflowMutableObject> GetMutableDataflowObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IHierarchicalCodelistMutableObject"/> .
        /// </returns>
        IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/> .
        /// </returns>
        IMaintainableMutableObject GetMutableMaintainable(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

        ///// <summary>
        ///// Gets a set of maintainable objects which includes the maintainable(s) being queried for, defined by the StructureQueryObject parameter,
        /////     and all the objects that are cross referenced by either the returned maintainable or any other maintainable in the returned set.
        ///// </summary>
        ///// <param name="query">
        ///// - containing both the referenced structure type and the attributes for that structure
        ///// </param>
        ///// <returns>
        ///// The <see cref="ISet{IMaintainableMutableObject}"/> .
        ///// </returns>
        // ISet<IMaintainableMutableObject> GetMutableMaintainableWithReferences(IStructureReference query);

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetaiy">
        /// The return Detaiy.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> .
        /// </returns>
        ISet<IMaintainableMutableObject> GetMutableMaintainables(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetaiy);

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
        /// The <see cref="IMetadataStructureDefinitionMutableObject"/> .
        /// </returns>
        IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IMetadataFlowMutableObject"/> .
        /// </returns>
        IMetadataFlowMutableObject GetMutableMetadataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IOrganisationUnitSchemeMutableObject"/> .
        /// </returns>
        IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IProcessMutableObject"/> .
        /// </returns>
        IProcessMutableObject GetMutableProcessObject(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IProcessMutableObject> GetMutableProcessObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        IProvisionAgreementMutableObject GetMutableProvisionAgreement(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementBeans(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IReportingTaxonomyMutableObject"/> .
        /// </returns>
        IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        /// The <see cref="IStructureSetMutableObject"/> .
        /// </returns>
        IStructureSetMutableObject GetMutableStructureSet(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

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
        ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail);

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableObjects.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    #endregion

    /// <summary>
    ///     MutableObjects is a container for MaintainableMutableObjects
    /// </summary>
    public interface IMutableObjects
    {
        // ADD
        #region Public Properties

        /// <summary>
        ///     Gets the agency scheme mutable objects.
        /// </summary>
        ISet<IAgencySchemeMutableObject> AgencySchemeMutableObjects { get; }

        /// <summary>
        ///     Gets all the maintainable objects
        /// </summary>
        /// <value> </value>
        ISet<IMaintainableMutableObject> AllMaintainables { get; }

        /// <summary>
        ///     Gets the categorisations.
        /// </summary>
        ISet<ICategorisationMutableObject> Categorisations { get; }

        /// <summary>
        ///     Gets the category schemes.
        /// </summary>
        ISet<ICategorySchemeMutableObject> CategorySchemes { get; }

        /// <summary>
        ///     Gets the codelists.
        /// </summary>
        ISet<ICodelistMutableObject> Codelists { get; }

        /// <summary>
        ///     Gets the concept schemes.
        /// </summary>
        ISet<IConceptSchemeMutableObject> ConceptSchemes { get; }

        /// <summary>
        ///     Gets the content constraints.
        /// </summary>
        ISet<IContentConstraintMutableObject> ContentConstraints { get; }

        /// <summary>
        ///     Gets the data consumer scheme mutable objects.
        /// </summary>
        ISet<IDataConsumerSchemeMutableObject> DataConsumberSchemeMutableObjects { get; }

        /// <summary>
        ///     Gets the data provider scheme mutable objects.
        /// </summary>
        ISet<IDataProviderSchemeMutableObject> DataProviderSchemeMutableObjects { get; }

        /// <summary>
        ///     Gets the data structures.
        /// </summary>
        ISet<IDataStructureMutableObject> DataStructures { get; }

        /// <summary>
        ///     Gets the dataflows.
        /// </summary>
        ISet<IDataflowMutableObject> Dataflows { get; }

        /// <summary>
        ///     Gets the hierarchical codelists.
        /// </summary>
        ISet<IHierarchicalCodelistMutableObject> HierarchicalCodelists { get; }

        /// <summary>
        ///     Gets an SDMX objects package containing the immutable objects instances of the mutable objects
        ///     contained within this package
        /// </summary>
        /// <value> </value>
        ISdmxObjects ImmutableObjects { get; }

        /// <summary>
        ///     Gets the metadata structures.
        /// </summary>
        ISet<IMetadataStructureDefinitionMutableObject> MetadataStructures { get; }

        /// <summary>
        ///     Gets the metadataflows.
        /// </summary>
        ISet<IMetadataFlowMutableObject> Metadataflows { get; }

        /// <summary>
        ///     Gets the organisation unit schemes.
        /// </summary>
        ISet<IOrganisationUnitSchemeMutableObject> OrganisationUnitSchemes { get; }

        /// <summary>
        ///     Gets the processes.
        /// </summary>
        ISet<IProcessMutableObject> Processes { get; }

        /// <summary>
        ///     Gets the provisions.
        /// </summary>
        ISet<IProvisionAgreementMutableObject> Provisions { get; }

        /// <summary>
        ///     Gets the registrations.
        /// </summary>
        ISet<IRegistrationMutableObject> Registrations { get; }

        /// <summary>
        ///     Gets the reporting taxonomies.
        /// </summary>
        ISet<IReportingTaxonomyMutableObject> ReportingTaxonomys { get; }

        /// <summary>
        ///     Gets the structure sets.
        /// </summary>
        ISet<IStructureSetMutableObject> StructureSets { get; }

        /// <summary>
        ///     Gets the subscriptions.
        /// </summary>
        ISet<ISubscriptionMutableObject> Subscriptions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specified agency scheme.
        /// </summary>
        /// <param name="agencySchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddAgencyScheme(IAgencySchemeMutableObject agencySchemeMutableObject);

        // FUNC - implement AttachmentConstraintMutableObject
        // void addAttachmentConstraint(AttachmentConstraintMutableObject agencySchemeMutableObject);

        /// <summary>
        /// Adds the specified categorisation.
        /// </summary>
        /// <param name="categorisationMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddCategorisation(ICategorisationMutableObject categorisationMutableObject);

        /// <summary>
        /// Adds the specified category scheme.
        /// </summary>
        /// <param name="categorySchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddCategoryScheme(ICategorySchemeMutableObject categorySchemeMutableObject);

        /// <summary>
        /// Adds the specified codelist.
        /// </summary>
        /// <param name="codelistMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddCodelist(ICodelistMutableObject codelistMutableObject);

        /// <summary>
        /// Adds the specified concept scheme.
        /// </summary>
        /// <param name="conceptSchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddConceptScheme(IConceptSchemeMutableObject conceptSchemeMutableObject);

        /// <summary>
        /// Adds the specified content constraint.
        /// </summary>
        /// <param name="contentConstraintMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddContentConstraint(IContentConstraintMutableObject contentConstraintMutableObject);

        /// <summary>
        /// Adds the specified data consumer scheme.
        /// </summary>
        /// <param name="dataConsumerSchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddDataConsumerScheme(IDataConsumerSchemeMutableObject dataConsumerSchemeMutableObject);

        /// <summary>
        /// Adds the specified data provider scheme.
        /// </summary>
        /// <param name="dataProviderSchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddDataProviderScheme(IDataProviderSchemeMutableObject dataProviderSchemeMutableObject);

        /// <summary>
        /// Adds the specified data structure.
        /// </summary>
        /// <param name="dataStructureMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddDataStructure(IDataStructureMutableObject dataStructureMutableObject);

        /// <summary>
        /// Adds the specified dataflow.
        /// </summary>
        /// <param name="dataflowMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddDataflow(IDataflowMutableObject dataflowMutableObject);

        /// <summary>
        /// Adds the specified hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddHierarchicalCodelist(IHierarchicalCodelistMutableObject hierarchicalCodelistMutableObject);

        /// <summary>
        /// Adds the specified identifiable.
        /// </summary>
        /// <param name="identifiableMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddIdentifiable(IIdentifiableMutableObject identifiableMutableObject);

        /// <summary>
        /// Adds an identifiable to the SdmxObjects container.  If the agencySchemeMutableObject is a maintainable
        ///     then it will be added directly into the container.  If the agencySchemeMutableObject is an identifiable that
        ///     has a maintainable ancestor, then the maintainable ancestor will be added to the container,
        ///     and will be retrievable through the respective getter method.
        /// </summary>
        /// <typeparam name="T">The generic type parameter.
        /// </typeparam>
        /// <param name="collection">
        /// The mutable object to add..
        /// </param>
        void AddIdentifiables<T>(ICollection<T> collection) where T : IIdentifiableMutableObject;

        /// <summary>
        /// Adds the specified metadata flow.
        /// </summary>
        /// <param name="metadataFlowMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddMetadataFlow(IMetadataFlowMutableObject metadataFlowMutableObject);

        /// <summary>
        /// Adds the specified metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinitionMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddMetadataStructure(IMetadataStructureDefinitionMutableObject metadataStructureDefinitionMutableObject);

        /// <summary>
        /// Adds the specified organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitSchemeMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddOrganisationUnitScheme(IOrganisationUnitSchemeMutableObject organisationUnitSchemeMutableObject);

        /// <summary>
        /// Adds the specified process.
        /// </summary>
        /// <param name="processMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddProcess(IProcessMutableObject processMutableObject);

        /// <summary>
        /// Adds the specified provision.
        /// </summary>
        /// <param name="provisionAgreementMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddProvision(IProvisionAgreementMutableObject provisionAgreementMutableObject);

        /// <summary>
        /// Adds the specified registration.
        /// </summary>
        /// <param name="registrationMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddRegistration(IRegistrationMutableObject registrationMutableObject);

        /// <summary>
        /// Adds the specified reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomyMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddReportingTaxonomy(IReportingTaxonomyMutableObject reportingTaxonomyMutableObject);

        /// <summary>
        /// Adds the specified structure set.
        /// </summary>
        /// <param name="structureSetMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddStructureSet(IStructureSetMutableObject structureSetMutableObject);

        /// <summary>
        /// Adds the specified subscription.
        /// </summary>
        /// <param name="subscriptionMutableObject">
        /// The mutable object to add..
        /// </param>
        void AddSubscription(ISubscriptionMutableObject subscriptionMutableObject);

        /// <summary>
        /// Gets all the maintainable objects of a given type
        /// </summary>
        /// <param name="structureType">Structure type
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> .
        /// </returns>
        ISet<IMaintainableMutableObject> GetMaintainables(SdmxStructureType structureType);

        // REMOVE

        /// <summary>
        /// Remove the specified agency scheme mutable objects.
        /// </summary>
        /// <param name="agencySchemeMutableObject">
        /// The agency Scheme Mutable Object.
        /// </param>
        void RemoveAgencySchemeMutableObjects(IAgencySchemeMutableObject agencySchemeMutableObject);

        // FUNC - implement AttachmentConstraintMutableObject
        // void removeAttachmentConstraint(AttachmentConstraintMutableObject agencySchemeMutableObject);

        /// <summary>
        /// Remove the specified categorisation.
        /// </summary>
        /// <param name="categorisationMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveCategorisation(ICategorisationMutableObject categorisationMutableObject);

        /// <summary>
        /// Remove the specified category scheme.
        /// </summary>
        /// <param name="categorySchemeMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveCategoryScheme(ICategorySchemeMutableObject categorySchemeMutableObject);

        /// <summary>
        /// Remove the specified codelist.
        /// </summary>
        /// <param name="codelistMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveCodelist(ICodelistMutableObject codelistMutableObject);

        /// <summary>
        /// Remove the specified concept scheme.
        /// </summary>
        /// <param name="conceptSchemeMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveConceptScheme(IConceptSchemeMutableObject conceptSchemeMutableObject);

        /// <summary>
        /// Remove the specified content constraint.
        /// </summary>
        /// <param name="contentConstraintMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveContentConstraint(IContentConstraintMutableObject contentConstraintMutableObject);

        /// <summary>
        /// Remove the specified <paramref name="dataConsumerSchemeMutableObject"/>
        /// </summary>
        /// <param name="dataConsumerSchemeMutableObject">
        /// The <see cref="IDataConsumerSchemeMutableObject"/>.
        /// </param>
        void RemoveDataConsumberSchemeMutableObjects(IDataConsumerSchemeMutableObject dataConsumerSchemeMutableObject);

        /// <summary>
        /// Remove the specified data provider scheme mutable objects.
        /// </summary>
        /// <param name="dataProviderSchemeMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveDataProviderSchemeMutableObjects(IDataProviderSchemeMutableObject dataProviderSchemeMutableObject);

        /// <summary>
        /// Remove the specified data structure.
        /// </summary>
        /// <param name="dataStructureMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveDataStructure(IDataStructureMutableObject dataStructureMutableObject);

        /// <summary>
        /// Remove the specified dataflow.
        /// </summary>
        /// <param name="dataflowMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveDataflow(IDataflowMutableObject dataflowMutableObject);

        /// <summary>
        /// Remove the specified hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveHierarchicalCodelist(IHierarchicalCodelistMutableObject hierarchicalCodelistMutableObject);

        /// <summary>
        /// Remove the specified metadata flow.
        /// </summary>
        /// <param name="metadataFlowMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveMetadataFlow(IMetadataFlowMutableObject metadataFlowMutableObject);

        /// <summary>
        /// Remove the specified metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinitionMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveMetadataStructure(IMetadataStructureDefinitionMutableObject metadataStructureDefinitionMutableObject);

        /// <summary>
        /// Remove the specified organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitSchemeMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveOrganisationUnitScheme(IOrganisationUnitSchemeMutableObject organisationUnitSchemeMutableObject);

        /// <summary>
        /// Remove the specified process.
        /// </summary>
        /// <param name="processMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveProcess(IProcessMutableObject processMutableObject);

        /// <summary>
        /// Remove the specified provision.
        /// </summary>
        /// <param name="provisionAgreementMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveProvision(IProvisionAgreementMutableObject provisionAgreementMutableObject);

        /// <summary>
        /// Remove the specified registration.
        /// </summary>
        /// <param name="registrationMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveRegistration(IRegistrationMutableObject registrationMutableObject);

        /// <summary>
        /// Remove the specified reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomyMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveReportingTaxonomy(IReportingTaxonomyMutableObject reportingTaxonomyMutableObject);

        /// <summary>
        /// Remove the specified structure set.
        /// </summary>
        /// <param name="structureSetMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveStructureSet(IStructureSetMutableObject structureSetMutableObject);

        /// <summary>
        /// Remove the specified subscription.
        /// </summary>
        /// <param name="subscriptionMutableObject">
        /// The mutable object to remove..
        /// </param>
        void RemoveSubscription(ISubscriptionMutableObject subscriptionMutableObject);

        #endregion
    }
}
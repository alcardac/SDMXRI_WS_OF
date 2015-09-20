// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxObjects.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
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

    #endregion

    /// <summary>
    ///     Container for Structure Sdmx Objects, contains methods to add, remove, merge, and retrieve sdmxObjects.  Also contains the means to
    ///     retrieve a mutable instance of the same container (<c>getMutableObjects</c>)
    /// </summary>
    public interface ISdmxObjects
    {
        #region Public Properties

        /// <summary>
        ///     Gets and sets the action for this container
        /// </summary>
        DatasetAction Action { get; set;  }

        /// <summary>
        ///     Gets all the agencies that exist in the sdmxObjects collection
        /// </summary>
        ISet<IAgency> Agencies { get; }

        /// <summary>
        ///     Gets all the agency schemes in this container, returns an empty set if no agencies exist
        /// </summary>
        ISet<IAgencyScheme> AgenciesSchemes { get; }

        /// <summary>
        ///     Gets all the attachment constraints that exist in the sdmxObjects collection
        /// </summary>
        ISet<IAttachmentConstraintObject> AttachmentConstraints { get; }

        /// <summary>
        ///     Gets all the categorisations in this container, returns an empty set if no categorisations exist
        /// </summary>
        ISet<ICategorisationObject> Categorisations { get; }

        /// <summary>
        ///     Gets all the category schemes in this container, returns an empty set if no category schemes exist
        /// </summary>
        ISet<ICategorySchemeObject> CategorySchemes { get; }

        /// <summary>
        ///     Gets all the codelists in this container, returns an empty set if no codelists exist
        /// </summary>
        ISet<ICodelistObject> Codelists { get; }

        /// <summary>
        ///     Gets all the concept schemes in this container, returns an empty set if no concept schemes exist
        /// </summary>
        ISet<IConceptSchemeObject> ConceptSchemes { get; }

        /// <summary>
        ///     Gets all the content constraints that exist in the sdmxObjects collection
        /// </summary>
        ISet<IContentConstraintObject> ContentConstraintObjects { get; }

        /// <summary>
        ///     Gets all the data consumers in this container, returns an empty set if no data consumers exist
        /// </summary>
        ISet<IDataConsumerScheme> DataConsumerSchemes { get; }

        /// <summary>
        ///     Gets all the data provider in this container, returns an empty set if no data providers exist
        /// </summary>
        ISet<IDataProviderScheme> DataProviderSchemes { get; }

        /// <summary>
        ///     Gets all the key families in this container, returns an empty set if no key families exist
        /// </summary>
        ISet<IDataStructureObject> DataStructures { get; }

        /// <summary>
        ///     Gets all the dataflows in this container, returns an empty set if no dataflows exist
        /// </summary>
        ISet<IDataflowObject> Dataflows { get; }

        /// <summary>
        ///     Gets or sets the header on this set of objects
        /// </summary>
        IHeader Header { get; set; }

        /// <summary>
        ///     Gets all the hierarchical codelists in this container, returns an empty set if no hierarchical codelists exist
        /// </summary>
        ISet<IHierarchicalCodelistObject> HierarchicalCodelists { get; }

        /// <summary>
        /// Returns a new, read-only identifier for this objects container
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets all the metadata structures in this container, returns an empty set if no metadata structures exist
        /// </summary>
        ISet<IMetadataStructureDefinitionObject> MetadataStructures { get; }

        /// <summary>
        ///     Gets all the metadataflows in this container, returns an empty set if no metadataflows exist
        /// </summary>
        ISet<IMetadataFlow> Metadataflows { get; }

        /// <summary>
        ///     Gets a MutableObjects package containing all the mutable instances of the sdmxObjects contained within this container
        /// </summary>
        IMutableObjects MutableObjects { get; }

        /// <summary>
        ///     Gets all the organisation unit schemes in this container, returns an empty set if no organisation schemes exist
        /// </summary>
        ISet<IOrganisationUnitSchemeObject> OrganisationUnitSchemes { get; }

        /// <summary>
        ///     Gets all the processes in this container, returns an empty set if no processes exist
        /// </summary>
        ISet<IProcessObject> Processes { get; }

        /// <summary>
        ///     Gets all the provision agreements in this container, returns an empty set if no provision agreements exist
        /// </summary>
        ISet<IProvisionAgreementObject> ProvisionAgreements { get; }

        /// <summary>
        ///     Gets all the registrations in this container, returns an empty set if no registrations exist
        /// </summary>
        ISet<IRegistrationObject> Registrations { get; }

        /// <summary>
        ///     Gets all the reporting taxonomies in this container, returns an empty set if no reporting taxonomies exist
        /// </summary>
        ISet<IReportingTaxonomyObject> ReportingTaxonomys { get; }

        /// <summary>
        ///     Gets a new instance of SdmxObjectsInfo containing information about what is stored in this sdmxObjects container
        /// </summary>
        /// <value> </value>
        ISdmxObjectsInfo SdmxObjectsInfo { get; }

        /// <summary>
        ///     Gets all the structure sets in this container, returns an empty set if no structure sets exist
        /// </summary>
        ISet<IStructureSetObject> StructureSets { get; }

        /// <summary>
        ///     Gets all the subscriptions in this container, returns an empty set if no subscriptions exist
        /// </summary>
        ISet<ISubscriptionObject> Subscriptions { get; }

        /// <summary>
        /// Gets a value indicating whether there are attachment constraints in this container
        /// </summary>
        bool HasAttachmentConstraints { get; }

        /// <summary>
        /// Gets a value indicating whether there are agency schemes in this container
        /// </summary>
        bool HasAgenciesSchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are content constraints in this container
        /// </summary>
        bool HasContentConstraintBeans { get; }

        /// <summary>
        /// Gets a value indicating whether there are organisation unit schemes in this container
        /// </summary>
        bool HasOrganisationUnitSchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are data consumer schemes in this container
        /// </summary>
        bool HasDataConsumerSchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are dataflows in this container
        /// </summary>
        bool HasDataflows { get; }

        /// <summary>
        /// Gets a value indicating whether there are data provider schemes in this container
        /// </summary>
        bool HasDataProviderSchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are metadataflows in this container
        /// </summary>
        bool HasMetadataflows { get; }

        /// <summary>
        /// Gets a value indicating whether there are category schemes in this container
        /// </summary>
        bool HasCategorySchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are codelists in this container
        /// </summary>
        bool HasCodelists { get; }

        /// <summary>
        /// Gets a value indicating whether there are hierarchical codelists in this container
        /// </summary>
        bool HasHierarchicalCodelists { get; }

        /// <summary>
        /// Gets a value indicating whether there are categorisations in this container
        /// </summary>
        bool HasCategorisations { get; }

        /// <summary>
        /// Gets a value indicating whether there are concept schemes in this container
        /// </summary>
        bool HasConceptSchemes { get; }

        /// <summary>
        /// Gets a value indicating whether there are metadata structures in this container
        /// </summary>
        bool HasMetadataStructures { get; }

        /// <summary>
        /// Gets a value indicating whether there are data structures in this container
        /// </summary>
        bool HasDataStructures { get; }

        /// <summary>
        /// Gets a value indicating whether there are reporting taxonomies in this container
        /// </summary>
        bool HasReportingTaxonomys { get; }

        /// <summary>
        /// Gets a value indicating whether there are structure sets in this container
        /// </summary>
        bool HasStructureSets { get; }

        /// <summary>
        /// Gets a value indicating whether there are processes in this container
        /// </summary>
        bool HasProcesses { get; }

        /// <summary>
        /// Gets a value indicating whether there are provision agreements in this container
        /// </summary>
        bool HasProvisionAgreements { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds an agency to this container, if the agency already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agency Scheme.
        /// </param>
        void AddAgencyScheme(IAgencyScheme agencyScheme);

        /// <summary>
        /// Adds a attachment constraint object to this container, if the category scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="attachmentConstraint">
        /// - the attachmentConstraint to add
        /// </param>
        void AddAttachmentConstraint(IAttachmentConstraintObject attachmentConstraint);

        /// <summary>
        /// Adds a categorisation to this container, if the categorisation already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="categorisation">
        /// The sdmx object to add
        /// </param>
        void AddCategorisation(ICategorisationObject categorisation);

        /// <summary>
        /// Adds a category scheme to this container, if the category scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="categoryScheme">
        /// The sdmx object to add
        /// </param>
        void AddCategoryScheme(ICategorySchemeObject categoryScheme);

        /// <summary>
        /// Adds a codelist to this container, if the codelist already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="codelist">
        /// The sdmx object to add
        /// </param>
        void AddCodelist(ICodelistObject codelist);

        /// <summary>
        /// Adds a concept scheme to this container, if the concept scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="conceptScheme">
        /// The sdmx object to add
        /// </param>
        void AddConceptScheme(IConceptSchemeObject conceptScheme);

        /// <summary>
        /// Adds a content constraint to this container, if the category scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="contentConstraint">
        /// The sdmx object to add
        /// </param>
        void AddContentConstraintObject(IContentConstraintObject contentConstraint);

        /// <summary>
        /// Adds a data consumer scheme to this container, if the data consumer scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The sdmx object to add
        /// </param>
        void AddDataConsumerScheme(IDataConsumerScheme dataConsumerScheme);

        /// <summary>
        /// Adds a data provider scheme to this container, if the data provider scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="dataProviderScheme">
        /// The sdmx object to add
        /// </param>
        void AddDataProviderScheme(IDataProviderScheme dataProviderScheme);

        /// <summary>
        /// Adds a Data Structure to this container.
        ///     If the Data Structure already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="dataStructure">
        /// The sdmx object to add
        /// </param>
        void AddDataStructure(IDataStructureObject dataStructure);

        /// <summary>
        /// Adds a dataflow to this container, if the dataflow already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="dataflow">
        /// The sdmx object to add
        /// </param>
        void AddDataflow(IDataflowObject dataflow);

        /// <summary>
        /// Adds a hierarchical codelist to this container, if the hierarchical codelist already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="hierarchicalCodelist">
        /// The sdmx object to add
        /// </param>
        void AddHierarchicalCodelist(IHierarchicalCodelistObject hierarchicalCodelist);

        /// <summary>
        /// Adds an identifiable to the SdmxObjects container.  If the <paramref name="identifiableObject"/> is a maintainable
        ///     then it will be added directly into the container.  If the object is an identifiable that
        ///     has a maintainable ancestor, then the maintainable ancestor will be added to the container,
        ///     and will be retrievable through the respective getter method.
        ///     <p/>
        ///     If the identifiable belongs to an already stored maintainable, then the stored maintainable will be overwritten
        /// </summary>
        /// <param name="identifiableObject">
        /// The agencyScheme.
        /// </param>
        void AddIdentifiable(IIdentifiableObject identifiableObject);

        /// <summary>
        /// Adds many identifiable objects to the SdmxObjects container.
        ///     <p/>
        ///     If the identifiable belongs to an already stored maintainable, then the stored maintainable will be overwritten
        /// </summary>
        /// <typeparam name="T">Generic type parameter.
        /// </typeparam>
        /// <param name="identifiableObjectCollection">
        /// The objects.
        /// </param>
        void AddIdentifiables<T>(ICollection<T> identifiableObjectCollection) where T : IIdentifiableObject;

        /// <summary>
        /// Adds a metadataflow to this container, if the metadataflow already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="metadataFlow">
        /// - the metadataFlow to add
        /// </param>
        void AddMetadataFlow(IMetadataFlow metadataFlow);

        /// <summary>
        /// Adds a metadata structure object to this container, if the metadata structure already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="metadataStructureDefinition">
        /// The sdmx object to add
        /// </param>
        void AddMetadataStructure(IMetadataStructureDefinitionObject metadataStructureDefinition);

        /// <summary>
        /// Adds a organisation unit scheme object to this container, if the organisation unit scheme already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="organisationUnitScheme">
        /// The sdmx object to add
        /// </param>
        void AddOrganisationUnitScheme(IOrganisationUnitSchemeObject organisationUnitScheme);

        /// <summary>
        /// Adds a process object to this container, if the process object already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="process">
        /// The sdmx object to add
        /// </param>
        void AddProcess(IProcessObject process);

        /// <summary>
        /// Adds a provision agreement object to this container, if the provision agreement object already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="provisionAgreement">
        /// The provision Agreement.
        /// </param>
        void AddProvisionAgreement(IProvisionAgreementObject provisionAgreement);

        /// <summary>
        /// Adds a registration object to this container, if the registration object already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        void AddRegistration(IRegistrationObject registration);

        /// <summary>
        /// Adds a reporting taxonomy object to this container, if the reporting taxonomy already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The sdmx object to add
        /// </param>
        void AddReportingTaxonomy(IReportingTaxonomyObject reportingTaxonomy);

        /// <summary>
        /// Adds a structure set object to this container, if the structure set already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="structureSet">
        /// The sdmx object to add
        /// </param>
        void AddStructureSet(IStructureSetObject structureSet);

        /// <summary>
        /// Adds a subscription object to this container, if the subscription object already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        void AddSubscription(ISubscriptionObject subscription);

        /// <summary>
        /// Gets all the agencies that exist in the sdmxObjects collection
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAgency}"/> .
        /// </returns>
        ISet<IAgency> GetAgencies(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets the agency schemes in this container that are maintained by the given agency, returns null if no agencies exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IAgencyScheme"/>; otherwise null if no agencies exist.
        /// </returns>
        IAgencyScheme GetAgenciesScheme(string agencyId);

        /// <summary>
        /// Gets all the agency schemes in this container, returns an empty set if no agencies exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAgencyScheme}"/> .
        /// </returns>
        ISet<IAgencyScheme> GetAgenciesSchemes(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the maintainable objects in this container, returns an empty set if no maintainable objects exist in this container
        /// </summary>
        /// <returns>
        /// set of all maintainable objects, minus any which match the optional exclude parameters
        /// </returns>
        /// <param name="exclude">
        /// do not return the maintainable objects which match the optional exclude parameters
        /// </param>
        ISet<IMaintainableObject> GetAllMaintainables(params SdmxStructureEnumType[] exclude);

        /// <summary>
        /// Gets all the attachment constraints that exist in the sdmxObjects collection and are maintained by the given agency
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAttachmentConstraintObject}"/> .
        /// </returns>
        ISet<IAttachmentConstraintObject> GetAttachmentConstraints(string agencyId);

        /// <summary>
        /// Gets all the attachment constraints that exist in the sdmxObjects collection
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IAttachmentConstraintObject}"/> .
        /// </returns>
        ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the categorisations in this container that are maintained by the given agency, returns an empty set if no categorisations exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationObject}"/> .
        /// </returns>
        ISet<ICategorisationObject> GetCategorisations(string agencyId);

        /// <summary>
        /// Gets all the categorisations in this container, returns an empty set if no categorisations exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationObject}"/> .
        /// </returns>
        ISet<ICategorisationObject> GetCategorisations(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the category schemes in this container that are maintained by the given agency, returns an empty set if no category schemes exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorySchemeObject}"/> .
        /// </returns>
        ISet<ICategorySchemeObject> GetCategorySchemes(string agencyId);

        /// <summary>
        /// Gets all the category schemes in this container, returns an empty set if no category schemes exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorySchemeObject}"/> .
        /// </returns>
        ISet<ICategorySchemeObject> GetCategorySchemes(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the codelists in this container that are maintained by the given agency, returns an empty set if no codelists exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICodelistObject}"/> .
        /// </returns>
        ISet<ICodelistObject> GetCodelists(string agencyId);

        /// <summary>
        /// Gets all the codelists in this container, returns an empty set if no codelists exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICodelistObject}"/> .
        /// </returns>
        ISet<ICodelistObject> GetCodelists(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the concept schemes in this container that are maintained by the given agency, returns an empty set if no concept schemes exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IConceptSchemeObject}"/> .
        /// </returns>
        ISet<IConceptSchemeObject> GetConceptSchemes(string agencyId);

        /// <summary>
        /// Gets all the concept schemes in this container, returns an empty set if no concept schemes exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IConceptSchemeObject}"/> .
        /// </returns>
        ISet<IConceptSchemeObject> GetConceptSchemes(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the content constraints in this container that are maintained by the given agency, returns an empty set if no agencies exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IContentConstraintObject}"/> .
        /// </returns>
        ISet<IContentConstraintObject> GetContentConstraintObjects(string agencyId);

        /// <summary>
        /// Gets all the content constraints that exist in the sdmxObjects collection
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IContentConstraintObject}"/> .
        /// </returns>
        ISet<IContentConstraintObject> GetContentConstraintObjects(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets all the data consumer scheme in this container that is maintained by the given agency, returns null if no data consumer scheme exists
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IDataConsumerScheme"/> .
        /// </returns>
        IDataConsumerScheme GetDataConsumerScheme(string agencyId);

        /// <summary>
        /// Gets all the data consumers in this container, returns an empty set if no data consumers exist
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataConsumerScheme}"/> .
        /// </returns>
        ISet<IDataConsumerScheme> GetDataConsumerSchemes(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Returns the data provider scheme in this container that are maintained by the given agency, returns an empty set if no data provider scheme exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IDataProviderScheme"/> .
        /// </returns>
        IDataProviderScheme GetDataProviderScheme(string agencyId);

        /// <summary>
        /// Gets all the data provider in this container, returns an empty set if no data providers exist
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataProviderScheme}"/> .
        /// </returns>
        ISet<IDataProviderScheme> GetDataProviderSchemes(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets all the key families in this container that are maintained by the given agency, returns an empty set if no key families exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataStructureObject}"/> .
        /// </returns>
        ISet<IDataStructureObject> GetDataStructures(string agencyId);

        /// <summary>
        /// Gets all the key families in this container, returns an empty set if no key families exist
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataStructureObject}"/> .
        /// </returns>
        ISet<IDataStructureObject> GetDataStructures(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets all the dataflows in this container that are maintained by the given agency, returns an empty set if no dataflows exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataflowObject}"/> .
        /// </returns>
        ISet<IDataflowObject> GetDataflows(string agencyId);

        /// <summary>
        /// Gets all the dataflows in this container, returns an empty set if no dataflows exist
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataflowObject}"/> .
        /// </returns>
        ISet<IDataflowObject> GetDataflows(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets all the hierarchical codelists in this container that are maintained by the given agency, returns an empty set if no hierarchical codelists exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IHierarchicalCodelistObject}"/> .
        /// </returns>
        ISet<IHierarchicalCodelistObject> GetHierarchicalCodelists(string agencyId);

        /// <summary>
        /// Gets all the hierarchical codelists in this container, returns an empty set if no hierarchical codelists exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IHierarchicalCodelistObject}"/> .
        /// </returns>
        ISet<IHierarchicalCodelistObject> GetHierarchicalCodelists(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the maintainable objects of a given type
        /// </summary>
        /// <param name="structureType">
        /// filter on this type for returned maintainable objects
        /// </param>
        /// <returns>
        /// set of all maintainable objects of given type
        /// </returns>
        ISet<IMaintainableObject> GetMaintainables(SdmxStructureEnumType structureType);

        /// <summary>
        /// Gets all the metadata structures in this container that are maintained by the given agency, returns an empty set if no metadata structures exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMetadataStructureDefinitionObject}"/> .
        /// </returns>
        ISet<IMetadataStructureDefinitionObject> GetMetadataStructures(string agencyId);

        /// <summary>
        /// Gets all the metadata structures in this container, returns an empty set if no metadata structures exist
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMetadataStructureDefinitionObject}"/> .
        /// </returns>
        ISet<IMetadataStructureDefinitionObject> GetMetadataStructures(IMaintainableRefObject maintainableRefObject);

        /// <summary>
        /// Gets all the metadataflows in this container that are maintained by the given agency, returns an empty set if no metadataflows exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMetadataFlow}"/> .
        /// </returns>
        ISet<IMetadataFlow> GetMetadataflows(string agencyId);

        /// <summary>
        /// Gets all the metadataflows in this container, returns an empty set if no metadataflows exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMetadataFlow}"/> .
        /// </returns>
        ISet<IMetadataFlow> GetMetadataflows(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the organisation unit schemes in this container that are maintained by the given agency, returns an empty set if no organisation schemes exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IOrganisationUnitSchemeObject}"/> .
        /// </returns>
        ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemes(string agencyId);

        /// <summary>
        /// Gets all the organisation unit schemes in this container, returns an empty set if no organisation schemes exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IOrganisationUnitSchemeObject}"/> .
        /// </returns>
        ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemes(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the processes in this container that are maintained by the given agency, returns an empty set if no processes exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProcessObject}"/> .
        /// </returns>
        ISet<IProcessObject> GetProcesses(string agencyId);

        /// <summary>
        /// Gets all the processes in this container, returns an empty set if no processes exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProcessObject}"/> .
        /// </returns>
        ISet<IProcessObject> GetProcesses(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the provision agreements in this container that are maintained by the given agency, returns an empty set if no provision agreements exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProvisionAgreementObject}"/> .
        /// </returns>
        ISet<IProvisionAgreementObject> GetProvisionAgreements(string agencyId);

        /// <summary>
        /// Gets all the provision agreements in this container, returns an empty set if no provision agreements exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProvisionAgreementObject}"/> .
        /// </returns>
        ISet<IProvisionAgreementObject> GetProvisionAgreements(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the registrations in this container that are maintained by the given agency, returns an empty set if no registrations exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IRegistrationObject}"/> .
        /// </returns>
        ISet<IRegistrationObject> GetRegistrations(string agencyId);

        /// <summary>
        /// Gets all the registrations in this container, returns an empty set if no registrations exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IRegistrationObject}"/> .
        /// </returns>
        ISet<IRegistrationObject> GetRegistrations(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the reporting taxonomies in this container that are maintained by the given agency, returns an empty set if no reporting taxonomies exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IReportingTaxonomyObject}"/> .
        /// </returns>
        ISet<IReportingTaxonomyObject> GetReportingTaxonomys(string agencyId);

        /// <summary>
        /// Gets all the reporting taxonomies in this container, returns an empty set if no reporting taxonomies exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IReportingTaxonomyObject}"/> .
        /// </returns>
        ISet<IReportingTaxonomyObject> GetReportingTaxonomys(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the structure sets in this container that are maintained by the given agency, returns an empty set if no structure sets exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IStructureSetObject}"/> .
        /// </returns>
        ISet<IStructureSetObject> GetStructureSets(string agencyId);

        /// <summary>
        /// Gets all the structure sets in this container, returns an empty set if no structure sets exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IStructureSetObject}"/> .
        /// </returns>
        ISet<IStructureSetObject> GetStructureSets(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the subscriptions in this container that are maintained by the given agency, returns an empty set if no subscriptions exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ISubscriptionObject}"/> .
        /// </returns>
        ISet<ISubscriptionObject> GetSubscriptions(string agencyId);

        /// <summary>
        /// Gets all the subscriptions in this container, returns an empty set if no subscriptions exist
        /// </summary>
        /// <param name="xref">
        /// The reference to the maintainable object.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ISubscriptionObject}"/> .
        /// </returns>
        ISet<ISubscriptionObject> GetSubscriptions(IMaintainableRefObject xref);

        /// <summary>
        ///     Gets a value indicating whether the container contains RegistrationObjects
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasRegistrations();

        /// <summary>
        ///     Gets a value indicating whether the container contains any MaintainableObjects which are not registration of subscription sdmxObjects
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasStructures();

        /// <summary>
        ///     Gets a value indicating whether the container contains SubscriptionObjects
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasSubscriptions();

        /// <summary>
        /// Adds all the Objects in the supplied sdmxObjects to this set of sdmxObjects,
        ///     any duplicates found in this set will be overwritten by the provided sdmxObjects.  If the sdmxObjects argument has a header this will overwrite
        ///     and header set on this sdmxObjects.
        /// <p/>
        /// The id of this SdmxBeans container will not be overwritten
        /// </summary>
        /// <param name="sdmxObjects">SDMX objects
        /// </param>
        void Merge(ISdmxObjects sdmxObjects);

        // REMOVE

        /// <summary>
        /// Removes the agency scheme from the container, if it exists
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmx object to remove.
        /// </param>
        void RemoveAgencyScheme(IAgencyScheme agencyScheme);

        /// <summary>
        /// Removes the IAttachmentConstraintObject from the container, if it exists
        /// </summary>
        /// <param name="attachmentConstraint">
        /// The sdmx object to remove..
        /// </param>
        void RemoveAttachmentConstraintObject(IAttachmentConstraintObject attachmentConstraint);

        /// <summary>
        /// Removes the categorisation from the container, if it exists
        /// </summary>
        /// <param name="categorisation">
        /// The sdmx object to remove..
        /// </param>
        void RemoveCategorisation(ICategorisationObject categorisation);

        /// <summary>
        /// Removes the category scheme from the container, if it exists
        /// </summary>
        /// <param name="categoryScheme">
        /// The sdmx object to remove..
        /// </param>
        void RemoveCategoryScheme(ICategorySchemeObject categoryScheme);

        /// <summary>
        /// Removes the codelist from the container, if it exists
        /// </summary>
        /// <param name="codelistObject">
        /// The sdmx object to remove..
        /// </param>
        void RemoveCodelist(ICodelistObject codelistObject);

        /// <summary>
        /// Removes the concept scheme from the container, if it exists
        /// </summary>
        /// <param name="conceptScheme">
        /// The sdmx object to remove..
        /// </param>
        void RemoveConceptScheme(IConceptSchemeObject conceptScheme);

        /// <summary>
        /// Removes the IContentConstraintObject from the container, if it exists
        /// </summary>
        /// <param name="contentConstraint">
        /// The sdmx object to remove..
        /// </param>
        void RemoveContentConstraintObject(IContentConstraintObject contentConstraint);

        /// <summary>
        /// Removes the data consumer scheme from the container, if it exists
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The sdmx object to remove.
        /// </param>
        void RemoveDataConsumerScheme(IDataConsumerScheme dataConsumerScheme);

        /// <summary>
        /// Removes the data provider scheme from the container, if it exists
        /// </summary>
        /// <param name="dataProviderScheme">
        /// The sdmx object to remove.
        /// </param>
        void RemoveDataProviderScheme(IDataProviderScheme dataProviderScheme);

        /// <summary>
        /// Removes the Data Structure from the container, if it exists
        /// </summary>
        /// <param name="dataStructureObject">
        /// The sdmx object to remove..
        /// </param>
        void RemoveDataStructure(IDataStructureObject dataStructureObject);

        /// <summary>
        /// Removes the dataflow from the container, if it exists
        /// </summary>
        /// <param name="dataflowObject">
        /// The sdmx object to remove..
        /// </param>
        void RemoveDataflow(IDataflowObject dataflowObject);

        /// <summary>
        /// Removes the hierarchical codelist from the container, if it exists
        /// </summary>
        /// <param name="hierarchicalCodelistObject">
        /// The sdmx object to remove..
        /// </param>
        void RemoveHierarchicalCodelist(IHierarchicalCodelistObject hierarchicalCodelistObject);

        /// <summary>
        /// Removes the maintainable from the container, if it exists
        /// </summary>
        /// <param name="maintainableObject">
        /// The sdmx object to remove..
        /// </param>
        void RemoveMaintainable(IMaintainableObject maintainableObject);

        /// <summary>
        /// Removes the metadataflow from the container, if it exists
        /// </summary>
        /// <param name="metadataFlow">
        /// The sdmx object to remove.
        /// </param>
        void RemoveMetadataFlow(IMetadataFlow metadataFlow);

        /// <summary>
        /// Removes the metadata structure definition from the container, if it exists
        /// </summary>
        /// <param name="metadataStructureDefinition">
        /// The sdmx object to remove..
        /// </param>
        void RemoveMetadataStructure(IMetadataStructureDefinitionObject metadataStructureDefinition);

        /// <summary>
        /// Removes the organisation unit scheme from the container, if it exists
        /// </summary>
        /// <param name="organisationUnitScheme">
        /// The sdmx object to remove..
        /// </param>
        void RemoveOrganisationUnitScheme(IOrganisationUnitSchemeObject organisationUnitScheme);

        /// <summary>
        /// Removes the process from the container, if it exists
        /// </summary>
        /// <param name="process">
        /// The sdmx object to remove..
        /// </param>
        void RemoveProcess(IProcessObject process);

        /// <summary>
        /// Removes the provision agreement from the container, if it exists
        /// </summary>
        /// <param name="provisionAgreement">
        /// The sdmx object to remove..
        /// </param>
        void RemoveProvisionAgreement(IProvisionAgreementObject provisionAgreement);

        /// <summary>
        /// Adds a registration object to this container, if the registration object already exists in this container then it will be overwritten.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        void RemoveRegistration(IRegistrationObject registration);

        /// <summary>
        /// Removes the reporting taxonomy from the container, if it exists
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The sdmx object to remove..
        /// </param>
        void RemoveReportingTaxonomy(IReportingTaxonomyObject reportingTaxonomy);

        /// <summary>
        /// Removes the structure set from the container, if it exists
        /// </summary>
        /// <param name="structureSet">
        /// The sdmx object to remove..
        /// </param>
        void RemoveStructureSet(IStructureSetObject structureSet);

        /// <summary>
        /// Removes a subscription from this container, if it exists.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        void RemoveSubscription(ISubscriptionObject subscription);

        #endregion
    }
}
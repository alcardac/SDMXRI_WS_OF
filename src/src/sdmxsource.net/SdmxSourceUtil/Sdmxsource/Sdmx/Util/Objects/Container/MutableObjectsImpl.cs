// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MutableObjectsImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Container
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
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
    ///     The mutable objects impl.
    /// </summary>
    public class MutableObjectsImpl : IMutableObjects
    {
        #region Fields

        /// <summary>
        ///     The _agency scheme.
        /// </summary>
        private readonly ISet<IAgencySchemeMutableObject> _agencyScheme;

        // FUNC this needs to be implemented    
        // private Set<AttachmentConstraintMutableBean> attachmentConstraint = new HashSet<AttachmentConstraintMutableBean>();

        /// <summary>
        ///     The _categorisations.
        /// </summary>
        private readonly ISet<ICategorisationMutableObject> _categorisations;

        /// <summary>
        ///     The _category schemes.
        /// </summary>
        private readonly ISet<ICategorySchemeMutableObject> _categorySchemes;

        /// <summary>
        ///     The _codelists.
        /// </summary>
        private readonly ISet<ICodelistMutableObject> _codelists;

        /// <summary>
        ///     The _concept schemes.
        /// </summary>
        private readonly ISet<IConceptSchemeMutableObject> _conceptSchemes;

        /// <summary>
        ///     The _content constraint.
        /// </summary>
        private readonly ISet<IContentConstraintMutableObject> _contentConstraint;

        /// <summary>
        ///     The _data consumer scheme.
        /// </summary>
        private readonly ISet<IDataConsumerSchemeMutableObject> _dataConsumerScheme;

        /// <summary>
        ///     The _data provider scheme.
        /// </summary>
        private readonly ISet<IDataProviderSchemeMutableObject> _dataProviderScheme;

        /// <summary>
        ///     The _data structures.
        /// </summary>
        private readonly ISet<IDataStructureMutableObject> _dataStructures;

        /// <summary>
        ///     The _dataflows.
        /// </summary>
        private readonly ISet<IDataflowMutableObject> _dataflows;

        /// <summary>
        ///     The _hcls.
        /// </summary>
        private readonly ISet<IHierarchicalCodelistMutableObject> _hcls;

        /// <summary>
        ///     The _metadata structures.
        /// </summary>
        private readonly ISet<IMetadataStructureDefinitionMutableObject> _metadataStructures;

        /// <summary>
        ///     The _metadataflows.
        /// </summary>
        private readonly ISet<IMetadataFlowMutableObject> _metadataflows;

        /// <summary>
        ///     The _organisation unit scheme.
        /// </summary>
        private readonly ISet<IOrganisationUnitSchemeMutableObject> _organisationUnitScheme;

        /// <summary>
        ///     The _processes.
        /// </summary>
        private readonly ISet<IProcessMutableObject> _processes;

        /// <summary>
        ///     The _provision agreement.
        /// </summary>
        private readonly ISet<IProvisionAgreementMutableObject> _provisionAgreement;

        /// <summary>
        ///     The _registrations.
        /// </summary>
        private readonly ISet<IRegistrationMutableObject> _registrations;

        /// <summary>
        ///     The _reporting taxonomy.
        /// </summary>
        private readonly ISet<IReportingTaxonomyMutableObject> _reportingTaxonomy;

        /// <summary>
        ///     The _structure set.
        /// </summary>
        private readonly ISet<IStructureSetMutableObject> _structureSet;

        /// <summary>
        ///     The _subscriptions.
        /// </summary>
        private readonly ISet<ISubscriptionMutableObject> _subscriptions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MutableObjectsImpl" /> class.
        /// </summary>
        public MutableObjectsImpl()
        {
            this._agencyScheme = new HashSet<IAgencySchemeMutableObject>();
            this._dataProviderScheme = new HashSet<IDataProviderSchemeMutableObject>();
            this._dataConsumerScheme = new HashSet<IDataConsumerSchemeMutableObject>();
            this._organisationUnitScheme = new HashSet<IOrganisationUnitSchemeMutableObject>();
            this._categorisations = new HashSet<ICategorisationMutableObject>();
            this._categorySchemes = new HashSet<ICategorySchemeMutableObject>();
            this._codelists = new HashSet<ICodelistMutableObject>();
            this._conceptSchemes = new HashSet<IConceptSchemeMutableObject>();
            this._contentConstraint = new HashSet<IContentConstraintMutableObject>();
            this._dataflows = new HashSet<IDataflowMutableObject>();
            this._hcls = new HashSet<IHierarchicalCodelistMutableObject>();
            this._dataStructures = new HashSet<IDataStructureMutableObject>();
            this._metadataflows = new HashSet<IMetadataFlowMutableObject>();
            this._metadataStructures = new HashSet<IMetadataStructureDefinitionMutableObject>();
            this._processes = new HashSet<IProcessMutableObject>();
            this._structureSet = new HashSet<IStructureSetMutableObject>();
            this._reportingTaxonomy = new HashSet<IReportingTaxonomyMutableObject>();
            this._provisionAgreement = new HashSet<IProvisionAgreementMutableObject>();
            this._registrations = new HashSet<IRegistrationMutableObject>();
            this._subscriptions = new HashSet<ISubscriptionMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableObjectsImpl"/> class.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        public MutableObjectsImpl(IEnumerable<IMaintainableMutableObject> maintainables)
        {
            this._agencyScheme = new HashSet<IAgencySchemeMutableObject>();
            this._dataProviderScheme = new HashSet<IDataProviderSchemeMutableObject>();
            this._dataConsumerScheme = new HashSet<IDataConsumerSchemeMutableObject>();
            this._organisationUnitScheme = new HashSet<IOrganisationUnitSchemeMutableObject>();
            this._categorisations = new HashSet<ICategorisationMutableObject>();
            this._categorySchemes = new HashSet<ICategorySchemeMutableObject>();
            this._codelists = new HashSet<ICodelistMutableObject>();
            this._conceptSchemes = new HashSet<IConceptSchemeMutableObject>();
            this._contentConstraint = new HashSet<IContentConstraintMutableObject>();
            this._dataflows = new HashSet<IDataflowMutableObject>();
            this._hcls = new HashSet<IHierarchicalCodelistMutableObject>();
            this._dataStructures = new HashSet<IDataStructureMutableObject>();
            this._metadataflows = new HashSet<IMetadataFlowMutableObject>();
            this._metadataStructures = new HashSet<IMetadataStructureDefinitionMutableObject>();
            this._processes = new HashSet<IProcessMutableObject>();
            this._structureSet = new HashSet<IStructureSetMutableObject>();
            this._reportingTaxonomy = new HashSet<IReportingTaxonomyMutableObject>();
            this._provisionAgreement = new HashSet<IProvisionAgreementMutableObject>();
            this._registrations = new HashSet<IRegistrationMutableObject>();
            this._subscriptions = new HashSet<ISubscriptionMutableObject>();

            if (maintainables != null)
            {
                foreach (IMaintainableMutableObject currentMaintainable in maintainables)
                {
                    this.AddIdentifiable(currentMaintainable);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the agency scheme mutable objects.
        /// </summary>
        public ISet<IAgencySchemeMutableObject> AgencySchemeMutableObjects
        {
            get
            {
                return this._agencyScheme;
            }
        }

        /// <summary>
        ///     Gets the all maintainables.
        /// </summary>
        public ISet<IMaintainableMutableObject> AllMaintainables
        {
            get
            {
                var returnSet = new HashSet<IMaintainableMutableObject>();

                returnSet.UnionWith(this._agencyScheme);

                // FUNC this needs to be implemented        
                // returnSet.addAll(this.attachmentConstraint);
                returnSet.UnionWith(this._dataConsumerScheme);
                returnSet.UnionWith(this._dataProviderScheme);
                returnSet.UnionWith(this._organisationUnitScheme);
                returnSet.UnionWith(this._categorisations);
                returnSet.UnionWith(this._categorySchemes);
                returnSet.UnionWith(this._codelists);
                returnSet.UnionWith(this._conceptSchemes);
                returnSet.UnionWith(this._contentConstraint);
                returnSet.UnionWith(this._dataflows);
                returnSet.UnionWith(this._hcls);
                returnSet.UnionWith(this._dataStructures);
                returnSet.UnionWith(this._metadataflows);
                returnSet.UnionWith(this._metadataStructures);
                returnSet.UnionWith(this._processes);
                returnSet.UnionWith(this._structureSet);
                returnSet.UnionWith(this._reportingTaxonomy);
                returnSet.UnionWith(this._provisionAgreement);
                returnSet.UnionWith(this._registrations);
                returnSet.UnionWith(this._subscriptions);
                return returnSet;
            }
        }

        /// <summary>
        ///     Gets the categorisations.
        /// </summary>
        public ISet<ICategorisationMutableObject> Categorisations
        {
            get
            {
                return this._categorisations;
            }
        }

        /// <summary>
        ///     Gets the category schemes.
        /// </summary>
        public ISet<ICategorySchemeMutableObject> CategorySchemes
        {
            get
            {
                return this._categorySchemes;
            }
        }

        /// <summary>
        ///     Gets the codelists.
        /// </summary>
        public ISet<ICodelistMutableObject> Codelists
        {
            get
            {
                return this._codelists;
            }
        }

        /// <summary>
        ///     Gets the concept schemes.
        /// </summary>
        public ISet<IConceptSchemeMutableObject> ConceptSchemes
        {
            get
            {
                return this._conceptSchemes;
            }
        }

        /// <summary>
        ///     Gets the content constraints.
        /// </summary>
        public ISet<IContentConstraintMutableObject> ContentConstraints
        {
            get
            {
                return this._contentConstraint;
            }
        }

        /// <summary>
        ///     Gets the data consumber scheme mutable objects.
        /// </summary>
        public ISet<IDataConsumerSchemeMutableObject> DataConsumberSchemeMutableObjects
        {
            get
            {
                return this._dataConsumerScheme;
            }
        }

        /// <summary>
        ///     Gets the data provider scheme mutable objects.
        /// </summary>
        public ISet<IDataProviderSchemeMutableObject> DataProviderSchemeMutableObjects
        {
            get
            {
                return this._dataProviderScheme;
            }
        }

        /// <summary>
        ///     Gets the data structures.
        /// </summary>
        public ISet<IDataStructureMutableObject> DataStructures
        {
            get
            {
                return this._dataStructures;
            }
        }

        /// <summary>
        ///     Gets the dataflows.
        /// </summary>
        public ISet<IDataflowMutableObject> Dataflows
        {
            get
            {
                return this._dataflows;
            }
        }

        /// <summary>
        ///     Gets the hierarchical codelists.
        /// </summary>
        public ISet<IHierarchicalCodelistMutableObject> HierarchicalCodelists
        {
            get
            {
                return this._hcls;
            }
        }

        /// <summary>
        ///     Gets the immutable collection.
        /// </summary>
        public ISdmxObjects ImmutableObjects
        {
            get
            {
                ISdmxObjects returnBeans = new SdmxObjectsImpl();
                foreach (IMaintainableMutableObject currentMaint in this.AllMaintainables)
                {
                    returnBeans.AddIdentifiable(currentMaint.ImmutableInstance);
                }

                return returnBeans;
            }
        }

        /// <summary>
        ///     Gets the metadata structures.
        /// </summary>
        public ISet<IMetadataStructureDefinitionMutableObject> MetadataStructures
        {
            get
            {
                return this._metadataStructures;
            }
        }

        /// <summary>
        ///     Gets the metadataflows.
        /// </summary>
        public ISet<IMetadataFlowMutableObject> Metadataflows
        {
            get
            {
                return this._metadataflows;
            }
        }

        /// <summary>
        ///     Gets the organisation unit schemes.
        /// </summary>
        public ISet<IOrganisationUnitSchemeMutableObject> OrganisationUnitSchemes
        {
            get
            {
                return this._organisationUnitScheme;
            }
        }

        /// <summary>
        ///     Gets the processes.
        /// </summary>
        public ISet<IProcessMutableObject> Processes
        {
            get
            {
                return this._processes;
            }
        }

        /// <summary>
        ///     Gets the provisions.
        /// </summary>
        public ISet<IProvisionAgreementMutableObject> Provisions
        {
            get
            {
                return this._provisionAgreement;
            }
        }

        /// <summary>
        ///     Gets the registrations.
        /// </summary>
        public ISet<IRegistrationMutableObject> Registrations
        {
            get
            {
                return this._registrations;
            }
        }

        /// <summary>
        ///     Gets the reporting taxonomys.
        /// </summary>
        public ISet<IReportingTaxonomyMutableObject> ReportingTaxonomys
        {
            get
            {
                return this._reportingTaxonomy;
            }
        }

        /// <summary>
        ///     Gets the structure sets.
        /// </summary>
        public ISet<IStructureSetMutableObject> StructureSets
        {
            get
            {
                return this._structureSet;
            }
        }

        /// <summary>
        ///     Gets the subscriptions.
        /// </summary>
        public ISet<ISubscriptionMutableObject> Subscriptions
        {
            get
            {
                return this._subscriptions;
            }
        }

        #endregion

        // ADDERS
        #region Public Methods and Operators

        /// <summary>
        /// The add agency scheme.
        /// </summary>
        /// <param name="agencySchemeMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddAgencyScheme(IAgencySchemeMutableObject agencySchemeMutableObject)
        {
            if (agencySchemeMutableObject != null)
            {
                this._agencyScheme.Remove(agencySchemeMutableObject);
                this._agencyScheme.Add(agencySchemeMutableObject);
            }
        }

        // FUNC - implement AttachmentConstraintMutableBean
        /*
        public void addAttachmentConstraint(AttachmentConstraintMutableBean agencySchemeMutableObject) {
            if(agencySchemeMutableObject != null) {
                this.attachmentConstraint.remove(agencySchemeMutableObject);
                this.attachmentConstraint.add(agencySchemeMutableObject);
            }
        }
        */

        /// <summary>
        /// The add categorisation.
        /// </summary>
        /// <param name="categorisationMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddCategorisation(ICategorisationMutableObject categorisationMutableObject)
        {
            if (categorisationMutableObject != null)
            {
                this._categorisations.Remove(categorisationMutableObject);
                this._categorisations.Add(categorisationMutableObject);
            }
        }

        /// <summary>
        /// The add category scheme.
        /// </summary>
        /// <param name="categorySchemeMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddCategoryScheme(ICategorySchemeMutableObject categorySchemeMutableObject)
        {
            if (categorySchemeMutableObject != null)
            {
                this._categorySchemes.Remove(categorySchemeMutableObject);
                this._categorySchemes.Add(categorySchemeMutableObject);
            }
        }

        /// <summary>
        /// The add codelist.
        /// </summary>
        /// <param name="codelistMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddCodelist(ICodelistMutableObject codelistMutableObject)
        {
            if (codelistMutableObject != null)
            {
                this._codelists.Remove(codelistMutableObject);
                this._codelists.Add(codelistMutableObject);
            }
        }

        /// <summary>
        /// The add concept scheme.
        /// </summary>
        /// <param name="conceptSchemeMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddConceptScheme(IConceptSchemeMutableObject conceptSchemeMutableObject)
        {
            if (conceptSchemeMutableObject != null)
            {
                this._conceptSchemes.Remove(conceptSchemeMutableObject);
                this._conceptSchemes.Add(conceptSchemeMutableObject);
            }
        }

        /// <summary>
        /// The add content constraint.
        /// </summary>
        /// <param name="contentConstraintMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddContentConstraint(IContentConstraintMutableObject contentConstraintMutableObject)
        {
            if (contentConstraintMutableObject != null)
            {
                this._contentConstraint.Remove(contentConstraintMutableObject);
                this._contentConstraint.Add(contentConstraintMutableObject);
            }
        }

        /// <summary>
        /// The add data consumer scheme.
        /// </summary>
        /// <param name="dataConsumerSchemeMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddDataConsumerScheme(IDataConsumerSchemeMutableObject dataConsumerSchemeMutableObject)
        {
            if (dataConsumerSchemeMutableObject != null)
            {
                this._dataConsumerScheme.Remove(dataConsumerSchemeMutableObject);
                this._dataConsumerScheme.Add(dataConsumerSchemeMutableObject);
            }
        }

        /// <summary>
        /// The add data provider scheme.
        /// </summary>
        /// <param name="dataProviderSchemeMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddDataProviderScheme(IDataProviderSchemeMutableObject dataProviderSchemeMutableObject)
        {
            if (dataProviderSchemeMutableObject != null)
            {
                this._dataProviderScheme.Remove(dataProviderSchemeMutableObject);
                this._dataProviderScheme.Add(dataProviderSchemeMutableObject);
            }
        }

        /// <summary>
        /// The add data structure.
        /// </summary>
        /// <param name="dataStructureMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddDataStructure(IDataStructureMutableObject dataStructureMutableObject)
        {
            if (dataStructureMutableObject != null)
            {
                this._dataStructures.Remove(dataStructureMutableObject);
                this._dataStructures.Add(dataStructureMutableObject);
            }
        }

        /// <summary>
        /// The add dataflow.
        /// </summary>
        /// <param name="dataflowMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddDataflow(IDataflowMutableObject dataflowMutableObject)
        {
            if (dataflowMutableObject != null)
            {
                this._dataflows.Remove(dataflowMutableObject);
                this._dataflows.Add(dataflowMutableObject);
            }
        }

        /// <summary>
        /// The add hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddHierarchicalCodelist(IHierarchicalCodelistMutableObject hierarchicalCodelistMutableObject)
        {
            if (hierarchicalCodelistMutableObject != null)
            {
                this._hcls.Remove(hierarchicalCodelistMutableObject);
                this._hcls.Add(hierarchicalCodelistMutableObject);
            }
        }

        /// <summary>
        /// The add identifiable.
        /// </summary>
        /// <param name="identifiableMutableObject">
        /// The agencySchemeMutableObject.
        /// </param>
        public void AddIdentifiable(IIdentifiableMutableObject identifiableMutableObject)
        {
            if (identifiableMutableObject == null)
            {
                return;
            }

            var agencySchemeMutableObject = identifiableMutableObject as IAgencySchemeMutableObject;
            if (agencySchemeMutableObject != null)
            {
                this.AddAgencyScheme(agencySchemeMutableObject);
                return;
            }

            var dataProviderSchemeMutableObject = identifiableMutableObject as IDataProviderSchemeMutableObject;
            if (dataProviderSchemeMutableObject != null)
            {
                this.AddDataProviderScheme(dataProviderSchemeMutableObject);
                return;
            }

            var dataConsumerSchemeMutableObject = identifiableMutableObject as IDataConsumerSchemeMutableObject;
            if (dataConsumerSchemeMutableObject != null)
            {
                this.AddDataConsumerScheme(dataConsumerSchemeMutableObject);
                return;
            }

            var organisationUnitSchemeMutableObject = identifiableMutableObject as IOrganisationUnitSchemeMutableObject;
            if (organisationUnitSchemeMutableObject != null)
            {
                this.AddOrganisationUnitScheme(organisationUnitSchemeMutableObject);
                return;
            }

            var categorisationMutableObject = identifiableMutableObject as ICategorisationMutableObject;
            if (categorisationMutableObject != null)
            {
                this.AddCategorisation(categorisationMutableObject);
                return;
            }

            var categorySchemeMutableObject = identifiableMutableObject as ICategorySchemeMutableObject;
            if (categorySchemeMutableObject != null)
            {
                this.AddCategoryScheme(categorySchemeMutableObject);
                return;
            }

            var codelistMutableObject = identifiableMutableObject as ICodelistMutableObject;
            if (codelistMutableObject != null)
            {
                this.AddCodelist(codelistMutableObject);
                return;
            }

            var conceptSchemeMutableObject = identifiableMutableObject as IConceptSchemeMutableObject;
            if (conceptSchemeMutableObject != null)
            {
                this.AddConceptScheme(conceptSchemeMutableObject);
                return;
            }

            var contentConstraintMutableObject = identifiableMutableObject as IContentConstraintMutableObject;
            if (contentConstraintMutableObject != null)
            {
                this.AddContentConstraint(contentConstraintMutableObject);
                return;
            }

            var dataflowMutableObject = identifiableMutableObject as IDataflowMutableObject;
            if (dataflowMutableObject != null)
            {
                this.AddDataflow(dataflowMutableObject);
                return;
            }

            var hierarchicalCodelistMutableObject = identifiableMutableObject as IHierarchicalCodelistMutableObject;
            if (hierarchicalCodelistMutableObject != null)
            {
                this.AddHierarchicalCodelist(hierarchicalCodelistMutableObject);
                return;
            }

            var dataStructureMutableObject = identifiableMutableObject as IDataStructureMutableObject;
            if (dataStructureMutableObject != null)
            {
                this.AddDataStructure(dataStructureMutableObject);
                return;
            }

            var metadataFlowMutableObject = identifiableMutableObject as IMetadataFlowMutableObject;
            if (metadataFlowMutableObject != null)
            {
                this.AddMetadataFlow(metadataFlowMutableObject);
                return;
            }

            var metadataStructureDefinitionMutableObject = identifiableMutableObject as IMetadataStructureDefinitionMutableObject;
            if (metadataStructureDefinitionMutableObject != null)
            {
                this.AddMetadataStructure(metadataStructureDefinitionMutableObject);
                return;
            }

            var processMutableObject = identifiableMutableObject as IProcessMutableObject;
            if (processMutableObject != null)
            {
                this.AddProcess(processMutableObject);
                return;
            }

            var subscription = identifiableMutableObject as ISubscriptionMutableObject;
            if (subscription != null)
            {
                this.AddSubscription(subscription);
                return;
            }

            var reportingTaxonomyMutableObject = identifiableMutableObject as IReportingTaxonomyMutableObject;
            if (reportingTaxonomyMutableObject != null)
            {
                this.AddReportingTaxonomy(reportingTaxonomyMutableObject);
                return;
            }

            var structureSetMutableObject = identifiableMutableObject as IStructureSetMutableObject;
            if (structureSetMutableObject != null)
            {
                this.AddStructureSet(structureSetMutableObject);
                return;
            }

            var registrationMutableObject = identifiableMutableObject as IRegistrationMutableObject;
            if (registrationMutableObject != null)
            {
                this.AddRegistration(registrationMutableObject);
            }

            var provisionAgreementMutableObject = identifiableMutableObject as IProvisionAgreementMutableObject;
            if (provisionAgreementMutableObject != null)
            {
                this.AddProvision(provisionAgreementMutableObject);
            }
        }

        /// <summary>
        /// The add identifiables.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        public void AddIdentifiables<T>(ICollection<T> collection) where T : IIdentifiableMutableObject
        {
            foreach (T identifiable in collection)
            {
                this.AddIdentifiable(identifiable);
            }
        }

        /// <summary>
        /// The add metadata flow.
        /// </summary>
        /// <param name="metadataFlowMutableObject">
        /// The metadata flow mutable object.
        /// </param>
        public void AddMetadataFlow(IMetadataFlowMutableObject metadataFlowMutableObject)
        {
            if (metadataFlowMutableObject != null)
            {
                this._metadataflows.Remove(metadataFlowMutableObject);
                this._metadataflows.Add(metadataFlowMutableObject);
            }
        }

        /// <summary>
        /// The add metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinitionMutableObject">
        /// The metadata structure definition mutable object.
        /// </param>
        public void AddMetadataStructure(IMetadataStructureDefinitionMutableObject metadataStructureDefinitionMutableObject)
        {
            if (metadataStructureDefinitionMutableObject != null)
            {
                this._metadataStructures.Remove(metadataStructureDefinitionMutableObject);
                this._metadataStructures.Add(metadataStructureDefinitionMutableObject);
            }
        }

        /// <summary>
        /// The add organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitSchemeMutableObject">
        /// The organisation unit scheme mutable object.
        /// </param>
        public void AddOrganisationUnitScheme(IOrganisationUnitSchemeMutableObject organisationUnitSchemeMutableObject)
        {
            if (organisationUnitSchemeMutableObject != null)
            {
                this._organisationUnitScheme.Remove(organisationUnitSchemeMutableObject);
                this._organisationUnitScheme.Add(organisationUnitSchemeMutableObject);
            }
        }

        /// <summary>
        /// The add process.
        /// </summary>
        /// <param name="processMutableObject">
        /// The process mutable object.
        /// </param>
        public void AddProcess(IProcessMutableObject processMutableObject)
        {
            if (processMutableObject != null)
            {
                this._processes.Remove(processMutableObject);
                this._processes.Add(processMutableObject);
            }
        }

        /// <summary>
        /// The add provision.
        /// </summary>
        /// <param name="provisionAgreementMutableObject">
        /// The provision agreement mutable object.
        /// </param>
        public void AddProvision(IProvisionAgreementMutableObject provisionAgreementMutableObject)
        {
            if (provisionAgreementMutableObject != null)
            {
                this._provisionAgreement.Remove(provisionAgreementMutableObject);
                this._provisionAgreement.Add(provisionAgreementMutableObject);
            }
        }

        /// <summary>
        /// The add registration.
        /// </summary>
        /// <param name="registrationMutableObject">
        /// The registration mutable object.
        /// </param>
        public void AddRegistration(IRegistrationMutableObject registrationMutableObject)
        {
            if (registrationMutableObject != null)
            {
                this._registrations.Remove(registrationMutableObject);
                this._registrations.Add(registrationMutableObject);
            }
        }

        /// <summary>
        /// The add reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomyMutableObject">
        /// The reporting taxonomy mutable object.
        /// </param>
        public void AddReportingTaxonomy(IReportingTaxonomyMutableObject reportingTaxonomyMutableObject)
        {
            if (reportingTaxonomyMutableObject != null)
            {
                this._reportingTaxonomy.Remove(reportingTaxonomyMutableObject);
                this._reportingTaxonomy.Add(reportingTaxonomyMutableObject);
            }
        }

        /// <summary>
        /// The add structure set.
        /// </summary>
        /// <param name="structureSetMutableObject">
        /// The structure set mutable object.
        /// </param>
        public void AddStructureSet(IStructureSetMutableObject structureSetMutableObject)
        {
            if (structureSetMutableObject != null)
            {
                this._structureSet.Remove(structureSetMutableObject);
                this._structureSet.Add(structureSetMutableObject);
            }
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="subscriptionMutableObject">
        /// The subscription mutable object.
        /// </param>
        public void AddSubscription(ISubscriptionMutableObject subscriptionMutableObject)
        {
            if (subscriptionMutableObject != null)
            {
                this._subscriptions.Remove(subscriptionMutableObject);
                this._subscriptions.Add(subscriptionMutableObject);
            }
        }

        /// <summary>
        /// Gets all the maintainables of a given type
        /// </summary>
        /// <param name="structureType">Structure type
        /// </param>
        /// <returns>
        /// The <see cref="ISet{T}"/> .
        /// </returns>
        public ISet<IMaintainableMutableObject> GetMaintainables(SdmxStructureType structureType)
        {
            SdmxStructureEnumType sdmxStructureEnumType = structureType.EnumType;
            switch (sdmxStructureEnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return new HashSet<IMaintainableMutableObject>(this._agencyScheme);
                case SdmxStructureEnumType.DataConsumerScheme:
                    return new HashSet<IMaintainableMutableObject>(this._dataConsumerScheme);
                case SdmxStructureEnumType.DataProviderScheme:
                    return new HashSet<IMaintainableMutableObject>(this._dataProviderScheme);
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return new HashSet<IMaintainableMutableObject>(this._organisationUnitScheme);
                case SdmxStructureEnumType.Categorisation:
                    return new HashSet<IMaintainableMutableObject>(this._categorisations);
                case SdmxStructureEnumType.CategoryScheme:
                    return new HashSet<IMaintainableMutableObject>(this._categorySchemes);
                case SdmxStructureEnumType.CodeList:
                    return new HashSet<IMaintainableMutableObject>(this._codelists);
                case SdmxStructureEnumType.ConceptScheme:
                    return new HashSet<IMaintainableMutableObject>(this._conceptSchemes);
                case SdmxStructureEnumType.ContentConstraint:
                    return new HashSet<IMaintainableMutableObject>(this._contentConstraint);
                case SdmxStructureEnumType.Dataflow:
                    return new HashSet<IMaintainableMutableObject>(this._dataflows);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return new HashSet<IMaintainableMutableObject>(this._hcls);
                case SdmxStructureEnumType.Dsd:
                    return new HashSet<IMaintainableMutableObject>(this._dataStructures);
                case SdmxStructureEnumType.MetadataFlow:
                    return new HashSet<IMaintainableMutableObject>(this._metadataflows);
                case SdmxStructureEnumType.Msd:
                    return new HashSet<IMaintainableMutableObject>(this._metadataStructures);
                case SdmxStructureEnumType.Process:
                    return new HashSet<IMaintainableMutableObject>(this._processes);
                case SdmxStructureEnumType.StructureSet:
                    return new HashSet<IMaintainableMutableObject>(this._structureSet);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return new HashSet<IMaintainableMutableObject>(this._reportingTaxonomy);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return new HashSet<IMaintainableMutableObject>(this._provisionAgreement);
                case SdmxStructureEnumType.Registration:
                    return new HashSet<IMaintainableMutableObject>(this._registrations);
                case SdmxStructureEnumType.Subscription:
                    return new HashSet<IMaintainableMutableObject>(this._subscriptions);
                default:
                    throw new ArgumentException("Can not return structure type:" + sdmxStructureEnumType, "structureType");
            }
        }

        // REMOVERS

        /// <summary>
        /// The remove agency scheme mutable objects.
        /// </summary>
        /// <param name="agencySchemeMutableObject">
        /// The agency scheme mutable object.
        /// </param>
        public void RemoveAgencySchemeMutableObjects(IAgencySchemeMutableObject agencySchemeMutableObject)
        {
            this._agencyScheme.Remove(agencySchemeMutableObject);
        }

        /// <summary>
        /// The remove categorisation.
        /// </summary>
        /// <param name="categorisationMutableObject">
        /// The categorisation mutable object.
        /// </param>
        public void RemoveCategorisation(ICategorisationMutableObject categorisationMutableObject)
        {
            this._categorisations.Remove(categorisationMutableObject);
        }

        /// <summary>
        /// The remove category scheme.
        /// </summary>
        /// <param name="categorySchemeMutableObject">
        /// The category scheme mutable object.
        /// </param>
        public void RemoveCategoryScheme(ICategorySchemeMutableObject categorySchemeMutableObject)
        {
            this._categorySchemes.Remove(categorySchemeMutableObject);
        }

        /// <summary>
        /// The remove codelist.
        /// </summary>
        /// <param name="codelistMutableObject">
        /// The codelist mutable object.
        /// </param>
        public void RemoveCodelist(ICodelistMutableObject codelistMutableObject)
        {
            this._codelists.Remove(codelistMutableObject);
        }

        /// <summary>
        /// The remove concept scheme.
        /// </summary>
        /// <param name="conceptSchemeMutableObject">
        /// The concept scheme mutable object.
        /// </param>
        public void RemoveConceptScheme(IConceptSchemeMutableObject conceptSchemeMutableObject)
        {
            this._conceptSchemes.Remove(conceptSchemeMutableObject);
        }

        /// <summary>
        /// The remove content constraint.
        /// </summary>
        /// <param name="contentConstraintMutableObject">
        /// The content constraint mutable object.
        /// </param>
        public void RemoveContentConstraint(IContentConstraintMutableObject contentConstraintMutableObject)
        {
            this._contentConstraint.Remove(contentConstraintMutableObject);
        }

        /// <summary>
        /// The remove data consumber scheme mutable objects.
        /// </summary>
        /// <param name="dataConsumerSchemeMutableObject">
        /// The data consumer scheme mutable object.
        /// </param>
        public void RemoveDataConsumberSchemeMutableObjects(IDataConsumerSchemeMutableObject dataConsumerSchemeMutableObject)
        {
            this._dataConsumerScheme.Remove(dataConsumerSchemeMutableObject);
        }

        /// <summary>
        /// The remove data provider scheme mutable objects.
        /// </summary>
        /// <param name="dataProviderSchemeMutableObject">
        /// The data provider scheme mutable object.
        /// </param>
        public void RemoveDataProviderSchemeMutableObjects(IDataProviderSchemeMutableObject dataProviderSchemeMutableObject)
        {
            this._dataProviderScheme.Remove(dataProviderSchemeMutableObject);
        }

        /// <summary>
        /// The remove data structure.
        /// </summary>
        /// <param name="dataStructureMutableObject">
        /// The data structure mutable object.
        /// </param>
        public void RemoveDataStructure(IDataStructureMutableObject dataStructureMutableObject)
        {
            this._dataStructures.Remove(dataStructureMutableObject);
        }

        /// <summary>
        /// The remove dataflow.
        /// </summary>
        /// <param name="dataflowMutableObject">
        /// The dataflow mutable object.
        /// </param>
        public void RemoveDataflow(IDataflowMutableObject dataflowMutableObject)
        {
            this._dataflows.Remove(dataflowMutableObject);
        }

        /// <summary>
        /// The remove hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistMutableObject">
        /// The hierarchical codelist mutable object.
        /// </param>
        public void RemoveHierarchicalCodelist(IHierarchicalCodelistMutableObject hierarchicalCodelistMutableObject)
        {
            this._hcls.Remove(hierarchicalCodelistMutableObject);
        }

        /// <summary>
        /// The remove metadata flow.
        /// </summary>
        /// <param name="metadataFlowMutableObject">
        /// The metadata flow mutable object.
        /// </param>
        public void RemoveMetadataFlow(IMetadataFlowMutableObject metadataFlowMutableObject)
        {
            this._metadataflows.Remove(metadataFlowMutableObject);
        }

        /// <summary>
        /// The remove metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinitionMutableObject">
        /// The metadata structure definition mutable object.
        /// </param>
        public void RemoveMetadataStructure(IMetadataStructureDefinitionMutableObject metadataStructureDefinitionMutableObject)
        {
            this._metadataStructures.Remove(metadataStructureDefinitionMutableObject);
        }

        /// <summary>
        /// The remove organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitSchemeMutableObject">
        /// The organisation unit scheme mutable object.
        /// </param>
        public void RemoveOrganisationUnitScheme(IOrganisationUnitSchemeMutableObject organisationUnitSchemeMutableObject)
        {
            this._organisationUnitScheme.Remove(organisationUnitSchemeMutableObject);
        }

        /// <summary>
        /// The remove process.
        /// </summary>
        /// <param name="processMutableObject">
        /// The process mutable object.
        /// </param>
        public void RemoveProcess(IProcessMutableObject processMutableObject)
        {
            this._processes.Remove(processMutableObject);
        }

        /// <summary>
        /// The remove provision.
        /// </summary>
        /// <param name="provisionAgreementMutableObject">
        /// The provision agreement mutable object.
        /// </param>
        public void RemoveProvision(IProvisionAgreementMutableObject provisionAgreementMutableObject)
        {
            this._provisionAgreement.Remove(provisionAgreementMutableObject);
        }

        /// <summary>
        /// The remove registration.
        /// </summary>
        /// <param name="registrationMutableObject">
        /// The registration mutable object.
        /// </param>
        public void RemoveRegistration(IRegistrationMutableObject registrationMutableObject)
        {
            this._registrations.Remove(registrationMutableObject);
        }

        /// <summary>
        /// The remove reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomyMutableObject">
        /// The reporting taxonomy mutable object.
        /// </param>
        public void RemoveReportingTaxonomy(IReportingTaxonomyMutableObject reportingTaxonomyMutableObject)
        {
            this._reportingTaxonomy.Remove(reportingTaxonomyMutableObject);
        }

        /// <summary>
        /// The remove structure set.
        /// </summary>
        /// <param name="structureSetMutableObject">
        /// The structure set mutable object.
        /// </param>
        public void RemoveStructureSet(IStructureSetMutableObject structureSetMutableObject)
        {
            this._structureSet.Remove(structureSetMutableObject);
        }

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="subscriptionMutableObject">
        /// The subscription mutable object.
        /// </param>
        public void RemoveSubscription(ISubscriptionMutableObject subscriptionMutableObject)
        {
            this._subscriptions.Remove(subscriptionMutableObject);
        }

        #endregion
    }
}
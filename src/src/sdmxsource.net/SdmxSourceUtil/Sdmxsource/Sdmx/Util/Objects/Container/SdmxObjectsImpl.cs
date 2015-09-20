// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsImpl.cs" company="Eurostat">
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
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
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
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Sort;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    #endregion


    /// <summary>
    ///     The sdmx objects impl.
    /// </summary>
    [Serializable]
    public class SdmxObjectsImpl : ISdmxObjects
    {
        #region Fields

        /// <summary>
        ///     The _agency schemes.
        /// </summary>
        private readonly ISet<IAgencyScheme> _agencySchemes;

        /// <summary>
        ///     The _attachment constraints.
        /// </summary>
        private readonly ISet<IAttachmentConstraintObject> _attachmentConstraints;

        /// <summary>
        ///     The _categorisation.
        /// </summary>
        private readonly ISet<ICategorisationObject> _categorisation;

        /// <summary>
        ///     The _category schemes.
        /// </summary>
        private readonly ISet<ICategorySchemeObject> _categorySchemes;

        /// <summary>
        ///     The _codelists.
        /// </summary>
        private readonly ISet<ICodelistObject> _codelists;

        /// <summary>
        ///     The _concept schemes.
        /// </summary>
        private readonly ISet<IConceptSchemeObject> _conceptSchemes;

        /// <summary>
        ///     The _content constraints.
        /// </summary>
        private readonly ISet<IContentConstraintObject> _contentConstraints;

        /// <summary>
        ///     The _data consumer schemes.
        /// </summary>
        private readonly ISet<IDataConsumerScheme> _dataConsumerSchemes;

        /// <summary>
        ///     The _data provider schemes.
        /// </summary>
        private readonly ISet<IDataProviderScheme> _dataProviderSchemes;

        /// <summary>
        ///     The _data structures.
        /// </summary>
        private readonly ISet<IDataStructureObject> _dataStructures;

        /// <summary>
        ///     The _dataflows.
        /// </summary>
        private readonly ISet<IDataflowObject> _dataflows;

        /// <summary>
        ///     The _hcls.
        /// </summary>
        private readonly ISet<IHierarchicalCodelistObject> _hcls;

        /// <summary>
        ///     The _metadata structures.
        /// </summary>
        private readonly ISet<IMetadataStructureDefinitionObject> _metadataStructures;

        /// <summary>
        ///     The _metadataflows.
        /// </summary>
        private readonly ISet<IMetadataFlow> _metadataflows;

        /// <summary>
        ///     The _organisation unit schemes.
        /// </summary>
        private readonly ISet<IOrganisationUnitSchemeObject> _organisationUnitSchemes;

        /// <summary>
        ///     The _processes.
        /// </summary>
        private readonly ISet<IProcessObject> _processes;

        /// <summary>
        ///     The _provision agreement.
        /// </summary>
        private readonly ISet<IProvisionAgreementObject> _provisionAgreement;

        /// <summary>
        ///     The _registrations.
        /// </summary>
        private readonly ISet<IRegistrationObject> _registrations;

        /// <summary>
        ///     The _reporting taxonomy.
        /// </summary>
        private readonly ISet<IReportingTaxonomyObject> _reportingTaxonomy;

        /// <summary>
        ///     The _structure set.
        /// </summary>
        private readonly ISet<IStructureSetObject> _structureSet;

        /// <summary>
        ///     The _subscriptions.
        /// </summary>
        private readonly ISet<ISubscriptionObject> _subscriptions;

        /// <summary>
        ///     The _action.
        /// </summary>
        private DatasetAction _action; // If Header is not available

        /// <summary>
        ///     The _header.
        /// </summary>
        private IHeader _header;

        /// <summary>
        /// The _id
        /// </summary>
        private readonly string _id = Guid.NewGuid().ToString();

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////CONSTRUCTORS                           //////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SdmxObjectsImpl" /> class.
        /// </summary>
        public SdmxObjectsImpl()
        {
            this._action = DatasetAction.GetFromEnum(DatasetActionEnumType.Information);
            this._agencySchemes = new HashSet<IAgencyScheme>();
            this._organisationUnitSchemes = new HashSet<IOrganisationUnitSchemeObject>();
            this._dataProviderSchemes = new HashSet<IDataProviderScheme>();
            this._dataConsumerSchemes = new HashSet<IDataConsumerScheme>();
            this._attachmentConstraints = new HashSet<IAttachmentConstraintObject>();
            this._contentConstraints = new HashSet<IContentConstraintObject>();
            this._categorySchemes = new HashSet<ICategorySchemeObject>();
            this._codelists = new HashSet<ICodelistObject>();
            this._conceptSchemes = new HashSet<IConceptSchemeObject>();
            this._dataflows = new HashSet<IDataflowObject>();
            this._hcls = new HashSet<IHierarchicalCodelistObject>();
            this._dataStructures = new HashSet<IDataStructureObject>();
            this._metadataflows = new HashSet<IMetadataFlow>();
            this._metadataStructures = new HashSet<IMetadataStructureDefinitionObject>();
            this._processes = new HashSet<IProcessObject>();
            this._structureSet = new HashSet<IStructureSetObject>();
            this._reportingTaxonomy = new HashSet<IReportingTaxonomyObject>();
            this._categorisation = new HashSet<ICategorisationObject>();
            this._provisionAgreement = new HashSet<IProvisionAgreementObject>();
            this._registrations = new HashSet<IRegistrationObject>();
            this._subscriptions = new HashSet<ISubscriptionObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsImpl"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public SdmxObjectsImpl(DatasetAction action)
            : this()
        {
            this._action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsImpl"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        public SdmxObjectsImpl(IHeader header)
            : this()
        {
            this._header = header;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="action"></param>
        public SdmxObjectsImpl(IHeader header, DatasetAction action)
            : this()
        {
            this._header = header;
            this._action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsImpl"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        public SdmxObjectsImpl(IHeader header, IEnumerable<IMaintainableObject> maintainables)
            : this()
        {
            this._header = header;
            if (maintainables != null)
            {
                foreach (IMaintainableObject currentMaintainable in maintainables)
                {
                    this.AddIdentifiable(currentMaintainable);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsImpl"/> class.
        /// </summary>
        /// <param name="beans">
        /// The identifiableObjectCollection.
        /// </param>
        public SdmxObjectsImpl(params ISdmxObjects[] beans)
            : this()
        {
            foreach (ISdmxObjects currentBean in beans)
            {
                this.Merge(currentBean);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsImpl"/> class.
        /// </summary>
        /// <param name="maintainableObjects">The maintainable objects.</param>
        public SdmxObjectsImpl(params IMaintainableObject[] maintainableObjects)
            : this()
        {
            if (maintainableObjects != null)
            {
                foreach (IMaintainableObject currentMaint in maintainableObjects)
                {
                    this.AddIdentifiable(currentMaint);
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////METHODS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///     Gets and sets the action.
        /// </summary>
        public DatasetAction Action
        {
            get
            {
                return this._action;
            }

            set
            {
                this._action = value;
            }
        }

        /// <summary>
        ///     Gets the agencies.
        /// </summary>
        public ISet<IAgency> Agencies
        {
            get
            {
                var agencySet = new HashSet<IAgency>();
                foreach (IAgencyScheme currentAgencyScheme in this._agencySchemes)
                {
                    agencySet.AddAll(currentAgencyScheme.Items);
                }

                return agencySet;
            }
        }

        /// <summary>
        ///     Gets the agencies schemes.
        /// </summary>
        public ISet<IAgencyScheme> AgenciesSchemes
        {
            get
            {
                return new RetrunSetCreator<IAgencyScheme>().CreateReturnSet(this._agencySchemes);
            }
        }

        /// <summary>
        ///     Gets the all maintainables.
        /// </summary>
        public ISet<IMaintainableObject> AllMaintainables
        {
            get
            {
                ISet<IMaintainableObject> returnSet =
                    new SortedSet<IMaintainableObject>(new MaintainableSortByIdentifiers<IMaintainableObject>());

                foreach (SdmxStructureType currentType in SdmxStructureType.Values)
                {
                    if (currentType.IsMaintainable)
                    {
                        returnSet.AddAll(this.GetMaintainables(currentType.EnumType));
                    }
                }

                /*  $$$ foreach (SdmxStructureType currentType in SdmxStructureType.Values(SdmxStructureType[])Enum.GetValues(typeof(SdmxStructureType))) {
                    if (currentType.IsMaintainable) {
                        returnSet.AddAll(GetMaintainables(currentType));
                    }
                }*/
                return returnSet;
            }
        }

        /// <summary>
        ///     Gets the attachment constraints.
        /// </summary>
        public ISet<IAttachmentConstraintObject> AttachmentConstraints
        {
            get
            {
                return new RetrunSetCreator<IAttachmentConstraintObject>().CreateReturnSet(this._attachmentConstraints);
            }
        }

        /// <summary>
        ///     Gets the categorisations.
        /// </summary>
        public ISet<ICategorisationObject> Categorisations
        {
            get
            {
                return new RetrunSetCreator<ICategorisationObject>().CreateReturnSet(this._categorisation);
            }
        }

        /// <summary>
        ///     Gets the category schemes.
        /// </summary>
        public ISet<ICategorySchemeObject> CategorySchemes
        {
            get
            {
                return new RetrunSetCreator<ICategorySchemeObject>().CreateReturnSet(this._categorySchemes);
            }
        }

        /// <summary>
        ///     Gets the codelists.
        /// </summary>
        public ISet<ICodelistObject> Codelists
        {
            get
            {
                return new RetrunSetCreator<ICodelistObject>().CreateReturnSet(this._codelists);
            }
        }

        /// <summary>
        ///     Gets the concept schemes.
        /// </summary>
        public ISet<IConceptSchemeObject> ConceptSchemes
        {
            get
            {
                return new RetrunSetCreator<IConceptSchemeObject>().CreateReturnSet(this._conceptSchemes);
            }
        }

        /// <summary>
        ///     Gets the content constraint objects.
        /// </summary>
        public ISet<IContentConstraintObject> ContentConstraintObjects
        {
            get
            {
                return new RetrunSetCreator<IContentConstraintObject>().CreateReturnSet(this._contentConstraints);
            }
        }

        /// <summary>
        ///     Gets the data consumer schemes.
        /// </summary>
        public ISet<IDataConsumerScheme> DataConsumerSchemes
        {
            get
            {
                return new RetrunSetCreator<IDataConsumerScheme>().CreateReturnSet(this._dataConsumerSchemes);
            }
        }

        /// <summary>
        ///     Gets the data provider schemes.
        /// </summary>
        public ISet<IDataProviderScheme> DataProviderSchemes
        {
            get
            {
                return new RetrunSetCreator<IDataProviderScheme>().CreateReturnSet(this._dataProviderSchemes);
            }
        }

        /// <summary>
        ///     Gets the data structures.
        /// </summary>
        public ISet<IDataStructureObject> DataStructures
        {
            get
            {
                return new RetrunSetCreator<IDataStructureObject>().CreateReturnSet(this._dataStructures);
            }
        }

        /// <summary>
        ///     Gets the dataflows.
        /// </summary>
        public ISet<IDataflowObject> Dataflows
        {
            get
            {
                return new RetrunSetCreator<IDataflowObject>().CreateReturnSet(this._dataflows);
            }
        }

        /// <summary>
        ///     Gets or sets the header.
        /// </summary>
        public IHeader Header
        {
            get
            {
                return this._header;
            }

            set
            {
                this._header = value;
            }
        }

        /// <summary>
        ///     Gets the hierarchical codelists.
        /// </summary>
        public ISet<IHierarchicalCodelistObject> HierarchicalCodelists
        {
            get
            {
                return new RetrunSetCreator<IHierarchicalCodelistObject>().CreateReturnSet(this._hcls);
            }
        }

        /// <summary>
        /// Get the Id
        /// </summary>
        public string Id
        {
            get
            {
                return this._id;
            }
        }

        /// <summary>
        ///     Gets the metadata structures.
        /// </summary>
        public ISet<IMetadataStructureDefinitionObject> MetadataStructures
        {
            get
            {
                return
                    new RetrunSetCreator<IMetadataStructureDefinitionObject>().CreateReturnSet(this._metadataStructures);
            }
        }

        /// <summary>
        ///     Gets the metadataflows.
        /// </summary>
        public ISet<IMetadataFlow> Metadataflows
        {
            get
            {
                return new RetrunSetCreator<IMetadataFlow>().CreateReturnSet(this._metadataflows);
            }
        }

        /// <summary>
        ///     Gets the mutable objects.
        /// </summary>
        public IMutableObjects MutableObjects
        {
            get
            {
                IMutableObjects returnBeans = new MutableObjectsImpl();
                foreach (IMaintainableObject currentMaint in this.AllMaintainables)
                {
                    returnBeans.AddIdentifiable(currentMaint.MutableInstance);
                }

                return returnBeans;
            }
        }

        /// <summary>
        ///     Gets the organisation unit schemes.
        /// </summary>
        public ISet<IOrganisationUnitSchemeObject> OrganisationUnitSchemes
        {
            get
            {
                return
                    new RetrunSetCreator<IOrganisationUnitSchemeObject>().CreateReturnSet(this._organisationUnitSchemes);
            }
        }

        /// <summary>
        ///     Gets the processes.
        /// </summary>
        public ISet<IProcessObject> Processes
        {
            get
            {
                return new RetrunSetCreator<IProcessObject>().CreateReturnSet(this._processes);
            }
        }

        /// <summary>
        ///     Gets the provision agreements.
        /// </summary>
        public ISet<IProvisionAgreementObject> ProvisionAgreements
        {
            get
            {
                return new RetrunSetCreator<IProvisionAgreementObject>().CreateReturnSet(this._provisionAgreement);
            }
        }

        /// <summary>
        ///     Gets the registrations.
        /// </summary>
        public ISet<IRegistrationObject> Registrations
        {
            get
            {
                return new RetrunSetCreator<IRegistrationObject>().CreateReturnSet(this._registrations);
            }
        }

        /// <summary>
        ///     Gets the reporting taxonomys.
        /// </summary>
        public ISet<IReportingTaxonomyObject> ReportingTaxonomys
        {
            get
            {
                return new RetrunSetCreator<IReportingTaxonomyObject>().CreateReturnSet(this._reportingTaxonomy);
            }
        }

        /// <summary>
        ///     Gets the sdmx objects info.
        /// </summary>
        public ISdmxObjectsInfo SdmxObjectsInfo
        {
            get
            {
                var info = new SdmxObjectsInfoImpl();
                IList<IAgencyMetadata> agencyMetadataList = new List<IAgencyMetadata>();
                IList<string> allAgencies = new List<string>();

                foreach (IMaintainableObject currentMaint in this.AllMaintainables)
                {
                    allAgencies.Add(currentMaint.AgencyId);
                }

                foreach (string currentAgencyId in allAgencies)
                {
                    IAgencyMetadata agencyMetadata = new AgencyMetadataImpl(currentAgencyId, this);
                    agencyMetadataList.Add(agencyMetadata);
                }

                info.AgencyMetadata = agencyMetadataList;
                return info;
            }
        }

        /// <summary>
        ///     Gets the structure sets.
        /// </summary>
        public ISet<IStructureSetObject> StructureSets
        {
            get
            {
                return new RetrunSetCreator<IStructureSetObject>().CreateReturnSet(this._structureSet);
            }
        }

        /// <summary>
        ///     Gets the subscriptions.
        /// </summary>
        public ISet<ISubscriptionObject> Subscriptions
        {
            get
            {
                return new RetrunSetCreator<ISubscriptionObject>().CreateReturnSet(this._subscriptions);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add agency scheme.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public void AddAgencyScheme(IAgencyScheme obj)
        {
            if (obj != null)
            {
                this._agencySchemes.Remove(obj);
                this._agencySchemes.Add(obj);
            }
        }

        /// <summary>
        /// The add attachment constraint.
        /// </summary>
        /// <param name="attachmentConstraint">
        /// The agencyScheme.
        /// </param>
        public void AddAttachmentConstraint(IAttachmentConstraintObject attachmentConstraint)
        {
            if (attachmentConstraint != null)
            {
                this._attachmentConstraints.Remove(attachmentConstraint);
                this._attachmentConstraints.Add(attachmentConstraint);
            }
        }

        /// <summary>
        /// The add categorisation.
        /// </summary>
        /// <param name="categorisation">
        /// The agencyScheme.
        /// </param>
        public void AddCategorisation(ICategorisationObject categorisation)
        {
            if (categorisation != null)
            {
                this._categorisation.Remove(categorisation);
                this._categorisation.Add(categorisation);
            }
        }

        /// <summary>
        /// The add category scheme.
        /// </summary>
        /// <param name="categoryScheme">
        /// The agencyScheme.
        /// </param>
        public void AddCategoryScheme(ICategorySchemeObject categoryScheme)
        {
            if (categoryScheme != null)
            {
                this._categorySchemes.Remove(categoryScheme);
                this._categorySchemes.Add(categoryScheme);
            }
        }

        /// <summary>
        /// The add codelist.
        /// </summary>
        /// <param name="codelist">
        /// The agencyScheme.
        /// </param>
        public void AddCodelist(ICodelistObject codelist)
        {
            if (codelist != null)
            {
                this._codelists.Remove(codelist);
                this._codelists.Add(codelist);
            }
        }

        /// <summary>
        /// The add concept scheme.
        /// </summary>
        /// <param name="conceptScheme">
        /// The agencyScheme.
        /// </param>
        public void AddConceptScheme(IConceptSchemeObject conceptScheme)
        {
            if (conceptScheme != null)
            {
                this._conceptSchemes.Remove(conceptScheme);
                this._conceptSchemes.Add(conceptScheme);
            }
        }

        /// <summary>
        /// The add content constraint contentConstraint.
        /// </summary>
        /// <param name="contentConstraint">
        /// The agencyScheme.
        /// </param>
        public void AddContentConstraintObject(IContentConstraintObject contentConstraint)
        {
            if (contentConstraint != null)
            {
                this._contentConstraints.Remove(contentConstraint);
                this._contentConstraints.Add(contentConstraint);
            }
        }

        /// <summary>
        /// The add data consumer scheme.
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The agencyScheme.
        /// </param>
        public void AddDataConsumerScheme(IDataConsumerScheme dataConsumerScheme)
        {
            if (dataConsumerScheme != null)
            {
                this._dataConsumerSchemes.Remove(dataConsumerScheme);
                this._dataConsumerSchemes.Add(dataConsumerScheme);
            }
        }

        /// <summary>
        /// The add data provider scheme.
        /// </summary>
        /// <param name="dataProviderScheme">
        /// The agencyScheme.
        /// </param>
        public void AddDataProviderScheme(IDataProviderScheme dataProviderScheme)
        {
            if (dataProviderScheme != null)
            {
                this._dataProviderSchemes.Remove(dataProviderScheme);
                this._dataProviderSchemes.Add(dataProviderScheme);
            }
        }

        /// <summary>
        /// The add data structure.
        /// </summary>
        /// <param name="dataStructure">
        /// The agencyScheme.
        /// </param>
        public void AddDataStructure(IDataStructureObject dataStructure)
        {
            if (dataStructure != null)
            {
                this._dataStructures.Remove(dataStructure);
                this._dataStructures.Add(dataStructure);
            }
        }

        /// <summary>
        /// The add dataflow.
        /// </summary>
        /// <param name="dataflow">
        /// The agencyScheme.
        /// </param>
        public void AddDataflow(IDataflowObject dataflow)
        {
            if (dataflow != null)
            {
                this._dataflows.Remove(dataflow);
                this._dataflows.Add(dataflow);
            }
        }

        /// <summary>
        /// The add hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelist">
        /// The agencyScheme.
        /// </param>
        public void AddHierarchicalCodelist(IHierarchicalCodelistObject hierarchicalCodelist)
        {
            if (hierarchicalCodelist != null)
            {
                this._hcls.Remove(hierarchicalCodelist);
                this._hcls.Add(hierarchicalCodelist);
            }
        }

        /// <summary>
        /// The add identifiable.
        /// </summary>
        /// <param name="identifiableObject">
        /// The agencyScheme.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException
        /// </exception>
        public void AddIdentifiable(IIdentifiableObject identifiableObject)
        {
            if (identifiableObject == null)
            {
                return;
            }

            var agencyScheme = identifiableObject as IAgencyScheme;
            if (agencyScheme != null)
            {
                this.AddAgencyScheme(agencyScheme);
                return;
            }

            var attachmentConstraintObject = identifiableObject as IAttachmentConstraintObject;
            if (attachmentConstraintObject != null)
            {
                this.AddAttachmentConstraint(attachmentConstraintObject);
                return;
            }

            var contentConstraintObject = identifiableObject as IContentConstraintObject;
            if (contentConstraintObject != null)
            {
                this.AddContentConstraintObject(contentConstraintObject);
                return;
            }

            var dataConsumerScheme = identifiableObject as IDataConsumerScheme;
            if (dataConsumerScheme != null)
            {
                this.AddDataConsumerScheme(dataConsumerScheme);
                return;
            }

            var dataProviderScheme = identifiableObject as IDataProviderScheme;
            if (dataProviderScheme != null)
            {
                this.AddDataProviderScheme(dataProviderScheme);
                return;
            }

            var categorySchemeObject = identifiableObject as ICategorySchemeObject;
            if (categorySchemeObject != null)
            {
                this.AddCategoryScheme(categorySchemeObject);
                return;
            }

            var codelistObject = identifiableObject as ICodelistObject;
            if (codelistObject != null)
            {
                this.AddCodelist(codelistObject);
                return;
            }

            var conceptSchemeObject = identifiableObject as IConceptSchemeObject;
            if (conceptSchemeObject != null)
            {
                this.AddConceptScheme(conceptSchemeObject);
                return;
            }

            var dataflowObject = identifiableObject as IDataflowObject;
            if (dataflowObject != null)
            {
                this.AddDataflow(dataflowObject);
                return;
            }

            var hierarchicalCodelistObject = identifiableObject as IHierarchicalCodelistObject;
            if (hierarchicalCodelistObject != null)
            {
                this.AddHierarchicalCodelist(hierarchicalCodelistObject);
                return;
            }

            var dataStructureObject = identifiableObject as IDataStructureObject;
            if (dataStructureObject != null)
            {
                this.AddDataStructure(dataStructureObject);
                return;
            }

            var metadataFlow = identifiableObject as IMetadataFlow;
            if (metadataFlow != null)
            {
                this.AddMetadataFlow(metadataFlow);
                return;
            }

            var metadataStructureDefinitionObject = identifiableObject as IMetadataStructureDefinitionObject;
            if (metadataStructureDefinitionObject != null)
            {
                this.AddMetadataStructure(metadataStructureDefinitionObject);
                return;
            }

            var organisationUnitSchemeObject = identifiableObject as IOrganisationUnitSchemeObject;
            if (organisationUnitSchemeObject != null)
            {
                this.AddOrganisationUnitScheme(organisationUnitSchemeObject);
                return;
            }

            var structureSetObject = identifiableObject as IStructureSetObject;
            if (structureSetObject != null)
            {
                this.AddStructureSet(structureSetObject);
                return;
            }

            var processObject = identifiableObject as IProcessObject;
            if (processObject != null)
            {
                this.AddProcess(processObject);
                return;
            }

            var reportingTaxonomyObject = identifiableObject as IReportingTaxonomyObject;
            if (reportingTaxonomyObject != null)
            {
                this.AddReportingTaxonomy(reportingTaxonomyObject);
                return;
            }

            var categorisationObject = identifiableObject as ICategorisationObject;
            if (categorisationObject != null)
            {
                this.AddCategorisation(categorisationObject);
                return;
            }

            var provisionAgreementObject = identifiableObject as IProvisionAgreementObject;
            if (provisionAgreementObject != null)
            {
                this.AddProvisionAgreement(provisionAgreementObject);
                return;
            }

            var registrationObject = identifiableObject as IRegistrationObject;
            if (registrationObject != null)
            {
                this.AddRegistration(registrationObject);
                return;
            }

            var subscriptionObject = identifiableObject as ISubscriptionObject;
            if (subscriptionObject != null)
            {
                this.AddSubscription(subscriptionObject);
                return;
            }

            if (identifiableObject.IdentifiableParent != null)
            {
                this.AddIdentifiable(identifiableObject.IdentifiableParent);
            }
            else
            {
                throw new ArgumentException("Could not add obj " + identifiableObject.Urn + " to SdmxObjects Container");
            }
        }

        /// <summary>
        /// The add identifiables.
        /// </summary>
        /// <param name="identifiableObjectCollection">
        /// The identifiableObjectCollection.
        /// </param>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        public void AddIdentifiables<T>(ICollection<T> identifiableObjectCollection) where T : IIdentifiableObject
        {
            foreach (T identifiable in identifiableObjectCollection)
            {
                this.AddIdentifiable(identifiable);
            }
        }

        /// <summary>
        /// The add metadata flow.
        /// </summary>
        /// <param name="metadataFlow">
        /// The agencyScheme.
        /// </param>
        public void AddMetadataFlow(IMetadataFlow metadataFlow)
        {
            if (metadataFlow != null)
            {
                this._metadataflows.Remove(metadataFlow);
                this._metadataflows.Add(metadataFlow);
            }
        }

        /// <summary>
        /// The add metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinition">
        /// The agencyScheme.
        /// </param>
        public void AddMetadataStructure(IMetadataStructureDefinitionObject metadataStructureDefinition)
        {
            if (metadataStructureDefinition != null)
            {
                this._metadataStructures.Remove(metadataStructureDefinition);
                this._metadataStructures.Add(metadataStructureDefinition);
            }
        }

        /// <summary>
        /// The add organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitScheme">
        /// The agencyScheme.
        /// </param>
        public void AddOrganisationUnitScheme(IOrganisationUnitSchemeObject organisationUnitScheme)
        {
            if (organisationUnitScheme != null)
            {
                this._organisationUnitSchemes.Remove(organisationUnitScheme);
                this._organisationUnitSchemes.Add(organisationUnitScheme);
            }
        }

        /// <summary>
        /// The add process.
        /// </summary>
        /// <param name="process">
        /// The agencyScheme.
        /// </param>
        public void AddProcess(IProcessObject process)
        {
            if (process != null)
            {
                this._processes.Remove(process);
                this._processes.Add(process);
            }
        }

        /// <summary>
        /// The add provision agreement.
        /// </summary>
        /// <param name="bean">
        /// The agencyScheme.
        /// </param>
        public void AddProvisionAgreement(IProvisionAgreementObject bean)
        {
            if (bean != null)
            {
                this._provisionAgreement.Remove(bean);
                this._provisionAgreement.Add(bean);
            }
        }

        /// <summary>
        /// The add registration.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        public void AddRegistration(IRegistrationObject registration)
        {
            if (registration != null)
            {
                this._registrations.Remove(registration);
                this._registrations.Add(registration);
            }
        }

        /// <summary>
        /// The add reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The agencyScheme.
        /// </param>
        public void AddReportingTaxonomy(IReportingTaxonomyObject reportingTaxonomy)
        {
            if (reportingTaxonomy != null)
            {
                this._reportingTaxonomy.Remove(reportingTaxonomy);
                this._reportingTaxonomy.Add(reportingTaxonomy);
            }
        }

        /// <summary>
        /// The add structure set.
        /// </summary>
        /// <param name="structureSet">
        /// The agencyScheme.
        /// </param>
        public void AddStructureSet(IStructureSetObject structureSet)
        {
            if (structureSet != null)
            {
                this._structureSet.Remove(structureSet);
                this._structureSet.Add(structureSet);
            }
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        public void AddSubscription(ISubscriptionObject subscription)
        {
            if (subscription != null)
            {
                this._subscriptions.Remove(subscription);
                this._subscriptions.Add(subscription);
            }
        }

        /// <summary>
        /// Gets the agencies.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IAgency> GetAgencies(IMaintainableRefObject maintainableRefObject)
        {
            return new HashSet<IAgency>(this._agencySchemes.SelectMany(scheme => scheme.Items));
        }

        /// <summary>
        /// Gets the agencies schemes.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IAgencyScheme> GetAgenciesSchemes(IMaintainableRefObject xref)
        {
            return new Filter<IAgencyScheme>(SdmxStructureEnumType.AgencyScheme).FilterSet(this._agencySchemes, xref);
        }

        /// <summary>
        /// Gets the all maintainables.
        /// </summary>
        /// <param name="exclude">
        /// The exclude.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMaintainableObject> GetAllMaintainables(params SdmxStructureEnumType[] exclude)
        {
            ISet<IMaintainableObject> returnSet =
                new SortedSet<IMaintainableObject>(new MaintainableSortByIdentifiers<IMaintainableObject>());
            ISet<SdmxStructureEnumType> enumValues =
                new HashSet<SdmxStructureEnumType>(exclude ?? new SdmxStructureEnumType[0]);

            foreach (var structureType in SdmxStructureType.MaintainableStructureTypes)
            {
                if (!enumValues.Contains(structureType.EnumType)
                    && structureType.IsMaintainable
                    && structureType.EnumType != SdmxStructureEnumType.MetadataSet)
                {
                    returnSet.AddAll(this.GetMaintainables(structureType.EnumType));
                }
            }

            return returnSet;
        }

        /// <summary>
        /// Gets the attachment constraints.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IAttachmentConstraintObject> GetAttachmentConstraints(string agencyId)
        {
            return new AgencyFilter<IAttachmentConstraintObject>().FilterSet(agencyId, this._attachmentConstraints);
        }

        /// <summary>
        /// Gets the attachment constraints.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IAttachmentConstraintObject> GetAttachmentConstraints(IMaintainableRefObject xref)
        {
            return
                new Filter<IAttachmentConstraintObject>(SdmxStructureEnumType.AttachmentConstraint).FilterSet(
                    this._attachmentConstraints, xref);
        }

        /// <summary>
        /// Gets the categorisations.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICategorisationObject> GetCategorisations(string agencyId)
        {
            return new AgencyFilter<ICategorisationObject>().FilterSet(agencyId, this._categorisation);
        }

        /// <summary>
        /// Gets the categorisations.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICategorisationObject> GetCategorisations(IMaintainableRefObject xref)
        {
            return
                new Filter<ICategorisationObject>(SdmxStructureEnumType.Categorisation).FilterSet(
                    this._categorisation, xref);
        }

        /// <summary>
        /// Gets the category schemes.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICategorySchemeObject> GetCategorySchemes(string agencyId)
        {
            return new AgencyFilter<ICategorySchemeObject>().FilterSet(agencyId, this._categorySchemes);
        }

        /// <summary>
        /// Gets the category schemes.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICategorySchemeObject> GetCategorySchemes(IMaintainableRefObject xref)
        {
            return
                new Filter<ICategorySchemeObject>(SdmxStructureEnumType.CategoryScheme).FilterSet(
                    this._categorySchemes, xref);
        }

        /// <summary>
        /// Gets the codelists.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICodelistObject> GetCodelists(string agencyId)
        {
            return new AgencyFilter<ICodelistObject>().FilterSet(agencyId, this._codelists);
        }

        /// <summary>
        /// Gets the codelists.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ICodelistObject> GetCodelists(IMaintainableRefObject xref)
        {
            return new Filter<ICodelistObject>(SdmxStructureEnumType.CodeList).FilterSet(this._codelists, xref);
        }

        /// <summary>
        /// Gets the concept schemes.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IConceptSchemeObject> GetConceptSchemes(string agencyId)
        {
            return new AgencyFilter<IConceptSchemeObject>().FilterSet(agencyId, this._conceptSchemes);
        }

        /// <summary>
        /// Gets the concept schemes.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IConceptSchemeObject> GetConceptSchemes(IMaintainableRefObject xref)
        {
            return new Filter<IConceptSchemeObject>(SdmxStructureEnumType.ConceptScheme).FilterSet(
                this._conceptSchemes, xref);
        }

        /// <summary>
        /// Gets the content constraint objects.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IContentConstraintObject> GetContentConstraintObjects(string agencyId)
        {
            return new AgencyFilter<IContentConstraintObject>().FilterSet(agencyId, this._contentConstraints);
        }

        /// <summary>
        /// Gets the content constraint objects.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IContentConstraintObject> GetContentConstraintObjects(IMaintainableRefObject maintainableRefObject)
        {
            return
                new Filter<IContentConstraintObject>(SdmxStructureEnumType.ContentConstraint).FilterSet(
                    this._contentConstraints, maintainableRefObject);
        }

        /// <summary>
        /// Gets the data consumer schemes.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataConsumerScheme> GetDataConsumerSchemes(IMaintainableRefObject maintainableRefObject)
        {
            return
                new Filter<IDataConsumerScheme>(SdmxStructureEnumType.DataConsumerScheme).FilterSet(
                    this._dataConsumerSchemes, maintainableRefObject);
        }

        /// <summary>
        /// Gets the data provider schemes.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataProviderScheme> GetDataProviderSchemes(IMaintainableRefObject maintainableRefObject)
        {
            return
                new Filter<IDataProviderScheme>(SdmxStructureEnumType.DataProviderScheme).FilterSet(
                    this._dataProviderSchemes, maintainableRefObject);
        }

        /// <summary>
        /// Gets the data structures.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataStructureObject> GetDataStructures(string agencyId)
        {
            return new AgencyFilter<IDataStructureObject>().FilterSet(agencyId, this._dataStructures);
        }

        /// <summary>
        /// Gets the data structures.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataStructureObject> GetDataStructures(IMaintainableRefObject maintainableRefObject)
        {
            return new Filter<IDataStructureObject>(SdmxStructureEnumType.Dsd).FilterSet(this._dataStructures, maintainableRefObject);
        }

        /// <summary>
        /// Gets the dataflows.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataflowObject> GetDataflows(string agencyId)
        {
            return new AgencyFilter<IDataflowObject>().FilterSet(agencyId, this._dataflows);
        }

        /// <summary>
        /// Gets the dataflows.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IDataflowObject> GetDataflows(IMaintainableRefObject maintainableRefObject)
        {
            return new Filter<IDataflowObject>(SdmxStructureEnumType.Dataflow).FilterSet(this._dataflows, maintainableRefObject);
        }

        /// <summary>
        /// Gets the hierarchical codelists.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IHierarchicalCodelistObject> GetHierarchicalCodelists(string agencyId)
        {
            return new AgencyFilter<IHierarchicalCodelistObject>().FilterSet(agencyId, this._hcls);
        }

        /// <summary>
        /// Gets the hierarchical codelists.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IHierarchicalCodelistObject> GetHierarchicalCodelists(IMaintainableRefObject xref)
        {
            return
                new Filter<IHierarchicalCodelistObject>(SdmxStructureEnumType.HierarchicalCodelist).FilterSet(
                    this._hcls, xref);
        }

        /// <summary>
        /// Gets the maintainables.
        /// </summary>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMaintainableObject> GetMaintainables(IAgency agency)
        {
            ISet<IMaintainableObject> returnSet =
                new SortedSet<IMaintainableObject>(new MaintainableSortByIdentifiers<IMaintainableObject>());
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._agencySchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._dataProviderSchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._dataConsumerSchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._categorySchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._codelists);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._conceptSchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._dataflows);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._hcls);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._dataStructures);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._metadataflows);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._metadataStructures);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._organisationUnitSchemes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._processes);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._structureSet);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._reportingTaxonomy);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._categorisation);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._attachmentConstraints);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._contentConstraints);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._registrations);
            AddAgencyMaintainedStructuresToSet(returnSet, agency, this._subscriptions);
            return returnSet;
        }

        /// <summary>
        /// Gets the maintainables.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// <paramref name="structureType"/> is not supported
        /// </exception>
        public ISet<IMaintainableObject> GetMaintainables(SdmxStructureEnumType structureType)
        {
            ISet<IMaintainableObject> returnSet =
                new SortedSet<IMaintainableObject>(new MaintainableSortByIdentifiers<IMaintainableObject>());
            switch (structureType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    {
                        returnSet.AddAll(this._agencySchemes);
                        break;
                    }

                case SdmxStructureEnumType.DataProviderScheme:
                    {
                        returnSet.AddAll(this._dataProviderSchemes);
                        break;
                    }

                case SdmxStructureEnumType.DataConsumerScheme:
                    {
                        returnSet.AddAll(this._dataConsumerSchemes);
                        break;
                    }

                case SdmxStructureEnumType.CategoryScheme:
                    {
                        returnSet.AddAll(this._categorySchemes);
                        break;
                    }

                case SdmxStructureEnumType.CodeList:
                    {
                        returnSet.AddAll(this._codelists);
                        break;
                    }

                case SdmxStructureEnumType.ConceptScheme:
                    {
                        returnSet.AddAll(this._conceptSchemes);
                        break;
                    }

                case SdmxStructureEnumType.Dataflow:
                    {
                        returnSet.AddAll(this._dataflows);
                        break;
                    }

                case SdmxStructureEnumType.HierarchicalCodelist:
                    {
                        returnSet.AddAll(this._hcls);
                        break;
                    }

                case SdmxStructureEnumType.Dsd:
                    {
                        returnSet.AddAll(this._dataStructures);
                        break;
                    }

                case SdmxStructureEnumType.MetadataFlow:
                    {
                        returnSet.AddAll(this._metadataflows);
                        break;
                    }

                case SdmxStructureEnumType.Msd:
                    {
                        returnSet.AddAll(this._metadataStructures);
                        break;
                    }

                case SdmxStructureEnumType.OrganisationUnitScheme:
                    {
                        returnSet.AddAll(this._organisationUnitSchemes);
                        break;
                    }

                case SdmxStructureEnumType.Process:
                    {
                        returnSet.AddAll(this._processes);
                        break;
                    }

                case SdmxStructureEnumType.StructureSet:
                    {
                        returnSet.AddAll(this._structureSet);
                        break;
                    }

                case SdmxStructureEnumType.ReportingTaxonomy:
                    {
                        returnSet.AddAll(this._reportingTaxonomy);
                        break;
                    }

                case SdmxStructureEnumType.Categorisation:
                    {
                        returnSet.AddAll(this._categorisation);
                        break;
                    }

                case SdmxStructureEnumType.ProvisionAgreement:
                    {
                        returnSet.AddAll(this._provisionAgreement);
                        break;
                    }

                case SdmxStructureEnumType.AttachmentConstraint:
                    {
                        returnSet.AddAll(this._attachmentConstraints);
                        break;
                    }

                case SdmxStructureEnumType.ContentConstraint:
                    {
                        returnSet.AddAll(this._contentConstraints);
                        break;
                    }

                case SdmxStructureEnumType.Registration:
                    {
                        returnSet.AddAll(this._registrations);
                        break;
                    }

                case SdmxStructureEnumType.Subscription:
                    {
                        returnSet.AddAll(this._subscriptions);
                        break;
                    }

                default:
                    throw new SdmxNotImplementedException(ExceptionCode.ReferenceErrorUnsupportedQueryForStructure, structureType);
            }

            return returnSet;
        }

        /// <summary>
        /// Gets the metadata structures.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMetadataStructureDefinitionObject> GetMetadataStructures(string agencyId)
        {
            return new AgencyFilter<IMetadataStructureDefinitionObject>().FilterSet(agencyId, this._metadataStructures);
        }

        /// <summary>
        /// Gets the metadata structures.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMetadataStructureDefinitionObject> GetMetadataStructures(IMaintainableRefObject maintainableRefObject)
        {
            return
                new Filter<IMetadataStructureDefinitionObject>(SdmxStructureEnumType.Msd).FilterSet(
                    this._metadataStructures, maintainableRefObject);
        }

        /// <summary>
        /// Gets the metadataflows.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMetadataFlow> GetMetadataflows(string agencyId)
        {
            return new AgencyFilter<IMetadataFlow>().FilterSet(agencyId, this._metadataflows);
        }

        /// <summary>
        /// Gets the metadataflows.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IMetadataFlow> GetMetadataflows(IMaintainableRefObject xref)
        {
            return new Filter<IMetadataFlow>(SdmxStructureEnumType.MetadataFlow).FilterSet(this._metadataflows, xref);
        }

        /// <summary>
        /// Gets the organisation unit schemes.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemes(string agencyId)
        {
            return new AgencyFilter<IOrganisationUnitSchemeObject>().FilterSet(agencyId, this._organisationUnitSchemes);
        }

        /// <summary>
        /// Gets the organisation unit schemes.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemes(IMaintainableRefObject xref)
        {
            return
                new Filter<IOrganisationUnitSchemeObject>(SdmxStructureEnumType.OrganisationUnitScheme).FilterSet(
                    this._organisationUnitSchemes, xref);
        }

        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IProcessObject> GetProcesses(string agencyId)
        {
            return new AgencyFilter<IProcessObject>().FilterSet(agencyId, this._processes);
        }

        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IProcessObject> GetProcesses(IMaintainableRefObject xref)
        {
            return new Filter<IProcessObject>(SdmxStructureEnumType.Process).FilterSet(this._processes, xref);
        }

        /// <summary>
        /// Gets the provision agreements.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IProvisionAgreementObject> GetProvisionAgreements(string agencyId)
        {
            return new AgencyFilter<IProvisionAgreementObject>().FilterSet(agencyId, this._provisionAgreement);
        }

        /// <summary>
        /// Gets the provision agreements.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IProvisionAgreementObject> GetProvisionAgreements(IMaintainableRefObject xref)
        {
            return
                new Filter<IProvisionAgreementObject>(SdmxStructureEnumType.ProvisionAgreement).FilterSet(
                    this._provisionAgreement, xref);
        }

        /// <summary>
        /// Gets the registrations.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IRegistrationObject> GetRegistrations(string agencyId)
        {
            return new AgencyFilter<IRegistrationObject>().FilterSet(agencyId, this._registrations);
        }

        /// <summary>
        /// Gets the registrations.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IRegistrationObject> GetRegistrations(IMaintainableRefObject xref)
        {
            return new Filter<IRegistrationObject>(SdmxStructureEnumType.Registration).FilterSet(
                this._registrations, xref);
        }

        /// <summary>
        /// Gets the reporting taxonomys.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IReportingTaxonomyObject> GetReportingTaxonomys(string agencyId)
        {
            return new AgencyFilter<IReportingTaxonomyObject>().FilterSet(agencyId, this._reportingTaxonomy);
        }

        /// <summary>
        /// Gets the reporting taxonomys.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IReportingTaxonomyObject> GetReportingTaxonomys(IMaintainableRefObject xref)
        {
            return
                new Filter<IReportingTaxonomyObject>(SdmxStructureEnumType.ReportingTaxonomy).FilterSet(
                    this._reportingTaxonomy, xref);
        }

        /// <summary>
        /// Gets the structure sets.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IStructureSetObject> GetStructureSets(string agencyId)
        {
            return new AgencyFilter<IStructureSetObject>().FilterSet(agencyId, this._structureSet);
        }

        /// <summary>
        /// Gets the structure sets.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<IStructureSetObject> GetStructureSets(IMaintainableRefObject xref)
        {
            return new Filter<IStructureSetObject>(SdmxStructureEnumType.StructureSet).FilterSet(
                this._structureSet, xref);
        }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ISubscriptionObject> GetSubscriptions(string agencyId)
        {
            return new AgencyFilter<ISubscriptionObject>().FilterSet(agencyId, this._subscriptions);
        }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <param name="xref">
        /// The <see cref="IMaintainableRefObject"/>..
        /// </param>
        /// <returns>
        /// A copy of the set of requested objects
        /// </returns>
        public ISet<ISubscriptionObject> GetSubscriptions(IMaintainableRefObject xref)
        {
            return new Filter<ISubscriptionObject>(SdmxStructureEnumType.Subscription).FilterSet(
                this._subscriptions, xref);
        }

        /// <summary>
        ///     The has registrations.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool HasRegistrations()
        {
            return this._registrations.Count > 0;
        }

        /// <summary>
        ///     The has structures.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool HasStructures()
        {
            return this.GetAllMaintainables(SdmxStructureEnumType.Registration, SdmxStructureEnumType.Subscription).Count
                   > 0;
        }

        /// <summary>
        ///     The has subscriptions.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool HasSubscriptions()
        {
            return this._subscriptions.Count > 0;
        }

        /// <summary>
        /// The merge.
        /// </summary>
        /// <param name="sdmxObjects">
        /// The sdmx objects.
        /// </param>
        public void Merge(ISdmxObjects sdmxObjects)
        {
            if (sdmxObjects.Header != null)
            {
                this._header = sdmxObjects.Header;
            }

            if (sdmxObjects.Action != null)
            {
                this._action = sdmxObjects.Action;
            }

            // ADD
            this._agencySchemes.UnionWith(sdmxObjects.AgenciesSchemes);
            this._attachmentConstraints.UnionWith(sdmxObjects.AttachmentConstraints);
            this._contentConstraints.UnionWith(sdmxObjects.ContentConstraintObjects);
            this._dataConsumerSchemes.UnionWith(sdmxObjects.DataConsumerSchemes);
            this._dataProviderSchemes.UnionWith(sdmxObjects.DataProviderSchemes);
            this._categorySchemes.UnionWith(sdmxObjects.CategorySchemes);
            this._codelists.UnionWith(sdmxObjects.Codelists);
            this._conceptSchemes.UnionWith(sdmxObjects.ConceptSchemes);
            this._dataflows.UnionWith(sdmxObjects.Dataflows);
            this._hcls.UnionWith(sdmxObjects.HierarchicalCodelists);
            this._dataStructures.UnionWith(sdmxObjects.DataStructures);
            this._metadataflows.UnionWith(sdmxObjects.Metadataflows);
            this._metadataStructures.UnionWith(sdmxObjects.MetadataStructures);
            this._organisationUnitSchemes.UnionWith(sdmxObjects.OrganisationUnitSchemes);
            this._processes.UnionWith(sdmxObjects.Processes);
            this._structureSet.UnionWith(sdmxObjects.StructureSets);
            this._reportingTaxonomy.UnionWith(sdmxObjects.ReportingTaxonomys);
            this._categorisation.UnionWith(sdmxObjects.Categorisations);
            this._provisionAgreement.UnionWith(sdmxObjects.ProvisionAgreements);
            this._registrations.UnionWith(sdmxObjects.Registrations);
            this._subscriptions.UnionWith(sdmxObjects.Subscriptions);
        }

        /// <summary>
        /// The remove agency scheme.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveAgencyScheme(IAgencyScheme agencyScheme)
        {
            this._agencySchemes.Remove(agencyScheme);
        }

        /// <summary>
        /// The remove attachment constraint contentConstraint.
        /// </summary>
        /// <param name="attachmentConstraint">
        /// The agencyScheme.
        /// </param>
        public void RemoveAttachmentConstraintObject(IAttachmentConstraintObject attachmentConstraint)
        {
            this._attachmentConstraints.Remove(attachmentConstraint);
        }

        /// <summary>
        /// The remove categorisation.
        /// </summary>
        /// <param name="categorisation">
        /// The agencyScheme.
        /// </param>
        public void RemoveCategorisation(ICategorisationObject categorisation)
        {
            this._categorisation.Remove(categorisation);
        }

        /// <summary>
        /// The remove category scheme.
        /// </summary>
        /// <param name="categoryScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveCategoryScheme(ICategorySchemeObject categoryScheme)
        {
            this._categorySchemes.Remove(categoryScheme);
        }

        /// <summary>
        /// The remove codelist.
        /// </summary>
        /// <param name="codelistObject">
        /// The agencyScheme.
        /// </param>
        public void RemoveCodelist(ICodelistObject codelistObject)
        {
            this._codelists.Remove(codelistObject);
        }

        /// <summary>
        /// The remove concept scheme.
        /// </summary>
        /// <param name="conceptScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveConceptScheme(IConceptSchemeObject conceptScheme)
        {
            this._conceptSchemes.Remove(conceptScheme);
        }

        /// <summary>
        /// The remove content constraint contentConstraint.
        /// </summary>
        /// <param name="contentConstraint">
        /// The agencyScheme.
        /// </param>
        public void RemoveContentConstraintObject(IContentConstraintObject contentConstraint)
        {
            this._contentConstraints.Remove(contentConstraint);
        }

        /// <summary>
        /// The remove data consumer scheme.
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveDataConsumerScheme(IDataConsumerScheme dataConsumerScheme)
        {
            this._dataConsumerSchemes.Remove(dataConsumerScheme);
        }

        /// <summary>
        /// The remove data provider scheme.
        /// </summary>
        /// <param name="dataProviderScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveDataProviderScheme(IDataProviderScheme dataProviderScheme)
        {
            this._dataProviderSchemes.Remove(dataProviderScheme);
        }

        /// <summary>
        /// The remove data structure.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The agencyScheme.
        /// </param>
        public void RemoveDataStructure(IDataStructureObject dataStructureObject)
        {
            this._dataStructures.Remove(dataStructureObject);
        }

        /// <summary>
        /// The remove dataflow.
        /// </summary>
        /// <param name="dataflowObject">
        /// The agencyScheme.
        /// </param>
        public void RemoveDataflow(IDataflowObject dataflowObject)
        {
            this._dataflows.Remove(dataflowObject);
        }

        /// <summary>
        /// The remove hierarchical codelist.
        /// </summary>
        /// <param name="hierarchicalCodelistObject">
        /// The agencyScheme.
        /// </param>
        public void RemoveHierarchicalCodelist(IHierarchicalCodelistObject hierarchicalCodelistObject)
        {
            this._hcls.Remove(hierarchicalCodelistObject);
        }

        /// <summary>
        /// The remove maintainable.
        /// </summary>
        /// <param name="maintainableObject">
        /// The agencyScheme.
        /// </param>
        public void RemoveMaintainable(IMaintainableObject maintainableObject)
        {
            var agencyScheme = maintainableObject as IAgencyScheme;
            if (agencyScheme != null)
            {
                this.RemoveAgencyScheme(agencyScheme);
                return;
            }

            var attachmentConstraintObject = maintainableObject as IAttachmentConstraintObject;
            if (attachmentConstraintObject != null)
            {
                this.RemoveAttachmentConstraintObject(attachmentConstraintObject);
                return;
            }

            var contentConstraintObject = maintainableObject as IContentConstraintObject;
            if (contentConstraintObject != null)
            {
                this.RemoveContentConstraintObject(contentConstraintObject);
                return;
            }

            var dataConsumerScheme = maintainableObject as IDataConsumerScheme;
            if (dataConsumerScheme != null)
            {
                this.RemoveDataConsumerScheme(dataConsumerScheme);
                return;
            }

            var dataProviderScheme = maintainableObject as IDataProviderScheme;
            if (dataProviderScheme != null)
            {
                this.RemoveDataProviderScheme(dataProviderScheme);
                return;
            }

            var categorySchemeObject = maintainableObject as ICategorySchemeObject;
            if (categorySchemeObject != null)
            {
                this.RemoveCategoryScheme(categorySchemeObject);
                return;
            }

            var codelistObject = maintainableObject as ICodelistObject;
            if (codelistObject != null)
            {
                this.RemoveCodelist(codelistObject);
                return;
            }

            var conceptSchemeObject = maintainableObject as IConceptSchemeObject;
            if (conceptSchemeObject != null)
            {
                this.RemoveConceptScheme(conceptSchemeObject);
                return;
            }

            var dataflowObject = maintainableObject as IDataflowObject;
            if (dataflowObject != null)
            {
                this.RemoveDataflow(dataflowObject);
                return;
            }

            var hierarchicalCodelistObject = maintainableObject as IHierarchicalCodelistObject;
            if (hierarchicalCodelistObject != null)
            {
                this.RemoveHierarchicalCodelist(hierarchicalCodelistObject);
                return;
            }

            var dataStructureObject = maintainableObject as IDataStructureObject;
            if (dataStructureObject != null)
            {
                this.RemoveDataStructure(dataStructureObject);
                return;
            }

            var metadataFlow = maintainableObject as IMetadataFlow;
            if (metadataFlow != null)
            {
                this.RemoveMetadataFlow(metadataFlow);
                return;
            }

            var metadataStructureDefinitionObject = maintainableObject as IMetadataStructureDefinitionObject;
            if (metadataStructureDefinitionObject != null)
            {
                this.RemoveMetadataStructure(metadataStructureDefinitionObject);
                return;
            }

            var organisationUnitSchemeObject = maintainableObject as IOrganisationUnitSchemeObject;
            if (organisationUnitSchemeObject != null)
            {
                this.RemoveOrganisationUnitScheme(organisationUnitSchemeObject);
                return;
            }

            var structureSetObject = maintainableObject as IStructureSetObject;
            if (structureSetObject != null)
            {
                this.RemoveStructureSet(structureSetObject);
                return;
            }

            var processObject = maintainableObject as IProcessObject;
            if (processObject != null)
            {
                this.RemoveProcess(processObject);
                return;
            }

            var reportingTaxonomyObject = maintainableObject as IReportingTaxonomyObject;
            if (reportingTaxonomyObject != null)
            {
                this.RemoveReportingTaxonomy(reportingTaxonomyObject);
                return;
            }

            var categorisationObject = maintainableObject as ICategorisationObject;
            if (categorisationObject != null)
            {
                this.RemoveCategorisation(categorisationObject);
                return;
            }

            var provisionAgreementObject = maintainableObject as IProvisionAgreementObject;
            if (provisionAgreementObject != null)
            {
                this.RemoveProvisionAgreement(provisionAgreementObject);
                return;
            }

            var registrationObject = maintainableObject as IRegistrationObject;
            if (registrationObject != null)
            {
                this.RemoveRegistration(registrationObject);
                return;
            }

            var subscriptionObject = maintainableObject as ISubscriptionObject;
            if (subscriptionObject != null)
            {
                this.RemoveSubscription(subscriptionObject);
            }
        }

        /// <summary>
        /// The remove metadata flow.
        /// </summary>
        /// <param name="metadataFlow">
        /// The agencyScheme.
        /// </param>
        public void RemoveMetadataFlow(IMetadataFlow metadataFlow)
        {
            this._metadataflows.Remove(metadataFlow);
        }

        /// <summary>
        /// The remove metadata structure.
        /// </summary>
        /// <param name="metadataStructureDefinition">
        /// The agencyScheme.
        /// </param>
        public void RemoveMetadataStructure(IMetadataStructureDefinitionObject metadataStructureDefinition)
        {
            this._metadataStructures.Remove(metadataStructureDefinition);
        }

        /// <summary>
        /// The remove organisation unit scheme.
        /// </summary>
        /// <param name="organisationUnitScheme">
        /// The agencyScheme.
        /// </param>
        public void RemoveOrganisationUnitScheme(IOrganisationUnitSchemeObject organisationUnitScheme)
        {
            this._organisationUnitSchemes.Remove(organisationUnitScheme);
        }

        /// <summary>
        /// The remove process.
        /// </summary>
        /// <param name="process">
        /// The agencyScheme.
        /// </param>
        public void RemoveProcess(IProcessObject process)
        {
            this._processes.Remove(process);
        }

        /// <summary>
        /// The remove provision agreement.
        /// </summary>
        /// <param name="provisionAgreement">
        /// The agencyScheme.
        /// </param>
        public void RemoveProvisionAgreement(IProvisionAgreementObject provisionAgreement)
        {
            this._provisionAgreement.Remove(provisionAgreement);
        }

        /// <summary>
        /// The remove registration.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        public void RemoveRegistration(IRegistrationObject registration)
        {
            this._registrations.Remove(registration);
        }

        /// <summary>
        /// The remove reporting taxonomy.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The agencyScheme.
        /// </param>
        public void RemoveReportingTaxonomy(IReportingTaxonomyObject reportingTaxonomy)
        {
            this._reportingTaxonomy.Remove(reportingTaxonomy);
        }

        /// <summary>
        /// The remove structure set.
        /// </summary>
        /// <param name="structureSet">
        /// The agencyScheme.
        /// </param>
        public void RemoveStructureSet(IStructureSetObject structureSet)
        {
            this._structureSet.Remove(structureSet);
        }

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        public void RemoveSubscription(ISubscriptionObject subscription)
        {
            this._subscriptions.Remove(subscription);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add agency maintained structures to set.
        /// </summary>
        /// <param name="toAdd">
        /// The to add.
        /// </param>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <param name="walkSet">
        /// The walk set.
        /// </param>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        private static void AddAgencyMaintainedStructuresToSet<T>(
            ISet<IMaintainableObject> toAdd, IAgency agency, IEnumerable<T> walkSet) where T : IMaintainableObject
        {
            string agencyId = agency.Id;

            foreach (T currentMaint in walkSet)
            {
                if (currentMaint.Id.Equals(agencyId))
                {
                    toAdd.Add(currentMaint);
                }
            }
        }

        #endregion

        /// <summary>
        /// The agency filter.
        /// </summary>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        private class AgencyFilter<T>
            where T : IMaintainableObject
        {
            #region Public Methods and Operators

            /// <summary>
            /// The filter set.
            /// </summary>
            /// <param name="agencyId">
            /// The agency id.
            /// </param>
            /// <param name="walkSet">
            /// The walk set.
            /// </param>
            /// <returns>
            /// A copy of the set of requested objects
            /// </returns>
            public ISet<T> FilterSet(string agencyId, IEnumerable<T> walkSet)
            {
                ISet<T> returnSet = new SortedSet<T>(new MaintainableSortByIdentifiers<T>());

                foreach (T currentMaint in walkSet)
                {
                    if (currentMaint.AgencyId.Equals(agencyId))
                    {
                        returnSet.Add(currentMaint);
                    }
                }

                return returnSet;
            }

            #endregion
        }

        /// <summary>
        /// The filter.
        /// </summary>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        private class Filter<T>
            where T : IMaintainableObject
        {
            #region Fields

            /// <summary>
            ///     The _structure type.
            /// </summary>
            private readonly SdmxStructureEnumType _structureType;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Filter{T}"/> class.
            /// </summary>
            /// <param name="structureType">
            /// The structure type.
            /// </param>
            public Filter(SdmxStructureEnumType structureType)
            {
                this._structureType = structureType;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The filter set.
            /// </summary>
            /// <param name="input">
            /// The input.
            /// </param>
            /// <param name="maintainableRefObject">
            /// The <see cref="IMaintainableRefObject"/>..
            /// </param>
            /// <returns>
            /// A copy of the set of requested objects
            /// </returns>
            public ISet<T> FilterSet(IEnumerable<T> input, IMaintainableRefObject maintainableRefObject)
            {
                ISet<T> returnSet = new SortedSet<T>(new MaintainableSortByIdentifiers<T>());
                if (maintainableRefObject == null)
                {
                    returnSet.AddAll(input);
                    return returnSet;
                }

                IStructureReference structureReference = new StructureReferenceImpl(
                    maintainableRefObject, this._structureType);

                foreach (T currentInput in input)
                {
                    if (structureReference.IsMatch(currentInput))
                    {
                        returnSet.Add(currentInput);
                    }
                }

                return returnSet;
            }

            #endregion
        }

        /// <summary>
        /// The retrun set creator.
        /// </summary>
        /// <typeparam name="T">
        /// Generic type param
        /// </typeparam>
        private class RetrunSetCreator<T>
            where T : IMaintainableObject
        {
            #region Public Methods and Operators

            /// <summary>
            /// The create return set.
            /// </summary>
            /// <param name="immutableSet">
            /// The immutable set.
            /// </param>
            /// <returns>
            /// A copy of the set of requested objects
            /// </returns>
            public ISet<T> CreateReturnSet(IEnumerable<T> immutableSet)
            {
                ISet<T> returnSet = new SortedSet<T>(new MaintainableSortByIdentifiers<T>());
                returnSet.AddAll(immutableSet);
                return returnSet;
            }

            #endregion
        }

        /// <summary>
        /// Gets a value indicating whether there are attachment constraints in this container
        /// </summary>
        public bool HasAttachmentConstraints
        {
            get { return ObjectUtil.ValidCollection(_attachmentConstraints); }
        }

        /// <summary>
        /// Gets a value indicating whether there are agency schemes in this container
        /// </summary>
        public bool HasAgenciesSchemes
        {
            get { return ObjectUtil.ValidCollection(_agencySchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are content constraints in this container
        /// </summary>
        public bool HasContentConstraintBeans
        {
            get { return ObjectUtil.ValidCollection(_contentConstraints); }
        }

        /// <summary>
        /// Gets a value indicating whether there are organisation unit schemes in this container
        /// </summary>
        public bool HasOrganisationUnitSchemes
        {
            get { return ObjectUtil.ValidCollection(_organisationUnitSchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are data consumer schemes in this container
        /// </summary>
        public bool HasDataConsumerSchemes
        {
            get { return ObjectUtil.ValidCollection(_dataConsumerSchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are dataflows in this container
        /// </summary>
        public bool HasDataflows
        {
            get { return ObjectUtil.ValidCollection(_dataflows); }
        }

        /// <summary>
        /// Gets a value indicating whether there are data provider schemes in this container
        /// </summary>
        public bool HasDataProviderSchemes
        {
            get { return ObjectUtil.ValidCollection(_dataProviderSchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are metadataflows in this container
        /// </summary>
        public bool HasMetadataflows
        {
            get { return ObjectUtil.ValidCollection(_metadataflows); }
        }

        /// <summary>
        /// Gets a value indicating whether there are category schemes in this container
        /// </summary>
        public bool HasCategorySchemes
        {
            get { return ObjectUtil.ValidCollection(_categorySchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are codelists in this container
        /// </summary>
        public bool HasCodelists
        {
            get { return ObjectUtil.ValidCollection(_codelists); }
        }

        /// <summary>
        /// Gets a value indicating whether there are codelists in this container
        /// </summary>
        public bool HasHierarchicalCodelists
        {
            get { return ObjectUtil.ValidCollection(_hcls); }
        }

        /// <summary>
        /// Gets a value indicating whether there are categorisations in this container
        /// </summary>
        public bool HasCategorisations
        {
            get { return ObjectUtil.ValidCollection(_categorisation); }
        }

        /// <summary>
        /// Gets a value indicating whether there are concept schemes in this container
        /// </summary>
        public bool HasConceptSchemes
        {
            get { return ObjectUtil.ValidCollection(_conceptSchemes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are metadata structures in this container
        /// </summary>
        public bool HasMetadataStructures
        {
            get { return ObjectUtil.ValidCollection(_metadataStructures); }
        }

        /// <summary>
        /// Gets a value indicating whether there are data structures in this container
        /// </summary>
        public bool HasDataStructures
        {
            get { return ObjectUtil.ValidCollection(_dataStructures); }
        }

        /// <summary>
        /// Gets a value indicating whether there are reporting taxonomies in this container
        /// </summary>
        public bool HasReportingTaxonomys
        {
            get { return ObjectUtil.ValidCollection(_reportingTaxonomy); }
        }

        /// <summary>
        /// Gets a value indicating whether there are structure sets in this container
        /// </summary>
        public bool HasStructureSets
        {
            get { return ObjectUtil.ValidCollection(_structureSet); }
        }

        /// <summary>
        /// Gets a value indicating whether there are processes in this container
        /// </summary>
        public bool HasProcesses
        {
            get { return ObjectUtil.ValidCollection(_processes); }
        }

        /// <summary>
        /// Gets a value indicating whether there are provision agreements in this container
        /// </summary>
        public bool HasProvisionAgreements
        {
            get { return ObjectUtil.ValidCollection(_provisionAgreement); }
        }

        /// <summary>
        /// Gets the agency schemes in this container that are maintained by the given agency, returns null if no agencies exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IAgencyScheme"/>; otherwise null if no agencies exist.
        /// </returns>
        public IAgencyScheme GetAgenciesScheme(string agencyId)
        {
            foreach (IAgencyScheme acyScheme in _agencySchemes)
            {
                if (acyScheme.AgencyId.Equals(agencyId))
                    return acyScheme;
            }
            return null;
        }

        /// <summary>
        /// Gets all the data consumer scheme in this container that is maintained by the given agency, returns null if no data consumer scheme exists
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IDataConsumerScheme"/> .
        /// </returns>
        public IDataConsumerScheme GetDataConsumerScheme(string agencyId)
        {
            foreach (IDataConsumerScheme scheme in _dataConsumerSchemes)
            {
                if (scheme.AgencyId.Equals(agencyId))
                    return scheme;
            }
            return null;
        }

        /// <summary>
        /// Returns the data provider scheme in this container that are maintained by the given agency, returns an empty set if no data provider scheme exist
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <returns>
        /// The <see cref="IDataProviderScheme"/> .
        /// </returns>
        public IDataProviderScheme GetDataProviderScheme(string agencyId)
        {
            foreach (IDataProviderScheme scheme in _dataProviderSchemes)
            {
                if (scheme.AgencyId.Equals(agencyId))
                    return scheme;
            }
            return null;
        }
    }
}
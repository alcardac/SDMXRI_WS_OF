// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationEventCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The notification event impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///   The notification event impl.
    /// </summary>
    public class NotificationEventCore : INotificationEvent
    {
        #region Fields

        /// <summary>
        ///   The _action.
        /// </summary>
        private readonly DatasetAction _action;

        /// <summary>
        ///   The objects.
        /// </summary>
        private readonly ISdmxObjects objects;

        /// <summary>
        ///   The _event time.
        /// </summary>
        private readonly DateTime? _eventTime;

        /// <summary>
        ///   The _object urn.
        /// </summary>
        private readonly Uri _objectUrn;

        /// <summary>
        ///   The _registration.
        /// </summary>
        private readonly IRegistrationObject _registration;

        /// <summary>
        ///   The _subscription urn.
        /// </summary>
        private readonly Uri _subscriptionUrn;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventCore"/> class.
        /// </summary>
        /// <param name="notification">
        /// The notification. 
        /// </param>
        public NotificationEventCore(NotifyRegistryEventType notification)
        {
            this._eventTime = notification.EventTime; //TODO: True param DateUtil
            this._objectUrn = notification.ObjectURN;
            this._subscriptionUrn = notification.SubscriptionURN;
            this._action = (DatasetAction)Enum.Parse(typeof(DatasetAction), notification.EventAction.ToUpperInvariant());
            if (notification.RegistrationEvent != null)
            {
                this._registration = new RegistrationObjectCore(notification.RegistrationEvent.Registration);
            }
            else
            {
                this.objects = new SdmxObjectsImpl();
                StructuresType structures = notification.StructuralEvent.Structures.Content;
                if (structures.Categorisations != null)
                {
                    foreach (CategorisationType type in structures.Categorisations.Categorisation)
                    {
                        this.objects.AddCategorisation(new CategorisationObjectCore(type));
                    }
                }

                if (structures.CategorySchemes != null)
                {
                    foreach (CategorySchemeType type0 in structures.CategorySchemes.CategoryScheme)
                    {
                        this.objects.AddCategoryScheme(new CategorySchemeObjectCore(type0));
                    }
                }

                if (structures.Codelists != null)
                {
                    foreach (CodelistType type1 in structures.Codelists.Codelist)
                    {
                        this.objects.AddCodelist(new CodelistObjectCore(type1));
                    }
                }

                if (structures.Concepts != null)
                {
                    foreach (ConceptSchemeType type2 in structures.Concepts.ConceptScheme)
                    {
                        this.objects.AddConceptScheme(new ConceptSchemeObjectCore(type2));
                    }
                }

                if (structures.Constraints != null)
                {
                    foreach (AttachmentConstraintType type3 in structures.Constraints.AttachmentConstraint)
                    {
                        this.objects.AddAttachmentConstraint(new AttachmentConstraintObjectCore(type3));
                    }

                    foreach (ContentConstraintType type4 in structures.Constraints.ContentConstraint)
                    {
                        this.objects.AddContentConstraintObject(new ContentConstraintObjectCore(type4));
                    }
                }

                if (structures.Dataflows != null)
                {
                    foreach (DataflowType type5 in structures.Dataflows.Dataflow)
                    {
                        this.objects.AddDataflow(new DataflowObjectCore(type5));
                    }
                }

                if (structures.DataStructures != null)
                {
                    foreach (DataStructureType type6 in structures.DataStructures.DataStructure)
                    {
                        this.objects.AddDataStructure(new DataStructureObjectCore(type6));
                    }
                }

                if (structures.HierarchicalCodelists != null)
                {
                    foreach (HierarchicalCodelistType type7 in structures.HierarchicalCodelists.HierarchicalCodelist)
                    {
                        this.objects.AddHierarchicalCodelist(new HierarchicalCodelistObjectCore(type7));
                    }
                }

                if (structures.Metadataflows != null)
                {
                    foreach (MetadataflowType type8 in structures.Metadataflows.Metadataflow)
                    {
                        this.objects.AddMetadataFlow(new MetadataflowObjectCore(type8));
                    }
                }

                if (structures.MetadataStructures != null)
                {
                    foreach (MetadataStructureType type9 in structures.MetadataStructures.MetadataStructure)
                    {
                        this.objects.AddMetadataStructure(new MetadataStructureDefinitionObjectCore(type9));
                    }
                }

                if (structures.OrganisationSchemes != null)
                {
                    foreach (AgencySchemeType type10 in structures.OrganisationSchemes.AgencyScheme)
                    {
                        this.objects.AddAgencyScheme(new AgencySchemeCore(type10));
                    }

                    foreach (DataConsumerSchemeType type11 in structures.OrganisationSchemes.DataConsumerScheme)
                    {
                        this.objects.AddDataConsumerScheme(new DataConsumerSchemeCore(type11));
                    }

                    foreach (DataProviderSchemeType type12 in structures.OrganisationSchemes.DataProviderScheme)
                    {
                        this.objects.AddDataProviderScheme(new DataProviderSchemeCore(type12));
                    }

                    foreach (OrganisationUnitSchemeType type13 in structures.OrganisationSchemes.OrganisationUnitScheme)
                    {
                        this.objects.AddOrganisationUnitScheme(new OrganisationUnitSchemeObjectCore(type13));
                    }
                }

                if (structures.Processes != null)
                {
                    foreach (ProcessType type14 in structures.Processes.Process)
                    {
                        this.objects.AddProcess(new ProcessObjectCore(type14));
                    }
                }

                if (structures.ProvisionAgreements != null)
                {
                    foreach (ProvisionAgreementType type15 in structures.ProvisionAgreements.ProvisionAgreement)
                    {
                        this.objects.AddProvisionAgreement(new ProvisionAgreementObjectCore(type15));
                    }
                }

                if (structures.ReportingTaxonomies != null)
                {
                    foreach (ReportingTaxonomyType type16 in structures.ReportingTaxonomies.ReportingTaxonomy)
                    {
                        this.objects.AddReportingTaxonomy(new ReportingTaxonomyObjectCore(type16));
                    }
                }

                if (structures.StructureSets != null)
                {
                    foreach (StructureSetType type17 in structures.StructureSets.StructureSet)
                    {
                        this.objects.AddStructureSet(new StructureSetObjectCore(type17));
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.0 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventCore"/> class.
        /// </summary>
        /// <param name="notification">
        /// The notification. 
        /// </param>
        public NotificationEventCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.NotifyRegistryEventType notification)
        {
            this._eventTime = notification.EventTime;
            this._objectUrn = notification.ObjectURN;
            this._subscriptionUrn = notification.SubscriptionURN;
            this._action = (DatasetAction)Enum.Parse(typeof(DatasetAction), notification.EventAction.ToUpperInvariant());
            if (notification.RegistrationEvent != null)
            {
                this._registration = new RegistrationObjectCore(notification.RegistrationEvent.Registration);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the action.
        /// </summary>
        public DatasetAction Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        ///   Gets the beans.
        /// </summary>
        public ISdmxObjects Objects
        {
            get
            {
                return this.objects;
            }
        }

        /// <summary>
        ///   Gets the event time.
        /// </summary>
        public DateTime? EventTime
        {
            get
            {
                return this._eventTime;
            }
        }

        /// <summary>
        ///   Gets the object urn.
        /// </summary>
        public Uri ObjectUrn
        {
            get
            {
                return this._objectUrn;
            }
        }

        /// <summary>
        ///   Gets the registration.
        /// </summary>
        public IRegistrationObject Registration
        {
            get
            {
                return this._registration;
            }
        }

        /// <summary>
        ///   Gets the subscription urn.
        /// </summary>
        public Uri SubscriptionUrn
        {
            get
            {
                return this._subscriptionUrn;
            }
        }

        #endregion
    }
}
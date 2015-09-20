// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The subscription object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Email;

    using AgencyScheme = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.AgencyScheme;
    using SubscriptionType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubscriptionType;

    /// <summary>
    ///   The subscription object core.
    /// </summary>
    [Serializable]
    public class SubscriptionObjectCore : MaintainableObjectCore<ISubscriptionObject, ISubscriptionMutableObject>, 
                                          ISubscriptionObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SubscriptionObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The _mail to.
        /// </summary>
        private readonly IList<string> _mailTo;

        /// <summary>
        ///   The _owner.
        /// </summary>
        private readonly ICrossReference _owner;

        /// <summary>
        ///   The _references.
        /// </summary>
        private readonly IList<IStructureReference> _references;

        /// <summary>
        ///   The _http post to.
        /// </summary>
        private readonly IList<string> _httpPostTo;

        /// <summary>
        /// The urn.
        /// </summary>
        private readonly Uri _urn;

        /// <summary>
        ///   The _subscription type.
        /// </summary>
        private SubscriptionEnumType _subscriptionType;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionObjectCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        public SubscriptionObjectCore(ISubscriptionMutableObject mutable)
            : base(mutable)
        {
            this._mailTo = new List<string>();
            this._httpPostTo = new List<string>();
            this._references = new List<IStructureReference>();
            _log.Debug("Building Subscription from Mutable Object");
            if (mutable.Owner != null)
            {
                this._owner = new CrossReferenceImpl(this, mutable.Owner);
            }

            if (mutable.MailTo != null)
            {
                this._mailTo = new List<string>(mutable.MailTo);
            }

            if (mutable.HttpPostTo != null)
            {
                this._httpPostTo = new List<string>(mutable.HttpPostTo);
            }

            if (mutable.References != null)
            {
                foreach (IStructureReference structureReference in mutable.References)
                {
                    this._references.Add(structureReference.CreateCopy());
                }
            }

            this._subscriptionType = mutable.SubscriptionType;
            this.Validate();
            this._urn = new Uri(base.Urn, "." + this._owner.ChildReference.Id + "." + this._owner.TargetReference.UrnClass);
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("subscription Built {0}", this._urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionObjectCore"/> class.
        /// </summary>
        /// <param name="subscription">
        /// The subscription. 
        /// </param>
        public SubscriptionObjectCore(SubscriptionType subscription)
            : base(
                subscription, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription), 
                null, 
                null, 
                MaintainableObject.DefaultVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset), 
                GetSubAgencyId(subscription), 
                GetSubId(subscription), 
                null, 
                null, 
                null, 
                null, 
                null)
        {
            this._mailTo = new List<string>();
            this._httpPostTo = new List<string>();
            this._references = new List<IStructureReference>();
            _log.Debug("Building Subscription from 2.1 SDMX");
            if (subscription.Organisation != null)
            {
                this._owner = RefUtil.CreateReference(this, subscription.Organisation);
            }

            if (subscription.ValidityPeriod != null)
            {
                if (subscription.ValidityPeriod.StartDate != default(DateTime))
                {
                    this.StartDate = new SdmxDateCore(
                        subscription.ValidityPeriod.StartDate, TimeFormatEnumType.DateTime);
                }

                if (subscription.ValidityPeriod.EndDate != default(DateTime))
                {
                    this.EndDate = new SdmxDateCore(subscription.ValidityPeriod.EndDate, TimeFormatEnumType.DateTime);
                }
            }

            if (subscription.NotificationHTTP != null)
            {
                foreach (NotificationURLType not in subscription.NotificationHTTP)
                {
                    this._httpPostTo.Add(not.TypedValue.ToString());
                }
            }

            if (subscription.NotificationMailTo != null)
            {
                foreach (NotificationURLType not0 in subscription.NotificationMailTo)
                {
                    this._mailTo.Add(not0.TypedValue.ToString());
                }
            }

            if (subscription.EventSelector != null)
            {
                EventSelectorType eventSelector = subscription.EventSelector;

                foreach (StructuralRepositoryEventsType repositoryEvent in eventSelector.StructuralRepositoryEvents)
                {
                    this._subscriptionType = SubscriptionEnumType.Structure;

                    IList<string> agencies = repositoryEvent.AgencyID.Cast<string>().ToList();

                    foreach (string currentAgency in agencies)
                    {
                        // If any of the agencies is a wildcard, then make the list null as it is not important
                        if (currentAgency.Equals(SubscriptionObject.Wildcard))
                        {
                            agencies = null;
                            break;
                        }
                    }

                    if (repositoryEvent.AllEvents != null)
                    {
                        if (!(agencies != null && agencies.Count > 0))
                        {
                            this._references.Add(
                                new StructureReferenceImpl(
                                    null, null, null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any)));
                        }
                        else
                        {
                            foreach (string currentAgency1 in agencies)
                            {
                                this._references.Add(
                                    new StructureReferenceImpl(
                                        currentAgency1, 
                                        null, 
                                        null, 
                                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any)));
                            }
                        }

                        // No need to process any more information, this is a subscription for everything
                        break;
                    }

                    this.AddEventSubscriptions(
                        repositoryEvent.AgencyScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.AttachmentConstraint, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint));

                    // addEventSubscriptions(repositoryEvent.getCategorisationList(), agencies, SdmxStructureType.C);
                    this.AddEventSubscriptions(
                        repositoryEvent.CategoryScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.Codelist, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
                    this.AddEventSubscriptions(
                        repositoryEvent.ConceptScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.ContentConstraint, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint));
                    this.AddEventSubscriptions(
                        repositoryEvent.DataProviderScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.DataConsmerScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.Dataflow, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
                    this.AddEventSubscriptions(
                        repositoryEvent.HierarchicalCodelist, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist));
                    this.AddEventSubscriptions(
                        repositoryEvent.KeyFamily, agencies, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
                    this.AddEventSubscriptions(
                        repositoryEvent.Metadataflow, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
                    this.AddEventSubscriptions(
                        repositoryEvent.MetadataStructureDefinition, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));
                    this.AddEventSubscriptions(
                        repositoryEvent.OrganisationUnitScheme, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme));
                    this.AddEventSubscriptions(
                        repositoryEvent.Process, agencies, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process));
                    this.AddEventSubscriptions(
                        repositoryEvent.ProvisionAgreement, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement));
                    this.AddEventSubscriptions(
                        repositoryEvent.ReportingTaxonomy, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy));
                    this.AddEventSubscriptions(
                        repositoryEvent.StructureSet, 
                        agencies, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet));
                }

                foreach (DataRegistrationEventsType repositoryEvent2 in eventSelector.DataRegistrationEvents)
                {
                    this._subscriptionType = SubscriptionEnumType.DataRegistration;
                    if (repositoryEvent2.AllEvents != null)
                    {
                        this._references.Add(
                            new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any)));

                        // No need to process any more information, this is a subscription for everything
                        break;
                    }

                    this.AddEventSubscriptions(
                        repositoryEvent2.DataflowReference, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
                    this.AddEventSubscriptions(
                        repositoryEvent2.KeyFamilyReference, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
                    this.AddEventSubscriptions(repositoryEvent2.ProvisionAgreement);
                    this.AddEventSubscriptions(repositoryEvent2.Category);
                    this.AddEventSubscriptions(repositoryEvent2.DataProvider);
                }

                foreach (MetadataRegistrationEventsType repositoryEvent3 in eventSelector.MetadataRegistrationEvents)
                {
                    this._subscriptionType = SubscriptionEnumType.MetadataRegistration;
                    if (repositoryEvent3.AllEvents != null)
                    {
                        this._references.Add(
                            new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any)));

                        // No need to process any more information, this is a subscription for everything
                        break;
                    }

                    this.AddEventSubscriptions(
                        repositoryEvent3.MetadataflowReference, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
                    this.AddEventSubscriptions(
                        repositoryEvent3.MetadataStructureDefinitionReference, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));
                    this.AddEventSubscriptions(repositoryEvent3.ProvisionAgreement);
                    this.AddEventSubscriptions(repositoryEvent3.Category);
                    this.AddEventSubscriptions(repositoryEvent3.DataProvider);
                }
            }

            this.Validate();
            this._urn = new Uri(base.Urn, "." + this._owner.ChildReference.Id + "." + this._owner.TargetReference.UrnClass);
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("subscription Built {0}", this._urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmxObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private SubscriptionObjectCore(IMaintainableObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this._mailTo = new List<string>();
            this._httpPostTo = new List<string>();
            this._references = new List<IStructureReference>();
            _urn = new Uri(base.Urn, "." + this._owner.ChildReference.Id + "." + this._owner.TargetReference.UrnClass);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the agency id.
        /// </summary>
        public override string AgencyId
        {
            get
            {
                if (this._owner.TargetReference == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency))
                {
                    if(_owner.MaintainableReference.AgencyId.Equals(AgencyScheme.DefaultScheme))
                        return this._owner.ChildReference.Id;

                    return this._owner.IdentifiableIds.Aggregate(this._owner.MaintainableReference.AgencyId, (current, currentIdentId) => current + ("." + currentIdentId));
                }
              
                return this._owner.MaintainableReference.AgencyId;
            }
        }

        /// <summary>
        ///   Gets the http post to.
        /// </summary>
        public virtual IList<string> HTTPPostTo
        {
            get
            {
                return new List<string>(this._httpPostTo);
            }
        }

        /// <summary>
        ///   Gets the mail to.
        /// </summary>
        public virtual IList<string> MailTo
        {
            get
            {
                return new List<string>(this._mailTo);
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override ISubscriptionMutableObject MutableInstance
        {
            get
            {
                return new SubscriptionMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the owner.
        /// </summary>
        public virtual ICrossReference Owner
        {
            get
            {
                return this._owner;
            }
        }

        /// <summary>
        ///   Gets the references.
        /// </summary>
        public virtual IList<IStructureReference> References
        {
            get
            {
                return new List<IStructureReference>(this._references);
            }
        }

        /// <summary>
        ///   Gets or sets the subscription type.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        public virtual SubscriptionEnumType SubscriptionType
        {
            get
            {
                return this._subscriptionType;
            }

            set
            {
                if (this._subscriptionType != value)
                {
                    throw new SdmxSemmanticException(
                        "Subscription can not be for more then one event type (structure event, registration event or a provision event)");
                }

                this._subscriptionType = value;
            }
        }

        /// <summary>
        ///   Gets the urn.
        /// </summary>
        public override Uri Urn
        {
            get
            {
                // if(owner.getTargetReference() == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AGENCY)) {
                // return super.getUrn();
                // }
                return this._urn;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            throw new SdmxNotImplementedException("deepEquals on subscription");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="ISubscriptionObject"/> . 
        /// </returns>
        public override ISubscriptionObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new SubscriptionObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate agency id.
        /// </summary>
        protected internal override void ValidateAgencyId()
        {
            // Do nothing yet, not yet fully built
        }

        /// <summary>
        /// The get sub agency id.
        /// </summary>
        /// <param name="subscription">
        /// The subscription. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        private static string GetSubAgencyId(SubscriptionType subscription)
        {
            return RefUtil.CreateReference(subscription.Organisation).MaintainableReference.AgencyId;
        }

        /// <summary>
        /// The get sub id.
        /// </summary>
        /// <param name="subscription">
        /// The subscription. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        private static string GetSubId(SubscriptionType subscription)
        {
            if (!string.IsNullOrWhiteSpace(subscription.SubscriberAssignedID))
            {
                return subscription.SubscriberAssignedID;
            }

            return "Generated" + DateTime.Now.Millisecond;
        }

        /// <summary>
        /// The add event subscriptions.
        /// </summary>
        /// <param name="events">
        /// The events. 
        /// </param>
        /// <param name="agencyIds">
        /// The agency ids. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        private void AddEventSubscriptions(
            IEnumerable<VersionableObjectEventType> events, ICollection<string> agencyIds, SdmxStructureType structureType)
        {
            foreach (VersionableObjectEventType vot in events)
            {
                if (vot.URN != null)
                {
                    // FUNC Test the urn is of the correct type?
                    this._references.Add(new StructureReferenceImpl(vot.URN));
                }
                else
                {
                    string id = vot.ID != null ? vot.ID.ToString() : null;
                    string version = vot.Version != null ? vot.Version.ToString() : null;
                    if (id != null && id.Equals(SubscriptionObject.Wildcard))
                    {
                        id = null;
                    }

                    if (version != null && version.Equals(SubscriptionObject.Wildcard))
                    {
                        version = null;
                    }

                    if (ObjectUtil.ValidCollection(agencyIds))
                    {
                        foreach (string agencyId in agencyIds)
                        {
                            this._references.Add(new StructureReferenceImpl(agencyId, id, version, structureType));
                        }
                    }
                    else
                    {
                        this._references.Add(new StructureReferenceImpl(null, id, version, structureType));
                    }
                }
            }
        }

        /// <summary>
        /// The add event subscriptions.
        /// </summary>
        /// <param name="events">
        /// The events. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        private void AddEventSubscriptions(IEnumerable<MaintainableEventType> events, SdmxStructureType structureType)
        {
            foreach (MaintainableEventType evt0 in events)
            {
                if (evt0.URN != null)
                {
                    this._references.Add(new StructureReferenceImpl(evt0.URN));
                }
                else
                {
                    MaintainableQueryType queryType = evt0.Ref;
                    this._references.Add(
                        new StructureReferenceImpl(
                            queryType.agencyID.ToString(), 
                            queryType.id.ToString(), 
                            queryType.version.ToString(), 
                            structureType));
                }
            }
        }

        /// <summary>
        /// The add event subscriptions.
        /// </summary>
        /// <param name="events">
        /// The events. 
        /// </param>
        /// <typeparam name="T">Generic type param of type ReferenceType
        /// </typeparam>
        private void AddEventSubscriptions<T>(IEnumerable<T> events) where T : ReferenceType
        {
            foreach (T ev in events)
            {
                this._references.Add(RefUtil.CreateReference(this, ev));
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.IsFinal.IsTrue)
            {
                throw new SdmxSemmanticException("Subscription can not be made final");
            }

            if (!this.Version.Equals(MaintainableObject.DefaultVersion))
            {
                throw new SdmxSemmanticException(
                    "Subscription can not have a version, other then the default version : "
                    + MaintainableObject.DefaultVersion);
            }

            if (this._owner == null)
            {
                throw new SdmxSemmanticException(
                    "Subscription must have an owner which must be a data consumer - no owner provided");
            }

            if (this._owner.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer)
                && this._owner.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider)
                && this._owner.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency))
            {
                throw new SdmxSemmanticException(
                    "Subscription must have an owner which must be an agency, data provider, or data consumer - "
                    + this._owner.TargetReference.StructureType + " was provided");
            }

            if (this._mailTo.Count == 0 && this.HTTPPostTo.Count == 0)
            {
                throw new SdmxSemmanticException(
                    "Subscription must declare at least one HTTP POST to, or mail to address to send notifications to");
            }

            foreach (string currentEmail in this._mailTo)
            {
                if (!EmailValidation.ValidateEmail(currentEmail))
                {
                    throw new SdmxSemmanticException("'" + currentEmail + "' is not a valid email address");
                }
            }

            foreach (string currentHttp in this.HTTPPostTo)
            {
                if (!Uri.IsWellFormedUriString(currentHttp, UriKind.RelativeOrAbsolute))
                {
                    throw new SdmxSemmanticException("'" + currentHttp + "' is not a valid Uri");
                }
            }

            // FUNC Validate email addresses?
            // FUNC Validate POST addresses?
            if (this._references.Count == 0)
            {
                // Subscribe to ALL
                this._references.Add(
                    new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any)));
            }

            if (this._subscriptionType == SubscriptionEnumType.Null)
            {
                throw new SdmxSemmanticException("Subscription type not declared");
            }

            base.AgencyId = this._owner.MaintainableReference.AgencyId;
            base.ValidateAgencyId();
        }

        #endregion
    }
}
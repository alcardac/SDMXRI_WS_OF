// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The subscription xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;
    using Org.Sdmxsource.Util;

    using Xml.Schema.Linq;

    using SubscriptionBeanConstants = Org.Sdmxsource.Sdmx.Api.Constants.SubscriptionBeanConstants;
    using SubscriptionType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubscriptionType;

    /// <summary>
    ///     The subscription xml bean builder.
    /// </summary>
    public class SubscriptionXmlBuilder : NameableAssembler, IBuilder<SubscriptionType, ISubscriptionObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="SubscriptionType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="ISubscriptionObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="SubscriptionType"/>.
        /// </returns>
        public virtual SubscriptionType Build(ISubscriptionObject buildFrom)
        {
            var subscriptionType = new SubscriptionType();

            // 1. Set organisation reference
            var orfRef = new OrganisationReferenceType();
            subscriptionType.Organisation = orfRef;
            var xref = new OrganisationRefType();
            orfRef.SetTypedRef(xref);

            this.SetReference(xref, buildFrom.Owner);

            // TODO HACK this needs testing compare with Java output. Similar method is used in Java 0.9.1
            // try to add XSD namespace of AgencyRefType
            XNamespace com = ((IXMetaData)(new AgencyRefType())).SchemaName.NamespaceName;
            xref.Untyped.Add(new XAttribute(XNamespace.Xmlns + "com", com.NamespaceName));
            IXMetaData metaData = null;
            switch (buildFrom.Owner.TargetReference.EnumType)
            {
                case SdmxStructureEnumType.Agency:
                    metaData = new AgencyRefType();
                    break;
                case SdmxStructureEnumType.DataProvider:
                    metaData = new DataProviderRefType();
                    break;
                case SdmxStructureEnumType.DataConsumer:
                    metaData = new DataConsumerRefType();
                    break;
                case SdmxStructureEnumType.OrganisationUnit:
                    metaData = new OrganisationUnitRefType();
                    break;
            }

            if (metaData != null)
            {
                xref.Untyped.Add(
                    new XAttribute(
                        XName.Get("http://www.w3.org/2001/XMLSchema-instance", "type"), 
                        "com:" + metaData.SchemaName.LocalName));
            }

            // 2. Set notification HTTP and Email
            foreach (string currentHttp in buildFrom.HTTPPostTo)
            {
                var notificationUrl = new NotificationURLType();
                subscriptionType.NotificationHTTP.Add(notificationUrl);
                notificationUrl.TypedValue = new Uri(currentHttp);
            }

            /* foreach */
            foreach (string currentHttp0 in buildFrom.MailTo)
            {
                var notificationUrl1 = new NotificationURLType();
                subscriptionType.NotificationMailTo.Add(notificationUrl1);
                notificationUrl1.TypedValue = new Uri(currentHttp0);
            }

            // 3. Set it
            subscriptionType.SubscriberAssignedID = buildFrom.Id;

            // 4. Set Validity Period
            var validityPeriod = new ValidityPeriodType();
            subscriptionType.ValidityPeriod = validityPeriod;
            if (buildFrom.StartDate != null && buildFrom.StartDate.Date.HasValue)
            {
                validityPeriod.StartDate = buildFrom.StartDate.Date.Value;
            }

            if (buildFrom.EndDate != null && buildFrom.EndDate.Date != null)
            {
                validityPeriod.EndDate = buildFrom.EndDate.Date.Value;
            }

            // 5. Build Subscription Events
            var eventSelector = new EventSelectorType();
            subscriptionType.EventSelector = eventSelector;
            IList<IStructureReference> structureReferences = buildFrom.References;
            switch (buildFrom.SubscriptionType)
            {
                case SubscriptionEnumType.DataRegistration:
                    this.BuildDataSubscription(eventSelector, structureReferences);
                    break;
                case SubscriptionEnumType.MetadataRegistration:
                    this.BuildMetadataSubscription(eventSelector, structureReferences);
                    break;
                case SubscriptionEnumType.Structure:
                    BuildStructureSubscription(eventSelector, structureReferences);
                    break;
            }

            return subscriptionType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build structure subscription.
        /// </summary>
        /// <param name="eventSelector">
        /// The event selector.
        /// </param>
        /// <param name="subscriptions">
        /// The subscriptions.
        /// </param>
        private static void BuildStructureSubscription(
            EventSelectorType eventSelector, IList<IStructureReference> subscriptions)
        {
            var structureEvents = new StructuralRepositoryEventsType();
            eventSelector.StructuralRepositoryEvents.Add(structureEvents);

            bool addedAgency = false;

            /* foreach */
            foreach (IStructureReference structureReference in subscriptions)
            {
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                string value = maintainableReference.AgencyId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    structureEvents.AgencyID.Add(maintainableReference.AgencyId);
                    addedAgency = true;
                }
            }

            if (!addedAgency)
            {
                structureEvents.AgencyID.Add(SubscriptionBeanConstants.Wildcard);
            }

            VersionableObjectEventType eventType = null;
            foreach (IStructureReference structureReference in subscriptions)
            {
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                switch (structureReference.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.Any:
                        structureEvents.AllEvents = new EmptyType();
                        break;
                    case SdmxStructureEnumType.AgencyScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.AgencyScheme.Add(eventType);
                        break;
                    case SdmxStructureEnumType.AttachmentConstraint:
                        eventType = new VersionableObjectEventType();
                        structureEvents.AttachmentConstraint.Add(eventType);

                        break;
                    case SdmxStructureEnumType.CategoryScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.CategoryScheme.Add(eventType);

                        break;
                    case SdmxStructureEnumType.CodeList:
                        eventType = new VersionableObjectEventType();
                        structureEvents.Codelist.Add(eventType);

                        break;
                    case SdmxStructureEnumType.ConceptScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.ConceptScheme.Add(eventType);

                        break;
                    case SdmxStructureEnumType.ContentConstraint:
                        eventType = new VersionableObjectEventType();
                        structureEvents.ContentConstraint.Add(eventType);

                        break;
                    case SdmxStructureEnumType.DataConsumerScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.DataConsmerScheme.Add(eventType);

                        break;
                    case SdmxStructureEnumType.Dataflow:
                        eventType = new VersionableObjectEventType();
                        structureEvents.Dataflow.Add(eventType);

                        break;
                    case SdmxStructureEnumType.DataProviderScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.DataProviderScheme.Add(eventType);

                        break;
                    case SdmxStructureEnumType.HierarchicalCodelist:
                        eventType = new VersionableObjectEventType();
                        structureEvents.HierarchicalCodelist.Add(eventType);

                        break;
                    case SdmxStructureEnumType.Dsd:
                        eventType = new VersionableObjectEventType();
                        structureEvents.KeyFamily.Add(eventType);

                        break;
                    case SdmxStructureEnumType.MetadataFlow:
                        eventType = new VersionableObjectEventType();
                        structureEvents.Metadataflow.Add(eventType);

                        break;
                    case SdmxStructureEnumType.Msd:
                        eventType = new VersionableObjectEventType();
                        structureEvents.MetadataStructureDefinition.Add(eventType);

                        break;
                    case SdmxStructureEnumType.OrganisationUnitScheme:
                        eventType = new VersionableObjectEventType();
                        structureEvents.OrganisationUnitScheme.Add(eventType);

                        break;
                    case SdmxStructureEnumType.Process:
                        eventType = new VersionableObjectEventType();
                        structureEvents.Process.Add(eventType);

                        break;
                    case SdmxStructureEnumType.ProvisionAgreement:
                        eventType = new VersionableObjectEventType();
                        structureEvents.ProvisionAgreement.Add(eventType);

                        break;
                    case SdmxStructureEnumType.ReportingTaxonomy:
                        eventType = new VersionableObjectEventType();
                        structureEvents.ReportingTaxonomy.Add(eventType);

                        break;
                    case SdmxStructureEnumType.StructureSet:
                        eventType = new VersionableObjectEventType();
                        structureEvents.StructureSet.Add(eventType);

                        break;
                    case SdmxStructureEnumType.Categorisation:
                        var identifiableObjectEventType = new IdentifiableObjectEventType();
                        structureEvents.Categorisation.Add(identifiableObjectEventType);
                        SetIdentifiableObjectEventInfo(identifiableObjectEventType, maintainableReference);
                        break;
                }

                if (eventType != null)
                {
                    SetVersionObjectEventInfo(eventType, maintainableReference);
                }
            }
        }

        /// <summary>
        /// Sets the identifiable object event info.
        /// </summary>
        /// <param name="identifiableObjectEventType">
        /// The identifiable Object Event Type.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainable Ref.
        /// </param>
        private static void SetIdentifiableObjectEventInfo(
            IdentifiableObjectEventType identifiableObjectEventType, IMaintainableRefObject maintainableRef)
        {
            string value = maintainableRef.MaintainableId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                identifiableObjectEventType.ID = maintainableRef.MaintainableId;
            }
            else
            {
                identifiableObjectEventType.All = new EmptyType();
            }
        }

        /// <summary>
        /// Sets the maintainable event info.
        /// </summary>
        /// <param name="maintEventType">
        /// The maintainable event type.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainableRef.
        /// </param>
        private static void SetMaintainableEventInfo(
            MaintainableEventType maintEventType, IMaintainableRefObject maintainableRef)
        {
            var maintQueryType = new MaintainableQueryType();
            maintEventType.Ref = maintQueryType;
            string value = maintainableRef.AgencyId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                maintQueryType.agencyID = maintainableRef.AgencyId;
            }

            string value1 = maintainableRef.MaintainableId;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                maintQueryType.id = maintainableRef.MaintainableId;
            }

            string value2 = maintainableRef.Version;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                maintQueryType.version = maintainableRef.Version;
            }
        }

        /// <summary>
        /// Sets the version object event info.
        /// </summary>
        /// <param name="vob">
        /// The identifiableObjectEventType.
        /// </param>
        /// <param name="xref">
        /// The maintainableRef.
        /// </param>
        private static void SetVersionObjectEventInfo(VersionableObjectEventType vob, IMaintainableRefObject xref)
        {
            if (ObjectUtil.ValidOneString(xref.MaintainableId, xref.Version))
            {
                string value = xref.MaintainableId;
                vob.ID = !string.IsNullOrWhiteSpace(value)
                             ? xref.MaintainableId
                             : SubscriptionBeanConstants.Wildcard;

                string value1 = xref.Version;
                vob.Version = !string.IsNullOrWhiteSpace(value1) ? xref.Version : SubscriptionBeanConstants.Wildcard;
            }
            else
            {
                vob.All = new EmptyType();
            }
        }

        /// <summary>
        /// The build data subscription.
        /// </summary>
        /// <param name="eventSelector">
        /// The event selector.
        /// </param>
        /// <param name="subscriptions">
        /// The subscriptions.
        /// </param>
        private void BuildDataSubscription(
            EventSelectorType eventSelector, IEnumerable<IStructureReference> subscriptions)
        {
            var dataRegistrationEventsType = new DataRegistrationEventsType();
            eventSelector.DataRegistrationEvents.Add(dataRegistrationEventsType);

            foreach (IStructureReference structureReference in subscriptions)
            {
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                switch (structureReference.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.Any:
                        {
                            dataRegistrationEventsType.AllEvents = new EmptyType();
                        }

                        break;
                    case SdmxStructureEnumType.Category:
                        {
                            var catRef = new CategoryReferenceType();
                            dataRegistrationEventsType.Category.Add(catRef);
                            var categoryRefType = new CategoryRefType();
                            catRef.SetTypedRef(categoryRefType);
                            this.SetReference(categoryRefType, structureReference);
                        }

                        break;
                    case SdmxStructureEnumType.ProvisionAgreement:
                        {
                            var provisionRef = new ProvisionAgreementReferenceType();
                            var provisionAgreementRefType = new ProvisionAgreementRefType();
                            provisionRef.SetTypedRef(provisionAgreementRefType);
                            dataRegistrationEventsType.ProvisionAgreement.Add(provisionRef);
                            this.SetReference(provisionAgreementRefType, structureReference);
                        }

                        break;
                    case SdmxStructureEnumType.DataProvider:
                        {
                            var providerRef = new DataProviderReferenceType();
                            dataRegistrationEventsType.DataProvider.Add(providerRef);
                            var refType = new DataProviderRefType();
                            providerRef.SetTypedRef(refType);

                            this.SetReference(refType, structureReference);
                        }

                        break;
                    case SdmxStructureEnumType.Dataflow:
                        {
                            var eventType = new MaintainableEventType();
                            dataRegistrationEventsType.DataflowReference.Add(eventType);
                            SetMaintainableEventInfo(eventType, maintainableReference);
                        }

                        break;
                    case SdmxStructureEnumType.Dsd:
                        {
                            var eventType = new MaintainableEventType();
                            dataRegistrationEventsType.KeyFamilyReference.Add(eventType);
                            SetMaintainableEventInfo(eventType, maintainableReference);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// The build metadata subscription.
        /// </summary>
        /// <param name="eventSelector">
        /// The event selector.
        /// </param>
        /// <param name="subscriptions">
        /// The subscriptions.
        /// </param>
        private void BuildMetadataSubscription(
            EventSelectorType eventSelector, IEnumerable<IStructureReference> subscriptions)
        {
            var metadatadataRegistrationEventsType = new MetadataRegistrationEventsType();
            eventSelector.MetadataRegistrationEvents.Add(new MetadataRegistrationEventsType());

            foreach (IStructureReference structureReference in subscriptions)
            {
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                switch (structureReference.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.Any:
                        metadatadataRegistrationEventsType.AllEvents = new EmptyType();
                        break;
                    case SdmxStructureEnumType.Category:
                        var catRef = new CategoryReferenceType();
                        metadatadataRegistrationEventsType.Category.Add(catRef);
                        this.SetReference(catRef.SetTypedRef(new CategoryRefType()), structureReference);
                        break;
                    case SdmxStructureEnumType.ProvisionAgreement:
                        var provisionRef = new ProvisionAgreementReferenceType();
                        metadatadataRegistrationEventsType.ProvisionAgreement.Add(provisionRef);
                        this.SetReference(provisionRef.SetTypedRef(new ProvisionAgreementRefType()), structureReference);
                        break;
                    case SdmxStructureEnumType.DataProvider:
                        var providerRef = new DataProviderReferenceType();
                        metadatadataRegistrationEventsType.DataProvider.Add(providerRef);
                        this.SetReference(providerRef.SetTypedRef(new DataProviderRefType()), structureReference);

                        break;
                    case SdmxStructureEnumType.MetadataFlow:
                        {
                            var maintainableEventType = new MaintainableEventType();
                            metadatadataRegistrationEventsType.MetadataflowReference.Add(maintainableEventType);
                            SetMaintainableEventInfo(maintainableEventType, maintainableReference);
                        }

                        break;
                    case SdmxStructureEnumType.Msd:
                        {
                            var maintainableEventType = new MaintainableEventType();
                            metadatadataRegistrationEventsType.MetadataStructureDefinitionReference.Add(
                                maintainableEventType);
                            SetMaintainableEventInfo(maintainableEventType, maintainableReference);
                        }

                        break;
                }
            }
        }

        #endregion
    }
}
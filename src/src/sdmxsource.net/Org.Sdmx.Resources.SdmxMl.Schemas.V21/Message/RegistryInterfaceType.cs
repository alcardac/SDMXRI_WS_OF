// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInterfaceType.cs" company="EUROSTAT">
//   EUPL
// </copyright>
// <summary>
//   The registry interface type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    using Xml.Schema.Linq;

    /// <summary>
    /// The registry interface type.
    /// </summary>
    public partial class RegistryInterfaceType
    {
        #region Static Fields

        /// <summary>
        /// The footer type.
        /// </summary>
        private static readonly Type _footerType = typeof(Footer.Footer);

        /// <summary>
        /// The _footer element name.
        /// </summary>
        private static readonly XName _footerXName = XName.Get(
            "Footer", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer");

        /// <summary>
        /// The header type.
        /// </summary>
        private static readonly Type _headerType = typeof(BasicHeaderType);

        /// <summary>
        /// The _header element name.
        /// </summary>
        private static readonly XName _headerXName = XName.Get(
            "Header", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary>
        /// The local element dictionary.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Dictionary<XName, Type> _localElementDictionary = new Dictionary<XName, Type>();

        /// <summary> NotifyRegistryEvent element name </summary>
        private static readonly XName _nameNotifyRegistryEvent = XName.Get(
            "NotifyRegistryEvent", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> QueryRegistrationRequest element name </summary>
        private static readonly XName _nameQueryRegistrationRequest = XName.Get(
            "QueryRegistrationRequest", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> QueryRegistrationResponse element name </summary>
        private static readonly XName _nameQueryRegistrationResponse = XName.Get(
            "QueryRegistrationResponse", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> QuerySubscriptionRequest element name </summary>
        private static readonly XName _nameQuerySubscriptionRequest = XName.Get(
            "QuerySubscriptionRequest", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> QuerySubscriptionResponse element name </summary>
        private static readonly XName _nameQuerySubscriptionResponse = XName.Get(
            "QuerySubscriptionResponse", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitRegistrationsRequest element name </summary>
        private static readonly XName _nameSubmitRegistrationsRequest = XName.Get(
            "SubmitRegistrationsRequest", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitRegistrationsResponse element name </summary>
        private static readonly XName _nameSubmitRegistrationsResponse = XName.Get(
            "SubmitRegistrationsResponse", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitStructureRequest element name </summary>
        private static readonly XName _nameSubmitStructureRequest = XName.Get(
            "SubmitStructureRequest", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitStructureResponse element name </summary>
        private static readonly XName _nameSubmitStructureResponse = XName.Get(
            "SubmitStructureResponse", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitSubscriptionsRequest element name </summary>
        private static readonly XName _nameSubmitSubscriptionsRequest = XName.Get(
            "SubmitSubscriptionsRequest", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> SubmitSubscriptionsResponse element name </summary>
        private static readonly XName _nameSubmitSubscriptionsResponse = XName.Get(
            "SubmitSubscriptionsResponse", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary> NotifyRegistryEvent type </summary>
        private static readonly Type _typeNotifyRegistryEvent = typeof(Registry.NotifyRegistryEventType);

        /// <summary> QueryRegistrationRequest type </summary>
        private static readonly Type _typeQueryRegistrationRequest = typeof(Registry.QueryRegistrationRequestType);

        /// <summary> QueryRegistrationResponse type </summary>
        private static readonly Type _typeQueryRegistrationResponse = typeof(Registry.QueryRegistrationResponseType);

        /// <summary> QuerySubscriptionRequest type </summary>
        private static readonly Type _typeQuerySubscriptionRequest = typeof(Registry.QuerySubscriptionRequestType);

        /// <summary> QuerySubscriptionResponse type </summary>
        private static readonly Type _typeQuerySubscriptionResponse = typeof(Registry.QuerySubscriptionResponseType);

        /// <summary> SubmitRegistrationsRequest type </summary>
        private static readonly Type _typeSubmitRegistrationsRequest = typeof(Registry.SubmitRegistrationsRequestType);

        /// <summary> SubmitRegistrationsResponse type </summary>
        private static readonly Type _typeSubmitRegistrationsResponse = typeof(Registry.SubmitRegistrationsResponseType);

        /// <summary> SubmitStructureRequest type </summary>
        private static readonly Type _typeSubmitStructureRequest = typeof(Registry.SubmitStructureRequestType);

        /// <summary> SubmitStructureResponse type </summary>
        private static readonly Type _typeSubmitStructureResponse = typeof(Registry.SubmitStructureResponseType);

        /// <summary> SubmitSubscriptionsRequest type </summary>
        private static readonly Type _typeSubmitSubscriptionsRequest = typeof(Registry.SubmitSubscriptionsRequestType);

        /// <summary> SubmitSubscriptionsResponse type </summary>
        private static readonly Type _typeSubmitSubscriptionsResponse = typeof(Registry.SubmitSubscriptionsResponseType);

        /// <summary>
        /// The validation states.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static FSM validationStates;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="RegistryInterfaceType"/> class.
        /// </summary>
        static RegistryInterfaceType()
        {
            BuildElementDictionary();
            InitFsm();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public new BasicHeaderType Header
        {
            get
            {
                XElement x = this.GetElement(_headerXName);
                return (BasicHeaderType)x;
            }

            set
            {
                this.SetElement(_headerXName, value);
            }
        }

        /// <summary> Gets or sets the NotifyRegistryEvent. NotifyRegistryEvent is sent by the registry services to subscribers, to notify them of specific registration and change events. Basic information about the event, such as the object that triggered it, the time of the event, the action that took place, and the subscription that triggered the notification are always sent. Optionally, the details of the changed object may also be provided. </summary>
        public Registry.NotifyRegistryEventType NotifyRegistryEvent
        {
            get
            {
                XElement x = this.GetElement(_nameNotifyRegistryEvent);
                return (Registry.NotifyRegistryEventType)x;
            }

            set
            {
                this.SetElement(_nameNotifyRegistryEvent, value);
            }
        }

        /// <summary> Gets or sets the QueryRegistrationRequest. QueryRegistrationRequest is used to query the contents of a registry for data sets and metadata sets. It specifies whether the result set should include metadata sets, data sets, or both. The search can be characterized by providing constraints including reference periods, data regions, and data keys. </summary>
        public Registry.QueryRegistrationRequestType QueryRegistrationRequest
        {
            get
            {
                XElement x = this.GetElement(_nameQueryRegistrationRequest);
                return (Registry.QueryRegistrationRequestType)x;
            }

            set
            {
                this.SetElement(_nameQueryRegistrationRequest, value);
            }
        }

        /// <summary> Gets or sets the QueryRegistrationResponse. QueryRegistrationResponse is sent as a response to any query of the contents of a registry. The result set contains a set of links to data and/or metadata If the result set is null, or there is some other problem with the query, then appropriate error messages and statuses will be returned. </summary>
        public Registry.QueryRegistrationResponseType QueryRegistrationResponse
        {
            get
            {
                XElement x = this.GetElement(_nameQueryRegistrationResponse);
                return (Registry.QueryRegistrationResponseType)x;
            }

            set
            {
                this.SetElement(_nameQueryRegistrationResponse, value);
            }
        }

        /// <summary> Gets or sets the QuerySubscriptionRequest. QuerySubscriptionRequest is used to query the registry for the subscriptions of a given organisation. </summary>
        public Registry.QuerySubscriptionRequestType QuerySubscriptionRequest
        {
            get
            {
                XElement x = this.GetElement(_nameQuerySubscriptionRequest);
                return (Registry.QuerySubscriptionRequestType)x;
            }

            set
            {
                this.SetElement(_nameQuerySubscriptionRequest, value);
            }
        }

        /// <summary> Gets or sets the QuerySubscriptionResponse. QuerySubscriptionResponse is sent as a response to a subscription query. If the query is successful, the details of all subscriptions for the requested organisation are sent. </summary>
        public Registry.QuerySubscriptionResponseType QuerySubscriptionResponse
        {
            get
            {
                XElement x = this.GetElement(_nameQuerySubscriptionResponse);
                return (Registry.QuerySubscriptionResponseType)x;
            }

            set
            {
                this.SetElement(_nameQuerySubscriptionResponse, value);
            }
        }

        /// <summary> Gets or sets the SubmitRegistrationsRequest. SubmitRegistrationsRequest is sent to the registry by an agency or data/metadata provider to request one or more registrations for a data set or metadata set. The data source to be registered must be accessible to the registry services at an indicated URL, so that it can be processed by those services. </summary>
        public Registry.SubmitRegistrationsRequestType SubmitRegistrationsRequest
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitRegistrationsRequest);
                return (Registry.SubmitRegistrationsRequestType)x;
            }

            set
            {
                this.SetElement(_nameSubmitRegistrationsRequest, value);
            }
        }

        /// <summary> Gets or sets the SubmitRegistrationsResponse. SubmitRegistrationsResponse is sent to the agency or data/metadata provider in response to a submit registrations request. It indicates the success or failure of each registration request, and contains any error messages generated by the registration service. </summary>
        public Registry.SubmitRegistrationsResponseType SubmitRegistrationsResponse
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitRegistrationsResponse);
                return (Registry.SubmitRegistrationsResponseType)x;
            }

            set
            {
                this.SetElement(_nameSubmitRegistrationsResponse, value);
            }
        }

        /// <summary> Gets or sets the SubmitStructureRequest. SubmitStructureRequest is used to submit structure definitions to the repository. The structure resources (key families, agencies, concepts and concept schemes, code lists, etc.) to be submitted may be communicated in-line or be supplied in a referenced SDMX-ML Structure messages external to the registry. A response will indicate status and contain any relevant error information. </summary>
        public Registry.SubmitStructureRequestType SubmitStructureRequest
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitStructureRequest);
                return (Registry.SubmitStructureRequestType)x;
            }

            set
            {
                this.SetElement(_nameSubmitStructureRequest, value);
            }
        }

        /// <summary> Gets or sets the SubmitStructureResponse. SubmitStructureResponse is returned by the registry when a structure submission request is received. It indicates the status of the submission, and carries any error messages which are generated, if relevant. </summary>
        public Registry.SubmitStructureResponseType SubmitStructureResponse
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitStructureResponse);
                return (Registry.SubmitStructureResponseType)x;
            }

            set
            {
                this.SetElement(_nameSubmitStructureResponse, value);
            }
        }

        /// <summary> Gets or sets the SubmitSubscriptionsRequest. SubmitSubscriptionsRequest contains one or more requests submitted to the registry to subscribe to registration and change events for specific registry resources. </summary>
        public Registry.SubmitSubscriptionsRequestType SubmitSubscriptionsRequest
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitSubscriptionsRequest);
                return (Registry.SubmitSubscriptionsRequestType)x;
            }

            set
            {
                this.SetElement(_nameSubmitSubscriptionsRequest, value);
            }
        }

        /// <summary> Gets or sets the SubmitSubscriptionsResponse. SubmitSubscriptionsResponse is the response to a submit subscriptions request. It contains information which describes the success or failure of each subscription request, providing any error messages in the event of failure. If successful, it returns the registry URN of the subscription, and the subscriber-assigned ID. </summary>
        public Registry.SubmitSubscriptionsResponseType SubmitSubscriptionsResponse
        {
            get
            {
                XElement x = this.GetElement(_nameSubmitSubscriptionsResponse);
                return (Registry.SubmitSubscriptionsResponseType)x;
            }

            set
            {
                this.SetElement(_nameSubmitSubscriptionsResponse, value);
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets the local elements dictionary.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return _localElementDictionary;
            }
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// Gets validation states.
        /// </summary>
        /// <returns>
        /// The validation states.
        /// </returns>
        FSM IXMetaData.GetValidationStates()
        {
            return validationStates;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build element dictionary.
        /// </summary>
        private static void BuildElementDictionary()
        {
            _localElementDictionary.Add(_headerXName, _headerType);
            _localElementDictionary.Add(_nameSubmitRegistrationsRequest, _typeSubmitRegistrationsRequest);
            _localElementDictionary.Add(_nameSubmitRegistrationsResponse, _typeSubmitRegistrationsResponse);
            _localElementDictionary.Add(_nameQueryRegistrationRequest, _typeQueryRegistrationRequest);
            _localElementDictionary.Add(_nameQueryRegistrationResponse, _typeQueryRegistrationResponse);
            _localElementDictionary.Add(_nameSubmitStructureRequest, _typeSubmitStructureRequest);
            _localElementDictionary.Add(_nameSubmitStructureResponse, _typeSubmitStructureResponse);
            _localElementDictionary.Add(_nameSubmitSubscriptionsRequest, _typeSubmitSubscriptionsRequest);
            _localElementDictionary.Add(_nameSubmitSubscriptionsResponse, _typeSubmitSubscriptionsResponse);
            _localElementDictionary.Add(_nameQuerySubscriptionRequest, _typeQuerySubscriptionRequest);
            _localElementDictionary.Add(_nameQuerySubscriptionResponse, _typeQuerySubscriptionResponse);
            _localElementDictionary.Add(_nameNotifyRegistryEvent, _typeNotifyRegistryEvent);

            _localElementDictionary.Add(_footerXName, _footerType);
        }

        /// <summary>
        /// Initialize the <c>FSM</c>
        /// </summary>
        private static void InitFsm()
        {
            // TODO check if the transition numbers just need to be in order or need to have specific values ?!? 
            var transitions = new Dictionary<int, Transitions>
                {
                    { 1, new Transitions(new SingleTransition(_headerXName, 2)) },
                    {
                        2,
                        new Transitions(
                        new SingleTransition(_nameSubmitRegistrationsRequest, 4),
                        new SingleTransition(_nameSubmitRegistrationsResponse, 4),
                        new SingleTransition(_nameQueryRegistrationRequest, 4), 
                    new SingleTransition(_nameQueryRegistrationResponse, 4), 
                    new SingleTransition(_nameSubmitStructureRequest, 4), 
                    new SingleTransition(_nameSubmitStructureResponse, 4), 
                    new SingleTransition(_nameSubmitSubscriptionsRequest, 4), 
                    new SingleTransition(_nameSubmitSubscriptionsResponse, 4), 
                    new SingleTransition(_nameQuerySubscriptionRequest, 4), 
                    new SingleTransition(_nameQuerySubscriptionResponse, 4), 
                    new SingleTransition(_nameNotifyRegistryEvent, 4)) }, 
                    {
                        4, 
                        new Transitions(
                        new SingleTransition(_footerXName, 6), 
                        new SingleTransition(
                        new WildCard("##targetNamespace", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message"), 2))
                    }
                };
            validationStates = new FSM(1, new Set<int>(new[] { 2, 4, 6 }), transitions);
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryMessageType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    #endregion
    /// <summary>
    ///     Contains a list of all the Registry Message Types as found under the RegistryInterfaceDocument XML node
    ///     <p />
    ///     This Enum offers the ability to retrieve the underlying <b>ARTIFACT_TYPE</b> and XML node names
    /// </summary>
    public enum RegistryMessageEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        // REQUESTS

        /// <summary>
        ///     The submit structure request.
        /// </summary>
        SubmitStructureRequest, 

        /// <summary>
        ///     The submit provision request.
        /// </summary>
        SubmitProvisionRequest, 

        /// <summary>
        ///     The submit registration request.
        /// </summary>
        SubmitRegistrationRequest, 

        /// <summary>
        ///     The submit subscription request.
        /// </summary>
        SubmitSubscriptionRequest, 

        /// <summary>
        ///     The query structure request.
        /// </summary>
        QueryStructureRequest, 

        /// <summary>
        ///     The query provision request.
        /// </summary>
        QueryProvisionRequest, 

        /// <summary>
        ///     The query registration request.
        /// </summary>
        QueryRegistrationRequest, 

        // RESPONSES

        /// <summary>
        ///     The submit structure response.
        /// </summary>
        SubmitStructureResponse, 

        /// <summary>
        ///     The submit provision response.
        /// </summary>
        SubmitProvisionResponse, 

        /// <summary>
        ///     The submit registration response.
        /// </summary>
        SubmitRegistrationResponse, 

        /// <summary>
        ///     The submit subscription response.
        /// </summary>
        SubmitSubscriptionResponse, 

        /// <summary>
        ///     The query provision response.
        /// </summary>
        QueryProvisionResponse, 

        /// <summary>
        ///     The query registration response.
        /// </summary>
        QueryRegistrationResponse, 

        /// <summary>
        ///     The query structure response.
        /// </summary>
        QueryStructureResponse, 

        // NOTIFICATIONS

        /// <summary>
        ///     The notify registry event.
        /// </summary>
        NotifyRegistryEvent
    }

    /// <summary>
    ///     Contains a list of all the Registry Message Types as found under the RegistryInterfaceDocument XML node
    ///     This Enum offers the ability to retrieve the underlying <b>ArtifactType</b> and XML node names
    /// </summary>
    public class RegistryMessageType : BaseConstantType<RegistryMessageEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<RegistryMessageEnumType, RegistryMessageType> _instances =
            new Dictionary<RegistryMessageEnumType, RegistryMessageType>
                {
                    {
                        RegistryMessageEnumType
                        .SubmitStructureRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitStructureRequest, 
                        ArtifactType.Structure, 
                        "SubmitStructureRequest", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitProvisionRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitProvisionRequest, 
                        ArtifactType.Provision, 
                        "SubmitProvisioningRequest", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitRegistrationRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitRegistrationRequest, 
                        ArtifactType.Registration, 
                        "SubmitRegistrationRequest", 
                        "SubmitRegistrationsRequest")
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitSubscriptionRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitSubscriptionRequest, 
                        ArtifactType.Subscription, 
                        "SubmitSubscriptionRequest", 
                        "SubmitSubscriptionsRequest")
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryStructureRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryStructureRequest, 
                        ArtifactType.Structure, 
                        "QueryStructureRequest", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryProvisionRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryProvisionRequest, 
                        ArtifactType.Provision, 
                        "QueryProvisioningRequest", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryRegistrationRequest, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryRegistrationRequest, 
                        ArtifactType.Registration, 
                        "QueryRegistrationRequest", 
                        null)
                    }, 
                    {
                        // RESPONSES
                        RegistryMessageEnumType
                        .SubmitStructureResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitStructureResponse, 
                        ArtifactType.Structure, 
                        "SubmitStructureResponse", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitProvisionResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitProvisionResponse, 
                        ArtifactType.Provision, 
                        "SubmitProvisioningResponse", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitRegistrationResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitRegistrationResponse, 
                        ArtifactType.Registration, 
                        "SubmitRegistrationResponse", 
                        "SubmitRegistrationsResponse")
                    }, 
                    {
                        RegistryMessageEnumType
                        .SubmitSubscriptionResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .SubmitSubscriptionResponse, 
                        ArtifactType.Subscription, 
                        "SubmitSubscriptionResponse", 
                        "SubmitSubscriptionsResponse")
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryProvisionResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryProvisionResponse, 
                        ArtifactType.Provision, 
                        "QueryProvisioningResponse", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryRegistrationResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryRegistrationResponse, 
                        ArtifactType.Registration, 
                        "QueryRegistrationResponse", 
                        null)
                    }, 
                    {
                        RegistryMessageEnumType
                        .QueryStructureResponse, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .QueryStructureResponse, 
                        ArtifactType.Structure, 
                        "QueryStructureResponse", 
                        null)
                    }, 
                    {
                        // NOTIFICATIONS
                        RegistryMessageEnumType
                        .NotifyRegistryEvent, 
                        new RegistryMessageType(
                        RegistryMessageEnumType
                        .NotifyRegistryEvent, 
                        ArtifactType.Notification, 
                        "NotifyRegistryEvent", 
                        null)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _artifact type.
        /// </summary>
        private readonly ArtifactType _artifactType;

        /// <summary>
        ///     The _enum type.
        /// </summary>
        private readonly RegistryMessageEnumType _enumType;

        /// <summary>
        ///     The _two point one type.
        /// </summary>
        private readonly string _twoPointOneType;

        /// <summary>
        ///     The _type.
        /// </summary>
        private readonly string _type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMessageType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="artifactType">
        /// The artifact type.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="twoPointOneType">
        /// The two point one type.
        /// </param>
        private RegistryMessageType(
            RegistryMessageEnumType enumType, ArtifactType artifactType, string type, string twoPointOneType)
            : base(enumType)
        {
            this._enumType = enumType;
            this._artifactType = artifactType;
            this._type = type;
            this._twoPointOneType = twoPointOneType;
            if (this.TwoPointOneType == null)
            {
                this._twoPointOneType = type;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<RegistryMessageType> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///     Gets the artifact type.
        /// </summary>
        public ArtifactType ArtifactType
        {
            get
            {
                return this._artifactType;
            }
        }

        /// <summary>
        ///     Gets the two point one type.
        /// </summary>
        public string TwoPointOneType
        {
            get
            {
                return this._twoPointOneType;
            }
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <remarks>This corresponds to the Java method <c>getType()</c>. Renamed to RegistryType to conform to <c>FxCop</c> rule <c>CA1721</c></remarks>
        public string RegistryType
        {
            get
            {
                return this._type;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="RegistryMessageType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="RegistryMessageType"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static RegistryMessageType GetFromEnum(RegistryMessageEnumType enumType)
        {
            RegistryMessageType output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Gets the REGISTRY_MESSAGE_TYPE for the SDMX Node
        /// </summary>
        /// <param name="xmlNode">
        /// The xml Node.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryMessageType"/> .
        /// </returns>
        public static RegistryMessageType GetMessageType(string xmlNode)
        {
            foreach (RegistryMessageType currentType in Values)
            {
                if (currentType.RegistryType.Equals(xmlNode) || currentType.TwoPointOneType.Equals(xmlNode))
                {
                    return currentType;
                }
            }

            throw new ArgumentException("Unknown node :" + xmlNode);
        }

        /// <summary>
        ///     Gets a value indicating whether the the message type is a notification event (NotifyRegistryEvent):
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsNotifyEvent()
        {
            return this._enumType == RegistryMessageEnumType.NotifyRegistryEvent;
        }

        /// <summary>
        ///     Gets a value indicating whether the the message type is a query request (one of the following):
        ///     <ul>
        ///         <li>QUERY_PROVISION_REQUEST</li>
        ///         <li>QUERY_REGISTRATION_REQUEST</li>
        ///         <li>QUERY_STRUCTURE_REQUEST</li>
        ///     </ul>
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsQueryRequest()
        {
            switch (this._enumType)
            {
                case RegistryMessageEnumType.QueryProvisionRequest:
                case RegistryMessageEnumType.QueryRegistrationRequest:
                case RegistryMessageEnumType.QueryStructureRequest:
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating whether the the message type is a query response (one of the following):
        ///     <ul>
        ///         <li>QUERY_PROVISION_RESPONSE</li>
        ///         <li>QUERY_REGISTRATION_RESPONSE</li>
        ///         <li>QUERY_STRUCTURE_RESPONSE</li>
        ///         <li>QUERY_STRUCTURE_RESPONSE</li>
        ///     </ul>
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsQueryResponse()
        {
            switch (this._enumType)
            {
                case RegistryMessageEnumType.QueryProvisionResponse:
                case RegistryMessageEnumType.QueryRegistrationResponse:
                case RegistryMessageEnumType.QueryStructureResponse:
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     The is submission request.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsSubmissionRequest()
        {
            switch (this._enumType)
            {
                case RegistryMessageEnumType.SubmitStructureRequest:
                case RegistryMessageEnumType.SubmitProvisionRequest:
                case RegistryMessageEnumType.SubmitRegistrationRequest:
                case RegistryMessageEnumType.SubmitSubscriptionRequest:
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating whether the the message type is a submission response (one of the following):
        ///     <ul>
        ///         <li>SUBMIT_STRUCTURE_RESPONSE</li>
        ///         <li>SUBMIT_PROVISION_RESPONSE</li>
        ///         <li>SUBMIT_REGISTRATION_RESPONSE</li>
        ///         <li>SUBMIT_SUBSCRIPTION_RESPONSE</li>
        ///     </ul>
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsSubmissionResponse()
        {
            switch (this._enumType)
            {
                case RegistryMessageEnumType.SubmitStructureResponse:
                case RegistryMessageEnumType.SubmitProvisionResponse:
                case RegistryMessageEnumType.SubmitRegistrationResponse:
                case RegistryMessageEnumType.SubmitSubscriptionResponse:
                    return true;
            }

            return false;
        }

        #endregion
    }
}
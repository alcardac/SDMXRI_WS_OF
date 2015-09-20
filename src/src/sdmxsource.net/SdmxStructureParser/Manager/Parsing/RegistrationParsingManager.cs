// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration parsing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.XmlHelper;

    using ActionTypeConstants = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.ActionTypeConstants;
    using MessageType = Org.Sdmxsource.Sdmx.Api.Constants.MessageType;
    using QueryResultType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.QueryResultType;
    using RegistrationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.RegistrationType;
    using RegistryInterface = Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.RegistryInterface;

    /// <summary>
    ///     The registration parsing manager implementation.
    /// </summary>
    public class RegistrationParsingManager : BaseParsingManager, IRegistrationParsingManager
    {
        #region Static Fields

        /// <summary>
        ///     The action info.
        /// </summary>
        private static readonly DatasetAction _actionInfo = DatasetAction.GetFromEnum(DatasetActionEnumType.Information);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegistrationParsingManager" /> class.
        /// </summary>
        public RegistrationParsingManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        public RegistrationParsingManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        public RegistrationParsingManager(SdmxSchemaEnumType sdmxSchema, MessageEnumType messageType)
            : base(sdmxSchema, messageType)
        {
        }

        #endregion

        // TODO Handle error responses (AS IN REGISTRY ERROR DOCUMENT AS THIS CAUSES THE PARSING TO FAIL)
        /* @SuppressWarnings("deprecation")*/
        #region Public Methods and Operators

        /// <summary>
        /// Process a SMDX document to retrieve the Registrations, these can either be in
        ///     a QueryRegistrationResponse message or inside a SubmitRegistrationRequest message
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the XML file
        /// </param>
        /// <returns>
        /// the Registrations from <paramref name="dataLocation"/>
        /// </returns>
        public virtual IList<IRegistrationInformation> ParseRegXML(IReadableDataLocation dataLocation)
        {
            SdmxSchemaEnumType schemaVersion = this.GetSchemaVersion(dataLocation);

            ////XMLParser.ValidateXml(dataLocation, schemaVersion);
            MessageEnumType messageType = this.GetMessageType(dataLocation);
            if (messageType != MessageEnumType.Error && messageType != MessageEnumType.RegistryInterface)
            {
                throw new Exception(
                    "Unexpected Document found, expecting RegistryInterface containing Registrations, received "
                    + MessageType.GetFromEnum(messageType).NodeName);
            }

            using (Stream stream = dataLocation.InputStream)
            {
                using (XmlReader reader = XMLParser.CreateSdmxMlReader(stream, schemaVersion))
                {
                    if (messageType == MessageEnumType.Error)
                    {
                        Error errorDocument = Error.Load(reader);

                        /* foreach */
                        foreach (CodedStatusMessageType csmt in errorDocument.ErrorMessage)
                        {
                            if (csmt.code != null && csmt.code.Equals("100"))
                            {
                                return new List<IRegistrationInformation>();
                            }

                            // IMPORTATNT THIS SHOULD THROW A BETTER EXCEPTION - AND SHOULD BE REFACTORED INTO A ERROR PARSING CLASS
                            throw new Exception(csmt.Text[0].TypedValue);
                        }

                        return new List<IRegistrationInformation>();
                    }

                    return ProcessRegistrationResponse(schemaVersion, reader);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process registration response and return the list of <see cref="IRegistrationInformation"/>.
        /// </summary>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// the list of <see cref="IRegistrationInformation"/>.
        /// </returns>
        private static IList<IRegistrationInformation> ProcessRegistrationResponse(
            SdmxSchemaEnumType schemaVersion, XmlReader reader)
        {
            IList<IRegistrationInformation> returnList = new List<IRegistrationInformation>();
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    RegistryInterface rid = RegistryInterface.Load(reader);
                    if (rid.QueryRegistrationResponse != null && rid.QueryRegistrationResponse.QueryResult != null)
                    {
                        /* foreach */
                        foreach (QueryResultType resultType in
                            rid.QueryRegistrationResponse.QueryResult)
                        {
                            if (resultType.DataResult != null)
                            {
                                IRegistrationObject registration =
                                    new RegistrationObjectCore(
                                        resultType.DataResult.ProvisionAgreementRef, resultType.DataResult.Datasource);
                                returnList.Add(new RegistrationInformationImpl(_actionInfo, registration));
                            }
                        }
                    }

                    if (rid.SubmitRegistrationRequest != null && rid.SubmitRegistrationRequest.Registration != null)
                    {
                        /* foreach */
                        foreach (RegistrationType registrationType in
                            rid.SubmitRegistrationRequest.Registration)
                        {
                            IRegistrationObject registration0 = new RegistrationObjectCore(registrationType);
                            var action1 = DatasetActionEnumType.Append;
                            switch (registrationType.Action)
                            {
                                case ActionTypeConstants.Append:
                                    action1 = DatasetActionEnumType.Append;
                                    break;
                                case ActionTypeConstants.Delete:
                                    action1 = DatasetActionEnumType.Delete;
                                    break;
                                case ActionTypeConstants.Replace:
                                    action1 = DatasetActionEnumType.Replace;
                                    break;
                            }

                            returnList.Add(
                                new RegistrationInformationImpl(DatasetAction.GetFromEnum(action1), registration0));
                        }
                    }

                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface rid21 =
                        Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface.Load(reader);
                    RegistryInterfaceType rit = rid21.Content;
                    if (rit.SubmitRegistrationsRequest != null
                        && rit.SubmitRegistrationsRequest.RegistrationRequest != null)
                    {
                        /* foreach */
                        foreach (RegistrationRequestType rt in rit.SubmitRegistrationsRequest.RegistrationRequest)
                        {
                            var action2 = DatasetActionEnumType.Append;
                            switch (rt.action)
                            {
                                case Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.ActionTypeConstants.Append:
                                    action2 = DatasetActionEnumType.Append;
                                    break;
                                case Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.ActionTypeConstants.Delete:
                                    action2 = DatasetActionEnumType.Delete;
                                    break;
                                case Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.ActionTypeConstants.Replace:
                                    action2 = DatasetActionEnumType.Replace;
                                    break;
                            }

                            if (action2 == DatasetActionEnumType.Delete || action2 == DatasetActionEnumType.Replace)
                            {
                                string value = rt.Registration.id;
                                if (!(!string.IsNullOrWhiteSpace(value)))
                                {
                                    throw new ArgumentException(
                                        "Registration submissions with REPLACE or DELETE actions must contain an id identifing the Registration to perform this action on");
                                }
                            }

                            IRegistrationObject registration3 = new RegistrationObjectCore(rt.Registration);
                            returnList.Add(
                                new RegistrationInformationImpl(DatasetAction.GetFromEnum(action2), registration3));
                        }
                    }

                    if (rit.QueryRegistrationResponse != null && rit.QueryRegistrationResponse.QueryResult != null)
                    {
                        /* foreach */
                        foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryResultType queryResult in
                            rit.QueryRegistrationResponse.QueryResult)
                        {
                            if (queryResult.DataResult != null && queryResult.DataResult.Registration != null)
                            {
                                IRegistrationObject registration4 =
                                    new RegistrationObjectCore(queryResult.DataResult.Registration);
                                returnList.Add(new RegistrationInformationImpl(_actionInfo, registration4));
                            }

                            if (queryResult.MetadataResult != null && queryResult.MetadataResult.Registration != null)
                            {
                                IRegistrationObject registration5 =
                                    new RegistrationObjectCore(queryResult.MetadataResult.Registration);
                                returnList.Add(new RegistrationInformationImpl(_actionInfo, registration5));
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentException("Schema version unsupported: " + schemaVersion);
            }

            return returnList;
        }

        #endregion
    }
}
// -----------------------------------------------------------------------
// <copyright file="SdmxStructureParserFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Factory
{
    using System;
    using System.IO;
    using System.Xml;
    using log4net;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Log;
    using Org.Sdmxsource.XmlHelper;

    class SdmxStructureParserFactory : IStructureParserFactory
    {
        #region Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxStructureParserFactory));

        /// <summary>
        ///     If set to a value other than <see cref="MessageEnumType.Null" />, this message type will be assumed for all input
        /// </summary>
        private readonly MessageEnumType _messageType;

        /// <summary>
        ///     If set to a value other than <see cref="SdmxSchemaEnumType.Null" />, this SDMX schema will be assumed for all input
        /// </summary>
        private readonly SdmxSchemaEnumType _sdmxSchema;

        /// <summary>
        /// The registry message type,If set to a value other than <see cref="RegistryMessageEnumType.Null" />, this registry message type will be assumed for all input
        /// </summary>
        private readonly RegistryMessageEnumType _registryMessageType;

        /// <summary>
        ///     The sdmx objects builder.
        /// </summary>
        private readonly ISdmxObjectsBuilder _sdmxBeansBuilder;

        /// <summary>
        ///     The provision parsing manager.
        /// </summary>
        private readonly IProvisionParsingManager _provisionParsingManager;

        /// <summary>
        ///     The registration parsing manager.
        /// </summary>
        private readonly IRegistrationParsingManager _registrationParsingManager;

        /// <summary>
        ///     The subscription parsing manager.
        /// </summary>
        private readonly ISubscriptionParsingManager _subscriptionParsingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureParserFactory"/> class.
        /// </summary>
        public SdmxStructureParserFactory() : this(MessageEnumType.Null, SdmxSchemaEnumType.Null, RegistryMessageEnumType.Null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureParserFactory"/> class.
        /// </summary>
        /// <param name="sdmxBeansBuilder">The SDMX beans builder.</param>
        /// <param name="provisionParsingManager">The provision parsing manager.</param>
        /// <param name="registrationParsingManager">The registration parsing manager.</param>
        /// <param name="subscriptionParsingManager">The subscription parsing manager.</param>
        public SdmxStructureParserFactory(ISdmxObjectsBuilder sdmxBeansBuilder, IProvisionParsingManager provisionParsingManager, IRegistrationParsingManager registrationParsingManager, ISubscriptionParsingManager subscriptionParsingManager) : this(MessageEnumType.Null, SdmxSchemaEnumType.Null, RegistryMessageEnumType.Null, sdmxBeansBuilder, provisionParsingManager, registrationParsingManager, subscriptionParsingManager)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureParserFactory" /> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="sdmxSchema">The SDMX schema.</param>
        /// <param name="registryMessageType">Type of the registry message.</param>
        /// <param name="sdmxBeansBuilder">The SDMX beans builder.</param>
        /// <param name="provisionParsingManager">The provision parsing manager.</param>
        /// <param name="registrationParsingManager">The registration parsing manager.</param>
        /// <param name="subscriptionParsingManager">The subscription parsing manager.</param>
        public SdmxStructureParserFactory(MessageEnumType messageType, SdmxSchemaEnumType sdmxSchema, RegistryMessageEnumType registryMessageType, ISdmxObjectsBuilder sdmxBeansBuilder, IProvisionParsingManager provisionParsingManager, IRegistrationParsingManager registrationParsingManager, ISubscriptionParsingManager subscriptionParsingManager)
        {
            this._messageType = messageType;
            this._sdmxSchema = sdmxSchema;
            this._registryMessageType = registryMessageType;
            this._sdmxBeansBuilder = sdmxBeansBuilder ?? new SdmxObjectsBuilder();
            this._provisionParsingManager = provisionParsingManager ?? new ProvisionParsingManager();
            this._registrationParsingManager = registrationParsingManager ?? new RegistrationParsingManager();
            this._subscriptionParsingManager = subscriptionParsingManager ?? new SubscriptionParsingManager();
        }

        #endregion

        /// <summary>
        /// Returns the <see cref="ISdmxObjects"/>.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public ISdmxObjects GetSdmxObjects(IReadableDataLocation dataLocation)
        {
            SdmxSchema schemaVersion = SdmxSchema.GetFromEnum( this._sdmxSchema == SdmxSchemaEnumType.Null ? SdmxMessageUtil.GetSchemaVersion(dataLocation) : this._sdmxSchema);

            _log.Debug("Schema Version : " + schemaVersion.EnumType);

            if (schemaVersion.EnumType == SdmxSchemaEnumType.Edi) return null;

            MessageEnumType messageType = this._messageType == MessageEnumType.Null ? SdmxMessageUtil.GetMessageType(dataLocation) : this._messageType;
            _log.Debug("Message type: " + messageType);

            /*
            /// VALIDATION IS PERFORMED ON PARSING.
            if (schemaVersion.IsXmlFormat())
            {
                _log.Debug("Validate XML");
                XMLParser.ValidateXml(dataLocation, schemaVersion.EnumType);
                LoggingUtil.Debug(_log, "XML VALID");
            }
            */

            RegistryMessageType registryMessage = RegistryMessageType.GetFromEnum(RegistryMessageEnumType.Null);
            if (messageType == MessageEnumType.RegistryInterface)
            {
                registryMessage = RegistryMessageType.GetFromEnum(this._registryMessageType == RegistryMessageEnumType.Null ? SdmxMessageUtil.GetRegistryMessageType(dataLocation) : this._registryMessageType);
                return ProcessRegistryInterfaceDocument(dataLocation, registryMessage, schemaVersion.EnumType);
            }

            try
            {
                using (Stream inputStream = dataLocation.InputStream)
                {
                    using (XmlReader reader = XMLParser.CreateSdmxMlReader(inputStream, schemaVersion))
                    {
                        return ParseSdmxStructureMessage(reader, schemaVersion.EnumType, messageType);
                    }
                }
            }
            catch (XmlException e)
            {
                throw new SdmxException("Error while attempting to process SDMX-ML Structure file", e);
            }
            catch (IOException e)
            {
                throw new SdmxException("Error while attempting to process SDMX-ML Structure file", e);
            }
        }

        /// <summary>
        /// parses the sdmx structure message.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version type.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        private ISdmxObjects ParseSdmxStructureMessage(XmlReader reader, SdmxSchemaEnumType schemaVersion, MessageEnumType messageType)
        {
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionOne:
                    //Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure sdV1 = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure.Factory.Parse(stream);
                    Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure sdV1 = Org.Sdmx.Resources.SdmxMl.Schemas.V10.MessageFactory.Load<Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure, Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.StructureType>(reader);
                    return _sdmxBeansBuilder.Build(sdV1);
                case SdmxSchemaEnumType.VersionTwo:
                    switch (messageType)
                    {
                        case MessageEnumType.Structure:
                            Structure sdV2 = MessageFactory.Load<Structure, Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.StructureType>(reader);
                            //Structure sdV2 = Structure.Parse(stream);//.Factory.Parse(stream);
                            return _sdmxBeansBuilder.Build(sdV2);
                        case MessageEnumType.RegistryInterface:
                            RegistryInterface rid = RegistryInterface.Load(reader);
                            return _sdmxBeansBuilder.Build(rid);
                        default:
                            throw new ArgumentException("StructureParsingManagerImpl can not parse document '" + messageType + "' was expecting Structure document or RegistryInterface document");
                    }
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    switch (messageType)
                    {
                        case MessageEnumType.Structure:
                            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure sdV2_1 = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MessageFactory.Load<Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure, Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.StructureType>(reader);
                            //Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure sdV2_1 = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure.Parse();//.Factory.Parse(stream);
                            return _sdmxBeansBuilder.Build(sdV2_1);
                        case MessageEnumType.Error:
                            return new SdmxObjectsImpl();
                        default:
                            throw new ArgumentException("StructureParsingManagerImpl can not parse document '" + messageType + "' was expecting Structure document or Error document");
                    }
            }
            return null;
        }

        /// <summary>
        /// Processes the registry interface document.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <param name="registryMessage">
        /// The registry message type.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version type.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        private ISdmxObjects ProcessRegistryInterfaceDocument(IReadableDataLocation dataLocation, RegistryMessageType registryMessage, SdmxSchemaEnumType schemaVersion)
        {
            ArtifactType artifactType = registryMessage.ArtifactType;
            ISdmxObjects returnBeans = new SdmxObjectsImpl();
            switch (artifactType)
            {
                case ArtifactType.Provision:
                    returnBeans.AddIdentifiables(_provisionParsingManager.ParseXML(dataLocation));
                    break;
                case ArtifactType.Registration:
                    foreach (IRegistrationInformation regInfo in _registrationParsingManager.ParseRegXML(dataLocation))
                        returnBeans.AddRegistration(regInfo.Registration);
                    break;
                case ArtifactType.Structure:
                        switch (schemaVersion)
                        {
                            case SdmxSchemaEnumType.VersionTwo:
                                RegistryInterface rid;
                                using (Stream inputStream = dataLocation.InputStream)
                                {
                                    using (XmlReader reader = XMLParser.CreateSdmxMlReader(inputStream, schemaVersion))
                                    {
                                        rid = RegistryInterface.Load(reader);
                                    }
                                }
                                return _sdmxBeansBuilder.Build(rid);
                            case SdmxSchemaEnumType.VersionTwoPointOne:
                                Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface rid2_1;
                                using (Stream inputStream = dataLocation.InputStream)
                                {
                                    using (XmlReader reader = XMLParser.CreateSdmxMlReader(inputStream, schemaVersion))
                                    {
                                        rid2_1 = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface.Load(reader);
                                    }
                                }
                                return _sdmxBeansBuilder.Build(rid2_1);
                            default:
                                throw new ArgumentException("Schema version unsupported for RegistryInterfaceDocument: " + schemaVersion.ToString());
                        }

                case ArtifactType.Subscription:
                    returnBeans.AddIdentifiables(_subscriptionParsingManager.ParseSubscriptionXML(dataLocation));
                    break;
                default:
                    throw new SdmxNotImplementedException("StructureParsingManager does not support message of type : " + registryMessage.EnumType.ToString());
            }
            return returnBeans;
        }
    }
}

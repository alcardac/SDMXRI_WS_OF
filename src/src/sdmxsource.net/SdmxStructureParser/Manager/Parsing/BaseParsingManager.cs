// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Base parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Log;

    /// <summary>
    ///     Base parsing manager.
    /// </summary>
    public abstract class BaseParsingManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BaseParsingManager));

        #endregion

        #region Fields

        /// <summary>
        ///     If set to a value other than <see cref="MessageEnumType.Null" />, this message type will be assumed for all input
        /// </summary>
        private readonly MessageEnumType _messageType;

        /// <summary>
        ///     If set to a value other than <see cref="SdmxSchemaEnumType.Null" />, this SDMX schema will be assumed for all input
        /// </summary>
        private readonly SdmxSchemaEnumType _sdmxSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseParsingManager" /> class.
        /// </summary>
        protected BaseParsingManager()
            : this(SdmxSchemaEnumType.Null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        protected BaseParsingManager(SdmxSchemaEnumType sdmxSchema)
            : this(sdmxSchema, MessageEnumType.Null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        /// <param name="messageType">
        /// The message Type.
        /// </param>
        protected BaseParsingManager(SdmxSchemaEnumType sdmxSchema, MessageEnumType messageType)
        {
            this._sdmxSchema = sdmxSchema;
            this._messageType = messageType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets message type, if set from the constructor otherwise from the <paramref name="dataLocation"/>
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <returns>
        /// The <see cref="MessageEnumType"/>.
        /// </returns>
        protected virtual MessageEnumType GetMessageType(IReadableDataLocation dataLocation)
        {
            SdmxSchemaEnumType sdmxSchemaEnumType = this.GetSchemaVersion(dataLocation);
            bool readDataLocation = this._messageType == MessageEnumType.Null
                                    && sdmxSchemaEnumType != SdmxSchemaEnumType.Edi;
            MessageEnumType messageType = readDataLocation
                                              ? SdmxMessageUtil.GetMessageType(dataLocation)
                                              : this._messageType;

            if (readDataLocation)
            {
                LoggingUtil.Debug(_log, "Message Type Determined to be : " + messageType);
            }
            else
            {
                LoggingUtil.Debug(_log, "Message Type specified on CTOR : " + messageType);
            }

            return messageType;
        }

        /// <summary>
        /// Gets schema version, if set from the constructor otherwise from the <paramref name="dataLocation"/>
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxSchemaEnumType"/>.
        /// </returns>
        protected virtual SdmxSchemaEnumType GetSchemaVersion(IReadableDataLocation dataLocation)
        {
            SdmxSchemaEnumType schemaVersion = this._sdmxSchema == SdmxSchemaEnumType.Null
                                                   ? SdmxMessageUtil.GetSchemaVersion(dataLocation)
                                                   : this._sdmxSchema;

            if (this._sdmxSchema == SdmxSchemaEnumType.Null)
            {
                LoggingUtil.Debug(_log, "Schema Version Determined to be : " + schemaVersion);
            }
            else
            {
                LoggingUtil.Debug(_log, "Using Schema Version specified on CTOR : " + schemaVersion);
            }

            return schemaVersion;
        }

        #endregion
    }
}
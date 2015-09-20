// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract response builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Xml;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// The abstract response builder.
    /// </summary>
    /// <typeparam name="T">
    /// The response type
    /// </typeparam>
    public abstract class AbstractResponseBuilder<T> : IBuilder<IList<T>, IReadableDataLocation>
    {
        #region Properties

        /// <summary>
        ///     Gets the expected message type.
        /// </summary>
        internal abstract RegistryMessageType ExpectedMessageType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build from <paramref name="buildFrom"/> the list of <typeparamref name="T"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The data location.
        /// </param>
        /// <returns>
        /// list of <typeparamref name="T"/> from <paramref name="buildFrom"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// <paramref name="buildFrom"/>
        ///     Not XML or parsing error
        /// </exception>
        /// <exception cref="UnsupportedException">
        /// Unsupported SDMX version
        /// </exception>
        public virtual IList<T> Build(IReadableDataLocation buildFrom)
        {
            // 2. Validate the response is XML 
            if (!XmlUtil.IsXML(buildFrom))
            {
                using (var reader = new StreamReader(buildFrom.InputStream))
                {
                    throw new SdmxSemmanticException(ExceptionCode.ParseErrorNotXml, reader.ReadLine());
                }
            }

            // 3. Validate it is valid SDMX-ML 
            SdmxSchemaEnumType schemaVersion = SdmxMessageUtil.GetSchemaVersion(buildFrom);

            ////XMLParser.ValidateXml(buildFrom, schemaVersion);
            RegistryMessageEnumType message = SdmxMessageUtil.GetRegistryMessageType(buildFrom);
            if (message != this.ExpectedMessageType.EnumType)
            {
                string type = RegistryMessageType.GetFromEnum(message).RegistryType;
                throw new SdmxSemmanticException("Expected '" + this.ExpectedMessageType.RegistryType + "' message, got " + type);
            }

            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    RegistryInterface rid;
                    using (buildFrom)
                    {
                        using (Stream inputStream = buildFrom.InputStream)
                        {
                            using (XmlReader reader = XMLParser.CreateSdmxMlReader(inputStream, schemaVersion))
                            {
                                rid = RegistryInterface.Load(reader);
                            }
                        }
                    }

                    return this.BuildInternal(rid);
            }

            throw new SdmxNotImplementedException(schemaVersion);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build <typeparamref name="T"/> list from the specified <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Interface message
        /// </param>
        /// <returns>
        /// The <typeparamref name="T"/> list from the specified <paramref name="registryInterface"/>
        /// </returns>
        internal abstract IList<T> BuildInternal(RegistryInterface registryInterface);

        #endregion
    }
}
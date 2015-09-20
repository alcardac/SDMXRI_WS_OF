// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoBase.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The writer/reader base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Engine
{
    using System;
    using System.Globalization;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The writer/reader base.
    /// </summary>
    public abstract class IoBase
    {
        #region Static Fields

        /// <summary>
        ///     The default sdmx schema version
        /// </summary>
        private static readonly SdmxSchema _defaultSchema = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);

        #endregion

        #region Fields

        /// <summary>
        ///     The SDMX namespaces.
        /// </summary>
        private readonly SdmxNamespaces _namespaces;

        /// <summary>
        ///     The target SDMX schema
        /// </summary>
        private readonly SdmxSchema _targetSchema = _defaultSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IoBase"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The SDMX schema version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="schema"/> is null
        /// </exception>
        protected IoBase(SdmxNamespaces namespaces, SdmxSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }

            this._targetSchema = schema;
            this._namespaces = namespaces ?? CreateNamespaces(schema.EnumType);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the <see cref="TargetSchema" /> is SDMX v2.1
        /// </summary>
        protected bool IsTwoPointOne
        {
            get
            {
                return this._targetSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne;
            }
        }

        /// <summary>
        ///     Gets the SDMX namespaces.
        /// </summary>
        protected SdmxNamespaces Namespaces
        {
            get
            {
                return this._namespaces;
            }
        }

        /// <summary>
        ///     Gets the target SDMX schema
        /// </summary>
        protected SdmxSchema TargetSchema
        {
            get
            {
                return this._targetSchema;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create namespaces for the specified <paramref name="version"/>
        /// </summary>
        /// <param name="version">
        /// The SDMX schema version.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxNamespaces"/>.
        /// </returns>
        protected static SdmxNamespaces CreateNamespaces(SdmxSchemaEnumType version)
        {
            var namespaces = new SdmxNamespaces
                                 {
                                     Xsi =
                                         new NamespacePrefixPair(
                                         XmlConstants.XmlSchemaNS, XmlConstants.XmlSchemaPrefix)
                                 };
            switch (version)
            {
                case SdmxSchemaEnumType.VersionOne:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs10, PrefixConstants.Common);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs10, PrefixConstants.Message);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs10, PrefixConstants.Generic);
                    namespaces.Registry = new NamespacePrefixPair(SdmxConstants.RegistryNs10, PrefixConstants.Registry);
                    namespaces.Structure = new NamespacePrefixPair(
                        SdmxConstants.StructureNs10, PrefixConstants.Structure);
                    namespaces.Query = new NamespacePrefixPair(SdmxConstants.QueryNs10, PrefixConstants.Query);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs10);
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs20, PrefixConstants.Common);
                    ////namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs20, PrefixConstants.Message);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs20, string.Empty);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs20, PrefixConstants.Generic);
                    namespaces.Registry = new NamespacePrefixPair(SdmxConstants.RegistryNs20, PrefixConstants.Registry);
                    namespaces.Structure = new NamespacePrefixPair(
                        SdmxConstants.StructureNs20, PrefixConstants.Structure);
                    namespaces.Query = new NamespacePrefixPair(SdmxConstants.QueryNs20, PrefixConstants.Query);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs20);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs21, PrefixConstants.Common);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs21, PrefixConstants.Message);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs21, PrefixConstants.Generic);
                    namespaces.Registry = new NamespacePrefixPair(SdmxConstants.RegistryNs21, PrefixConstants.Registry);
                    namespaces.Structure = new NamespacePrefixPair(
                        SdmxConstants.StructureNs21, PrefixConstants.Structure);
                    namespaces.Query = new NamespacePrefixPair(SdmxConstants.QueryNs21, PrefixConstants.Query);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs21);
                    break;
            }

            return namespaces;
        }

        #endregion
    }
}
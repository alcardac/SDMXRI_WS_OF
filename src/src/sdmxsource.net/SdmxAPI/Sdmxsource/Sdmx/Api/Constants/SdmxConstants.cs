// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxConstants.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    ///     The sdmx constants.
    /// </summary>
    public static class SdmxConstants
    {
        #region Constants

        /// <summary>
        ///     The common ns 10.
        /// </summary>
        public const string CommonNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/common";

        /// <summary>
        ///     The common ns 20.
        /// </summary>
        public const string CommonNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common";

        /// <summary>
        ///     The common ns 20 registry.
        /// </summary>
        public const string CommonNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/common";

        /// <summary>
        ///     The common ns 21.
        /// </summary>
        public const string CommonNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common";

        /// <summary>
        ///     The compact data root node.
        /// </summary>
        public const string CompactDataRootNode = "CompactData";

        /// <summary>
        ///     The compact ns 10.
        /// </summary>
        public const string CompactNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/compact";

        /// <summary>
        ///     The compact ns 20.
        /// </summary>
        public const string CompactNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/compact";

        /// <summary>
        ///     The compact ns 20 registry.
        /// </summary>
        public const string CompactNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/compact";

        /// <summary>
        ///     The compact ns 21.
        /// </summary>
        public const string CompactNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structurespecific";

        /// <summary>
        ///     The cross ns 10.
        /// </summary>
        public const string CrossNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/cross";

        /// <summary>
        ///     The cross ns 20.
        /// </summary>
        public const string CrossNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/cross";

        /// <summary>
        ///     The cross ns 20 registry.
        /// </summary>
        public const string CrossNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/cross";

        /// <summary>
        ///     The cross sectional data root node.
        /// </summary>
        public const string CrossSectionalDataRootNode = "CrossSectionalData";

        /// <summary>
        ///     The generic data root node.
        /// </summary>
        public const string GenericDataRootNode = "GenericData";

        /// <summary>
        ///     The generic ns 10.
        /// </summary>
        public const string GenericNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/generic";

        /// <summary>
        ///     The generic ns 20.
        /// </summary>
        public const string GenericNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic";

        /// <summary>
        ///     The generic ns 20 registry.
        /// </summary>
        public const string GenericNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/generic";

        /// <summary>
        ///     The generic ns 21.
        /// </summary>
        public const string GenericNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic";

        /// <summary>
        ///     The generic time series data root node.
        /// </summary>
        public const string GenericTimeseriesDataRootNode = "GenericTimeSeriesData";

        /// <summary>
        ///     The generic metadata ns 10.
        /// </summary>
        public const string GenericMetadataNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/genericmetadata";

        /// <summary>
        ///     The generic metadata ns 20.
        /// </summary>
        public const string GenericMetadataNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata";

        /// <summary>
        ///     The generic metadata ns 20 registry.
        /// </summary>
        public const string GenericMetadataNs20Registry =
            "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/genericmetadata";

        /// <summary>
        ///     The generic metadata ns 21.
        /// </summary>
        public const string GenericMetadataNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/genericmetadata";

        /// <summary>
        ///     The message group root node.
        /// </summary>
        public const string MessageGroupRootNode = "MessageGroup";

        /// <summary>
        ///     The message ns 10.
        /// </summary>
        public const string MessageNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/message";

        /// <summary>
        ///     The message ns 20.
        /// </summary>
        public const string MessageNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message";

        /// <summary>
        ///     The message ns 20 registry.
        /// </summary>
        public const string MessageNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/message";

        /// <summary>
        ///     The message ns 21.
        /// </summary>
        public const string MessageNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message";

        /// <summary>
        ///     The metadata report ns 10.
        /// </summary>
        public const string MetadatareportNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/metadatareport";

        /// <summary>
        ///     The metadata report ns 20.
        /// </summary>
        public const string MetadatareportNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/metadatareport";

        /// <summary>
        ///     The metadata report ns 20 registry.
        /// </summary>
        public const string MetadatareportNs20Registry =
            "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/metadatareport";

        /// <summary>
        ///     The metadata report ns 21.
        /// </summary>
        public const string MetadatareportNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadatareport";

        /// <summary>
        ///     The missing data value.
        /// </summary>
        public const string MissingDataValue = "NaN";

        /// <summary>
        ///     The query message root node.
        /// </summary>
        public const string QueryMessageRootNode = "QueryMessage";

        /// <summary>
        ///     The query ns 10.
        /// </summary>
        public const string QueryNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/query";

        /// <summary>
        ///     The query ns 20.
        /// </summary>
        public const string QueryNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query";

        /// <summary>
        ///     The query ns 20 registry.
        /// </summary>
        public const string QueryNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/query";

        /// <summary>
        ///     The query ns 21.
        /// </summary>
        public const string QueryNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query";

        /// <summary>
        ///     The registry interface root node.
        /// </summary>
        public const string RegistryInterfaceRootNode = "RegistryInterface";

        /// <summary>
        ///     The registry ns 10.
        /// </summary>
        public const string RegistryNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/registry";

        /// <summary>
        ///     The registry ns 20.
        /// </summary>
        public const string RegistryNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry";

        /// <summary>
        ///     The registry ns 20 registry.
        /// </summary>
        public const string RegistryNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/registry";

        /// <summary>
        ///     The registry ns 21.
        /// </summary>
        public const string RegistryNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry";

        /// <summary>
        ///     The structure ns 10.
        /// </summary>
        public const string StructureNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/structure";

        /// <summary>
        ///     The structure ns 20.
        /// </summary>
        public const string StructureNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure";

        /// <summary>
        ///     The structure ns 20 registry.
        /// </summary>
        public const string StructureNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/structure";

        /// <summary>
        ///     The structure ns 21.
        /// </summary>
        public const string StructureNs21 = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure";

        /// <summary>
        ///     The structure root node.
        /// </summary>
        public const string StructureRootNode = "Structure";

        /// <summary>
        ///     The structure specific data.
        /// </summary>
        public const string StructureSpecificData = "StructureSpecificData";

        /// <summary>
        ///     The structure specific ns 21.
        /// </summary>
        public const string StructureSpecificNs21 =
            "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific";

        // DATA ROOT NODES

        // 2.1

        /// <summary>
        ///     The structure specific time series data.
        /// </summary>
        public const string StructureSpecificTimeSeriesData = "StructureSpecificTimeSeriesData";

        /// <summary>
        ///     The utility data root node.
        /// </summary>
        public const string UtilityDataRootNode = "UtilityData";

        /// <summary>
        ///     The utility ns 10.
        /// </summary>
        public const string UtilityNs10 = "http://www.SDMX.org/resources/SDMXML/schemas/v1_0/utility";

        /// <summary>
        ///     The utility ns 20.
        /// </summary>
        public const string UtilityNs20 = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/utility";

        /// <summary>
        ///     The utility ns 20 registry.
        /// </summary>
        public const string UtilityNs20Registry = "http://metadatatechnology.com/sdmx/registry/schemas/v2_0/utility";

        /// <summary>
        ///     The XML NS.
        /// </summary>
        public const string Xmlns = "http://www.w3.org/XML/1998/namespace";

        /// <summary>
        /// The rest wild card
        /// </summary>
        public const string RestWildcard = "*";

        #endregion

        #region Static Fields

        /// <summary>
        /// The version 1 to version 2 map.
        /// </summary>
        private static readonly IDictionary<string, string> _version1ToVersion2Map = IntitV1ToV2Mapping();

        /// <summary>
        /// The version 2 to version 1 map.
        /// </summary>
        private static readonly IDictionary<string, string> _version2ToVersion1Map = IntitV2ToV1Mapping();

        /// <summary>
        /// The namespace to schema
        /// </summary>
        private static readonly IDictionary<string, string> _namespaceToSchema = IntitNamespaceToSchemaMapping();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the namespaces v 1.
        /// </summary>
        public static IList<string> NamespacesV1
        {
            get
            {
                IList<string> allNamespaces = new List<string>();
                allNamespaces.Add(GenericNs10);
                allNamespaces.Add(MessageNs10);
                allNamespaces.Add(CompactNs10);
                allNamespaces.Add(UtilityNs10);
                allNamespaces.Add(CrossNs10);
                allNamespaces.Add(CommonNs10);
                allNamespaces.Add(QueryNs10);
                allNamespaces.Add(RegistryNs10);
                allNamespaces.Add(StructureNs10);
                allNamespaces.Add(GenericMetadataNs10);
                allNamespaces.Add(MetadatareportNs10);
                return allNamespaces;
            }
        }

        /// <summary>
        ///     Gets the namespaces v 2.
        /// </summary>
        public static IList<string> NamespacesV2
        {
            get
            {
                IList<string> allNamespaces = new List<string>();
                allNamespaces.Add(GenericNs20);
                allNamespaces.Add(MessageNs20);
                allNamespaces.Add(CompactNs20);
                allNamespaces.Add(UtilityNs20);
                allNamespaces.Add(CrossNs20);
                allNamespaces.Add(CommonNs20);
                allNamespaces.Add(QueryNs20);
                allNamespaces.Add(RegistryNs20);
                allNamespaces.Add(StructureNs20);
                allNamespaces.Add(GenericMetadataNs20);
                allNamespaces.Add(MetadatareportNs20);
                return allNamespaces;
            }
        }

        /// <summary>
        ///     Gets the namespaces v 21.
        /// </summary>
        public static IList<string> NamespacesV21
        {
            get
            {
                IList<string> allNamespaces = new List<string>();
                allNamespaces.Add(GenericNs20);
                allNamespaces.Add(MessageNs20);
                allNamespaces.Add(CompactNs20);
                allNamespaces.Add(UtilityNs20);
                allNamespaces.Add(CrossNs20);
                allNamespaces.Add(CommonNs20);
                allNamespaces.Add(QueryNs20);
                allNamespaces.Add(RegistryNs20);
                allNamespaces.Add(StructureNs20);
                allNamespaces.Add(GenericMetadataNs20);
                allNamespaces.Add(MetadatareportNs20);
                return allNamespaces;
            }
        }

        /*
         * Gets a list of all the SDMX version 2.1 namespaces 
         */

        /// <summary>
        ///     Gets the namespaces v 2 registry.
        /// </summary>
        public static IList<string> NamespacesV2Registry
        {
            get
            {
                IList<string> allNamespaces = new List<string>();
                allNamespaces.Add(GenericNs20Registry);
                allNamespaces.Add(MessageNs20Registry);
                allNamespaces.Add(CompactNs20Registry);
                allNamespaces.Add(UtilityNs20Registry);
                allNamespaces.Add(CrossNs20Registry);
                allNamespaces.Add(CommonNs20Registry);
                allNamespaces.Add(QueryNs20Registry);
                allNamespaces.Add(RegistryNs20Registry);
                allNamespaces.Add(StructureNs20Registry);
                allNamespaces.Add(GenericMetadataNs20Registry);
                allNamespaces.Add(MetadatareportNs20Registry);
                return allNamespaces;
            }
        }

        /// <summary>
        ///     Gets a copy of the map containing the version 1.0 namespaces mapped to the 2.1 namespaces
        /// </summary>
        /// <value> </value>
        public static IDictionary<string, string> V1ToV2Map
        {
            get
            {
                return new Dictionary<string, string>(_version1ToVersion2Map);
            }
        }

        /// <summary>
        ///     Gets a copy of the map containing the version 2.0 namespaces mapped to the 2.1 namespaces
        /// </summary>
        /// <value> </value>
        public static IDictionary<string, string> V2ToV1Map
        {
            get
            {
                return new Dictionary<string, string>(_version2ToVersion1Map);
            }
        }

        /// <summary>
        ///     Gets a copy of the namespaces mapped to the schemas
        /// </summary>
        /// <value> </value>
        public static IDictionary<string, string> NamespaceToSchema
        {
            get
            {
                return new Dictionary<string, string>(_namespaceToSchema);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the name of the schema for a schema URI
        /// </summary>
        /// <param name="schemaUri">schemaUri string
        /// </param>
        /// <returns>
        /// Schema file name, or null if the URI does not map to anything
        /// </returns>
        public static string GetSchemaName(string schemaUri)
        {
            string ns;
            return NamespaceToSchema.TryGetValue(schemaUri, out ns) ? ns : null;
        }

        /// <summary>
        /// returns true if the namespace is a valid SDMX namespace
        /// </summary>
        /// <param name="uri">Uri string.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        public static bool IsKnownNamespace(string uri)
        {
            return NamespacesV1.Contains(uri) || NamespacesV21.Contains(uri) || NamespacesV21.Contains(uri)
                   || NamespacesV2Registry.Contains(uri);
        }

        /// <summary>
        /// returns true if the namespace is a valid SDMX namespace
        /// </summary>
        /// <param name="uri">Uri string.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        public static bool IsKnownNamespace(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return IsKnownNamespace(uri.OriginalString);
        }

        /// <summary>
        /// Gets the schema version.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The SDMX schema
        /// </returns>
        public static SdmxSchema GetSchemaVersion(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return GetSchemaVersion(uri.ToString());
        }

        /// <summary>
        /// Returns the version of the Schema for the given URI
        /// </summary>
        /// <param name="uri">The URI string
        /// </param>
        /// <returns>
        /// The SDMX schema
        /// </returns>
        public static SdmxSchema GetSchemaVersion(string uri)
        {
            if (NamespacesV1.Contains(uri))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionOne);
            }
            if (NamespacesV2.Contains(uri))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);
            }
            if (NamespacesV21.Contains(uri))
            {
                return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);
            }

            throw new ArgumentException("Unknown schema URI: " + uri);
        }

        /// <summary>
        /// Takes a namespace and returns the equivalent in the given SWSDMX_SCHEMA
        /// </summary>
        /// <param name="inputNamespace">The namespace.
        /// </param>
        /// <param name="schemaVersion">Schema version
        /// </param>
        /// <returns>
        /// The namespace <see cref="Uri"/>
        /// </returns>
        public static Uri SwitchNamespaceUri(string inputNamespace, SdmxSchemaEnumType schemaVersion)
        {
            if (NamespacesV1.Contains(inputNamespace))
            {
                return new Uri(SwitchNamespace(SdmxSchemaEnumType.VersionOne, schemaVersion, inputNamespace));
            }

            if (NamespacesV21.Contains(inputNamespace))
            {
                return new Uri(SwitchNamespace(SdmxSchemaEnumType.VersionTwo, schemaVersion, inputNamespace));
            }

            if (NamespacesV21.Contains(inputNamespace))
            {
                return new Uri(SwitchNamespace(SdmxSchemaEnumType.VersionTwoPointOne, schemaVersion, inputNamespace));
            }

            throw new ArgumentException("unknown namespace : " + inputNamespace); // TODO Exception
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize SDMX v 1 to v 2 mapping.
        /// </summary>
        /// <returns> The SDMX v 1 to v 2 mapping </returns>
        private static IDictionary<string, string> IntitV1ToV2Mapping()
        {
            IDictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add(GenericNs10, GenericNs20);
            retVal.Add(MessageNs10, MessageNs20);
            retVal.Add(CompactNs10, CompactNs20);
            retVal.Add(UtilityNs10, UtilityNs20);
            retVal.Add(CrossNs10, CrossNs20);
            retVal.Add(CommonNs10, CommonNs20);
            retVal.Add(QueryNs10, QueryNs20);
            retVal.Add(RegistryNs10, RegistryNs20);
            retVal.Add(StructureNs10, StructureNs20);
            retVal.Add(GenericMetadataNs10, GenericMetadataNs20);
            retVal.Add(MetadatareportNs10, MetadatareportNs20);

            return retVal;
        }

        /// <summary>
        ///     The initialize version 2 to version 1 mapping.
        /// </summary>
        /// <returns>
        ///     The v2 to v1 mapping
        /// </returns>
        private static IDictionary<string, string> IntitV2ToV1Mapping()
        {
            IDictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add(GenericNs20, GenericNs10);
            retVal.Add(MessageNs20, MessageNs10);
            retVal.Add(CompactNs20, CompactNs10);
            retVal.Add(UtilityNs20, UtilityNs10);
            retVal.Add(CrossNs20, CrossNs10);
            retVal.Add(CommonNs20, CommonNs10);
            retVal.Add(QueryNs20, QueryNs10);
            retVal.Add(RegistryNs20, RegistryNs10);
            retVal.Add(StructureNs20, StructureNs10);
            retVal.Add(GenericMetadataNs20, GenericMetadataNs10);
            retVal.Add(MetadatareportNs20, MetadatareportNs10);
            return retVal;
        }

        /// <summary>
        ///     The initialize namespace to schema mapping.
        /// </summary>
        /// <returns>
        ///     The v2 to v1 mapping
        /// </returns>
        private static IDictionary<string, string> IntitNamespaceToSchemaMapping()
        {
            IDictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add(CommonNs10, "SDMXCommon.xsd");
            retVal.Add(CommonNs20, "SDMXCommon.xsd");
            retVal.Add(CommonNs21, "SDMXCommon.xsd");
            retVal.Add(MessageNs10, "SDMXMessage.xsd");
            retVal.Add(MessageNs20, "SDMXMessage.xsd");
            retVal.Add(MessageNs21, "SDMXMessage.xsd");
            retVal.Add(GenericNs10, "SDMXGenericData.xsd");
            retVal.Add(GenericNs20, "SDMXGenericData.xsd");
            retVal.Add(GenericNs21, "SDMXDataGeneric.xsd");
            retVal.Add(CompactNs10, "SDMXCompactData.xsd");
            retVal.Add(CompactNs20, "SDMXCompactData.xsd");
            retVal.Add(CompactNs21, "SDMXDataStructureSpecific.xsd");
            retVal.Add(UtilityNs10, "SDMXUtilityData.xsd");
            retVal.Add(UtilityNs20, "SDMXUtilityData.xsd");
            retVal.Add(CrossNs10, "SDMXCrossSectionalData.xsd");
            retVal.Add(CrossNs20, "SDMXCrossSectionalData.xsd");
            retVal.Add(QueryNs10, "SDMXQuery.xsd");
            retVal.Add(QueryNs20, "SDMXQuery.xsd");
            retVal.Add(QueryNs21, "SDMXQuery.xsd");
            retVal.Add(RegistryNs10, "SDMXRegistry.xsd");
            retVal.Add(RegistryNs20, "SDMXRegistry.xsd");
            retVal.Add(RegistryNs21, "SDMXRegistry.xsd");
            retVal.Add(StructureNs10, "SDMXStructure.xsd");
            retVal.Add(StructureNs20, "SDMXStructure.xsd");
            retVal.Add(StructureNs21, "SDMXStructure.xsd");
            retVal.Add(GenericMetadataNs10, "SDMXStructure.xsd");
            retVal.Add(GenericMetadataNs20, "SDMXGenericMetadata.xsd");
            retVal.Add(GenericMetadataNs21, "SDMXMetadataGeneric.xsd");
            retVal.Add(MetadatareportNs10, "SDMXMetadataReport.xsd");
            retVal.Add(MetadatareportNs20, "SDMXMetadataReport.xsd");
            return retVal;
        }

        /// <summary>
        /// The switch namespace.
        /// </summary>
        /// <param name="inputFormat">
        /// The input format.
        /// </param>
        /// <param name="outputFormat">
        /// The output format.
        /// </param>
        /// <param name="namespaceUri">
        /// The namespace uri.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception. 
        /// </exception>
        private static string SwitchNamespace(
            SdmxSchemaEnumType inputFormat, SdmxSchemaEnumType outputFormat, string namespaceUri)
        {
            switch (inputFormat)
            {
                case SdmxSchemaEnumType.VersionOne:
                    switch (outputFormat)
                    {
                        case SdmxSchemaEnumType.VersionOne:
                            return namespaceUri;
                        case SdmxSchemaEnumType.VersionTwo:
                            return _version1ToVersion2Map[namespaceUri];
                        case SdmxSchemaEnumType.VersionTwoPointOne:
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "2.1"); // TODO 2.1
                        default:
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, outputFormat);
                    }

                case SdmxSchemaEnumType.VersionTwo:
                    switch (outputFormat)
                    {
                        case SdmxSchemaEnumType.VersionOne:
                            return _version2ToVersion1Map[namespaceUri];
                        case SdmxSchemaEnumType.VersionTwo:
                            return namespaceUri;
                        case SdmxSchemaEnumType.VersionTwoPointOne:
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "2.1"); // TODO 2.1
                        default:
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, outputFormat);
                    }

                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, outputFormat);
            }
        }

        #endregion

        /* $$$
        static SdmxConstants() {
                v1Tov2Map = new Dictionary<string, string>();
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(GenericNs10),(string)(GenericNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(MessageNs10),(string)(MessageNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(CompactNs10),(string)(CompactNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(UtilityNs10),(string)(UtilityNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(CrossNs10),(string)(CrossNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(CommonNs10),(string)(CommonNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(QueryNs10),(string)(QueryNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(RegistryNs10),(string)(RegistryNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(StructureNs10),(string)(StructureNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(GenericmetadataNs10),(string)(GenericmetadataNs20));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v1Tov2Map,(string)(MetadatareportNs10),(string)(MetadatareportNs20));
                v2Tov1Map = new Dictionary<string, string>();
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(GenericNs20),(string)(GenericNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(MessageNs20),(string)(MessageNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(CompactNs20),(string)(CompactNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(UtilityNs20),(string)(UtilityNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(CrossNs20),(string)(CrossNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(CommonNs20),(string)(CommonNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(QueryNs20),(string)(QueryNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(RegistryNs20),(string)(RegistryNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(StructureNs20),(string)(StructureNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(GenericmetadataNs20),(string)(GenericmetadataNs10));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(v2Tov1Map,(string)(MetadatareportNs20),(string)(MetadatareportNs10));
            }*/
    }
}
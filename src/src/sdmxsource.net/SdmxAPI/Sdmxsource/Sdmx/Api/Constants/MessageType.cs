// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Eurostat">
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
    ///     Contains a list of the types of SDMX document that can be processed
    /// </summary>
    public enum MessageEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     The structure.
        /// </summary>
        Structure,

        /// <summary>
        ///     The registry interface.
        /// </summary>
        RegistryInterface,

        /// <summary>
        ///     The query.
        /// </summary>
        Query,

        /// <summary>
        ///     The generic data.
        /// </summary>
        GenericData,

        /// <summary>
        ///     The utility data.
        /// </summary>
        UtilityData,

        /// <summary>
        ///     The compact data.
        /// </summary>
        CompactData,

        /// <summary>
        ///     The cross sectional data.
        /// </summary>
        CrossSectionalData,

        /// <summary>
        ///     The generic metadata.
        /// </summary>
        GenericMetadata,

        /// <summary>
        ///     The metadata report.
        /// </summary>
        MetadataReport,

        /// <summary>
        ///     The message group.
        /// </summary>
        MessageGroup,

        /// <summary>
        ///     The sdmx edi.
        /// </summary>
        SdmxEdi,

        /// <summary>
        ///     The error.
        /// </summary>
        Error,

        /// <summary>
        ///     The unknown.
        /// </summary>
        Unknown
    }

    /// <summary>
    ///     The message type.
    /// </summary>
    public class MessageType : BaseConstantType<MessageEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<MessageEnumType, MessageType> _instances =
            new Dictionary<MessageEnumType, MessageType>
                {
                    {
                        MessageEnumType.Structure, 
                        new MessageType(MessageEnumType.Structure, "Structure")
                    }, 
                    {
                        MessageEnumType.RegistryInterface, 
                        new MessageType(
                        MessageEnumType.RegistryInterface, "RegistryInterface")
                    }, 
                    {
                        MessageEnumType.Query, 
                        new MessageType(MessageEnumType.Query, "QueryMessage")
                    }, 
                    {
                        MessageEnumType.GenericData, 
                        new MessageType(
                        MessageEnumType.GenericData, "GenericData")
                    }, 
                    {
                        MessageEnumType.UtilityData, 
                        new MessageType(
                        MessageEnumType.UtilityData, "UtilityData")
                    }, 
                    {
                        MessageEnumType.CompactData, 
                        new MessageType(
                        MessageEnumType.CompactData, "CompactData")
                    }, 
                    {
                        MessageEnumType.CrossSectionalData, 
                        new MessageType(
                        MessageEnumType.CrossSectionalData, 
                        "CrossSectionalData")
                    }, 
                    {
                        MessageEnumType.GenericMetadata, 
                        new MessageType(
                        MessageEnumType.GenericMetadata, "GenericMetadata")
                    }, 
                    {
                        MessageEnumType.MetadataReport, 
                        new MessageType(
                        MessageEnumType.MetadataReport, "MetadataReport")
                    }, 
                    {
                        MessageEnumType.MessageGroup, 
                        new MessageType(
                        MessageEnumType.MessageGroup, "MessageGroup")
                    }, 
                    {
                        MessageEnumType.SdmxEdi, 
                        new MessageType(MessageEnumType.SdmxEdi, "EDI")
                    },  
                    {
                        MessageEnumType.Error, 
                        new MessageType(MessageEnumType.Error, "Error")
                    }, 
                    {
                        MessageEnumType.Unknown, 
                        new MessageType(MessageEnumType.Unknown, "Unknown")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _node name.
        /// </summary>
        private readonly string _nodeName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The message type.
        /// </param>
        /// <param name="nodeName">
        /// The node name.
        /// </param>
        private MessageType(MessageEnumType enumType, string nodeName)
            : base(enumType)
        {
            this._nodeName = nodeName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<MessageType> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///     Gets the node name.
        /// </summary>
        public string NodeName
        {
            get
            {
                return this._nodeName;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="MessageType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="MessageType"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static MessageType GetFromEnum(MessageEnumType enumType)
        {
            MessageType output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The parse string.
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="MessageType"/> .
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException. 
        /// </exception>
        public static MessageType ParseString(string messageType)
        {
            if (messageType == null)
            {
                throw new ArgumentException("MESSAGE_TYPE.parseString can not parse null");
            }

            switch (messageType)
            {
                    // Dataset messages
                case "StructureSpecificTimeSeriesData":
                case "StructureSpecificData":
                    return GetFromEnum(MessageEnumType.CompactData);

                case "GenericTimeSeriesData":
                    return GetFromEnum(MessageEnumType.GenericData);

                    // Data and metadata query v2.1 messages
                case "GenericDataQuery":
                case "StructureSpecificDataQuery":
                case "GenericTimeSeriesDataQuery":
                case "StructureSpecificTimeSeriesDataQuery":
                case "GenericMetadataQuery":
                case "StructureSpecificMetadataQuery":
                case "StructuresQuery":
                case "DataflowQuery":
                case "MetadataflowQuery":
                case "DataStructureQuery":
                case "MetadataStructureQuery":
                case "CategorySchemeQuery":
                case "ConceptSchemeQuery":
                case "CodelistQuery":
                case "HierarchicalCodelistQuery":
                case "OrganisationSchemeQuery":
                case "ReportingTaxonomyQuery":
                case "StructureSetQuery":
                case "ProcessQuery":
                case "CategorisationQuery":
                case "ProvisionAgreementQuery":
                case "ConstraintQuery":
                    return GetFromEnum(MessageEnumType.Query);
            }

            // check messages that the enumeration value correspont to message root element name
            foreach (MessageType currentType in Values)
            {
                if (currentType.NodeName.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                {
                    return currentType;
                }
            }

            throw new ArgumentException("'" + messageType + "' is not a known root node for an SDMX message");
        }

        #endregion
    }
}
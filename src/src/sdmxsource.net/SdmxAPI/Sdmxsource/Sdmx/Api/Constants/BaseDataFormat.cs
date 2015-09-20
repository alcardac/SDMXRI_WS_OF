// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseDataFormat.cs" company="Eurostat">
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
    ///     Contains the Data Formats, such as Generic and Compact - without specifying any SDMX schema version
    ///     information
    /// </summary>
    public enum BaseDataFormatEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Generic Data Messages - includes 2.1 Generic and GenericTimeSeries
        /// </summary>
        Generic, 

        /// <summary>
        ///     Relates to Compact (1.0/2.0) and StructureSpecific, StructureSpecificTime Series (2.1)
        /// </summary>
        Compact, 

        /// <summary>
        ///     Relates to Utility Data (1.0 and 2.0 only)
        /// </summary>
        Utility, 

        /// <summary>
        ///     Relates to EDI
        /// </summary>
        Edi, 

        /// <summary>
        ///     Relates to 2.0 Cross Sectional Data
        /// </summary>
        CrossSectional, 

        /// <summary>
        ///     Relates to Message Group Data (2.0 only)
        /// </summary>
        MessageGroup, 

        /// <summary>
        ///     Any type of delimited data
        /// </summary>
        Csv,

        /// <summary>
        /// Sdmx Json Format
        /// </summary>
        Json
    }

    /// <summary>
    ///     Contains the Data Formats, such as Generic and Compact - without specifying any SDMX schema version
    ///     information
    /// </summary>
    public class BaseDataFormat : BaseConstantType<BaseDataFormatEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The <see cref="BaseDataFormat" /> instances.
        /// </summary>
        private static readonly Dictionary<BaseDataFormatEnumType, BaseDataFormat> Instances =
            new Dictionary<BaseDataFormatEnumType, BaseDataFormat>
                {
                    {
                        BaseDataFormatEnumType.Generic, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.Generic, "GenericData")
                    }, 
                    {
                        BaseDataFormatEnumType.Compact, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.Compact, "CompactData")
                    }, 
                    {
                        BaseDataFormatEnumType.Utility, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.Utility, "UtilityData")
                    }, 
                    {
                        BaseDataFormatEnumType.Edi, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.Edi, null)
                    }, 
                    {
                        BaseDataFormatEnumType.CrossSectional, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.CrossSectional, 
                        "CrossSectionalData")
                    }, 
                    {
                        BaseDataFormatEnumType.MessageGroup, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.MessageGroup, 
                        "MessageGroup")
                    }, 
                    {
                        BaseDataFormatEnumType.Csv, 
                        new BaseDataFormat(
                        BaseDataFormatEnumType.Csv, null)
                    },
                    {
                          BaseDataFormatEnumType.Json, 
                          new BaseDataFormat(
                        BaseDataFormatEnumType.Json, null)
                    }


                };

        #endregion

        #region Fields

        /// <summary>
        ///     The enumeration value of this instance.
        /// </summary>
        private readonly BaseDataFormatEnumType _node;

        /// <summary>
        ///     The root node
        /// </summary>
        private readonly string _rootNode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataFormat"/> class.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="rootNode">
        /// The root node.
        /// </param>
        private BaseDataFormat(BaseDataFormatEnumType node, string rootNode)
            : base(node)
        {
            this._node = node;
            this._rootNode = rootNode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="BaseDataFormat" />
        /// </summary>
        public static IEnumerable<BaseDataFormat> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the root node.
        /// </summary>
        public string RootNode
        {
            get
            {
                return this._rootNode;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a <see cref="BaseDataFormat"/> from the specified <paramref name="messageType"/>
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="BaseDataFormat"/> .
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageType"/>
        ///     is not a data message
        /// </exception>
        public static BaseDataFormat GetDataFormat(MessageType messageType)
        {
            if (messageType == null)
            {
                throw new ArgumentNullException("messageType");
            }

            BaseDataFormatEnumType format;
            switch (messageType.EnumType)
            {
                case MessageEnumType.GenericData:
                    format = BaseDataFormatEnumType.Generic;
                    break;
                case MessageEnumType.UtilityData:
                    format = BaseDataFormatEnumType.Utility;
                    break;
                case MessageEnumType.CompactData:
                    format = BaseDataFormatEnumType.Compact;
                    break;
                case MessageEnumType.CrossSectionalData:
                    format = BaseDataFormatEnumType.CrossSectional;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(messageType + " is not a data message");
            }

            return GetFromEnum(format);

            /* &&&
        switch (messageType)
        {
            case MessageEnumType.COMPACT_DATA:
                return Compact;
            case MessageEnumType.CROSS_SECTIONAL_DATA:
                return CrossSectional;
            case MessageEnumType.GENERIC_DATA:
                return Generic;
            case MessageEnumType.UTILITY_DATA:
                return Utility;
            default:
                throw new ArgumentOutOfRangeException(swmessageType + " is not a data message");
        }*/
        }

        /// <summary>
        /// Gets the instance of <see cref="BaseDataFormat"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="DataQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static BaseDataFormat GetFromEnum(BaseDataFormatEnumType enumType)
        {
            BaseDataFormat output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        ///     Gets a <see cref="T:System.string" /> that represents the current <see cref="BaseDataFormat" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.string" /> that represents the current <see cref="BaseDataFormat" /> .
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            switch (this._node)
            {
                case BaseDataFormatEnumType.Generic:
                    return "Generic";
                case BaseDataFormatEnumType.Compact:
                    return "Structure Specific (Compact)";
                case BaseDataFormatEnumType.Utility:
                    return "Utility";
                case BaseDataFormatEnumType.Edi:
                    return "EDI";
                case BaseDataFormatEnumType.CrossSectional:
                    return "Cross Sectional";
            }

            return this._node.ToString();
        }

        #endregion
    }
}
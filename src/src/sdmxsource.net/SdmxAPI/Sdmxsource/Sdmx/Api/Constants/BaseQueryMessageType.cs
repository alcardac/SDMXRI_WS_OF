// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseQueryMessageType.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Contains the base query types, without concern for the detail of the query.
    ///     <p />
    ///     The types are structure query, data query or metadata query
    /// </summary>
    public enum BaseQueryMessageEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Query for Structures
        /// </summary>
        Structure, 

        /// <summary>
        ///     Query for Data
        /// </summary>
        Data, 

        /// <summary>
        ///     Query for Metadata
        /// </summary>
        Metadata, 

        /// <summary>
        ///     Query for Schemas
        /// </summary>
        Schema
    }

    /// <summary>
    ///     Contains the base query types, without concern for the detail of the query.
    ///     The types are structure query, data query or metadata query
    /// </summary>
    public class BaseQueryMessageType : BaseConstantType<BaseQueryMessageEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<BaseQueryMessageEnumType, BaseQueryMessageType> Instances =
            new Dictionary<BaseQueryMessageEnumType, BaseQueryMessageType>
                {
                    {
                        BaseQueryMessageEnumType.Structure, 
                        new BaseQueryMessageType(
                        BaseQueryMessageEnumType.Structure, 
                        "Structure")
                    }, 
                    {
                        BaseQueryMessageEnumType.Data, 
                        new BaseQueryMessageType(
                        BaseQueryMessageEnumType.Data, "Data")
                    }, 
                    {
                        BaseQueryMessageEnumType.Metadata, 
                        new BaseQueryMessageType(
                        BaseQueryMessageEnumType.Metadata, 
                        "Metadata")
                    }, 
                    {
                        BaseQueryMessageEnumType.Schema, 
                        new BaseQueryMessageType(
                        BaseQueryMessageEnumType.Schema, 
                        "Schema")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _type.
        /// </summary>
        private readonly string _queryType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQueryMessageType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="queryType">
        /// The type.
        /// </param>
        private BaseQueryMessageType(BaseQueryMessageEnumType enumType, string queryType)
            : base(enumType)
        {
            this._queryType = queryType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="BaseQueryMessageType" />
        /// </summary>
        public static IEnumerable<BaseQueryMessageType> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <remarks>This is the corresponds to <c>getType()</c> in Java. Renamed to <c>QueryType</c> to conform to <c>FxCop</c> rule CA1721</remarks>
        public string QueryType
        {
            get
            {
                return this._queryType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="BaseQueryMessageType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="BaseQueryMessageType"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static BaseQueryMessageType GetFromEnum(BaseQueryMessageEnumType enumType)
        {
            BaseQueryMessageType output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        #endregion
    }
}
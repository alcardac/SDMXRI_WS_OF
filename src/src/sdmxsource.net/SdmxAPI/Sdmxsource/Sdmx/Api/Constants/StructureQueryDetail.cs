// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureQueryDetail.cs" company="Eurostat">
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
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    ///     For a 2.1 REST structure query, this represents the detail parameter
    /// </summary>
    public enum StructureQueryDetailEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     The full.
        /// </summary>
        Full,

        /// <summary>
        ///     The all stubs.
        /// </summary>
        AllStubs,

        /// <summary>
        ///     The referenced stubs.
        /// </summary>
        ReferencedStubs

        // TODODODODODODOD ********
    }
    /// <summary>
    ///     The query message type.
    /// </summary>
    public class StructureQueryDetail : BaseConstantType<StructureQueryDetailEnumType>
    {
        #region Static Fields

        /// <summary>
        ///   The _instances.
        /// </summary>
        private static readonly Dictionary<StructureQueryDetailEnumType, StructureQueryDetail> _instances =
            new Dictionary<StructureQueryDetailEnumType, StructureQueryDetail>
                {
                    {
                        StructureQueryDetailEnumType.Null, 
                        new StructureQueryDetail(StructureQueryDetailEnumType.Null, "null")
                    }, 
                    {
                        StructureQueryDetailEnumType.Full, 
                        new StructureQueryDetail(StructureQueryDetailEnumType.Full, "full")
                    }, 
                    {
                        StructureQueryDetailEnumType.AllStubs, 
                        new StructureQueryDetail(
                        StructureQueryDetailEnumType.AllStubs, "allstubs")
                    }, 
                    {
                        StructureQueryDetailEnumType.ReferencedStubs, 
                        new StructureQueryDetail(StructureQueryDetailEnumType.ReferencedStubs, "referencedstubs")
                    }, 
                };

        #endregion

        #region Fields

        /// <summary>
        ///   The _value.
        /// </summary>
        private readonly string _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureQueryDetail"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The structure reference type. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        private StructureQueryDetail(StructureQueryDetailEnumType enumType, string name)
            : base(enumType)
        {
            this._value = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets all instances for this type
        /// </summary>
        public static IEnumerable<StructureQueryDetail> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this._value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="StructureQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type 
        /// </param>
        /// <returns>
        /// the instance of <see cref="StructureQueryDetail"/> mapped to <paramref name="enumType"/> 
        /// </returns>
        public static StructureQueryDetail GetFromEnum(StructureQueryDetailEnumType enumType)
        {
            StructureQueryDetail output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Gets the StructureQueryDetailEnumType equivalent of the input string (ignores case):
        ///   <ul>
        ///     <li>none - StructureQueryDetailEnumType.Null</li>
        ///     <li>parents - StructureQueryDetailEnumType.Full</li>
        ///     <li><c>parentsandsiblings</c> - StructureQueryDetailEnumType.AllStubs</li>
        ///     <li>children - StructureQueryDetailEnumType.ReferencedStubs</li>
        ///   </ul>
        /// </summary>
        /// <param name="value">String value. </param>
        /// <returns>
        /// The <see cref="StructureQueryDetail"/> . 
        /// </returns>
        public static StructureQueryDetail ParseString(string value)
        {
            foreach (StructureQueryDetail currentQueryDetail in Values)
            {
                if (currentQueryDetail.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;
            try
            {
                SdmxStructureType structEnumType = SdmxStructureType.ParseClass(value);
                if (!structEnumType.IsMaintainable)
                {
                    foreach (SdmxStructureType currentStructType in SdmxStructureType.Values)
                    {
                        if (currentStructType.IsMaintainable)
                        {
                            sb.Append(concat).Append(currentStructType.UrnClass.ToLowerInvariant());
                            concat = ", ";
                        }
                    }

                    throw new SdmxSemmanticException(
                        "Disallowed structure type " + structEnumType.UrnClass.ToLowerInvariant() + " allowed parameters: " + sb.ToString());
                }
            }
            catch (Exception)
            {
                foreach (StructureQueryDetail currentQueryDetail in Values)
                {
                    sb.Append(concat + currentQueryDetail.Value);
                    concat = ", ";
                }

                throw new SdmxSemmanticException(
                    "Unknown Parameter " + value + " allowed parameters: " + sb.ToString()
                    + " or a specific structure reference such as 'codelist'");
            }

            return GetFromEnum(StructureQueryDetailEnumType.Null);
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}
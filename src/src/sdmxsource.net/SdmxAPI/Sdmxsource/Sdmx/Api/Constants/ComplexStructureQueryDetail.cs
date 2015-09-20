// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexStructureQueryDetail.cs" company="Eurostat">
//   Date Created : 2013-08-19
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
    ///     For a 2.1 REST data query, this enum contains a list of the parameters.
    ///     <p />
    ///     Detail of complex structure query; possible options are:
    ///     <ul>
    ///      <li>full - Return everything</li>
    ///      <li>stub - Return the stub</li>
    ///      <li>completestub - Return the complete stub</li>
    ///      <li>matcheditems - Return the matched items</li>
    ///      <li>cascadedmatcheditems - Return the cascaded matched items</li>
    ///     </ul>
    /// </summary>
    public enum ComplexStructureQueryDetailEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     Return everything
        /// </summary>
        Full,

        /// <summary>
        ///     The stub
        /// </summary>
        Stub,

        /// <summary>
        ///     The complete stub
        /// </summary>
        CompleteStub,

        /// <summary>
        ///     The matched items
        /// </summary>
        MatchedItems,

        /// <summary>
        ///     The cascaded matched items
        /// </summary>
        CascadedMatchedItems
    }

    /// <summary>
    /// The complex structure query detail
    /// </summary>
    public class ComplexStructureQueryDetail : BaseConstantType<ComplexStructureQueryDetailEnumType>
    {

        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<ComplexStructureQueryDetailEnumType, ComplexStructureQueryDetail> Instances =
            new Dictionary<ComplexStructureQueryDetailEnumType, ComplexStructureQueryDetail>
                {
                    {
                        ComplexStructureQueryDetailEnumType.Full, 
                        new ComplexStructureQueryDetail(
                        ComplexStructureQueryDetailEnumType.Full, 
                        "full")
                    }, 
                    {
                        ComplexStructureQueryDetailEnumType.Stub, 
                        new ComplexStructureQueryDetail(
                        ComplexStructureQueryDetailEnumType.Stub, 
                        "stub")
                    }, 
                    {
                        ComplexStructureQueryDetailEnumType.CompleteStub, 
                        new ComplexStructureQueryDetail(
                        ComplexStructureQueryDetailEnumType.CompleteStub, "completestub")
                    }, 
                    {
                        ComplexStructureQueryDetailEnumType.MatchedItems, 
                        new ComplexStructureQueryDetail(
                        ComplexStructureQueryDetailEnumType.MatchedItems, "matcheditems")
                    }, 
                    {
                        ComplexStructureQueryDetailEnumType.CascadedMatchedItems, 
                        new ComplexStructureQueryDetail(
                        ComplexStructureQueryDetailEnumType.CascadedMatchedItems, "cascadedmatcheditems")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _value.
        /// </summary>
        private readonly string _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexStructureQueryDetail"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        private ComplexStructureQueryDetail(ComplexStructureQueryDetailEnumType enumType, string value)
            : base(enumType)
        {
            this._value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="ComplexStructureQueryDetail" />
        /// </summary>
        public static IEnumerable<ComplexStructureQueryDetail> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the value.
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
        /// Gets the instance of <see cref="ComplexStructureQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="ComplexStructureQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static ComplexStructureQueryDetail GetFromEnum(ComplexStructureQueryDetailEnumType enumType)
        {
            ComplexStructureQueryDetail output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="value">The string Value.</param>
        /// <returns>
        /// The <see cref="ComplexStructureQueryDetail" /> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception</exception>
        public static ComplexStructureQueryDetail ParseString(string value)
        {
            foreach (ComplexStructureQueryDetail currentQueryDetail in Values) 
            {
                if (currentQueryDetail.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (ComplexStructureQueryDetail currentQueryDetail in Values)
            {
                sb.Append(concat + currentQueryDetail.Value);
                concat = ", ";
            }

            throw new SdmxSemmanticException("Unknown Parameter " + value + " allowed parameters: " + sb.ToString());

        }

        #endregion

    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderedOperator.cs" company="Eurostat">
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
    ///     Ordered operator; possible options are:
    ///     <ul>
    ///      <li>greaterthanorequal - The greater than or equal</li>
    ///      <li>lessthanorequal - The less than or equal</li>
    ///      <li>greaterthan - The greater than</li>
    ///      <li>lessthan - The less than</li>
    ///      <li>equal - The equal</li>
    ///      <li>notequal - The not equal</li>
    ///     </ul>
    /// </summary>
    public enum OrderedOperatorEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     The greater than or equal
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        ///     The less than or equal
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        ///     The greater than
        /// </summary>
        GreaterThan,

        /// <summary>
        ///     The less than
        /// </summary>
        LessThan,

        /// <summary>
        ///     The equal
        /// </summary>
        Equal,

        /// <summary>
        ///     The not equal
        /// </summary>
        NotEqual
    }

    /// <summary>
    /// The ordered operator
    /// </summary>
    public class OrderedOperator : BaseConstantType<OrderedOperatorEnumType>
    {

        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<OrderedOperatorEnumType, OrderedOperator> Instances =
            new Dictionary<OrderedOperatorEnumType, OrderedOperator>
                {
                    {
                        OrderedOperatorEnumType.GreaterThanOrEqual, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.GreaterThanOrEqual, 
                        "greaterThanOrEqual")
                    }, 
                    {
                        OrderedOperatorEnumType.LessThanOrEqual, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.LessThanOrEqual, 
                        "lessThanOrEqual")
                    }, 
                    {
                        OrderedOperatorEnumType.GreaterThan, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.GreaterThan, 
                        "greaterThan")
                    }, 
                    {
                        OrderedOperatorEnumType.LessThan, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.LessThan, 
                        "lessThan")
                    }, 
                    {
                        OrderedOperatorEnumType.Equal, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.Equal, 
                        "equal")
                    }, 
                    {
                        OrderedOperatorEnumType.NotEqual, 
                        new OrderedOperator(
                        OrderedOperatorEnumType.NotEqual, 
                        "notEqual")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _ordOperator.
        /// </summary>
        private readonly string _ordOperator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedOperator"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type
        /// </param>
        /// <param name="ordOperator">
        /// The ordOperator
        /// </param>
        private OrderedOperator(OrderedOperatorEnumType enumType, string ordOperator)
            : base(enumType)
        {
            this._ordOperator = ordOperator;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="OrderedOperator" />
        /// </summary>
        public static IEnumerable<OrderedOperator> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the ordOperator.
        /// </summary>
        public string OrdOperator
        {
            get
            {
                return this._ordOperator;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Implicit convert the ordered operator
        /// </summary>
        /// <param name="enumType">The <c>Enum</c>.</param>
        /// <returns>The ordered operator</returns>
        public static implicit operator OrderedOperator(OrderedOperatorEnumType enumType)
        {
            return GetFromEnum(enumType);
        }

        /// <summary>
        /// Gets the instance of <see cref="OrderedOperator"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="OrderedOperator"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static OrderedOperator GetFromEnum(OrderedOperatorEnumType enumType)
        {
            OrderedOperator output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The parse string.
        /// </summary>
        /// <param name="value">
        /// The sring value.
        /// </param>
        /// <returns>
        /// The <see cref="OrderedOperator"/> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validation Exception
        /// </exception>
        public static OrderedOperator ParseString(string value)
        {
            foreach (OrderedOperator currentQueryDetail in Values)
            {
                if (currentQueryDetail.OrdOperator.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (OrderedOperator currentQueryDetail in Values)
            {
                sb.Append(concat + currentQueryDetail.OrdOperator);
                concat = ", ";
            }

            throw new SdmxSemmanticException("Unknown Parameter " + value + " allowed parameters: " + sb.ToString());
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this._ordOperator;
        }

        #endregion

    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexMaintainableQueryDetail.cs" company="Eurostat">
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
    ///     Detail of complex maintainable query; possible options are:
    ///    <ul>
    ///     <li>full - Return everything</li>
    ///     <li>stub - Return the stub</li>
    ///     <li>completestub - Return the complete stub</li>
    ///    </ul>
    /// </summary>
    public enum ComplexMaintainableQueryDetailEnumType
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
        CompleteStub
    }

    /// <summary>
    /// The complex maintainable query detail
    /// </summary>
    public class ComplexMaintainableQueryDetail : BaseConstantType<ComplexMaintainableQueryDetailEnumType>
    {

        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<ComplexMaintainableQueryDetailEnumType, ComplexMaintainableQueryDetail> Instances =
            new Dictionary<ComplexMaintainableQueryDetailEnumType, ComplexMaintainableQueryDetail>
                {
                    {
                        ComplexMaintainableQueryDetailEnumType.Full, 
                        new ComplexMaintainableQueryDetail(
                        ComplexMaintainableQueryDetailEnumType.Full, 
                        "full")
                    }, 
                    {
                        ComplexMaintainableQueryDetailEnumType.Stub, 
                        new ComplexMaintainableQueryDetail(
                        ComplexMaintainableQueryDetailEnumType.Stub, 
                        "stub")
                    }, 
                    {
                        ComplexMaintainableQueryDetailEnumType.CompleteStub, 
                        new ComplexMaintainableQueryDetail(
                        ComplexMaintainableQueryDetailEnumType.CompleteStub, "completestub")
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
        /// Initializes a new instance of the <see cref="ComplexMaintainableQueryDetail"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        private ComplexMaintainableQueryDetail(ComplexMaintainableQueryDetailEnumType enumType, string value)
            : base(enumType)
        {
            this._value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="ComplexMaintainableQueryDetail" />
        /// </summary>
        public static IEnumerable<ComplexMaintainableQueryDetail> Values
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
        /// Gets the instance of <see cref="ComplexMaintainableQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="ComplexMaintainableQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static ComplexMaintainableQueryDetail GetFromEnum(ComplexMaintainableQueryDetailEnumType enumType)
        {
            ComplexMaintainableQueryDetail output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>
        /// The <see cref="ComplexMaintainableQueryDetail" /> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception</exception>
        public static ComplexMaintainableQueryDetail ParseString(string value)
        {
            foreach (ComplexMaintainableQueryDetail currentQueryDetail in Values) 
            {
                if (currentQueryDetail.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
				    return currentQueryDetail;
			    }
		    }

            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (ComplexMaintainableQueryDetail currentQueryDetail in Values) 
            {
			    sb.Append(concat + currentQueryDetail.Value);
			    concat = ", ";
		    }

		    throw new SdmxSemmanticException("Unknown Parameter "+ value + " allowed parameters: " + sb.ToString());

        }

        #endregion

    }
}

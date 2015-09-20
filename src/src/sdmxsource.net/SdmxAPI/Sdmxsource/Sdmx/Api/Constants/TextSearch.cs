// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextSearch.cs" company="Eurostat">
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
    /// TODO: Update summary.
    /// </summary>
    public enum TextSearchEnumType
    {
        /// <summary>
        /// The contains
        /// </summary>
        Contains,

        /// <summary>
        /// The start with
        /// </summary>
        StartsWith,

        /// <summary>
        /// The end with
        /// </summary>
        EndsWith,

        /// <summary>
        /// The does not contain
        /// </summary>
        DoesNotContain,

        /// <summary>
        /// The does not start with
        /// </summary>
        DoesNotStartWith,

        /// <summary>
        /// The does not end with
        /// </summary>
        DoesNotEndWith,

        /// <summary>
        /// The equal
        /// </summary>
        Equal,

        /// <summary>
        /// The not equal
        /// </summary>
        NotEqual
    }


    /// <summary>
    /// The text search
    /// </summary>
    public class TextSearch : BaseConstantType<TextSearchEnumType>
    {

        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<TextSearchEnumType, TextSearch> Instances =
            new Dictionary<TextSearchEnumType, TextSearch>
                {
                    {
                        TextSearchEnumType.Contains, 
                        new TextSearch(
                        TextSearchEnumType.Contains, 
                        "contains")
                    }, 
                    {
                        TextSearchEnumType.StartsWith, 
                        new TextSearch(
                        TextSearchEnumType.StartsWith, 
                        "startsWith")
                    }, 
                    {
                        TextSearchEnumType.EndsWith, 
                        new TextSearch(
                        TextSearchEnumType.EndsWith, 
                        "endsWith")
                    }, 
                    {
                        TextSearchEnumType.DoesNotContain, 
                        new TextSearch(
                        TextSearchEnumType.DoesNotContain, 
                        "doesNotContain")
                    }, 
                    {
                        TextSearchEnumType.DoesNotStartWith, 
                        new TextSearch(
                        TextSearchEnumType.DoesNotStartWith, 
                        "doesNotStartWith")
                    }, 
                    {
                        TextSearchEnumType.DoesNotEndWith, 
                        new TextSearch(
                        TextSearchEnumType.DoesNotEndWith, 
                        "doesNotEndWith")
                    }, 
                    {
                        TextSearchEnumType.Equal, 
                        new TextSearch(
                        TextSearchEnumType.Equal, 
                        "equal")
                    }, 
                    {
                        TextSearchEnumType.NotEqual, 
                        new TextSearch(
                        TextSearchEnumType.NotEqual, 
                        "notEqual")
                    }
                };

        #endregion


        #region Fields

        /// <summary>
        ///     The _operator.
        /// </summary>
        private readonly string _operator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSearch"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type
        /// </param>
        /// <param name="ope">
        /// The ope
        /// </param>
        private TextSearch(TextSearchEnumType enumType, string ope)
            : base(enumType)
        {
            this._operator = ope;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="TextSearch" />
        /// </summary>
        public static IEnumerable<TextSearch> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the operator.
        /// </summary>
        public string Operator
        {
            get
            {
                return this._operator;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Implicit conversion from <see cref="TextSearchEnumType"/> to <see cref="TextSearch"/>
        /// </summary>
        /// <param name="enumType">The <see cref="TextSearchEnumType"/>.</param>
        /// <returns>
        /// the instance of <see cref="TextSearch"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static implicit operator TextSearch(TextSearchEnumType enumType)
        {
            return GetFromEnum(enumType);
        }


        /// <summary>
        /// Gets the instance of <see cref="TextSearch"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="TextSearch"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static TextSearch FromTextSearchEnumType(TextSearchEnumType enumType)
        {
            return GetFromEnum(enumType);
        }

        /// <summary>
        /// Gets the instance of <see cref="TextSearch"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="TextSearch"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static TextSearch GetFromEnum(TextSearchEnumType enumType)
        {
            TextSearch output;
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
        /// The <see cref="TextSearch" /> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception</exception>
        public static TextSearch ParseString(string value)
        {
            foreach (TextSearch ts in Values)
            {
                if (ts.Operator.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return ts;
                }
            }

            throw new SdmxSemmanticException("TextSearch parseString unknown operator: " + value);

        }

        #endregion

    }
}

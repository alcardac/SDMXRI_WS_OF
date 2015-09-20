// -----------------------------------------------------------------------
// <copyright file="EDIUtil.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Util
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    using Estat.Sri.SdmxEdiDataWriter.Helper;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Extension;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;

    /// <summary>
    /// EDI related utilities.
    /// </summary>
    /// <seealso cref="GesmesHelper"/>
    public static class EDIUtil
    {
        /// <summary>
        /// The _split on plus
        /// </summary>
        private static readonly Regex _splitOnPlus = new Regex("(?<!\\?)\\+", RegexOptions.Compiled);

        /// <summary>
        /// The _split on colon
        /// </summary>
        private static readonly Regex _splitOnColon = new Regex("(?<!\\?):", RegexOptions.Compiled);

        /// <summary>
        /// The sibling group identifier
        /// </summary>
        private static string _siblingGroupId = "Sibling";

        /// <summary>
        /// Gets or sets the sibling group identifier.
        /// </summary>
        /// <value>
        /// The sibling group identifier.
        /// </value>
        public static string SiblingGroupId
        {
            get
            {
                return _siblingGroupId;
            }

            set
            {
                _siblingGroupId = value;
            }
        }

        /// <summary>
        /// Checks the prefix is as expected
        /// <p />
        /// If the prefix is not as expected and <paramref name="errorOnFail"/> is set to true, an exception will be thrown
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="errorOnFail">if set to <c>true</c> will throw an exception on fail.</param>
        /// <returns>true if it the prefix is as expected; otherwise false.</returns>
        /// <exception cref="System.ArgumentException">Expecting prefix : ' <c> prefix.GetPrefix() </c> ' but got ' <c> dataReader.LineType </c> '</exception>
        public static bool AssertPrefix(IEdiReader dataReader, EdiPrefix prefix, bool errorOnFail)
        {
            if (dataReader.LineType != prefix)
            {
                if (errorOnFail)
                {
                    throw new ArgumentException("Expecting prefix : '" + prefix.GetPrefix() + "' but got '" + dataReader.LineType + "'");
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Splits the specified <paramref name="input"/> on plus character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array split with plus character.</returns>
        public static string[] SplitOnPlus(string input)
        {
            return SplitOnPlus(input, -1);
        }

        /// <summary>
        /// Splits the specified <paramref name="input" /> on plus character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expectedNumberOfSplits">The expected number of splits. Set to -1 to disable.</param>
        /// <returns>
        /// An array split with plus character.
        /// </returns>
        public static string[] SplitOnPlus(string input, int expectedNumberOfSplits)
        {
            return SplitOnChar(input, _splitOnPlus, "+", expectedNumberOfSplits);
        }

        /// <summary>
        /// Splits the specified <paramref name="input"/> on colon character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array split with colon character.</returns>
        public static string[] SplitOnColon(string input)
        {
            return SplitOnColon(input, -1);
        }

        /// <summary>
        /// Splits the specified <paramref name="input" /> on colon character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expectedNumberOfSplits">The expected number of splits. Set to -1 to disable.</param>
        /// <returns>
        /// An array split with colon character.
        /// </returns>
        public static string[] SplitOnColon(string input, int expectedNumberOfSplits)
        {
            return SplitOnChar(input, _splitOnColon, ":", expectedNumberOfSplits);
        }

        /// <summary>
        /// Takes an Edi string, removes all Edi escape characters
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>THe <paramref name="inputString"/> without the escape character.</returns>
        public static string EdiToString(string inputString)
        {
            inputString = Regex.Replace(inputString, "(?<!\\?):", string.Empty);
            inputString = inputString.Replace("?:", EdiConstants.Colon);
            inputString = inputString.Replace("?'", EdiConstants.EndTag);
            inputString = inputString.Replace("?+", EdiConstants.Plus);
            inputString = inputString.Replace("??", EdiConstants.EscapeChar);
            return inputString;
        }

        /// <summary>
        /// Parses the string as <see cref="int"/>.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <returns>An <see cref="int"/>.from the specified <paramref name="s"/></returns>
        public static int ParseStringAsInt(string s)
        {
            // NOte in .NET all exceptions are unchecked.
            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Splits the specified <paramref name="input" /> with the specified <paramref name="expression" />
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="character">The character.</param>
        /// <param name="expectedNumberOfSplits">The expected number of splits. Set to -1 to disable.</param>
        /// <returns>
        /// An array split with colon character.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Unexpected number of characters, expecting <paramref name="expectedNumberOfSplits"/> actual '<c>splitOnPlus.Length</c>'</exception>
        private static string[] SplitOnChar(string input, Regex expression, string character, int expectedNumberOfSplits)
        {
            var splitOnPlus = expression.Split(input);
            if (expectedNumberOfSplits > 0)
            {
                if (splitOnPlus.Length != expectedNumberOfSplits)
                {
                    throw new SdmxSemmanticException("Unexpected number of '" + character + "' characters, expecting " + expectedNumberOfSplits + " actual '" + splitOnPlus.Length + "'");
                }
            }

            return splitOnPlus;
        }
    }
}
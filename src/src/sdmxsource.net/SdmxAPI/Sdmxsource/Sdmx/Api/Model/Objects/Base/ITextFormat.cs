// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextFormat.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    #endregion

    /// <summary>
    ///     ITextFormat gives information about the restrictions on a text value
    /// </summary>
    public interface ITextFormat : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the number of decimal places, if the type is number
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        long? Decimals { get; }

        /// <summary>
        ///     Gets the latest end time the value can take -
        /// </summary>
        /// <value> null if not defined </value>
        ISdmxDate EndTime { get; }

        /// <summary>
        ///     Gets the end value of text field that the value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        decimal? EndValue { get; }

        /// <summary>
        ///     Gets the interval of for the text values, this is typical given if isInterval() is true
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        decimal? Interval { get; }

        /// <summary>
        ///     Gets the is multi lingual.
        /// </summary>
        TertiaryBool Multilingual { get; }

        /// <summary>
        ///     Gets the maximum length of text field that the value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        long? MaxLength { get; }

        /// <summary>
        ///     Gets the maximum value that the numerical value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        decimal? MaxValue { get; }

        /// <summary>
        ///     Gets the minimum length of text field that the value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        long? MinLength { get; }

        /// <summary>
        ///     Gets the maximum value that the numerical value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        decimal? MinValue { get; }

        /// <summary>
        ///     Gets the pattern that the reported value must adhere to
        /// </summary>
        /// <value> </value>
        string Pattern { get; }

        /// <summary>
        ///     Gets the earliest start time the value can take -
        /// </summary>
        /// <value> null if not defined </value>
        ISdmxDate StartTime { get; }

        /// <summary>
        ///     Gets the start value of text field that the value must take
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        decimal? StartValue { get; }

        /// <summary>
        ///     Gets the type the values the text value can take
        /// </summary>
        /// <value> null if not defined </value>
        TextType TextType { get; }

        /// <summary>
        ///     Gets the interval of for the text values, this is typically given if isInterval() is true
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> </value>
        string TimeInterval { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Indicates whether the values are intended to be ordered, and it may work in combination with the interval attribute
        ///     <p />
        ///     If null, then interpret as N/A
        /// </summary>
        /// <value> The &lt; see cref= &quot; TertiaryBool &quot; / &gt; . </value>
        TertiaryBool Sequence { get; }

        #endregion
    }
}
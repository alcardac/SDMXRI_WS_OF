// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateFormat.cs" company="Eurostat">
//   Date Created : 2014-07-24
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The date format.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Util.Date
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The date format.
    /// </summary>
    /// <remarks>Note the <see cref="DateFormat"/> is a very simple port from JDK DateFormat because no such class exists in .NET framework. It only supports Parse and Format.</remarks>
    public class DateFormat
    {
        #region Fields

        /// <summary>
        /// The _format string.
        /// </summary>
        private readonly string _formatString;

        /// <summary>
        /// The _invariant culture.
        /// </summary>
        private readonly CultureInfo _invariantCulture;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateFormat"/> class.
        /// </summary>
        /// <param name="formatString">
        /// The format string.
        /// </param>
        public DateFormat(string formatString)
        {
            this._invariantCulture = CultureInfo.InvariantCulture;
            this._formatString = formatString;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Formats the specified input.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The specified <see cref="DateTime"/> as text.
        /// </returns>
        public string Format(DateTime input)
        {
            return input.ToString(this._formatString, this._invariantCulture);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime Parse(string input)
        {
            return DateTime.ParseExact(input, this._formatString, this._invariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AllowWhiteSpaces);
        }

        #endregion
    }
}
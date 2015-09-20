// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionableUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util
{
    #region Using directives

    using System;
    using System.Text.RegularExpressions;

    #endregion


    /// <summary>
    ///  Utility class providing helper methods to determine if a String is a valid version1.
    /// </summary>
    public static class VersionableUtil
    {
        /// <summary>
        /// The version regex matching.
        /// </summary>
        private static readonly Regex _versionRegex = new Regex("^([0-9])+(\\.([0-9])+)*$", RegexOptions.Compiled);

        #region Public Methods and Operators

        /// <summary>
        /// Formats the supplied version1 String.
        ///  TODO: Review whether this method is actually required any more.
        /// </summary>
        /// <param name="version">
        /// The version1 string to format. 
        /// </param>
        /// <returns>
        /// The formatted version1 of the input. 
        /// </returns>
        public static string FormatVersion(string version)
        {
            if (version == null)
            {
                return null;
            }

            VerifyVersion(version);
            return version;
        }

        /// <summary>
        /// The increment version1.
        /// </summary>
        /// <param name="version">
        /// The version1 1.
        /// </param>
        /// <param name="majorIncrement">
        /// The major increment.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IncrementVersion(string version, bool majorIncrement)
        {
            string[] itemsV1 = version.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int majorVersion = int.Parse(itemsV1[0]);
            if (majorIncrement)
            {
                return ++majorVersion + ".0";
            }

            int minorVersion = 0;
            if (itemsV1.Length > 1)
            {
                minorVersion = int.Parse(itemsV1[1]);
            }

            minorVersion = minorVersion + 1;
            return majorVersion + "." + minorVersion;
        }

        /// <summary>
        /// Returns true if <paramref name="version1"/> has a higher version than <paramref name="version2"/>
        /// </summary>
        /// <param name="version1">
        /// a String representing a valid SDMX version
        /// </param>
        /// <param name="version2">
        /// a String representing a valid SDMX version
        /// </param>
        /// <returns>
        /// true if <paramref name="version1"/> has a higher version then <paramref name="version2"/> 
        /// </returns>
        public static bool IsHigherVersion(string version1, string version2)
        {
            VerifyVersion(version1);
            VerifyVersion(version2);

            string[] itemsV1 = version1.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string[] itemsV2 = version2.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int limit = (itemsV1.Length > itemsV2.Length) ? itemsV2.Length : itemsV1.Length;

            for (int i = 0; i < limit; i++)
            {
                decimal value1 = decimal.Parse(itemsV1[i]);
                decimal value2 = decimal.Parse(itemsV2[i]);
                int compare = value1.CompareTo(value2);
                if (compare != 0)
                {
                    return compare > 0;
                }
            }

            return itemsV1.Length > itemsV2.Length;
        }

        /// <summary>
        /// Returns if a version1 String is a valid version1 number for the registry.
        /// </summary>
        /// <param name="version">
        /// the String to check validity of 
        /// </param>
        /// <returns>
        /// true if the version1 is valid for the registry 
        /// </returns>
        public static bool ValidVersion(string version)
        {
            return !string.IsNullOrEmpty(version) && _versionRegex.IsMatch(version);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Verifies a version1 is in the correct format.
        ///  The accepted format is a positive integer followed by zero or more sub-values, where a sub-value is 
        ///  a period (.) followed by a positive integer. There may only be 10 characters in total.
        ///  Examples of legal values: 1 , 1.1 , 1.2.3.4.5.6.7 . 12341.122423221 .
        ///  <p/>
        ///  An ArgumentException is thrown if the String argument cannot be represented as a number.
        /// </summary>
        /// <param name="version">
        /// The string representation of the version1 
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// If an illegal value was supplied
        /// </exception>
        private static void VerifyVersion(string version)
        {
            if (version == null)
            {
                throw new ArgumentException("Null version1 supplied.");
            }

            if (version.Equals("*"))
            {
                return;
            }

            if (!_versionRegex.IsMatch(version))
            {
                throw new ArgumentException("Illegal in version1 supplied '" + version + "'");
            }

            if (version.Length > 10)
            {
                throw new ArgumentException("Supplied version contains more characters than is allowed. The version can only be 10 characters at most.");
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthUtils.cs" company="Eurostat">
//   Date Created : 2011-06-17
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds a collection of helper static methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This class holds a collection of helper static methods
    /// </summary>
    public static class AuthUtils
    {
        #region Public Methods

        /// <summary>
        /// Convert the specified object base type to the generic type.
        /// </summary>
        /// <typeparam name="T">
        /// The output base type
        /// </typeparam>
        /// <param name="value">
        /// The value to convert
        /// </param>
        /// <returns>
        /// If <c>param</c> is <see cref="DBNull"/> then return the <c>default(T)</c> else the dbValue converted to (T) type
        /// </returns>
        public static T ConvertDBValue<T>(object value)
        {
            if (Convert.IsDBNull(value))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if a configuration setting is set and throw an <see cref="AuthConfigurationException"/> if not
        /// </summary>
        /// <param name="config">
        /// The config artefact to check
        /// </param>
        /// <param name="type">
        /// The type of the calling module
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// See the <see cref="Errors.MissingConfiguration"/>
        /// </exception>
        public static void ValidateConfig(object config, Type type)
        {
            if (config == null)
            {
                throw new AuthConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Errors.MissingConfiguration, type));
            }
        }

        /// <summary>
        /// Validates that all the specified strings exist in the input string in any order
        /// </summary>
        /// <param name="input">
        /// The input string to check
        /// </param>
        /// <param name="values">
        /// A non-null and non empty collection of strings to check
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// strings is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// strings is empty
        /// </exception>
        /// <returns>
        /// True if all the specified strings are included in the input string. Else false.
        /// </returns>
        public static bool ValidateContains(string input, params string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0)
            {
                throw new ArgumentException("strings is empty");
            }

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (string s in values)
            {
                if (!input.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
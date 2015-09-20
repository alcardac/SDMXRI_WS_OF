// -----------------------------------------------------------------------
// <copyright file="DatabaseExtension.cs" company="Eurostat">
//   Date Created : 2013-07-29
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Data;

    /// <summary>
    /// This class contains various database extension methods.
    /// </summary>
    public static class DatabaseExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Convert the specified <paramref name="value"/> to a value suitable for Mapping Store.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ToDbValue(this long value)
        {
            return ToDbValue(value, DBNull.Value);
        }

        /// <summary>
        /// Convert the specified <paramref name="value"/> to a value suitable for Mapping Store.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ToDbValue(this long value, object defaultValue)
        {
            if (value < 0)
            {
                return DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// Convert the specified <paramref name="value" /> to a value suitable for Mapping Store.
        /// </summary>
        /// <typeparam name="T">The type of</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The normalized for DB object.
        /// </returns>
        public static object ToDbValue<T>(this T? value) where T : struct
        {
            return ToDbValue(value, DBNull.Value);
        }

        /// <summary>
        /// Convert the specified <paramref name="value" /> to a value suitable for Mapping Store.
        /// </summary>
        /// <typeparam name="T">The type of</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The normalized for DB object.
        /// </returns>
        public static object ToDbValue<T>(this T? value, object defaultValue) where T : struct
        {
            if (!value.HasValue)
            {
                return defaultValue;
            }

            return value.Value;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="reader"/> has a field with the specified <paramref name="fieldName"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>True if there is a field with name <paramref name="fieldName"/>; otherwise false</returns>
        public static bool HasFieldName(this IDataReader reader, string fieldName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion 
    }
}
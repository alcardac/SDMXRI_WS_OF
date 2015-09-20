// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReaderHelper.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is an wrapper class used to extend the functionality of the
//   <see cref="System.Data.IDataReader" /> by providing methods that retrieve
//   values from the reader and checking them for DBNull values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System;
    using System.Data;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    /// This class is an wrapper class used to extend the functionality of the
    /// <see cref="System.Data.IDataReader"/> by providing methods that retrieve
    /// values from the reader and checking them for DBNull values.
    /// </summary>
    public static class DataReaderHelper
    {
        #region Public Methods

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="bool"/> data type
        /// In case the retrieved value is null, the returned <see cref="bool"/> value is false
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="bool"/>
        /// </returns>
        public static bool GetBoolean(IDataRecord dataReader, string fieldName)
        {
            int index = dataReader.GetOrdinal(fieldName);
            return GetBoolean(dataReader, index);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="bool"/> data type
        /// In case the retrieved value is null, the returned <see cref="bool"/> value is false
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="bool"/>
        /// </returns>
        public static bool GetBoolean(IDataRecord dataReader, int index)
        {
            bool ret = false;

            var value = dataReader.GetValue(index);
            if (!Convert.IsDBNull(value))
            {
                ret = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }

            return ret;
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int16"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int16.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int16"/>
        /// </returns>
        public static short GetInt16(IDataRecord dataReader, string fieldName)
        {
            int index = dataReader.GetOrdinal(fieldName);

            return GetInt16(dataReader, index);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int16"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int16.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int16"/>
        /// </returns>
        public static short GetInt16(IDataRecord dataReader, int index)
        {
            short ret = short.MinValue;

            if (!dataReader.IsDBNull(index))
            {
                ret = dataReader.GetInt16(index);
            }

            return ret;
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int32"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int32.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int32"/>
        /// </returns>
        public static int GetInt32(IDataRecord dataReader, string fieldName)
        {
            int index = dataReader.GetOrdinal(fieldName);

            return GetInt32(dataReader, index);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int32"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int32.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int32"/>
        /// </returns>
        public static int GetInt32(IDataRecord dataReader, int index)
        {
            int ret = int.MinValue;

            if (!dataReader.IsDBNull(index))
            {
                ret = dataReader.GetInt32(index);
            }

            return ret;
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int64"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int64.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int64"/>
        /// </returns>
        public static long GetInt64(IDataRecord dataReader, string fieldName)
        {
            int index = dataReader.GetOrdinal(fieldName);

            return GetInt64(dataReader, index);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="Int64"/> data type
        /// In case the retrieved value is null, the returned value is <see cref="Int64.MinValue"/>
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="Int64"/>
        /// </returns>
        public static long GetInt64(IDataRecord dataReader, int index)
        {
            long ret = long.MinValue;

            if (!dataReader.IsDBNull(index))
            {
                ret = dataReader.GetInt64(index);
            }

            return ret;
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to string data type
        /// In case the retrieved value is null, the returned value is also null
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as string
        /// </returns>
        public static string GetString(IDataRecord dataReader, string fieldName)
        {
            int index = dataReader.GetOrdinal(fieldName);
            return GetString(dataReader, index);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to string data type
        /// In case the retrieved value is null, the returned value is also null
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as string
        /// </returns>
        public static string GetString(IDataRecord dataReader, int index)
        {
            var value = dataReader.GetValue(index);

            // tasos: added this check because it thrown an exception
            // for fields with double/float type at GetString()
            // ret = datareader.GetString(index);
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        #endregion
        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="DateTime"/> data type
        /// In case the retrieved value is null, the returned value is also null
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="ordinal">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="DateTime"/>
        /// </returns>
        public static DateTime? GetStringDate(IDataRecord dataReader, int ordinal)
        {
            DateTime? ret = null;
            var value = dataReader.GetValue(ordinal);
            if (!Convert.IsDBNull(value))
            {
                var str = value as string;
                DateTime dateTime;
                if (str != null && DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime))
                {
                    ret = dateTime;
                }
                else
                {
                    ret = value as DateTime?;
                    if (ret == null && value is long)
                    {
                       ret = new DateTime((long)value); 
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="TertiaryBool"/> data type
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="fieldName">
        /// The name of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="TertiaryBool"/>
        /// </returns>
        public static TertiaryBool GetTristate(IDataRecord dataReader, string fieldName)
        {
            var ordinal = dataReader.GetOrdinal(fieldName);
            return GetTristate(dataReader, ordinal);
        }

        /// <summary>
        /// The method retrieve a value from the reader and cast it to <see cref="TertiaryBool"/> data type
        /// </summary>
        /// <param name="dataReader">
        /// The source for reading the data
        /// </param>
        /// <param name="index">
        /// The index of the column containing the value
        /// </param>
        /// <returns>
        /// The extracted value as <see cref="TertiaryBool"/>
        /// </returns>
        public static TertiaryBool GetTristate(IDataRecord dataReader, int index)
        {
            bool? ret = null;

            var value = dataReader.GetValue(index);
            if (!Convert.IsDBNull(value))
            {
                ret = Convert.ToBoolean(dataReader.GetValue(index), CultureInfo.InvariantCulture);
            }

            return SdmxObjectUtil.CreateTertiary(ret);
        }
    }
}
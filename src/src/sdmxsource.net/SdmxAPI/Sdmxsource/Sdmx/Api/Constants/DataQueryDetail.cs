// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryDetail.cs" company="Eurostat">
//   Date Created : 2013-04-01
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
    ///     Detail of data query; possible options are:
    ///     <ul>
    ///         <li>data only - Exclude attributes and groups</li>
    ///         <li>series keys only - return only the series elements and dimensions that make up the series key</li>
    ///         <li>full - Return everything</li>
    ///         <li>no data -</li>
    ///     </ul>
    /// </summary>
    public enum DataQueryDetailEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     Exclude attributes and groups
        /// </summary>
        DataOnly,

        /// <summary>
        ///     Return only the series elements and dimensions that make up the series key
        /// </summary>
        SeriesKeysOnly,

        /// <summary>
        ///     Return everything
        /// </summary>
        Full,

        /// <summary>
        ///     No data
        /// </summary>
        NoData
    }

    /// <summary>
    ///     The data query detail.
    /// </summary>
    public class DataQueryDetail : BaseConstantType<DataQueryDetailEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<DataQueryDetailEnumType, DataQueryDetail> Instances =
            new Dictionary<DataQueryDetailEnumType, DataQueryDetail>
                {
                    {
                        DataQueryDetailEnumType.DataOnly, 
                        new DataQueryDetail(
                        DataQueryDetailEnumType.DataOnly, 
                        "dataonly")
                    }, 
                    {
                        DataQueryDetailEnumType.SeriesKeysOnly, 
                        new DataQueryDetail(
                        DataQueryDetailEnumType.SeriesKeysOnly, 
                        "serieskeysonly")
                    }, 
                    {
                        DataQueryDetailEnumType.Full, 
                        new DataQueryDetail(
                        DataQueryDetailEnumType.Full, "full")
                    }, 
                    {
                        DataQueryDetailEnumType.NoData, 
                        new DataQueryDetail(
                        DataQueryDetailEnumType.NoData, "nodata")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _rest param.
        /// </summary>
        private readonly string _restParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryDetail"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="restParam">
        /// The rest param.
        /// </param>
        private DataQueryDetail(DataQueryDetailEnumType enumType, string restParam)
            : base(enumType)
        {
            this._restParam = restParam;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="DataQueryDetail" />
        /// </summary>
        public static IEnumerable<DataQueryDetail> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the rest param.
        /// </summary>
        public string RestParam
        {
            get
            {
                return this._restParam;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="DataQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="DataQueryDetail"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static DataQueryDetail GetFromEnum(DataQueryDetailEnumType enumType)
        {
            DataQueryDetail output;
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
        /// The <see cref="DataQueryDetail" /> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception</exception>
        public static DataQueryDetail ParseString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            /**
             * In SDMXQueryData.xsd the value for the detail is SeriesKeyOnly and not serieskeysonly(as in the REST Guidelines)
             * a temporary hack for this value
             */
            if (value.Equals("SeriesKeyOnly"))
            {
                value = "serieskeysonly";
            }

            foreach (DataQueryDetail currentQueryDetail in Values)
            {
                if (currentQueryDetail.RestParam.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (DataQueryDetail currentQueryDetail in Values)
            {
                sb.Append(concat + currentQueryDetail.RestParam);
                concat = ", ";
            }

            throw new SdmxSemmanticException("Unknown Parameter " + value + " allowed parameters: " + sb);
        }

        #endregion
    }
}
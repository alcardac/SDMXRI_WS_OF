// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxSchema.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Defines the different versions of the SDMX-ML schema + EDI / Delimited
    /// </summary>
    public enum SdmxSchemaEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The version one.
        /// </summary>
        VersionOne, 

        /// <summary>
        ///     The version two.
        /// </summary>
        VersionTwo, 

        /// <summary>
        ///     The version two point one.
        /// </summary>
        VersionTwoPointOne, 

        /// <summary>
        ///     The EDI.
        /// </summary>
        Edi, 

        /// <summary>
        ///     The ECV.
        /// </summary>
        Ecv, 

        /// <summary>
        ///     The CSV.
        /// </summary>
        Csv,

        /// <summary>
        /// JSON
        /// </summary>
        Json,

        /// <summary>
        /// The XLSX
        /// </summary>
        Xlsx
    }

    /// <summary>
    ///     The sdmx schema.
    /// </summary>
    public sealed class SdmxSchema : BaseConstantType<SdmxSchemaEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly IDictionary<SdmxSchemaEnumType, SdmxSchema> Instances =
            new Dictionary<SdmxSchemaEnumType, SdmxSchema>
                {
                    {
                        SdmxSchemaEnumType.VersionOne, 
                        new SdmxSchema(SdmxSchemaEnumType.VersionOne, true)
                    }, 
                    {
                        SdmxSchemaEnumType.VersionTwo, 
                        new SdmxSchema(SdmxSchemaEnumType.VersionTwo, true)
                    }, 
                    {
                        SdmxSchemaEnumType.VersionTwoPointOne, 
                        new SdmxSchema(
                        SdmxSchemaEnumType.VersionTwoPointOne, true)
                    }, 
                    {
                        SdmxSchemaEnumType.Edi, 
                        new SdmxSchema(SdmxSchemaEnumType.Edi, false)
                    }, 
                    {
                        SdmxSchemaEnumType.Ecv, 
                        new SdmxSchema(SdmxSchemaEnumType.Ecv, false)
                    }, 
                    {
                        SdmxSchemaEnumType.Csv, 
                        new SdmxSchema(SdmxSchemaEnumType.Csv, false)
                    },
                    {
                        SdmxSchemaEnumType.Json, 
                        new SdmxSchema(SdmxSchemaEnumType.Json, false)
                    },
                    {
                        SdmxSchemaEnumType.Xlsx, 
                        new SdmxSchema(SdmxSchemaEnumType.Xlsx, false)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _xml format.
        /// </summary>
        private readonly bool _xmlFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxSchema"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="xmlFormat">
        /// The xml format.
        /// </param>
        private SdmxSchema(SdmxSchemaEnumType format, bool xmlFormat)
            : base(format)
        {
            this._xmlFormat = xmlFormat;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all instances
        /// </summary>
        public static IEnumerable<SdmxSchema> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="SdmxSchema"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="SdmxSchema"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static SdmxSchema GetFromEnum(SdmxSchemaEnumType enumType)
        {
            SdmxSchema output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        ///     Gets true is this SDMX_SCHEMA is representing an XML (SDMX) format.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsXmlFormat()
        {
            return this._xmlFormat;
        }

        /// <summary>
        ///     Gets a slightly more human-readable version of the toString() method.
        /// </summary>
        /// <returns> A string representing this Enumeration </returns>
        public string ToEnglishString()
        {
            switch (this.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                case SdmxSchemaEnumType.VersionTwo:
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return "SDMX " + this;
            }

            return this.ToString();
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" /> .
        /// </returns>
        public override string ToString()
        {
            switch (this.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                    return "1.0";
                case SdmxSchemaEnumType.VersionTwo:
                    return "2.0";
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return "2.1";
                case SdmxSchemaEnumType.Edi:
                    return "SDMX-EDI";
                case SdmxSchemaEnumType.Ecv:
                    return "CSV";
                case SdmxSchemaEnumType.Csv:
                    return "ECV";
                case SdmxSchemaEnumType.Json:
                    return "JSON";
                case SdmxSchemaEnumType.Xlsx:
                    return "XLSX";
            }

            return this.EnumType.ToString();
        }

        #endregion
    }
}
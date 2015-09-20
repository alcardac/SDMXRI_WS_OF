// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureOutputFormat.cs" company="Eurostat">
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
    ///   The structure output format enum type.
    /// </summary>
    public enum StructureOutputFormatEnumType
    {
        /// <summary>
        ///   Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///   The sdmx v 1 structure document.
        /// </summary>
        SdmxV1StructureDocument, 

        /// <summary>
        ///   The sdmx v 2 structure document.
        /// </summary>
        SdmxV2StructureDocument, 

        /// <summary>
        ///   The sdmx v 2 registry submit document.
        /// </summary>
        SdmxV2RegistrySubmitDocument, 

        /// <summary>
        ///   The sdmx v 2 registry query response document.
        /// </summary>
        SdmxV2RegistryQueryResponseDocument, 

        /// <summary>
        ///   The sdmx v 21 structure document.
        /// </summary>
        SdmxV21StructureDocument, 

        /// <summary>
        ///   The sdmx v 21 registry submit document.
        /// </summary>
        SdmxV21RegistrySubmitDocument, 

        /// <summary>
        ///   The sdmx v 21 query response document.
        /// </summary>
        SdmxV21QueryResponseDocument, 

        /// <summary>
        ///   The edi.
        /// </summary>
        Edi, 

        /// <summary>
        ///   The csv.
        /// </summary>
        Csv,

        /// <summary>
        /// The XLSX
        /// </summary>
        Xlsx
        

    }

    /// <summary>
    ///   The structure output format.
    /// </summary>
    public class StructureOutputFormat : BaseConstantType<StructureOutputFormatEnumType>
    {
        #region Static Fields

        /// <summary>
        ///   The _instances.
        /// </summary>
        private static readonly Dictionary<StructureOutputFormatEnumType, StructureOutputFormat> Instances =
            new Dictionary<StructureOutputFormatEnumType, StructureOutputFormat>
                {
                    {
                        StructureOutputFormatEnumType.SdmxV1StructureDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV1StructureDocument, 
                        SdmxSchemaEnumType.VersionOne, 
                        false, 
                        false)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV2StructureDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV2StructureDocument, 
                        SdmxSchemaEnumType.VersionTwo, 
                        false, 
                        false)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV2RegistrySubmitDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV2RegistrySubmitDocument, 
                        SdmxSchemaEnumType.VersionTwo, 
                        false, 
                        true)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument, 
                        SdmxSchemaEnumType.VersionTwo, 
                        true, 
                        true)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV21StructureDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV21StructureDocument, 
                        SdmxSchemaEnumType.VersionTwoPointOne, 
                        false, 
                        false)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV21RegistrySubmitDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV21RegistrySubmitDocument, 
                        SdmxSchemaEnumType.VersionTwoPointOne, 
                        false, 
                        true)
                    }, 
                    {
                        StructureOutputFormatEnumType.SdmxV21QueryResponseDocument, 
                        new StructureOutputFormat(
                        StructureOutputFormatEnumType.SdmxV21QueryResponseDocument, 
                        SdmxSchemaEnumType.VersionTwoPointOne, 
                        true, 
                        false)
                    }, 
                    {
                        StructureOutputFormatEnumType.Edi, 
                        new StructureOutputFormat(StructureOutputFormatEnumType.Edi, SdmxSchemaEnumType.Edi, false, false)
                    }, 
                    {
                        StructureOutputFormatEnumType.Csv, 
                        new StructureOutputFormat(StructureOutputFormatEnumType.Csv, SdmxSchemaEnumType.Csv, false, false)
                    }, 
                    {
                        StructureOutputFormatEnumType.Xlsx, 
                        new StructureOutputFormat(StructureOutputFormatEnumType.Xlsx, SdmxSchemaEnumType.Xlsx, false, false)
                    }, 
                };

        #endregion

        #region Fields

        /// <summary>
        ///   The _is query response.
        /// </summary>
        private readonly bool _isQueryResponse;

        /// <summary>
        ///   The _is registry document.
        /// </summary>
        private readonly bool _isRegistryDocument;

        /// <summary>
        ///   The _output version.
        /// </summary>
        private readonly SdmxSchemaEnumType _outputVersion;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureOutputFormat"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="isQueryResponse">
        /// The is query response. 
        /// </param>
        /// <param name="isRegistryDocument">
        /// The is registry document. 
        /// </param>
        private StructureOutputFormat(
            StructureOutputFormatEnumType enumType, 
            SdmxSchemaEnumType version, 
            bool isQueryResponse, 
            bool isRegistryDocument)
            : base(enumType)
        {
            this._outputVersion = version;
            this._isQueryResponse = isQueryResponse;
            this._isRegistryDocument = isRegistryDocument;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets all instances
        /// </summary>
        public static IEnumerable<StructureOutputFormat> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether is query response.
        /// </summary>
        public bool IsQueryResponse
        {
            get
            {
                return this._isQueryResponse;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether is registry document.
        /// </summary>
        public bool IsRegistryDocument
        {
            get
            {
                return this._isRegistryDocument;
            }
        }

        /// <summary>
        ///   Gets the output version.
        /// </summary>
        public SdmxSchema OutputVersion
        {
            get
            {
                return SdmxSchema.GetFromEnum(this._outputVersion);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="StructureOutputFormat"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type 
        /// </param>
        /// <returns>
        /// the instance of <see cref="StructureOutputFormat"/> mapped to <paramref name="enumType"/> 
        /// </returns>
        public static StructureOutputFormat GetFromEnum(StructureOutputFormatEnumType enumType)
        {
            StructureOutputFormat output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        #endregion
    }
}
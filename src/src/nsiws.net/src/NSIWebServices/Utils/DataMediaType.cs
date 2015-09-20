// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataMediaType.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mime;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public enum DataMediaEnumType
    {
        /// <summary>
        /// The generic data.
        /// </summary>
        GenericData, 

        /// <summary>
        /// The structure specific data.
        /// </summary>
        StructureSpecificData, 

        /// <summary>
        /// The application xml.
        /// </summary>
        ApplicationXml, 

        /// <summary>
        /// The text xml mime type
        /// </summary>
        TextXml,

        /// <summary>
        /// The compact data.
        /// </summary>
        CompactData, 

        /// <summary>
        /// The cross sectional data.
        /// </summary>
        CrossSectionalData, 

        /// <summary>
        /// The edi data.
        /// </summary>
        EdiData, 

        /// <summary>
        /// The csv data.
        /// </summary>
        CsvData,

        RdfData,
        CsvZipData,
        DsplData,
        JsonData,
        JsonZipData
    }

    /// <summary>
    /// The data media type.
    /// </summary>
    public sealed class DataMediaType : BaseConstantType<DataMediaEnumType>
    {
        #region Static Fields

        /// <summary>
        /// The instances.
        /// </summary>
        private static readonly IDictionary<DataMediaEnumType, DataMediaType> Instances = new Dictionary<DataMediaEnumType, DataMediaType>
                                                                                              {
                                                                                                  {
                                                                                                      DataMediaEnumType.GenericData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.GenericData, 
                                                                                                      SdmxMedia.GenericData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.StructureSpecificData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.StructureSpecificData, 
                                                                                                      SdmxMedia.StructureSpecificData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Compact))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.ApplicationXml, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.ApplicationXml, 
                                                                                                      SdmxMedia.ApplicationXml, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.TextXml, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.TextXml, 
                                                                                                      SdmxMedia.TextXml, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.CompactData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.CompactData, 
                                                                                                      SdmxMedia.CompactData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Compact))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.CrossSectionalData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.CrossSectionalData, 
                                                                                                      SdmxMedia.CrossSectionalData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.CrossSectional))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.EdiData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.EdiData, 
                                                                                                      SdmxMedia.EdiData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Edi))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.CsvData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.CsvData, 
                                                                                                      SdmxMedia.CsvData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Csv))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.CsvZipData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.CsvZipData, 
                                                                                                      SdmxMedia.CsvZipData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.RdfData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.RdfData, 
                                                                                                      SdmxMedia.RdfData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.DsplData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.DsplData, 
                                                                                                      SdmxMedia.DsplData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.JsonData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.JsonData, 
                                                                                                      SdmxMedia.JsonData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }, 
                                                                                                  {
                                                                                                      DataMediaEnumType.JsonZipData, 
                                                                                                      new DataMediaType(
                                                                                                      DataMediaEnumType.JsonZipData, 
                                                                                                      SdmxMedia.JsonZipData, 
                                                                                                      BaseDataFormat.GetFromEnum(
                                                                                                          BaseDataFormatEnumType.Generic))
                                                                                                  }
                                                                                              };

        #endregion

        #region Fields

        /// <summary>
        /// The _format.
        /// </summary>
        private readonly BaseDataFormat _format;

        /// <summary>
        /// The _media type name.
        /// </summary>
        private readonly string _mediaTypeName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMediaType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="mediaTypeName">
        /// The media type name.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        private DataMediaType(DataMediaEnumType enumType, string mediaTypeName, BaseDataFormat format)
            : base(enumType)
        {
            this._mediaTypeName = mediaTypeName;
            this._format = format;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the values.
        /// </summary>
        public static IEnumerable<DataMediaType> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        public BaseDataFormat Format
        {
            get
            {
                return this._format;
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary>
        public ContentType MediaType
        {
            get
            {
                var contentType = new ContentType();
                contentType.MediaType = this._mediaTypeName;
                return contentType;
            }
        }

        /// <summary>
        /// Gets the media type name.
        /// </summary>
        public string MediaTypeName
        {
            get
            {
                return this._mediaTypeName;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get from enum.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <returns>
        /// The <see cref="DataMediaType"/>.
        /// </returns>
        public static DataMediaType GetFromEnum(DataMediaEnumType enumType)
        {
            DataMediaType output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The get type from name.
        /// </summary>
        /// <param name="mediaTypeName">
        /// The media type name.
        /// </param>
        /// <returns>
        /// The <see cref="DataMediaType"/>.
        /// </returns>
        public static DataMediaType GetTypeFromName(string mediaTypeName)
        {
            if (string.IsNullOrEmpty(mediaTypeName) || new ContentType(mediaTypeName).MediaType.EndsWith("/*", StringComparison.Ordinal))
            {
                mediaTypeName = GetFromEnum(DataMediaEnumType.GenericData).MediaTypeName;
            }

            return Values.FirstOrDefault(m => m.MediaTypeName.Equals(mediaTypeName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// The get media type version.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="ContentType"/>.
        /// </returns>
        public ContentType GetMediaTypeVersion(string version)
        {
            string outVersion = version;
            if (string.IsNullOrWhiteSpace(version))
            {
                version = "2.1";
            }

            switch (EnumType)
            {
                case DataMediaEnumType.GenericData:
                    outVersion = string.IsNullOrWhiteSpace(version) ? "2.1" : version;
                    break;
                case DataMediaEnumType.StructureSpecificData:
                    outVersion = string.IsNullOrWhiteSpace(version) ? "2.1" : version;
                    if (!"2.1".Equals(outVersion))
                    {
                        return null;
                    }

                    break;
                case DataMediaEnumType.ApplicationXml:
                case DataMediaEnumType.TextXml:
                    return GetFromEnum(DataMediaEnumType.GenericData).GetMediaTypeVersion("2.1");
                case DataMediaEnumType.CompactData:
                case DataMediaEnumType.CrossSectionalData:
                    outVersion = string.IsNullOrWhiteSpace(version) ? "2.0" : version;
                    if (!"2.0".Equals(outVersion))
                    {
                        return null;
                    } 

                    break;
                case DataMediaEnumType.EdiData:
                case DataMediaEnumType.CsvData:
                    return this.MediaType;
            }

            var contentType = new ContentType(string.Format(CultureInfo.InvariantCulture, "{0};version={1}", this._mediaTypeName, outVersion));

            return contentType;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this._mediaTypeName;
        }

        #endregion
    }
}
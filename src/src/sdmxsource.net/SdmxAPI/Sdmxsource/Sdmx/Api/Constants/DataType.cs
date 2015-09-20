// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataType.cs" company="Eurostat">
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
    using System.Text;

    #endregion

    /// <summary>
    ///   Enumerated list of all the different data formats that SDMX supports.
    ///   <p />
    ///   DataType contains the means to retrieve the underlying <b>SWSDMX_SCHEMA ENUM</b> and the
    ///   underlying <b>BASE_DATA_FORMAT</b> ENUM
    /// </summary>
    public enum DataEnumType
    {
        /// <summary>
        ///   Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///   The generic 10.
        /// </summary>
        Generic10, 

        /// <summary>
        ///   The cross sectional 10.
        /// </summary>
        CrossSectional10, 

        /// <summary>
        ///   The utility 10.
        /// </summary>
        Utility10, 

        /// <summary>
        ///   The compact 10.
        /// </summary>
        Compact10, 

        /// <summary>
        ///   The generic 20.
        /// </summary>
        Generic20, 

        /// <summary>
        ///   The cross sectional 20.
        /// </summary>
        CrossSectional20, 

        /// <summary>
        ///   The utility 20.
        /// </summary>
        Utility20, 

        /// <summary>
        ///   The compact 20.
        /// </summary>
        Compact20, 

        /// <summary>
        ///   The message group 10 compact.
        /// </summary>
        MessageGroup10Compact, 

        /// <summary>
        ///   The message group 10 generic.
        /// </summary>
        MessageGroup10Generic, 

        /// <summary>
        ///   The message group 10 utility.
        /// </summary>
        MessageGroup10Utility, 

        /// <summary>
        ///   The message group 20 compact.
        /// </summary>
        MessageGroup20Compact, 

        /// <summary>
        ///   The message group 20 generic.
        /// </summary>
        MessageGroup20Generic, 

        /// <summary>
        ///   The message group 20 utility.
        /// </summary>
        MessageGroup20Utility, 

        /// <summary>
        ///   The generic 21.
        /// </summary>
        Generic21, 

        /// <summary>
        ///   The compact 21.
        /// </summary>
        Compact21, 

        /// <summary>
        ///   The generic 21 xs.
        /// </summary>
        Generic21Xs, 

        /// <summary>
        ///   The compact 21 xs.
        /// </summary>
        Compact21Xs, 

        /// <summary>
        ///   The edi ts.
        /// </summary>
        EdiTs, 

        /// <summary>
        ///   The csv dates x axis.
        /// </summary>
        CsvDatesXAxis, 

        /// <summary>
        ///   The csv dates y axis.
        /// </summary>
        CsvDatesYAxis, 

        /// <summary>
        ///   The csv dates x axis code names.
        /// </summary>
        CsvDatesXAxisCodeNames, 

        /// <summary>
        ///   The csv dates y axis code names.
        /// </summary>
        CsvDatesYAxisCodeNames, 

        /// <summary>
        ///   The csv dates x axis code id and names.
        /// </summary>
        CsvDatesXAxisCodeIdAndNames, 

        /// <summary>
        ///   The csv dates y axis code id and names.
        /// </summary>
        CsvDatesYAxisCodeIdAndNames,

        /// <summary>
        /// Json
        /// </summary>
        Json
    }

    /// <summary>
    ///   The data type.
    /// </summary>
    public class DataType : BaseConstantType<DataEnumType>
    {
        #region Static Fields

        /// <summary>
        ///   The _instances.
        /// </summary>
        private static readonly Dictionary<DataEnumType, DataType> Instances = new Dictionary<DataEnumType, DataType>
            {
                {
                    DataEnumType.Generic10, 
                    new DataType(DataEnumType.Generic10, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Generic)
                }, 
                {
                    DataEnumType.CrossSectional10, 
                    new DataType(
                    DataEnumType.CrossSectional10, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.CrossSectional)
                }, 
                {
                    DataEnumType.Utility10, 
                    new DataType(DataEnumType.Utility10, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Utility)
                }, 
                {
                    DataEnumType.Compact10, 
                    new DataType(DataEnumType.Compact10, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Compact)
                }, 
                {
                    DataEnumType.Generic20, 
                    new DataType(DataEnumType.Generic20, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Generic)
                }, 
                {
                    DataEnumType.CrossSectional20, 
                    new DataType(
                    DataEnumType.CrossSectional20, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.CrossSectional)
                }, 
                {
                    DataEnumType.Utility20, 
                    new DataType(DataEnumType.Utility20, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Utility)
                }, 
                {
                    DataEnumType.Compact20, 
                    new DataType(DataEnumType.Compact20, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Compact)
                }, 
                {
                    DataEnumType.MessageGroup10Compact, 
                    new DataType(
                    DataEnumType.MessageGroup10Compact, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Compact)
                }, 
                {
                    DataEnumType.MessageGroup10Generic, 
                    new DataType(
                    DataEnumType.MessageGroup10Generic, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Generic)
                }, 
                {
                    DataEnumType.MessageGroup10Utility, 
                    new DataType(
                    DataEnumType.MessageGroup10Utility, SdmxSchemaEnumType.VersionOne, BaseDataFormatEnumType.Utility)
                }, 
                {
                    DataEnumType.MessageGroup20Compact, 
                    new DataType(
                    DataEnumType.MessageGroup20Compact, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Compact)
                }, 
                {
                    DataEnumType.MessageGroup20Generic, 
                    new DataType(
                    DataEnumType.MessageGroup20Generic, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Generic)
                }, 
                {
                    DataEnumType.MessageGroup20Utility, 
                    new DataType(
                    DataEnumType.MessageGroup20Utility, SdmxSchemaEnumType.VersionTwo, BaseDataFormatEnumType.Utility)
                }, 
                {
                    DataEnumType.Generic21, 
                    new DataType(
                    DataEnumType.Generic21, SdmxSchemaEnumType.VersionTwoPointOne, BaseDataFormatEnumType.Generic)
                }, 
                {
                    DataEnumType.Compact21, 
                    new DataType(
                    DataEnumType.Compact21, SdmxSchemaEnumType.VersionTwoPointOne, BaseDataFormatEnumType.Compact)
                }, 
                {
                    DataEnumType.Generic21Xs, 
                    new DataType(
                    DataEnumType.Generic21Xs, 
                    SdmxSchemaEnumType.VersionTwoPointOne, 
                    BaseDataFormatEnumType.CrossSectional)
                }, 
                {
                    DataEnumType.Compact21Xs, 
                    new DataType(
                    DataEnumType.Compact21Xs, 
                    SdmxSchemaEnumType.VersionTwoPointOne, 
                    BaseDataFormatEnumType.CrossSectional)
                }, 
                {
                    DataEnumType.EdiTs, 
                    new DataType(DataEnumType.EdiTs, SdmxSchemaEnumType.Edi, BaseDataFormatEnumType.Edi)
                },
                {
                    DataEnumType.Json, 
                    new DataType(DataEnumType.Json, SdmxSchemaEnumType.Json, BaseDataFormatEnumType.Json)
                }


            };

        #endregion

        #region Fields

        /// <summary>
        ///   The _base data format.
        /// </summary>
        private readonly BaseDataFormatEnumType _baseDataFormat;

        /// <summary>
        ///   The _schema version.
        /// </summary>
        private readonly SdmxSchemaEnumType _schemaVersion;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type. 
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version. 
        /// </param>
        /// <param name="baseDataFormat">
        /// The base data format. 
        /// </param>
        private DataType(DataEnumType enumType, SdmxSchemaEnumType schemaVersion, BaseDataFormatEnumType baseDataFormat)
            : base(enumType)
        {
            this._schemaVersion = schemaVersion;
            this._baseDataFormat = baseDataFormat;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the values.
        /// </summary>
        public static IEnumerable<DataType> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///   Gets the base data format.
        /// </summary>
        public BaseDataFormat BaseDataFormat
        {
            // TODO cache result we are dealing with constants.
            get
            {
                return BaseDataFormat.GetFromEnum(this._baseDataFormat);
            }
        }

        /// <summary>
        ///   Gets the schema version.
        /// </summary>
        public SdmxSchema SchemaVersion
        {
            get
            {
                return SdmxSchema.GetFromEnum(this._schemaVersion);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="DataType"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type 
        /// </param>
        /// <returns>
        /// the instance of <see cref="DataType"/> mapped to <paramref name="enumType"/> 
        /// </returns>
        public static DataType GetFromEnum(DataEnumType enumType)
        {
            DataType output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this._baseDataFormat.ToString());
            sb.Append(" ");
            sb.Append(this._schemaVersion.ToString());
            return sb.ToString();
        }

        #endregion
    }
}
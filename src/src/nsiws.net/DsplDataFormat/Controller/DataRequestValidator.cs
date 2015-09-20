// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRequestValidator.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="IDataRequestValidator" /> validator.
//   It validates the data query, the SDMX version and data format.
//   It throws an exception when invalid combination is requested.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace DsplDataFormat.Controller
{
    using System;

    using Estat.Nsi.DataRetriever;
    using Estat.Sri.SdmxParseBase.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Estat.Sri.Ws.Controllers.Controller;

    /// <summary>
    ///     The <see cref="IDataRequestValidator" /> validator.
    ///     It validates the data query, the SDMX version and data format.
    ///     It throws an exception when invalid combination is requested.
    /// </summary>
    public class DataRequestValidator : IDataRequestValidator
    {
        #region Fields

        /// <summary>
        ///     The _format.
        /// </summary>
        private readonly BaseDataFormat _format;

        /// <summary>
        ///     The _schema.
        /// </summary>
        private readonly SdmxSchema _schema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRequestValidator"/> class.
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public DataRequestValidator(BaseDataFormat format, SdmxSchema schema)
        {
            this._format = format;
            this._schema = schema;
        }

        public DataRequestValidator()
        {
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Validates the specified data query.
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Unsupported format or validation error.
        /// </exception>
        //public void Validate(IBaseDataQuery dataQuery)
        //{
        //    if (dataQuery == null)
        //    {
        //        throw new ArgumentNullException("dataQuery");
        //    }

        //    switch (this._schema.EnumType)
        //    {
        //        case SdmxSchemaEnumType.Null:
        //            break;
        //        case SdmxSchemaEnumType.VersionOne:
        //            break;
        //        case SdmxSchemaEnumType.VersionTwo:
        //            ValidateSdmxV20(dataQuery, this._format);
        //            break;
        //        case SdmxSchemaEnumType.VersionTwoPointOne:
        //            ValidateSdmxV21(dataQuery);
        //            break;
        //        case SdmxSchemaEnumType.Edi:
        //            ValidateSdmxV20TimeSeries(dataQuery);
        //            break;
        //        case SdmxSchemaEnumType.Ecv:
        //            break;
        //        case SdmxSchemaEnumType.Csv:
        //            break;
        //        case SdmxSchemaEnumType.Json:
        //            break;
        //        default:
        //            throw new SdmxSemmanticException(string.Format("Unsupported format {0}", this._schema.ToEnglishString()));
        //    }
        //}

        public void Validate(IBaseDataQuery dataQuery)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            ValidateSdmxV21(dataQuery);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the specified <paramref name="errors"/> contains any errors. i.e. is not empty.
        ///     If it is not empty an <see cref="SdmxSemmanticException"/> is thrown
        /// </summary>
        /// <param name="errors">
        /// The string containing
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// the specified <paramref name="errors"/> is not empty and contains errors
        /// </exception>
        private static void ValidateErrors(string errors)
        {
            if (!string.IsNullOrEmpty(errors))
            {
                throw new SdmxSemmanticException(errors);
            }
        }

        /// <summary>
        /// Validates the SDMX V20.
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNoResultsException">
        /// This dataflow uses SDMX v2.0 only
        ///     DataStructureDefinition.
        /// </exception>
        private static void ValidateSdmxV20(IBaseDataQuery dataQuery, BaseConstantType<BaseDataFormatEnumType> format)
        {
            if (!dataQuery.DataStructure.IsCompatible(SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                // TODO check java message/error in this case
                throw new SdmxNoResultsException("This dataflow uses SDMX v2.1 only DataStructureDefinition.");
            }

            switch (format.EnumType)
            {
                case BaseDataFormatEnumType.Null:
                    break;
                case BaseDataFormatEnumType.Generic:
                case BaseDataFormatEnumType.Compact:
                case BaseDataFormatEnumType.Utility:
                case BaseDataFormatEnumType.Edi:
                    ValidateSdmxV20TimeSeries(dataQuery);
                    break;
                case BaseDataFormatEnumType.CrossSectional:
                    ValidateErrors(Validator.ValidateForCrossSectional(dataQuery.DataStructure));
                    break;
                default:
                    throw new SdmxSemmanticException("SDMX v2.0 Unsupported format " + format);
            }
        }

        /// <summary>
        /// Validates a SDMX V20 <paramref name="dataQuery"/> if it is valid for Time Series data.
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        /// <exception cref="DataRetrieverException">
        /// It is not compatible with time series
        /// </exception>
        private static void ValidateSdmxV20TimeSeries(IBaseDataQuery dataQuery)
        {
            var errors = Validator.ValidateForCompact(dataQuery.DataStructure);
            ValidateErrors(errors);
        }

        /// <summary>
        /// Validates the SDMX V21.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNoResultsException">
        /// This dataflow uses SDMX v2.0 only
        ///     DataStructureDefinition.
        /// </exception>
        private static void ValidateSdmxV21(IBaseDataQuery query)
        {
            // ReSharper restore UnusedParameter.Local
            // TODO check java message/error in this case
            if (query.DataStructure is ICrossSectionalDataStructureObject)
            {
                throw new SdmxSemmanticException("This dataflow uses SDMX v2.0 only DataStructureDefinition.");
            }
        }

        #endregion
    }
}
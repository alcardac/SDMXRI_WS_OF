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
namespace RDFProvider.Controller
{
    using System;

    using Estat.Nsi.DataRetriever;
    //using Estat.Sri.SdmxParseBase.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

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
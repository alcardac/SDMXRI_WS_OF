// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSdmxDataRetrievalWithCrossWriter.cs" company="Eurostat">
//   Date Created : 2014-07-08
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The abstract sdmx data retrieval with cross writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Manager.Data
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;

    /// <summary>
    /// The decorator class for <see cref="ISdmxDataRetrievalWithCrossWriter" />
    /// </summary>
    public abstract class AbstractSdmxDataRetrievalWithCrossWriter : ISdmxDataRetrievalWithCrossWriter
    {
        #region Fields

        /// <summary>
        /// The _data retrieval with cross writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithCrossWriter _dataRetrievalWithCrossWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSdmxDataRetrievalWithCrossWriter"/> class.
        /// </summary>
        /// <param name="dataRetrievalWithCrossWriter">
        /// The data retrieval with cross writer.
        /// </param>
        protected AbstractSdmxDataRetrievalWithCrossWriter(ISdmxDataRetrievalWithCrossWriter dataRetrievalWithCrossWriter)
        {
            if (dataRetrievalWithCrossWriter == null)
            {
                throw new ArgumentNullException("dataRetrievalWithCrossWriter");
            }

            this._dataRetrievalWithCrossWriter = dataRetrievalWithCrossWriter;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        /// <param name="dataWriter">
        /// The data writer.
        /// </param>
        public virtual void GetData(IDataQuery dataQuery, ICrossSectionalWriterEngine dataWriter)
        {
            this._dataRetrievalWithCrossWriter.GetData(dataQuery, dataWriter);
        }

        #endregion
    }
}
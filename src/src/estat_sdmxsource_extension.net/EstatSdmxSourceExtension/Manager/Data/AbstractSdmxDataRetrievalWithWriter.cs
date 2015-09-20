// -----------------------------------------------------------------------
// <copyright file="AbstractSdmxDataRetrievalWithWriter.cs" company="Eurostat">
//   Date Created : 2014-07-08
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Manager.Data
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// The decorator class for <see cref="ISdmxDataRetrievalWithWriter" />
    /// </summary>
    public abstract class AbstractSdmxDataRetrievalWithWriter : ISdmxDataRetrievalWithWriter
    {
        /// <summary>
        /// The _data retrieval with writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithWriter _dataRetrievalWithWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSdmxDataRetrievalWithWriter"/> class.
        /// </summary>
        /// <param name="dataRetrievalWithWriter">The data retrieval with writer.</param>
        protected AbstractSdmxDataRetrievalWithWriter(ISdmxDataRetrievalWithWriter dataRetrievalWithWriter)
        {
            if (dataRetrievalWithWriter == null)
            {
                throw new ArgumentNullException("dataRetrievalWithWriter");
            }

            this._dataRetrievalWithWriter = dataRetrievalWithWriter;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="dataWriter">The data writer.</param>
        public virtual void GetData(IDataQuery dataQuery, IDataWriterEngine dataWriter)
        {
            this._dataRetrievalWithWriter.GetData(dataQuery, dataWriter);
        }
    }
}
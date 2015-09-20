// -----------------------------------------------------------------------
// <copyright file="AbstractAdvancedSdmxDataRetrievalWithWriter .cs" company="Eurostat">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    /// The decorator class for <see cref="IAdvancedSdmxDataRetrievalWithWriter" />
    /// </summary>
    public abstract class AbstractAdvancedSdmxDataRetrievalWithWriter : IAdvancedSdmxDataRetrievalWithWriter
    {
        /// <summary>
        /// The _advanced SDMX data retrieval with writer
        /// </summary>
        private readonly IAdvancedSdmxDataRetrievalWithWriter _advancedSdmxDataRetrievalWithWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAdvancedSdmxDataRetrievalWithWriter"/> class.
        /// </summary>
        /// <param name="advancedSdmxDataRetrievalWithWriter">The advanced SDMX data retrieval with writer.</param>
        protected AbstractAdvancedSdmxDataRetrievalWithWriter(IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter)
        {
            if (advancedSdmxDataRetrievalWithWriter == null)
            {
                throw new ArgumentNullException("advancedSdmxDataRetrievalWithWriter");
            }

            this._advancedSdmxDataRetrievalWithWriter = advancedSdmxDataRetrievalWithWriter;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="dataWriter">The data writer.</param>
        public virtual void GetData(IComplexDataQuery dataQuery, IDataWriterEngine dataWriter)
        {
            this._advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
        }
    }
}
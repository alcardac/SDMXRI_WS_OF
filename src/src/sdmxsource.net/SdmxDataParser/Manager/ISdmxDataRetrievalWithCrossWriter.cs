// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxDataRetrievalWithCrossWriter.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    /// The SDMX Data Retrieval interface for writing the output to <c>SDMX v2.0</c> CrossSectional Datasets.
    /// </summary>
    public interface ISdmxDataRetrievalWithCrossWriter
    {
        /// <summary>
        /// Retrieves data requested in <paramref name="dataQuery"/> and write it to <paramref name="dataWriter"/>
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        /// <param name="dataWriter">
        /// The data writer.
        /// </param>
        void GetData(IDataQuery dataQuery, ICrossSectionalWriterEngine dataWriter);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxDataRetrievalWithWriter.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Retriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using RDFProvider.Writer.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// The SdmxDataRetrievalWithWrtier is capable of executing a DataQuery and writing the response to
    /// the DataWriterEngine.
    /// This SdmxDataRetrievalWithWrtier does not need to concern itself with the response format -
    /// as this is handled by the DataWriterEngine
    /// </summary>
    public interface IRDFDataRetrievalWithWriter
    {
        void GetData(IDataQuery dataQuery, IRDFDataWriterEngine dataWriter);
    }
}

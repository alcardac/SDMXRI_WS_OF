// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxDataRetrievalWithStream.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    /// The SdmxDataRetrievalWithStream is capable of executing a DataQuery and writing the response to
    /// the OutputStream in the requested response format.
    /// </summary>
    public interface ISdmxDataRetrievalWithStream
    {
        void GetData(IDataQuery dataQuery, IDataFormat responseFormat, Stream outputStream);
    }
}

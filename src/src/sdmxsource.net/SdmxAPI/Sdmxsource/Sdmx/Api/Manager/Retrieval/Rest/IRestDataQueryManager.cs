// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestDataQueryManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Rest
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    /// Executes the data query, and writes the response to the output stream in the format specified
    /// </summary>
    public interface IRestDataQueryManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Execute the rest query, write the response out to the output stream based on the data format
        /// </summary>
        /// <param name="dataQuery">
        /// The data query
        /// </param>
        /// <param name="dataFormat">
        /// The data format
        /// </param>
        /// <param name="outPutStream">
        /// The output stream
        /// </param>
        void ExecuteQuery(IRestDataQuery dataQuery, IDataFormat dataFormat, Stream outPutStream);

        #endregion
    }
}

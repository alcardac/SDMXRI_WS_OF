// -----------------------------------------------------------------------
// <copyright file="ISdmxDataSource.cs" company="Eurostat">
//   Date Created : 2014-03-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    /// The SdmxDataSource interface.
    /// </summary>
    public interface ISdmxDataSource : ISdmxDataRetrievalWithWriter
    {
        /// <summary>
        /// Determines whether there is data for the specified dataflow.
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <returns>true if there is data for this dataflow</returns>
        bool HasData(IDataflowObject dataflow);
    }
}
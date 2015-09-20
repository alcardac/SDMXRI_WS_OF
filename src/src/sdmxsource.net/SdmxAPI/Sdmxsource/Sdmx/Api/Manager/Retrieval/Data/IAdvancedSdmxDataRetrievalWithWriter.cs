// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdvancedSdmxDataRetrievalWithWriter.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    #endregion

    /// <summary>
    /// The {@link AdvancedSdmxDataRetrievalWithWriter} is capable of executing a {@link ComplexDataQuery} and writing the response to 
    /// the {@link DataWriterEngine}.
    /// </p>
    /// This {@link AdvancedSdmxDataRetrievalWithWriter} does not need to concern itself with the response format -
    /// as this is handled by the {@link DataWriterEngine}
    /// </summary>
    public interface IAdvancedSdmxDataRetrievalWithWriter
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dataQuery">
        /// The data query
        /// </param>
        /// <param name="dataWriter">
        /// The data writer
        /// </param>
        void GetData(IComplexDataQuery dataQuery, IDataWriterEngine dataWriter);

        #endregion
    }
}

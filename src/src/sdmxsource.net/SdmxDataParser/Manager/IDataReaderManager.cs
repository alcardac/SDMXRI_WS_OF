// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataReaderManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    /// Used to Obtain a DataReaderEngine capable of reading the information contained in a ReadableDataLocation
    /// </summary>
    public interface IDataReaderManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
        /// </summary>
        /// <param name="sourceData">
        /// SourceData - giving access to an InputStream of the data
        /// </param>
        /// <param name="dsd">
        /// Describes the data in terms of the dimensionality
        /// </param>
        /// <param name="dataflowObject">
        /// The data flow object (optional)
        /// </param>
        /// <returns>
        /// The data reader engine
        /// </returns>
        /// throws SdmxNotImplementedException if ReadableDataLocation or DataStructureBean is null, also if additioanlInformation is not of the expected type
        IDataReaderEngine GetDataReaderEngine(
                                              IReadableDataLocation sourceData,
                                              IDataStructureObject dsd,
                                              IDataflowObject dataflowObject);

       /// <summary>
        /// Obtains a DataReaderEngine that is capable of reading the data which is exposed via the ReadableDataLocation
       /// </summary>
       /// <param name="sourceData">
        /// SourceData - giving access to an InputStream of the data
       /// </param>
       /// <param name="retrievalManager">
        /// RetrievalManager - used to obtain the DataStructure(s) that describe the data
       /// </param>
       /// <returns>
       /// The data reader engine
       /// </returns>
        /// throws SdmxNotImplementedException if ReadableDataLocation or DataStructureBean is null, also if additioanlInformation is not of the expected type
        IDataReaderEngine GetDataReaderEngine(
                                              IReadableDataLocation sourceData,
                                              ISdmxObjectRetrievalManager retrievalManager);
 
        #endregion
    }
}

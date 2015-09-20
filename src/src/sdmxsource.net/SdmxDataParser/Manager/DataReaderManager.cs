// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReaderManager.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data reader manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Factory;
    using Org.Sdmxsource.Sdmx.EdiParser.Manager;

    /// <summary>
    ///     The data reader manager.
    /// </summary>
    public class DataReaderManager : IDataReaderManager
    {
        #region Fields

        /// <summary>
        ///     The _engines
        /// </summary>
        private readonly IDataReaderFactory[] _engines;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderManager"/> class.
        /// </summary>
        /// <param name="engines">
        ///     The engines.
        /// </param>
        public DataReaderManager(params IDataReaderFactory[] engines)
        {
            if (engines == null || engines.Length == 0)
            {
                this._engines = new IDataReaderFactory[] { new SdmxDataReaderFactory(new DataInformationManager(), new EdiParseManager()) };
            }
            else
            {
                this._engines = engines;
            }
        }

        #endregion

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
        public IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, IDataStructureObject dsd, IDataflowObject dataflowObject)
        {
            foreach (var currentFactory in this._engines)
            {
                IDataReaderEngine dre = currentFactory.GetDataReaderEngine(sourceData, dsd, dataflowObject);
                if (dre != null)
                {
                    return dre;
                }
            }

            throw new SdmxNotImplementedException("Data format is either not supported, or has an invalid syntax");
        }

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
        public IDataReaderEngine GetDataReaderEngine(IReadableDataLocation sourceData, ISdmxObjectRetrievalManager retrievalManager)
        {
            foreach (var currentFactory in this._engines)
            {
                IDataReaderEngine dre = currentFactory.GetDataReaderEngine(sourceData, retrievalManager);
                if (dre != null)
                {
                    return dre;
                }
            }

            throw new SdmxNotImplementedException("Data format is either not supported, or has an invalid syntax");
        }

        #endregion
    }
}
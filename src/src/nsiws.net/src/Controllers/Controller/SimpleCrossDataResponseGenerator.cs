// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleCrossDataResponseGenerator.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The simple xml response generator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.Ws.Controllers.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;

    /// <summary>
    /// The simple xml response generator.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output writer e.g. XmlWriter or Stream
    /// </typeparam>
    public class SimpleCrossDataResponseGenerator<T> : IResponseGenerator<T, IDataQuery>
    {
        #region Fields

        /// <summary>
        ///     The _data writer builder.
        /// </summary>
        private readonly IWriterBuilder<ICrossSectionalWriterEngine, T> _dataWriterBuilder;

        /// <summary>
        ///     The _sdmx data retrieval with writer.
        /// </summary>
        private readonly ISdmxDataRetrievalWithCrossWriter _sdmxDataRetrievalWithWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCrossDataResponseGenerator{T}"/> class.
        /// </summary>
        /// <param name="sdmxDataRetrievalWithWriter">
        /// The SDMX data retrieval with writer.
        /// </param>
        /// <param name="dataWriterBuilder">
        /// The data Writer Builder.
        /// </param>
        public SimpleCrossDataResponseGenerator(ISdmxDataRetrievalWithCrossWriter sdmxDataRetrievalWithWriter, IWriterBuilder<ICrossSectionalWriterEngine, T> dataWriterBuilder)
        {
            this._sdmxDataRetrievalWithWriter = sdmxDataRetrievalWithWriter;
            this._dataWriterBuilder = dataWriterBuilder;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Generates the response function.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/> that will write the response.
        /// </returns>
        public Action<T, Queue<Action>> GenerateResponseFunction(IDataQuery query)
        {
            return (writer, action) => this._sdmxDataRetrievalWithWriter.GetData(query, this._dataWriterBuilder.Build(writer, action));
        }

        #endregion
    }
}
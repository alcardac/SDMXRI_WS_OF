namespace DsplDataFormat.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.Ws.Controllers.Builder;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Estat.Nsi.DataRetriever;
    using Estat.Sri.Ws.Controllers.Controller;
    using DsplDataFormat.Engine.Manager;



    /// <summary>
    /// The simple xml response generator.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output writer e.g. XmlWriter or Stream
    /// </typeparam>
    public class DsplDataResponseGenerator<T> : IResponseGenerator<T, IDataQuery>
    {
        #region Fields

        /// <summary>
        ///     The _data writer builder.
        /// </summary>
        private readonly IWriterBuilder<IDsplDataWriterEngine, T> _dataWriterBuilder;

        /// <summary>
        ///     The _sdmx data retrieval with writer.
        /// </summary>
        private readonly IDsplDataRetrievalWithWriter _dsplDataRetrievalWithWriter;

        ////andrea
        //private readonly Estat.Nsi.DataRetriever.IDataRetrieverTabular _dsplDataRetrieverTabular;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataResponseGenerator{T}"/> class.
        /// </summary>
        /// <param name="sdmxDataRetrievalWithWriter">
        /// The SDMX data retrieval with writer.
        /// </param>
        /// <param name="dataWriterBuilder">
        /// The data Writer Builder.
        /// </param>
        /// 
        public DsplDataResponseGenerator(IDsplDataRetrievalWithWriter dsplDataRetrievalWithWriter, IWriterBuilder<IDsplDataWriterEngine, T> dataWriterBuilder)
        {
            this._dsplDataRetrievalWithWriter = dsplDataRetrievalWithWriter;
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
            //andrea
            return (writer, actions) => this._dsplDataRetrievalWithWriter.GetData(query, this._dataWriterBuilder.Build(writer, actions));
            //return (writer, actions) => this._jsonDataRetrieverTabular.RetrieveData(query, this._dataWriterBuilder.Build(writer, actions), false);
        }

        #endregion

        Action<T, Queue<Action>> IResponseGenerator<T, IDataQuery>.GenerateResponseFunction(IDataQuery query)
        {

            return (writer, actions) => this._dsplDataRetrievalWithWriter.GetData(query, this._dataWriterBuilder.Build(writer, actions));
           // throw new NotImplementedException();
        }
    }
}

namespace IstatExtension.Controllers.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using IstatExtension.Controllers.Engine;
    
    //SDMXJSON
    using IstatExtension.SdmxJson.DataWriter.Engine;
    using Estat.Sri.Ws.Controllers.Builder;

    /// <summary>
    ///     The <see cref="IDataWriterEngine" /> engine builder from <see cref="Newtonsoft.Json.JsonTextWriter" /> or <see cref="Stream" />
    /// </summary>
    public class SdmxJsonDataWriterBuilder : IWriterBuilder<IDataWriterEngine, Newtonsoft.Json.JsonTextWriter>
    {
        #region Fields

        /// <summary>
        ///     The _data format.
        /// </summary>
        private readonly BaseDataFormat _dataFormat;

        /// <summary>
        ///     The _sdmx schema.
        /// </summary>
        private readonly SdmxSchema _sdmxSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterBuilder"/> class.
        /// </summary>
        /// <param name="dataFormat">
        /// The data Format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The sdmx Schema.
        /// </param>
        public SdmxJsonDataWriterBuilder(BaseDataFormat dataFormat, SdmxSchema sdmxSchema)
        {
            this._dataFormat = dataFormat;
            this._sdmxSchema = sdmxSchema;
        }

        #endregion


        #region SDMXJSON
        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">
        /// Not supported IDataWriterEngine for Stream output
        /// </exception>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        /// 

        //public IDataWriterEngine Build(Newtonsoft.Json.JsonTextWriter writer, Queue<Action> actions)
        //{
        //    return new SdmxJsonDelayedDataWriterEngine(new SdmxJsonBaseDataWriter(writer), actions);
        //}
        public IDataWriterEngine Build(Newtonsoft.Json.JsonTextWriter writer, Queue<Action> actions)
        {
            return new SdmxJsonBaseDataWriter(writer);
        }

        #endregion
    }
}

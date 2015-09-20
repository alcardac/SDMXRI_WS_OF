using Estat.Sri.Ws.Controllers.Builder;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Manager.Output;
using Org.Sdmxsource.Sdmx.Api.Model.Objects;
using System;
using System.Collections.Generic;
using System.Xml;
using Estat.Sri.Ws.Controllers.Controller;
using Org.Sdmxsource.Sdmx.SdmxObjects.Model;

namespace IstatExtension.Controllers.Builder
{
    public class JsonStructureResponseGenerator : IResponseGenerator<Newtonsoft.Json.JsonTextWriter, ISdmxObjects>
    {

        
        #region Fields

        /// <summary>
        ///     The _sdmx structure format.
        /// </summary>
        private readonly SdmxStructureFormat _sdmxStructureFormat;

        /// <summary>
        ///     The _structure manager builder.
        /// </summary>
        private readonly IWriterBuilder<IStructureWriterManager, Newtonsoft.Json.JsonTextWriter> _structureManagerBuilder;

        #endregion

        #region Constructors and Destructors

        public JsonStructureResponseGenerator(IWriterBuilder<IStructureWriterManager, Newtonsoft.Json.JsonTextWriter> structureManagerBuilder, StructureOutputFormatEnumType format)
        {
            this._structureManagerBuilder = structureManagerBuilder;
            this._sdmxStructureFormat = new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(format));
        }
        #endregion

        #region Public Methods and Operators

        public Action<Newtonsoft.Json.JsonTextWriter, Queue<Action>> GenerateResponseFunction(ISdmxObjects query)
        {
            return (writer, actions) => this.StreamTo(query, writer, actions);
        }
        #endregion

        #region Methods

        /// <summary>
        /// The stream to.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        /// <param name="actions"></param>
        private void StreamTo(ISdmxObjects query, Newtonsoft.Json.JsonTextWriter writer, Queue<Action> actions)
        {
            IStructureWriterManager structureWritingManager = this._structureManagerBuilder.Build(writer, actions);

            structureWritingManager.WriteStructures(query, this._sdmxStructureFormat, null);
        }

        #endregion

        Action<Newtonsoft.Json.JsonTextWriter, Queue<Action>> IResponseGenerator<Newtonsoft.Json.JsonTextWriter, ISdmxObjects>.GenerateResponseFunction(ISdmxObjects query)
        {
            throw new NotImplementedException();
        }
    }
}

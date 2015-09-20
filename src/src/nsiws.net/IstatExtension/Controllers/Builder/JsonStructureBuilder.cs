using Estat.Sri.Ws.Controllers.Constants;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Manager.Output;
using System;
using System.Collections.Generic;
using System.Xml;
using Estat.Sri.Ws.Controllers.Builder;
using Org.Sdmxsource.Sdmx.Structureparser.Manager;
using Org.Sdmxsource.Sdmx.Structureparser.Factory;
using Org.Sdmxsource.Sdmx.Api.Exception;

namespace IstatExtension.Controllers.Builder
{
    public class JsonStructureBuilder : IWriterBuilder<IStructureWriterManager, Newtonsoft.Json.JsonTextWriter>
    {

        #region Fields

        /// <summary>
        ///     The _endpoint.
        /// </summary>
        private readonly WebServiceEndpoint _endpoint;

        /// <summary>
        ///     The _schema.
        /// </summary>
        private readonly SdmxSchema _schema;

        #endregion

        #region Constructors and Destructors

        public JsonStructureBuilder(WebServiceEndpoint endpoint, SdmxSchema schema)
        {
            this._endpoint = endpoint;
            this._schema = schema;
        }

        #endregion


        #region Public Methods and Operators

        public IStructureWriterManager Build(Newtonsoft.Json.JsonTextWriter writer, Queue<Action> actions)
        {
            //actions.RunAll();
            Estat.Sri.Ws.Controllers.Extension.WriterExtension.RunAll(actions);
            IStructureWriterManager structureWritingManager;
            switch (this._schema.EnumType)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    structureWritingManager = this._endpoint == WebServiceEndpoint.EstatEndpoint
                                                  ? new StructureWriterManager(new SdmxStructureWriterV2Factory(writer))
                                                  : new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    structureWritingManager = new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                default:
                    throw new SdmxSemmanticException(string.Format("Unsupported format {0}", this._schema));
            }

            return structureWritingManager;
        }


        #endregion

    }
}

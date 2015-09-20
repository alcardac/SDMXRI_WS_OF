using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Org.Sdmxsource.Sdmx.Api.Model.Query;
using System.ServiceModel.Web;
using System.Net;
using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Estat.Nsi.StructureRetriever.Factory;
using Estat.Sri.Ws.Controllers.Manager;
using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21;
using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;

namespace DsplDataFormat.Engine
{
    public class DsplMetadataEngine : IDisposable
    {
        #region Fields

        public const string structure = "codelist";

        private IMutableStructureSearchManager _structureSearchManager;

        #endregion

        #region Constructors

        public DsplMetadataEngine(string agencyID, string resourceID, string version)
        {
            this.agencyID = agencyID;
            this.resourceID = resourceID;
            this.version = version;
        }

        #endregion


        #region Methods

        public string agencyID { get; set; }
        public string resourceID { get; set; }
        public string version { get; set; }


        private static IRestStructureQuery BuildRestQueryBean(string structure, string agencyId, string resourceId, string version, NameValueCollection queryParameters)
        {
            var queryString = new string[4];
            queryString[0] = structure;
            queryString[1] = agencyId;
            queryString[2] = resourceId;
            queryString[3] = version;

            IDictionary<string, string> paramsDict = new Dictionary<string, string>();

            IRestStructureQuery query;
            try
            {
                query = new RESTStructureQueryCore(queryString, paramsDict);
            }

            catch (Exception e)
            {

                throw new WebFaultException<string>(e.Message, HttpStatusCode.BadRequest);
            }

            return query;
        }


        public ISet<ICodelistObject> GetCodelistStruc()
        {
                WebOperationContext ctx = WebOperationContext.Current;
                IRestStructureQuery input = BuildRestQueryBean(structure, agencyID, resourceID, version, ctx.IncomingRequest.UriTemplateMatch.QueryParameters);

                CodelistXmlBuilder codelistXmlBuilderBean = new CodelistXmlBuilder();
                var codelistsType = new CodelistsType();
                var structures = new StructuresType();
                structures.Codelists = codelistsType;
                IMutableStructureSearchManager mutableStructureSearchManagerV21;

                IStructureSearchManagerFactory<IMutableStructureSearchManager> structureSearchManager = new MutableStructureSearchManagerFactory();
                var sdmxSchemaV21 = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne);

                mutableStructureSearchManagerV21 = structureSearchManager.GetStructureSearchManager(SettingsManager.MappingStoreConnectionSettings, sdmxSchemaV21);
                this._structureSearchManager = mutableStructureSearchManagerV21; ;
                IMutableObjects mutableObjects = this._structureSearchManager.GetMaintainables(input);
                var immutableObj = mutableObjects.ImmutableObjects;
                immutableObj.Header = SettingsManager.Header;
                ISet<ICodelistObject> codelists = immutableObj.Codelists;

                return codelists;
        }

        public void Dispose()
        {
            //TODO
        }
        #endregion
    }
}

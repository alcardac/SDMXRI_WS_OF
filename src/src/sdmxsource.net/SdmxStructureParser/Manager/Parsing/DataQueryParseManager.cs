// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryParseManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query parse manager implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using log4net;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Log;
    using Org.Sdmxsource.XmlHelper;
    using QueryMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.QueryMessageType;

    /// <summary>
    ///     The data query parse manager implementation
    /// </summary>
    public class DataQueryParseManager : BaseParsingManager, IDataQueryParseManager
    {
        #region Fields

        /// <summary>
        ///     Data query builder
        /// </summary>
        private readonly IDataQueryBuilder _dataQueryBuilder = new DataQueryBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(DataQueryParseManager));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryParseManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        public DataQueryParseManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds a <see cref="IDataQuery"/> list from a message that contains one or more data queries
        /// </summary>
        /// <param name="dataQueryLocation">
        /// The data location
        /// </param>
        /// <param name="beanRetrievalManager">
        /// optional, if given will retrieve the key family bean the query is for
        /// </param>
        /// <returns>
        /// a <see cref="IDataQuery"/> list
        /// </returns>
        public IList<IDataQuery> BuildDataQuery(
            IReadableDataLocation dataQueryLocation, ISdmxObjectRetrievalManager beanRetrievalManager)
        {
            this._log.Debug("DataParseManagerImpl.buildDataQuery");
            using (ISdmxXmlStream xmlStream = new SdmxXmlStream(dataQueryLocation))
            {
                LoggingUtil.Debug(this._log, "Schema Version Determined to be : " + xmlStream.SdmxVersion);
                return this.BuildDataQuery(xmlStream, beanRetrievalManager);
            }
        }

        /// <summary>
        /// Builds a <see cref="IDataQuery"/> list from a message that contains one or more data queries
        /// </summary>
        /// <param name="dataQueryLocation">
        /// The data location
        /// </param>
        /// <param name="beanRetrievalManager">
        /// optional, if given will retrieve the key family bean the query is for
        /// </param>
        /// <returns>
        /// a <see cref="IDataQuery"/> list
        /// </returns>
        public IList<IDataQuery> BuildDataQuery(
            ISdmxXmlStream dataQueryLocation, ISdmxObjectRetrievalManager beanRetrievalManager)
        {
            if (!dataQueryLocation.HasReader)
            {
                throw new ArgumentException("ISdmxXmlStream doesnt have a Reader", "dataQueryLocation");
            }

            this._log.Debug("DataParseManagerImpl.buildDataQuery");

            switch (dataQueryLocation.SdmxVersion)
            {
                case SdmxSchemaEnumType.VersionOne:
                    QueryMessage queryV1 = MessageFactory.Load<QueryMessage, QueryMessageType>(dataQueryLocation.Reader);
                    return this._dataQueryBuilder.BuildDataQuery(queryV1.Query, beanRetrievalManager);
                case SdmxSchemaEnumType.VersionTwo:
                    Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessage queryV2 =
                        Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessage.Load(dataQueryLocation.Reader);
                    return this._dataQueryBuilder.BuildDataQuery(queryV2.Query, beanRetrievalManager);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    throw new ArgumentException("Build Data Query concerns sdmx messages of schema version 1.0 and 2.0 ");
                default:
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported, "buildDataQuery in version " + dataQueryLocation.SdmxVersion);
            }
        }


        /// <summary>
        /// Parse the specified <paramref name="query"/>.
        /// </summary>
        /// <param name="query">
        /// The REST data query.
        /// </param>
        /// <param name="beanRetrievalManager">
        /// The <c>SDMX</c> object retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IDataQuery"/> from <paramref name="query"/>.
        /// </returns>
        public virtual IDataQuery ParseRestQuery(string query, ISdmxObjectRetrievalManager beanRetrievalManager)
        {
            // NOTE fixed MT bug Data -> data
            if (!query.ToLower().StartsWith("data/"))
            {
                throw new SdmxSemmanticException("Expecting REST Query for Data to start with 'Data/'");
            }

            string[] split = Regex.Split(query, "\\?");

            //Everything one split[1] this point are query parameters
            IDictionary<string, string> queryParameters = new Dictionary<string, string>(StringComparer.Ordinal);
            if (split.Length > 1)
            {
                string[] queryParametersSplit = Regex.Split(split[1], "&");
                for (int i = 0; i < queryParametersSplit.Length; i++)
                {
                    string[] queryParam = Regex.Split(queryParametersSplit[i], "=");
                    if (queryParam.Length != 2)
                    {
                        throw new SdmxSemmanticException("Missing equals '=' in query parameters '" + split[i] + "'");
                    }

                    queryParameters.Add(queryParam[0], queryParam[1]);
                }
            }

            string queryPrefix = split[0];
            var dataQuery = new RESTDataQueryCore(queryPrefix.Split('/'), queryParameters);
            return new DataQueryImpl(dataQuery, beanRetrievalManager);
        }

        public IList<IComplexDataQuery> BuildComplexDataQuery(IReadableDataLocation dataQueryLocation, ISdmxObjectRetrievalManager beanRetrievalManager)
        {
            _log.Debug("DataParseManagerImpl.buildComplexDataQuery");

            SdmxSchemaEnumType schemaVersion = SdmxMessageUtil.GetSchemaVersion(dataQueryLocation);
            LoggingUtil.Debug(_log, "Schema Version Determined to be : " + schemaVersion);
            XMLParser.ValidateXml(dataQueryLocation, schemaVersion);

            using (Stream inputStream = dataQueryLocation.InputStream)
            using (var textReader = new StreamReader(inputStream))
            {
                switch (schemaVersion)
                {
                    case SdmxSchemaEnumType.VersionOne:
                        throw new ArgumentException("Build Complex Data Query concerns sdmx messages of schema version 2.1 ");
                    case SdmxSchemaEnumType.VersionTwo:
                        throw new ArgumentException("Build Complex Data Query concerns sdmx messages of schema version 2.1 ");
                    case SdmxSchemaEnumType.VersionTwoPointOne:
                        IList<QueryMessageEnumType> queryMessageTypes = SdmxMessageUtil.GetQueryMessageTypes(dataQueryLocation);
                        QueryMessageEnumType queryMessageType = queryMessageTypes[0];
                        if (queryMessageType.Equals(QueryMessageEnumType.GenericDataQuery))
                        {
                            GenericDataQuery queryV21 = GenericDataQuery.Load(textReader);
                            return this._dataQueryBuilder.BuildComplexDataQuery(queryV21.Content.BaseDataQueryType, beanRetrievalManager);
                        }
                        else if (queryMessageType.Equals(QueryMessageEnumType.GenericTimeseriesDataQuery))
                        {
                            GenericTimeSeriesDataQuery queryV21 = GenericTimeSeriesDataQuery.Load(textReader);
                            return this._dataQueryBuilder.BuildComplexDataQuery(queryV21.Content.BaseDataQueryType, beanRetrievalManager);
                        }
                        else if (queryMessageType.Equals(QueryMessageEnumType.StructureSpecificDataQuery))
                        {
                            StructureSpecificDataQuery queryV21 = StructureSpecificDataQuery.Load(textReader);
                            return this._dataQueryBuilder.BuildComplexDataQuery(queryV21.Content.BaseDataQueryType, beanRetrievalManager);
                        }
                        else //if (queryMessageType.Equals(QueryMessageEnumType.StructureSpecificTimeSeriesDataQuery))
                        {
                            StructureSpecificTimeSeriesDataQuery queryV21 = StructureSpecificTimeSeriesDataQuery.Load(textReader);
                            return this._dataQueryBuilder.BuildComplexDataQuery(queryV21.Content.BaseDataQueryType, beanRetrievalManager);
                        }
                    default:
                        throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "buildComplexDataQuery in version " + schemaVersion);
                }
            }
        }

        #endregion
    }
}
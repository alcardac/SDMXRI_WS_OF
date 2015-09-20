// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestQueryBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test query builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SdmxQueryBuilderTests
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Manager.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Manager;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Model;
    using System.IO;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary> Test unit class for <see cref="TestQueryBuilder"/> </summary>
    [TestFixture]
    public class TestQueryBuilder
    {

        /// <summary>
        /// Test unit for <see cref="TestDownload"/>
        /// </summary>
        /// <param name="dsd">
        /// The dsd.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", "tests/v20/dataflows.xml", "tests/v20/query.xml")]
        public void TestDownload(string dsd, string dataflow, string query)
        {

            IStructureParsingManager parsingManager = new StructureParsingManager();
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (IReadableDataLocation location = new FileReadableDataLocation(dsd))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            using (IReadableDataLocation location = new FileReadableDataLocation(dataflow))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            ISdmxObjectRetrievalManager retrievalManager = new InMemoryRetrievalManager(objects);
            IList<IDataQuery> buildDataQuery;
            IDataQueryParseManager parseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            using (IReadableDataLocation readable = new FileReadableDataLocation(query))
            {
                // call BuildDataQuery to process the query.xml and get a list of IDataQuery
                buildDataQuery = parseManager.BuildDataQuery(readable, retrievalManager);
            }
            IList<IDataQuery> buildDataQuery1;
            foreach (var dataQuery in buildDataQuery)
            {
                IDataQueryBuilderManager dataQueryBuilderManager = new DataQueryBuilderManager(new DataQueryFactory());
                var xdoc = dataQueryBuilderManager.BuildDataQuery(dataQuery, new QueryMessageV2Format());
                Assert.IsNotNull(xdoc);
                MemoryStream xmlStream = new MemoryStream();
                xdoc.Save(xmlStream);
                using (IReadableDataLocation readable = new MemoryReadableLocation(xmlStream.ToArray()))
                {
                    // call BuildDataQuery to process the xmlStream and get a list of IDataQuery
                    buildDataQuery1 = parseManager.BuildDataQuery(readable, retrievalManager);
                }
                Assert.AreEqual(dataQuery.ToString(),buildDataQuery1[0].ToString());
                xmlStream.Flush();
            }

        }

        /// <summary>
        /// Test unit for <see cref="TestRestQueryFormat"/>
        /// </summary>
        /// <param name="dsd">
        /// The dsd.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", "tests/v20/dataflows.xml", "tests/v20/query.xml")]
        public void TestRestQueryFormat(string dsd, string dataflow, string query)
        {

            IStructureParsingManager parsingManager = new StructureParsingManager();
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (IReadableDataLocation location = new FileReadableDataLocation(dsd))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            using (IReadableDataLocation location = new FileReadableDataLocation(dataflow))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            ISdmxObjectRetrievalManager retrievalManager = new InMemoryRetrievalManager(objects);
            IList<IDataQuery> buildDataQuery;
            IDataQueryParseManager parseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            using (IReadableDataLocation readable = new FileReadableDataLocation(query))
            {
                // call BuildDataQuery to process the query.xml and get a list of IDataQuery
                buildDataQuery = parseManager.BuildDataQuery(readable, retrievalManager);
            }
            IDataQueryFormat<string> structureQueryFormat = new RestQueryFormat();
            IDataQueryFactory dataQueryFactory = new DataQueryFactory();
            foreach (var dataQuery in buildDataQuery)
            {
                IDataQueryBuilderManager dataQueryBuilderManager = new DataQueryBuilderManager(dataQueryFactory);
                string request = dataQueryBuilderManager.BuildDataQuery(dataQuery, structureQueryFormat);
                Assert.IsNotNull(request);
            }

        }


        /// <summary>
        /// Test unit for <see cref="TestRestStructureQuery"/>
        /// </summary>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result.
        /// </param>
        [TestCase(null, null, null, "dataflow/all/all/latest/?references=none&detail=full")]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=none&detail=full")]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=none&detail=full")]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=none&detail=full")]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=none&detail=full")]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=none&detail=full")]
        [TestCase("AGENCY", "TST", "2.0", "dataflow/AGENCY/TST/2.0/?references=none&detail=full")]
        [TestCase("AGNCY", "TEST", "2.0", "dataflow/AGNCY/TEST/2.0/?references=none&detail=full")]
        [TestCase("AGENCY", "TEST", "2.0", "dataflow/AGENCY/TEST/2.0/?references=none&detail=full")]
        public void TestRestStructureQuery(string agency, string id, string version, string expectedResult)
        {

            var dataflowRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
            {
                MaintainableId = id,
                AgencyId = agency,
                Version =version,
            };
            string request = GetStructureQueryFormat(dataflowRef, version, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None);
            Assert.IsNotNull(request);
            Assert.AreEqual(request, expectedResult);

        }

        /// <summary>
        /// Test unit for <see cref="TestRestStructureQueryExpectedResultTest2"/>
        /// </summary>
        /// <param name="agency">
        /// The agency.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result.
        /// </param>
        /// <param name="structureQueryDetail">
        /// The structure query detail.
        /// </param>
        /// <param name="structureReferenceDetail">
        /// The structure reference detail.
        /// </param>
        [TestCase(null, null, null, "dataflow/all/all/latest/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=all&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All)]

        [TestCase(null, null, null, "dataflow/all/all/latest/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=children&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]

        [TestCase(null, null, null, "dataflow/all/all/latest/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=none&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Null)]

        [TestCase(null, null, null, "dataflow/all/all/latest/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=parents&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]

        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=parentsandsiblings&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=codelist&detail=allstubs", StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific)]
       
        [TestCase(null, null, null, "dataflow/all/all/latest/?references=all&detail=full", StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=children&detail=full", StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=none&detail=full",  StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=parents&detail=full",  StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=parentsandsiblings&detail=full", StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=codelist&detail=full",  StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific)]

        [TestCase(null, null, null, "dataflow/all/all/latest/?references=all&detail=full", StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.All)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=children&detail=full", StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=none&detail=full",  StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=parents&detail=full", StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.Parents)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=parentsandsiblings&detail=full", StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=codelist&detail=full", StructureQueryDetailEnumType.Null, StructureReferenceDetailEnumType.Specific)]

        [TestCase(null, null, null, "dataflow/all/all/latest/?references=all&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All)]
        [TestCase(null, null, "1.0", "dataflow/all/all/1.0/?references=children&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(null, "TEST", null, "dataflow/all/TEST/latest/?references=none&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Null)]
        [TestCase(null, "TEST", "1.0", "dataflow/all/TEST/1.0/?references=parents&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase("AGENCY", "TEST", null, "dataflow/AGENCY/TEST/latest/?references=parentsandsiblings&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase("AGENCY", "TEST", "1.0", "dataflow/AGENCY/TEST/1.0/?references=codelist&detail=referencedstubs", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
       

        public void TestRestStructureQueryExpectedResultTest2(string agency, string id, string version, string expectedResult, StructureQueryDetailEnumType structureQueryDetail, StructureReferenceDetailEnumType structureReferenceDetail)
        {

            var dataflowRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
            {
                MaintainableId = id,
                AgencyId = agency,
                Version = version,
            };

            string request = GetStructureQueryFormat(dataflowRef, version, structureQueryDetail, structureReferenceDetail);
            Assert.IsNotNull(request);
            Assert.AreEqual(request, expectedResult);

        }

        [TestCase("AGENCY", "TEST", "1.0", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
        public void TestRestStructureQueryStructureReferenceSpecific(string agency, string id, string version,  StructureQueryDetailEnumType structureQueryDetail, StructureReferenceDetailEnumType structureReferenceDetail)
        {

            var dataflowRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
            {
                MaintainableId = id,
                AgencyId = agency,
                Version = version,
            };

            bool returnLatest = version == null;
            IRestStructureQuery structureQuery;
            Assert.Throws<SdmxSemmanticException>(() =>  structureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(structureQueryDetail), StructureReferenceDetail.GetFromEnum(structureReferenceDetail), null, dataflowRef, returnLatest));

        }

        [TestCase(null, null, "1.0", StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
        public void TestRestStructureQueryIsMaintainable(string agency, string id, string version, StructureQueryDetailEnumType structureQueryDetail, StructureReferenceDetailEnumType structureReferenceDetail)
        {

            var dataflowRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
            {
                MaintainableId = id,
                AgencyId = agency,
                Version = version,
            };

            bool returnLatest = version == null;
            IRestStructureQuery structureQuery;
            
            Assert.Throws<SdmxSemmanticException>(() => structureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(structureQueryDetail), StructureReferenceDetail.GetFromEnum(structureReferenceDetail), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code), dataflowRef, returnLatest));
            Assert.Throws<SdmxSemmanticException>(() => structureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(structureQueryDetail), StructureReferenceDetail.GetFromEnum(structureReferenceDetail), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category), dataflowRef, returnLatest));
            Assert.Throws<SdmxSemmanticException>(() => structureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(structureQueryDetail), StructureReferenceDetail.GetFromEnum(structureReferenceDetail), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension), dataflowRef, returnLatest));

        }

        [TestCase("datastructure/all/all/all")]
        public void TestRestStructureQueryRoundTrip(string restQuery)
        {
            var restStructureQueryCore = new RESTStructureQueryCore(restQuery);
            Assert.NotNull(restStructureQueryCore.StructureReference);
            var manager = new StructureQueryBuilderManager(new RestStructureQueryFactory());
            var buildStructureQuery = manager.BuildStructureQuery(restStructureQueryCore, new RestQueryFormat());
            Assert.NotNull(buildStructureQuery);
            Assert.IsTrue(buildStructureQuery.StartsWith(restQuery));
        }

        private string  GetStructureQueryFormat(IStructureReference dataflowRef, string version,StructureQueryDetailEnumType structureQueryDetail, StructureReferenceDetailEnumType structureReference )
        {
            IStructureQueryFormat<string> structureQueryFormat = new RestQueryFormat();
            IStructureQueryFactory factory = new RestStructureQueryFactory();
            IStructureQueryBuilderManager structureQueryBuilderManager = new StructureQueryBuilderManager(factory);
            bool returnLatest = version == null;
            SdmxStructureType specificStructureReference = structureReference == StructureReferenceDetailEnumType.Specific? SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList) : null;
            IRestStructureQuery structureQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(structureQueryDetail), StructureReferenceDetail.GetFromEnum(structureReference), specificStructureReference, dataflowRef, returnLatest);
            string request = structureQueryBuilderManager.BuildStructureQuery(structureQuery, structureQueryFormat);
            return request;
        }
    }
}

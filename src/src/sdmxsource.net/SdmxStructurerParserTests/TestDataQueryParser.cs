// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataQueryParser.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test data query parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureParserTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Model;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Io;


    /// <summary>
    /// The test data query parser.
    /// </summary>
    [TestFixture]
    public class TestDataQueryParser
    {
        #region Constants

        /// <summary>
        /// The file data flow.
        /// </summary>
        private const string FileDataFlow = "tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml";

        /// <summary>
        /// The filekey bean.
        /// </summary>
        private const string FilekeyBean = "tests/v20/ESTAT+STS+2.0.xml";

        #endregion

        #region Fields

        /// <summary>
        /// The bean retrival manager.
        /// </summary>
        private Mock<ISdmxObjectRetrievalManager> beanRetrivalManager;

        /// <summary>
        /// The data query parse manager.
        /// </summary>
        private IDataQueryParseManager dataQueryParseManager;

        /// <summary>
        /// The parsing manager.
        /// </summary>
        private IStructureParsingManager parsingManager;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The init.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            IStructureWorkspace structureWorkspace;
            IDataflowObject dataflowObject;
            IDataStructureObject dataStructureObject;

            dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.Null);
            parsingManager = new StructureParsingManager();
            beanRetrivalManager = new Mock<ISdmxObjectRetrievalManager>();

            using (var fileDataFlowReadableDataLocation = new FileReadableDataLocation(FileDataFlow))
            {
                structureWorkspace = parsingManager.ParseStructures(fileDataFlowReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                dataflowObject = structureBeans.Dataflows.First();
                Assert.IsNotNull(dataflowObject);
            }

            using (var fileKeybeanReadableDataLocation = new FileReadableDataLocation(FilekeyBean))
            {
                structureWorkspace = parsingManager.ParseStructures(fileKeybeanReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                dataStructureObject = structureBeans.DataStructures.First();
                Assert.IsNotNull(dataStructureObject);
            }

            beanRetrivalManager.Setup(m => m.GetMaintainableObject<IDataflowObject>(It.IsAny<IMaintainableRefObject>())).Returns(dataflowObject);
            beanRetrivalManager.Setup(m => m.GetMaintainableObject<IDataStructureObject>(It.IsAny<IMaintainableRefObject>())).Returns(dataStructureObject);
        }

        /// <summary>
        /// The test data query parser manager empty.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result
        /// </param>
        /// <param name="dataflowFile">
        /// The dataflow file
        /// </param>
        /// <param name="dsdFile">
        /// The dsd file
        /// </param>
        [TestCase("tests/v20/DataQuery/empty-query.xml", -1, "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/DataQuery/defaultLimit.xml", 5, "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/DataQuery/defaultLimit2.xml", 1002, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", -1, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        public void TestDataQueryParserManageDefaultLimit(string file, int expectedResult, string dataflowFile, string dsdFile)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);

            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IList<IDataQuery> dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
                Assert.AreEqual(1, dataQuery.Count);
                IDataQuery query = dataQuery[0];
                Assert.NotNull(query);
                Assert.NotNull(query.Dataflow);
                Assert.NotNull(query.DataStructure);
                Assert.AreEqual(expectedResult, query.FirstNObservations);
            }
        }

        /// <summary>
        /// The test data query parser manager empty.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="dataflowFile">
        /// The dataflow file.
        /// </param>
        /// <param name="dsdFile">
        /// The dsd file.
        /// </param>
        [TestCase("tests/v20/DataQuery/empty-query.xml", "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        public void TestDataQueryParserManagerEmpty(string file, string dataflowFile, string dsdFile)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);

            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IList<IDataQuery> dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
                Assert.AreEqual(1, dataQuery.Count);
                IDataQuery query = dataQuery[0];
                Assert.NotNull(query);
                Assert.NotNull(query.Dataflow);
                Assert.NotNull(query.DataStructure);
                Assert.IsFalse(query.HasSelections());
                Assert.IsEmpty(query.SelectionGroups);
            }
        }

        /// <summary>
        /// The test data query parser manager v 20.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="dataflowFile">The dataflow file.</param>
        /// <param name="dsdFile">The DSD file.</param>
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+CENSUSHUB+2.0_fixed.xml")]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/two-components-under-or.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        public void TestDataQueryParserManagerV20(string file, string dataflowFile, string dsdFile)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);

            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IList<IDataQuery> dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
            }
        }

      
        [TestCase("tests/v21/query/SOAPUI_HC01-300k-Query-failed.xml","tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/TC_HC01-Query_failed.xml", "tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/TC_HC01-Query_working.xml", "tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/TestQuery_equal.xml", "tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/TestQuery_notEqual.xml", "tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/TestQuery_notequal_equal.xml", "tests/v21/Dataflow.xml", "tests/v21/ESTAT+HC01+1.0+Description.xml")]
        [TestCase("tests/v21/query/get-structure-specific-nested-and-in-or.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        public void TestDataQueryParserManagerV21(string file,string dataflowFile, string dsdFile)
        {
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (IReadableDataLocation readableDataLocation = new FileReadableDataLocation(dataflowFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(readableDataLocation);
                objects.Merge(structureWorkspace.GetStructureObjects(false));
            }
            using (IReadableDataLocation readableDataLocation = new FileReadableDataLocation(dsdFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(readableDataLocation);
                objects.Merge(structureWorkspace.GetStructureObjects(false));
            }

            var retrievalManager = new InMemoryRetrievalManager(objects);
            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                var dataQuery = this.dataQueryParseManager.BuildComplexDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
            }
        }

        [TestCase("tests/v21/query/get-structure-specific-nested-and-in-or.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml",2)]
        public void TestNestedAndInOrV21(string file, string dataflowFile, string dsdFile, int expectedDataSelectionGroup)
        {
            // To be analysed  in SDMXRI-124
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (IReadableDataLocation readableDataLocation = new FileReadableDataLocation(dataflowFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(readableDataLocation);
                objects.Merge(structureWorkspace.GetStructureObjects(false));
            }
            using (IReadableDataLocation readableDataLocation = new FileReadableDataLocation(dsdFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(readableDataLocation);
                objects.Merge(structureWorkspace.GetStructureObjects(false));
            }

            var retrievalManager = new InMemoryRetrievalManager(objects);
            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                var dataQuery = this.dataQueryParseManager.BuildComplexDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
                Assert.AreEqual(expectedDataSelectionGroup, dataQuery.First().SelectionGroups.Count);
            }
        }

        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+CENSUSHUB+2.0_fixed.xml")]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        public void TestDataQueryWriterV20(string file, string dataflowFile, string dsdFile)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);
            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IList<IDataQuery> dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
                IDataQueryFactory factory = new DataQueryFactory();
                IDataQueryBuilder<XDocument> builder = factory.GetDataQueryBuilder(new QueryMessageV2Format());

                foreach (var query in dataQuery)
                {
                  XDocument xQuery = builder.BuildDataQuery(query);
                  Assert.IsNotNull(xQuery);
                }
                

            }
        }


        /// <summary>
        /// The test data query parser manager v 20 deep.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result.
        /// </param>
        /// <param name="startTime">
        /// The start time.
        /// </param>
        /// <param name="endTime">
        /// The end time.
        /// </param>
        /// <param name="dataflowFile">
        /// The dataflow file.
        /// </param>
        /// <param name="dsdFile">
        /// The dsd file.
        /// </param>
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020 OR NS0030))", null, null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", "(REF_AREA = (IT) AND OBS_CONF = (F) AND STS_ACTIVITY = (NS0020 OR NS0030))", null, null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (AAANS0020 OR BBBNS0030))", null, null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml",
            "(REF_AREA = (IT) AND FREQ = (Q) AND OBS_CONF = (F) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q1 AND  Date <= 2006-Q2)", "2005-Q1", "2006-Q2", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q1 AND  Date <= 2006-Q2)",
            "2005-Q1", "2006-Q2", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q2)", "2005-Q2", null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020) AND  Date >= 2005-01 AND  Date <= 2005-06)", "2005-01", "2005-06", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020) AND  Date >= 2005-05)", "2005-05", null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020) AND  Date >= 2005-01 AND Date <= 2005-01)", "2005-01", "2005-01", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q1 AND  Date <= 2006-Q2)"
            , "2005-Q1", "2006-Q2", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q2)", "2005-Q2", null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q2 AND Date <= 2005-Q2)",
            "2005-Q2", "2005-Q2", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020) AND  Date >= 2005-01 AND  Date <= 2005-06)", "2005-01", "2005-06", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", "(REF_AREA = (IT) AND STS_ACTIVITY = (NS0020) AND  Date >= 2005-05)", "2005-05", null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml",
            "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-Q2 AND  Date <= 2005-Q4)", "2005-Q2", "2005-Q4", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml",
            "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005-02 AND  Date <= 2005-12)", "2005-02", "2005-12", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", "(REF_AREA = (IT) AND FREQ = (Q) AND STS_ACTIVITY = (NS0020 OR NS0030) AND  Date >= 2005 AND  Date <= 2006)",
            "2005", "2006", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", "( Date >= 1920-01 AND  Date <= 1940-01)", "1920-01", "1940-01", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", "(FREQ = (A) AND SEX = (F OR M) AND COUNTRY = (ES OR GR OR HU OR DE) AND Date >= 1920-01 AND  Date <= 1940-01)",
            "1920-01", "1940-01", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", "(FREQ = (A) AND SEX = (F OR M) AND COUNTRY = (KAKOSALESI) AND  Date >= 1920-01 AND Date <= 1940-01)", "1920-01", "1940-01", "tests/v20/dataflows.xml", "tests/v20/ESTAT+DEMOGRAPHY+3.0.xml")]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml",
            "(SEX = (001 OR 002) AND AGE = (TOT OR 008 OR 010 OR 012) AND CAS = (TOT OR 003 OR 004 OR NST) AND GEO = (CZ OR CZ01 OR CZ02 OR CZ03 OR CZ032))", null, null, "tests/v20/dataflows.xml", "tests/v20/ESTAT+CENSUSHUB+2.0_fixed.xml")]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", "(REF_AREA = (ES))", null, null, "tests/v20/DUMMY_CPI_DATAFLOW1.xml", "tests/v20/ESTAT_CPI_v1.0.xml")]
        public void TestDataQueryParserManagerV20Deep(string file, string expectedResult, string startTime, string endTime, string dataflowFile, string dsdFile)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);

            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IList<IDataQuery> dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
                Assert.AreEqual(1, dataQuery.Count);
                IDataQuery query = dataQuery[0];
                Assert.NotNull(query);
                Assert.NotNull(query.Dataflow);
                Assert.NotNull(query.DataStructure);
                Assert.IsTrue(query.HasSelections());
                Assert.IsNotEmpty(query.SelectionGroups);
                IDataQuerySelectionGroup dataQuerySelectionGroup = query.SelectionGroups[0];
                if (startTime == null)
                {
                    Assert.IsNull(dataQuerySelectionGroup.DateFrom);
                    Assert.IsNull(dataQuerySelectionGroup.DateTo);
                }
                else
                {
                    Assert.IsNotNull(dataQuerySelectionGroup.DateFrom);
                    Assert.AreEqual(startTime, dataQuerySelectionGroup.DateFrom.DateInSdmxFormat);
                    if (endTime == null)
                    {
                        Assert.IsNull(dataQuerySelectionGroup.DateTo);
                    }
                    else
                    {
                        Assert.IsNotNull(dataQuerySelectionGroup.DateTo);
                        Assert.AreEqual(endTime, dataQuerySelectionGroup.DateTo.DateInSdmxFormat);
                    }
                }

                var sb = new StringBuilder();

                string concat = string.Empty;
                foreach (IDataQuerySelectionGroup selectionGroup in query.SelectionGroups)
                {
                    sb.Append(concat).Append("(").Append(selectionGroup).Append(")");
                    concat = "OR";
                }

                string s = sb.Replace("  ", " ").ToString();
                Assert.AreEqual(expectedResult.Replace("  ", " "), s);
            }
        }

        #endregion

        /// <summary>
        /// Gets the SDMX object retrieval manager.
        /// </summary>
        /// <param name="dataflowFile">The dataflow file.</param>
        /// <param name="dsdFile">The DSD file.</param>
        /// <returns>The SDMX Object retrieval manager</returns>
        private ISdmxObjectRetrievalManager GetSdmxObjectRetrievalManager(string dataflowFile, string dsdFile)
        {
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (var fileDataFlowReadableDataLocation = new FileReadableDataLocation(dataflowFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(fileDataFlowReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                objects.Merge(structureBeans);
                Assert.IsNotEmpty(structureBeans.Dataflows);
            }

            using (var fileKeybeanReadableDataLocation = new FileReadableDataLocation(dsdFile))
            {
                var structureWorkspace = this.parsingManager.ParseStructures(fileKeybeanReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                objects.Merge(structureBeans);
                Assert.IsNotEmpty(structureBeans.DataStructures);
            }
            ISdmxObjectRetrievalManager retrievalManager = new InMemoryRetrievalManager(objects);
            return retrievalManager;
        }

    }
}
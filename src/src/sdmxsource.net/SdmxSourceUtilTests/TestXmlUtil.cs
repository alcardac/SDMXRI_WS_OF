namespace SdmxSourceUtilTests
{
    using NUnit.Framework;

    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Xml;

    /// <summary> Test unit class for <see cref="XmlUtil"/> </summary>
    [TestFixture]
    public class TestXmlUtil
    {
        /// <summary>
        /// Test method for <see cref="XmlUtil.IsXML"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", true)]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", true)]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", true)]
        [TestCase("tests/v20/CR1_12_data_report.xml", true)]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", true)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", true)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", true)]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", true)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", true)]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", true)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", true)]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", true)]
        [TestCase("tests/v20/query.xml", true)]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", true)]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", true)]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", true)]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", true)]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", true)]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", true)]
        [TestCase("tests/v20/QueryStructureRequest.xml", true)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", true)]
        [TestCase("tests/v20/QueryStructureResponse.xml", true)]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", true)]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", true)]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", true)]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", true)]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", true)]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", true)]
        [TestCase("tests/v20/SubmitStructureRequest.xml", true)]
        [TestCase("tests/v20/SubmitStructureResponse.xml", true)]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", true)]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", true)]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", true)]
        [TestCase("tests/TestFile.csv", false)]
        public void Test(string file, bool expectedResult)
        {
            using (var reader = new FileReadableDataLocation(file))
            {
                Assert.AreEqual(expectedResult, XmlUtil.IsXML(reader));
            }
        }
    }
}
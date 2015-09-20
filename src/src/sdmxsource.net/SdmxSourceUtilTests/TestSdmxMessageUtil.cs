// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxMessageUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Unit tests for <see cref="SdmxMessageUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     Unit tests for <see cref="SdmxMessageUtil" />
    /// </summary>
    [TestFixture]
    public class TestSdmxMessageUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Unit tests for <see cref="SdmxMessageUtil.GetDataSetAction"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/Data/Compact-VersionTwo.xml", DatasetActionEnumType.Information)]
        [TestCase("tests/Data/Generic-VersionTwo.xml", DatasetActionEnumType.Information)]
        [TestCase("tests/Data/Generic-VersionTwoPointOne.xml", DatasetActionEnumType.Information)]
        [TestCase("tests/Data/Compact-VersionTwoPointOne.xml", DatasetActionEnumType.Information)]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/CR1_12_data_report.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/query.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryStructureRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QueryStructureResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitStructureRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitStructureResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v21/demography.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v21/ecb_exr_ng_full.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v21/response_demo_stub.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v21/repsonse_cl_all.xml", DatasetActionEnumType.Append)]
        [TestCase("tests/v21/query_cl_all.xml", DatasetActionEnumType.Append, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_cl_regions.xml", DatasetActionEnumType.Append, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_demo_stub.xml", DatasetActionEnumType.Append, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_esms_children.xml", DatasetActionEnumType.Append, Ignore = true, IgnoreReason = "Not supported yet")]
        public void TestGetDataSetAction(string file, DatasetActionEnumType expectedResult)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                Assert.AreEqual(expectedResult, SdmxMessageUtil.GetDataSetAction(readable));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="SdmxMessageUtil.GetMessageType"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/Data/Compact-VersionTwo.xml", MessageEnumType.CompactData)]
        [TestCase("tests/Data/Generic-VersionTwo.xml", MessageEnumType.GenericData)]
        [TestCase("tests/Data/Generic-VersionTwoPointOne.xml", MessageEnumType.GenericData)]
        [TestCase("tests/Data/Compact-VersionTwoPointOne.xml", MessageEnumType.CompactData)]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/CR1_12_data_report.xml", MessageEnumType.GenericMetadata)]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", MessageEnumType.Structure)]
        [TestCase("tests/v20/query.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryStructureRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/QueryStructureResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitStructureRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitStructureResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", MessageEnumType.RegistryInterface)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", MessageEnumType.Query)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", MessageEnumType.Query)]
        [TestCase("tests/v21/demography.xml", MessageEnumType.Structure)]
        [TestCase("tests/v21/ecb_exr_ng_full.xml", MessageEnumType.Structure)]
        [TestCase("tests/v21/response_demo_stub.xml", MessageEnumType.Structure)]
        [TestCase("tests/v21/repsonse_cl_all.xml", MessageEnumType.Structure)]
        [TestCase("tests/v21/query_cl_all.xml", MessageEnumType.Query, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_cl_regions.xml", MessageEnumType.Query, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_demo_stub.xml", MessageEnumType.Query, Ignore = true, IgnoreReason = "Not supported yet")]
        [TestCase("tests/v21/query_esms_children.xml", MessageEnumType.Query, Ignore = true, IgnoreReason = "Not supported yet")]
        public void TestGetMessageType(string file, MessageEnumType expectedResult)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                Assert.AreEqual(expectedResult, SdmxMessageUtil.GetMessageType(readable));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="SdmxMessageUtil.GetQueryMessageTypes"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/Data/Compact-VersionTwo.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Generic-VersionTwo.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Generic-VersionTwoPointOne.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Compact-VersionTwoPointOne.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CR1_12_data_report.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/query.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryStructureRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryStructureResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitStructureRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitStructureResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", QueryMessageEnumType.DataWhere)]
        [TestCase("tests/v21/demography.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/ecb_exr_ng_full.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/response_demo_stub.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/repsonse_cl_all.xml", QueryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/query_cl_all.xml", QueryMessageEnumType.CodelistWhere)]
        [TestCase("tests/v21/query_cl_regions.xml", QueryMessageEnumType.CodelistWhere)]
        [TestCase("tests/v21/query_demo_stub.xml", QueryMessageEnumType.DsdWhere)]
        [TestCase("tests/v21/query_esms_children.xml", QueryMessageEnumType.MdsWhere)]
        public void TestGetQueryMessageTypes(string file, QueryMessageEnumType expectedResult)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                IList<QueryMessageEnumType> queryMessageEnumTypes = SdmxMessageUtil.GetQueryMessageTypes(readable);
                Assert.IsNotNull(queryMessageEnumTypes);
                if (expectedResult != QueryMessageEnumType.Null)
                {
                    Assert.IsNotEmpty(queryMessageEnumTypes);
                    var messageEnumTypes = from x in queryMessageEnumTypes where (x == expectedResult) select x;
                    CollectionAssert.AreEquivalent(queryMessageEnumTypes, messageEnumTypes);
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="SdmxMessageUtil.GetRegistryMessageType"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/Data/Compact-VersionTwo.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Generic-VersionTwo.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Generic-VersionTwoPointOne.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/Data/Compact-VersionTwoPointOne.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/CR1_12_data_report.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/query.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", RegistryMessageEnumType.QueryProvisionRequest)]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", RegistryMessageEnumType.QueryProvisionResponse)]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", RegistryMessageEnumType.QueryRegistrationRequest)]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", RegistryMessageEnumType.QueryRegistrationResponse)]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", RegistryMessageEnumType.QueryStructureResponse)]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", RegistryMessageEnumType.QueryStructureResponse)]
        [TestCase("tests/v20/QueryStructureRequest.xml", RegistryMessageEnumType.QueryStructureRequest)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", RegistryMessageEnumType.QueryStructureRequest)]
        [TestCase("tests/v20/QueryStructureResponse.xml", RegistryMessageEnumType.QueryStructureResponse)]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", RegistryMessageEnumType.SubmitProvisionRequest)]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", RegistryMessageEnumType.SubmitProvisionResponse)]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", RegistryMessageEnumType.SubmitRegistrationRequest)]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", RegistryMessageEnumType.SubmitRegistrationResponse)]
        [TestCase("tests/v20/SubmitStructureRequest.xml", RegistryMessageEnumType.SubmitStructureRequest)]
        [TestCase("tests/v20/SubmitStructureResponse.xml", RegistryMessageEnumType.SubmitStructureResponse)]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", RegistryMessageEnumType.SubmitSubscriptionRequest)]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", RegistryMessageEnumType.SubmitSubscriptionResponse)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/demography.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/ecb_exr_ng_full.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/response_demo_stub.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/repsonse_cl_all.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/query_cl_all.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/query_cl_regions.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/query_demo_stub.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        [TestCase("tests/v21/query_esms_children.xml", RegistryMessageEnumType.Null, ExpectedException = typeof(ArgumentException))]
        public void TestGetRegistryMessageType(string file, RegistryMessageEnumType expectedResult)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                RegistryMessageEnumType registryMessageEnumType = SdmxMessageUtil.GetRegistryMessageType(readable);
                Assert.AreEqual(expectedResult, registryMessageEnumType);
            }
        }

        /// <summary>
        /// Unit tests for <see cref="SdmxMessageUtil.GetSchemaVersion"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/Data/Compact-VersionTwo.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/Data/Generic-VersionTwo.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/Data/Generic-VersionTwoPointOne.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/Data/Compact-VersionTwoPointOne.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/CL_SEX_v1.1.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/CR1_12_data_report.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/query.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryProvisioningRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryProvisioningResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryRegistrationRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryRegistrationResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/queryResponse-estat-sts.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryStructureRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QueryStructureResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitStructureRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitStructureResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml", SdmxSchemaEnumType.VersionTwo)]
        [TestCase("tests/v21/demography.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/ecb_exr_ng_full.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/response_demo_stub.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/repsonse_cl_all.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/query_cl_all.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/query_cl_regions.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/query_demo_stub.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        [TestCase("tests/v21/query_esms_children.xml", SdmxSchemaEnumType.VersionTwoPointOne)]
        public void TestGetSchemaVersion(string file, SdmxSchemaEnumType expectedResult)
        {
            using (var readable = new FileReadableDataLocation(file))
            {
                Assert.AreEqual(expectedResult, SdmxMessageUtil.GetSchemaVersion(readable));
            }
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestUtilValidateXml.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test validate xml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     The test validate xml.
    /// </summary>
    [TestFixture]
    public class TestUtilValidateXml
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests the <see cref="XMLParser.ValidateXml(System.IO.Stream,Org.Sdmxsource.Sdmx.Api.Constants.SdmxSchemaEnumType)"/>
        /// </summary>
        /// <param name="file">
        /// The input filename
        /// </param>
        [Test]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/CR1_12_data_report.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/ESTAT_STS2GRP_v2.2.xml")]
        [TestCase("tests/v20/query.xml")]
        [TestCase("tests/v20/QueryProvisioningRequest.xml")]
        [TestCase("tests/v20/QueryProvisioningResponse.xml")]
        [TestCase("tests/v20/QueryRegistrationRequest.xml")]
        [TestCase("tests/v20/QueryRegistrationResponse.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/QueryStructureResponse.xml")]
        [TestCase("tests/v20/QUESTIONNAIRE_MSD_v0.xml")]
        [TestCase("tests/v20/SDMX_Query_Type_A_NO_TRANS.xml")]
        [TestCase("tests/v20/SubmitProvisioningRequest.xml")]
        [TestCase("tests/v20/SubmitProvisioningResponse.xml")]
        [TestCase("tests/v20/SubmitRegistrationRequest.xml")]
        [TestCase("tests/v20/SubmitRegistrationResponse.xml")]
        [TestCase("tests/v20/SubmitStructureRequest.xml")]
        [TestCase("tests/v20/SubmitStructureResponse.xml")]
        [TestCase("tests/v20/SubmitSubscriptionRequest.xml")]
        [TestCase("tests/v20/SubmitSubscriptionResponse.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.2_SSTSCONS_PROD_QT_StartTime_Q.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.1_SSTSCONS_PROD_MT_TimeRange_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.2_SSTSCONS_PROD_MT_StartTime_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.2.3_SSTSCONS_PROD_MT_Time_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.1_SSTSCONS_PROD_QT2_TimeRange_Q.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.2_SSTSCONS_PROD_QT2_StartTime_Q.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.3.3_SSTSCONS_PROD_QT2_Time_Q.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.1_SSTSCONS_PROD_MT2_TimeRange_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.4.2_SSTSCONS_PROD_MT2_StartTime_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.5.1_SSTSCONS_PROD_DT_Q_TimeRange_Q.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.6.1_SSTSCONS_PROD_DT_M_TimeRange_M.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.7.1_SSTSCONS_PROD_DT_A_TimeRange_A.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.1_DEMOGRAPHY_RQ_LargeTimeRange.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.2_DEMOGRAPHY_RQ_complexConstraints.xml")]
        [TestCase("tests/v20/DataQuery/TC-2.3_DEMOGRAPHY_RQ_noData.xml")]
        [TestCase("tests/v20/DataQuery/TC-4.1_CENSUSHUB_TEST_Q_Complex query.xml")]
        [TestCase("tests/v20/DataQuery/TC-7.1.1_CPI_PCAXIS_AllCountryRows.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.1_SSTSCONS_PROD_A_DimsClauses.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.2_SSTSCONS_PROD_A_DimsAttsClauses.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.1.3_SSTSCONS_PROD_A_NoData.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.2.1_SSTSCONS_PROD_QT_TRANS.xml")]
        [TestCase("tests/v20/DataQuery/TC-1.3.1.1_SSTSCONS_PROD_QT_TimeRange_Q.xml")]
        [TestCase("tests/v20/StructureRequest/get a category with resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a category.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist failure.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get a concept scheme.xml")]
        [TestCase("tests/v20/StructureRequest/get a dataflow failure.xml")]
        [TestCase("tests/v20/StructureRequest/get a dataflow resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a dataflow.xml")]
        [TestCase("tests/v20/StructureRequest/get a keyfamily resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a keyfamily.xml")]
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml", ExpectedException = typeof(SdmxSyntaxException))]
        [TestCase("tests/v20/StructureRequest/get all categories resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get all categories.xml")]
        [TestCase("tests/v20/StructureRequest/get all codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes.xml")]
        [TestCase("tests/v20/StructureRequest/get all dataflow resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get all dataflow.xml")]
        [TestCase("tests/v20/StructureRequest/get all keyfamily resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get all keyfamily.xml")]
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data FREQ no constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data REF AREA with FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data TIME fake CL with STS ACTIVITY ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        [TestCase("tests/v20/StructureRequest/get two codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get two concept scheme.xml")]
        public void TestValidateXmlV20(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }

        /// <summary>
        /// The test validate xml v 20 error.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/not_well_formed-structure.xml")]
        [TestCase("tests/v20/not_well_formed.xml")]
        [ExpectedException(typeof(XmlException))]
        public void TestValidateXmlV20Error(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }

        /// <summary>
        /// The test validate xml v 20 validation error.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/validation-error-structure.xml")]
        [TestCase("tests/v20/validation-error.xml")]
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml")]
        [ExpectedException(typeof(SdmxSyntaxException))]
        public void TestValidateXmlV20ValidationError(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwo);
            }
        }

        /// <summary>
        /// The test validate xml v 21.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        //// [TestCase("tests/v21/query_cl_all.xml")]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        public void TestValidateXmlV21(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                XMLParser.ValidateXml(stream, SdmxSchemaEnumType.VersionTwoPointOne);
            }
        }

        #endregion
    }
}
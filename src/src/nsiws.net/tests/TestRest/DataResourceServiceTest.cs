// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataResourceServiceTest.cs" company="Eurostat">
//   Date Created : 2013-10-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for <see cref="DataResource" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestRest
{
    using NUnit.Framework;

    /// <summary>
    ///     Test unit for REST data requests
    /// </summary>
    [TestFixture]
    public class DataResourceServiceTest : ResourceServiceTestBase
    {
         #region Public Methods and Operators

        /// <summary>
        /// The test CSV data dataflow.
        /// </summary>
        [Test]
        public void TestCsvDataDaflow()
        {
            RestClientNotImplemented("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "text/csv");
        }

        /**Client OK Requests**/

        // TODO test for parameter DimensionAtObservation, This parameter is useful for cross-sectional data queries, <br>
        // to indicate which dimension should be attached at the observation level (?dimensionAtObservation=FREQ)
        
        /// <summary>
        /// the detail parameter can take the following values "full", "dataonly", "serieskeysonly", "nodata"
        /// </summary>
        [Test]
        public void TestCompact20DetailDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?detail=full", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// the detail parameter can take the following values "full", "dataonly", "serieskeysonly", "nodata"
        /// </summary>
        [Test]
        public void TestBigData()
        {
            RestClientOk("data/ESTAT,HC01/......../?detail=full", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// the detail parameter can take the following values "full", "dataonly", "serieskeysonly", "nodata"
        /// </summary>
        [Test]
        public void TestCrossSectional20()
        {
            RestClientOk("data/ESTAT,DEMOGRAPHY_RQ,1.0/ALL/?detail=full", "application/vnd.sdmx.crosssectionaldata+xml;version=2.0");
        }


        /// <summary>
        /// The test compact 20 end period_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20EndPeriodDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?startPeriod=1920-01&endPeriod=2040-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 first n observation start end period updated after date_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20FirstNObsStartEndPeriodUpdatedAfterDateDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?firstNObservations=10&startPeriod=1920-01&endPeriod=2940-01&updatedAfter=2940-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 first n observations_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20FirstNObservationsDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?firstNObservations=10", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 invalid detail_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20InvalidDetailDataflow()
        {
            RestClientBadRequest("data/ESTAT,SSTSCONS_PROD_A/......./?detail=dummy", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// Test when there is an invalid parameter combination OR
        /// </summary>
      [Test]
        public void TestCompact20InvalidParameterCombinationDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?firstNObservations=5&lastNObservations=10", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 last n observation start end period_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20LastNObsStartEndPeriodDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?lastNObservations=5&startPeriod=1920-01&endPeriod=2940-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 last n observations_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20LastNObservationsDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?lastNObservations=5", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /**Client BAD Requests*/ // </summary>

        /// <summary>
        /// The test compact 20 less keys_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20LessKeysDataflow()
        {
            RestClientBadRequest("data/ESTAT,SSTSCONS_PROD_A/...../?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 no keys_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20NoKeysDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 start end period updated after date_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20StartEndPeriodUpdatedAfterDateDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?startPeriod=1920-01&endPeriod=2940-01&updatedAfter=2940-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 u end period_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20UEndPeriodDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?endPeriod=2940-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20 updated after_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20UpdatedAfterDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......./?updatedAfter=2940-01", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20Dataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact 20_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact20DataflowNoTrailingSlash()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/M.IT.....?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=2.0");
        }

        /// <summary>
        /// The test compact <c>SDMX v2.1</c> dataflow.
        /// </summary>
        [Test]
        public void TestCompact21Dataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=2.1");
        }

        /// <summary>
        /// The test compact 50_ dataflow.
        /// </summary>
        [Test]
        public void TestCompact50Dataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.compactdata+xml;version=5.0");
        }

        /// <summary>
        /// The test cross sectional <c>SDMX v2.1</c> dataflow.
        /// </summary>
        [Test]
        public void TestCrossSectional21Dataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.crosssectionaldata+xml;version=2.1");
        }

        /// <summary>
        /// The test cross sectional data dataflow.
        /// </summary>
        [Test]
        public void TestCrossSectionalDataDataflow()
        {
            RestClientBadRequest("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.crosssectionaldata+xml;version=2.0");
        }

        /// <summary>
        /// The test edi data dataflow.
        /// </summary>
        [Test]
        public void TestEDIDataDataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.edidata");
        }

        /// <summary>
        /// The test generic <c>SDMX v2.1</c> dataflow.
        /// </summary>
        [Test]
        public void TestGeneric21Dataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/......../?startPeriod=2000", "application/vnd.sdmx.genericdata+xml;version=2.1");
        }

        /// <summary>
        /// The test generic metadata_ dataflow.
        /// </summary>
        [Test]
        public void TestGenericMetadataDataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.genericmetadata+xml;version=2.0");
        }

        /// <summary>
        /// The test generic time series data_ dataflow.
        /// </summary>
        [Test]
        public void TestGenericTimeSeriesDataDataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.generictimeseriesdata+xml;version=2.0");
        }

        /// <summary>
        /// The test structure specific <c>SDMX v2.1</c> dataflow.
        /// </summary>
        [Test]
        public void TestStructureSpecific21Dataflow()
        {
            RestClientOk("data/ESTAT,SSTSCONS_PROD_A/A......./?startPeriod=2000", "application/vnd.sdmx.structurespecificdata+xml;version=2.1");
        }

        /// <summary>
        /// The test structure specific metadata_ dataflow.
        /// </summary>
        [Test]
        public void TestStructureSpecificMetadataDataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.structurespecificmetadata+xml;version=2.0");
        }

        /// <summary>
        /// The test structure specific time series data_ dataflow.
        /// </summary>
        [Test]
        public void TestStructureSpecificTimeSeriesDataDataflow()
        {
            RestClientNotAcceptable("data/ESTAT,SSTSCONS_PROD_A/M.IT...../?startPeriod=2000", "application/vnd.sdmx.structurespecifictimeseriesdata+xml;version=2.0");
        }

        #endregion

        #region Methods

        #endregion
    }
}
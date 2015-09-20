// -----------------------------------------------------------------------
// <copyright file="NSIService20Test.cs" company="Eurostat">
//   Date Created : 2013-10-23
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace TestWsSoap
{
    using System;
    using System.IO;

    using Estat.Sri.Ws.Controllers.Constants;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    using ReUsingExamples.NsiWebService;

    /// <summary>
    /// Test unit for SDMX v2.0 based services
    /// </summary>
    [TestFixture("NSIStdV20Service.asmx")]
    [TestFixture("NSIEstatV20Service.asmx")]
    [TestFixture("NSIStdV20Service")]
    [TestFixture("NSIEstatV20Service")]
    public class NSIService20Test
    {
        /// <summary>
        /// The data output.
        /// </summary>
        private const string DataOutput = "../../../../resources/test/output/v20/data/";

        /// <summary>
        /// The data queries.
        /// </summary>
        private const string DataQueries = "../../../../resources/test/queries/v20/data/";

        /// <summary>
        /// The structure output.
        /// </summary>
        private const string StructureOutput = "../../../../resources/test/output/v20/structure/";

        /// <summary>
        /// The structure queries.
        /// </summary>
        private const string StructureQueries = "../../../../resources/test/queries/v20/structure/";

        /// <summary>
        /// The schema version.
        /// </summary>
        private const SdmxSchemaEnumType SchemaVersion = SdmxSchemaEnumType.VersionTwo;
        private readonly string _endpoint;

        private readonly SdmxServiceTestInvoker _wsInvoker;

        /// <summary>
        /// The WS client.
        /// </summary>
        private readonly SdmxWsClient wsClient;
        public NSIService20Test(string endpoint)
        {
            this._wsInvoker = new SdmxServiceTestInvoker();
            var baseUrl = new Uri(Properties.Settings.Default.baseURL);
            var endpointUri = new Uri(baseUrl, endpoint);
            this.wsClient = new SdmxWsClient(endpointUri, SchemaVersion); 
        }

        // data operation queries
        [Test]
        public void GetGenericData()
        {
            FileInfo request = new FileInfo(Path.Combine(DataQueries, "Query_SSTSCONS_PROD_A_v2.0.xml"));
            FileInfo response = new FileInfo(Path.Combine(DataOutput, "Query_SSTSCONS_PROD_A_v2.0_out_generic.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeService(wsClient, SoapOperation.GetGenericData, request, response, SchemaVersion), "Wrong response.");
        }

        [Test]
        public void GetCompactData()
        {
            FileInfo request = new FileInfo(Path.Combine(DataQueries, "Query_SSTSCONS_PROD_A_v2.0.xml"));
            FileInfo response = new FileInfo(Path.Combine(DataOutput, "Query_SSTSCONS_PROD_A_v2.0_out_compact.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeService(wsClient, SoapOperation.GetCompactData, request, response, SchemaVersion), "Wrong response.");
        }

        // error for data queries
        [Test]
        public void GetCompactDataInvalid()
        {
            FileInfo request = new FileInfo(Path.Combine(DataQueries, "Query_SSTSCONS_PROD_A_v2.0_invalid.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeServiceErrorAssert(wsClient, SoapOperation.GetCompactData, request, 2000), "Didn't got error expected.");
        }

        [Test]
        public void GetXsForNoXsDsd()
        {
            FileInfo request = new FileInfo(Path.Combine(DataQueries, "Query_SSTSCONS_PROD_A_v2.0.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeServiceErrorAssert(wsClient, SoapOperation.GetCrossSectionalData, request, 2000), "Didn't got error expected.");
        }


        // structure operation queries
        [Test]
        public void QueryStructure()
        {
            FileInfo request = new FileInfo(Path.Combine(StructureQueries, "QueryStructureRequest_CL_ESTAT_ALL.xml"));
            FileInfo response = new FileInfo(Path.Combine(StructureOutput, "QueryStructureRequest_CL_ESTAT_ALL_out.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeService(wsClient, SoapOperation.QueryStructure, request, response, SchemaVersion), "Wrong response.");
        }

        // error for structure queries
        [Test]
        public void QueryStructureInvalidOperation()
        {
            FileInfo request = new FileInfo(Path.Combine(StructureQueries, "QueryStructureRequest_CL_ESTAT_ALL.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeServiceErrorAssert(wsClient, SoapOperation.GetCodelist, request, 2000), "Didn't got error expected.");
        }

        [Test]
        public void QueryStructureInvalid()
        {
            FileInfo request = new FileInfo(Path.Combine(StructureQueries, "QueryStructureRequest_invalid.xml"));
            Assert.IsTrue(this._wsInvoker.InvokeServiceErrorAssert(wsClient, SoapOperation.QueryStructure, request, 2000), "Didn't got error expected.");
        }

    }
}
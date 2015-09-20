// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NSIStdService21Test.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The nsi std service 21 test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestWsSoap
{
    using System;
    using System.IO;

    using Estat.Sri.Ws.Controllers.Constants;

    using log4net;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    using ReUsingExamples.NsiWebService;

    /// <summary>
    /// The SDMX v2.1 SOAP service test.
    /// </summary>
    [TestFixture]
    public class NSIStdService21Test
    {
        #region Constants

        /// <summary>
        /// The data output.
        /// </summary>
        private const string DataOutput = "../../../../resources/test/output/v21/data/";

        /// <summary>
        /// The data queries.
        /// </summary>
        private const string DataQueries = "../../../../resources/test/queries/v21/data/";

        /// <summary>
        /// The endpoint.
        /// </summary>
        private const string Endpoint = "SdmxService";

        /// <summary>
        /// The structure output.
        /// </summary>
        private const string StructureOutput = "../../../../resources/test/output/v21/structure/";

        /// <summary>
        /// The structure queries.
        /// </summary>
        private const string StructureQueries = "../../../../resources/test/queries/v21/structure/";

        /// <summary>
        /// The schema version.
        /// </summary>
        private const SdmxSchemaEnumType SchemaVersion = SdmxSchemaEnumType.VersionTwoPointOne;

        #endregion

        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NSIStdService21Test));

        /// <summary>
        /// The WS invoker.
        /// </summary>
        private static readonly SdmxServiceTestInvoker _wsInvoker = new SdmxServiceTestInvoker();

        #endregion

        #region Fields

        /// <summary>
        /// The WS client.
        /// </summary>
        private SdmxWsClient _wsClient;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the categorisation query.
        /// </summary>
        [Test]
        public void GetCategorisationQuery()
        {
            var request = new FileInfo(StructureQueries + "CategorisationQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "CategorisationQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetCategorisation, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the categorisation no results.
        /// </summary>
        [Test]
        public void GetCategorisationNoResults()
        {
            var request = new FileInfo(StructureQueries + "CategorisationQuery_v21_no_results.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetCategorisation, request, SdmxErrorCodeEnumType.NoResultsFound), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the category schema query.
        /// </summary>
        [Test]
        public void GetCategorySchemaQuery()
        {
            var request = new FileInfo(StructureQueries + "CategorySchemaQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "CategorySchemaQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetCategoryScheme, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the codelist query.
        /// </summary>
        [Test]
        public void GetCodelistQuery()
        {
            var request = new FileInfo(StructureQueries + "CodelistQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "CodelistQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetCodelist, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the codelist query.
        /// </summary>
        [Test]
        public void GetCodelistQueryNoResults()
        {
            var request = new FileInfo(StructureQueries + "CodelistQuery_v21_no_results.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetCodelist, request, SdmxErrorCodeEnumType.NoResultsFound), "Wrong response.");
        }

        /// <summary>
        ///     Gets the concept scheme query.
        /// </summary>
        [Test]
        public void GetConceptSchemeQuery()
        {
            var request = new FileInfo(StructureQueries + "ConceptSchemeQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "ConceptSchemeQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetConceptScheme, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the concept scheme query.
        /// </summary>
        [Test]
        public void GetConceptSchemeQueryNoResults()
        {
            var request = new FileInfo(StructureQueries + "ConceptSchemeQuery_v21_no_results.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetConceptScheme, request, SdmxErrorCodeEnumType.NoResultsFound), "Wrong response.");
        }
        
        /// <summary>
        ///     Gets the data structure query.
        /// </summary>
        [Test]
        public void GetDataStructureQuery()
        {
            var request = new FileInfo(StructureQueries + "DataStructureQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "DataStructureQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetDataStructure, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the data structure query.
        /// </summary>
        [Test]
        public void GetDataStructureQueryNoResults()
        {
            var request = new FileInfo(StructureQueries + "DataStructureQuery_v21_no_results.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetDataStructure, request, SdmxErrorCodeEnumType.NoResultsFound), "Wrong response.");
        }

        /// <summary>
        ///     Gets the data structure query invalid.
        /// </summary>
        [Test]
        public void GetDataStructureQueryInvalid()
        {
            var request = new FileInfo(StructureQueries + "DataStructureQuery_v21_invalid.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetDataStructure, request, SdmxErrorCodeEnumType.SyntaxError), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the data structure query using codelist operation.
        /// </summary>
        [Test]
        public void GetDataStructureQueryUsingCodelistOperation()
        {
            var request = new FileInfo(StructureQueries + "DataStructureQuery_v21.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetCodelist, request, SdmxErrorCodeEnumType.SyntaxError), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the dataflow query.
        /// </summary>
        [Test]
        public void GetDataflowQuery()
        {
            var request = new FileInfo(StructureQueries + "DataflowQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "DataflowQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetDataflow, request, response, SchemaVersion), "Wrong response.");
        }


        /// <summary>
        ///     Gets the dataflow query.
        /// </summary>
        [Test]
        public void GetDataflowQueryNoResults()
        {
            var request = new FileInfo(StructureQueries + "DataflowQuery_v21_no_results.xml");
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetDataflow, request, SdmxErrorCodeEnumType.NoResultsFound), "Wrong response.");
        }


        /// <summary>
        ///     Gets the generic data.
        /// </summary>
        [Test]
        public void GetGenericData()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "GenericDataQuery_sample_v21.xml"));
            var response = new FileInfo(Path.Combine(DataOutput, "GenericDataQuery_sample_v21_out.xml"));
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetGenericData, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the generic data.
        /// </summary>
        [Test]
        public void GetGenericDataNoResults()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "GenericDataQuery_sample_v21_no_results.xml"));
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetGenericData, request, SdmxErrorCodeEnumType.NoResultsFound), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the generic data.
        /// </summary>
        [Test]
        public void GetGenericDataNoResults2()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "GenericDataQuery_sample_v21_no_results2.xml"));
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetGenericData, request, SdmxErrorCodeEnumType.NoResultsFound), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the hierarchical codelist query.
        /// </summary>
        [Test]
        public void GetHierarchicalCodelistQuery()
        {
            var request = new FileInfo(StructureQueries + "HCLQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "HCLQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetHierarchicalCodelist, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the structure specific data.
        /// </summary>
        [Test]
        public void GetStructureSpecificData()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "StructureSpecificDataQuery_sample1_v21.xml"));
            var response = new FileInfo(Path.Combine(DataOutput, "StructureSpecificDataQuery_sample1_v21_out.xml"));
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetStructureSpecificData, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Gets the structure specific data with generic operation.
        /// </summary>
        [Test]
        public void GetStructureSpecificDataWithGenericOperation()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "StructureSpecificDataQuery_sample1_v21.xml"));
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetGenericData, request, SdmxErrorCodeEnumType.SyntaxError), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the structure specific data_ invalid.
        /// </summary>
        [Test]
        public void GetStructureSpecificDataInvalid()
        {
            var request = new FileInfo(Path.Combine(DataQueries, "StructureSpecificDataQuery_sample1_v21_invalid.xml"));
            Assert.IsTrue(_wsInvoker.InvokeServiceErrorAssert(this._wsClient, SoapOperation.GetStructureSpecificData, request, SdmxErrorCodeEnumType.SyntaxError), "Didn't got error expected.");
        }

        /// <summary>
        ///     Gets the structures.
        /// </summary>
        [Test]
        public void GetStructures()
        {
            var request = new FileInfo(StructureQueries + "StucturesQuery_v21.xml");
            var response = new FileInfo(StructureOutput + "StucturesQuery_v21_out.xml");
            Assert.IsTrue(_wsInvoker.InvokeService(this._wsClient, SoapOperation.GetStructures, request, response, SchemaVersion), "Wrong response.");
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            try
            {
                var baseUrl = new Uri(Properties.Settings.Default.baseURL);
                this._wsClient = new SdmxWsClient(new Uri(baseUrl, Endpoint), SchemaVersion);
            }
            catch (Exception e)
            {
                _log.Error("Error at Test Init", e);
                Assert.Fail(e.Message);
            }
        }

        #endregion
    }
}
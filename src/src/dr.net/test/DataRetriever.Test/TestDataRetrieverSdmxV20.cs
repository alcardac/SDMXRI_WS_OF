// -----------------------------------------------------------------------
// <copyright file="TestDataRetrieverSdmxV20.cs" company="EUROSTAT">
//   Date Created : 2015-03-05
//   Copyright (c) 2015 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace DataRetriever.Test
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Xml;

    using Estat.Nsi.DataRetriever;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="DataRetrieverCore"/>
    /// </summary>
    [TestFixture]
    public class TestDataRetrieverSdmxV20
    {
        /// <summary>
        /// The data query parse manager.
        /// </summary>
        private IDataQueryParseManager dataQueryParseManager;

        /// <summary>
        /// The parsing manager.
        /// </summary>
        private IStructureParsingManager parsingManager;

        /// <summary>
        /// Test unit for <see cref="DataRetrieverCore"/> 
        /// </summary>
        [TestCase("tests/v20/two-components-under-or.xml", "tests/v20/dataflows.xml", "tests/v20/ESTAT+STS+2.0.xml", "sqlserver4")]
        public void TestNestedAndOr(string queryFile, string dataflowFile, string dsdFile, string name)
        {
            var retrievalManager = this.GetSdmxObjectRetrievalManager(dataflowFile, dsdFile);
            var connectionString = ConfigurationManager.ConnectionStrings [name];
            ISdmxDataRetrievalWithWriter dr = new DataRetrieverCore(new HeaderImpl("TestNestedAndOr", "ZZ9"), connectionString, SdmxSchemaEnumType.VersionTwo);
            IList<IDataQuery> dataQuery;
            using (var fileReadableDataLocation = new FileReadableDataLocation(queryFile))
            {
                dataQuery = this.dataQueryParseManager.BuildDataQuery(fileReadableDataLocation, retrievalManager);
                Assert.IsNotEmpty(dataQuery);
            }

            var outputFileName = string.Format("{0}-TestNestedAndOr-out.xml", queryFile);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            using (IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                dr.GetData(dataQuery.First(), dataWriter);
            }
        }

        /// <summary>
        /// The init.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.Null);
            parsingManager = new StructureParsingManager();
        }

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
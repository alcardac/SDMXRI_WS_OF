// -----------------------------------------------------------------------
// <copyright file="TestXS.cs" company="Eurostat">
//   Date Created : 2013-12-03
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace DataRetriever.Test
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Nsi.DataRetriever;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    [TestFixture("sqlserver3")]
    public class TestXS
    {
        #region Fields

        /// <summary>
        /// The _data query parse manager.
        /// </summary>
        private readonly IDataQueryParseManager _dataQueryParseManager;

        /// <summary>
        /// The _data retrieval.
        /// </summary>
        private readonly ISdmxDataRetrievalWithCrossWriter _dataRetrieval;

        /// <summary>
        /// The _retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _retrievalManager;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TestXS"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public TestXS(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings [name];
            this._dataRetrieval = new DataRetrieverCore(new HeaderImpl("TestXS", "ZZ9"), connectionString, SdmxSchemaEnumType.VersionTwo);
            this._dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            this._retrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
        }

        [TestCase("tests/v20/get-CENSUSHUB_Q_XS1.xml")]
        public void TestXSOutput(string filePath)
        {
            IList<IDataQuery> dataQueries;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildDataQuery(dataLocation, this._retrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName))
            {
                var dataWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));

                this._dataRetrieval.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            var fileInfo = new FileInfo(outputFileName);
            Assert.IsTrue(fileInfo.Exists);
            var dsd = dataQuery.DataStructure as ICrossSectionalDataStructureObject;
            Assert.NotNull(dsd);
            using (var fileStream = fileInfo.OpenRead())
            using (var reader = XmlReader.Create(fileStream))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                var localName = reader.LocalName;
                                switch (localName)
                                {
                                    case "DataSet":
                                        AssertDimensionsAreMapped(dsd.GetCrossSectionalAttachDataSet(true, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension)), reader);
                                        break;
                                    case "Section":
                                        AssertDimensionsAreMapped(dsd.GetCrossSectionalAttachSection(true, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension)), reader);

                                        break;
                                    case "Group":
                                        AssertDimensionsAreMapped(dsd.GetCrossSectionalAttachGroup(true, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension)), reader);

                                        break;
                                    case "OBS_VALUE":
                                        AssertDimensionsAreMapped(dsd.GetCrossSectionalAttachObservation(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension)), reader);
                                        break;
                                }
                            }

                            break;
                    }
                }
            }
        }

        private static void AssertDimensionsAreMapped(IEnumerable<IComponent> attachedComponents, XmlReader reader)
        {
            foreach (var dimension in attachedComponents)
            {
                var value = reader.GetAttribute(dimension.Id);
                Assert.IsNotNullOrEmpty(value, "Dimension : {0}", dimension.Id);
            }
        }
    }
}
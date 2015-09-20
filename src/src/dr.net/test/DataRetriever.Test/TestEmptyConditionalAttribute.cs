// -----------------------------------------------------------------------
// <copyright file="TestEmptyConditionalAttribute.cs" company="Eurostat">
//   Date Created : 2014-05-19
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace DataRetriever.Test
{
    using System.Configuration;
    using System.IO;
    using System.Xml;

    using Estat.Nsi.DataRetriever;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;

    /// <summary>
    /// Test unit for <see cref="DataRetrieverCore"/>
    /// </summary>
    public class TestEmptyConditionalAttribute
    {
        /// <summary>
        /// Test unit for <see cref="DataRetrieverCore.GetData(Org.Sdmxsource.Sdmx.Api.Model.Data.Query.IDataQuery,Org.Sdmxsource.Sdmx.Api.Engine.IDataWriterEngine)"/> 
        /// </summary>
        [TestCase("/data/SSTSCONS_PROD_A/ALL", "sqlserver2")]
        public void TestGetDataDataWriterEngineEmptyAttr(string query, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings [name];
            var dataQuery = new DataQueryImpl(new RESTDataQueryCore(query), new MappingStoreSdmxObjectRetrievalManager(connectionString));
            const string EmptyAttributeXML = "empty-attribute.xml";
            using (XmlWriter writer = XmlWriter.Create(EmptyAttributeXML, new XmlWriterSettings() {Indent = true}))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                ISdmxDataRetrievalWithWriter sdmxDataRetrievalWithWriter = new DataRetrieverCore(new HeaderImpl("TestEmptyConditionalAttribute", "ZZ9"), connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                sdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }
            var fileInfo = new FileInfo(EmptyAttributeXML);
            Assert.IsTrue(fileInfo.Exists);
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
                                if (localName.Equals("Group"))
                                {
                                    Assert.IsTrue(reader.HasAttributes);
                                    var dateStr = reader.GetAttribute("NAT_TITLE");
                                    Assert.IsNull(dateStr);
                                }
                                else if (localName.Equals("Obs"))
                                {
                                    Assert.IsTrue(reader.HasAttributes);
                                    var attribute = reader.GetAttribute("OBS_COM");
                                    Assert.IsTrue(attribute == null || !string.IsNullOrWhiteSpace(attribute));
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
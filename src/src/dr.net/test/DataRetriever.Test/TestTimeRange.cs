// -----------------------------------------------------------------------
// <copyright file="TestTimeRange.cs" company="Eurostat">
//   Date Created : 2014-07-14
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
    using System.Linq;
    using System.Xml;

    using Estat.Nsi.DataRetriever;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    [TestFixture("sqlserver3")]
    public class TestTimeRange
    {
        /// <summary>
        /// The _data retrieval.
        /// </summary>
        private readonly ISdmxDataRetrievalWithWriter _dataRetrievalRest;

        /// <summary>
        /// The _retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _retrievalManager;

        private readonly IDataQueryParseManager _dataQueryParseManager;

        public TestTimeRange(string connectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            this._dataRetrievalRest = new DataRetrieverCore(new HeaderImpl("TEST_ID", "TEST_SENDER"), connectionString, SdmxSchemaEnumType.VersionTwo);
            this._retrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            this._dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.Null);
        }

        [TestCase("tests/v20/QueryTimeRangeV20.xml")]
        public void TestSDMXv20(string queryFile)
        {
            IDataQuery query;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(queryFile))
            {
                query = this._dataQueryParseManager.BuildDataQuery(dataLocation, this._retrievalManager).First();
            }
            
            var outputFileName = string.Format("{0}-out.xml", queryFile);

            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings() {Indent = true}))
            {
                var compactWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
                this._dataRetrievalRest.GetData(query, compactWriter);
            }
            
            var selectionGroup = query.SelectionGroups.First();

            var fileInfo = new FileInfo(outputFileName);
            Assert.IsTrue(fileInfo.Exists);
            using (var fileStream = fileInfo.OpenRead())
            using (var reader = XmlReader.Create(fileStream))
            {
                TimeFormat freqValue = TimeFormat.GetFromEnum(TimeFormatEnumType.Null);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                var localName = reader.LocalName;
                                if (localName.Equals("Series"))
                                {
                                    Assert.IsTrue(reader.HasAttributes);
                                    freqValue = TimeFormat.GetTimeFormatFromCodeId(reader.GetAttribute("FREQ"));
                                }
                                else if (localName.Equals("Obs"))
                                {
                                    Assert.NotNull(freqValue);
                                    Assert.IsTrue(reader.HasAttributes);
                                    var dateStr = reader.GetAttribute(DimensionObject.TimeDimensionFixedId);
                                    ISdmxDate date = new SdmxDateCore(dateStr);
                                    var dateFrom = new SdmxDateCore(new SdmxDateCore(selectionGroup.DateFrom.Date, freqValue).DateInSdmxFormat);
                                    var dateTo = new SdmxDateCore(new SdmxDateCore(selectionGroup.DateTo.Date, freqValue).DateInSdmxFormat);
                                    Assert.GreaterOrEqual(date.Date, dateFrom.Date);
                                    Assert.LessOrEqual(date.Date, dateTo.Date);
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
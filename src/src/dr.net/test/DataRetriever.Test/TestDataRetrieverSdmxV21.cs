// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataRetrieverSdmxV21.cs" company="Eurostat">
//   Date Created : 2013-12-02
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The test data retriever sdmx v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace DataRetriever.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Nsi.DataRetriever;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util.Collections;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The test data retriever sdmx v 21.
    /// </summary>
    public class TestDataRetrieverSdmxV21
    {
        #region Fields

        /// <summary>
        /// The _data query parse manager.
        /// </summary>
        private readonly IDataQueryParseManager _dataQueryParseManager;

        /// <summary>
        /// The _default header
        /// </summary>
        private readonly HeaderImpl _defaultHeader;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataRetrieverSdmxV21"/> class. 
        /// </summary>
        public TestDataRetrieverSdmxV21()
        {
            this._dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwoPointOne);
            this._defaultHeader = new HeaderImpl("TestDataRetrieverSdmxV21", "ZZ9");
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The test dimension at OBS.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <param name="name">The connection name.</param>
        [TestCase("tests/get-structure-specific-primary-measure-op.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-primary-measure-op2.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-primary-measure-op-and.xml", "sqlserver2")]
        public void TestPrimaryMeasureOperator(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IList<IComplexDataQuery> dataQueries;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, new MappingStoreSdmxObjectRetrievalManager(connectionString));
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();
            var selectionGroup = dataQuery.SelectionGroups.First();
            ////CollectionAssert.IsNotEmpty(selectionGroup.PrimaryMeasureValue);

            Predicate<string> validatePm = null;
            if (selectionGroup.PrimaryMeasureValue.Count > 0)
            {
                validatePm = GetPrimaryMeasureValues(selectionGroup.PrimaryMeasureValue);
            }
            else if (selectionGroup.HasSelectionForConcept(PrimaryMeasure.FixedId))
            {
                var querySelection = selectionGroup.GetSelectionsForConcept(PrimaryMeasure.FixedId);
                validatePm = querySelection.HasMultipleValues() ? GetPrimaryMeasureValues(querySelection.Values) : GetPrimaryMeasureValues(new[] { querySelection.Value });
            }
            else
            {
                Assert.Fail("could not set the rules");
            }

            var outputFileName = string.Format("{0}-TestPrimaryMeasureOperator-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            var fileInfo = new FileInfo(outputFileName);
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
                                if (localName.Equals("Obs"))
                                {
                                    validatePm(reader.GetAttribute(PrimaryMeasure.FixedId));
                                }
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="DataRetrieverCore"/> 
        /// </summary>
        [TestCase("tests/get-structure-specific-nested-and-in-or.xml", "sqlserver4")]
        public void TestNestedAndOr(string queryFile,  string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings [name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(queryFile))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-TestNestedAndOrv21-out.xml", queryFile);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }
        }

        /// <summary>
        /// Tests the default limit.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("tests/get-structure-specific-data-firstnobs-lastnobs.xml", "sqlserver2")]
        public void TestFirstLastNObs(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
                var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-TestFirstNObs-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// Tests the default limit.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("tests/get-structure-specific-data-firstnobs.xml", "sqlserver2")]
        public void TestFirstNObs(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-TestFirstNObs-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// Tests the default limit.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("tests/get-structure-specific-full-notequal-and.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-full-equal-or.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-full-numeric-less.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-full-numeric-greater.xml", "sqlserver2")]
        [TestCase("tests/text-search-madb-contains.xml", "sqlserver3")]
        [TestCase("tests/text-search-madb-not-contains.xml", "sqlserver3")]
        public void TestSimpleOperators(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-TestSimpleOperators-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// Tests the default limit.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("tests/get-structure-specific-data-lastnobs.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-data-lastnobs2.xml", "sqlserver2")]
        public void TestLastNObs(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-TestLastNObs-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// Tests the default limit.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("tests/get-structure-specific-data.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-data-default-limit.xml", "sqlserver2", ExpectedException = typeof(SdmxResponseSizeExceedsLimitException))]
        public void TestDefaultLimit(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
            IList<IComplexDataQuery> dataQueries;
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// The test component id in query.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <param name="name">The connection name.</param>
        [TestCase("tests/get-structure-specific-data.xml", "sqlserver2")]
        public void TestComponentIdInQuery(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IList<IComplexDataQuery> dataQueries;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, new MappingStoreSdmxObjectRetrievalManager(connectionString));
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            var fileInfo = new FileInfo(outputFileName);
            Assert.IsTrue(fileInfo.Exists);
        }

        /// <summary>
        /// The test dimension at OBS.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <param name="name">The connection name.</param>
        [TestCase("tests/get-structure-specific-data-dimatobs.xml", "sqlserver2")]
        public void TestDimensionAtObsArea(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IList<IComplexDataQuery> dataQueries;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, new MappingStoreSdmxObjectRetrievalManager(connectionString));
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            var fileInfo = new FileInfo(outputFileName);
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
                                if (localName.Equals("Series"))
                                {
                                    Assert.IsTrue(reader.HasAttributes);
                                    var dateStr = reader.GetAttribute("TIME_PERIOD");
                                    Assert.IsNotNull(dateStr);
                                    var timeFormatOfDate = DateUtil.GetTimeFormatOfDate(dateStr);
                                    Assert.NotNull(timeFormatOfDate);
                                }
                                else if (localName.Equals("Obs"))
                                {
                                    Assert.IsTrue(reader.HasAttributes);
                                    Assert.AreEqual("IT", reader.GetAttribute("AREA"));
                                }
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Tests the equal time value.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The connection name</param>
        [TestCase("tests/ESTAT+SSTSIND_PROD_M+2.0_2014_01_28_18_05_46.query.xml", "sqlserver3")]
        [TestCase("tests/IT1+161_267+1.0_2014_06_03_17_17_55.query.xml", "sqlserver")]
        public void TestEqualTimeValue(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
             IList<IComplexDataQuery> dataQueries;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, new MappingStoreSdmxObjectRetrievalManager(connectionString));
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();
            var selectionGroup = dataQuery.SelectionGroups.First();
            Assert.AreEqual(OrderedOperatorEnumType.GreaterThanOrEqual, selectionGroup.DateFromOperator.EnumType);
            Assert.AreEqual(OrderedOperatorEnumType.LessThanOrEqual, selectionGroup.DateToOperator.EnumType);
            Assert.AreEqual(selectionGroup.DateFrom, selectionGroup.DateTo);
            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }
           
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

        /// <summary>
        /// The test dimension at OBS.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <param name="name">The connection name</param>
        [TestCase("tests/get-structure-specific-data-dimatobs.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-data-dimatobs-sts_activity.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-data2.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-dataonly.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-full.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-full-all-dim.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-serieskey.xml", "sqlserver2")]
        [TestCase("tests/get-structure-specific-nodata.xml", "sqlserver2")]
        public void TestDimensionAtObsGeneral(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            IList<IComplexDataQuery> dataQueries;
                var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(filePath))
            {
                dataQueries = this._dataQueryParseManager.BuildComplexDataQuery(dataLocation, mappingStoreSdmxObjectRetrievalManager);
            }

            Assert.IsNotEmpty(dataQueries);
            var dataQuery = dataQueries.First();

            var outputFileName = string.Format("{0}-out.xml", filePath);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                advancedSdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// The test dimension at OBS.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="name">The name.</param>
        [TestCase("data/IT,56_259/ALL/ALL", "sqlserver")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_A/ALL/ALL?dimensionAtObservation=AREA", "sqlserver2")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_A/ALL/ALL?dimensionAtObservation=STS_ACTIVITY", "sqlserver2")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_A/ALL/ALL?dimensionAtObservation=STS_INDICATOR", "sqlserver2")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_A/ALL/ALL?dimensionAtObservation=TIME_PERIOD", "sqlserver2")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_A/ALL/ALL?dimensionAtObservation=AllDimensions", "sqlserver2")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&lastNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&lastNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&lastNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&lastNObservations=1&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&lastNObservations=1&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&lastNObservations=1&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSCONS_PROD_DT_M/ALL/ALL?dimensionAtObservation=AllDimensions&lastNObservations=1&firstNObservations=1", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&firstNObservations=3", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&firstNObservations=4", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&firstNObservations=4", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&firstNObservations=4", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&lastNObservations=3", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&lastNObservations=5", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&lastNObservations=5", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&lastNObservations=5", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&lastNObservations=5&firstNObservations=4", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&lastNObservations=3&firstNObservations=4", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&lastNObservations=5&firstNObservations=3", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&lastNObservations=5&firstNObservations=3", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&detail=dataonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&detail=nodata", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&detail=serieskeysonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_ACTIVITY&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&detail=dataonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&detail=nodata", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&detail=serieskeysonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=STS_INDICATOR&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=dataonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=nodata", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=serieskeysonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&detail=dataonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&detail=nodata", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&detail=serieskeysonly", "sqlserver3")]
        [TestCase("data/ESTAT,SSTSIND_PROD_M/ALL/ALL?dimensionAtObservation=AllDimensions&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,DEMOGRAPHY/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,DEMOGRAPHY/ALL/ALL?dimensionAtObservation=AllDimensions&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,CPI_PCAXIS/ALL/ALL?dimensionAtObservation=TIME_PERIOD&detail=full", "sqlserver3")]
        [TestCase("data/ESTAT,CPI_PCAXIS/ALL/ALL?dimensionAtObservation=AllDimensions&detail=full", "sqlserver3")]
        public void TestDimensionAtObsRest(string filePath, string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            var mappingStoreSdmxObjectRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(connectionString);
            var dataQuery = this._dataQueryParseManager.ParseRestQuery(filePath, mappingStoreSdmxObjectRetrievalManager);

            Assert.IsNotNull(dataQuery);

            var outputFileName = string.Format("REST-{0}-{1}-{2}-{3}-out.xml", dataQuery.Dataflow.Id, dataQuery.DimensionAtObservation, dataQuery.FirstNObservations, dataQuery.LastNObservations);
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            {
                IDataWriterEngine dataWriter = new CompactDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                ISdmxDataRetrievalWithWriter sdmxDataRetrievalWithWriter = new DataRetrieverCore(this._defaultHeader, connectionString, SdmxSchemaEnumType.VersionTwoPointOne);
                sdmxDataRetrievalWithWriter.GetData(dataQuery, dataWriter);
                writer.Flush();
            }

            Validate(dataQuery, outputFileName, mappingStoreSdmxObjectRetrievalManager);
        }

        /// <summary>
        /// Validates the dimension at observation.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="outputFileName">Name of the output file.</param>
        /// <param name="sdmxObjectRetrievalManager">The SDMX object retrieval manager.</param>
        private static void Validate(IBaseDataQuery dataQuery, string outputFileName, ISdmxObjectRetrievalManager sdmxObjectRetrievalManager)
        {
            var dimensionAtObservation = dataQuery.DimensionAtObservation;
            var isFlat = dimensionAtObservation.Equals(DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value);
            var detailLevel = dataQuery.DataQueryDetail.EnumType;
            int obsCountPerSeries = 0;
            bool hasFirstLast = dataQuery.FirstNObservations.HasValue || dataQuery.LastNObservations.HasValue;
            int maxObsPerSeries = (dataQuery.FirstNObservations.HasValue ? dataQuery.FirstNObservations.Value : 0) + (dataQuery.LastNObservations.HasValue ? dataQuery.LastNObservations.Value : 0);
            var componentMap = BuildComponentCodeMap(dataQuery.DataStructure, sdmxObjectRetrievalManager);
            var fileInfo = new FileInfo(outputFileName);
            Assert.IsTrue(fileInfo.Exists);

            var queriedOrValues = new DictionaryOfLists<string, Predicate<string>>(StringComparer.Ordinal);
            var queriedAndValues = new DictionaryOfLists<string, Predicate<string>>(StringComparer.Ordinal);

            var complex = dataQuery as IComplexDataQuery;
            if (complex != null)
            {
                GetComplexQueriedValues(complex, queriedOrValues, queriedAndValues);
            }
            else
            {
                var simple = dataQuery as IDataQuery;
                if (simple != null)
                {
                    GetSimpleQueriedValues(simple, queriedOrValues);
                }
            }

            var seriesKeyObs = new Dictionary<string, int>(StringComparer.Ordinal);

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
                                if (localName.Equals("Series"))
                                {
                                    Assert.IsFalse(isFlat);
                                    obsCountPerSeries = 0;
                                    Assert.IsTrue(reader.HasAttributes);
                                    foreach (var dimension in dataQuery.DataStructure.GetDimensions())
                                    {
                                        var value = reader.GetAttribute(dimension.Id);
                                        if (dimension.Id.Equals(dimensionAtObservation))
                                        {
                                            Assert.IsNull(value, "Dimension {0}", dimension.Id);
                                        }
                                        else
                                        {
                                            Assert.IsNotNullOrEmpty(value, "Dimension {0}", dimension.Id);
                                            if (dimension.HasCodedRepresentation())
                                            {
                                                Assert.IsTrue(componentMap[dimension.Id].Contains(value));
                                            }

                                            ValidateAgainstCriteria(dimension, value, queriedOrValues, queriedAndValues);
                                        }
                                    }

                                    ValidateAttribute(dataQuery, reader, componentMap, new[] { DataQueryDetailEnumType.Full, DataQueryDetailEnumType.NoData }, dataQuery.DataStructure.GetSeriesAttributes(dimensionAtObservation), queriedOrValues, queriedAndValues);
                                }
                                else if (localName.Equals("Obs"))
                                {
                                    Assert.AreNotEqual(DataQueryDetailEnumType.NoData, detailLevel);
                                    Assert.AreNotEqual(DataQueryDetailEnumType.SeriesKeysOnly, detailLevel);
                                    if (isFlat && hasFirstLast)
                                    {
                                        var key = BuildKey(dataQuery.DataStructure, reader);
                                        int count;
                                        if (!seriesKeyObs.TryGetValue(key, out count))
                                        {
                                            count = 0;
                                        }

                                        count++;
                                        seriesKeyObs[key] = count;

                                        Assert.LessOrEqual(count, maxObsPerSeries);
                                    }

                                    obsCountPerSeries++;

                                    Assert.IsTrue(reader.HasAttributes);
                                    foreach (var dimension in dataQuery.DataStructure.GetDimensions())
                                    {
                                        var value = reader.GetAttribute(dimension.Id);
                                        if (dimension.Id.Equals(dimensionAtObservation) || isFlat)
                                        {
                                            Assert.IsNotNullOrEmpty(value, "Dimension {0}", dimension.Id);
                                            if (dimension.HasCodedRepresentation())
                                            {
                                                var codes = componentMap[dimension.Id];
                                                CollectionAssert.Contains(codes, value);
                                            }

                                            ValidateAgainstCriteria(dimension, value, queriedOrValues, queriedAndValues);
                                        }
                                        else
                                        {
                                            Assert.IsNull(value, "Dimension {0}", dimension.Id);
                                        }
                                    }

                                    ValidateAgainstCriteria(dataQuery.DataStructure.PrimaryMeasure, reader.GetAttribute(PrimaryMeasure.FixedId), queriedOrValues, queriedAndValues);

                                    ValidateAttribute(
                                        dataQuery,
                                        reader,
                                        componentMap,
                                        new[] { DataQueryDetailEnumType.Full },
                                        dataQuery.DataStructure.GetObservationAttributes(dimensionAtObservation),
                                        queriedOrValues,
                                        queriedAndValues);

                                    if (!isFlat && hasFirstLast)
                                    {
                                        Assert.LessOrEqual(obsCountPerSeries, maxObsPerSeries);
                                    }
                                }
                                else if (localName.Equals("Group"))
                                {
                                    CollectionAssert.Contains(new[] { DataQueryDetailEnumType.Full, DataQueryDetailEnumType.NoData }, detailLevel);
                                    var urn = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
                                    Assert.NotNull(urn);
                                    var groupId = urn.Split(':').Last();
                                    var dsdGroup = dataQuery.DataStructure.GetGroup(groupId);
                                    foreach (var dimensionRef in dsdGroup.DimensionRefs)
                                    {
                                        var dimValue = reader.GetAttribute(dimensionRef);
                                        Assert.IsNotNullOrEmpty(dimValue);
                                        var dimension = dataQuery.DataStructure.GetDimension(dimensionRef);
                                        if (dimension.HasCodedRepresentation())
                                        {
                                            var codes = componentMap[dimension.Id];
                                            CollectionAssert.Contains(codes, dimValue);
                                        }

                                        ValidateAgainstCriteria(dimension, dimValue, queriedOrValues, queriedAndValues);
                                    }

                                    ValidateAttribute(dataQuery, reader, componentMap, new[] { DataQueryDetailEnumType.Full, DataQueryDetailEnumType.NoData }, dataQuery.DataStructure.GetGroupAttributes(groupId, false), queriedOrValues, queriedAndValues);
                                }
                            }

                            break;
                    }
                }
            }

            switch (detailLevel)
            {
                case DataQueryDetailEnumType.Null:
                case DataQueryDetailEnumType.DataOnly:
                case DataQueryDetailEnumType.Full:
                    Assert.GreaterOrEqual(obsCountPerSeries, 1);
                    break;
                case DataQueryDetailEnumType.SeriesKeysOnly:
                case DataQueryDetailEnumType.NoData:
                    Assert.AreEqual(obsCountPerSeries, 0);
                    break;
            }
        }

        /// <summary>
        /// Builds the series key.
        /// </summary>
        /// <param name="dsd">The DSD.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>The series key</returns>
        private static string BuildKey(IDataStructureObject dsd, XmlReader reader)
        {
            return string.Join("+", dsd.GetDimensions().OrderBy(dimension => dimension.Position).Select(dimension => reader.GetAttribute(dimension.Id)));
        }

        /// <summary>
        /// Validates the against criteria.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="value">The value.</param>
        /// <param name="queriedOrValues">The queried or values.</param>
        /// <param name="queriedAndValues">The queried and values.</param>
        private static void ValidateAgainstCriteria(IIdentifiableObject component, string value, IDictionary<string, IList<Predicate<string>>> queriedOrValues, IDictionary<string, IList<Predicate<string>>> queriedAndValues)
        {
            IList<Predicate<string>> criteria;
            if (queriedOrValues.TryGetValue(component.Id, out criteria))
            {
                Assert.IsTrue(criteria.Any(predicate => predicate(value)), "Value {0} not in query", value);
            }

            if (queriedAndValues.TryGetValue(component.Id, out criteria))
            {
                Assert.IsTrue(criteria.All(predicate => predicate(value)), "Value {0} not in query", value);
            }
        }

        /// <summary>
        /// Gets the simple queried values.
        /// </summary>
        /// <param name="simple">The simple.</param>
        /// <param name="queriedValues">The queried values.</param>
        private static void GetSimpleQueriedValues(IDataQuery simple, IDictionary<string, IList<Predicate<string>>> queriedValues)
        {
            var dataQuerySelectionGroup = simple.SelectionGroups.FirstOrDefault();
            if (dataQuerySelectionGroup != null)
            {
                foreach (var dataQuerySelection in dataQuerySelectionGroup.Selections)
                {
                    var values = dataQuerySelection.HasMultipleValues ? dataQuerySelection.Values : new HashSet<string> { dataQuerySelection.Value };
                    queriedValues.Add(dataQuerySelection.ComponentId, values.Select<string, Predicate<string>>(s => x => string.Equals(s, x, StringComparison.Ordinal)).ToArray());
                }
            }
        }

        /// <summary>
        /// Gets the complex queried values.
        /// </summary>
        /// <param name="complex">The complex.</param>
        /// <param name="queriedOrValues">The queried or values.</param>
        /// <param name="queriedAndValues">The queried and values.</param>
        private static void GetComplexQueriedValues(IComplexDataQuery complex, IDictionary<string, IList<Predicate<string>>> queriedOrValues, IDictionary<string, IList<Predicate<string>>> queriedAndValues)
        {
            var complexDataQuerySelectionGroup = complex.SelectionGroups.FirstOrDefault();
            if (complexDataQuerySelectionGroup != null)
            {
                foreach (var selection in complexDataQuerySelectionGroup.Selections)
                {
                    var valuesWithOperatorsOr = new List<Predicate<string>>();
                    var valuesWithOperatorsAnd = new List<Predicate<string>>();
                    var values = selection.HasMultipleValues() ? selection.Values : new HashSet<IComplexComponentValue> { selection.Value };
                    var useAnd = values.All(value => value.OrderedOperator != null && value.OrderedOperator == OrderedOperatorEnumType.NotEqual);
                    foreach (var complexComponentValue in values)
                    {
                        var func = GetPredicate(complexComponentValue);
                        if (func != null)
                        {
                            if (useAnd)
                            {
                                valuesWithOperatorsAnd.Add(func);
                            }
                            else
                            {
                                valuesWithOperatorsOr.Add(func);
                            }
                        }
                    }

                    if (valuesWithOperatorsOr.Count > 0)
                    {
                        queriedOrValues.Add(selection.ComponentId, valuesWithOperatorsOr);
                    }

                    if (valuesWithOperatorsAnd.Count > 0)
                    {
                        queriedAndValues.Add(selection.ComponentId, valuesWithOperatorsAnd);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the predicate.
        /// </summary>
        /// <param name="complexComponentValue">The complex component value.</param>
        /// <returns>The validation function for the specified <paramref name="complexComponentValue"/></returns>
        private static Predicate<string> GetPredicate(IComplexComponentValue complexComponentValue)
        {
            Predicate<string> func = null;
            var value = complexComponentValue.Value;
            decimal number;
            bool isNumber = decimal.TryParse(complexComponentValue.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out number);

            if (complexComponentValue.OrderedOperator != null)
            {
                switch (complexComponentValue.OrderedOperator.EnumType)
                {
                    case OrderedOperatorEnumType.GreaterThanOrEqual:
                        if (isNumber)
                        {
                            func = s => HandleNumbers(s, d => d >= number);
                        }
                        else
                        {
                            func = s => string.Compare(s, value, StringComparison.Ordinal) >= 0;
                        }

                        break;
                    case OrderedOperatorEnumType.LessThanOrEqual:
                        if (isNumber)
                        {
                            func = s => HandleNumbers(s, d => d <= number);
                        }
                        else
                        {
                            func = s => string.Compare(s, value, StringComparison.Ordinal) <= 0;
                        }

                        break;
                    case OrderedOperatorEnumType.GreaterThan:
                        if (isNumber)
                        {
                            func = s => HandleNumbers(s, d => d > number);
                        }
                        else
                        {
                            func = s => string.Compare(s, value, StringComparison.Ordinal) > 0;
                        }

                        break;
                    case OrderedOperatorEnumType.LessThan:
                        if (isNumber)
                        {
                            func = s => HandleNumbers(s, d => d < number);
                        }
                        else
                        {
                            func = s => string.Compare(s, value, StringComparison.Ordinal) < 0;
                        }

                        break;
                    case OrderedOperatorEnumType.NotEqual:
                        func = s => !string.Equals(s, value);
                        break;
                    default:
                        func = s => string.Equals(s, value);
                        break;
                }
            }
            else if (complexComponentValue.TextSearchOperator != null)
            {
                switch (complexComponentValue.TextSearchOperator.EnumType)
                {
                    case TextSearchEnumType.Contains:
                        func = s => s.Contains(value);
                        break;
                    case TextSearchEnumType.StartsWith:
                        func = s => s.StartsWith(value);
                        break;
                    case TextSearchEnumType.EndsWith:
                        func = s => !s.EndsWith(value);
                        break;
                    case TextSearchEnumType.DoesNotContain:
                        func = s => !s.Contains(value);
                        break;
                    case TextSearchEnumType.DoesNotStartWith:
                        func = s => !s.StartsWith(value);
                        break;
                    case TextSearchEnumType.DoesNotEndWith:
                        func = s => !s.EndsWith(value);
                        break;
                    case TextSearchEnumType.NotEqual:
                        func = s => !string.Equals(s, value);
                        break;
                    default:
                        func = s => string.Equals(s, value);
                        break;
                }
            }

            return func;
        }

        /// <summary>
        /// Handles the numbers.
        /// </summary>
        /// <param name="s">The value.</param>
        /// <param name="func">The function.</param>
        /// <returns><c>true</c> if <paramref name="s"/> is a number and <paramref name="func"/> returns true</returns>
        private static bool HandleNumbers(string s, Func<decimal, bool> func)
        {
            decimal currentNumber;
            if (decimal.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out currentNumber))
            {
                return func(currentNumber);
            }

            return false;
        }

        /// <summary>
        /// Validates the attribute.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="componentMap">The component map.</param>
        /// <param name="allowedLevels">The allowed levels.</param>
        /// <param name="getObservationAttributes">The get observation attributes.</param>
        /// <param name="queriedOrValues">The queried or values.</param>
        /// <param name="queriedAndValues">The queried and values.</param>
        private static void ValidateAttribute(IBaseDataQuery dataQuery, XmlReader reader, IDictionary<string, ISet<string>> componentMap, ICollection<DataQueryDetailEnumType> allowedLevels, IEnumerable<IAttributeObject> getObservationAttributes, IDictionary<string, IList<Predicate<string>>> queriedOrValues, IDictionary<string, IList<Predicate<string>>> queriedAndValues)
        {
            var detailLevel = dataQuery.DataQueryDetail.EnumType;
            foreach (var attributeObject in getObservationAttributes)
            {
                var value = reader.GetAttribute(attributeObject.Id);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CollectionAssert.Contains(allowedLevels, detailLevel);
                }

                if (allowedLevels.Contains(detailLevel))
                {
                    if ("Mandatory".Equals(attributeObject.AssignmentStatus))
                    {
                        Assert.IsNotNullOrEmpty(value, "Attribute {0}", attributeObject.Id);
                    }
                }

                if (!string.IsNullOrWhiteSpace(value) && attributeObject.HasCodedRepresentation())
                {
                    var validCodes = componentMap[attributeObject.Id];
                    CollectionAssert.Contains(validCodes, value, "Attribute {0} has code {1}, expected values [{2}]", attributeObject.Id, value, string.Join(", ", validCodes));
                }

                if (!string.IsNullOrWhiteSpace(value))
                {
                    ValidateAgainstCriteria(attributeObject, value, queriedOrValues, queriedAndValues);
                }
            }
        }

        #endregion

        /// <summary>
        /// Builds the component code map.
        /// </summary>
        /// <param name="dsd">The DSD.</param>
        /// <param name="retrievalManager">The retrieval manager.</param>
        /// <returns>
        /// The map of component id to code ids
        /// </returns>
        private static IDictionaryOfSets<string, string> BuildComponentCodeMap(IDataStructureObject dsd, ISdmxObjectRetrievalManager retrievalManager)
        {
            var componentCodeMap = new DictionaryOfSets<string, string>(StringComparer.Ordinal);
            foreach (var dimension in dsd.Components)
            {
                if (dimension.HasCodedRepresentation())
                {
                    if (dimension.Representation.Representation.MaintainableStructureEnumType == SdmxStructureEnumType.CodeList)
                    {
                        var maintainableObject = retrievalManager.GetMaintainableObject<ICodelistObject>(dimension.Representation.Representation);
                        AddToComponentMap(componentCodeMap, dimension.Id, maintainableObject.Items);
                    }
                    else
                    {
                        var maintainableObject = retrievalManager.GetMaintainableObject<IConceptSchemeObject>(dimension.Representation.Representation);
                        AddToComponentMap(componentCodeMap, dimension.Id, maintainableObject.Items);
                    }
                }
            }

            return componentCodeMap;
        }

        /// <summary>
        /// Adds <paramref name="items"/> to component map.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="map">The map.</param>
        /// <param name="dimensionId">The dimension id.</param>
        /// <param name="items">The items.</param>
        private static void AddToComponentMap<T>(IDictionaryOfSets<string, string> map, string dimensionId, IEnumerable<T> items) where T : IItemObject
        {
            foreach (var code in items)
            {
                map.AddToSet(dimensionId, code.Id);
            }
        }

        /// <summary>
        /// Gets the primary measure values.
        /// </summary>
        /// <param name="complexComponent">The complex component.</param>
        /// <returns>
        /// The method to validate
        /// </returns>
        private static Predicate<string> GetPrimaryMeasureValues(IEnumerable<IComplexComponentValue> complexComponent)
        {
            var tests = new List<Predicate<string>>();
            foreach (var complexComponentValue in complexComponent)
            {
                var value = complexComponentValue.Value;
                if (complexComponentValue.OrderedOperator != null && complexComponentValue.OrderedOperator.EnumType != OrderedOperatorEnumType.Null)
                {
                    var number = decimal.Parse(value);
                    switch (complexComponentValue.OrderedOperator.EnumType)
                    {
                        case OrderedOperatorEnumType.Null:
                            break;
                        case OrderedOperatorEnumType.GreaterThanOrEqual:
                            tests.Add(s => decimal.Parse(s) >= number);
                            break;
                        case OrderedOperatorEnumType.LessThanOrEqual:
                            tests.Add(s => decimal.Parse(s) <= number);
                            break;
                        case OrderedOperatorEnumType.GreaterThan:
                            tests.Add(s => decimal.Parse(s) > number);
                            break;
                        case OrderedOperatorEnumType.LessThan:
                            tests.Add(s => decimal.Parse(s) > number);
                            break;
                        case OrderedOperatorEnumType.Equal:
                            tests.Add(s => decimal.Parse(s) == number);
                            break;
                        case OrderedOperatorEnumType.NotEqual:
                            tests.Add(s => decimal.Parse(s) != number);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                } 
                else if (complexComponentValue.TextSearchOperator != null)
                {
                    switch (complexComponentValue.TextSearchOperator.EnumType)
                    {
                        case TextSearchEnumType.Contains:
                            tests.Add(s => s.Contains(value));
                            break;
                        case TextSearchEnumType.StartsWith:
                            tests.Add(s => s.StartsWith(value));
                            break;
                        case TextSearchEnumType.EndsWith:
                            tests.Add(s => s.EndsWith(value));
                            break;
                        case TextSearchEnumType.DoesNotContain:
                            tests.Add(s => !s.Contains(value));
                            break;
                        case TextSearchEnumType.DoesNotStartWith:
                            tests.Add(s => !s.StartsWith(value));
                            break;
                        case TextSearchEnumType.DoesNotEndWith:
                            tests.Add(s => !s.EndsWith(value));
                            break;
                        case TextSearchEnumType.Equal:
                            tests.Add(s => s.Equals(value));
                            break;
                        case TextSearchEnumType.NotEqual:
                            tests.Add(s => !s.Equals(value));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return s => tests.TrueForAll(predicate => predicate(s));
        }
    }
}
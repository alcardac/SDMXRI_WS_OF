// -----------------------------------------------------------------------
// <copyright file="TestComplexDataQueryToXDoc.cs" company="EUROSTAT">
//   Date Created : 2014-10-31
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Estat.Sri.CustomRequests.Factory;
    using Estat.Sri.CustomRequests.Manager;
    using Estat.Sri.CustomRequests.Model;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// Test unit for creating XML from ComplexDataQueries
    /// </summary>
    [TestFixture]
    public class TestComplexDataQueryToXDoc
    {
        /// <summary>
        /// The _data query builder manager
        /// </summary>
        private readonly IComplexDataQueryBuilderManager _dataQueryBuilderManager;

        /// <summary>
        /// The _dataflow object
        /// </summary>
        private readonly IDataflowObject _dataflowObject;

        /// <summary>
        /// The _data structure object
        /// </summary>
        private readonly IDataStructureObject _dataStructureObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestComplexDataQueryToXDoc"/> class.
        /// </summary>
        public TestComplexDataQueryToXDoc()
        {
            IStructureParsingManager manager = new StructureParsingManager();
            this._dataQueryBuilderManager = new ComplexDataQueryBuilderManager(new ComplexDataQueryFactoryV21());
            using (var readable = new FileReadableDataLocation("tests/V21/Structure/test-sdmxv2.1-ESTAT+SSTSCONS_PROD_M+2.0.xml"))
            {
                var structureWorkspace = manager.ParseStructures(readable);
                this._dataflowObject = structureWorkspace.GetStructureObjects(false).Dataflows.First();
            }

            using (var readable = new FileReadableDataLocation("tests/V21/Structure/test-sdmxv2.1-ESTAT+STS+2.0.xml"))
            {
                var structureWorkspace = manager.ParseStructures(readable);
                this._dataStructureObject = structureWorkspace.GetStructureObjects(false).DataStructures.First();
            }
        }

        /// <summary>
        /// Test unit for creating XML from ComplexDataQueries
        /// </summary>
        [Test]
        public void TestNotEqual()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue[] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension) });
            var complexDataQueryImpl = this.BuildComplexDataQueryImpl(freqCriteria);

            var document = this._dataQueryBuilderManager.BuildComplexDataQuery(complexDataQueryImpl, new StructSpecificDataFormatV21());
            Assert.NotNull(document);
            using (var reader = document.CreateReader())
            {
                XmlNameTable nameTable = reader.NameTable;
                Assert.NotNull(nameTable);
                var namespaceManager = new XmlNamespaceManager(nameTable);
                namespaceManager.AddNamespace("q", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query");
                var atts = (IEnumerable)document.XPathEvaluate("//q:DataWhere/q:DimensionValue/q:Value/@operator", namespaceManager);
                var attList = atts.Cast<XAttribute>().ToArray();
                Assert.AreEqual(freqCriteria.Values.Count, attList.Length);
                var all = attList.All(attribute => attribute.Value.Equals("notEqual"));
                Assert.IsTrue(all);
            }

            var outInfo = new FileInfo("testNotEqual.xml");
            ValidateDocument(outInfo, document);
        }

        /// <summary>
        /// Test unit for creating XML from ComplexDataQueries
        /// </summary>
        [Test]
        public void TestEqual()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue[] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension) });
            var complexDataQueryImpl = this.BuildComplexDataQueryImpl(freqCriteria);

            var document = this._dataQueryBuilderManager.BuildComplexDataQuery(complexDataQueryImpl, new StructSpecificDataFormatV21());
            Assert.NotNull(document);
            using (var reader = document.CreateReader())
            {
                XmlNameTable nameTable = reader.NameTable;
                Assert.NotNull(nameTable);
                var namespaceManager = new XmlNamespaceManager(nameTable);
                namespaceManager.AddNamespace("q", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query");
                var atts = (IEnumerable)document.XPathEvaluate("//q:DataWhere/q:Or/q:DimensionValue/q:Value/@operator", namespaceManager);
                var attList = atts.Cast<XAttribute>().ToArray();
                Assert.AreEqual(freqCriteria.Values.Count, attList.Length);
                var all = attList.All(attribute => attribute.Value.Equals("equal"));
                Assert.IsTrue(all);
            }

            var outInfo = new FileInfo("testEqual.xml");
            ValidateDocument(outInfo, document);
        }

        /// <summary>
        /// Test unit for creating XML from ComplexDataQueries
        /// </summary>
        [Test]
        public void TestMixedEqual()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue[] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension) });
            var complexDataQueryImpl = this.BuildComplexDataQueryImpl(freqCriteria);

            var document = this._dataQueryBuilderManager.BuildComplexDataQuery(complexDataQueryImpl, new StructSpecificDataFormatV21());
            Assert.NotNull(document);
            using (var reader = document.CreateReader())
            {
                XmlNameTable nameTable = reader.NameTable;
                Assert.NotNull(nameTable);
                var namespaceManager = new XmlNamespaceManager(nameTable);
                namespaceManager.AddNamespace("q", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query");
                var atts = (IEnumerable)document.XPathEvaluate("//q:DataWhere/q:Or/q:DimensionValue/q:Value/@operator", namespaceManager);
                var attList = atts.Cast<XAttribute>().ToArray();
                Assert.AreEqual(freqCriteria.Values.Count, attList.Length, document.ToString());
                var all = attList.All(attribute => attribute.Value.Equals("equal") || attribute.Value.Equals("notEqual"));
                Assert.IsTrue(all);
            }

            var outInfo = new FileInfo("testMixedEqual.xml");
            ValidateDocument(outInfo, document);
        }

        /// <summary>
        /// Validates the document.
        /// </summary>
        /// <param name="outInfo">The out information.</param>
        /// <param name="document">The document.</param>
        private static void ValidateDocument(FileInfo outInfo, XDocument document)
        {
            using (var stream = outInfo.Create())
            {
                document.Save(stream);
                stream.Flush();
            }

            using (var readableLocation = new FileReadableDataLocation(outInfo))
            {
                XMLParser.ValidateXml(readableLocation, SdmxSchemaEnumType.VersionTwoPointOne);
            }
        }

        /// <summary>
        /// Builds the complex data query implementation.
        /// </summary>
        /// <param name="freqCriteria">The frequency criteria.</param>
        /// <returns>
        /// The <see cref="ComplexDataQueryImpl"/>
        /// </returns>
        private ComplexDataQueryImpl BuildComplexDataQueryImpl(IComplexDataQuerySelection freqCriteria)
        {
            ISet<IComplexDataQuerySelection> sections = new HashSet<IComplexDataQuerySelection>();
            sections.Add(freqCriteria);

            ICollection<IComplexDataQuerySelectionGroup> collection = new[]
                                                                          {
                                                                              new ComplexDataQuerySelectionGroupImpl(
                                                                                  sections,
                                                                                  null,
                                                                                  OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal),
                                                                                  null,
                                                                                  OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal),
                                                                                  null)
                                                                          };

            var complexDataQueryImpl = new ComplexDataQueryImpl(
                null,
                null,
                null,
                this._dataStructureObject,
                this._dataflowObject,
                null,
                null,
                0,
                null,
                false,
                null,
                DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.Time).Value,
                false,
                DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full),
                collection);
            return complexDataQueryImpl;
        }
    }
}
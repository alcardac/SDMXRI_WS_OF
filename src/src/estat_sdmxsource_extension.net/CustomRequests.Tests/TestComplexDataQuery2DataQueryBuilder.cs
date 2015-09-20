// -----------------------------------------------------------------------
// <copyright file="TestComplexDataQuery2DataQueryBuilder.cs" company="Eurostat">
//   Date Created : 2014-01-24
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Builder;
    using Estat.Sri.CustomRequests.Factory;
    using Estat.Sri.CustomRequests.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="ComplexDataQuery2DataQueryBuilder"/>
    /// </summary>
    [TestFixture]
    public class TestComplexDataQuery2DataQueryBuilder
    {

        /// <summary>
        /// The _builder
        /// </summary>
        private readonly ComplexDataQuery2DataQueryBuilder _builder = new ComplexDataQuery2DataQueryBuilder();

        /// <summary>
        /// Test unit for <see cref="ComplexDataQuery2DataQueryBuilder.Build"/> 
        /// </summary>
        [Test]
        public void TestBuild()
        {
            IStructureParsingManager manager = new StructureParsingManager();
            IDataQueryParseManager queryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwoPointOne);
            IComplexDataQueryBuilderManager dataQueryBuilderManager = new ComplexDataQueryBuilderManager(new ComplexDataQueryFactoryV21());
            
            IDataflowObject dataFlow;
            IDataStructureObject dsd;
            using (var readable = new FileReadableDataLocation("tests/V21/Structure/test-sdmxv2.1-ESTAT+SSTSCONS_PROD_M+2.0.xml"))
            {
                var structureWorkspace = manager.ParseStructures(readable);
                dataFlow = structureWorkspace.GetStructureObjects(false).Dataflows.First();
            }

            using (var readable = new FileReadableDataLocation("tests/V21/Structure/test-sdmxv2.1-ESTAT+STS+2.0.xml"))
            {
                var structureWorkspace = manager.ParseStructures(readable);
                dsd = structureWorkspace.GetStructureObjects(false).DataStructures.First();
            }

            ISet<IComplexDataQuerySelection> sections = new HashSet<IComplexDataQuerySelection>();
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue[] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension) });
            sections.Add(freqCriteria);
            var adjustmentCriteria = new ComplexDataQuerySelectionImpl("ADJUSTMENT", new IComplexComponentValue[] { new ComplexComponentValueImpl("01", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension) });
            sections.Add(adjustmentCriteria);
            var titleCriteria = new ComplexDataQuerySelectionImpl(
                "TITLE", 
                new IComplexComponentValue[] { new ComplexComponentValueImpl("PAOKARA", TextSearch.GetFromEnum(TextSearchEnumType.Contains), SdmxStructureEnumType.DataAttribute),  new ComplexComponentValueImpl("ARIS", TextSearch.GetFromEnum(TextSearchEnumType.DoesNotContain), SdmxStructureEnumType.DataAttribute), });
            sections.Add(titleCriteria);
            OrderedOperator equalOperator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal);
            var dateFrom = new SdmxDateCore("2000-01");
            var dateFromOperator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.GreaterThanOrEqual);
            var primaryMeasureValue = new ComplexComponentValueImpl("200.20", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.GreaterThan), SdmxStructureEnumType.PrimaryMeasure);
            ICollection<IComplexDataQuerySelectionGroup> collection = new[] { new ComplexDataQuerySelectionGroupImpl(sections, dateFrom, dateFromOperator, null, equalOperator, new HashSet<IComplexComponentValue> { primaryMeasureValue }) };

            var complexDataQueryImpl = new ComplexDataQueryImpl(
                null, 
                null, 
                null, 
                dsd, 
                dataFlow, 
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

            var dataQuery = this._builder.Build(complexDataQueryImpl);
            Assert.NotNull(dataQuery);
        }
    }
}